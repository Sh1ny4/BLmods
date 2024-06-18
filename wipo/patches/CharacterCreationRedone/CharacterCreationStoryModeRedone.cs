using HarmonyLib;
using StoryMode.CharacterCreationContent;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using StoryMode;

namespace wipo.patches.CharacterCreationPatch
{
    //this github does not contain the needed XML for the starting equipement, at least for now.
    //feel free to use it as a base for your mod, but while it is functionnal, it also is far from being done atm, a LOT of polishing has to be done.
    //If you have any idea for improvement please do share it, I am quite lacking in the idea department tbh
    [HarmonyPatch(typeof(StoryModeCharacterCreationContent), "OnInitialized")]
    public class CharacterCreationStoryModeRedone : StoryModeCharacterCreationContent
    {
        //patches the function that create the menus and replaces it to mine, which inits my menus
        [HarmonyPrefix]
        static bool Prefix(ref CharacterCreationStoryModeRedone __instance, CharacterCreation characterCreation)
        {
            __instance.AddMenus(characterCreation);
            return false;
        }
        // if you want to add/remove menus you can do it here and copy/paste one of the other menus, then tweak it to your liking
        public void AddMenus(CharacterCreation characterCreation)
        {
            AddParentsMenuPatch(characterCreation);
            AddEducationMenuPatch(characterCreation);
            AddChildhoodMenuPatch(characterCreation);
            AddYouthMenuPatch(characterCreation);
            AddAdulthoodMenuPatch(characterCreation);
            AddEscapeMenuPatch(characterCreation);
        }

        //first menu : family you were born in, divided per culture in submenus. I haven't been able to redo the same structure in other menu (haven't tried much I admit), so I replaced the submenus structure with a culture specific condition in each option instead
        public void AddParentsMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Family", null), new TextObject("{=!}Your were born into a family of..", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            //AddCategoryOption can change :
            // in which skill you level, how many focus point you put in and how many level you gain in it, for up to three skill. Haven't tried more but it would most likely require an edit of the function that displays what you gain.
            // in which attribute you gain levels and how much you gain
            // the conditions for the menu option to appear, so far I got the culture and the type of parents, have yet to try for the player's gender
            // the consequences of the choice, that's where you can change the equipement (more on that later), the animation. You might be able to change some other things like joining a faction in it but I have yet to try it.
            //      You can also have some conditions in it to avoid creating dozens of new menu entries for the same idea, can be useful when creating the starting equipement for example (being a soldier coming from a noble family means better equipment than when being commoner, can heavily reduce the workload )
            // Onapply, have yet to play with it but depending on the step you're in it either does nothing (like in my education menu) or everything (like in the age menu), be carefull with this one
            // the last ones are : the traits to level in a list, by how much, then comes gained renown, gained gold, free & unassigned focus point and attribute point.
            CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(AseraiParentsOnCondition));
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_ARais}rais", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiRaisOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiTribesmanOnApply), new TextObject("{=!}", null), null, 0, 150, 3000, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AMamluks}mamluks", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiMamluksOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiWariorSlaveOnApply), new TextObject("{=!}", null), null, 0, 20, -600, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AMerchant}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiMerchantsOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 4000, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AFarmers}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiFarmersOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiOasisFarmerOnApply), new TextObject("{=!}", null), null, 0, 0, -900, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AArtisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiArtisansOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBedouinOnApply), new TextObject("{=!}", null), null, 0, 10, 800, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AThugs}thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiThugOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBackAlleyThugOnApply), new TextObject("{=!}", null), null, 0, 0, -800, 0, 0);

            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(BattanianParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bchieftains}chieftains", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaChieftainsOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaChieftainsHearthguardOnApply), new TextObject("{=!}", null), null, 0, 150, 1000, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bhealers}healers", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaHealersOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaHealerOnApply), new TextObject("{=!}", null), null, 50, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bfarmers}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Crafting }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaFarmersOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaTribesmanOnApply), new TextObject("{=!}", null), null, 0, -800, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaArtisansOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaSmithOnApply), new TextObject("{=!}", null), null, 0, 10, 500, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bforesters}foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaForesterOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaWoodsmanOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bbards}bards", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaBardsOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaBardOnApply), new TextObject("{=!}", null), null, 0, 20, -200, 0, 0);

            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(EmpireParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Earistocrates}aristocrates", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireAristocratesOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireLandlordsRetainerOnApply), new TextObject("{=!}", null), null, 0, 150, 2000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Emerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireMerchantsOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 6000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Efreeholders}freeholders", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireFreeholdersOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireFreeholderOnApply), new TextObject("{=!}", null), null, 0, 0, -600, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Eartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireArtisansOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireArtisanOnApply), new TextObject("{=!}", null), null, 0, 20, 1000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Esoldiers}soldiers", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireSoldiersOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireWoodsmanOnApply), new TextObject("{=!}", null), null, 0, 40, 500, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Evagabonds}vagabonds", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireVagabondsOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireVagabondOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);

            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(KhuzaitParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Knoyans}noyans", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNoyansOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNoyansKinsmanOnApply), new TextObject("{=!}", null), null, 0, 150, 1000, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Knomads}nomads", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNomadsOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitTribesmanOnApply), new TextObject("{=!}", null), null, 0, 50, -300, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kmerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitMerchantsOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 800, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitFarmersOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitFarmerOnApply), new TextObject("{=!}", null), null, 0, 20, 400, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kwarriors}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitWarriorsOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitShamanOnApply), new TextObject("{=!}", null), null, 0, 30, 700, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kthugs}thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitThugsOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNomadOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);

            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(SturgianParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sboyars}boyars", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaBoyarsOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaBoyarsCompanionOnApply), new TextObject("{=!}", null), null, 0, 150, 2000, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Smerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaMerchantsOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaTraderOnApply), new TextObject("{=!}", null), null, 0, 0, 2500, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sfarmers}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaFarmersOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaFreemanOnApply), new TextObject("{=!}", null), null, 0, 0, -800, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaArtisansOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaArtisanOnApply), new TextObject("{=!}", null), null, 0, 20, 1200, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Swarriors}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaWarriorsOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaHunterOnApply), new TextObject("{=!}", null), null, 0, 60, 600, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sraiders}raiders", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaRaiderOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaVagabondOnApply), new TextObject("{=!}", null), null, 0, 40, 1000, 0, 0);

            CharacterCreationCategory characterCreationCategory6 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(VlandianParentsOnCondition));
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vbarons}barons", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBaronsOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBaronsRetainerOnApply), new TextObject("{=!}", null), null, 0, 150, 3000, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vmerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMerchantsOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMerchantOnApply), new TextObject("{=!}", null), null, 0, 20, 5000, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vyeomens}yeomen", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaYeomensOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaYeomanOnApply), new TextObject("{=!}", null), null, 0, 0, -800, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaArtisansOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBlacksmithOnApply), new TextObject("{=!}", null), null, 0, 30, 500, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vsoldiers}soldiers", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaSoldiersOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaHunterOnApply), new TextObject("{=!}", null), null, 0, 20, 200, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vmercenaries}mercenaries", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMercenariesOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMercenaryOnApply), new TextObject("{=!}.", null), null, 0, 40, 600, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }


        
        //sets the parents job, I use hunter as warrior
        new public bool AseraiParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai";
        }
        public void AseraiRaisOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, StoryModeCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        public void AseraiMamluksOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, StoryModeCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        public void AseraiMerchantsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, StoryModeCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        public void AseraiFarmersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, StoryModeCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        public void AseraiArtisansOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, StoryModeCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        public void AseraiThugOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, StoryModeCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }


        new public bool BattanianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        public void BattaniaChieftainsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, StoryModeCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        public void BattaniaHealersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, StoryModeCharacterCreationContent.OccupationTypes.Healer, "", "", true, true);
        }
        public void BattaniaFarmersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, StoryModeCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        public void BattaniaArtisansOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, StoryModeCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        public void BattaniaForesterOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, StoryModeCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }
        public void BattaniaBardsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, StoryModeCharacterCreationContent.OccupationTypes.Bard, "", "", true, true);
        }


        new public bool EmpireParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire";
        }
        public void EmpireAristocratesOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, StoryModeCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        public void EmpireMerchantsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, StoryModeCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        public void EmpireFreeholdersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, StoryModeCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        public void EmpireArtisansOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, StoryModeCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        public void EmpireSoldiersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, StoryModeCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        public void EmpireVagabondsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, StoryModeCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }


        new public bool KhuzaitParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait";
        } 
        public void KhuzaitNoyansOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, StoryModeCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        public void KhuzaitNomadsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, StoryModeCharacterCreationContent.OccupationTypes.Herder, "", "", true, true);
        }
        public void KhuzaitMerchantsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, StoryModeCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        public void KhuzaitWarriorsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, StoryModeCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        public void KhuzaitFarmersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, StoryModeCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        public void KhuzaitThugsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, StoryModeCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }


        new public bool SturgianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia";
        }
        public void SturgiaBoyarsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, StoryModeCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        public void SturgiaMerchantsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, StoryModeCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        public void SturgiaFarmersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, StoryModeCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        public void SturgiaArtisansOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, StoryModeCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        public void SturgiaWarriorsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, StoryModeCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        public void SturgiaRaiderOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, StoryModeCharacterCreationContent.OccupationTypes.Vagabond, "", "", true, true);
        }


        new public bool VlandianParentsOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia";
        }
        public void VlandiaBaronsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 1, StoryModeCharacterCreationContent.OccupationTypes.Retainer, "", "", true, true);
        }
        public void VlandiaMerchantsOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 2, StoryModeCharacterCreationContent.OccupationTypes.Merchant, "", "", true, true);
        }
        public void VlandiaYeomensOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 3, StoryModeCharacterCreationContent.OccupationTypes.Farmer, "", "", true, true);
        }
        public void VlandiaArtisansOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 4, StoryModeCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
        }
        public void VlandiaSoldiersOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 5, StoryModeCharacterCreationContent.OccupationTypes.Hunter, "", "", true, true);
        }
        public void VlandiaMercenariesOnConsequence(CharacterCreation characterCreation)
        {
            this.SetParentAndOccupationType(characterCreation, 6, StoryModeCharacterCreationContent.OccupationTypes.Mercenary, "", "", true, true);
        }










        // I changed the adolescence menu into a menu that specifies the education the parent gave to the player
        public void AddEducationMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Received education", null), new TextObject("{=!}Your parents wanted you to...", null), new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Faris}become a faris", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Athletics, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AseraiRetainerOnCondition), new CharacterCreationOnSelect(EdNobleTroopOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Hearthguard}enter the hearthguard", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaRetainerOnCondition), new CharacterCreationOnSelect(EdNobleTroopOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Cataphract}become a cataphract", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(EmpireRetainerOnCondition), new CharacterCreationOnSelect(EdNobleTroopOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_KhanGuard}enter a khan's guard", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Athletics, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitRetainerOnCondition), new CharacterCreationOnSelect(EdNobleTroopOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Druzhina}join a druzhina", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaRetainerOnCondition), new CharacterCreationOnSelect(EdNobleTroopOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Knight}become a knight", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(VlandiaRetainerOnCondition), new CharacterCreationOnSelect(EdNobleTroopOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Commander}lead armies", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsRetainerOnCondition), new CharacterCreationOnSelect(EdLeadTroopsOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_Court}be part of the court", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Roguery, DefaultSkills.Tactics }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsRetainerOnCondition), new CharacterCreationOnSelect(EdCourtLifeOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_trade}become a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(EdMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_crafting}learn a trade", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EdArtisanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_scholar}become a scholar", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Engineering, DefaultSkills.Tactics }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EdScholarOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_religious}be a {?PLAYER.GENDER}lady{?}man{\\?} of faith", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EdReligiousOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_farmer}tend to the fields", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Medicine, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EdFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_lady}become a lady in waiting", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonersFemaleOnConditions), new CharacterCreationOnSelect(EdLadyOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_defense}learn how to defend yourself", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonersMaleOnConditions), new CharacterCreationOnSelect(EdDefenseOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_trickery}know how to trick others", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonerOnCondition), new CharacterCreationOnSelect(EdTrickeryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        public bool ParentsCommonerOnCondition()
        {
            return this._familyOccupationType != StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }

        public bool ParentsRetainerOnCondition()
        {
            return this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public bool ParentsCommonersFemaleOnConditions() 
        {
            return (Hero.MainHero.IsFemale && this._familyOccupationType != StoryModeCharacterCreationContent.OccupationTypes.Retainer);
        }
        public bool ParentsCommonersMaleOnConditions()
        {
            return (!Hero.MainHero.IsFemale && this._familyOccupationType != StoryModeCharacterCreationContent.OccupationTypes.Retainer);
        }
        public bool ParentsRetainerFemaleOnConditions()
        {
            return (Hero.MainHero.IsFemale && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer);
        }
        public bool ParentsRetainerMaleOnConditions()
        {
            return (!Hero.MainHero.IsFemale && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer);
        }


        public void EdNobleTroopOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdLeadTroopsOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_leader" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdCourtLifeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_manners" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdMerchantOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_peddlers" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdArtisanOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdScholarOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdReligiousOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_closed_tutor" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdFarmerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_peddlers" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdLadyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_manners" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdDefenseOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        public void EdTrickeryOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_streets" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }










        //this one is subject to change, I went for the idiom as it was in a game I played, but tbh I consider removing it altogether a good option
        // I kept it because it seems to me that having 5 steps is nice, to get 5 focus points
        public void AddChildhoodMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Idioms", null), new TextObject("{=!}Growing up, you were inculcated the saying...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            // noble fighter
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(AseraiRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(EmpireRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            // commoner fighter
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_healthy}A healthy mind in a healthy body.", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Medicine }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomHealthyBodyOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_prevention}Prevention is better than cure.", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomPreventionOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_wellbegun}Well begun is half done.", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomWellBegunOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_bold}Fortune favors the bold.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Tactics }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomBoldOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_forwarned}Forewarned is forearmed.", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Tactics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomForwarnedOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_invention}Necessity is the mother of invention.", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Trade }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomInventionOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_armed}Men with weapons never starve.", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomArmedOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_conquer}To conquer without risk is to triumph without glory.", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomRiskOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_means}The end justifies the means.", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomMeansOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }

        public static void IdiomFighterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_honor" });
        }
        public static void IdiomHealthyBodyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tactician" });
        }
        public static void IdiomPreventionOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_sharp" });
        }
        public static void IdiomWellBegunOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_appearances" });
        }
        public static void IdiomBoldOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_leader_2" });
        }
        public static void IdiomForwarnedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        public static void IdiomInventionOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
        }
        public static void IdiomArmedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_fierce" });
        }
        public static void IdiomRiskOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_confident_tutor" });
        }
        public static void IdiomMeansOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_fox" });
        }










        // here we choose the starting equipment alongside the start in life. as I said earlier I used culture specific conditions to have culture specific jobs when I could think of them
        // structure is basically noble troop, culture unique job, merchant, craftman, farmer, type of troop made according to my troop mod (https://www.nexusmods.com/mountandblade2bannerlord/mods/4932 , which is why some skills/names might be odd) and then criminals
        public void AddYouthMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Start in life", null), new TextObject("{=!}You started your life as..", null), new CharacterCreationOnInit(YouthOnInitDebug), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            //Aserai
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Afaris}a faris", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.OneHanded, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(AseraiRetainerOnCondition), new CharacterCreationOnSelect(YouthAseraiFarisOnConsequence), new CharacterCreationApplyFinalEffects(NobleOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Acaravaner}a caravaner", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Trade, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiCaravanerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Amerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Acraftman}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Afarmer}a farmer", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Amamluke}mamluke", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiSlaveWarriorOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ahorsearcher}mounted archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiMountedArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Aarcher}archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Adesertbandit}a desert bandit", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(AseraiOnCondition), new CharacterCreationOnSelect(YouthAseraiBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Battania
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bfian}a member of a fianna", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaRetainerOnCondition), new CharacterCreationOnSelect(YouthBattaniaFiannaOnConsequence), new CharacterCreationApplyFinalEffects(NobleOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bdruid}a druid", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Scouting, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaDruidOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bmerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward,DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBataniaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bcraftman}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade,DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bforester}a forester", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Scouting, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattanniaForesterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bwildling}wildling", null), new MBList<SkillObject> { DefaultSkills.Throwing, DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaFalxOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bscout}scout", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Throwing, DefaultSkills.Polearm }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaScoutOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bkern}part of the kern", null), new MBList<SkillObject> { DefaultSkills.Throwing, DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaKernOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Bforestbandit}a forest bandit", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(BattaniaOnCondition), new CharacterCreationOnSelect(YouthBattaniaBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Empire
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ecommander}a centurion", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Tactics,DefaultSkills.Leadership }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(EmpireRetainerOnCondition), new CharacterCreationOnSelect(YouthEmpireCommanderOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Eengineer}an engineer's apprentice", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Steward, DefaultSkills.Crafting }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireEngineerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Emerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ecraftman}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Epeasant}a peasant", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Elegionary}legionary", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireLegionaryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Earcher}archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ecavalry}light cavalry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireCavalryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ehorsearcher}mounted archer", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireHorseArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Elooter}a looter", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(EmpireOnCondition), new CharacterCreationOnSelect(YouthEmpireBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Khuzait
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Kkhanguard}part of a khan's guard", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm, DefaultSkills.Bow }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitRetainerOnCondition), new CharacterCreationOnSelect(YouthKhuzaitKhansGuardOnConsequence), new CharacterCreationApplyFinalEffects(NobleOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Knomad}a nomad", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Bow, DefaultSkills.Scouting }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Kmerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Kcraftman}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Kfarmer}a farmer", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Kcavalry}part of the cavalry", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitCavalryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Khorsearcher}mounted archer", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitHorseArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Kinfantry}infantry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitInfantryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ksteppebandit}a steppe bandit", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitOnCondition), new CharacterCreationOnSelect(YouthKhuzaitBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Sturgia
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sdruzhina}a member of a druzhina", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaRetainerOnCondition), new CharacterCreationOnSelect(YouthSturgiaDruzhinaOnConsequence), new CharacterCreationApplyFinalEffects(NobleOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sfur_hunter}fur hunter", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Scouting, DefaultSkills.Trade }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Smerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Scraftman}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Speasant}a peasant", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sinfantry}part of the infantry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaInfantryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sshocktroop}shock troop", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaShockTroopOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sarcher}bowman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ssearaider}a raider", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Roguery, DefaultSkills.Scouting }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            //Vlandia
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vknight}a knight", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaRetainerOnCondition), new CharacterCreationOnSelect(YouthVlandiaKnightOnConsequence), new CharacterCreationApplyFinalEffects(NobleOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vchamberlain}chamberlain", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaChamberlainOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vmerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vguild}a member of a guild", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaGuildOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vserf}a serf", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaSerfOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vinfantry}levied footman", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaInfantryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vcavalry}light cavalry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaLightCavalryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vcrossbowman}levied crossbowman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Crossbow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiaCrossbowmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Vbandit}a highwayman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Scouting, DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(VlandiaOnCondition), new CharacterCreationOnSelect(YouthVlandiBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            
            characterCreation.AddNewMenu(characterCreationMenu);
        }


        public void YouthOnInitDebug(CharacterCreation characterCreation)
        {
            TextObject _youthText = new TextObject("{=!}{YOUTH_INTRO}", null);
            characterCreation.IsPlayerAlone = true;
            characterCreation.HasSecondaryCharacter = false;
            characterCreation.ClearFaceGenPrefab();
            TextObject textObject = new TextObject("{=F7OO5SAa}As a youngster growing up in Calradia, war was never too far away. You...");
            TextObject textObject2 = new TextObject("{=5kbeAC7k}In wartorn Calradia, especially in frontier or tribal areas, some women as well as men learn to fight from an early age. You...");
            _youthText.SetTextVariable("YOUTH_INTRO", CharacterObject.PlayerCharacter.IsFemale ? textObject2 : textObject);
            characterCreation.ChangeFaceGenChars(ChangePlayerFaceWithAge(YouthAge));
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            if (base.SelectedTitleType < 1 || base.SelectedTitleType > 10)
            {
                base.SelectedTitleType = 1;
            }

            RefreshPlayerAppearance(characterCreation);
        }
        public void NothingOnApply(CharacterCreation characterCreation)
        {
        }
        public void BanditOnApply(CharacterCreation characterCreation)
        {
            foreach (Kingdom kingdom in Kingdom.All)
            {
                ChangeCrimeRatingAction.Apply(kingdom.MapFaction, 50, false);
            }
        }
        public void NobleOnApply(CharacterCreation characterCreation)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, false);
            CharacterObject wanderer = (from character in CharacterObject.All where character.Occupation == Occupation.Wanderer && character.Culture == Hero.MainHero.Culture select character).GetRandomElementInefficiently();
            Hero companion = HeroCreator.CreateSpecialHero(wanderer);
            AddCompanionAction.Apply(Clan.PlayerClan, companion);
            AddHeroToPartyAction.Apply(companion, Hero.MainHero.PartyBelongedTo);
        }


        // the way the starting gears are chosen is in the sanbdbox > sandbox_equipment_sets xml , and it work this way : 
        // player_char_creation_<the culture you chose>_<the TitleType you put in the consequences below>_<the gender, m or f>, it's done in RefreshPlayerAppearance
        // so far I have ten for the empire and it doesn't seem to be too much, I think you can go crazy and have dozens of it
        // I haven't been able to remove gold to have an equipement cost something to the player in the AddCategoryOption so either it will have to wait until I patch it or it has to be done in the consequences
        public bool AseraiOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai";
        }
        public bool AseraiRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "aserai" && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public void YouthAseraiFarisOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_hardened" });
        }
        public void YouthAseraiCaravanerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_leader" });
        }
        public void YouthAseraiMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        public void YouthAseraiCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        public void YouthAseraiFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        public void YouthAseraiSlaveWarriorOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tough" });
        }
        public void YouthAseraiMountedArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthAseraiArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthAseraiBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }


        public bool BattaniaOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania";
        }
        public bool BattaniaRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "battania" && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public void YouthBattaniaFiannaOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_sharp" });
        }
        public void YouthBattaniaDruidOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_schooled" });
        }
        public void YouthBataniaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        public void YouthBattaniaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        public void YouthBattanniaForesterOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthBattaniaFalxOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_fierce" });
        }
        public void YouthBattaniaScoutOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        public void YouthBattaniaKernOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready_throw" });
        }
        public void YouthBattaniaBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }


        public bool EmpireOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire";
        }
        public bool EmpireRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "empire" && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public void YouthEmpireCommanderOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tactician" });
        }
        public void YouthEmpireEngineerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_apprentice" });
        }
        public void YouthEmpireMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        public void YouthEmpireCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        public void YouthEmpireFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        public void YouthEmpireLegionaryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_defend" });
        }
        public void YouthEmpireArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthEmpireCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        public void YouthEmpireHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthEmpireBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }


        public bool KhuzaitOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait";
        }
        public bool KhuzaitRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "khuzait" && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public void YouthKhuzaitKhansGuardOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tough" });
        }
        public void YouthKhuzaitNomadOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        public void YouthKhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        public void YouthKhuzaitCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        public void YouthKhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        public void YouthKhuzaitCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_polearm" });
        }
        public void YouthKhuzaitHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthKhuzaitInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthKhuzaitBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }


        public bool SturgiaOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia";
        }
        public bool SturgiaRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "sturgia" && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public void YouthSturgiaDruzhinaOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tough" });
        }
        public void YouthSturgiaHunterOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        public void YouthSturgiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        public void YouthSturgiaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        public void YouthSturgiaFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        public void YouthSturgiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_defend" });
        }
        public void YouthSturgiaShockTroopOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_fierce" });
        }
        public void YouthSturgiaArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        public void YouthSturgiaBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }


        public bool VlandiaOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia";
        }
        public bool VlandiaRetainerOnCondition()
        {
            return base.GetSelectedCulture().StringId == "vlandia" && this._familyOccupationType == StoryModeCharacterCreationContent.OccupationTypes.Retainer;
        }
        public void YouthVlandiaKnightOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_honor" });
        }
        public void YouthVlandiaChamberlainOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_schooled" });
        }
        public void YouthVlandiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        public void YouthVlandiaGuildOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_apprentice" });
        }
        public void YouthVlandiaSerfOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        public void YouthVlandiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_defend" });
        }
        public void YouthVlandiaLightCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        public void YouthVlandiaCrossbowmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        public void YouthVlandiBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }










        // here is where I give the player traits. As said earlier I can not give them a negative value yet, will have to wait
        public void AddAdulthoodMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Reason for Adventuring", null), new TextObject("{=!}You started adventuring...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_travel}to discover the world", null), new MBList<SkillObject> { DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonDiscoverTheWorldOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_travel}The temptation of travel was to much for you, as you always dreamt of seeing the world.", null), null, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_revenge}to take revenge", null), new MBList<SkillObject> { DefaultSkills.Tactics }, DefaultCharacterAttributes.Vigor, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonTakeRevengeOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_revenge}After being wronged, you felt the need for revenge. With that goal in mind, you wandered throughout Calradia to get reparations", null), new MBList<TraitObject> { DefaultTraits.Mercy }, -1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_forced_out}after being forced out", null), new MBList<SkillObject> { DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonForcedOutOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_forced_out}After one last disagreement with you, your parents forced you out. With nowhere to go, adventuring was all you could do", null), new MBList<TraitObject> { DefaultTraits.Honor }, -1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_money}in search of money", null), new MBList<SkillObject> { DefaultSkills.Trade}, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonSearchForMoneyOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_money}You always wanted to make riches, and it was obvious for you that staying at home would never allow you do it.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_power}to become one of the powerful", null), new MBList<SkillObject> { DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonBecomeNobleOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_power}Seeing how those in power had many advantages, you joined on an adventure to join them, or even replace them.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_history}to mark history", null), new MBList<SkillObject> { DefaultSkills.Charm  }, DefaultCharacterAttributes.Social, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonMarkHistorydOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_history}With all the wars in Calradia, there are many way one could carve {?PLAYER.GENDER}her{?}his{\\?} name in history.", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_loss}after the loss of a loved one", null), new MBList<SkillObject> { DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonLossLovedOneOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_loss}After losing some close to you, you left to see if you could fill that hole", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_crafting}to practice your trade", null), new MBList<SkillObject> { DefaultSkills.Crafting }, DefaultCharacterAttributes.Endurance, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonPracticeTradeOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_crafting}Your craftmanship has been an important part of your life, but your skill wasn't enough. You thus decided to embark on a journey to improve at it.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_helping}to help those in need", null), new MBList<SkillObject> { DefaultSkills.Medicine }, DefaultCharacterAttributes.Intelligence, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonHelpOthersOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_helping}Having been taught in medicine, you decide to set out and help those in need, as with the wars many have suffered.", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 2, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Reason_prove_worth}to prove your fighting skills", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 20, 2, null, new CharacterCreationOnSelect(ReasonProveFightingSkillOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=CCR_Reason_desc_prove_worth}Attaching much importance to your fighting skill, you figured there was no better place than calradia to prove your might.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 0, 5, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }



        public void ReasonDiscoverTheWorldOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_explorer" });
        }
        public void ReasonTakeRevengeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_hardened" });
        }
        public void ReasonForcedOutOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_streets" });
        }
        public void ReasonSearchForMoneyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_numbers" });
        }
        public void ReasonBecomeNobleOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
        }
        public void ReasonMarkHistorydOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_leader_2" });
        }
        public void ReasonLossLovedOneOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        public void ReasonPracticeTradeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
        }
        public void ReasonHelpOthersOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
        }
        public void ReasonProveFightingSkillOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_fierce" });
        }




        public void AddEscapeMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", SkillLevelToAdd);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=peNBA0WW}Story Background"), new TextObject("{=jg3T5AyE}Like many families in Calradia, your life was upended by war. Your home was ravaged by the passage of army after army. Eventually, you sold your property and set off with your father, mother, brother, and your two younger siblings to a new town you'd heard was safer. But you did not make it. Along the way, the inn at which you were staying was attacked by raiders. Your parents were slain and your two youngest siblings seized, but you and your brother survived because..."), EscapeOnInit);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory();
            characterCreationCategory.AddCategoryOption(new TextObject("{=6vCHovVH}you subdued a raider."), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, EscapeSubdueRaiderOnConsequence, EscapeSubdueRaiderOnApply, new TextObject("{=CvBoRaFv}You were able to grab a knife in the confusion of the attack. You stabbed a raider blocking your way."));
            characterCreationCategory.AddCategoryOption(new TextObject("{=2XhW49TX}you drove them off with arrows."), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Tactics }, DefaultCharacterAttributes.Control, 1, 30, 2, null, EscapeDrawArrowsOnConsequence, EscapeDrawArrowsOnApply, new TextObject("{=ccf67J3J}You grabbed a bow and sent a few arrows the raiders' way. They took cover, giving you the opportunity to flee with your brother."));
            characterCreationCategory.AddCategoryOption(new TextObject("{=gOI8lKcl}you rode off on a fast horse."), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, 2, 30, 2, null, EscapeFastHorseOnConsequence, EscapeFastHorseOnApply, new TextObject("{=cepWNzEA}Jumping on the two remaining horses in the inn's burning stable, you and your brother broke out of the encircling raiders and rode off."));
            characterCreationCategory.AddCategoryOption(new TextObject("{=EdUppdLZ}you tricked the raiders."), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Tactics }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, EscapeRoadOffWithBrotherOnConsequence, EscapeRoadOffWithBrotherOnApply, new TextObject("{=ZqOvtLBM}In the confusion of the attack you shouted that someone had found treasure in the back room. You then made your way out of the undefended entrance with your brother."));
            characterCreationCategory.AddCategoryOption(new TextObject("{=qhAhPWdp}you organized the travelers to break out."), new MBList<SkillObject> { DefaultSkills.Leadership, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, EscapeOrganizeTravellersOnConsequence, EscapeOrganizeTravellersOnApply, new TextObject("{=Lmfi0cYk}You encouraged the few travellers in the inn to break out in a coordinated fashion. Raiders killed or captured most but you and your brother were able to escape."));
            characterCreation.AddNewMenu(characterCreationMenu);
        }
    }
}
