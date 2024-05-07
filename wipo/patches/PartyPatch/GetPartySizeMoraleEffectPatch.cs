using Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace wipo.patches.PartyPatch
{
    internal class GetPartySizeMoraleEffectPatch : DefaultPartyMoraleModel
    {
        public override ExplainedNumber GetEffectivePartyMorale(MobileParty mobileParty, bool includeDescription = false)
        {
            ExplainedNumber result = new ExplainedNumber(50f, includeDescription, null);
            result.Add(mobileParty.RecentEventsMorale, this._recentEventsText, null);
            this.GetMoraleEffectsFromSkill(mobileParty, ref result);
            if (mobileParty.IsMilitia || mobileParty.IsGarrison)
            {
                if (mobileParty.IsMilitia)
                {
                    if (mobileParty.HomeSettlement.IsStarving)
                    {
                        result.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
                    }
                }
                else if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
                {
                    result.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
                }
            }
            else if (mobileParty.Party.IsStarving)
            {
                result.Add((float)this.GetStarvationMoralePenalty(mobileParty), this._starvationMoraleText, null);
            }
            if (mobileParty.HasUnpaidWages > 0f)
            {
                result.Add(mobileParty.HasUnpaidWages * (float)this.GetNoWageMoralePenalty(mobileParty), this._noWageMoraleText, null);
            }
            this.GetMoraleEffectsFromPerks(mobileParty, ref result);
            this.CalculateFoodVarietyMoraleBonus(mobileParty, ref result);
            this.GetPartySizeMoraleEffect(mobileParty, ref result);
            this.GetForeignTroopsMoraleEffect(mobileParty, ref result);
            return result;
        }

        public void GetMoraleEffectsFromSkill(MobileParty party, ref ExplainedNumber bonus)
        {
            CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
            if (effectivePartyLeaderForSkill != null && effectivePartyLeaderForSkill.GetSkillValue(DefaultSkills.Leadership) > 0)
            {
                SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Leadership, DefaultSkillEffects.LeadershipMoraleBonus, effectivePartyLeaderForSkill, ref bonus, -1, true, 0);
            }
        }

        public void GetMoraleEffectsFromPerks(MobileParty party, ref ExplainedNumber bonus)
        {
            if (party.HasPerk(DefaultPerks.Crossbow.PeasantLeader, false))
            {
                float num = this.CalculateTroopTierRatio(party);
                bonus.AddFactor(DefaultPerks.Crossbow.PeasantLeader.PrimaryBonus * num, DefaultPerks.Crossbow.PeasantLeader.Name);
            }
            Settlement currentSettlement = party.CurrentSettlement;
            if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null && party.HasPerk(DefaultPerks.Charm.SelfPromoter, true))
            {
                bonus.Add(DefaultPerks.Charm.SelfPromoter.SecondaryBonus, DefaultPerks.Charm.SelfPromoter.Name, null);
            }
            if (party.HasPerk(DefaultPerks.Steward.Logistician, false))
            {
                int num2 = 0;
                for (int i = 0; i < party.MemberRoster.Count; i++)
                {
                    TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
                    if (elementCopyAtIndex.Character.IsMounted)
                    {
                        num2 += elementCopyAtIndex.Number;
                    }
                }
                if (party.Party.NumberOfMounts > party.MemberRoster.TotalManCount - num2)
                {
                    bonus.Add(DefaultPerks.Steward.Logistician.PrimaryBonus, DefaultPerks.Steward.Logistician.Name, null);
                }
            }
        }

        public float CalculateTroopTierRatio(MobileParty party)
        {
            int totalManCount = party.MemberRoster.TotalManCount;
            float num = 0f;
            foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
            {
                if (troopRosterElement.Character.Tier <= 3)
                {
                    num += (float)troopRosterElement.Number;
                }
            }
            return num / (float)totalManCount;
        }

        public void GetPartySizeMoraleEffect(MobileParty mobileParty, ref ExplainedNumber result)
        {
            if (!mobileParty.IsMilitia && !mobileParty.IsVillager)
            {
                int num = mobileParty.Party.NumberOfAllMembers;
                if (num > 0)
                {
                    result.Add(-1f * num , this._partySizeMoraleText, null);
                }
            }
        }

        public void GetForeignTroopsMoraleEffect(MobileParty party, ref ExplainedNumber result)
        {
            int num = 0;
            foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
            {
                if (troopRosterElement.Character.Culture != party.ActualClan.Kingdom.Culture)
                {
                    num ++;
                }
            }
            result.Add(-1f * num, new TextObject("Troops from a cultre you're at war with"));
        }

        public void CalculateFoodVarietyMoraleBonus(MobileParty party, ref ExplainedNumber result)
        {
            if (!party.Party.IsStarving)
            {
                float num;
                num = party.ItemRoster.FoodVariety - 3;
                if (num < 0f && party.LeaderHero != null && party.LeaderHero.GetPerkValue(DefaultPerks.Steward.WarriorsDiet))
                {
                    num = 0f;
                }
                if (num != 0f)
                {
                    result.Add(num, this._foodBonusMoraleText, null);
                    if (num > 0f && party.HasPerk(DefaultPerks.Steward.Gourmet, false))
                    {
                        result.Add(num, DefaultPerks.Steward.Gourmet.Name, null);
                    }
                }
            }
        }

        public int GetStarvationMoralePenalty(MobileParty party)
        {
            return -30;
        }
        public int GetNoWageMoralePenalty(MobileParty party)
        {
            return -20;
        }
        public override float GetStandardBaseMorale(PartyBase party)
        {
            return 50f;
        }
        public override float GetVictoryMoraleChange(PartyBase party)
        {
            return 20f;
        }
        public override float GetDefeatMoraleChange(PartyBase party)
        {
            return -20f;
        }
        public const float BaseMoraleValue = 50f;
        public readonly TextObject _recentEventsText = GameTexts.FindText("str_recent_events", null);
        public readonly TextObject _starvationMoraleText = GameTexts.FindText("str_starvation_morale", null);
        public readonly TextObject _noWageMoraleText = GameTexts.FindText("str_no_wage_morale", null);
        public readonly TextObject _foodBonusMoraleText = GameTexts.FindText("str_food_bonus_morale", null);
        public readonly TextObject _partySizeMoraleText = GameTexts.FindText("str_party_size_morale", null);
    }
}
