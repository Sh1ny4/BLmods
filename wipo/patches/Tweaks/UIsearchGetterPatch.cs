using System;
using HarmonyLib;
using TaleWorlds;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.ViewModelCollection;
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
