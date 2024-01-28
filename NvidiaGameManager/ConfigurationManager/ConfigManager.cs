using Avalonia.Controls.Shapes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvidiaGameManager.ConfigurationManager
{
    class ConfigManager
    {
        public static string DefaultPath = "config.json";

        public static DisplayConfig[] LoadConfig(string path)
        {
            if (!File.Exists(path))
            {
                return new DisplayConfig[] { };
            }

            return JsonConvert.DeserializeObject<DisplayConfig[]>(File.ReadAllText(path)) ?? new DisplayConfig[] { };
        }

        public static void SaveConfig(DisplayConfig[] configs)
        {
            string json =
                JsonConvert.SerializeObject(configs, Formatting.Indented);
            File.WriteAllText(DefaultPath, json);
        }
    }
}