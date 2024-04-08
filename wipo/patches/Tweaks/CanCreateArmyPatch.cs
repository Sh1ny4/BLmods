using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(ArmyManagementVM), "CanCreateArmy", MethodType.Getter)]
    internal class CanCreateArmyPatch
    {
        [HarmonyPrefix]
        static bool CanCreateArmy(ref bool __result)
        {
            __result = Hero.MainHero.Clan.Tier >= 4;
            return false;
        }
    }
}
