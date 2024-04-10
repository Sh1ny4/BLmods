using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace wipo.patches.CostPatch
{
    [HarmonyPatch(typeof(DefaultPartyTroopUpgradeModel), nameof(DefaultPartyTroopUpgradeModel.GetXpCostForUpgrade))]
    public class XpUpgradeCostPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref int __result, PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget)
        {
            int tier = upgradeTarget.Tier;
            int num = 0;
            for (int i = characterObject.Tier + 1; i <= tier; i++)
            {
                if (i <= 1)
                {
                    num += 100;
                }
                else if (i == 2)
                {
                    num += 200;
                }
                else if (i == 3)
                {
                    num += 400;
                }
                else if (i == 4)
                {
                    num += 800;
                }
                else if (i == 5)
                {
                    num += 1600;
                }
                else if (i == 6)
                {
                    num += 2400;
                }
                else if (i == 7)
                {
                    num += 3600;
                }
                else
                {
                    int num2 = upgradeTarget.Level + 4;
                    num += (int)(1.333f * (float)num2 * (float)num2);
                }
            }
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