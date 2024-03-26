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
    public class CharacterCreationRedoneVanilla : SandboxCharacterCreationContent
    {
        [HarmonyPrefix]
        static bool prefix(ref CharacterCreationRedoneVanilla __instance, CharacterCreation characterCreation)
        {
            __instance.AddMenus(characterCreation);
            return false;
        }

        public void AddMenus(CharacterCreation characterCreation)
        {
            AddParentsMenuPatch(characterCreation);
            AddChildhoodMenuPatch(characterCreation);
            AddEducationMenuPatch(characterCreation);
            AddYouthMenuPatch(characterCreation);
            AddAdulthoodMenuPatch(characterCreation);
            AddAgeSelectionMenuPatch(characterCreation);
        }

        protected void AddParentsMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=b4lDDcli}Family", null), new TextObject("{=XgFU1pCx}You were born into a family of...", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(AseraiParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("{=Sw8OxnNr}Kinsfolk of an emir", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Throwing }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiTribesmanOnApply), new TextObject("{=MFrIHJZM}Your family was from a smaller offshoot of an emir's tribe. Your father's land gave him enough income to afford a horse but he was not quite wealthy enough to buy the armor needed to join the heavier cavalry. He fought as one of the light horsemen for which the desert is famous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=ngFVgwDD}Warrior-slaves", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiWariorSlaveOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiWariorSlaveOnApply), new TextObject("{=GsPC2MgU}Your father was part of one of the slave-bodyguards maintained by the Aserai emirs. He fought by his master's side with tribe's armored cavalry, and was freed - perhaps for an act of valor, or perhaps he paid for his freedom with his share of the spoils of battle. He then married your mother.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiMerchantOnApply), new TextObject("{=1zXrlaav}Your family were respected traders in an oasis town. They ran caravans across the desert, and were experts in the finer points of negotiating passage through the desert tribes' territories.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=g31pXuqi}Oasis farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiOasisFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiOasisFarmerOnApply), new TextObject("{=5P0KqBAw}Your family tilled the soil in one of the oases of the Nahasa and tended the palm orchards that produced the desert's famous dates. Your father was a member of the main foot levy of his tribe, fighting with his kinsmen under the emir's banner.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=EEedqolz}Bedouin", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBedouinOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBedouinOnApply), new TextObject("{=PKhcPbBX}Your family were part of a nomadic clan, crisscrossing the wastes between wadi beds and wells to feed their herds of goats and camels on the scraggly scrubs of the Nahasa.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=tRIrbTvv}Urban back-alley thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Polearm }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBackAlleyThugOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBackAlleyThugOnApply), new TextObject("{=6bUSbsKC}Your father worked for a fitiwi, one of the strongmen who keep order in the poorer quarters of the oasis towns. He resolved disputes over land, dice and insults, imposing his authority with the fitiwi's traditional staff.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(BattanianParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("{=GeNKQlHR}Members of the chieftain's hearthguard", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaChieftainsHearthguardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaChieftainsHearthguardOnApply), new TextObject("{=LpH8SYFL}Your family were the trusted kinfolk of a Battanian chieftain, and sat at his table in his great hall. Your father assisted his chief in running the affairs of the clan and trained with the traditional weapons of the Battanian elite, the two-handed sword or falx and the bow.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=AeBzTj6w}Healers", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaHealerOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaHealerOnApply), new TextObject("{=j6py5Rv5}Your parents were healers who gathered herbs and treated the sick. As a living reservoir of Battanian tradition, they were also asked to adjudicate many disputes between the clans.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=tGEStbxb}Tribespeople", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaTribesmanOnApply), new TextObject("{=WchH8bS2}Your family were middle-ranking members of a Battanian clan, who tilled their own land. Your father fought with the kern, the main body of his people's warriors, joining in the screaming charges for which the Battanians were famous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=BCU6RezA}Smiths", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaSmithOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaSmithOnApply), new TextObject("{=kg9YtrOg}Your family were smiths, a revered profession among the Battanians. They crafted everything from fine filigree jewelry in geometric designs to the well-balanced longswords favored by the Battanian aristocracy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=7eWmU2mF}Foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaWoodsmanOnApply), new TextObject("{=7jBroUUQ}Your family had little land of their own, so they earned their living from the woods, hunting and trapping. They taught you from an early age that skills like finding game trails and killing an animal with one shot could make the difference between eating and starvation.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=SpJqhEEh}Bards", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaBardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaBardOnApply), new TextObject("{=aVzcyhhy}Your father was a bard, drifting from chieftain's hall to chieftain's hall making his living singing the praises of one Battanian aristocrat and mocking his enemies, then going to his enemy's hall and doing the reverse. You learned from him that a clever tongue could spare you  from a life toiling in the fields, if you kept your wits about you.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(EmpireParentsOnCondition));
            characterCreationCategory.AddCategoryOption(new TextObject("{=InN5ZZt3}A landlord's retainers", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireLandlordsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireLandlordsRetainerOnApply), new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireMerchantOnApply), new TextObject("{=FQntPChs}Your family were merchants in one of the main cities of the Empire. They sometimes organized caravans to nearby towns, and discussed issues in the town council.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=sb4gg8Ak}Freeholders", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireFreeholderOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireFreeholderOnApply), new TextObject("{=09z8Q08f}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the imperial rural economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=v48N6h1t}Urban artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireArtisanOnApply), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=7eWmU2mF}Foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireWoodsmanOnApply), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=aEke8dSb}Urban vagabonds", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireVagabondOnApply), new TextObject("{=Jvf6K7TZ}Your family numbered among the many poor migrants living in the slums that grow up outside the walls of imperial cities, making whatever money they could from a variety of odd jobs. Sometimes they did service for one of the Empire's many criminal gangs, and you had an early look at the dark side of life.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory6 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(KhuzaitParentsOnCondition));
            characterCreationCategory6.AddCategoryOption(new TextObject("{=FVaRDe2a}A noyan's kinsfolk", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNoyansKinsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNoyansKinsmanOnApply), new TextObject("{=jAs3kDXh}Your family were the trusted kinsfolk of a Khuzait noyan, and shared his meals in the chieftain's yurt. Your father assisted his chief in running the affairs of the clan and fought in the core of armored lancers in the center of the Khuzait battle line.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=TkgLEDRM}Merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitMerchantOnApply), new TextObject("{=qPg3IDiq}Your family came from one of the merchant clans that dominated the cities in eastern Calradia before the Khuzait conquest. They adjusted quickly to their new masters, keeping the caravan routes running and ensuring that the tariff revenues that once went into imperial coffers now flowed to the khanate.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=tGEStbxb}Tribespeople", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitTribesmanOnApply), new TextObject("{=URgZ4ai4}Your family were middle-ranking members of one of the Khuzait clans. He had some  herds of his own, but was not rich. When the Khuzait horde was summoned to battle, he fought with the horse archers, shooting and wheeling and wearing down the enemy before the lancers delivered the final punch.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=gQ2tAvCz}Farmers", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitFarmerOnApply), new TextObject("{=5QSGoRFj}Your family tilled one of the small patches of arable land in the steppes for generations. When the Khuzaits came, they ceased paying taxes to the emperor and providing conscripts for his army, and served the khan instead.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=vfhVveLW}Shamans", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitShamanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitShamanOnApply), new TextObject("{=WOKNhaG2}Your family were guardians of the sacred traditions of the Khuzaits, channelling the spirits of the wilderness and of the ancestors. They tended the sick and dispensed wisdom, resolving disputes and providing practical advice.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=Xqba1Obq}Nomads", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Riding }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNomadOnApply), new TextObject("{=9aoQYpZs}Your family's clan never pledged its loyalty to the khan and never settled down, preferring to live out in the deep steppe away from his authority. They remain some of the finest trackers and scouts in the grasslands, as the ability to spot an enemy coming and move quickly is often all that protects their herds from their neighbors' predations.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(SturgianParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("{=mc78FEbA}A boyar's companions", null), new MBList<SkillObject>{DefaultSkills.Riding,DefaultSkills.TwoHanded}, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaBoyarsCompanionOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaBoyarsCompanionOnApply), new TextObject("{=hob3WVkU}Your father was a member of a boyar's druzhina, the 'companions' that make up his retinue. He sat at his lord's table in the great hall, oversaw the boyar's estates, and stood by his side in the center of the shield wall in battle.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=HqzVBfpl}Urban traders", null), new MBList<SkillObject>{DefaultSkills.Trade,DefaultSkills.Tactics}, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaTraderOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaTraderOnApply), new TextObject("{=bjVMtW3W}Your family were merchants who lived in one of Sturgia's great river ports, organizing the shipment of the north's bounty of furs, honey and other goods to faraway lands.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=zrpqSWSh}Free farmers", null), new MBList<SkillObject>{DefaultSkills.Athletics,DefaultSkills.Polearm}, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaFreemanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaFreemanOnApply), new TextObject("{=Mcd3ZyKq}Your family had just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=v48N6h1t}Urban artisans", null), new MBList<SkillObject>{DefaultSkills.Crafting,DefaultSkills.OneHanded}, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaArtisanOnApply), new TextObject("{=ueCm5y1C}Your family owned their own workshop in a city, making goods from raw materials brought in from the countryside. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=YcnK0Thk}Hunters", null), new MBList<SkillObject>{DefaultSkills.Scouting,DefaultSkills.Bow}, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaHunterOnApply), new TextObject("{=WyZ2UtFF}Your family had no taste for the authority of the boyars. They made their living deep in the woods, slashing and burning fields which they tended for a year or two before moving on. They hunted and trapped fox, hare, ermine, and other fur-bearing animals.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=TPoK3GSj}Vagabonds", null), new MBList<SkillObject>{DefaultSkills.Roguery,DefaultSkills.Throwing}, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaVagabondOnApply), new TextObject("{=2SDWhGmQ}Your family numbered among the poor migrants living in the slums that grow up outside the walls of the river cities, making whatever money they could from a variety of odd jobs. Sometimes they did services for one of the region's many criminal gangs.", null), null, 0, 0, 0, 0, 0);

            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(VlandianParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("{=2TptWc4m}A baron's retainers", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBaronsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBaronsRetainerOnApply), new TextObject("{=0Suu1Q9q}Your father was a bailiff for a local feudal magnate. He looked after his liege's estates, resolved disputes in the village, and helped train the village levy. He rode with the lord's cavalry, fighting as an armored knight.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=651FhzdR}Urban merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMerchantOnApply), new TextObject("{=qNZFkxJb}Your family were merchants in one of the main cities of the kingdom. They organized caravans to nearby towns and were active in the local merchant's guild.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=RDfXuVxT}Yeomen", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaYeomanOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaYeomanOnApply), new TextObject("{=BLZ4mdhb}Your family were small farmers with just enough land to feed themselves and make a small profit. People like them were the pillars of the kingdom's economy, as well as the backbone of the levy.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=p2KIhGbE}Urban blacksmith", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBlacksmithOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBlacksmithOnApply), new TextObject("{=btsMpRcA}Your family owned a smithy in a city. Your father played an active if minor role in the town council, and also served in the militia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=YcnK0Thk}Hunters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaHunterOnApply), new TextObject("{=yRFSzSDZ}Your family lived in a village, but did not own their own land. Instead, your father supplemented paid jobs with long trips in the woods, hunting and trapping, always keeping a wary eye for the lord's game wardens.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=ipQP6aVi}Mercenaries", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMercenaryOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMercenaryOnApply), new TextObject("{=yYhX6JQC}Your father joined one of Vlandia's many mercenary companies, composed of men who got such a taste for war in their lord's service that they never took well to peace. Their crossbowmen were much valued across Calradia. Your mother was a camp follower, taking you along in the wake of bloody campaigns.", null), null, 0, 0, 0, 0, 0);

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











        protected void AddChildhoodMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=8Yiwt1z6}Early Childhood", null), new TextObject("{=character_creation_content_16}As a child you were noted for...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=kmM68Qx4}your leadership skills.", null), new MBList<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodLeadingOnApply), new TextObject("{=FfNwXtii}If the wolf pup gang of your early childhood had an alpha, it was definitely you. All the other kids followed your lead as you decided what to play and where to play, and led them in games and mischief.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=5HXS8HEY}your brawn.", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodYourBrawnOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodAthleticsOnApply), new TextObject("{=YKzuGc54}You were big, and other children looked to have you around in any scrap with children from a neighboring village. You pushed a plough and threw an axe like an adult.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=QrYjPUEf}your attention to detail.", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Crafting }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodAttentionToDetailOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMemoryOnApply), new TextObject("{=JUSHAPnu}You were quick on your feet and attentive to what was going on around you. Usually you could run away from trouble, though you could give a good account of yourself in a fight with other children if cornered.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=Y3UcaX74}your aptitude for numbers.", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodAptitudeForNumbersOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMathOnApply), new TextObject("{=DFidSjIf}Most children around you had only the most rudimentary education, but you lingered after class to study letters and mathematics. You were fascinated by the marketplace - weights and measures, tallies and accounts, the chatter about profits and losses.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=GEYzLuwb}your way with people.", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodWayWithPeopleOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodGoodMannersOnApply), new TextObject("{=w2TEQq26}You were always attentive to other people, good at guessing their motivations. You studied how individuals were swayed, and tried out what you learned from adults on your friends.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=MEgLE2kj}your skill with horses.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SandboxCharacterCreationContent.ChildhoodSkillsWithHorsesOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.ChildhoodAffinityWithAnimalsOnApply), new TextObject("{=ngazFofr}You were always drawn to animals, and spent as much time as possible hanging out in the village stables. You could calm horses, and were sometimes called upon to break in new colts. You learned the basics of veterinary arts, much of which is applicable to humans as well.", null), null, 0, 0, 0, 0, 0);

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











        protected void AddEducationMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=rcoueCmk}Adolescence", null), this._educationIntroductoryText, new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=RKVNvimC}herded the sheep.", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHerderOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHerderOnApply), new TextObject("{=KfaqPpbK}You went with other fleet-footed youths to take the villages' sheep, goats or cattle to graze in pastures near the village. You were in charge of chasing down stray beasts, and always kept a big stone on hand to be hurled at lurking predators if necessary.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=bTKiN0hr}worked in the village smithy.", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Crafting }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceSmithyOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceSmithyOnApply), new TextObject("{=y6j1bJTH}You were apprenticed to the local smith. You learned how to heat and forge metal, hammering for hours at a time until your muscles ached.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=tI8ZLtoA}repaired projects.", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceRepairmanOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceRepairmanOnApply), new TextObject("{=6LFj919J}You helped dig wells, rethatch houses, and fix broken plows. You learned about the basics of construction, as well as what it takes to keep a farming community prosperous.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=TRwgSLD2}gathered herbs in the wild.", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceGathererOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceGathererOnApply), new TextObject("{=9ks4u5cH}You were sent by the village healer up into the hills to look for useful medicinal plants. You learned which herbs healed wounds or brought down a fever, and how to find them.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=T7m7ReTq}hunted small game.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHunterOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHunterOnApply), new TextObject("{=RuvSk3QT}You accompanied a local hunter as he went into the wilderness, helping him set up traps and catch small animals.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=qAbMagWq}sold product at the market.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(RuralAdolescenceOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHelperOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHelperOnApply), new TextObject("{=DIgsfYfz}You took your family's goods to the nearest town to sell your produce and buy supplies. It was hard work, but you enjoyed the hubbub of the marketplace.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=nOfSqRnI}at the town watch's training ground.", null), new MBList<SkillObject> { DefaultSkills.Crossbow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceWatcherOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceWatcherOnApply), new TextObject("{=qnqdEJOv}You watched the town's watch practice shooting and perfect their plans to defend the walls in case of a siege.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=8a6dnLd2}with the alley gangs.", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceGangerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceGangerOnApply), new TextObject("{=1SUTcF0J}The gang leaders who kept watch over the slums of Calradian cities were always in need of poor youth to run messages and back them up in turf wars, while thrill-seeking merchants' sons and daughters sometimes slummed it in their company as well.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=7Hv984Sf}at docks and building sites.", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Crafting }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceDockerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=bhdkegZ4}All towns had their share of projects that were constantly in need of both skilled and unskilled labor. You learned how hoists and scaffolds were constructed, how planks and stones were hewn and fitted, and other skills.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kbcwb5TH}in the markets and caravanserais.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceMarketerOnApply), new TextObject("{=lLJh7WAT}You worked in the marketplace, selling trinkets and drinks to busy shoppers.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kbcwb5TH}in the markets and caravanserais.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceMarketerOnApply), new TextObject("{=rmMcwSn8}You helped your family handle their business affairs, going down to the marketplace to make purchases and oversee the arrival of caravans.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=mfRbx5KE}reading and studying.", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=elQnygal}Your family scraped up the money for a rudimentary schooling and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=etG87fB7}with your tutor.", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=hXl25avg}Your family arranged for a private tutor and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=FKpLEamz}caring for horses.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Steward }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=Ghz90npw}Your family owned a few horses at the town stables and you took charge of their care. Many evenings you would take them out beyond the walls and gallup through the fields, racing other youth.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=vH7GtuuK}working at the stables.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Steward }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=csUq1RCC}You were employed as a hired hand at the town's stables. The overseers recognized that you had a knack for horses, and you were allowed to exercise them and sometimes even break in new steeds.", null), null, 0, 0, 0, 0, 0);
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











        protected void AddYouthMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=ok8lSW6M}Youth", null), this._youthIntroductoryText, new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CITG915d}joined a commander's staff.", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(YouthCommanderOnCondition), new CharacterCreationOnSelect(YouthCommanderOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCommanderOnApply), new TextObject("{=Ay0G3f7I}Your family arranged for you to be part of the staff of an imperial strategos. You were not given major responsibilities - mostly carrying messages and tending to his horse -- but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=bhE2i6OU}served as a baron's groom.", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(YouthGroomOnCondition), new CharacterCreationOnSelect(YouthGroomOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGroomOnApply), new TextObject("{=iZKtGI6Y}Your family arranged for you to accompany a minor baron of the Vlandian kingdom. You were not given major responsibilities - mostly carrying messages and tending to his horse -- but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=F2bgujPo}were a chieftain's servant.", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(YouthChieftainOnCondition), new CharacterCreationOnSelect(YouthChieftainOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthChieftainOnApply), new TextObject("{=7AYJ3SjK}Your family arranged for you to accompany a chieftain of your people. You were not given major responsibilities - mostly carrying messages and tending to his horse -- but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=h2KnarLL}trained with the cavalry.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(YouthCavalryOnCondition), new CharacterCreationOnSelect(YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own, but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=zsC2t5Hb}trained with the hearth guard.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(YouthHearthGuardOnCondition), new CharacterCreationOnSelect(YouthHearthGuardOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthHearthGuardOnApply), new TextObject("{=RmbWW6Bm}You were a big and imposing enough youth that the chief's guard allowed you to train alongside them, in preparation to join them some day.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new MBList<SkillObject> { DefaultSkills.Crossbow, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(YouthGarrisonOnCondition), new CharacterCreationOnSelect(YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(YouthOtherGarrisonOnCondition), new CharacterCreationOnSelect(YouthOtherGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=1EkEElZd}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=VlXOgIX6}rode with the scouts.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Bow }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(YouthOutridersOnCondition), new CharacterCreationOnSelect(YouthOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOutridersOnApply), new TextObject("{=888lmJqs}All of Calradia's kingdoms recognize the value of good light cavalry and horse archers, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a good bow so that you could fulfill their levy obligations.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=VlXOgIX6}rode with the scouts.", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Bow }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(YouthOtherOutridersOnCondition), new CharacterCreationOnSelect(YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("{=sYuN6hPD}All of Calradia's kingdoms recognize the value of good light cavalry, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a sheaf of javelins so that you could fulfill their levy obligations.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=a8arFSra}trained with the infantry.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(YouthInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Calradia.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=oMbOIPc9}joined the skirmishers.", null), new MBList<SkillObject> { DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(YouthSkirmisherOnCondition), new CharacterCreationOnSelect(YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=cDWbwBwI}joined the kern.", null), new MBList<SkillObject> { DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(YouthKernOnCondition), new CharacterCreationOnSelect(YouthKernOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthKernOnApply), new TextObject("{=tTb28jyU}Many Battanians fight as kern, versatile troops who could both harass the enemy line with their javelins or join in the final screaming charge once it weakened.", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=GFUggps8}marched with the camp followers.", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(YouthCamperOnCondition), new CharacterCreationOnSelect(YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }
        new protected bool YouthCommanderOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        new protected void YouthCommanderOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_decisive"
            });
        }

        new protected bool YouthGroomOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia" && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        new protected void YouthGroomOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_sharp"
            });
        }

        new protected bool YouthChieftainOnCondition()
        {
            return (base.GetSelectedCulture().StringId == "battania" || base.GetSelectedCulture().StringId == "khuzait") && this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        new protected void YouthChieftainOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_ready"
            });
        }

        new protected bool YouthCavalryOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait" || base.GetSelectedCulture().StringId == "aserai" || base.GetSelectedCulture().StringId == "vlandia";
        }
        new protected void YouthCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_apprentice"
            });
        }

        new protected bool YouthHearthGuardOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia" || base.GetSelectedCulture().StringId == "battania";
        }
        new protected void YouthHearthGuardOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_athlete"
            });
        }

        new protected bool YouthOutridersOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait";
        }
        new protected void YouthOutridersOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_gracious"
            });
        }

        new protected bool YouthOtherOutridersOnCondition()
        {
            return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "khuzait";
        }
        new protected void YouthOtherOutridersOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_gracious"
            });
        }

        new protected void YouthInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_fierce"
            });
        }

        new protected bool YouthSkirmisherOnCondition()
        {
            return base.GetSelectedCulture().StringId != "battania";
        }
        new protected void YouthSkirmisherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_fox"
            });
        }

        new protected bool YouthGarrisonOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "vlandia";
        }
        new protected void YouthGarrisonOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_vibrant"
            });
        }

        new protected bool YouthOtherGarrisonOnCondition()
        {
            return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "vlandia";
        }
        new protected void YouthOtherGarrisonOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_sharp"
            });
        }

        new protected bool YouthKernOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        new protected void YouthKernOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_apprentice"
            });
        }

        new protected bool YouthCamperOnCondition()
        {
            return this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        new protected void YouthCamperOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>
            {
                "act_childhood_militia"
            });
        }











        protected void AddAdulthoodMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=MafIe9yI}Young Adulthood", null), new TextObject("{=4WYY0X59}Before you set out for a life of adventure, your biggest achievement was...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=8bwpVpgy}you defeated an enemy in battle.", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(AccomplishmentDefeatedEnemyOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentDefeatedEnemyOnApply), new TextObject("{=1IEroJKs}Not everyone who musters for the levy marches to war, and not everyone who goes on campaign sees action. You did both, and you also took down an enemy warrior in direct one-to-one combat, in the full view of your comrades.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=mP3uFbcq}you led a successful manhunt.", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentPosseOnConditions), new CharacterCreationOnSelect(AccomplishmentExpeditionOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=4f5xwzX0}When your community needed to organize a posse to pursue horse thieves, you were the obvious choice. You hunted down the raiders, surrounded them and forced their surrender, and took back your stolen property.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=wfbtS71d}you led a caravan.", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentMerchantOnCondition), new CharacterCreationOnSelect(AccomplishmentMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=joRHKCkm}Your family needed someone trustworthy to take a caravan to a neighboring town. You organized supplies, ensured a constant watch to keep away bandits, and brought it safely to its destination.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=x1HTX5hq}you saved your village from a flood.", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentSavedVillageOnCondition), new CharacterCreationOnSelect(AccomplishmentSavedVillageOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=bWlmGDf3}When a sudden storm caused the local stream to rise suddenly, your neighbors needed quick-thinking leadership. You provided it, directing them to build levees to save their homes.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=s8PNllPN}you saved your city quarter from a fire.", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentSavedStreetOnCondition), new CharacterCreationOnSelect(AccomplishmentSavedStreetOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=ZAGR6PYc}When a sudden blaze broke out in a back alley, your neighbors needed quick-thinking leadership and you provided it. You organized a bucket line to the nearest well, putting the fire out before any homes were lost.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=xORjDTal}you invested some money in a workshop.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Crafting }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=PyVqDLBu}Your parents didn't give you much money, but they did leave just enough for you to secure a loan against a larger amount to build a small workshop. You paid back what you borrowed, and sold your enterprise for a profit.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=xKXcqRJI}you invested some money in land.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Crafting }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=cbF9jdQo}Your parents didn't give you much money, but they did leave just enough for you to purchase a plot of unused land at the edge of the village. You cleared away rocks and dug an irrigation ditch, raised a few seasons of crops, than sold it for a considerable profit.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=TbNRtUjb}you hunted a dangerous animal.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=I3PcdaaL}Wolves, bears are a constant menace to the flocks of northern Calradia, while hyenas and leopards trouble the south. You went with a group of your fellow villagers and fired the missile that brought down the beast.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=WbHfGCbd}you survived a siege.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=FhZPjhli}Your hometown was briefly placed under siege, and you were called to defend the walls. Everyone did their part to repulse the enemy assault, and everyone is justly proud of what they endured.", null), null, 0, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=kNXet6Um}you had a famous escapade in town.", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(AccomplishmentEscapadeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentEscapadeOnApply), new TextObject("{=DjeAJtix}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, on one of your trips into town you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=qlOuiKXj}you had a famous escapade.", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(AccomplishmentEscapadeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentEscapadeOnApply), new TextObject("{=lD5Ob3R4}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 5, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=Yqm0Dics}you treated people well.", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AccomplishmentTreaterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentTreaterOnApply), new TextObject("{=dDmcqTzb}Yours wasn't the kind of reputation that local legends are made of, but it was the kind that wins you respect among those around you. You were consistently fair and honest in your business dealings and helpful to those in trouble. In doing so, you got a sense of what made people tick.", null), new MBList<TraitObject> { DefaultTraits.Mercy, DefaultTraits.Generosity, DefaultTraits.Honor }, 1, 5, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        new protected bool AccomplishmentRuralOnCondition()
        {
            return this.RuralType();
        }
        new protected bool AccomplishmentMerchantOnCondition()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Merchant;
        }
        new protected bool AccomplishmentPosseOnConditions()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Herder || this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Mercenary;
        }
        new protected bool AccomplishmentSavedVillageOnCondition()
        {
            return this.RuralType() && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Herder;
        }
        new protected bool AccomplishmentSavedStreetOnCondition()
        {
            return !this.RuralType() && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Merchant && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Mercenary;
        }
        new protected bool AccomplishmentUrbanOnCondition()
        {
            return !this.RuralType();
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

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}20", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeYoungOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeYoungOnApply), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), null, 0, 0, 0, 2, 1);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}30", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeAdultOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeAdultOnApply), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), null, 0, 0, 0, 4, 2);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}40", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeMiddleAgedOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeMiddleAgedOnApply), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), null, 0, 0, 0, 6, 3);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}50", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeElderlyOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeElderlyOnApply), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), null, 0, 0, 0, 8, 4);
            characterCreation.AddNewMenu(characterCreationMenu);
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
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.YoungAdult;
            this.SetHeroAge(20f);
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
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Adult;
            this.SetHeroAge(30f);
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
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.MiddleAged;
            this.SetHeroAge(40f);
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
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Elder;
            this.SetHeroAge(50f);
        }
        new protected void StartingAgeYoungOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.YoungAdult;
        }
        new protected void StartingAgeAdultOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Adult;
        }
        new protected void StartingAgeMiddleAgedOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.MiddleAged;
        }
        new protected void StartingAgeElderlyOnApply(CharacterCreation characterCreation)
        {
            this._startingAge = SandboxCharacterCreationContent.SandboxAgeOptions.Elder;
        }
    }
}
