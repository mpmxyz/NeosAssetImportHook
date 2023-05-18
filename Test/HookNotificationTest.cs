using FrooxEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace NeosAssetImportHook
{
    public class HookNotificationTest
    {
        [Fact]
        public void UntypedHooksAreNotified()
        {
            var called = false;
            Console.WriteLine("Before adding delegate");
            AssetImportHooks.PostImport += (slot, type, assets) =>
            {
                Console.WriteLine("Within delegate call");
                var elems = new List<IAssetProvider>(assets);
                Assert.Null(slot);
                Assert.Equal(typeof(Mesh), type);

                Assert.Equal(3, elems.Count);
                Assert.IsType<FakeMeshAsset>(elems[0]);
                Assert.IsType<FakeMeshAsset>(elems[1]);
                Assert.IsType<FakeTextureAsset>(elems[2]);
                called = true;
            };
            Console.WriteLine("Before getting generic method");
            var notify = typeof(AssetImportHooks)
                .GetMethod("NotifyPostImport", BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(Mesh));
            Console.WriteLine("Before invoking generic method");

            notify.Invoke(
                null,
                new object[]
                {
                    null,
                    new List<IAssetProvider>()
                    {
                        new FakeMeshAsset(),
                        new FakeMeshAsset(),
                        new FakeTextureAsset(),
                    }
                }
            );
            Console.WriteLine("After invoking generic method");

            Assert.True(called);
        }

        [Fact]
        public void MatchingTypedHooksAreNotified()
        {
            var called = false;
            AssetImportHooks.PostImport += AssetImportHooks.Typed<Mesh>((slot, primary, secondary) =>
            {
                var prim = new List<IAssetProvider>(primary);
                var scnd = new List<IAssetProvider>(secondary);

                Assert.Null(slot);

                Assert.Equal(2, prim.Count);
                Assert.IsType<FakeMeshAsset>(prim[0]);
                Assert.IsType<FakeMeshAsset>(prim[1]);

                Assert.Single(scnd);
                Assert.IsType<FakeTextureAsset>(scnd[0]);
                called = true;
            });
            var notify = typeof(AssetImportHooks)
                .GetMethod("NotifyPostImport", BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(Mesh));

            notify.Invoke(
                null,
                new object[]
                {
                    null,
                    new List<IAssetProvider>()
                    {
                        new FakeMeshAsset(),
                        new FakeMeshAsset(),
                        new FakeTextureAsset(),
                    }
                }
            );

            Assert.True(called);
        }

        [Fact]
        public void UnmatchingTypedHooksAreIgnored()
        {
            var called = false;
            AssetImportHooks.PostImport += AssetImportHooks.Typed<Mesh>((slot, primary, secondary) =>
            {
                called = true;
            });
            var notify = typeof(AssetImportHooks)
                .GetMethod("NotifyPostImport", BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(typeof(Texture2D));

            notify.Invoke(
                null,
                new object[]
                {
                    null,
                    new List<IAssetProvider>()
                    {
                        new FakeMeshAsset(),
                        new FakeMeshAsset(),
                        new FakeTextureAsset(),
                    }
                }
            );

            Assert.False(called);
        }

        private class FakeMeshAsset : AssetProvider<Mesh>
        {
            public override Mesh Asset => throw new System.NotImplementedException();

            public override bool IsAssetAvailable => throw new System.NotImplementedException();

            protected override void FreeAsset()
            {
                throw new System.NotImplementedException();
            }

            protected override void UpdateAsset()
            {
                throw new System.NotImplementedException();
            }
        }

        private class FakeTextureAsset : AssetProvider<Texture2D>
        {
            public override Texture2D Asset => throw new System.NotImplementedException();

            public override bool IsAssetAvailable => throw new System.NotImplementedException();

            protected override void FreeAsset()
            {
                throw new System.NotImplementedException();
            }

            protected override void UpdateAsset()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}