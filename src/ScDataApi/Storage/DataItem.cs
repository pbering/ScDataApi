using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace ScDataApi.Storage
{
    public class DataItem
    {
        private IEnumerable<DataField> _fields = Enumerable.Empty<DataField>();

        public DataItem(Item item, string payload, string fields)
        {
            var dataFields = new List<DataField>();

            if (payload.Equals("min", StringComparison.OrdinalIgnoreCase))
            {
                dataFields.AddRange(GetMinimalFields(item));
            }
            else if (payload.Equals("full", StringComparison.OrdinalIgnoreCase))
            {
                dataFields.AddRange(GetMinimalFields(item));
                dataFields.AddRange(GetAllFields(item));
            }
            else if (payload.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                dataFields.AddRange(GetMinimalFields(item).Where(f => fields.IndexOf(f.Key, StringComparison.OrdinalIgnoreCase) > -1));
            }
            else
            {
                throw new ArgumentException("Please use either 'min', 'full' or 'custom'", "payload");
            }

            dataFields.AddRange(GetSpecificFields(item, fields));

            _fields = dataFields.Distinct();
        }

        public IEnumerable<DataField> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        private IEnumerable<DataField> GetMinimalFields(Item item)
        {
            if (item == null)
            {
                yield break;
            }

            yield return new DataField("__Id", item.ID.ToString());
            yield return new DataField("__ParentId", item.ParentID.ToString());
            yield return new DataField("__Key", item.Key);
            yield return new DataField("__Path", item.Paths.FullPath.ToLowerInvariant());
            yield return new DataField("__TemplateId", item.TemplateID.ToString());
            yield return new DataField("__TemplateName", item.TemplateName);
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

        private IEnumerable<DataField> GetSpecificFields(Item item, string fields)
        {
            if (item == null)
            {
                yield break;
            }

            if (string.IsNullOrEmpty(fields))
            {
                yield break;
            }

            foreach (var key in fields.Split(',').Select(f => f.Trim()))
            {
                var field = item.Fields[key];

                if (field != null)
                {
                    yield return new DataField(field.Key, GetFieldValue(field));
                }
            }
        }

        private string GetFieldValue(Field field)
        {
            if (field == null)
            {
                return string.Empty;
            }

            var value = field.Value;

            return EncodeValue(value);
        }

        private string EncodeValue(string value)
        {
            var encodeValue = Json.Encode(value);

            encodeValue = encodeValue.Substring(1, encodeValue.Length - 2);

            return encodeValue;
        }
    }
}