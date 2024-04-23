using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace wipo.patches.EliteInCastle
{
    [HarmonyPatch(typeof(DefaultPartyTrainingModel), nameof(DefaultPartyTrainingModel.GetEffectiveDailyExperience))]
    internal class GetEffectiveDailyExperience
    {
        [HarmonyPostfix]
        static void postfix(ref ExplainedNumber __result, MobileParty mobileParty, TroopRosterElement troop)
        {
            Settlement currentSettlement = mobileParty.CurrentSettlement;
            if (mobileParty.IsGarrison && currentSettlement.IsCastle)
            {
                    __result.Add((float)20, null, null);
            }
            if (mobileParty.IsGarrison && currentSettlement.IsTown && !currentSettlement.IsCastle)
            {
                __result.Add((float)5, null, null);
            }
        }
    }
}
