using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;

namespace wipo.patches
{

    [HarmonyPatch(typeof(AiVisitSettlementBehavior), "AiHourlyTick")]
    public class Testpatch
    {
        [HarmonyPrefix]
        static bool Prefix(ref SortedList __result, AiVisitSettlementBehavior __instance, IDisbandPartyCampaignBehavior ____disbandPartyCampaignBehavior, MobileParty mobileParty, PartyThinkParams p)
        {
            Settlement currentSettlement = mobileParty.CurrentSettlement;
            if (((currentSettlement != null) ? currentSettlement.SiegeEvent : null) != null)
            {
                return false;
            }
            Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty);
            if (mobileParty.IsBandit)
            {
                this.CalculateVisitHideoutScoresForBanditParty(mobileParty, currentSettlementOfMobilePartyForAICalculation, p);
                return false;
            }
            IFaction mapFaction = mobileParty.MapFaction;
            if (mobileParty.IsMilitia || mobileParty.IsCaravan || mobileParty.IsVillager || (!mapFaction.IsMinorFaction && !mapFaction.IsKingdomFaction && (mobileParty.LeaderHero == null || !mobileParty.LeaderHero.IsLord)))
            {
                return false;
            }
            if (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty || mobileParty.Army.Cohesion < (float)mobileParty.Army.CohesionThresholdForDispersion)
            {
                Hero leaderHero = mobileParty.LeaderHero;
                ValueTuple<float, float, int, int> valueTuple = this.CalculatePartyParameters(mobileParty);
                float item = valueTuple.Item1;
                float item2 = valueTuple.Item2;
                int item3 = valueTuple.Item3;
                int item4 = valueTuple.Item4;
                float num = item2 / Math.Min(1f, Math.Max(0.1f, item));
                float num2 = (num >= 1f) ? 0.33f : ((MathF.Max(1f, MathF.Min(2f, num)) - 0.5f) / 1.5f);
                float num3 = mobileParty.Food;
                float num4 = -mobileParty.FoodChange;
                int num5 = (leaderHero != null) ? leaderHero.Gold : 0;
                if (mobileParty.Army != null && mobileParty == mobileParty.Army.LeaderParty)
                {
                    foreach (MobileParty mobileParty2 in mobileParty.Army.LeaderParty.AttachedParties)
                    {
                        num3 += mobileParty2.Food;
                        num4 += -mobileParty2.FoodChange;
                        int num6 = num5;
                        Hero leaderHero2 = mobileParty2.LeaderHero;
                        num5 = num6 + ((leaderHero2 != null) ? leaderHero2.Gold : 0);
                    }
                }
                float num7 = 1f;
                if (leaderHero != null && mobileParty.IsLordParty)
                {
                    num7 = this.CalculateSellItemScore(mobileParty);
                }
                int num8 = mobileParty.Party.PrisonerSizeLimit;
                if (mobileParty.Army != null)
                {
                    foreach (MobileParty mobileParty3 in mobileParty.Army.LeaderParty.AttachedParties)
                    {
                        num8 += mobileParty3.Party.PrisonerSizeLimit;
                    }
                }
                SortedList<ValueTuple<float, int>, Settlement> sortedList = this.FindSettlementsToVisitWithDistances(mobileParty);
                float num9 = PartyBaseHelper.FindPartySizeNormalLimit(mobileParty);
                float num10 = Campaign.MapDiagonalSquared;
                if (num3 - num4 < 0f)
                {
                    foreach (KeyValuePair<ValueTuple<float, int>, Settlement> keyValuePair in sortedList)
                    {
                        float item5 = keyValuePair.Key.Item1;
                        Settlement value = keyValuePair.Value;
                        if (item5 < 250f && item5 < num10 && (float)value.ItemRoster.TotalFood > num4 * 2f)
                        {
                            num10 = item5;
                        }
                    }
                }
                float num11 = 2000f;
                float num12 = 2000f;
                if (leaderHero != null)
                {
                    num11 = HeroHelper.StartRecruitingMoneyLimitForClanLeader(leaderHero);
                    num12 = HeroHelper.StartRecruitingMoneyLimit(leaderHero);
                }
                float num13 = Campaign.AverageDistanceBetweenTwoFortifications * 0.4f;
                float num14 = (84f + Campaign.AverageDistanceBetweenTwoFortifications * 1.5f) * 0.5f;
                float num15 = (424f + 7.57f * Campaign.AverageDistanceBetweenTwoFortifications) * 0.5f;
                foreach (KeyValuePair<ValueTuple<float, int>, Settlement> keyValuePair2 in sortedList)
                {
                    Settlement value2 = keyValuePair2.Value;
                    float item6 = keyValuePair2.Key.Item1;
                    float num16 = 1.6f;
                    if (mobileParty.IsDisbanding)
                    {
                        {
                            goto IL_37E;
                        }
                        IDisbandPartyCampaignBehavior disbandPartyCampaignBehavior = ____disbandPartyCampaignBehavior;
                        if (disbandPartyCampaignBehavior != null && disbandPartyCampaignBehavior.IsPartyWaitingForDisband(mobileParty))
                        {
                            goto IL_37E;
                        }
                        if (leaderHero == null)
                        {
                            this.AddBehaviorTupleWithScore(p, value2, this.CalculateMergeScoreForLeaderlessParty(mobileParty, value2, item6));
                        }
                        else
                        {
                            float num17 = item6;
                            if (num17 >= 250f)
                            {
                                this.AddBehaviorTupleWithScore(p, value2, 0.025f);
                                continue;
                            }
                            float num18 = num17;
                            num17 = MathF.Max(num13, num17);
                            float num19 = MathF.Max(0.1f, MathF.Min(1f, num14 / (num14 - num13 + num17)));
                            float num20 = num19;
                            if (item < 0.6f)
                            {
                                num20 = MathF.Pow(num19, MathF.Pow(0.6f / MathF.Max(0.15f, item), 0.3f));
                            }
                            int? num21 = (currentSettlementOfMobilePartyForAICalculation != null) ? new int?(currentSettlementOfMobilePartyForAICalculation.ItemRoster.TotalFood) : null;
                            int num22 = item4 / Campaign.Current.Models.MobilePartyFoodConsumptionModel.NumberOfMenOnMapToEatOneFood * 3;
                            bool flag = (num21.GetValueOrDefault() > num22 & num21 != null) || num3 > (float)(item4 / Campaign.Current.Models.MobilePartyFoodConsumptionModel.NumberOfMenOnMapToEatOneFood);
                            float num23 = (float)item3 / (float)item4;
                            float num24 = 1f + ((item4 > 0) ? (num23 * MathF.Max(0.25f, num19 * num19) * MathF.Pow((float)item3, 0.25f) * ((mobileParty.Army != null) ? 4f : 3f) * ((value2.IsFortification && flag) ? 18f : 0f)) : 0f);
                            if (mobileParty.MapEvent != null || mobileParty.SiegeEvent != null)
                            {
                                num24 = MathF.Sqrt(num24);
                            }
                            float num25 = 1f;
                            if ((value2 == currentSettlementOfMobilePartyForAICalculation && currentSettlementOfMobilePartyForAICalculation.IsFortification) || (currentSettlementOfMobilePartyForAICalculation == null && value2 == mobileParty.TargetSettlement))
                            {
                                num25 = 1.2f;
                            }
                            else if (currentSettlementOfMobilePartyForAICalculation == null && value2 == mobileParty.LastVisitedSettlement)
                            {
                                num25 = 0.8f;
                            }
                            float num26 = 0.16f;
                            float num27 = Math.Max(0f, num3) / num4;
                            if (num4 > 0f && (mobileParty.BesiegedSettlement == null || num27 <= 1f) && num5 > 100 && (value2.IsTown || value2.IsVillage) && num27 < 4f)
                            {
                                float num28 = (float)((int)(num4 * ((num27 < 1f && value2.IsVillage) ? Campaign.Current.Models.PartyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromVillage : Campaign.Current.Models.PartyFoodBuyingModel.MinimumDaysFoodToLastWhileBuyingFoodFromTown)) + 1);
                                float num29 = 3f - Math.Min(3f, Math.Max(0f, num27 - 1f));
                                float num30 = num28 + 20f * (float)(value2.IsTown ? 2 : 1) * ((num18 > 100f) ? 1f : (num18 / 100f));
                                int val = (int)((float)(num5 - 100) / Campaign.Current.Models.PartyFoodBuyingModel.LowCostFoodPriceAverage);
                                num26 += num29 * num29 * 0.093f * ((num27 < 2f) ? (1f + 0.5f * (2f - num27)) : 1f) * (float)Math.Pow((double)(Math.Min(num30, (float)Math.Min(val, value2.ItemRoster.TotalFood)) / num30), 0.5);
                            }
                            float num31 = 0f;
                            int num32 = 0;
                            int num33 = 0;
                            if (item < 1f && mobileParty.CanPayMoreWage())
                            {
                                num32 = value2.NumberOfLordPartiesAt;
                                num33 = value2.NumberOfLordPartiesTargeting;
                                if (currentSettlementOfMobilePartyForAICalculation == value2)
                                {
                                    int num34 = num32;
                                    Army army = mobileParty.Army;
                                    num32 = num34 - ((army != null) ? army.LeaderPartyAndAttachedPartiesCount : 1);
                                    if (num32 < 0)
                                    {
                                        num32 = 0;
                                    }
                                }
                                if (mobileParty.TargetSettlement == value2 || (mobileParty.Army != null && mobileParty.Army.LeaderParty.TargetSettlement == value2))
                                {
                                    int num35 = num33;
                                    Army army2 = mobileParty.Army;
                                    num33 = num35 - ((army2 != null) ? army2.LeaderPartyAndAttachedPartiesCount : 1);
                                    if (num33 < 0)
                                    {
                                        num33 = 0;
                                    }
                                }
                                if (!mobileParty.Party.IsStarving && (float)leaderHero.Gold > num12 && (leaderHero.Clan.Leader == leaderHero || (float)leaderHero.Clan.Gold > num11) && num9 > mobileParty.PartySizeRatio)
                                {
                                    num31 = (float)this.ApproximateNumberOfVolunteersCanBeRecruitedFromSettlement(leaderHero, value2);
                                    num31 = ((num31 > (float)((int)((num9 - mobileParty.PartySizeRatio) * 100f))) ? ((float)((int)((num9 - mobileParty.PartySizeRatio) * 100f))) : num31);
                                }
                            }
                            float num36 = num31 * num19 / MathF.Sqrt((float)(1 + num32 + num33));
                            float num37 = (num36 < 1f) ? num36 : ((float)Math.Pow((double)num36, (double)num2));
                            float num38 = Math.Max(Math.Min(1f, num26), Math.Max((mapFaction == value2.MapFaction) ? 0.25f : 0.16f, num * Math.Max(1f, Math.Min(2f, num)) * num37 * (1f - 0.9f * num23) * (1f - 0.9f * num23)));
                            if (mobileParty.Army != null)
                            {
                                num38 /= (float)mobileParty.Army.LeaderPartyAndAttachedPartiesCount;
                            }
                            num16 *= num38 * num24 * num26 * num20;
                            if (num16 >= 2.5f)
                            {
                                this.AddBehaviorTupleWithScore(p, value2, num16);
                                break;
                            }
                            float num39 = 1f;
                            if (num31 > 0f)
                            {
                                num39 = 1f + ((mobileParty.DefaultBehavior == AiBehavior.GoToSettlement && value2 != currentSettlementOfMobilePartyForAICalculation && num17 < num13) ? (0.1f * MathF.Min(5f, num31) - 0.1f * MathF.Min(5f, num31) * (num17 / num13) * (num17 / num13)) : 0f);
                            }
                            float num40 = value2.IsCastle ? 1.4f : 1f;
                            num16 *= (value2.IsTown ? num7 : 1f) * num39 * num40;
                            if (num16 >= 2.5f)
                            {
                                this.AddBehaviorTupleWithScore(p, value2, num16);
                                break;
                            }
                            int num41 = mobileParty.PrisonRoster.TotalRegulars;
                            if (mobileParty.PrisonRoster.TotalHeroes > 0)
                            {
                                foreach (TroopRosterElement troopRosterElement in mobileParty.PrisonRoster.GetTroopRoster())
                                {
                                    if (troopRosterElement.Character.IsHero && troopRosterElement.Character.HeroObject.Clan.IsAtWarWith(value2.MapFaction))
                                    {
                                        num41 += 6;
                                    }
                                }
                            }
                            float num42 = 1f;
                            float num43 = 1f;
                            if (mobileParty.Army != null)
                            {
                                if (mobileParty.Army.LeaderParty != mobileParty)
                                {
                                    num42 = ((float)mobileParty.Army.CohesionThresholdForDispersion - mobileParty.Army.Cohesion) / (float)mobileParty.Army.CohesionThresholdForDispersion;
                                }
                                num43 = ((MobileParty.MainParty != null && mobileParty.Army == MobileParty.MainParty.Army) ? 0.6f : 0.8f);
                                foreach (MobileParty mobileParty4 in mobileParty.Army.LeaderParty.AttachedParties)
                                {
                                    num41 += mobileParty4.PrisonRoster.TotalRegulars;
                                    if (mobileParty4.PrisonRoster.TotalHeroes > 0)
                                    {
                                        foreach (TroopRosterElement troopRosterElement2 in mobileParty4.PrisonRoster.GetTroopRoster())
                                        {
                                            if (troopRosterElement2.Character.IsHero && troopRosterElement2.Character.HeroObject.Clan.IsAtWarWith(value2.MapFaction))
                                            {
                                                num41 += 6;
                                            }
                                        }
                                    }
                                }
                            }
                            float num44 = value2.IsFortification ? (1f + 2f * (float)(num41 / num8)) : 1f;
                            float num45 = 1f;
                            if (mobileParty.Ai.NumberOfRecentFleeingFromAParty > 0)
                            {
                                Vec2 v = value2.Position2D - mobileParty.Position2D;
                                v.Normalize();
                                float num46 = mobileParty.AverageFleeTargetDirection.Distance(v);
                                num45 = 1f - Math.Max(2f - num46, 0f) * 0.25f * (Math.Min((float)mobileParty.Ai.NumberOfRecentFleeingFromAParty, 10f) * 0.2f);
                            }
                            float num47 = 1f;
                            float num48 = 1f;
                            float num49 = 1f;
                            float num50 = 1f;
                            float num51 = 1f;
                            if (num26 <= 0.5f)
                            {
                                ValueTuple<float, float, float, float> valueTuple2 = this.CalculateBeingSettlementOwnerScores(mobileParty, value2, currentSettlementOfMobilePartyForAICalculation, -1f, num19, item);
                                num47 = valueTuple2.Item1;
                                num48 = valueTuple2.Item2;
                                num49 = valueTuple2.Item3;
                                num50 = valueTuple2.Item4;
                            }
                            else
                            {
                                float num52 = MathF.Sqrt(num10);
                                num51 = ((num52 > num15) ? (1f + 7f * MathF.Min(1f, num26 - 0.5f)) : (1f + 7f * (num52 / num15) * MathF.Min(1f, num26 - 0.5f)));
                            }
                            num16 *= num45 * num51 * num25 * num42 * num44 * num43 * num47 * num49 * num48 * num50;
                        }
                    IL_CC7:
                        if (num16 > 0.025f)
                        {
                            this.AddBehaviorTupleWithScore(p, value2, num16);
                            continue;
                        }
                        continue;
                    IL_37E:
                        this.AddBehaviorTupleWithScore(p, value2, this.CalculateMergeScoreForDisbandingParty(mobileParty, value2, item6));
                        goto IL_CC7;
                    }
                }
            }
        }
    }
}