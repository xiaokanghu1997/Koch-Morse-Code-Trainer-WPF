using Windows.Storage;

namespace Koch.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDataContainer _localSettings;

        public SettingsService()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            if (_localSettings.Values.ContainsKey(key))
            {
                var value = _localSettings.Values[key];
                if (value is T typedValue)
                {
                    return typedValue;
                }
                // 处理枚举类型
                if (typeof(T).IsEnum && value is int intValue)
                {
                    return (T)(object)intValue;
                }
            }
            return defaultValue;
        }

        public void SetValue<T>(string key, T value)
        {
            // 处理枚举类型，转换为 int 存储
            if (typeof(T).IsEnum)
            {
                _localSettings.Values[key] = (int)(object)value!;
            }
            else
            {
                _localSettings.Values[key] = value;
            }
        }
    }
}
