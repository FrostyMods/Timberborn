using HarmonyLib;
using Timberborn.Persistence;
using Timberborn.WorkSystem;

namespace FrostyMods.ShamingWheel
{
    /// <summary>
    /// Patch Worker's Save method so it saves the WorkerType with the entity
    /// </summary>
    [HarmonyPatch(typeof(Worker), nameof(Worker.Save))]
    public static class WorkerSavePatch {
        private static readonly PropertyKey<string> WorkerTypeKey = new("WorkerType");

        [HarmonyPostfix]
        public static void Postfix(IEntitySaver entitySaver, ref ComponentKey ___WorkerKey, ref string ____workerType) {
            IObjectSaver component = entitySaver.GetComponent(___WorkerKey);
            component.Set(WorkerTypeKey, ____workerType);
        }
    }

    /// <summary>
    /// Patch Worker's Load method so it loads the saved WorkerType.
    /// If the mod is removed or disabled, it will simply ignore value
    /// </summary>
    [HarmonyPatch(typeof(Worker), nameof(Worker.Load))]
    public static class WorkerLoadPatch {
        private static readonly PropertyKey<string> WorkerTypeKey = new("WorkerType");

        [HarmonyPostfix]
        public static void Postfix(IEntityLoader entityLoader, ref ComponentKey ___WorkerKey, ref string ____workerType) {
            IObjectLoader component = entityLoader.GetComponent(___WorkerKey);
            if (component.Has(WorkerTypeKey)) {
                ____workerType = component.Get(WorkerTypeKey);
            }
        }
    }

}
