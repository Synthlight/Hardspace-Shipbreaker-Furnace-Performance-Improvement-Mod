using System.Reflection;
using BBI.Unity.Game;
using HarmonyLib;
using JetBrains.Annotations;
using Unity.Entities;

namespace FurnacePerformanceImprovements.Patches {
    [HarmonyPatch]
    [UsedImplicitly]
    public class FurnaceVolumePatch1 {
        [HarmonyTargetMethod]
        [UsedImplicitly]
        public static MethodBase TargetMethod() {
            return typeof(FurnaceVolume).GetMethod(nameof(FurnaceVolume.HandlePositiveSalvage), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool Prefix(ref FurnaceVolume __instance, ref EntityCommandBuffer commandBuffer, ref EntityManager entityManager, ref Entity entity, ref StructurePart part) {
            InvokeDestroy(__instance, entityManager, entity, part);
            SalvageableUtility.MarkAsSalvaged(commandBuffer, entityManager, entity, false, SalvagedBy.Furnace);
            return false;
        }

        public static void InvokeDestroy(FurnaceVolume __instance, EntityManager entityManager, Entity entity, StructurePart part) {
            var destroyPartResult = __instance.DestroyPart(Main.Instance.MainSettings.SalvageSettings.ObjectDestructionDelay, part, entityManager, entity);
            __instance.StartCoroutine(destroyPartResult);
        }
    }

    [HarmonyPatch]
    [UsedImplicitly]
    public class FurnaceVolumePatch2 {
        [HarmonyTargetMethod]
        [UsedImplicitly]
        public static MethodBase TargetMethod() {
            return typeof(FurnaceVolume).GetMethod(nameof(FurnaceVolume.HandleNegativeSalvage), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool Prefix(ref FurnaceVolume __instance, ref EntityCommandBuffer commandBuffer, ref EntityManager entityManager, ref Entity entity, ref StructurePart part) {
            FurnaceVolumePatch1.InvokeDestroy(__instance, entityManager, entity, part);
            SalvageableUtility.MarkAsDestroyed(commandBuffer, entityManager, entity, SalvagedBy.Furnace);
            return false;
        }
    }
}