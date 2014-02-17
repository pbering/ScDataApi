using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;

namespace ScDataApi.Storage
{
    public class FullDataItem : MinimalDataItem
    {
        private IEnumerable<DataField> _fields = Enumerable.Empty<DataField>();

        public FullDataItem(Item item, string fields) : base(item, fields)
        {
            var dataFields = new List<DataField>();

            dataFields.AddRange(GetAllFields(item));
            dataFields.AddRange(GetSpecificFields(item, fields));

            _fields = dataFields.Distinct();
        }

        public override IEnumerable<DataField> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        private IEnumerable<DataField> GetAllFields(Item item)
        {
            if (item == null)
            {
                yield break;
            }

            foreach (var field in item.Fields.Where(f => !f.Key.StartsWith("__")))
            {
                yield return new DataField(field.Key.ToLowerInvariant(), GetFieldValue(field));
            }
        }
    }
}