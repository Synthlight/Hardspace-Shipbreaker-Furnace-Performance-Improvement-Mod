using System.Collections;
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
            return typeof(FurnaceVolume).GetMethod("HandlePositiveSalvage", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool Prefix(ref FurnaceVolume __instance, ref EntityCommandBuffer commandBuffer, ref EntityManager entityManager, ref Entity entity, ref StructurePart part) {
            InvokeDestroy(__instance, entityManager, entity, part);
            SalvageableUtility.MarkAsSalvaged(commandBuffer, entityManager, entity, false, SalvagedBy.Furnace);
            return false;
        }

        private static MethodInfo destroyPartMethod;

        public static void InvokeDestroy(FurnaceVolume __instance, EntityManager entityManager, Entity entity, StructurePart part) {
            if (destroyPartMethod == null) {
                destroyPartMethod = typeof(FurnaceVolume).GetMethod("DestroyPart", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            }

            var destroyPartResult = (IEnumerator) destroyPartMethod.Invoke(__instance, new object[] {Main.Instance.MainSettings.SalvageSettings.ObjectDestructionDelay, part, entityManager, entity});
            __instance.StartCoroutine(destroyPartResult);
        }
    }

    [HarmonyPatch]
    [UsedImplicitly]
    public class FurnaceVolumePatch2 {
        [HarmonyTargetMethod]
        [UsedImplicitly]
        public static MethodBase TargetMethod() {
            return typeof(FurnaceVolume).GetMethod("HandleNegativeSalvage", BindingFlags.NonPublic | BindingFlags.Instance);
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