namespace ConfigurationServer
{
    public abstract class ConfigRepository
    {
        public abstract void Add<T>(string key, T value);

        public abstract T Get<T>(string key);

        public object Get(string key)
        {
            return Get<object>(key);
        }

        public T GetOrCreate<T>(string key, T defaultValue)
        {
            var returnValue = Get<T>(key);
            if (returnValue == null)
            {
                Add(key, defaultValue);
                returnValue = defaultValue;
            }
            return returnValue;
        }
    }
}
