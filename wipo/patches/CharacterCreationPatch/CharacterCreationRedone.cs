﻿using HarmonyLib;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Conversation.Tags;
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

        //first menu : family you were born in, divided per culture in submenus. I haven't been able to redo the same structure in other menu (haven't tried much I admit), so I replaced the submenus with a culture specific condition in each option
        protected void AddParentsMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Family", null), new TextObject("{=!}You were born into a family of..", null), new CharacterCreationOnInit(this.ParentsOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

            CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(AseraiParentsOnCondition));

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
            characterCreationCategory1.AddCategoryOption(new TextObject("{=!}rais", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiTribesmanOnApply), new TextObject("{=!}", null), null, 0, 150, 3000, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=!}warrior-slaves", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Throwing }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiWariorSlaveOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiWariorSlaveOnApply), new TextObject("{=!}", null), null, 0, 20, -600, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=!}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 4000, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=!}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiOasisFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiOasisFarmerOnApply), new TextObject("{=!}", null), null, 0, 0, -900, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=!}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBedouinOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBedouinOnApply), new TextObject("{=!}", null), null, 0, 10, 800, 0, 0);
            characterCreationCategory1.AddCategoryOption(new TextObject("{=!}thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(AseraiBackAlleyThugOnConsequence), new CharacterCreationApplyFinalEffects(this.AseraiBackAlleyThugOnApply), new TextObject("{=!}", null), null, 0, 0, -800, 0, 0);

            CharacterCreationCategory characterCreationCategory2 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(BattanianParentsOnCondition));
            characterCreationCategory2.AddCategoryOption(new TextObject("{=!}chieftains", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaChieftainsHearthguardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaChieftainsHearthguardOnApply), new TextObject("{=!}", null), null, 0, 150, 1000, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=!}healers", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaHealerOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaHealerOnApply), new TextObject("{=!}", null), null, 50, 0, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=!}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Crafting }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaTribesmanOnApply), new TextObject("{=!}", null), null, 0, -800, 0, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=!}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaSmithOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaSmithOnApply), new TextObject("{=!}", null), null, 0, 10, 500, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=!}foresters", null), new MBList<SkillObject> { DefaultSkills.Scouting, DefaultSkills.Bow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaWoodsmanOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);
            characterCreationCategory2.AddCategoryOption(new TextObject("{=!}bards", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(BattaniaBardOnConsequence), new CharacterCreationApplyFinalEffects(this.BattaniaBardOnApply), new TextObject("{=!}", null), null, 0, 20, -200, 0, 0);

            CharacterCreationCategory characterCreationCategory3 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(EmpireParentsOnCondition));
            characterCreationCategory3.AddCategoryOption(new TextObject("{=!}aristocrates", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireLandlordsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireLandlordsRetainerOnApply), new TextObject("{=!}", null), null, 0, 150, 2000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=!}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 6000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=!}freeholders", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireFreeholderOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireFreeholderOnApply), new TextObject("{=!}", null), null, 0, 0, -600, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=!}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireArtisanOnApply), new TextObject("{=!}", null), null, 0, 20, 1000, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=!}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireWoodsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireWoodsmanOnApply), new TextObject("{=!}", null), null, 0, 40, 500, 0, 0);
            characterCreationCategory3.AddCategoryOption(new TextObject("{=!}vagabonds", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(EmpireVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.EmpireVagabondOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);

            CharacterCreationCategory characterCreationCategory4 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(KhuzaitParentsOnCondition));
            characterCreationCategory4.AddCategoryOption(new TextObject("{=!}noyans", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNoyansKinsmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNoyansKinsmanOnApply), new TextObject("{=!}", null), null, 0, 150, 1000, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=!}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitMerchantOnApply), new TextObject("{=!}", null), null, 0, 0, 800, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=!}nomads", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitTribesmanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitTribesmanOnApply), new TextObject("{=!}", null), null, 0, 50, -300, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=!}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitFarmerOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitFarmerOnApply), new TextObject("{=!}", null), null, 0, 20, 400, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=!}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitShamanOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitShamanOnApply), new TextObject("{=!}", null), null, 0, 30, 700, 0, 0);
            characterCreationCategory4.AddCategoryOption(new TextObject("{=!}thugs", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(KhuzaitNomadOnConsequence), new CharacterCreationApplyFinalEffects(this.KhuzaitNomadOnApply), new TextObject("{=!}", null), null, 0, 0, -980, 0, 0);

            CharacterCreationCategory characterCreationCategory5 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(SturgianParentsOnCondition));
            characterCreationCategory5.AddCategoryOption(new TextObject("{=!}boyars", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaBoyarsCompanionOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaBoyarsCompanionOnApply), new TextObject("{=!}", null), null, 0, 150, 2000, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=!}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaTraderOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaTraderOnApply), new TextObject("{=!}=", null), null, 0, 0, 2500, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=!}farmers", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaFreemanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaFreemanOnApply), new TextObject("{=!}=", null), null, 0, 0, -800, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=!}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaArtisanOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaArtisanOnApply), new TextObject("{=!}=", null), null, 0, 20, 1200, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=!}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaHunterOnApply), new TextObject("{=!}=", null), null, 0, 60, 600, 0, 0);
            characterCreationCategory5.AddCategoryOption(new TextObject("{=!}raiders", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Throwing }, DefaultCharacterAttributes.Control, 1, 30, 2, null, new CharacterCreationOnSelect(SturgiaVagabondOnConsequence), new CharacterCreationApplyFinalEffects(this.SturgiaVagabondOnApply), new TextObject("{=!}=", null), null, 0, 40, 1000, 0, 0);

            CharacterCreationCategory characterCreationCategory6 = characterCreationMenu.AddMenuCategory(new CharacterCreationOnCondition(VlandianParentsOnCondition));
            characterCreationCategory6.AddCategoryOption(new TextObject("{=!}barons", null), new MBList<SkillObject> { DefaultSkills.Riding, DefaultSkills.Polearm }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBaronsRetainerOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBaronsRetainerOnApply), new TextObject("{=!}=", null), null, 0, 150, 3000, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=!}merchants", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMerchantOnApply), new TextObject("{=!}=", null), null, 0, 20, 5000, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=!}yeomen", null), new MBList<SkillObject> { DefaultSkills.Athletics, DefaultSkills.Polearm }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaYeomanOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaYeomanOnApply), new TextObject("{=!}=", null), null, 0, 0, -800, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=!}artisans", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaBlacksmithOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaBlacksmithOnApply), new TextObject("{=!}", null), null, 0, 30, 500, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=!}warriors", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaHunterOnApply), new TextObject("{=!}", null), null, 0, 20, 200, 0, 0);
            characterCreationCategory6.AddCategoryOption(new TextObject("{=!}mercenaries", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Crossbow }, DefaultCharacterAttributes.Cunning, 1, 30, 2, null, new CharacterCreationOnSelect(VlandiaMercenaryOnConsequence), new CharacterCreationApplyFinalEffects(this.VlandiaMercenaryOnApply), new TextObject("{=!}.", null), null, 0, 40, 600, 0, 0);

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


        //sets the parents job, can be used as a condition
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











        protected void AddEducationMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Education", null), new TextObject("{=!}Your parents wanted you to...", null), new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}become a faris", null), new MBList<SkillObject> { DefaultSkills.Polearm, DefaultSkills.Athletics, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(AseraiRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}enter the hearthguard", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Bow, DefaultSkills.Athletics }, DefaultCharacterAttributes.Control, 1, 30, 2, new CharacterCreationOnCondition(BattaniaRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}become a cataphract", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Polearm, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(EmpireRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}enter a khan's guard", null), new MBList<SkillObject> { DefaultSkills.Bow, DefaultSkills.Athletics, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(KhuzaitRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}join a druzhina", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded, DefaultSkills.Athletics }, DefaultCharacterAttributes.Vigor, 1, 30, 2, new CharacterCreationOnCondition(SturgiaRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}become a knight", null), new MBList<SkillObject> { DefaultSkills.TwoHanded, DefaultSkills.Athletics, DefaultSkills.Riding }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(VlandiaRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}lead armies", null), new MBList<SkillObject> { DefaultSkills.Tactics, DefaultSkills.Leadership, DefaultSkills.Charm }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}be part of the court", null), new MBList<SkillObject> { DefaultSkills.Charm, DefaultSkills.Roguery, DefaultSkills.Tactics }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsRetainerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}become a merchant", null), new MBList<SkillObject> { DefaultSkills.Trade, DefaultSkills.Charm, DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 30, 2, null, new CharacterCreationOnSelect(RuralAdolescenceHerderOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHerderOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}learn a trade", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Trade, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(RuralAdolescenceSmithyOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceSmithyOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}become a scholar", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Engineering, DefaultSkills.Tactics }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(RuralAdolescenceRepairmanOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceRepairmanOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}be a {?PLAYER.GENDER}woman{?}man{\\?} of faith", null), new MBList<SkillObject> { DefaultSkills.Medicine, DefaultSkills.Steward, DefaultSkills.Trade }, DefaultCharacterAttributes.Intelligence, 1, 30, 2, null, new CharacterCreationOnSelect(RuralAdolescenceGathererOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceGathererOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}tend to the fields", null), new MBList<SkillObject> { DefaultSkills.Crafting, DefaultSkills.Medicine, DefaultSkills.Athletics }, DefaultCharacterAttributes.Endurance, 1, 30, 2, null, new CharacterCreationOnSelect(RuralAdolescenceHunterOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHunterOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}learn how to defend yourself", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.Athletics, DefaultSkills.Roguery }, DefaultCharacterAttributes.Endurance, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonerOnCondition), new CharacterCreationOnSelect(UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.UrbanAdolescenceDockerOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}know how to trick others", null), new MBList<SkillObject> { DefaultSkills.Roguery, DefaultSkills.Charm, DefaultSkills.OneHanded }, DefaultCharacterAttributes.Social, 1, 30, 2, new CharacterCreationOnCondition(ParentsCommonerOnCondition), new CharacterCreationOnSelect(RuralAdolescenceHelperOnConsequence), new CharacterCreationApplyFinalEffects(SandboxCharacterCreationContent.RuralAdolescenceHelperOnApply), new TextObject("{=!}", null), null, 0, 0, 0, 0, 0);
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
        protected bool PlayerIsFemaleOnConditions()
        {
            return true;
        }


        //these just change the displayed equipment and animation
        new protected void RuralAdolescenceHerderOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_streets"});
            this.RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
        }
        new protected void RuralAdolescenceSmithyOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_militia"});
            this.RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
        }
        new protected void RuralAdolescenceRepairmanOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_grit"});
            this.RefreshPropsAndClothing(characterCreation, false, "carry_hammer", true, "");
        }
        new protected void RuralAdolescenceGathererOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_peddlers"});
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
        }
        new protected void RuralAdolescenceHunterOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_sharp"});
            this.RefreshPropsAndClothing(characterCreation, false, "composite_bow", true, "");
        }
        new protected void RuralAdolescenceHelperOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_peddlers_2"});
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
        }
        new protected void UrbanAdolescenceWatcherOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_fox"});
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        new protected void UrbanAdolescenceMarketerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_manners"});
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        new protected void UrbanAdolescenceGangerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_athlete"});
            this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
        }
        new protected void UrbanAdolescenceDockerOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_peddlers"});
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
        }
        new protected void UrbanAdolescenceHorserOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_peddlers_2"});
            this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
        }
        new protected void UrbanAdolescenceTutorOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_book"});
            this.RefreshPropsAndClothing(characterCreation, false, "character_creation_notebook", false, "");
        }










        //this one will be completely reworked, looking into what to put instead
        protected void AddChildhoodMenuPatch(CharacterCreation characterCreation)
        {
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}", null), new TextObject("{=!}", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_leader"});
        }
        new protected static void ChildhoodYourBrawnOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_athlete"});
        }
        new protected static void ChildhoodAttentionToDetailOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_memory"});
        }
        new protected static void ChildhoodAptitudeForNumbersOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_numbers"});
        }
        new protected static void ChildhoodWayWithPeopleOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_manners"});
        }
        new protected static void ChildhoodSkillsWithHorsesOnConsequence(CharacterCreation characterCreation)
        {
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_animals"});
        }










        // here we choose the starting equipment alongside the start in life. as I said earlier I used culture specific conditions to have them unique jobs when I could think of them
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiCaravanerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiSlaveWarriorOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiMountedArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthAseraiHashashinOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});}
        protected void YouthBattaniaDruidOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBataniaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBattaniaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBattanniaForesterOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBattaniaKernOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBattaniaFalxOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBattaniaScoutOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthBattaniaThugOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireEngineerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireLegionaryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthEmpireGangOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 10;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitNomadOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitHorseArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthKhuzaitThugOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaCraftmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaFarmerOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaShockTroopOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaArcherOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthSturgiaRaiderOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaChamberlainOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 2;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaMerchantOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 3;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaGuildOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 4;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaSerfOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 5;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaInfantryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 6;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaLightCavalryOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 7;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiaCrossbowmanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 8;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }
        protected void YouthVlandiHighwaymanOnConsequence(CharacterCreation characterCreation)
        {
            base.SelectedTitleType = 9;
            this.RefreshPlayerAppearance(characterCreation);
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_decisive"});
        }












        // here is where I give the player traits. As said earlier I can not give them a negative value yet, will have to wait
        protected void AddAdulthoodMenuPatch(CharacterCreation characterCreation)
        {
            MBTextManager.SetTextVariable("EXP_VALUE", 30);
            CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=!}Adulthood", null), new TextObject("{=!}You started adventuring...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
            CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to discover the world", null), new MBList<SkillObject> { DefaultSkills.Scouting }, DefaultCharacterAttributes.Endurance, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonDiscoverTheWorldOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentDefeatedEnemyOnApply), new TextObject("{=!}The temptation of travel was to much for you, as you always dreamt of seeing the world.", null), null, 1, 20, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to take revenge", null), new MBList<SkillObject> { DefaultSkills.Tactics }, DefaultCharacterAttributes.Vigor, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonTakeRevengeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}After being wronged, you felt the need for revenge. With that goal in mind, you wandered throughout Calradia to get reparations", null), new MBList<TraitObject> { DefaultTraits.Mercy }, -1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}after being forced out", null), new MBList<SkillObject> { DefaultSkills.Roguery }, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonForcedOutOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}After one last disagreement with you, your parents forced you out. With nowhere to go, adventuring was all you could do", null), new MBList<TraitObject> { DefaultTraits.Honor }, -1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}in search of money", null), new MBList<SkillObject> { DefaultSkills.Trade}, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonSearchForMoneyOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}You always wanted to make riches, and it was obvious for you that staying at home would never allow you do it.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to become one of the powerful", null), new MBList<SkillObject> { DefaultSkills.Leadership }, DefaultCharacterAttributes.Cunning, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonBecomeNobleOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}Seeing how those in power had many advantages, you joined on an adventure to join them, or even replace them.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 10, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to mark history", null), new MBList<SkillObject> { DefaultSkills.Charm  }, DefaultCharacterAttributes.Social, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonMarkHistorydOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=!}With all the wars in Calradia, there are many way one could carve {?PLAYER.GENDER}her{?}his{\\?} name in history.", null), new MBList<TraitObject> { DefaultTraits.Valor, DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}after the loss of a loved one", null), new MBList<SkillObject> { DefaultSkills.Steward }, DefaultCharacterAttributes.Social, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonLossLovedOneOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=!}After losing some close to you, you left to see if you could fill that hole", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to practice your trade", null), new MBList<SkillObject> { DefaultSkills.Crafting }, DefaultCharacterAttributes.Endurance, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonPracticeTradeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=!}Your craftmanship has been an important part of your life, but your skill wasn't enough. You thus decided to embark on a journey to improve at it.", null), new MBList<TraitObject> { DefaultTraits.Calculating }, 1, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to help those in need", null), new MBList<SkillObject> { DefaultSkills.Medicine }, DefaultCharacterAttributes.Intelligence, 1, 50, 2, null, new CharacterCreationOnSelect(ReasonHelpOthersOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=!}Having been taught in medicine, you decide to set out and help those in need, as with the wars many have suffered.", null), new MBList<TraitObject> { DefaultTraits.Mercy }, 2, 0, 0, 0, 0);
            characterCreationCategory.AddCategoryOption(new TextObject("{=!}to prove your fighting skills", null), new MBList<SkillObject> { DefaultSkills.OneHanded, DefaultSkills.TwoHanded, DefaultSkills.Polearm }, DefaultCharacterAttributes.Vigor, 1, 20, 2, null, new CharacterCreationOnSelect(ReasonProveFightingSkillOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=!}Attaching much importance to your fighting skill, you figured there was no better place than calradia to prove your might.", null), new MBList<TraitObject> { DefaultTraits.Valor }, 0, 5, 0, 0, 0);
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
            characterCreation.ChangeCharsAnimation(new List<string>{"act_childhood_focus"});
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
