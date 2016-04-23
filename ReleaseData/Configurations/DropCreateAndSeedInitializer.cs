using RecordLabel.Catalogue;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace RecordLabel.Catalogue.Configurations
{
    public class DropCreateAndSeedInitializer<T> : DropCreateDatabaseAlways<T> where T : ReleaseContext
    {
        protected override void Seed(T db)
        {
            var attrib1_local = createLocalStringSet(db, "attribute", "attribute-lt");
            var attrib1 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = attrib1_local });

            var genre1_local = createLocalStringSet(db, "metal", "metal-lt");
            var attrib2 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = genre1_local, Type = Catalogue.Metadata.AttributeType.Genre });

            //Artist 1
            //var description1 = createLocalStringSet(db, "Some short description", "some short description-lt");
            var artist1 = db.Artists.Add(new Artist() { Name = "Iggy & The Stooges" });
            artist1.Attributes = new AttributeSet();
            artist1.Attributes.Collection.Add(attrib1);

            //Artist 3
            //var description2 = createLocalStringSet(db, "Just a really cool band from Switzerland", null);
            var artist2 = db.Artists.Add(new Artist() { Name = "Celtic Frost" });
            artist2.Attributes = new AttributeSet();
            artist2.Attributes.Collection.Add(attrib1);
            artist2.Attributes.Collection.Add(attrib2);

            //Artist 3
            //var description3 = createLocalStringSet(db, "Need no introduction", null);
            var artist3 = db.Artists.Add(new Artist() { Name = "The Rolling Stones" });


            db.SaveChanges();


            var genre2_local = createLocalStringSet(db, "stoner rock and roll", "stoner rock and roll-lit");
            var attrib5 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = genre2_local, Type = Catalogue.Metadata.AttributeType.Genre });

            var genre3 = createLocalStringSet(db, "oldschool metal", "oldschool metal");
            var attrib7 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = genre3, Type = Catalogue.Metadata.AttributeType.Genre });

            var genre4_local = createLocalStringSet(db, "progressive rock", "progressive rock-lt");
            var attrib8 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = genre4_local, Type = Catalogue.Metadata.AttributeType.Genre });

            var attr_local3 = createLocalStringSet(db, null, "best-lt");
            var attrib3 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = attr_local3 });

            var attr_local2 = createLocalStringSet(db, "artpop", "artpop-lt");
            var attrib4 = db.Attributes.Add(new Catalogue.Metadata.Attribute() { Localization = attr_local2 });

            artist1.Attributes.Collection.Add(attrib4);


            //References
            var reference1 = new Reference()
            {
                Localization = createLocalStringSet(db, "Special website", null),
                Target = "http://www.metallica.com",
                Type = ReferenceType.Website
            };

            var reference2 = new Reference()
            {
                Localization = createLocalStringSet(db, null, "Relevant info"),
                Target = "http://www.microsoft.com",
                Type = ReferenceType.Website
            };

            var reference3 = new Reference()
            {
                Localization = createLocalStringSet(db, "Youtube video", null),
                Target = "B_o-tG09Bj8",
                Type = ReferenceType.Youtube
            };

            var reference4 = new Reference()
            {
                Localization = createLocalStringSet(db, "Guitar lesson", null),
                Target = "jqgo8kl32No",
                Type = ReferenceType.Youtube
            };

            var reference_Stooges = new Reference()
            {
                Localization = createLocalStringSet(db, "The Stooges' Homepage", null),
                Target = "http://www.iggyandthestoogesmusic.com",
                Type = ReferenceType.Website
            };

            //Reference sets
            ReferenceSet refSet = new ReferenceSet()
            {
                Collection = new List<Reference>()
                        {
                            reference1, reference2, reference3, reference4
                        }
            };
            db.ReferenceSets.Add(refSet);

            //Media Types
            var mediaCD = new Catalogue.Metadata.MediaType()
            {
                Localization = createLocalStringSet(db, "CD", null)
            };
            var mediaFLAC = new Catalogue.Metadata.MediaType()
            {
                Localization = createLocalStringSet(db, "FLAC", null)
            };

            var mediaLP = new Catalogue.Metadata.MediaType()
            {
                Localization = createLocalStringSet(db, "LP", null)
            };

            //Releases
            Release release1 = new Release()
            {
                Artist = artist1,
                Title = "Raw Power",
                References = refSet,
                Date = 1973,
                Media = mediaLP,
                IsMasterVersion = true,
                CatalogueNumber = "CK32003",
                Descriptions = createLocalStringSet(db, "Last record by the Stooges... until 2007", null),
                Localization = createLocalStringSet(db, "Greatest record on earth! After a bunch of others :)", null)
            };
            db.Releases.Add(release1);

            Release release2 = new Release()
            {
                Artist = artist1,
                Title = "Raw Power",
                //References = refSet,
                Date = 2014, //DateTime.Now,
                Media = mediaFLAC,
                MasterVersion = release1,
                CatalogueNumber = "FLAC322",
                Attributes = new AttributeSet(new List<Catalogue.Metadata.Attribute>() { attrib5, attrib7, attrib3 })
            };

            Release release3 = new Release()
            {
                Artist = artist2,
                Title = "Monotheist",
                Date = 2006,
                Media = mediaCD,
                CatalogueNumber = "CM2500",
                IsMasterVersion = true,
                Attributes = new AttributeSet(new List<Catalogue.Metadata.Attribute>() { attrib4, attrib8, attrib7, attrib5 })
            };

            Release release4 = new Release()
            {
                Artist = artist3,
                Title = "Through The Past, Darkly (Big Hits Vol.2)",
                Date = 1967,
                Media = mediaLP,
                CatalogueNumber = "Dec 25",
                Attributes = new AttributeSet(new List<Catalogue.Metadata.Attribute>() { attrib4, attrib7 })
            };

            Release release5 = new Release()
            {
                Artist = artist3,
                Title = "Out of Our Heads",
                Date = 2003,
                Media = mediaCD,
                CatalogueNumber = "38505",
                Attributes = new AttributeSet(new List<Catalogue.Metadata.Attribute>() { attrib4 })
            };

            Release release6 = new Release()
            {
                Artist = artist2,
                Title = "Monotheist",
                Date = 2016,
                Media = mediaLP,
                CatalogueNumber = "CMLP500",
                MasterVersion = release3,
                Attributes = new AttributeSet(new List<Catalogue.Metadata.Attribute>() { attrib4, attrib7 })
            };

            Release release7 = new Release()
            {
                Artist = artist2,
                Title = "Monotheist",
                Date = 2015,
                Media = mediaFLAC,
                CatalogueNumber = "artist2",
                MasterVersion = release3,
                Attributes = new AttributeSet(new List<Catalogue.Metadata.Attribute>() { attrib1, attrib3 })
            };

            db.Releases.Add(release2);
            db.Releases.Add(release3);
            db.Releases.Add(release4);
            db.Releases.Add(release5);
            db.Releases.Add(release6);


            //Tracklist
            release1.Tracklist = new Tracklist();
            release1.Tracklist.Collection.Add(new Track()
            {
                Title = "Search And Destroy",
                Reference = reference_Stooges
            });
            release1.Tracklist.Collection.Add(new Track()
            {
                Title = "Search And Destroy"
            });

            //Articles
            Article new1 = new Article()
            {
                Localization = createLocalStringSet(db, "is an American heavy metal band formed in Los Angeles, California. Metallica was formed in 1981 when vocalist/guitarist James Hetfield responded to an advertisement posted by drummer Lars Ulrich in a local newspaper. The band's current line-up comprises founding members Hetfield and Ulrich, longtime lead guitarist Kirk Hammett and bassist Robert Trujillo. Guitarist Dave Mustaine and bassists Ron McGovney, Cliff Burton and Jason Newsted are former members of the band. Metallica collaborated over a long period with producer Bob Rock, who produced four of the band's studio albums between 1990 and 2003 and served as a temporary bassist during the production of St. Anger (2003) prior to recruiting Trujillo.", null),
                Date = DateTime.Now,
                Type = ArticleType.News,
                Titles = createLocalStringSet(db, "Some news about Metallica!", null)
            };

            Article new2 = new Article()
            {
                Localization = createLocalStringSet(db, "<p>The band's fast tempos, instrumentals, and aggressive musicianship placed them as one of the founding \"big four\" bands of <a href=\"/wiki/Thrash_metal\" title=\"Thrash metal\">thrash metal</a>, alongside <a href=\"/wiki/Anthrax_(American_band)\" title=\"Anthrax (American band)\">Anthrax</a>, <a href=\"/wiki/Megadeth\" title=\"Megadeth\">Megadeth</a>, and <a href=\"/wiki/Slayer\" title=\"Slayer\">Slayer</a>. Metallica earned a growing fan base in the <a href=\"/wiki/Underground_music\" title=\"Underground music\">underground music</a> community and won critical acclaim with its first four albums; their third album <i><a href=\"/wiki/Master_of_Puppets\" title=\"Master of Puppets\">Master of Puppets</a></i> (1986) was described as one of the most influential and heaviest of thrash metal albums. The band achieved substantial commercial success with its <a href=\"/wiki/Eponymy\" title=\"Eponymy\" class=\"mw-redirect\">eponymous</a> fifth album <i><a href=\"/wiki/Metallica_(album)\" title=\"Metallica (album)\">Metallica</a></i> (1991), which debuted at number one on the <a href=\"/wiki/Billboard_200\" title=\"Billboard 200\"><i>Billboard</i> 200</a>. With this release the band expanded its musical direction, resulting in an album that appealed to a more mainstream audience. In 2000, Metallica was among a number of artists who filed a <a href=\"/wiki/Metallica_v._Napster,_Inc.\" title=\"Metallica v. Napster, Inc.\">lawsuit</a> against <a href=\"/wiki/Napster\" title=\"Napster\">Napster</a> for sharing the band's copyright-protected material for free without consent from any band member. A settlement was reached and Napster became a pay-to-use service. The release of <i>St. Anger</i> (2003) alienated many fans with the exclusion of guitar solos and the \"steel-sounding\" <a href=\"/wiki/Snare_drum\" title=\"Snare drum\">snare drum</a>, and a film titled <i><a href=\"/wiki/Some_Kind_of_Monster_(film)\" title=\"Some Kind of Monster (film)\">Some Kind of Monster</a></i> documented the recording of <i>St. Anger</i> and the tensions within the band during that time. The band returned to its original musical style with the release of <i><a href=\"/wiki/Death_Magnetic\" title=\"Death Magnetic\">Death Magnetic</a></i> (2008), and in 2009, Metallica was inducted into the <a href=\"/wiki/Rock_and_Roll_Hall_of_Fame\" title=\"Rock and Roll Hall of Fame\">Rock and Roll Hall of Fame</a>.</p>", null),
                Date = DateTime.Now,
                Type = ArticleType.Article,
                Titles = createLocalStringSet(db, "First article in a long series of articles about Metallica", null)
            };
            db.Articles.Add(new1);
            db.Articles.Add(new2);

            base.Seed(db);
        }

        private static LocalStringSet createLocalStringSet(ReleaseContext db, string en, string loc2)
        {
            var locen = new LocalString() { Text = en };
            var loclt = new LocalString() { Language = Language.Lithuanian, Text = loc2 };
            List<LocalString> locoLection = new List<LocalString>() { locen, loclt };

            return db.LocalStringSet.Add(new LocalStringSet() { Collection = locoLection });
        }
    }
}