namespace ScDataApi.Storage
{
    public class DataField
    {
        public DataField(string key, string value)
        {
            Key = key.ToLowerInvariant();
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }

        protected bool Equals(DataField other)
        {
            return string.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((DataField)obj);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}