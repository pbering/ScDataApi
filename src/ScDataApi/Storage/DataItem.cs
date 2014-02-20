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

        public DataItem(Item item, string fields)
        {
            var dataFields = new List<DataField>();

            dataFields.AddRange(GetSpecificFields(item, fields));

            _fields = dataFields.Distinct();
        }

        public virtual IEnumerable<DataField> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        protected IEnumerable<DataField> GetSpecificFields(Item item, string fields)
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
                    yield return new DataField(field.Key, field.ID.ToString(), GetFieldValue(field));
                }
            }
        }

        protected string GetFieldValue(Field field)
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