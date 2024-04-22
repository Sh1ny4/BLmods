using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.Tweaks
{
    [HarmonyPatch(typeof(DefaultPartySizeLimitModel), "CalculateBaseMemberSize")]
    internal class CalculateBaseMemberSizePatch : DefaultPartySizeLimitModel
    {
        [HarmonyPrefix]
        static bool Prefix(ref CalculateBaseMemberSizePatch __instance, Hero partyLeader, IFaction partyMapFaction, Clan actualClan, ref ExplainedNumber result)
        {
            if (partyMapFaction != null && partyMapFaction.IsKingdomFaction && partyLeader.MapFaction.Leader == partyLeader)
            {
                result.Add(40f, __instance._factionLeaderText, null);
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
                result.Add((float)num * DefaultPerks.Leadership.UltimateLeader.PrimaryBonus, __instance._leadershipPerkUltimateLeaderBonusText, null);
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
                result.Add(40f, DefaultPolicies.NobleRetinues.Name, null);
                if (partyLeader.Clan.Tier >= 5 && partyMapFaction.IsKingdomFaction && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.NobleRetinues))
                {
                    result.Add(40f, DefaultPolicies.NobleRetinues.Name, null);
                }
                if (partyMapFaction.IsKingdomFaction && partyMapFaction.Leader == partyLeader && ((Kingdom)partyMapFaction).ActivePolicies.Contains(DefaultPolicies.RoyalGuard))
                {
                    result.Add(60f, DefaultPolicies.RoyalGuard.Name, null);
                }
                return false;
            }
            return false;
        }

        private readonly TextObject _factionLeaderText = GameTexts.FindText("str_faction_leader_bonus", null);
        private readonly TextObject _leadershipPerkUltimateLeaderBonusText = GameTexts.FindText("str_leadership_perk_bonus", null);
    }
}
