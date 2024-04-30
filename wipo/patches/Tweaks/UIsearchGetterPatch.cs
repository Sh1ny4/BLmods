using HarmonyLib;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(SPInventoryVM), "IsSearchAvailable", MethodType.Getter)]
    internal class UIsearchGetterPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref bool __result)
        {
            //always enable the search bar in inventoiry
            __result = true;
        }
    }
}
