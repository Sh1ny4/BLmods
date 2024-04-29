using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace wipo.patches.CostPatch
{
    [HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel), nameof(DefaultPartyTroopUpgradeModel.GetXpCostForUpgrade))]
    public class GetXpCostForUpgradePatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
        {
            int curtier = characterObject.Tier;
            int targtier = upgradeTarget.Tier;
            int num = 100 * curtier * (int)Math.Pow(2, 1 + targtier - curtier);
            if (upgradeTarget.Occupation == Occupation.Mercenary) 
            {
                num = (int)((float)num * 0.8f);
            }
            if (upgradeTarget.Occupation == Occupation.Bandit)
            {
                num = (int)((float)num * 1.5f);
            }
            if (upgradeTarget.IsMounted)
            {
                num = (int)((float)num * 1.4f);
            }
            if (upgradeTarget.IsRanged)
            {
                num = (int)((float)num * 1.2f);
            }
            __result = num;
        }
    }
}