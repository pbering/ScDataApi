using Sitecore.Data.Items;

namespace ScDataApi.Storage
{
    public class MinimalDataItem : DataItem
    {
        public MinimalDataItem(Item item, string fields) : base(item, fields)
        {
            Id = item.ID.ToString();
            ParentId = item.ParentID.ToString();
            TemplateId = item.TemplateID.ToString();
            TemplateName = item.TemplateName;
            Key = item.Key;
            Path = item.Paths.FullPath;
            HasChildren = item.HasChildren;
            DisplayName = item.DisplayName;
        }

        public bool HasChildren { get; set; }

        public string Path { get; set; }

        public string Key { get; set; }

        public string TemplateName { get; set; }

        public string TemplateId { get; set; }

        public string ParentId { get; set; }

        public string Id { get; set; }

        public string DisplayName { get; set; }
    }
}