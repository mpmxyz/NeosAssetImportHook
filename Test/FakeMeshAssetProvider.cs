using BaseX;
using FrooxEngine;
using System;
using System.Collections.Generic;

namespace NeosAssetImportHook
{
    public partial class HookNotificationTest
    {
        private class FakeMeshAssetProvider : IAssetProvider<Mesh>
        {
            public Mesh Asset => throw new NotImplementedException();

            public int AssetReferenceCount => throw new NotImplementedException();

            public IEnumerable<IAssetRef> References => throw new NotImplementedException();

            public IAsset GenericAsset => throw new NotImplementedException();

            public bool IsAssetAvailable => throw new NotImplementedException();

            public Slot Slot => throw new NotImplementedException();

            public bool IsUnderLocalUser => throw new NotImplementedException();

            public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Type WorkerType => throw new NotImplementedException();

            public string WorkerTypeName => throw new NotImplementedException();

            public bool IsStarted => throw new NotImplementedException();

            public bool IsDestroyed => throw new NotImplementedException();

            public bool IsChangeDirty => throw new NotImplementedException();

            public int LastChangeUpdateIndex => throw new NotImplementedException();

            public int UpdateOrder => throw new NotImplementedException();

            public bool IsInInitPhase => throw new NotImplementedException();

            public bool IsLinked => throw new NotImplementedException();

            public bool IsDriven => throw new NotImplementedException();

            public bool IsHooked => throw new NotImplementedException();

            public ILinkRef ActiveLink => throw new NotImplementedException();

            public ILinkRef DirectLink => throw new NotImplementedException();

            public ILinkRef InheritedLink => throw new NotImplementedException();

            public IEnumerable<ILinkable> LinkableChildren => throw new NotImplementedException();

            public RefID ReferenceID => throw new NotImplementedException();

            public string Name => throw new NotImplementedException();

            public World World => throw new NotImplementedException();

            public IWorldElement Parent => throw new NotImplementedException();

            public bool IsLocalElement => throw new NotImplementedException();

            public bool IsPersistent => throw new NotImplementedException();

            public bool IsRemoved => throw new NotImplementedException();

            public event Action<IChangeable> Changed;

            public bool AssignKey(string key, int version = 0, bool onlyFree = false)
            {
                throw new NotImplementedException();
            }

            public void ChildChanged(IWorldElement child)
            {
                Changed(null);
                throw new NotImplementedException();
            }

            public void Destroy()
            {
                throw new NotImplementedException();
            }

            public void EndInitPhase()
            {
                throw new NotImplementedException();
            }

            public void GetReferencedObjects(List<IWorldElement> referencedObjects, bool assetRefOnly, bool persistentOnly = true, bool skipDontCopy = false)
            {
                throw new NotImplementedException();
            }

            public string GetSyncMemberName(ISyncMember member)
            {
                throw new NotImplementedException();
            }

            public bool HasKey(string key)
            {
                throw new NotImplementedException();
            }

            public void InheritLink(ILinkRef link)
            {
                throw new NotImplementedException();
            }

            public void InternalRunApplyChanges(int changeUpdateIndex)
            {
                throw new NotImplementedException();
            }

            public void InternalRunAudioConfigurationChanged()
            {
                throw new NotImplementedException();
            }

            public void InternalRunAudioUpdate()
            {
                throw new NotImplementedException();
            }

            public void InternalRunDestruction()
            {
                throw new NotImplementedException();
            }

            public void InternalRunStartup()
            {
                throw new NotImplementedException();
            }

            public void InternalRunUpdate()
            {
                throw new NotImplementedException();
            }

            public void Link(ILinkRef link)
            {
                throw new NotImplementedException();
            }

            public void Load(DataTreeNode node, LoadControl control)
            {
                throw new NotImplementedException();
            }

            public void ReferenceFreed(IAssetRef reference)
            {
                throw new NotImplementedException();
            }

            public void ReferenceSet(IAssetRef reference)
            {
                throw new NotImplementedException();
            }

            public void RegisterUpdateListener(IAssetRef reference)
            {
                throw new NotImplementedException();
            }

            public void ReleaseInheritedLink(ILinkRef link)
            {
                throw new NotImplementedException();
            }

            public void ReleaseLink(ILinkRef link)
            {
                throw new NotImplementedException();
            }

            public void RemoveKey(string key)
            {
                throw new NotImplementedException();
            }

            public Coroutine RunInSeconds(float seconds, Action action)
            {
                throw new NotImplementedException();
            }

            public Coroutine RunInUpdates(int updates, Action action)
            {
                throw new NotImplementedException();
            }

            public void RunSynchronously(Action action, bool immediatellyIfPossible = false)
            {
                throw new NotImplementedException();
            }

            public DataTreeNode Save(SaveControl control)
            {
                throw new NotImplementedException();
            }

            public void SendAssetCreated()
            {
                throw new NotImplementedException();
            }

            public void SendAssetRemoved()
            {
                throw new NotImplementedException();
            }

            public void SendAssetUpdated()
            {
                throw new NotImplementedException();
            }

            public IField TryGetField(string name)
            {
                throw new NotImplementedException();
            }

            public IField<T> TryGetField<T>(string name)
            {
                throw new NotImplementedException();
            }

            public void UnregisterUpdateListener(IAssetRef reference)
            {
                throw new NotImplementedException();
            }
        }
    }
}