using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace wipo.patches.EliteInCastle
{

    [HarmonyPatch(typeof(NotablesCampaignBehavior), nameof(NotablesCampaignBehavior.OnNewGameCreated))]
    public class NotableSpawnPatch
    {
        [HarmonyPostfix]
        static void Postfix()
        {
            foreach (Settlement settlement in Settlement.All)
            {
                if (settlement.IsCastle)
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
        }
    }
}