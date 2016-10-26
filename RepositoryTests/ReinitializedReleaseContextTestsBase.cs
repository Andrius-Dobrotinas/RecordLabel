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
    /// <summary>
    /// Context is reinitialized with fresh test data before testing
    /// </summary>
    public abstract class ReinitializedReleaseContextTestsBase
    {
        protected static string ConnectionString = "UnitTestReinitializableConnection";

        protected ReleaseContext Context;

        [TestInitialize]
        public void Initialize()
        {
            //Reinitialize the database with test data
            System.Data.Entity.Database.SetInitializer(new RecordLabel.Data.Models.Configurations.DropCreateAndSeedInitializer<ReleaseContext>());

            Context = new ReleaseContext(ConnectionString);
        }

        [TestCleanup]
        public void Destroy()
        {
            Context.Dispose();
        }
    }
}
