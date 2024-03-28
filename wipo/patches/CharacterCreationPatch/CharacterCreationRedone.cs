using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace wipo.patches.CharacterCreationPatch
{

    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "OnInitialized")]
    public class CharacterCreationRedone : SandboxCharacterCreationContent
    {
        [HarmonyPrefix]
        static bool prefix(ref CharacterCreationRedone __instance, CharacterCreation characterCreation)
        {
            __instance.AddMenus(characterCreation);
            return false;
        }

        public void AddMenus(CharacterCreation characterCreation)
        {
            AddParentsMenuPatch(characterCreation);
            AddEducationMenuPatch(characterCreation);
            AddChildhoodMenuPatch(characterCreation);
            AddYouthMenuPatch(characterCreation);
            AddAdulthoodMenuPatch(characterCreation);
            AddAgeSelectionMenuPatch(characterCreation);
        }

        protected void AddParentsMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=b4lDDcli}Family", null), new TextObject("{=XgFU1pCx}You were born into a family of..", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(AseraiParentsOnCondition));
            characterCreationCategory1.AddCategoryOption(new TextObject("{=Sw8OxnNr}Kinsfolk of an emir", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Throwing }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiTribesmanOnApply), new TextObject("{=MFrIHJZM}Your family was from a smaller offshoot of an emir's tribe. Your father's land gave him enough income to afford a horse but he was not quite wealthy enough to buy the armor needed to join the heavier cavalry. He fought as one of the light horsemen for which the desert is famous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=ngFVgwDD}Warrior-slaves", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiWariorSlaveOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiWariorSlaveOnApply), new TextObject("{=GsPC2MgU}Your father was part of one of the slave-bodyguards maintained by the Aserai emirs. He fought by his master's side with tribe's armored cavalry, and was freed - perhaps for an act of valor, or perhaps he paid for his freedom with his share of the spoils of battle. He then married your mother.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiMerchantOnApply), new TextObject("{=1zXrlaav}Your family were respected traders in an oasis town. They ran caravans across the desert, and were experts in the finer points of negotiating passage through the desert tribes' territories.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=g31pXuqi}Oasis farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiOasisFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiOasisFarmerOnApply), new TextObject("{=5P0KqBAw}Your family tilled the soil in one of the oases of the Nahasa and tended the palm orchards that produced the desert's famous dates. Your father was a member of the main foot levy of his tribe, fighting with his kinsmen under the emir's banner.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=EEedqolz}Bedouin", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBedouinOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBedouinOnApply), new TextObject("{=PKhcPbBX}Your family were part of a nomadic clan, crisscrossing the wastes between wadi beds and wells to feed their herds of goats and camels on the scraggly scrubs of the Nahasa.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=tRIrbTvv}Urban back-alley thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Polearm }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBackAlleyThugOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBackAlleyThugOnApply), new TextObject("{=6bUSbsKC}Your father worked for a fitiwi, one of the strongmen who keep order in the poorer quarters of the oasis towns. He resolved disputes over land, dice and insults, imposing his authority with the fitiwi's traditional staff.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(BattanianParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("{=GeNKQlHR}Members of the chieftain's hearthguard", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaChieftainsHearthguardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaChieftainsHearthguardOnApply), new TextObject("{=LpH8SYFL}Your family were the trusted kinfolk of a Battanian chieftain, and sat at his table in his great hall. Your father assisted his chief in running the affairs of the clan and trained with the traditional weapons of the Battanian elite, the two-handed sword or falx and the bow.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=AeBzTj6w}Healers", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaHealerOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaHealerOnApply), new TextObject("{=j6py5Rv5}Your parents were healers who gathered herbs and treated the sick. As a living reservoir of Battanian tradition, they were also asked to adjudicate many disputes between the clans.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=tGEStbxb}Tribespeople", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaTribesmanOnApply), new TextObject("{=WchH8bS2}Your family were middle-ranking members of a Battanian clan, who tilled their own land. Your father fought with the kern, the main body of his people's warriors, joining in the screaming charges for which the Battanians were famous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=BCU6RezA}Smiths", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaSmithOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaSmithOnApply), new TextObject("{=kg9YtrOg}Your family were smiths, a revered profession among the Battanians. They crafted everything from fine filigree jewelry in geometric designs to the well-balanced longswords favored by the Battanian aristocracy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=7eWmU2mF}Foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaWoodsmanOnApply), new TextObject("{=7jBroUUQ}Your family had little land of their own, so they earned their living from the woods, hunting and trapping. They taught you from an early age that skills like finding game trails and killing an animal with one shot could make the difference between eating and starvation.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=SpJqhEEh}Bards", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaBardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaBardOnApply), new TextObject("{=aVzcyhhy}Your father was a bard, drifting from chieftain's hall to chieftain's hall making his living singing the praises of one Battanian aristocrat and mocking his enemies, then going to his enemy's hall and doing the reverse. You learned from him that a clever tongue could spare you  from a life toiling in the fields, if you kept your wits about you.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(EmpireParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("{=InN5ZZt3}A landlord's retainers", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireLandlordsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireLandlordsRetainerOnApply), new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireMerchantOnApply), new TextObject("{=FQntPChs}Your family were merchants in one of the main cities of the Empire. They sometimes organized caravans to nearby towns, and discussed issues in the town council.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=sb4gg8Ak}Freeholders", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireFreeholderOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireFreeholderOnApply), new TextObject("{=09z8Q08f}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the imperial rural economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=v48N6h1t}Urban artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireArtisanOnApply), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=7eWmU2mF}Foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireWoodsmanOnApply), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=aEke8dSb}Urban vagabonds", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireVagabondOnApply), new TextObject("{=Jvf6K7TZ}Your family numbered among the many poor migrants living in the slums that grow up outside the walls of imperial cities, making whatever money they could from a variety of odd jobs. Sometimes they did service for one of the Empire's many criminal gangs, and you had an early look at the dark side of life.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(KhuzaitParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("{=FVaRDe2a}A noyan's kinsfolk", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNoyansKinsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNoyansKinsmanOnApply), new TextObject("{=jAs3kDXh}Your family were the trusted kinsfolk of a Khuzait noyan, and shared his meals in the chieftain's yurt. Your father assisted his chief in running the affairs of the clan and fought in the core of armored lancers in the center of the Khuzait battle line.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=TkgLEDRM}Merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitMerchantOnApply), new TextObject("{=qPg3IDiq}Your family came from one of the merchant clans that dominated the cities in eastern Calradia before the Khuzait conquest. They adjusted quickly to their new masters, keeping the caravan routes running and ensuring that the tariff revenues that once went into imperial coffers now flowed to the khanate.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=tGEStbxb}Tribespeople", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitTribesmanOnApply), new TextObject("{=URgZ4ai4}Your family were middle-ranking members of one of the Khuzait clans. He had some  herds of his own, but was not rich. When the Khuzait horde was summoned to battle, he fought with the horse archers, shooting and wheeling and wearing down the enemy before the lancers delivered the final punch.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=gQ2tAvCz}Farmers", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitFarmerOnApply), new TextObject("{=5QSGoRFj}Your family tilled one of the small patches of arable land in the steppes for generations. When the Khuzaits came, they ceased paying taxes to the emperor and providing conscripts for his army, and served the khan instead.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=vfhVveLW}Shamans", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitShamanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitShamanOnApply), new TextObject("{=WOKNhaG2}Your family were guardians of the sacred traditions of the Khuzaits, channelling the spirits of the wilderness and of the ancestors. They tended the sick and dispensed wisdom, resolving disputes and providing practical advice.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=Xqba1Obq}Nomads", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Riding }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNomadOnApply), new TextObject("{=9aoQYpZs}Your family's clan never pledged its loyalty to the khan and never settled down, preferring to live out in the deep steppe away from his authority. They remain some of the finest trackers and scouts in the grasslands, as the ability to spot an enemy coming and move quickly is often all that protects their herds from their neighbors' predations.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(SturgianParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("{=mc78FEbA}A boyar's companions", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaBoyarsCompanionOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaBoyarsCompanionOnApply), new TextObject("{=hob3WVkU}Your father was a member of a boyar's druzhina, the 'companions' that make up his retinue. He sat at his lord's table in the great hall, oversaw the boyar's estates, and stood by his side in the center of the shield wall in battle.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=HqzVBfpl}Urban traders", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaTraderOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaTraderOnApply), new TextObject("{=bjVMtW3W}Your family were merchants who lived in one of Sturgia's great river ports, organizing the shipment of the north's bounty of furs, honey and other goods to faraway lands.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=zrpqSWSh}Free farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaFreemanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaFreemanOnApply), new TextObject("{=Mcd3ZyKq}Your family had just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=v48N6h1t}Urban artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaArtisanOnApply), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=YcnK0Thk}Hunters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaHunterOnApply), new TextObject("{=WyZ2UtFF}Your family had no taste for the authority of the boyars. They made their living deep in the woods, slashing and burning fields which they tended for a year or two before moving on. They hunted and trapped fox, hare, ermine, and other fur-bearing animals.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=TPoK3GSj}Vagabonds", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaVagabondOnApply), new TextObject("{=2SDWhGmQ}Your family numbered among the poor migrants living in the slums that grow up outside the walls of the river cities, making whatever money they could from a variety of odd jobs. Sometimes they did services for one of the region's many criminal gangs.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory6 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(VlandianParentsOnCondition));
            characterCreationCategory6.AddCategoryOption(new TextObject("{=2TptWc4m}A baron's retainers", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBaronsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBaronsRetainerOnApply), new TextObject("{=0Suu1Q9q}Your father was a bailiff for a local feudal magnate. He looked after his liege's estates, resolved disputes in the village, and helped train the village levy. He rode with the lord's cavalry, fighting as an armored knight.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMerchantOnApply), new TextObject("{=qNZFkxJb}Your family were merchants in one of the main cities of the kingdom. They organized caravans to nearby towns and were active in the local merchant's guild.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=RDfXuVxT}Yeomen", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaYeomanOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaYeomanOnApply), new TextObject("{=BLZ4mdhb}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=p2KIhGbE}Urban blacksmith", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBlacksmithOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBlacksmithOnApply), new TextObject("{=btsMpRcA}Your family owned a smithy in a city. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=YcnK0Thk}Hunters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaHunterOnApply), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=ipQP6aVi}Mercenaries", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMercenaryOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMercenaryOnApply), new TextObject("{=yYhX6JQC}Your father joined one of Vlandia's many mercenary companies, composed of men who got such a taste for war in their lord's service that they never took well to peace. Their crossbowmen were much valued across Calradia. Your mother was a camp follower, taking you along in the wake of bloody campaigns.", null), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }

        new protected bool EmpireParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire";
        }
        new protected bool VlandianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia";
        }
        new protected bool SturgianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia";
        }
        new protected bool AseraiParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai";
        }
        new protected bool BattanianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        new protected bool KhuzaitParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait";
        }

        new protected void AseraiTribesmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void AseraiWariorSlaveOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Mercenary, "", "", true, true);
        }
        new protected void AseraiMerchantOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        new protected void AseraiOasisFarmerOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void AseraiBedouinOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Herder, "", "", true, true);
        }
        new protected void AseraiBackAlleyThugOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }


        new protected void BattaniaChieftainsHearthguardOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void BattaniaHealerOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Healer, "", "", true, true);
        }
        new protected void BattaniaTribesmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void BattaniaSmithOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        new protected void BattaniaWoodsmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        new protected void BattaniaBardOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Bard, "", "", true, true);
        }


        new protected void EmpireLandlordsRetainerOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void EmpireMerchantOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        new protected void EmpireFreeholderOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void EmpireArtisanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        new protected void EmpireWoodsmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        new protected void EmpireVagabondOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }


        new protected void KhuzaitNoyansKinsmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void KhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        new protected void KhuzaitTribesmanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Herder, "", "", true, true);
        }
        new protected void KhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void KhuzaitShamanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Healer, "", "", true, true);
        }
        new protected void KhuzaitNomadOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Herder, "", "", true, true);
        }


        new protected void SturgiaBoyarsCompanionOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void SturgiaTraderOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        new protected void SturgiaFreemanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void SturgiaArtisanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        new protected void SturgiaHunterOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        new protected void SturgiaVagabondOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }


        new protected void VlandiaBaronsRetainerOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, SandboxCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        new protected void VlandiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, SandboxCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        new protected void VlandiaYeomanOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, SandboxCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        new protected void VlandiaBlacksmithOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, SandboxCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        new protected void VlandiaHunterOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        new protected void VlandiaMercenaryOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, SandboxCharacterCreationContent.OccupationTypes.Mercenary, "", "", true, true);
        }











        protected void AddEducationMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Education", null), new TextObject("{=!}Your parents insisted that you receive an education in...", null), new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHerderOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHerderOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Crafting }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceSmithyOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceSmithyOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceRepairmanOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceRepairmanOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceGathererOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceGathererOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHunterOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHunterOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHelperOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHelperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Crossbow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceWatcherOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceWatcherOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceGangerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceGangerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Crafting }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceDockerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceMarketerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Steward }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected bool RuralType()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Farmer || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Hunter || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Bard || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Herder || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Vagabond || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Healer || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Artisan;
        }
        new protected bool RichParents()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Merchant;
        }
        new protected bool RuralAdolescenceOnCondition()
        {
            return this.RuralType();
        }
        new protected bool UrbanAdolescenceOnCondition()
        {
            return !this.RuralType();
        }
        new protected bool UrbanRichAdolescenceOnCondition()
        {
            return !this.RuralType() && this.RichParents();
        }
        new protected bool UrbanPoorAdolescenceOnCondition()
        {
            return !this.RuralType() && !this.RichParents();
        }

        new protected void RuralAdolescenceHerderOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_streets"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
        }
        new protected void RuralAdolescenceSmithyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_militia"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
        }
        new protected void RuralAdolescenceRepairmanOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_grit"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "carry_hammer", true, "");
        }
        new protected void RuralAdolescenceGathererOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_peddlers"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
        }
        new protected void RuralAdolescenceHunterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_sharp"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "composite_bow", true, "");
        }
        new protected void RuralAdolescenceHelperOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_peddlers_2"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
        }
        new protected void UrbanAdolescenceWatcherOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_fox"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        new protected void UrbanAdolescenceMarketerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_manners"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        new protected void UrbanAdolescenceGangerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_athlete"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        new protected void UrbanAdolescenceDockerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_peddlers"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
        }
        new protected void UrbanAdolescenceHorserOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_peddlers_2"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
        }
        new protected void UrbanAdolescenceTutorOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_book"
            });
            this.RefreshPropsAndClothing(characterCreation, false, "character_creation_notebook", false, "");
        }











        protected void AddChildhoodMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Character", null), new TextObject("{=!}Whe you were a kid, you were noted for your...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}leadership skills", null), new MBList<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Tactics, DefaultSkills.Scouting }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodLeadingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}brawn", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Throwing, DefaultSkills.Trade }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodYourBrawnOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodAthleticsOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}attention to detail", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Crafting , DefaultSkills.Riding}, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodAttentionToDetailOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMemoryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}aptitude for numbers", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Trade, DefaultSkills.Crafting }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodAptitudeForNumbersOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMathOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}way with people", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Leadership, DefaultSkills.Trade }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodWayWithPeopleOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMannersOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}skill with horses", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Medicine, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodSkillsWithHorsesOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodAffinityWithAnimalsOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }

        new protected static void ChildhoodYourLeadershipSkillsOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_leader"
            });
        }
        new protected static void ChildhoodYourBrawnOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_athlete"
            });
        }
        new protected static void ChildhoodAttentionToDetailOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_memory"
            });
        }
        new protected static void ChildhoodAptitudeForNumbersOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_numbers"
            });
        }
        new protected static void ChildhoodWayWithPeopleOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_manners"
            });
        }
        new protected static void ChildhoodSkillsWithHorsesOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_animals"
            });
        }











        protected void AddYouthMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Early Adulthood", null), new TextObject("{=!}You started your life as..", null), new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            //Aserai
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a faris", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(AseraiRetainerOnCondition), new CharacterCreationOnSelect(YouthAseraiFarisOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a caravaner", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiCaravanerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOutridersOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a farmer", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}slave warrior", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiSlaveWarriorOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}mounted archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiMountedArcherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiArcherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}an hashashin", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiHashashinOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Battania
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a member of a fianna", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaRetainerOnCondition), new CharacterCreationOnSelect(YouthBattaniaFiannaOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthChieftainOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a druid", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Scouting, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaDruidOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthKernOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward,DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBataniaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade,DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthHearthGuardOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a forester", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Scouting, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattanniaForesterOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}part of the kern", null), new MBList<SkillObject> { DefaultSkills.Throwing, DefaultSkills.OneHanded,DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaKernOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}shock troop", null), new MBList<SkillObject> { DefaultSkills.Throwing, DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaFalxOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}scout", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Throwing, DefaultSkills.Polearm }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaScoutOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a thug", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaThugOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Empire
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a small unit commander", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics,DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(EmpireRetainerOnCondition), new CharacterCreationOnSelect(YouthEmpireEngineerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCommanderOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}an engineer's student", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Steward, DefaultSkills.Crafting }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireEngineerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a peasant", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}legionary", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireLegionaryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireArcherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}light cavalry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}mounted archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireHorseArcherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a member of a gang", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireGangOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Khuzait
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}part of a khan's guard", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm, DefaultSkills.Bow }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitRetainerOnCondition), new CharacterCreationOnSelect(YouthKhuzaitKhansGuardOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthChieftainOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a nomad", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Bow, DefaultSkills.Scouting }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a farmer", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}part of the cavalry", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}mounted archer", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitHorseArcherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}infantry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a thug", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitThugOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Sturgia
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a member of a druzhina", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaDruzhinaOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}culture unique route", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Scouting, DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaUNIQUEROUTEOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthHearthGuardOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a peasant", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}part of the infantry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}shock troop", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaShockTroopOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}bowman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaArcherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a raider", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Roguery, DefaultSkills.Scouting }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaRaiderOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Vlandia
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a knight", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaRetainerOnCondition), new CharacterCreationOnSelect(YouthVlandiaKnightOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGroomOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}chamberlain", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaChamberlainOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a member of a guild", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaGuildOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a serf", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaSerfOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}levied footman", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}light cavalry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaLightCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}levied crossbowman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Crossbow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaCrossbowmanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}a highwayman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Scouting, DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiHighwaymanOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            
            characterCreation.AddNewMenu(characterCreationMenu);
        }


        protected bool AseraiOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai";
        }
        protected bool AseraiRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected void YouthAseraiFarisOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiCaravanerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiSlaveWarriorOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiMountedArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthAseraiHashashinOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }


        protected bool BattaniaOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        protected bool BattaniaRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected void YouthBattaniaFiannaOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattaniaDruidOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBataniaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattaniaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattanniaForesterOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattaniaKernOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattaniaFalxOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattaniaScoutOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthBattaniaThugOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }


        protected bool EmpireOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire";
        }
        protected bool EmpireRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected void YouthEmpireCommanderOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireEngineerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireLegionaryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthEmpireGangOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }


        protected bool KhuzaitOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait";
        }
        protected bool KhuzaitRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected void YouthKhuzaitKhansGuardOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitNomadOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthKhuzaitThugOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }


        protected bool SturgiaOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia";
        }
        protected bool SturgiaRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected void YouthSturgiaDruzhinaOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaUNIQUEROUTEOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaShockTroopOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthSturgiaRaiderOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }


        protected bool VlandiaOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia";
        }
        protected bool VlandiaRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected void YouthVlandiaKnightOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaChamberlainOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaGuildOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaSerfOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaLightCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiaCrossbowmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        protected void YouthVlandiHighwaymanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }













        protected void AddAdulthoodMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Adulthood", null), new TextObject("{=!}You started adventuring...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to discover the world", null), new MBList<SkillObject> { DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentDefeatedEnemyOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentDefeatedEnemyOnApply), new TextObject("{=!}The temptation of travel was to much for you, as you always dreamt of seeing the world.", null), null, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to take revenge", null), new MBList<SkillObject> { DefaultSkills.Tactics }, DefaultCharacterAttributes.Vigor, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentExpeditionOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}After being wronged, you felt the need for revenge. With that goal in mind, you wandered throughout Calradia to get reparations", null), new MBList<TraitObject> { DefaultTraits.Mercy }, -1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}after being forced out", null), new MBList<SkillObject> { DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}After one last disagreement with you, your parents forced you out. With nowhere to go, adventuring was all you could do", null), new MBList<TraitObject> { DefaultTraits.Honor }, -1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}in search of money", null), new MBList<SkillObject> { DefaultSkills.Trade}, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentSavedVillageOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}You always wanted to make riches, and it was obvious for you that staying at home would never allow you do it.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to become one of the powerful", null), new MBList<SkillObject> { DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentSavedStreetOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}Seeing how those in power had many advantages, you joined on an adventure to join them, or even replace them.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to mark history", null), new MBList<SkillObject> { DefaultSkills.Charm  }, DefaultCharacterAttributes.Social, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=!}With all the wars in Calradia, there are many way one could carve {?PLAYER.GENDER}her{?}his{\\?} name in history.", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}after the loss of a loved one", null), new MBList<SkillObject> { DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentExpeditionOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}After losing some close to you, you left to see if you could fill that hole", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to practice your trade", null), new MBList<SkillObject> { DefaultSkills.Crafting }, DefaultCharacterAttributes.Endurance, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=!}Your craftmanship has been an important part of your life, but your skill wasn't enough. You thus decided to embark on a journey to improve at it.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to help those in need", null), new MBList<SkillObject> { DefaultSkills.Medicine }, DefaultCharacterAttributes.Intelligence, 1, 50, 2, null, new CharacterCreationOnSelect(AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=!}Having been taught in medicine, you decide to set out and help those in need, as with the wars many have suffered.", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 2, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to prove your fighting skills", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 20, 2, null, new CharacterCreationOnSelect(AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=!}Attaching much importance to your fighting skill, you figured there was no better place than calradia to prove your might.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 0, 5, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }



        new protected void AccomplishmentDefeatedEnemyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_athlete"
            });
        }
        new protected void AccomplishmentExpeditionOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_gracious"
            });
        }
        new protected void AccomplishmentMerchantOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_ready"
            });
        }
        new protected void AccomplishmentSavedVillageOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_vibrant"
            });
        }
        new protected void AccomplishmentSavedStreetOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_vibrant"
            });
        }
        new protected void AccomplishmentWorkshopOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }
        new protected void AccomplishmentSiegeHunterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_tough"
            });
        }
        new protected void AccomplishmentEscapadeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_clever"
            });
        }
        new protected void AccomplishmentTreaterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_manners"
            });
        }











        protected void AddAgeSelectionMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), new CharacterCreationOnInit(this.StartingAgeOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}16", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeMinorOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeMinorOnApply), new TextObject("{=!}Some say you are too young, but your eagerness more than makes up for your lack of experience.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}21", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeYoungOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeYoungOnApply), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), null, 0, 0, 0, 2, 1);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}30", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeAdultOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeAdultOnApply), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), null, 0, 0, 0, 4, 2);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}40", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeMiddleAgedOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeMiddleAgedOnApply), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), null, 0, 0, 0, 6, 3);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}50", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeElderlyOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeElderlyOnApply), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), null, 0, 0, 0, 8, 4);
            characterCreation.AddNewMenu(characterCreationMenu);
        }


        protected void StartingAgeMinorOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(20f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_focus"
            });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Minor;
            this.SetHeroAge(16f);
        }
        protected void StartingAgeMinorOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Minor;
        }


        new protected void StartingAgeYoungOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(20f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_focus"
            });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.YoungAdult;
            this.SetHeroAge(21f);
        }
        new protected void StartingAgeYoungOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.YoungAdult;
        }


        new protected void StartingAgeAdultOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(30f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_ready"
            });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Adult;
            this.SetHeroAge(30f);
        }
        new protected void StartingAgeAdultOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Adult;
        }


        new protected void StartingAgeMiddleAgedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(40f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_sharp"
            });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.MiddleAged;
            this.SetHeroAge(40f);
        }
        new protected void StartingAgeMiddleAgedOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.MiddleAged;
        }


        new protected void StartingAgeElderlyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ClearFaceGenPrefab();
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(50f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_tough"
            });
            this.RefreshPlayerAppearance(characterCreation);
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Elder;
            this.SetHeroAge(50f);
        }
        new protected void StartingAgeElderlyOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = (SandboxCharacterCreationContent.SandboxAgeOptions)CharacterCreationRedone.SandboxAgeOptions.Elder;
        }


        new protected enum SandboxAgeOptions
        {
            Minor = 16,
            YoungAdult = 21,
            Adult = 30,
            MiddleAged = 40,
            Elder = 50
        }
    }
}
