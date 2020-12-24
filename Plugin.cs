using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;

namespace FurnacePerformanceImprovements {
    [BepInPlugin(UUID, "Furnace Performance Improvements", "1.0.0.0")]
    [UsedImplicitly]
    public class Plugin : BaseUnityPlugin {
        private const  string          UUID = "com.FurnacePerformanceImprovements";
        private static ManualLogSource logSource;

        [UsedImplicitly]
        public void Awake() {
            logSource = Logger;

            Log(LogLevel.Info, "FurnacePerformanceImprovements loaded.");

            var harmony = new Harmony(UUID);
            harmony.PatchAll();

            Log(LogLevel.Info, "FurnacePerformanceImprovements patched.");

            foreach (var patchedMethod in harmony.GetPatchedMethods()) {
                Log(LogLevel.Info, $"Patched: {patchedMethod.DeclaringType?.FullName}:{patchedMethod}");
            }
        }

        public static void Log(LogLevel level, string msg) {
            logSource.Log(level, msg);
        }
    }
}