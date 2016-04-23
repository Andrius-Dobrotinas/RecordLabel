using RecordLabel.Catalogue;

namespace RecordLabel.Web.Controllers
{
    public class AttributeController : MetadataController<RecordLabel.Catalogue.Metadata.Attribute>
    {
        public AttributeController() : base(new ReleaseContext(), context => context.Attributes)
        {

        }
    }
}
