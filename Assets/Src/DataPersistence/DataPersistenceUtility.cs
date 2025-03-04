using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Src.DataPersistence
{
    public static class DataPersistenceUtility
    {
        public static void SaveData<T>(T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            File.WriteAllText($"{Application.persistentDataPath}/DATA.dat", json);
        }

        public static T LoadData<T>() where T : new()
        {
            var file = $"{Application.persistentDataPath}/DATA.dat";
            
            return !ValidateFile(file) ? new T() : JsonConvert.DeserializeObject<T>(File.ReadAllText(file));
        }

        private static bool ValidateFile(string file)
        {
            if (File.Exists(file)) return true;
            
            Debug.Log("File Not Found");
            return false;
        }
    }
}