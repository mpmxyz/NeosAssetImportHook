using FrooxEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace NeosAssetImportHook
{
    public partial class HookNotificationTest
    {
        ITestOutputHelper output;
        public HookNotificationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void UntypedHooksAreNotified()
        {
            var called = false;
            output.WriteLine("Before adding delegate");
            UntypedPostImportHandler handler = (slot, type, assets) =>
            {
                output.WriteLine("Within delegate call");
                var elems = new List<IAssetProvider>(assets);
                Assert.Null(slot);
                Assert.Equal(typeof(Mesh), type);

                Assert.Equal(3, elems.Count);
                Assert.IsType<FakeMeshAssetProvider>(elems[0]);
                Assert.IsType<FakeMeshAssetProvider>(elems[1]);
                Assert.IsType<FakeTextureAssetProvider>(elems[2]);
                called = true;
            };

            try
            {
                AssetImportHooks.PostImport += handler;
                output.WriteLine("Before getting generic method");
                var notify = typeof(AssetImportHooks)
                    .GetMethod("NotifyPostImport", BindingFlags.NonPublic | BindingFlags.Static)
                    .MakeGenericMethod(typeof(Mesh));
                output.WriteLine("Before invoking generic method");

                notify.Invoke(
                    null,
                    new object[]
                    {
                    null,
                    new List<IAssetProvider>()
                    {
                        new FakeMeshAssetProvider(),
                        new FakeMeshAssetProvider(),
                        new FakeTextureAssetProvider(),
                    }
                    }
                );
                output.WriteLine("After invoking generic method");

                Assert.True(called);
            }
            finally
            {
                AssetImportHooks.PostImport -= handler;
            }
        }

        [Fact]
        public void MatchingTypedHooksAreNotified()
        {
            var called = false;

            UntypedPostImportHandler handler = AssetImportHooks.Typed<Mesh>((slot, primary, secondary) =>
            {
                var prim = new List<IAssetProvider>(primary);
                var scnd = new List<IAssetProvider>(secondary);

                Assert.Null(slot);

                Assert.Equal(2, prim.Count);
                Assert.IsType<FakeMeshAssetProvider>(prim[0]);
                Assert.IsType<FakeMeshAssetProvider>(prim[1]);

                Assert.Single(scnd);
                Assert.IsType<FakeTextureAssetProvider>(scnd[0]);
                called = true;
            });

            try
            {
                AssetImportHooks.PostImport += handler;
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
                        new FakeMeshAssetProvider(),
                        new FakeMeshAssetProvider(),
                        new FakeTextureAssetProvider(),
                    }
                    }
                );

                Assert.True(called);
            }
            finally
            {
                AssetImportHooks.PostImport -= handler;
            }
        }

        [Fact]
        public void UnmatchingTypedHooksAreIgnored()
        {
            var called = false;

            UntypedPostImportHandler handler = AssetImportHooks.Typed<Mesh>((slot, primary, secondary) =>
            {
                called = true;
            });

            try
            {
                AssetImportHooks.PostImport += handler;
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
                        new FakeMeshAssetProvider(),
                        new FakeMeshAssetProvider(),
                        new FakeTextureAssetProvider(),
                    }
                    }
                );

                Assert.False(called);
            }
            finally
            {
                AssetImportHooks.PostImport -= handler;
            }
        }
    }
}