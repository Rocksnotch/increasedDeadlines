using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Unity.Netcode;
using IncreasedDeadlines.HostDebug;

namespace increasedDeadlines
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInProcess("Lethal Company.exe")]
    public class IncreasedDeadlinesMod : BaseUnityPlugin
    {
        private ConfigEntry<string> configGreeting;
        public static ConfigEntry<int> configQuota;
        public static ConfigEntry<bool> configDebug;
        internal static IncreasedDeadlinesMod Instance;
        public static int quotaCheck = 0;
        public Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource logSrc = BepInEx.Logging.Logger.CreateLogSource("loggingSource");
        private const string modName = "Increased Deadlines";
        private const string modVersion = "1.0.0";
        private const string modGUID = "Rocksnotch.IncreasedDeadlines";

        public IncreasedDeadlinesMod()
        {
            // Constructor
            // Greeting message for plugin
            configGreeting = Config.Bind(
                "General",
                "Plugin Startup Message",
                $"Plugin {modGUID} is loaded!",
                "Message that is displayed when the plugin loads"
            );

            // Setting for quote goal to update at each time
            configQuota = Config.Bind(
                "General",
                "Quota Goal",
                900,
                "Days until quota updates by when you reach past this goal (Ex: 900 = 1 day, 1800 = 2 days, etc.)"
            );

            //Setting for host debug mode
            configDebug = Config.Bind(
                "Debug",
                "Host Debug Mode",
                false,
                "Enable this to allow debug commands to be used by the host (MAY BREAK GAME!)"
            );
        }
        private void Awake()
        {
            // Plugin startup logic
            if (Instance == null) {
                Instance = this;
            }
            //Set quotaCheck to quota value
            if (configQuota.Value > 0) {
                quotaCheck = configQuota.Value;
                Logger.LogInfo($"Quota check set to {quotaCheck}");
            }

            //Run plugin greeting in console
            if (configGreeting.Value != null) {
                Logger.LogInfo(configGreeting.Value);
            }
            harmony.PatchAll();
        }
    }
}
