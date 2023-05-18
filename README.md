# NeosAssetImportHook

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that allows other mods to hook into Neos' asset import.

## Installation
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
2. Download [NeosAssetImportHook.dll](https://github.com/mpmxyz/NeosAssetImportHook/releases/latest/download/NeosAssetImportHook.dll)
3. Copy it into the `nml_mods` directory inside your Neos install.
4. Start the game. If you want to verify that the mod is working you can check your Neos logs.

## Library Usage

### Local Development
Install the mod into Neos and reference it there.

### Github workflow
Make sure that you put [NeosAssetImportHook.dll](https://github.com/mpmxyz/NeosAssetImportHook/releases/latest/download/NeosAssetImportHook.dll) into the nml_mods directory before building your own mod.

### API

```C#
AssetImportHooks.PostImport += (Slot slot, Type assetType, List<IAssetProvider> assets) =
{
	//will be executed on every import
};
AssetImportHooks.PostImport += AssetImportHooks.Typed<Mesh>(Slot slot, List<IAssetProvider> mainAssets, List<IAssetProvider> secondaryAssets) =
{
	//will only be executed for 3D model imports
	//mainAssets contains all AssetProvider<Mesh> instances, secondaryAssets everything else
};
```

See [AssetImportHooks.cs](https://github.com/mpmxyz/NeosAssetImportHook/blob/main/NeosAssetImportHook/AssetImportHooks.cs) more info.