using RecordLabel.Catalogue;

namespace RecordLabel.Web.Controllers
{
    public class MediaTypeController : MetadataController<RecordLabel.Catalogue.Metadata.MediaType>
    {
        public MediaTypeController() : base(new ReleaseContext(), context => context.MediaTypes)
        {

        }
    }
}
