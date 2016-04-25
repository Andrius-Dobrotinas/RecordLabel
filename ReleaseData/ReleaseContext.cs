using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;

namespace RecordLabel.Content
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ReleaseContext : DbContext
    {
        public DbSet<Release> Releases { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<AttributeSet> AttributeSets { get; set; }
        public DbSet<Metadata.MediaType> MediaTypes { get; set; }
        public DbSet<Metadata.Attribute> Attributes { get; set; }
        public DbSet<ImageSet> ImageSets { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<LocalStringSet> LocalStringSet { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<ReferenceSet> ReferenceSets { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Tracklist> Tracklists { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.Add(new Configurations.ReleaseConfiguration());
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<AttributeSet>().HasMany(x => x.Collection).WithMany(x => x.AttributeSets).Map(x => x.ToTable("AttributeMap", "JoinTables"));
            modelBuilder.Entity<LocalStringSet>().HasMany(x => x.Collection).WithRequired(y => y.LocalizationObject).WillCascadeOnDelete();
        }

        public void UpdateModel<TModel>(TModel model, TModel copyFrom) where TModel : EntityBase
        {
            model.UpdateModel(this, copyFrom);
        }

        public static void UpdatePropertyValues<T>(ref int sourceId, ref T sourceValue, ref int targetId, ref T targetValue) where T : class
        {
            if (targetId != sourceId)
            {
                targetId = sourceId;
                if (sourceValue != null) //check if the id property is non-nullable
                {
                    targetValue = sourceValue;
                }
            }
            else if (targetValue != sourceValue)
            {
                //this implies that the property is non-nullable
                if (sourceValue != null)
                {
                    targetValue = sourceValue;
                }
            }
        }

        /*public static void UpdateProperty<TModel, TProperty>(TModel targetModel, TModel sourceModel, Expression<Func<TModel, TProperty>> property, Expression<Func<TModel, int>> idProperty) where TModel : class where TProperty : class
        {
            PropertyInfo pinfo = (PropertyInfo)((MemberExpression)property.Body).Member;
            PropertyInfo idpinfo = (PropertyInfo)((MemberExpression)idProperty.Body).Member;

            TProperty targetValue = pinfo.GetValue(sourceModel) as TProperty;
            int targetIdValue = Convert.ToInt32(idpinfo.GetValue(sourceModel));

            if (targetModel != source.MediaId)
            {
                MediaId = source.MediaId;
                if (source.Media != null) //chech if the id property is non-nullable
                {
                    Media = source.Media;
                }
            }
            else if (Media != source.Media)
            {
                //this implies that the property is non-nullable
                if (source.Media != null)// && Media != source.Media)
                {
                    Media = source.Media;
                }
            }
        }*/
    }
}
