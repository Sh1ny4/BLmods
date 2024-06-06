using Helpers;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.PerksPatch
{
    [HarmonyPatch(typeof(DefaultPartyMoraleModel), nameof(DefaultPartyMoraleModel.GetEffectivePartyMorale))]
    internal class GetPartySizeMoraleEffectPatch : DefaultPartyMoraleModel
    {
        [HarmonyPostfix]
        static void Postfix(ref ExplainedNumber __result, ref DefaultPartyMoraleModel __instance, MobileParty mobileParty, bool includeDescription = false)
        {
            ExplainedNumber result = new ExplainedNumber(50f, includeDescription, null);
            TextObject _recentEventsText = GameTexts.FindText("str_recent_events", null);
            TextObject _starvationMoraleText = GameTexts.FindText("str_starvation_morale", null);
            TextObject _noWageMoraleText = GameTexts.FindText("str_no_wage_morale", null);
            result.Add(mobileParty.RecentEventsMorale, _recentEventsText, null);
            GetMoraleEffectsFromSkill(mobileParty, ref result);
            if (mobileParty.IsMilitia || mobileParty.IsGarrison)
            {
                if (mobileParty.IsMilitia)
                {
                    if (mobileParty.HomeSettlement.IsStarving)
                    {
                        result.Add((float)GetStarvationMoralePenalty(mobileParty), _starvationMoraleText, null);
                    }
                }
                else if (SettlementHelper.IsGarrisonStarving(mobileParty.CurrentSettlement))
                {
                    result.Add((float)GetStarvationMoralePenalty(mobileParty), _starvationMoraleText, null);
                }
            }
            else if (mobileParty.Party.IsStarving)
            {
                result.Add((float)GetStarvationMoralePenalty(mobileParty), _starvationMoraleText, null);
            }
            if (mobileParty.HasUnpaidWages > 0f)
            {
                result.Add(mobileParty.HasUnpaidWages * (float)GetNoWageMoralePenalty(mobileParty), _noWageMoraleText, null);
            }
            GetMoraleEffectsFromPerks(mobileParty, ref result);
            CalculateFoodVarietyMoraleBonus(mobileParty, ref result);
            GetPartySizeMoraleEffect(mobileParty, ref result);
            GetForeignTroopsMoraleEffect(mobileParty, ref result);
            __result = result;
        }

        static void GetMoraleEffectsFromSkill(MobileParty party, ref ExplainedNumber bonus)
        {
            CharacterObject effectivePartyLeaderForSkill = SkillHelper.GetEffectivePartyLeaderForSkill(party.Party);
            if (effectivePartyLeaderForSkill != null && effectivePartyLeaderForSkill.GetSkillValue(DefaultSkills.Leadership) > 0)
            {
                SkillHelper.AddSkillBonusForCharacter(DefaultSkills.Leadership, DefaultSkillEffects.LeadershipMoraleBonus, effectivePartyLeaderForSkill, ref bonus, -1, true, 0);
            }
        }

        static void GetMoraleEffectsFromPerks(MobileParty party, ref ExplainedNumber bonus)
        {
            if (party.HasPerk(DefaultPerks.Crossbow.PeasantLeader, false))
            {
                float num = CalculateTroopTierRatio(party);
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

        static float CalculateTroopTierRatio(MobileParty party)
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

        static void GetPartySizeMoraleEffect(MobileParty mobileParty, ref ExplainedNumber result)
        {
            TextObject _partySizeMoraleText = new TextObject("Amount of troops");
            if (!mobileParty.IsMilitia && !mobileParty.IsVillager)
            {
                int num = mobileParty.Party.NumberOfAllMembers;
                if (num > 0)
                {
                    result.Add(-1f * num, _partySizeMoraleText, null);
                }
            }
        }

        static void GetForeignTroopsMoraleEffect(MobileParty party, ref ExplainedNumber result)
        {
            int num = 0;
            foreach (TroopRosterElement troopRosterElement in party.MemberRoster.GetTroopRoster())
            {
                if (troopRosterElement.Character.Culture != party.MapFaction.Culture)
                {

                    num += troopRosterElement.Number;
                }
            }
            result.Add(-1f * num, new TextObject("Troops from a culture you are at war with"));
        }

        static void CalculateFoodVarietyMoraleBonus(MobileParty party, ref ExplainedNumber result)
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
                    TextObject _foodBonusMoraleText = GameTexts.FindText("str_food_bonus_morale", null);
                    result.Add(num, _foodBonusMoraleText, null);
                    if (num > 0f && party.HasPerk(DefaultPerks.Steward.Gourmet, false))
                    {
                        result.Add(num, DefaultPerks.Steward.Gourmet.Name, null);
                    }
                }
            }
        }

        static int GetStarvationMoralePenalty(MobileParty party)
        {
            return -30;
        }
        static int GetNoWageMoralePenalty(MobileParty party)
        {
            return -20;
        }
    }
}
