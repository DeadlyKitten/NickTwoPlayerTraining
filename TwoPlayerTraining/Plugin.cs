using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using BepInEx.Configuration;
using MenuCore;
using System.Collections.Generic;

namespace TwoPlayerTraining
{
    [BepInPlugin("com.steven.nasb.2playertraining", "Two Player Training", "1.0.0")]
    [BepInDependency("com.pink.nasb.menucore", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;

        internal static ConfigEntry<bool> Disabled;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            Disabled = Config.Bind("Settings", "Disabled", false, "Disable the plugin.");

            var harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            #region menu
            CustomOptionsMenuHandler.InitiateMenu(() =>
            {
                var menu = new CustomOptionsMenu("Two-Player Training");

                menu.CreateSelector(
                    "disabled",
                    "Disable",
                    new List<string> { "Yes", "No" },
                    Disabled.Value ? 0 : 1,
                    SelectorType.RightSide,
                    (_, value) =>
                    {
                        Disabled.Value = value == 0;
                        Config.Save();
                    });

                CustomOptionsMenuHandler.CreateMenuTab(menu);
            });
            #endregion
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        internal static void LogError(Exception ex) => Instance.Log($"{ex.Message}\n{ex.StackTrace}", LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
