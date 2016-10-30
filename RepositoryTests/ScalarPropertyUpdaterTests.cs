using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using RecordLabel;
using RecordLabel.Data;
using RecordLabel.Data.ok;
using RecordLabel.Data.Models;
using RecordLabel.Data.Context;
using AndrewD.EntityPlus;
using AndrewD.EntityPlus.Persistence;

namespace RepositoryTests
{
    [TestClass]
    public class ScalarPropertyUpdaterTests : ReinitializedReleaseContextTestsBase
    {
        private TEntity UpdateEntity<TEntity>(TEntity model) where TEntity : class, IHasId
        {
            var reflector = new DbContextReflector(Context, GlobalValues.ReleaseContextModelsNamespace, GlobalValues.ReleaseContextModelsAssembly);
            IEntityUpdater updater = new ScalarPropertyUpdater(Context, reflector);
            return updater.UpdateEntity(model);
        }

        [TestMethod]
        public void SingleKey_EntityMustBeFoundInTheContext()
        {
            var model = new Release { Id = 3 };
            var updatedModel = UpdateEntity(model);
            Assert.AreNotEqual(model, updatedModel, "Model was added to the context instead of being retrieved from it");
        }

        [TestMethod]
        public void SingleKey_EntityMustBeAddedToTheContext()
        {
            var model = new Release { Id = 0 };
            var updatedModel = UpdateEntity(model);
            Assert.AreEqual(model, updatedModel, "Model was not added to the context as new entity");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void SingleKey_EntityKeyHasNonDefaultValueAndDoesNotExistInContext()
        {
            var model = new Release { Id = 100 };
            var updatedModel = UpdateEntity(model);
        }

        // Composite Keys //
        [TestMethod]
        public void CompositeKey_EntityMustBeFoundInTheContext1()
        {
            int targetObjectId = Context.Releases.First().Id;
            var model = new LocalizedString { TargetObjectId = targetObjectId, Language = Language.Japanese };
            var updatedModel = UpdateEntity(model);
            Assert.AreNotEqual(model, updatedModel, "Model was added to the context instead of being retrieved from it");
        }

        [TestMethod]
        public void CompositeKey_EntityMustBeFoundInTheContext2()
        {
            int targetObjectId = Context.Releases.First().Id;
            var model = new LocalizedString { TargetObjectId = targetObjectId, Language = Language.English };
            var updatedModel = UpdateEntity(model);
            Assert.AreNotEqual(model, updatedModel, "Model was added to the context instead of being retrieved from it");
        }

        [TestMethod]
        public void CompositeKey_EntityForeignKeyValueDoesNotPointToAnExistingEntity_IsDefault_MustBeAdded1()
        {
            var model = new LocalizedString { TargetObjectId = 0, Language = Language.Japanese };
            var updatedModel = UpdateEntity(model);
            Assert.AreEqual(model, updatedModel);
        }

        [TestMethod]
        public void CompositeKey_EntityForeignKeyValueDoesNotPointToAnExistingEntity_IsDefault_MustBeAdded2()
        {
            var model = new LocalizedString { TargetObjectId = 0, Language = Language.English };
            var updatedModel = UpdateEntity(model);
            Assert.AreEqual(model, updatedModel);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CompositeKey_EntityForeignKeyValueDoesNotPointToAnExistingEntity()
        {
            var model = new LocalizedString { TargetObjectId = 100, Language = Language.Japanese };
            var updatedModel = UpdateEntity(model);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CompositeKey_EntityKeyHasNonDefaultValueAndDoesNotExistInContext2()
        {
            var model = new LocalizedString { TargetObjectId = 100, Language = Language.Japanese };
            var updatedModel = UpdateEntity(model);
        }
    }
}
