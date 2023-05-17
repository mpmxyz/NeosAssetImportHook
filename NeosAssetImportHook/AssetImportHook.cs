using BaseX;
using FrooxEngine;
using System.Collections.Generic;

namespace NeosAssetImportHook
{
    public delegate void PostImportEvent(Slot slot, List<IAssetProvider> assetProviders);

    public static class AssetImportHooks
    {
        public static event PostImportEvent PostImport = null;

        internal static void NotifyPostImport(Slot slot, List<IAssetProvider> assetProviders)
        {
            UniLog.Log($"Imported {assetProviders.Count} assets into: {slot}");
            assetProviders.ForEach(assetProvider =>
            {
                UniLog.Log(assetProvider.GetType().Name);
            });
            PostImport?.Invoke(slot, assetProviders);
        }
    }
}
