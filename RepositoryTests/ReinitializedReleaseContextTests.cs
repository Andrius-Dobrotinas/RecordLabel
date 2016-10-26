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
    public class ReinitializedReleaseContextTests : ReinitializedReleaseContextTestsBase
    {
        [TestMethod]
        public void Test1()
        {
                var release1 = new Release
                {
                    Id = 3,
                    ArtistId = 1,
                    Title = "Raw Power",
                    CatalogueNumber = "KC3200",
                    Date = 1973,
                    MediaId = 1,
                    /*LocalizedText = createLocalizedStrings<LocalizedString>(db,
                    "This is awesome!", "これは素晴らしいです")*/
                };

                var track1 = new Track { Id = 1, Title = "Down On The Street" };
                track1.Reference = new TrackReference { Id = 1, Target = "http://www.NewReference.com", Type = ReferenceType.File };


                var track5 = new Track { Title = "Seek And Destroy II" };
                track5.Reference = new TrackReference { Target = "http://www.Metallica.com", Type = ReferenceType.OtherVideo };
                /*
                var track6 = new Track { Title = "Seek And Destroy III" };
                track6.Reference = new TrackReference { Target = "http://www.TheStooges.com", Type = ReferenceType.Website };*/

                var metadata3 = new Metadata
                {
                    Id = 1,
                    /*Type = MetadataType.Attribute,
                    LocalizedText = RecordLabel.Data.Models.Configurations.DropCreateAndSeedInitializer<ReleaseContext>.createLocalizedStrings<MetadataLocalizedString>(context,
                    "Heavy Metal", "M.H.")*/
                };


                release1.Tracks.Add(track1);
                release1.Tracks.Add(track5);
                // release1.Tracks.Add(track6);
                release1.Metadata.Add(metadata3);

                var repository = new RecordLabel.Data.ok.ReleaseRepository(Context);
                repository.SaveModel(release1);
                repository.SaveChanges();


            // TODO: add asserts
        }
    }
}
