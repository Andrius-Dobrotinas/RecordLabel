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
    public class EntityComparerByNonForeignKeysTests
    {
        protected ReleaseContext Context;
        protected DbContextReflector reflector;
        protected EntityComparerByKeys comparer;

        [TestInitialize]
        public void Initialize()
        {
            Context = new ReleaseContext(GlobalValues.UnitTestReinitializableConnectionString);

            reflector = new DbContextReflector(Context, GlobalValues.ReleaseContextModelsNamespace, GlobalValues.ReleaseContextModelsAssembly);
            comparer = new EntityComparerByNonForeignKeys();
        }

        [TestMethod]
        public void CompositeKey_NonForeignKeys_AreEqual1()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.English, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 1, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_NonForeignKeys_AreEqual2()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.Japanese, Id = 1, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_NonForeignKeys_AreNotEqual1()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.English, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.Japanese, Id = 1, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_NonForeignKeys_AreNotEqua2()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.English, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 1, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreNotEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_AllKeys_AreEqual1()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 1, Language = Language.English, Id = 5, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }

        [TestMethod]
        public void CompositeKey_AllKeys_AreEqual2()
        {
            var keyProperties = reflector.GetKeyProperties<LocalizedString>();

            var entity1 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 1, Text = "Text value 1" };
            var entity2 = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese, Id = 5, Text = "Some other value" };

            bool areEqual = comparer.CompareEntities<LocalizedString>(entity1, entity2, keyProperties);

            Assert.AreEqual(true, areEqual);
        }
    }
}