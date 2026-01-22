using BepInEx;
using DaggerfallWorkshop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFUVR
{
    internal class Settings
    {
        public double heightOffset = -0.989447891712189;
        public int headsetRefreshRate = 90;
        public string headsetType = "Oculus/Meta";
        public string sheathOffset = "-0.1195223,0.07885736,-0.02833132";
        public int connectedJoysticks = 0;
        public bool showStartMenu = true;
        public string dominantHand = "right";
        public string turnStyle = "snap";
        public bool toggleRun = true;

        public static Settings LoadFromFile()
        {
            try
            {
                string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
                if (!File.Exists(filePath))
                {
                    Plugin.LoggerInstance.LogWarning("Settings file not found, creating default settings file and setting default values.");
                    var defaultSettings = new Settings();
                    defaultSettings.SaveToFile();

                    return defaultSettings;
                }

                string[] lines = File.ReadAllLines(filePath);

                Settings settings = new Settings();
                settings.heightOffset = double.Parse(lines[0].Trim());
                settings.headsetRefreshRate = int.Parse(lines[1].Trim());
                settings.headsetType = lines[2].Trim();
                settings.sheathOffset = lines[3].Trim();
                settings.connectedJoysticks = int.Parse(lines[4].Trim());
                settings.showStartMenu = bool.Parse(lines[5].Trim());
                settings.dominantHand = lines[6].Trim();
                settings.turnStyle = lines[7].Trim();
                if (lines.Length > 8)
                    settings.toggleRun = bool.Parse(lines[8].Trim());

                return settings;
            }
            catch (Exception ex)
            {
                Plugin.LoggerInstance.LogError($"Failed to load Settings.txt: {ex.Message}");
                return new Settings();
            }
        }

        public bool SaveToFile()
        {
            string[] lines = new string[]
            {
                heightOffset.ToString(),
                headsetRefreshRate.ToString(),
                headsetType,
                sheathOffset,
                connectedJoysticks.ToString(),
                showStartMenu.ToString(),
                dominantHand,
                turnStyle,
                toggleRun.ToString()
            };

            string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
            try
            {
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Plugin.LoggerInstance.LogError($"Failed to save Settings.txt: {ex.Message}");
                return false;
            }

            return true;
        }
    }
}
