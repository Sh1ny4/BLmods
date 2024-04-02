using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace wipo.patches.EliteInCastle
{

    [HarmonyPatch(typeof(NotablesCampaignBehavior), nameof(NotablesCampaignBehavior.OnNewGameCreated))]
    public class NotableSpawnPatch
    {
        [HarmonyPrefix]
        static bool Prefix()
        {
            foreach (Settlement settlement in Settlement.All)
            {
                if (settlement.IsTown)
                {
                    int targetNotableCountForSettlement = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Artisan);
                    for (int i = 0; i < targetNotableCountForSettlement; i++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.Artisan, settlement);
                    }
                    int targetNotableCountForSettlement2 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Merchant);
                    for (int j = 0; j < targetNotableCountForSettlement2; j++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.Merchant, settlement);
                    }
                    int targetNotableCountForSettlement3 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.GangLeader);
                    for (int k = 0; k < targetNotableCountForSettlement3; k++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.GangLeader, settlement);
                    }
                }
                else if (settlement.IsVillage)
                {
                    int targetNotableCountForSettlement4 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.RuralNotable);
                    for (int l = 0; l < targetNotableCountForSettlement4; l++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.RuralNotable, settlement);
                    }
                    int targetNotableCountForSettlement5 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Headman);
                    for (int m = 0; m < targetNotableCountForSettlement5; m++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.Headman, settlement);
                    }
                }
                else if (settlement.IsCastle)
                {
                    int targetNotableCountForSettlement6 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.RuralNotable);
                    for (int l = 0; l < targetNotableCountForSettlement6; l++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.RuralNotable, settlement);
                    }
                    int targetNotableCountForSettlement7 = Campaign.Current.Models.NotableSpawnModel.GetTargetNotableCountForSettlement(settlement, Occupation.Headman);
                    for (int m = 0; m < targetNotableCountForSettlement7; m++)
                    {
                        HeroCreator.CreateHeroAtOccupation(Occupation.Headman, settlement);
                    }
                }
            }
            return false;
        }
    }
}