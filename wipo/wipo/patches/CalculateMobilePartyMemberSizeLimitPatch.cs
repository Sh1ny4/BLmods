using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches
{
    [HarmonyPatch(typeof(DefaultPartySizeLimitModel), "CalculateMobilePartyMemberSizeLimit")]
    internal class CalculateMobilePartyMemberSizeLimitPatch : DefaultPartySizeLimitModel
    {
        static void Postfix(ref ExplainedNumber __result, ref DefaultPartySizeLimitModel __instance, MobileParty party, bool includeDescriptions = false)
        {

            TextObject _leadershipText = new TextObject("{=!}Leadership level", null);
            TextObject _randomSizeBonusTemporary = new TextObject("{=hynFV8jC}Extra size bonus (Perk-like Effect)", null);
            TextObject _baseSizeText = GameTexts.FindText("str_base_size", null);

            ExplainedNumber result = new ExplainedNumber(20f, includeDescriptions, _baseSizeText);
            if (party.LeaderHero != null && party.LeaderHero.Clan != null && !party.IsCaravan)
            {
                CalculateBaseMemberSize(party.LeaderHero, party.MapFaction, party.ActualClan, ref result);
                result.Add((float)(party.LeaderHero.GetSkillValue(DefaultSkills.Leadership)), _leadershipText, null);
            }
            else if (party.IsCaravan)
            {
                if (party.Party.Owner == Hero.MainHero)
                {
                    result.Add(10f, _randomSizeBonusTemporary, null);
                }
                else
                {
                    Hero owner = party.Party.Owner;
                    if (owner != null && owner.IsNotable)
                    {
                        result.Add((float)(10 * ((party.Party.Owner.Power < 100f) ? 1 : ((party.Party.Owner.Power < 200f) ? 2 : 3))), _randomSizeBonusTemporary, null);
                    }
                }
            }
            else if (party.IsVillager)
            {
                result.Add(40f, _randomSizeBonusTemporary, null);
            }
            __result = result;
        }


        static void CalculateBaseMemberSize(Hero partyLeader, IFaction partyMapFaction, Clan actualClan, ref ExplainedNumber result)
        {

            TextObject _factionLeaderText = GameTexts.FindText("str_faction_leader_bonus", null);
            TextObject _leadershipPerkUltimateLeaderBonusText = GameTexts.FindText("str_leadership_perk_bonus", null);
            if (partyMapFaction != null && partyMapFaction.IsKingdomFaction && partyLeader.MapFaction.Leader == partyLeader)
            {
                result.Add(20f, _factionLeaderText, null);
            }
            if (partyLeader.GetSkillValue(DefaultSkills.Leadership) > Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus && partyLeader.GetPerkValue(DefaultPerks.Leadership.UltimateLeader))
            {
                int num = partyLeader.GetSkillValue(DefaultSkills.Leadership) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
                result.Add((float)num * DefaultPerks.Leadership.UltimateLeader.PrimaryBonus, _leadershipPerkUltimateLeaderBonusText, null);
            }
            if (actualClan != null)
            {
                Hero leader = actualClan.Leader;
                bool? flag = (leader != null) ? new bool?(leader.GetPerkValue(DefaultPerks.Leadership.LeaderOfMasses)) : null;
                bool flag2 = true;
                if (flag.GetValueOrDefault() == flag2 & flag != null)
                {
                    int num2 = 0;
                    using (List<Settlement>.Enumerator enumerator = actualClan.Settlements.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.IsTown)
                            {
                                num2++;
                            }
                        }
                    }
                    float num3 = (float)num2 * DefaultPerks.Leadership.LeaderOfMasses.PrimaryBonus;
                    if (num3 > 0f)
                    {
                        result.Add(num3, DefaultPerks.Leadership.LeaderOfMasses.Name, null);
                    }
                }
            }
            if (partyLeader.Clan.Leader == partyLeader)
            {
                if (partyLeader.Clan.Tier >= 5 && partyMapFaction.IsKingdomFaction && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.NobleRetinues))
                {
                    result.Add(40f, DefaultPolicies.NobleRetinues.Name, null);
                }
                if (partyMapFaction.IsKingdomFaction && partyMapFaction.Leader == partyLeader && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalGuard))
                {
                    result.Add(60f, DefaultPolicies.RoyalGuard.Name, null);
                }
            }
        }
    }
}
