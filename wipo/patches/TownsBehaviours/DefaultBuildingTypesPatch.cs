using System;
using HarmonyLib;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace wipo.patches.TownsBehaviours
{
    [HarmonyPatch(typeof(DefaultBuildingTypes), "InitializeAll")]
    internal class test
    {
        [HarmonyPrefix]
        static bool Prefix(ref DefaultBuildingTypes __instance)
        {
            _buildingFortifications = Create("building_fortifications");
            _buildingSettlementGarrisonBarracks = Create("building_settlement_garrison_barracks");
            _buildingSettlementTrainingFields = Create("building_settlement_training_fields");
            _buildingSettlementFairgrounds = Create("building_settlement_fairgrounds");
            _buildingSettlementMarketplace = Create("building_settlement_marketplace");
            _buildingSettlementAquaducts = Create("building_settlement_aquaducts");
            _buildingSettlementForum = Create("building_settlement_forum");
            _buildingSettlementGranary = Create("building_settlement_granary");
            _buildingSettlementOrchard = Create("building_settlement_lime_kilns");
            _buildingSettlementMilitiaBarracks = Create("building_settlement_militia_barracks");
            _buildingSettlementSiegeWorkshop = Create("building_siege_workshop");
            _buildingSettlementLimeKilns = Create("building_settlement_workshop");

            _buildingWall = Create("building_wall");
            _buildingCastleBarracks = Create("building_castle_barracks");
            _buildingCastleTrainingFields = Create("building_castle_training_fields");
            _buildingCastleGranary = Create("building_castle_granary");
            _buildingCastleGardens = Create("building_castle_gardens");
            _buildingCastleCastallansOffice = Create("building_castle_castallans_office");
            _buildingCastleWorkshop = Create("building_castle_workshops");
            _buildingCastleFairgrounds = Create("building_castle_fairgrounds");
            _buildingCastleSiegeWorkshop = Create("building_castle_siege_workshop");
            _buildingCastleMilitiaBarracks = Create("building_castle_militia_barracks");
            _buildingCastleTollCollector = Create("building_castle_lime_kilns");

            _buildingDailyBuildHouse = Create("building_daily_build_house");
            _buildingDailyTrainMilitia = Create("building_daily_train_militia");
            _buildingDailyFestivalsAndGames = Create("building_festivals_and_games");
            _buildingDailyIrrigation = Create("building_irrigation");
            InitializeAll();
            return false;
        }
        static BuildingType Create(string stringId)
        {
            return Game.Current.ObjectManager.RegisterPresumedObject<BuildingType>(new BuildingType(stringId));
        }
        static void InitializeAll()
        {
            //towns
            _buildingFortifications.Initialize(new TextObject("{=CVdK1ax1}Fortifications", null), new TextObject("{=dIM6xa2O}Better fortifications and higher walls around town, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[] { 0, 8000, 16000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 25f, 50f, 100f) }, 1);
            _buildingSettlementGarrisonBarracks.Initialize(new TextObject("{=54vkRuHo}Garrison Barracks", null), new TextObject("{=DHm1MBsj}Lodging for the garrisoned troops. Each level increases the garrison capacity of the stronghold.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 30f, 60f, 100f) }, 0);
            _buildingSettlementTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=otWlERkc}A field for military drills that increases the daily experience gain of all garrisoned units.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Experience, 1f, 2f, 3f) }, 0);
            _buildingSettlementFairgrounds.Initialize(new TextObject("{=ixHqTrX5}Fairgrounds", null), new TextObject("{=0B91pZ2R}A permanent space that hosts fairs. Citizens can gather, drink dance and socialize,  increasing the daily morale of the settlement.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Loyalty, 0.5f, 1f, 1.5f) }, 0);
            _buildingSettlementMarketplace.Initialize(new TextObject("{=zLdXCpne}Marketplace", null), new TextObject("{=Z9LWA6A3}Scheduled market days lure folks from surrounding villages to the settlement and of course the local ruler takes a handsome cut of any sales. Increases the wealth and tax yield of the settlement.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Tax, 5f, 10f, 15f) }, 0);
            _buildingSettlementAquaducts.Initialize(new TextObject("{=f5jHMbOq}Aqueducts", null), new TextObject("{=UojHRjdG}Access to clean water provides room for growth with healthy citizens and a clean infrastructure. Increases daily Prosperity change.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Prosperity, 0.3f, 0.6f, 1f) }, 0);
            _buildingSettlementForum.Initialize(new TextObject("{=paelEWj1}Forum", null), new TextObject("{=wTBtu1t5}An open square in the settlement where people can meet, spend time, and share their ideas. Increases influence of the settlement owner.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Influence, 0.5f, 1f, 1.5f) }, 0);
            _buildingSettlementGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=aK23T43P}Keeps stockpiles of food so that the settlement has more food supply. Each level increases the local food supply.", null), new int[] { 1000, 1500, 2000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Foodstock, 200f, 400f, 600f) }, 0);
            _buildingSettlementLimeKilns.Initialize(new TextObject("{=NbgeKwVr}Workshops", null), new TextObject("{=qR9bEE6g}A building which provides the means required for the manufacture or repair of buildings. Improves project development speed. Also stonemasons reinforce the walls.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Construction, 20f, 40f, 80f) }, 0);
            _buildingSettlementMilitiaBarracks.Initialize(new TextObject("{=l91xAgmU}Militia Grounds", null), new TextObject("{=RliyRJKl}Provides weapons training for citizens. Increases daily militia recruitment.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Militia, 0.5f, 1f, 1.5f) }, 0);
            _buildingSettlementSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=MharAceZ}A workshop dedicated to sieges. Contains tools and materials to repair walls, build and repair siege engines.", null), new int[] { 1000, 1500, 2000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.WallRepairSpeed, 50f, 50f, 50f), new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, 30f, 60f, 100f) }, 0);
            _buildingSettlementOrchard.Initialize(new TextObject("{=AkbiPIij}Orchards", null), new TextObject("{=ZCLVOXgM}Fruit trees and vegetable gardens outside the walls provide food as long as there is no siege.", null), new int[] { 2000, 3000, 4000 }, BuildingLocation.Settlement, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.FoodProduction, 6f, 12f, 18f) }, 0);

            //castles
            _buildingWall.Initialize(new TextObject("{=6pNrNj93}Wall", null), new TextObject("{=oS5Nesmi}Better fortifications and higher walls around the keep, also increases the max garrison limit since it provides more space for the resident troops.", null), new int[] { 0, 2500, 5000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 25f, 50f, 100f) }, 1);
            _buildingCastleBarracks.Initialize(new TextObject("{=x2B0OjhI}Barracks", null), new TextObject("{=HJ1is924}Lodgings for the garrisoned troops. Increases the garrison capacity of the stronghold.", null), new int[] { 500, 1000, 1500 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonCapacity, 30f, 60f, 100f) }, 0);
            _buildingCastleTrainingFields.Initialize(new TextObject("{=BkTiRPT4}Training Fields", null), new TextObject("{=otWlERkc}A field for military drills that increases the daily experience gain of all garrisoned units.", null), new int[] { 500, 1000, 1500 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Experience, 10f, 20f, 30f) }, 0);
            _buildingCastleGranary.Initialize(new TextObject("{=PstO2f5I}Granary", null), new TextObject("{=iazij7fO}Keeps stockpiles of food so that the settlement has more food supply. Increases the local food supply.", null), new int[] { 500, 1000, 1500 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Foodstock, 100f, 150f, 200f) }, 0);
            _buildingCastleGardens.Initialize(new TextObject("{=yT6XN4Mr}Gardens", null), new TextObject("{=ZCLVOXgM}Fruit trees and vegetable gardens outside the walls provide food as long as there is no siege.", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.FoodProduction, 3f, 6f, 9f) }, 0);
            _buildingCastleCastallansOffice.Initialize(new TextObject("{=kLNnFMR9}Castellan's Office", null), new TextObject("{=GDsI6daq}Provides a warden for the castle who maintains discipline and upholds the law.", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.GarrisonWageReduce, 10f, 20f, 30f) }, 0);
            _buildingCastleWorkshop.Initialize(new TextObject("{=NbgeKwVr}Workshops", null), new TextObject("{=qR9bEE6g}A building which provides the means required for the manufacture or repair of buildings. Improves project development speed. Also stonemasons reinforce the walls.", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Construction, 10f, 20f, 40f) }, 0);
            _buildingCastleFairgrounds.Initialize(new TextObject("{=ixHqTrX5}Fairgrounds", null), new TextObject("{=QHZeCDJy}A permanent space that hosts fairs. Citizens can gather, drink dance and socialize, increasing the daily morale of the settlement.", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Loyalty, 0.5f, 1f, 1.5f) }, 0);
            _buildingCastleSiegeWorkshop.Initialize(new TextObject("{=9Bnwttn6}Siege Workshop", null), new TextObject("{=MharAceZ}A workshop dedicated to sieges. Contains tools and materials to repair walls, build and repair siege engines.", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.WallRepairSpeed, 50f, 50f, 50f), new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.SiegeEngineSpeed, 30f, 60f, 100f) }, 0);
            _buildingCastleMilitiaBarracks.Initialize(new TextObject("{=l91xAgmU}Militia Grounds", null), new TextObject("{=YRrx8bAK}Provides weapons training for citizens. Each level increases daily militia recruitment.", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Militia, 1f, 2f, 3f) }, 0);
            _buildingCastleTollCollector.Initialize(new TextObject("{=VawDQKLl}Toll Collector", null), new TextObject("{=ac8PkfhG}Increases tax income from the region", null), new int[] { 500, 750, 1000 }, BuildingLocation.Castle, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.Tax, 10f, 20f, 30f) }, 0);

            //daily
            _buildingDailyBuildHouse.Initialize(new TextObject("{=F4V7oaVx}Housing", null), new TextObject("{=yWXtcxqb}Construct housing so that more folks can settle, increasing population.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.ProsperityDaily, 3f, 3f, 3f) }, 0);
            _buildingDailyTrainMilitia.Initialize(new TextObject("{=p1Y3EU5O}Train Militia", null), new TextObject("{=61J1wa6k}Schedule drills for commoners, increasing militia recruitment.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.MilitiaDaily, 5f, 5f, 5f) }, 0);
            _buildingDailyFestivalsAndGames.Initialize(new TextObject("{=aEmYZadz}Festival and Games", null), new TextObject("{=ovDbQIo9}Organize festivals and games in the settlement, increasing morale.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.LoyaltyDaily, 3f, 3f, 3f) }, 0);
            _buildingDailyIrrigation.Initialize(new TextObject("{=O4cknzhW}Irrigation", null), new TextObject("{=CU9g49fo}Provide irrigation, increasing growth in bound villages.", null), new int[3], BuildingLocation.Daily, new Tuple<BuildingEffectEnum, float, float, float>[] { new Tuple<BuildingEffectEnum, float, float, float>(BuildingEffectEnum.VillageDevelopmentDaily, 5f, 5f, 5f) }, 0);
        }

        static BuildingType _buildingFortifications;
        static BuildingType _buildingSettlementGarrisonBarracks;
        static BuildingType _buildingSettlementTrainingFields;
        static BuildingType _buildingSettlementFairgrounds;
        static BuildingType _buildingSettlementMarketplace;
        static BuildingType _buildingSettlementAquaducts;
        static BuildingType _buildingSettlementForum;
        static BuildingType _buildingSettlementGranary;
        static BuildingType _buildingSettlementOrchard;
        static BuildingType _buildingSettlementMilitiaBarracks;
        static BuildingType _buildingSettlementSiegeWorkshop;
        static BuildingType _buildingSettlementLimeKilns;

        static BuildingType _buildingWall;
        static BuildingType _buildingCastleBarracks;
        static BuildingType _buildingCastleTrainingFields;
        static BuildingType _buildingCastleGranary;
        static BuildingType _buildingCastleGardens;
        static BuildingType _buildingCastleCastallansOffice;
        static BuildingType _buildingCastleWorkshop;
        static BuildingType _buildingCastleFairgrounds;
        static BuildingType _buildingCastleSiegeWorkshop;
        static BuildingType _buildingCastleMilitiaBarracks;
        static BuildingType _buildingCastleTollCollector;

        static BuildingType _buildingDailyBuildHouse;
        static BuildingType _buildingDailyTrainMilitia;
        static BuildingType _buildingDailyFestivalsAndGames;
        static BuildingType _buildingDailyIrrigation;
    }
}