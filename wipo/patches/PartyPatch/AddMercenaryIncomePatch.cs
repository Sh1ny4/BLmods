using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.Localization;

namespace wipo.patches.CostPatch
{
    [HarmonyPatch(typeof(DefaultClanFinanceModel), "AddMercenaryIncome")]
    internal class AddMercenaryIncomePatch : DefaultClanFinanceModel
    {
        [HarmonyPrefix]
        static bool Prefix(Clan clan, ref ExplainedNumber goldChange, bool applyWithdrawals)
        {
            if (clan.IsUnderMercenaryService && clan.Leader != null && clan.Kingdom != null)
            {
                int value = 0;
                foreach (WarPartyComponent warPartyComponent in clan.WarPartyComponents)
                {
                    value += (int)(warPartyComponent.Party.MemberRoster.TotalManCount * 5 * clan.Tier);
                }
                if (applyWithdrawals)
                {
                    clan.Kingdom.KingdomBudgetWallet -= value;
                }
                goldChange.Add((float)value, _mercenaryStr, null);
            }
            return false;
        }

        static readonly TextObject _mercenaryStr = new TextObject("{=qcaaJLhx}Mercenary Contract", null);
    }
}
