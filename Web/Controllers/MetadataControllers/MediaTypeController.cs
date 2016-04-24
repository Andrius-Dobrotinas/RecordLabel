using RecordLabel.Content;

namespace RecordLabel.Web.Controllers
{
    public class MediaTypeController : MetadataController<RecordLabel.Content.Metadata.MediaType>
    {
        public MediaTypeController() : base(new ReleaseContext(), context => context.MediaTypes)
        {

        }
    }
}
