using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CppPlayerVolume
{
    public static class MetadataSerializer
    {
        public static Dictionary<string, int> volumes = new Dictionary<string, int>();

        public static void Deserialize()
        {
            volumes.Clear();
            // Read from the metadata file
            string[] lines = File.ReadAllLines(Paths.ConfigPath + "/" + PluginInfo.Name + ".dat");
            foreach (string line in lines)
            {
                if (line == "" || line.TrimStart().StartsWith('#')) continue;
                string[] split = line.Split(':');
                volumes.Add(split[0], int.Parse(split[1]));
            }
        }

        public static void Serialize()
        {
            // Write to the metadata file
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in volumes)
            {
                sb.AppendLine($"{kvp.Key}:{kvp.Value}");
            }
            File.WriteAllText(Paths.ConfigPath + "/" + PluginInfo.Name + ".dat", sb.ToString());
        }

    }
}
