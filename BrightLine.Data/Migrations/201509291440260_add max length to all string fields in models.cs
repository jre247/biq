namespace BrightLine.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmaxlengthtoallstringfieldsinmodels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ad", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.AdGroup", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.AdType", "ManifestName", c => c.String(maxLength: 255));
            AlterColumn("dbo.AdTypeGroup", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Creative", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Feature", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Feature", "ButtonName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Blueprint", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Blueprint", "ManifestName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Resource", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.FileType", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.ResourceType", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.StorageSource", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Platform", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Channel", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Channel", "ManifestName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Category", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.FeatureCategory", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.FeatureTypeGroup", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.ProductLine", "HexBackgroundColor", c => c.String(maxLength: 255));
            AlterColumn("dbo.ProductLine", "Abbreviation", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsModelDefinition", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsModelDefinition", "DisplayFieldName", c => c.String(maxLength: 255));
            AlterColumn("dbo.FieldType", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsField", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsField", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.CmsField", "DefaultValueString", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsRefType", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.PageDefinition", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Expose", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsSettingDefinition", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsModel", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsModelInstance", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsModelInstanceField", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.CmsModelInstanceField", "Metatype", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsModelInstanceFieldValue", "StringValue", c => c.String(maxLength: 255));
            AlterColumn("dbo.Placement", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.App", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Agency", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsSettingInstance", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.CmsSetting", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.FileItem", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.FileItem", "FullNameRaw", c => c.String(maxLength: 255));
            AlterColumn("dbo.Metric", "Display", c => c.String(maxLength: 255));
            AlterColumn("dbo.Metric", "ShortDisplay", c => c.String(maxLength: 255));
            AlterColumn("dbo.Metric", "HexColor", c => c.String(maxLength: 255));
            AlterColumn("dbo.SecurableAction", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.SecurableAction", "Operation", c => c.String(maxLength: 255));
            AlterColumn("dbo.SecurableArea", "Name", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SecurableArea", "Name", c => c.String());
            AlterColumn("dbo.SecurableAction", "Operation", c => c.String());
            AlterColumn("dbo.SecurableAction", "Name", c => c.String());
            AlterColumn("dbo.Metric", "HexColor", c => c.String());
            AlterColumn("dbo.Metric", "ShortDisplay", c => c.String());
            AlterColumn("dbo.Metric", "Display", c => c.String());
            AlterColumn("dbo.FileItem", "FullNameRaw", c => c.String());
            AlterColumn("dbo.FileItem", "Name", c => c.String());
            AlterColumn("dbo.CmsSetting", "Name", c => c.String());
            AlterColumn("dbo.CmsSettingInstance", "Name", c => c.String());
            AlterColumn("dbo.Agency", "Name", c => c.String());
            AlterColumn("dbo.App", "Name", c => c.String());
            AlterColumn("dbo.Placement", "Name", c => c.String());
            AlterColumn("dbo.CmsModelInstanceFieldValue", "StringValue", c => c.String());
            AlterColumn("dbo.CmsModelInstanceField", "Metatype", c => c.String());
            AlterColumn("dbo.CmsModelInstanceField", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CmsModelInstance", "Name", c => c.String());
            AlterColumn("dbo.CmsModel", "Name", c => c.String());
            AlterColumn("dbo.CmsSettingDefinition", "Name", c => c.String());
            AlterColumn("dbo.Expose", "Name", c => c.String());
            AlterColumn("dbo.PageDefinition", "Name", c => c.String());
            AlterColumn("dbo.CmsRefType", "Name", c => c.String());
            AlterColumn("dbo.CmsField", "DefaultValueString", c => c.String());
            AlterColumn("dbo.CmsField", "Description", c => c.String());
            AlterColumn("dbo.CmsField", "Name", c => c.String());
            AlterColumn("dbo.FieldType", "Name", c => c.String());
            AlterColumn("dbo.CmsModelDefinition", "DisplayFieldName", c => c.String());
            AlterColumn("dbo.CmsModelDefinition", "Name", c => c.String());
            AlterColumn("dbo.ProductLine", "Abbreviation", c => c.String());
            AlterColumn("dbo.ProductLine", "HexBackgroundColor", c => c.String());
            AlterColumn("dbo.FeatureTypeGroup", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.FeatureCategory", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Category", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Channel", "ManifestName", c => c.String());
            AlterColumn("dbo.Channel", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Platform", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.StorageSource", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ResourceType", "Name", c => c.String());
            AlterColumn("dbo.FileType", "Name", c => c.String());
            AlterColumn("dbo.Resource", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Blueprint", "ManifestName", c => c.String());
            AlterColumn("dbo.Blueprint", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Feature", "ButtonName", c => c.String());
            AlterColumn("dbo.Feature", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Creative", "Description", c => c.String());
            AlterColumn("dbo.AdTypeGroup", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.AdType", "ManifestName", c => c.String());
            AlterColumn("dbo.AdGroup", "Name", c => c.String());
            AlterColumn("dbo.Ad", "Name", c => c.String());
        }
    }
}
