using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem;

namespace wipo.patches.EliteInCastle
{
    [HarmonyPatch(typeof(RecruitmentCampaignBehavior), "UpdateVolunteersOfNotablesInSettlement")]
    internal class AiHourlyTickPatch : AiVisitSettlementBehavior
    {
        [HarmonyPrefix]
        static bool Prefix(ref AiVisitSettlementBehavior __instance, MobileParty mobileParty, PartyThinkParams p)
        {
            return false;
        }
    }
}
