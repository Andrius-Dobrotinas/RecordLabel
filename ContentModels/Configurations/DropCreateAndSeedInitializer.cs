using RecordLabel.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecordLabel.Data.Models.Configurations
{
    public class DropCreateAndSeedInitializer<T> : DropCreateDatabaseAlways<T> where T : ReleaseContext
    {
        protected override void Seed(T db)
        {
            // MEDIA TYPES
            var media_LP = new MediaType { LocalizedText = createLocalizedStrings<MediaTypeLocalizedString>(db, "LP", "ビニールレコード") };
            var media_CD = new MediaType { LocalizedText = createLocalizedStrings<MediaTypeLocalizedString>(db, "CD", null) };
            var media_FLAC = new MediaType { LocalizedText = createLocalizedStrings<MediaTypeLocalizedString>(db, "FLAC", "F.L.A.C") };


            // ARTISTS
            var artist1 = new Artist {
                Name = "Iggy & The Stooges",
                LocalizedText = createLocalizedStrings<LocalizedString>(db,
                    "Iggy with Asheton bros and the new guy on guitar", "Iggy")
            };
            db.Artists.Add(artist1);

            var artist2 = new Artist
            {
                Name = "The Stooges",
                LocalizedText = createLocalizedStrings<LocalizedString>(db,
                    "Iggy, Asheton bros and Dave", "Iggy, Asheton ブラザーズ そして Dave")
            };
            db.Artists.Add(artist2);


            // RELEASES
            var release1 = new Release
            {
                Artist = artist1,
                Title = "Raw Power",
                CatalogueNumber = "KC3200",
                Date = 1973,
                Media = media_LP,
                LocalizedText = createLocalizedStrings<LocalizedString>(db,
                    "This is awesome!", "これは素晴らしいです")
            };
            db.Releases.Add(release1);

            var release2 = new Release
            {
                Artist = artist2,
                Title = "The Stooges",
                CatalogueNumber = "EK74000",
                Date = 1973,
                Media = media_LP,
                LocalizedText = createLocalizedStrings< LocalizedString>(db, 
                    "Their first record", "彼らの最初のレコード")
            };
            db.Releases.Add(release2);

            var release3 = new Release
            {
                Artist = artist2,
                Title = "Fun House",
                CatalogueNumber = "EK74007",
                Date = 1973,
                Media = media_LP,
                LocalizedText = createLocalizedStrings<LocalizedString>(db,
                    "They had no idea this would be the best rock and roll record ever!", "彼らはこれが史上最高のロックンロールのレコードになります知りませんでした！")
            };
            db.Releases.Add(release3);

            db.SaveChanges();


            // METADATA
            var metadata1 = new Metadata {
                Type = MetadataType.Genre,
                LocalizedText = createLocalizedStrings<MetadataLocalizedString>(db,
                    "Hard Rock", "ハードロック")
            };
            var metadata2 = new Metadata {
                Type = MetadataType.Attribute,
                LocalizedText = createLocalizedStrings<MetadataLocalizedString>(db,
                    "Classic Rock", "クラシック・ロック")
            };

            metadata1.Targets.Add(artist1);
            metadata1.Targets.Add(release1);
            metadata1.Targets.Add(release2);
            metadata2.Targets.Add(release1);
            metadata2.Targets.Add(release3);

            db.Metadata.Add(metadata1);
            db.Metadata.Add(metadata2);

            db.SaveChanges();


            // TRACKS
            var track1 = new Track { Title = "Search And Destroy" };
            track1.Reference = new TrackReference { Target = "http://www.google.com", Type = ReferenceType.Website };
            release1.Tracks.Add(track1);

            var track2 = new Track { Title = "Blank" };
            release1.Tracks.Add(track2);
            db.SaveChanges();

            var track3 = new Track { Title = "Gotcha!" };
            var trackReference3 = new TrackReference { Target = "http://www.youtube.com", Type = ReferenceType.Youtube };
            track3.Reference = trackReference3;

            release1.Tracks.Add(track3);

            db.SaveChanges();


            base.Seed(db);
        }

        private static List<TLocalization> createLocalizedStrings<TLocalization>(ReleaseContext db, string en, string loc2) where TLocalization : LocalizedStringBase, new()
        {
            var list = new List<TLocalization>();

            if (!string.IsNullOrEmpty(en))
                list.Add(new TLocalization() { Text = en });

            if (!string.IsNullOrEmpty(en))
                list.Add(new TLocalization() { Language = Language.Japanese, Text = loc2 });

            return list;
        }
    }
}