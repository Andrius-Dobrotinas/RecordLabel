using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using RecordLabel;
using RecordLabel.Data;
using RecordLabel.Data.ok;
using RecordLabel.Data.Models;
using RecordLabel.Data.Context;

namespace RepositoryTests
{
    [TestClass]
    public class ScalarPropertyUpdaterTests : ReinitializedReleaseContextTestsBase
    {
        private TEntity UpdateEntity<TEntity>(TEntity model) where TEntity : class, IHasId
        {
            var reflector = new DbContextReflector(Context, "RecordLabel.Data.Models");
            IEntityUpdater updater = new ScalarPropertyUpdater(Context, reflector);
            return updater.UpdateEntity(model);
        }

        [TestMethod]
        public void CheckIfEntityIsFoundInTheContext()
        {
            var model = new Release { Id = 3 };
            var updatedModel = UpdateEntity(model);
            Assert.AreNotEqual(model, updatedModel, "Model was added to the context instead of being retrieved from it");
        }

        [TestMethod]
        public void CheckIfEntityIsAddedToTheContext()
        {
            var model = new Release { Id = 0 };
            var updatedModel = UpdateEntity(model);
            Assert.AreEqual(model, updatedModel, "Model was not added to the context as new entity");
        }
    }
}
