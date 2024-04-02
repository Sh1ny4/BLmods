using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors;
using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Actions;

namespace wipo.patches.EliteInCastle
{
    internal class HourlyTickPartyPatch : RecruitmentCampaignBehavior
    {
        new public void HourlyTickParty(MobileParty mobileParty)
        {
            if ((mobileParty.IsCaravan || mobileParty.IsLordParty) && mobileParty.MapEvent == null && mobileParty != MobileParty.MainParty)
            {
                Settlement currentSettlementOfMobilePartyForAICalculation = MobilePartyHelper.GetCurrentSettlementOfMobilePartyForAICalculation(mobileParty);
                if (currentSettlementOfMobilePartyForAICalculation != null)
                {
                    if ((currentSettlementOfMobilePartyForAICalculation.IsVillage && !currentSettlementOfMobilePartyForAICalculation.IsRaided && !currentSettlementOfMobilePartyForAICalculation.IsUnderRaid) || (currentSettlementOfMobilePartyForAICalculation.IsTown && !currentSettlementOfMobilePartyForAICalculation.IsUnderSiege))
                    {
                        this.CheckRecruiting(mobileParty, currentSettlementOfMobilePartyForAICalculation);
                        return;
                    }
                }
                else if (MBRandom.RandomFloat < 0.05f && mobileParty.LeaderHero != null && mobileParty.ActualClan != Clan.PlayerClan && !mobileParty.IsCaravan)
                {
                    IFaction mapFaction = mobileParty.MapFaction;
                    if (mapFaction != null && mapFaction.IsMinorFaction && MobileParty.MainParty.Position2D.DistanceSquared(mobileParty.Position2D) > (MobileParty.MainParty.SeeingRange + 5f) * (MobileParty.MainParty.SeeingRange + 5f))
                    {
                        int partySizeLimit = mobileParty.Party.PartySizeLimit;
                        float num = (float)mobileParty.Party.NumberOfAllMembers / (float)partySizeLimit;
                        float num2 = ((double)num < 0.2) ? 1000f : (((double)num < 0.3) ? 2000f : (((double)num < 0.4) ? 3000f : (((double)num < 0.55) ? 4000f : (((double)num < 0.7) ? 5000f : 7000f))));
                        float num3 = ((float)mobileParty.LeaderHero.Gold > num2) ? 1f : MathF.Sqrt((float)mobileParty.LeaderHero.Gold / num2);
                        if (MBRandom.RandomFloat < (1f - num) * num3)
                        {
                            CharacterObject basicTroop = mobileParty.ActualClan.BasicTroop;
                            int num4 = MBRandom.RandomInt(3, 8);
                            if (num4 + mobileParty.Party.NumberOfAllMembers > partySizeLimit)
                            {
                                num4 = partySizeLimit - mobileParty.Party.NumberOfAllMembers;
                            }
                            int troopRecruitmentCost = Campaign.Current.Models.PartyWageModel.GetTroopRecruitmentCost(basicTroop, mobileParty.LeaderHero, false);
                            if (num4 * troopRecruitmentCost > mobileParty.LeaderHero.Gold)
                            {
                                num4 = mobileParty.LeaderHero.Gold / troopRecruitmentCost;
                            }
                            if (num4 > 0)
                            {
                                this.GetRecruitVolunteerFromMap(mobileParty, basicTroop, num4);
                            }
                        }
                    }
                }
            }
        }

    }
}
