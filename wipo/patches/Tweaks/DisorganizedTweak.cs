using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultPartyImpairmentModel), nameof(DefaultPartyImpairmentModel.CanGetDisorganized))]
    internal class DisorganizedTweak
    {
        [HarmonyPostfix]
        protected static void Postfix(ref bool __result, PartyBase party)
        {
            __result = (party.IsActive && party.IsMobile && party.MobileParty.MemberRoster.TotalManCount >= 50);
        }
    }
}