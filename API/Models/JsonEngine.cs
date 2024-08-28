using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Shared.Model
{
    class JsonEngine
    {
        public static T Deserialize<T>(string json)
        {
            using (MemoryStream _Stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var _Serializer = new DataContractJsonSerializer(typeof(T));
                return (T)_Serializer.ReadObject(_Stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(instance.GetType());
                _Serializer.WriteObject(_Stream, instance);
                _Stream.Position = 0;
                return (new StreamReader(_Stream)).ReadToEnd();
            }
        }
    }
}
