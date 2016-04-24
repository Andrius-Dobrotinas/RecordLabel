using RecordLabel.Content;

namespace RecordLabel.Web.Controllers
{
    public class AttributeController : MetadataController<RecordLabel.Content.Metadata.Attribute>
    {
        public AttributeController() : base(new ReleaseContext(), context => context.Attributes)
        {

        }
    }
}
