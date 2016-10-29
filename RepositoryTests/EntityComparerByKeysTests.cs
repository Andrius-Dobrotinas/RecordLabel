using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecordLabel;
using RecordLabel.Data.ok;
using RecordLabel.Data.Models;
using RecordLabel.Data.Context;
using System.Data.Entity;
using AndrewD.EntityPlus;

namespace RepositoryTests
{
    [TestClass]
    public class EntityComparerByKeysTests
    {
        protected ReleaseContext Context;
        protected DbContextReflector reflector;
        protected EntityComparerByKeys comparer;

        [TestInitialize]
        public void Initialize()
        {
            Context = new ReleaseContext(GlobalValues.UnitTestReinitializableConnectionString);

            reflector = new DbContextReflector(Context, GlobalValues.ReleaseContextModelsNamespace);
            comparer = new EntityComparerByKeys();
        }

        [TestCleanup]
        public void Destroy()
        {
            Context.Dispose();
        }

        [TestMethod]
        public void SingleKey_IdsNotEqual()
        {
            var keyProperties = reflector.GetKeyProperties<Release>();

            var release1 = new Release { Id = 1, ArtistId = 2, Date = 1999, MediaId = 1 };
            var release2 = new Release { Id = 2, ArtistId = 2, Date = 1999, MediaId = 1 };

            bool areEqual = comparer.CompareEntities<Release>(release1, release2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void SingleKey_IdsNotEqual_OneIdEqualsZero()
        {
            var keyProperties = reflector.GetKeyProperties<Release>();

            var release1 = new Release { Id = 1, ArtistId = 2, Date = 1999, MediaId = 1 };
            var release2 = new Release { Id = 0, ArtistId = 2, Date = 1999, MediaId = 1 };

            bool areEqual = comparer.CompareEntities<Release>(release1, release2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void SingleKey_IdsAreEqual()
        {
            var keyProperties = reflector.GetKeyProperties<Release>();

            var release1 = new Release { Id = 2, ArtistId = 2, Date = 1999, MediaId = 1 };
            var release2 = new Release { Id = 2, ArtistId = 1, Date = 2000, MediaId = 2 };

            bool areEqual = comparer.CompareEntities<Release>(release1, release2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }

        [TestMethod]
        public void SingleKey_IdsAreEqual_IdEqualsZero()
        {
            var keyProperties = reflector.GetKeyProperties<Release>();

            var release1 = new Release { Id = 0, ArtistId = 2, Date = 1999, MediaId = 1 };
            var release2 = new Release { Id = 0, ArtistId = 1, Date = 2000, MediaId = 2 };

            bool areEqual = comparer.CompareEntities<Release>(release1, release2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }

        // Composite Keys //
        [TestMethod]
        public void CompositeKey_AllKeys_AreEqual()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 5, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_NonForeignKeys_AreEqual_EntitiesNotEqual()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.English, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 1, Text = "Text value 1" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_ForeignKeys_AreEqual_EntitiesNotEqual()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 1, Language = Language.Japanese, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 1, Text = "Text value 1" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_ForeignKeys_AreDefault_EntitiesNotEqual()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 0, Language = Language.English, Id = 1, Text = "Text value 1" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_ForeignKeys_AreDefault_EntitiesAreEqual()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 1, Text = "Text value 1" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }
    }

    
}
