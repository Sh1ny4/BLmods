using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(SPInventoryVM), "IsSearchAvailable", MethodType.Getter)]
    internal class UIsearchGetterPatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
