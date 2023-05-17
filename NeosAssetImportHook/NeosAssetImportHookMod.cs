using BaseX;
using CodeX;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NeosAssetImportHook
{
    public class NeosAssetImportHookMod : NeosMod
    {
        public override string Name => "NeosAssetImportHook";
        public override string Author => "mpmxyz";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/mpmxyz/NeosAssetImportHook/";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("com.github.mpmxyz.assetimporthook");
            harmony.PatchAll();
        }

        /*
         * AssetClass.Unknown/AssetClass.Document && File.Exists
         * DONE: UniversalImporter.ImportRawFile(slot1, file)
         * 
         * AssetClass.Texture
         * -> imageImportDialog.RunImport()
         * DONE:     ImageImporter.ImportLUT -> UniversalImporter.SpawnVolume
         * DONE:     ImageImporter.ImportImage
         * 
         * AssetClass.Cubemap
         * DONE: CubemapImporter.CreateSphere(slot4, (IAssetProvider<Cubemap>) cubemap)
         *  
         * AssetClass.Volume
         * DONE: UniversalImporter.SpawnVolume(Slot root, Uri tex3Durl, float3 scale, bool grayscale = false)
         *  
         * AssetClass.Model/AssetClass.PointCloud
         * DONE: -> modelImportDialog.RunImport() -> ModelImporter.ImportModelAsync -> ModelImporter.ImportModel
         * 
         * AssetClass.Audio
         * DONE: only partial coverage: UniversalImporter.DetectMultimedia(player)
         *  
         * AssetClass.Video
         * DONE: -> videoImportDialog.RunImport() -> VideoImportDialog.ImportAsync
         *  
         * AssetClass.Shader
         *  noop
         *  
         * AssetClass.Animation
         *  no coverage
         *  
         * AssetClass.Font
         * DONE: UniversalImporter.SpawnFont(slot9, (IAssetProvider<Font>) font)
         *  
         * AssetClass.Folder
         *  -> FolderImportDialog -> (delegate) FolderImport.ImportMethod -> methods marked with FolderImporterAttribute
         *      notable methods:
         * DONE:         ImageSlicesFolderImporter.ImportImageSlicesAsVolume -> UniversalImporter.SpawnVolume
         *         
         *  
         * AssetClass.Subtitle
         *  no coverage
         *  
         * AssetClass.Special (logixstring)
         *  no coverage
         */
        [HarmonyPatch]
        static class Patches
        {
            [HarmonyPatch(typeof(UniversalImporter), "ImportRawFile")]
            [HarmonyPostfix]
            public static void PostfixRawFile(ref Task __result, Slot root, string file)
            {
                __result = AwaitAndNotify(__result, root);
            }

            [HarmonyPatch(typeof(ImageImporter), "ImportImage")]
            [HarmonyPostfix]
            public static void PostfixImage(ref Task __result, string path, Slot targetSlot, bool addCollider = true, ImageProjection projection = ImageProjection.Perspective, StereoLayout stereoLayout = StereoLayout.None, float3? forward = null, TextureConversion convert = TextureConversion.Auto, bool setupScreenshotMetadata = false)
            {
                __result = AwaitAndNotify(__result, targetSlot);
            }

            [HarmonyPatch(typeof(UniversalImporter), "SpawnVolume", typeof(Slot), typeof(Uri), typeof(float3), typeof(bool))] //also: ImportLUT
            [HarmonyPostfix]
            public static void PostfixVolume(Slot root, Uri tex3Durl, float3 scale, bool grayscale)
            {
                var container = root.FindChild((child) => child.GetComponent<IAssetProvider<Texture3D>>() != null);
                NotifyAllAssetProviders(container);
            }


            [HarmonyPatch(typeof(CubemapImporter), "CreateSphere")]
            [HarmonyPostfix]
            public static void PostfixCubemap(Slot slot, IAssetProvider<Cubemap> cubemap, bool grabbable = true)
            {
                NotifyAllAssetProviders(slot);
            }

            [HarmonyPatch(typeof(ModelImporter), "ImportModelAsync")]
            [HarmonyPostfix]
            public static void PostfixModel(ref Task __result, string file, Slot targetSlot, ModelImportSettings settings, Slot assetsSlot = null, IProgressIndicator progressIndicator = null)
            {
                __result = AwaitAndNotify(__result, targetSlot, assetsSlot, file);
            }

            [HarmonyPatch(typeof(UniversalImporter), "SpawnFont")]
            [HarmonyPostfix]
            public static void PostfixFont(Slot root, IAssetProvider<Font> font)
            {
                NotifyAllAssetProviders(root);
            }

            [HarmonyPatch(typeof(UniversalImporter), "DetectMultimedia")]
            [HarmonyPostfix]
            public static void PostfixAudio(ref Task __result, AudioPlayerOrb playerOrb)
            {
                //TODO: convert to Transpiler patch starting after AttachComponent<StaticAudioClip>()
                __result = AwaitAndNotify(__result, playerOrb.Slot);
            }

            [HarmonyPatch(typeof(VideoImportDialog), "ImportAsync")]
            [HarmonyPostfix]
            public static void PostfixVideo(ref Task __result, Slot slot)
            {
                __result = AwaitAndNotify(__result, slot);
            }
        }

        private static async Task AwaitAndNotify(Task original, Slot slot, Slot assetsSlot, string file)
        {
            await original;
            if (assetsSlot == null)
            {
                var expectedName = Path.GetFileNameWithoutExtension(file); //see ModelImporter.ImportModel
                slot.World.AssetsSlot.ForeachChild((child) =>
                {
                    if (child.Name == expectedName)
                    {
                        //last child wins, may be incorrect when importing multiple models with equal name
                        assetsSlot = child;
                    }
                });
            }
            NotifyAllAssetProviders(slot, assetsSlot);
        }

        private static async Task AwaitAndNotify(Task original, Slot slot)
        {
            await original;
            NotifyAllAssetProviders(slot, null);
        }

        private static void NotifyAllAssetProviders(Slot root, Slot assetsSlot = null)
        {
            var assetProviders = new List<IAssetProvider>();

            if (assetsSlot != root)
            {
                assetProviders.AddRange(root.GetComponents<IAssetProvider>());
            }

            if (assetsSlot != null)
            {
                assetProviders.AddRange(assetsSlot.GetComponentsInChildren<IAssetProvider>());
            }

            if (assetProviders.Any())
            {
                AssetImportHooks.NotifyPostImport(root, assetProviders);
            }
            else
            {
                UniLog.Error($"No assets found on {root}");
            }
        }
    }
}