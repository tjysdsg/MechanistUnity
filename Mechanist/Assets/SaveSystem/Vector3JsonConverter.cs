using System;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class Vector3JsonConverter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.x},{value.y},{value.z}");
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            string[] v = value.Split(',');
            return new Vector3(Int32.Parse(v[0]), Int32.Parse(v[1]), Int32.Parse(v[2]));
        }
    }
}