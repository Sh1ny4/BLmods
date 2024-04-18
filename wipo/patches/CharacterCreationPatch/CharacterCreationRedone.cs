using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace wipo.patches.CharacterCreationPatch
{
    //this github does not contain the needed XML for the starting equipement, at least for now.
    //feel free to use it as a base for your mod, but while it is functionnal, it also is far from being done atm, a LOT of polishing has to be done.
    //If you have any idea for improvement please do share it, I am quite lacking in the idea department tbh
    [HarmonyPatch(typeof(SandboxCharacterCreationContent), "OnInitialized")]
    public class CharacterCreationRedone : SandboxCharacterCreationContent
    {
        //patches the function that create the menus and replaces it to mine, which inits my menus
        [HarmonyPrefix]
        static bool Prefix(ref CharacterCreationRedone __instance, CharacterCreation characterCreation)
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
            AddAgeSelectionMenuPatch(characterCreation);
        }

        //first menu : family you were born in, divided per culture in submenus. I haven't been able to redo the same structure in other menu (haven't tried much I admit), so I replaced the submenus structure with a culture specific condition in each option instead
        protected void AddParentsMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Family", null), new TextObject("{=!}Your were born into a family of..", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            //AddCategoryOption can change :
            // in which skill you level, how many focus point you put in and how many level you gain in it, for up to three skill. Haven't tried more but it would most likely require an edit of the function that displays what you gain.
            // in which attribute you gain levels and how much you gain
            // the conditions for the menu option to appear, so far I got the culture and the type of parents, have yet to try for the player's gender
            // the consequences of the choice, that's where you can change the equipement (more on that later), the animation. You might be able to change some other things like joining a faction in it but I have yet to try it.
            //      You can also have some conditions in it to avoid creating dozens of new menu entries for the same idea, can be useful when creating the starting equipement for example (being a soldier coming from a noble family means better equipment than when being commoner, can heavily reduce the workload )
            // Onapply, have yet to play with it but depending on the step you're in it either does nothing (like in my education menu) or everything (like in the age menu), be carefull with this one
            // the last ones are : the traits to level in a list, by how much, then comes gained renown, gained gold, free & unassigned focus point and attribute point
            // I tried to give negative values to some of these like gold, renown or trait level but it doesn't seem to have any effect. so unless some other patching is done, no starting indebted, with the cruel trait, and most likely any other debuff
            // to patch : CharacterCreationContentBase.ApplySkillAndAttributeEffects to have debuffs possible, most likely will do later
            // succesfuly managed to remove gold after patching, haven't tried other options
            CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(AseraiParentsOnCondition));
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_ARais}rais", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiTribesmanOnApply), new TextObject("{=!}", null), null, 0, 150, 3000, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AMamluks}mamluks", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiWariorSlaveOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiWariorSlaveOnApply), new TextObject("{=!}", null), null, 0, 20, -600, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AMerchant}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 4000, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AFarmers}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiOasisFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiOasisFarmerOnApply), new TextObject("{=!}", null), null, 0, 0, -900, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AArtisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBedouinOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBedouinOnApply), new TextObject("{=!}", null), null, 0, 10, 800, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=CCR_Family_Choice_AThugs}thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBackAlleyThugOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBackAlleyThugOnApply), new TextObject("{=!}", null), null, 0, 0, -800, 0, 0);

            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(BattanianParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bchieftains}chieftains", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaChieftainsHearthguardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaChieftainsHearthguardOnApply), new TextObject("{=!}", null), null, 0, 150, 1000, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bhealers}healers", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaHealerOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaHealerOnApply), new TextObject("{=!}", null), null, 50, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bfarmers}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Crafting }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaTribesmanOnApply), new TextObject("{=!}", null), null, 0, -800, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaSmithOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaSmithOnApply), new TextObject("{=!}", null), null, 0, 10, 500, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bforesters}foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaWoodsmanOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Bbards}bards", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaBardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaBardOnApply), new TextObject("{=!}", null), null, 0, 20, -200, 0, 0);

            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(EmpireParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Earistocrates}aristocrates", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireLandlordsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireLandlordsRetainerOnApply), new TextObject("{=!}", null), null, 0, 150, 2000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Emerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 6000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Efreeholders}freeholders", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireFreeholderOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireFreeholderOnApply), new TextObject("{=!}", null), null, 0, 0, -600, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Eartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireArtisanOnApply), new TextObject("{=!}", null), null, 0, 20, 1000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Esoldiers}soldiers", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireWoodsmanOnApply), new TextObject("{=!}", null), null, 0, 40, 500, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Evagabonds}vagabonds", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireVagabondOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);

            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(KhuzaitParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Knoyans}noyans", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNoyansKinsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNoyansKinsmanOnApply), new TextObject("{=!}", null), null, 0, 150, 1000, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kmerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 800, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Knomads}nomads", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitTribesmanOnApply), new TextObject("{=!}", null), null, 0, 50, -300, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitFarmerOnApply), new TextObject("{=!}", null), null, 0, 20, 400, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kwarriors}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitShamanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitShamanOnApply), new TextObject("{=!}", null), null, 0, 30, 700, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Kthugs}thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNomadOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);

            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(SturgianParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sboyars}boyars", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaBoyarsCompanionOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaBoyarsCompanionOnApply), new TextObject("{=!}", null), null, 0, 150, 2000, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Smerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaTraderOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaTraderOnApply), new TextObject("{=!}=", null), null, 0, 0, 2500, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sfarmers}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaFreemanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaFreemanOnApply), new TextObject("{=!}=", null), null, 0, 0, -800, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaArtisanOnApply), new TextObject("{=!}=", null), null, 0, 20, 1200, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Swarriors}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaHunterOnApply), new TextObject("{=!}=", null), null, 0, 60, 600, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Sraiders}raiders", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaVagabondOnApply), new TextObject("{=!}=", null), null, 0, 40, 1000, 0, 0);

            CharacterCreationCategory characterCreationCategory6 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(VlandianParentsOnCondition));
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vbarons}barons", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBaronsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBaronsRetainerOnApply), new TextObject("{=!}=", null), null, 0, 150, 3000, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vmerchants}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMerchantOnApply), new TextObject("{=!}=", null), null, 0, 20, 5000, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vyeomens}yeomen", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaYeomanOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaYeomanOnApply), new TextObject("{=!}=", null), null, 0, 0, -800, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vartisans}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBlacksmithOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBlacksmithOnApply), new TextObject("{=!}", null), null, 0, 30, 500, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vsoldiers}soldiers", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaHunterOnApply), new TextObject("{=!}", null), null, 0, 20, 200, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=CCR_Family_Choice_Vmercenaries}mercenaries", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMercenaryOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMercenaryOnApply), new TextObject("{=!}.", null), null, 0, 40, 600, 0, 0);

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


        //sets the parents job, can be used as a condition. I have yet to rewrite these
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
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Artisan, "", "", true, true);
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
            this.SetParentAndOccupationType(characterCreation, 5, SandboxCharacterCreationContent.OccupationTypes.Mercenary, "", "", true, true);
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










        // I changed the adolescence menu into a menu that specifies the education the parent gave to the player
        protected void AddEducationMenuPatch(CharacterCreation characterCreation)
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
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_religious}be a {?PLAYER.GENDER}woman{?}man{\\?} of faith", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EdReligiousOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_farmer}tend to the fields", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Medicine, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EdFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_lady}become a lady in waiting", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonersFemaleOnConditions), new CharacterCreationOnSelect(EdLadyOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_defense}learn how to defend yourself", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonersMaleOnConditions), new CharacterCreationOnSelect(EdDefenseOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Education_Choice_trickery}know how to trick others", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonerOnCondition), new CharacterCreationOnSelect(EdTrickeryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected bool ParentsCommonerOnCondition()
        {
            return this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }

        protected bool ParentsRetainerOnCondition()
        {
            return this._familyOccupationType == SandboxCharacterCreationContent.OccupationTypes.Retainer;
        }
        protected bool ParentsCommonersFemaleOnConditions() 
        {
            return (Hero.MainHero.IsFemale && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer);
        }
        protected bool ParentsCommonersMaleOnConditions()
        {
            return (!Hero.MainHero.IsFemale && this._familyOccupationType != SandboxCharacterCreationContent.OccupationTypes.Retainer);
        }


         protected void EdNobleTroopOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdLeadTroopsOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_leader" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdCourtLifeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_manners" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdMerchantOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_peddlers" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdArtisanOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdScholarOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdReligiousOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_closed_tutor" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdFarmerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "anim_childhood_peddlers" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdLadyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_manners" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdDefenseOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "anim_childhood_ready" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        protected void EdTrickeryOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_streets" });
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }










        //this one is subject to change, I went for the idiom as it was in a game I played, but tbh I consider removing it altogether a good option
        // I kept it because it seems to me that having 5 steps is nice, to get 5 focus points
        protected void AddChildhoodMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Idioms", null), new TextObject("{=!}Growing up, you were inculcated the saying...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            // noble fighter
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(AseraiRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor}, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(EmpireRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(VlandiaRetainerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            // commoner fighter
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_fighter}Better to be a warrior in a garden than a gardener in a war.", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonerOnCondition), new CharacterCreationOnSelect(IdiomFighterOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_healthy}A healthy mind in a healthy body.", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Medicine }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomHealthyBodyOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Honor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_prevention}Prevention is better than cure.", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomPreventionOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_wellbegun}Well begun is half done.", null), new MBList<SkillObject> { DefaultSkills.Steward, DefaultSkills.Engineering }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomWellBegunOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Calculating, DefaultTraits.Valor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_bold}Fortune favors the bold.", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Tactics }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomBoldOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_forwarned}Forewarned is forearmed.", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Tactics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomForwarnedOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_invention}Necessity is the mother of invention.", null), new MBList<SkillObject> { DefaultSkills.Engineering, DefaultSkills.Trade }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomInventionOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_armed}Men with weapons never starve", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomArmedOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_conquer}To conquer without risk is to triumph without glory.", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Leadership }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomRiskOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Valor }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Idiom_choice_means}The end justifies the means.", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(IdiomMeansOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);

            characterCreation.AddNewMenu(characterCreationMenu);
        }

        protected static void IdiomFighterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_honor" });
        }
        protected static void IdiomHealthyBodyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tactician" });
        }
        protected static void IdiomPreventionOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_sharp" });
        }
        protected static void IdiomWellBegunOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_appearances" });
        }
        protected static void IdiomBoldOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_leader_2" });
        }
        protected static void IdiomForwarnedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_grit" });
        }
        protected static void IdiomInventionOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_grit" });
        }
        protected static void IdiomArmedOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_grit" });
        }
        protected static void IdiomRiskOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_grit" });
        }
        protected static void IdiomMeansOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_grit" });
        }










        // here we choose the starting equipment alongside the start in life. as I said earlier I used culture specific conditions to have culture specific jobs when I could think of them
        // structure is basically noble troop, culture unique job, merchant, craftman, farmer, type of troop made according to my troop mod (https://www.nexusmods.com/mountandblade2bannerlord/mods/4932 , which is why some skills/names might be odd) and then criminals
        protected void AddYouthMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Start in life", null), new TextObject("{=!}You started your life as..", null), new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
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
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sdruzhina}a member of a druzhina", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaDruzhinaOnConsequence), new CharacterCreationApplyFinalEffects(NobleOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_fur_hunter}fur hunter", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Scouting, DefaultSkills.Trade }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaUNIQUEROUTEOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Smerchant}a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Steward, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Scraftman}a craftman", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaCraftmanOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Speasant}a peasant", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Steward, DefaultSkills.Medicine }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaFarmerOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sinfantry}part of the infantry", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaInfantryOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sshocktroop}shock troop", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Throwing, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaShockTroopOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Sarcher}bowman", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaArcherOnConsequence), new CharacterCreationApplyFinalEffects(NothingOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=CCR_Start_Choice_Ssearaider}a sea raider", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Roguery, DefaultSkills.Scouting }, DefaultCharacterAttributes.Cunning, 1, 30, 2, new CharacterCreationOnCondition(SturgiaOnCondition), new CharacterCreationOnSelect(YouthSturgiaBanditOnConsequence), new CharacterCreationApplyFinalEffects(BanditOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

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


        protected void NothingOnApply(CharacterCreation characterCreation)
        {
        }
        protected void BanditOnApply(CharacterCreation characterCreation)
        {
            foreach (Kingdom kingdom in Campaign.Current.Kingdoms)
            {
                ChangeCrimeRatingAction.Apply(kingdom.MapFaction, 50, false);
            }
        }
        protected void NobleOnApply(CharacterCreation characterCreation)
        {
            Hero ruler = Hero.FindAll(hero => hero.Culture == Hero.MainHero.Culture && hero.IsAlive && hero.IsFactionLeader && !hero.MapFaction.IsMinorFaction).GetRandomElementInefficiently();
            ChangeKingdomAction.ApplyByJoinToKingdom(Hero.MainHero.Clan, ruler.Clan.Kingdom, false);
        }


        // the way the starting gears are chosen is in the sanbdbox > sandbox_equipment_sets xml , and it work this way : 
        // player_char_creation_<the culture you chose>_<the TitleType you put in the consequences below>_<the gender, m or f>, it's done in RefreshPlayerAppearance
        // so far I have ten for the empire and it doesn't seem to be too much, I think you can go crazy and have dozens of it
        // I haven't been able to remove gold to have an equipement cost something to the player in the AddCategoryOption so either it will have to wait until I patch it or it has to be done in the consequences
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
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_hardened" });
        }
        protected void YouthAseraiCaravanerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_leader" });
        }
        protected void YouthAseraiMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        protected void YouthAseraiCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        protected void YouthAseraiFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        protected void YouthAseraiSlaveWarriorOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tough" });
        }
        protected void YouthAseraiMountedArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthAseraiArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthAseraiBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
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
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_sharp" });
        }
        protected void YouthBattaniaDruidOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_schooled" });
        }
        protected void YouthBataniaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        protected void YouthBattaniaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        protected void YouthBattanniaForesterOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthBattaniaFalxOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_fierce" });
        }
        protected void YouthBattaniaScoutOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "anim_childhood_spotting" });
        }
        protected void YouthBattaniaKernOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready_throw" });
        }
        protected void YouthBattaniaBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
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
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tactician" });
        }
        protected void YouthEmpireEngineerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_apprentice" });
        }
        protected void YouthEmpireMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        protected void YouthEmpireCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        protected void YouthEmpireFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        protected void YouthEmpireLegionaryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_defend" });
        }
        protected void YouthEmpireArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthEmpireCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        protected void YouthEmpireHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthEmpireBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
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
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tough" });
        }
        protected void YouthKhuzaitNomadOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        protected void YouthKhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        protected void YouthKhuzaitCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        protected void YouthKhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        protected void YouthKhuzaitCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_polearm" });
        }
        protected void YouthKhuzaitHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthKhuzaitInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthKhuzaitBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
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
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_tough" });
        }
        protected void YouthSturgiaUNIQUEROUTEOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        protected void YouthSturgiaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_artisan" });
        }
        protected void YouthSturgiaFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        protected void YouthSturgiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_defend" });
        }
        protected void YouthSturgiaShockTroopOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_fierce" });
        }
        protected void YouthSturgiaArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready_bow" });
        }
        protected void YouthSturgiaBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
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
            base.SelectedTitleType = 1;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_honor" });
        }
        protected void YouthVlandiaChamberlainOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_schooled" });
        }
        protected void YouthVlandiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_numbers" });
        }
        protected void YouthVlandiaGuildOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_apprentice" });
        }
        protected void YouthVlandiaSerfOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_animals" });
        }
        protected void YouthVlandiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_defend" });
        }
        protected void YouthVlandiaLightCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_spotting" });
        }
        protected void YouthVlandiaCrossbowmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiBanditOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_roguery" });
        }










        // here is where I give the player traits. As said earlier I can not give them a negative value yet, will have to wait
        protected void AddAdulthoodMenuPatch(CharacterCreation characterCreation)
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



        protected void ReasonDiscoverTheWorldOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_explorer" });
        }
        protected void ReasonTakeRevengeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_hardened" });
        }
        protected void ReasonForcedOutOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_streets" });
        }
        protected void ReasonSearchForMoneyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_numbers" });
        }
        protected void ReasonBecomeNobleOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_decisive" });
        }
        protected void ReasonMarkHistorydOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_leader_2" });
        }
        protected void ReasonLossLovedOneOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
        }
        protected void ReasonPracticeTradeOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_apprentice" });
        }
        protected void ReasonHelpOthersOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_schooled" });
        }
        protected void ReasonProveFightingSkillOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_fierce" });
        }










        // Here I added the 16 yo option to have an early start, and changed the 20 yo option to 21, to avoid the horrendous babyface we have at 20. You can add more option by adding them to SandboxAgeOptions and copy/pasting the OnConsequence/OnApply
        // I don't have any idea of what it could bring but you can also add some conditions to these, like someone living in the slums doesn't live long or some culture refuse adolescent to roam free I guess
        protected void AddAgeSelectionMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), new CharacterCreationOnInit(this.StartingAgeOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}16", null), new MBList<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(StartingAgeMinorOnConsequence), new CharacterCreationApplyFinalEffects(StartingAgeMinorOnApply), new TextObject("{=CCR_Age_16_desc}Some might say you are too young, but your eagerness more than makes up for your lack of experience.", null), null, 0, 0, 0, 0, 0);
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
            characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_genius" });
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
            characterCreation.ChangeFaceGenChars(SandboxCharacterCreationContent.ChangePlayerFaceWithAge(21f, "act_childhood_schooled"));
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_focus"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_ready"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_sharp"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_tough"});
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
