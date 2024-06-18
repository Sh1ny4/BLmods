using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using HarmonyLib;
using TaleWorlds.Localization;

namespace wipo.patches.TownsBehaviours
{
    [HarmonyPatch(typeof(DefaultBuildingConstructionModel), "CalculateDailyConstructionPowerInternal")]
    internal class CalculateDailyConstructionPowerInternalPatch : DefaultBuildingConstructionModel
    {
        [HarmonyPrefix]
        static bool Prefix(ref DefaultBuildingConstructionModel __instance, ref int __result, Town town, ref ExplainedNumber result, bool omitBoost = false)
        {
            float value = town.Prosperity * 0.01f;
            result.Add(value, GameTexts.FindText("str_prosperity", null), null);
            if (!omitBoost && town.BoostBuildingProcess > 0)
            {
                int num = town.IsCastle ? __instance.CastleBoostCost : __instance.TownBoostCost;
                int num2 = __instance.GetBoostAmount(town);
                float num3 = MathF.Min(1f, (float)town.BoostBuildingProcess / (float)num);
                float num4 = 0f;
                if (town.IsTown && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.Clockwork))
                {
                    num4 += DefaultPerks.Engineering.Clockwork.SecondaryBonus;
                }
                num2 += MathF.Round((float)num2 * num4);
                result.Add((float)num2 * num3, BoostText, null);
            }
            if (town.Governor != null)
            {
                Settlement currentSettlement = town.Governor.CurrentSettlement;
                if (((currentSettlement != null) ? currentSettlement.Town : null) == town)
                {
                    SkillHelper.AddSkillBonusForTown(DefaultSkills.Engineering, DefaultSkillEffects.TownProjectBuildingBonus, town, ref result);
                    PerkHelper.AddPerkBonusForTown(DefaultPerks.Steward.ForcedLabor, town, ref result);
                }
                if (((currentSettlement != null) ? currentSettlement.Town : null) == town && !town.BuildingsInProgress.IsEmpty<Building>())
                {
                    if (town.Governor.GetPerkValue(DefaultPerks.Steward.ForcedLabor) && town.Settlement.Party.PrisonRoster.TotalManCount > 0)
                    {
                        float value2 = MathF.Min(0.3f, (float)(town.Settlement.Party.PrisonRoster.TotalManCount / 3) * DefaultPerks.Steward.ForcedLabor.SecondaryBonus);
                        result.AddFactor(value2, DefaultPerks.Steward.ForcedLabor.Name);
                    }
                    if (town.IsCastle && town.Governor.GetPerkValue(DefaultPerks.Engineering.MilitaryPlanner))
                    {
                        result.AddFactor(DefaultPerks.Engineering.MilitaryPlanner.SecondaryBonus, DefaultPerks.Engineering.MilitaryPlanner.Name);
                    }
                    else if (town.IsTown && town.Governor.GetPerkValue(DefaultPerks.Engineering.Carpenters))
                    {
                        result.AddFactor(DefaultPerks.Engineering.Carpenters.SecondaryBonus, DefaultPerks.Engineering.Carpenters.Name);
                    }
                    Building building = town.BuildingsInProgress.Peek();
                    if ((building.BuildingType == DefaultBuildingTypes.Fortifications || building.BuildingType == DefaultBuildingTypes.CastleBarracks || building.BuildingType == DefaultBuildingTypes.CastleMilitiaBarracks || building.BuildingType == DefaultBuildingTypes.SettlementGarrisonBarracks || building.BuildingType == DefaultBuildingTypes.SettlementMilitiaBarracks || building.BuildingType == DefaultBuildingTypes.SettlementAquaducts) && town.Governor.GetPerkValue(DefaultPerks.Engineering.Stonecutters))
                    {
                        result.AddFactor(DefaultPerks.Engineering.Stonecutters.PrimaryBonus, DefaultPerks.Engineering.Stonecutters.Name);
                    }
                }
            }
            SettlementLoyaltyModel settlementLoyaltyModel = Campaign.Current.Models.SettlementLoyaltyModel;
            int num5 = town.SoldItems.Sum(delegate (Town.SellLog x) { 
                if (x.Category.Properties != ItemCategory.Property.BonusToProduction) 
                { 
                    return 0; 
                } 
                return x.Number; 
            });
            if (num5 > 0)
            {
                result.Add(0.25f * (float)num5, ProductionFromMarketText, null);
            }
            BuildingType buildingType = town.BuildingsInProgress.IsEmpty<Building>() ? null : town.BuildingsInProgress.Peek().BuildingType;
            if (DefaultBuildingTypes.MilitaryBuildings.Contains(buildingType))
            {
                PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.Confidence, town, ref result);
            }
            if (buildingType == DefaultBuildingTypes.SettlementMarketplace || buildingType == DefaultBuildingTypes.SettlementAquaducts || buildingType == DefaultBuildingTypes.SettlementLimeKilns)
            {
                PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.SelfMadeMan, town, ref result);
            }
            float effectOfBuildings = town.GetEffectOfBuildings(BuildingEffectEnum.Construction);
            if (effectOfBuildings > 0f)
            {
                result.Add(effectOfBuildings, GameTexts.FindText("str_building_bonus", null), null);
            }
            result.AddFactor(town.Loyalty / 20 , new TextObject("{=!}Loyalty", null));
            result.LimitMin(10f);
            __result = (int)result.ResultNumber;
            return false;
        }

        static TextObject ProductionFromMarketText = new TextObject("{=vaZDJGMx}Construction from Market", null);
        static TextObject BoostText = new TextObject("{=yX1RycON}Boost from Reserve", null);
    }
}
