using HarmonyLib;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultPartyImpairmentModel), nameof(DefaultPartyImpairmentModel.CanGetDisorganized))]
    internal class CanGetDisorganizedPatch
    {
        [HarmonyPostfix]
        static void Postfix(ref bool __result, PartyBase party)
        {
            __result = (party.IsActive && party.IsMobile && party.MobileParty.MemberRoster.TotalManCount >= 50);
        }
    }
}