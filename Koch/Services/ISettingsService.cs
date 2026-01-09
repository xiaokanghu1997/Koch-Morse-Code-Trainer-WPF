using System;
using System.Collections.Generic;
using System.Text;

namespace Koch.Services
{
    public interface ISettingsService
    {
        T GetValue<T>(string key, T defalueValue);

        void SetValue<T>(string key, T value);
    }
}
