using FrooxEngine;
using System;
using System.Collections.Generic;

namespace NeosAssetImportHook
{
    /// <summary>
    /// Triggered on all asset imports
    /// </summary>
    /// <param name="slot">Main slot of the imported object. (special case Texture3D: the slot containing the volumetric texture)</param>
    /// <param name="mainAssetType">Type of the asset (a subtype of IAsset)</param>
    /// <param name="allAssets">All assets related to this import</param>
    public delegate void UntypedPostImportHandler(Slot slot, Type mainAssetType, IEnumerable<IAssetProvider> allAssets);

    /// <summary>
    /// Only triggered on imports of type <typeparamref name="A"/>
    /// </summary>
    /// <typeparam name="A">Primary asset type</typeparam>
    /// <param name="slot">Main slot of the imported object. (special case Texture3D: the slot containing the volumetric texture)</param>
    /// <param name="mainAssets">All assets of type A</param>
    /// <param name="otherAssets">All other assets</param>
    public delegate void TypedPostImportHandler<A>(Slot slot, IEnumerable<IAssetProvider<A>> mainAssets, IEnumerable<IAssetProvider> otherAssets) where A : class, IAsset;

    public static class AssetImportHooks
    {
        /// <summary>
        /// Contains all active post-import handlers, add your own to react to Neos asset imports
        /// </summary>
        public static event UntypedPostImportHandler PostImport = null;

        /// <summary>
        /// creates an <see cref="UntypedPostImportHandler"/> from a typed one: <br/>
        /// <code>AssetImportHooks.PostImport += Typed&lt;A&gt;((a, b, c) => { ... })</code>
        /// </summary>
        /// <typeparam name="A">Asset type of handler</typeparam>
        /// <param name="handler">A typed handler</param>
        /// <returns>An untyped handler only executing the given one if the type matches.</returns>
        public static UntypedPostImportHandler Typed<A>(TypedPostImportHandler<A> handler) where A : class, IAsset
        {
            return (slot, mainAssetType, allAssets) =>
            {
                if (typeof(A).Equals(mainAssetType))
                {
                    List<IAssetProvider<A>> mainAssets = new List<IAssetProvider<A>>();
                    List<IAssetProvider> otherAssets = new List<IAssetProvider>();
                    foreach (IAssetProvider asset in allAssets)
                    {
                        if (asset is IAssetProvider<A> a)
                        {
                            mainAssets.Add(a);
                        }
                        else
                        {
                            otherAssets.Add(asset);
                        }
                    }
                    handler(slot, mainAssets, otherAssets);
                }
            };
        }

        /// <summary>
        /// called internally to trigger asset import handlers
        /// </summary>
        /// <typeparam name="A">Asset type being imported</typeparam>
        /// <param name="slot">Main slot of the imported asset</param>
        /// <param name="assetProviders">All assets associated with the import (even secondary/tertiary)</param>
        internal static void NotifyPostImport<A>(Slot slot, List<IAssetProvider> assetProviders) where A : class, IAsset
        {
            PostImport?.Invoke(slot, typeof(A), assetProviders);
        }
    }
}
