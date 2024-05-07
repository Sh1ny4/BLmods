using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.CostPatch
{
    [HarmonyPatch(typeof(DefaultPartySizeLimitModel), "CalculateMobilePartyMemberSizeLimit")]
    internal class CalculateMobilePartyMemberSizeLimitPatch : DefaultPartySizeLimitModel
    {
        static void Postfix(ref ExplainedNumber __result, ref CalculateMobilePartyMemberSizeLimitPatch __instance, MobileParty party, bool includeDescriptions = false)
        {
            ExplainedNumber result = new ExplainedNumber(20f, includeDescriptions, __instance._baseSizeText);
            if (party.LeaderHero != null && party.LeaderHero.Clan != null && !party.IsCaravan)
            {
                __instance.CalculateBaseMemberSize(party.LeaderHero, party.MapFaction, party.ActualClan, ref result);
                result.Add((float)(party.LeaderHero.GetSkillValue(DefaultSkills.Leadership)), __instance._leadershipText, null);
            }
            else if (party.IsCaravan)
            {
                if (party.Party.Owner == Hero.MainHero)
                {
                    result.Add(10f, __instance._randomSizeBonusTemporary, null);
                }
                else
                {
                    Hero owner = party.Party.Owner;
                    if (owner != null && owner.IsNotable)
                    {
                        result.Add((float)(10 * ((party.Party.Owner.Power < 100f) ? 1 : ((party.Party.Owner.Power < 200f) ? 2 : 3))), __instance._randomSizeBonusTemporary, null);
                    }
                }
            }
            else if (party.IsVillager)
            {
                result.Add(40f, __instance._randomSizeBonusTemporary, null);
            }
            __result = result;
        }

        public TextObject _leadershipText = new TextObject("{=!}Leadership level", null);
        public TextObject _randomSizeBonusTemporary = new TextObject("{=hynFV8jC}Extra size bonus (Perk-like Effect)", null);
        public TextObject _baseSizeText = GameTexts.FindText("str_base_size", null);
        private readonly TextObject _factionLeaderText = GameTexts.FindText("str_faction_leader_bonus", null);
        private readonly TextObject _leadershipPerkUltimateLeaderBonusText = GameTexts.FindText("str_leadership_perk_bonus", null);


        private void CalculateBaseMemberSize(Hero partyLeader, IFaction partyMapFaction, Clan actualClan, ref ExplainedNumber result)
        {
            if (partyMapFaction != null && partyMapFaction.IsKingdomFaction && partyLeader.MapFaction.Leader == partyLeader)
            {
                result.Add(20f, this._factionLeaderText, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.OneHanded.Prestige))
            {
                result.Add(DefaultPerks.OneHanded.Prestige.SecondaryBonus, DefaultPerks.OneHanded.Prestige.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.TwoHanded.Hope))
            {
                result.Add(DefaultPerks.TwoHanded.Hope.SecondaryBonus, DefaultPerks.TwoHanded.Hope.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Athletics.ImposingStature))
            {
                result.Add(DefaultPerks.Athletics.ImposingStature.SecondaryBonus, DefaultPerks.Athletics.ImposingStature.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Bow.MerryMen))
            {
                result.Add(DefaultPerks.Bow.MerryMen.PrimaryBonus, DefaultPerks.Bow.MerryMen.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Tactics.HordeLeader))
            {
                result.Add(DefaultPerks.Tactics.HordeLeader.PrimaryBonus, DefaultPerks.Tactics.HordeLeader.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Scouting.MountedScouts))
            {
                result.Add(DefaultPerks.Scouting.MountedScouts.SecondaryBonus, DefaultPerks.Scouting.MountedScouts.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Leadership.Authority))
            {
                result.Add(DefaultPerks.Leadership.Authority.SecondaryBonus, DefaultPerks.Leadership.Authority.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Leadership.UpliftingSpirit))
            {
                result.Add(DefaultPerks.Leadership.UpliftingSpirit.SecondaryBonus, DefaultPerks.Leadership.UpliftingSpirit.Name, null);
            }
            if (partyLeader.GetPerkValue(DefaultPerks.Leadership.TalentMagnet))
            {
                result.Add(DefaultPerks.Leadership.TalentMagnet.PrimaryBonus, DefaultPerks.Leadership.TalentMagnet.Name, null);
            }
            if (partyLeader.GetSkillValue(DefaultSkills.Leadership) > Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus && partyLeader.GetPerkValue(DefaultPerks.Leadership.UltimateLeader))
            {
                int num = partyLeader.GetSkillValue(DefaultSkills.Leadership) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
                result.Add((float)num * DefaultPerks.Leadership.UltimateLeader.PrimaryBonus, this._leadershipPerkUltimateLeaderBonusText, null);
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
