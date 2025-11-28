using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace wipo.patches
{
    [HarmonyPatch(typeof(SPInventoryVM), nameof(SPInventoryVM.IsSearchAvailable), MethodType.Getter)]
    internal class IsSearchAvailablePatch
    {
        [HarmonyPostfix]
        static void Postfix(ref bool __result)
        {
            //always enable the search bar in inventoiry
            __result = true;
        }
    }
}
