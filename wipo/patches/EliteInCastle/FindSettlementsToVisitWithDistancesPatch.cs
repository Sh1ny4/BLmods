using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using TaleWorlds.CampaignSystem.Party;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem;

namespace wipo.patches.EliteInCastle
{
    [HarmonyPatch(typeof(AiVisitSettlementBehavior), "FindSettlementsToVisitWithDistances")]
    internal class FindSettlementsToVisitWithDistancesPatch : AiVisitSettlementBehavior
    {
        [HarmonyPrefix]
        static bool Prefix(ref SortedList<ValueTuple<float, int>, Settlement> __result, ref FindSettlementsToVisitWithDistancesPatch __instance ,MobileParty mobileParty)
        {
            SortedList<ValueTuple<float, int>, Settlement> sortedList = new SortedList<ValueTuple<float, int>, Settlement>();
            MapDistanceModel mapDistanceModel = Campaign.Current.Models.MapDistanceModel;
            if (mobileParty.LeaderHero != null && mobileParty.LeaderHero.MapFaction.IsKingdomFaction)
            {
                if (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty)
                {
                    LocatableSearchData<Settlement> locatableSearchData = Settlement.StartFindingLocatablesAroundPosition(mobileParty.Position2D, 30f);
                    for (Settlement settlement = Settlement.FindNextLocatable(ref locatableSearchData); settlement != null; settlement = Settlement.FindNextLocatable(ref locatableSearchData))
                    {
                        if (settlement.MapFaction != mobileParty.MapFaction && __instance.IsSettlementSuitableForVisitingConditionPatch(mobileParty, settlement))
                        {
                            float distance = mapDistanceModel.GetDistance(mobileParty, settlement);
                            if (distance < 350f)
                            {
                                sortedList.Add(new ValueTuple<float, int>(distance, settlement.GetHashCode()), settlement);
                            }
                        }
                    }
                }
                using (List<Settlement>.Enumerator enumerator = mobileParty.MapFaction.Settlements.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Settlement settlement2 = enumerator.Current;
                        if (__instance.IsSettlementSuitableForVisitingConditionPatch(mobileParty, settlement2))
                        {
                            float distance2 = mapDistanceModel.GetDistance(mobileParty, settlement2);
                            if (distance2 < 350f)
                            {
                                sortedList.Add(new ValueTuple<float, int>(distance2, settlement2.GetHashCode()), settlement2);
                            }
                        }
                    }
                    __result = sortedList;
                    return false;
                }
            }
            LocatableSearchData<Settlement> locatableSearchData2 = Settlement.StartFindingLocatablesAroundPosition(mobileParty.Position2D, 50f);
            for (Settlement settlement3 = Settlement.FindNextLocatable(ref locatableSearchData2); settlement3 != null; settlement3 = Settlement.FindNextLocatable(ref locatableSearchData2))
            {
                if (__instance.IsSettlementSuitableForVisitingConditionPatch(mobileParty, settlement3))
                {
                    float distance3 = mapDistanceModel.GetDistance(mobileParty, settlement3);
                    if (distance3 < 350f)
                    {
                        sortedList.Add(new ValueTuple<float, int>(distance3, settlement3.GetHashCode()), settlement3);
                    }
                }
            }
            __result = sortedList;
            return false;
        }

        public bool IsSettlementSuitableForVisitingConditionPatch(MobileParty mobileParty, Settlement settlement)
        {
            return settlement.Party.MapEvent == null 
                && settlement.Party.SiegeEvent == null 
                && (!mobileParty.Party.Owner.MapFaction.IsAtWarWith(settlement.MapFaction) || (mobileParty.Party.Owner.MapFaction.IsMinorFaction && settlement.IsVillage)) 
                && (settlement.IsVillage || settlement.IsFortification) 
                && (!settlement.IsVillage || settlement.Village.VillageState == Village.VillageStates.Normal);
        }
    }
}
