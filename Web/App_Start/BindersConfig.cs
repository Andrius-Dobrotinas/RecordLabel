using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordLabel.Content;
using RecordLabel.Web.ModelBinding;

namespace RecordLabel.Web
{
    public class BindersConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(Artist), new ArtistBinder());
            binders.Add(typeof(Article), new BaseWithImagesBinder());
            binders.Add(typeof(Release), new ReleaseBinder());

            //Binders that return null for these types so that they could be easily removed from their respective sets
            binders.Add(typeof(Reference), new IKnowIfImEmptyBinder());
            binders.Add(typeof(Track), new IKnowIfImEmptyBinder());

            //Binders that remove empty elements from sets
            binders.Add(typeof(ReferenceSet), new SetBinder<Reference>()); //new ReferenceSetBinder());
            binders.Add(typeof(Tracklist), new SetBinder<Track>()); //new TracklistBinder());
            binders.Add(typeof(LocalStringSet), new SetBinder<LocalString>()); //new LocalStringSetBinder());
        }
    }
}