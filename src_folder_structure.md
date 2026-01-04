```text
src/
├─ Core/
│  ├─ BalanceManager.cs
│  ├─ EventBus.cs
│  ├─ GameManager.cs
│  ├─ InputManager.cs
│  ├─ IWallet.cs
│  ├─ PlayerProfile.cs
│  ├─ SaveGameConfig.cs
│  ├─ SaveSystem.cs
│  └─ StatisticService.cs
├─ Data/
│  ├─ Config/
│  │  ├─ GameBalanceConfig.cs
│  │  ├─ HeroXpConfig.cs
│  │  ├─ ImplicitGearTypeConfig.cs
│  │  └─ RuneForgeConfig.cs
│  ├─ Defs/
│  │  ├─ AffixDef.cs
│  │  ├─ AffixFamilyDef.cs
│  │  ├─ ArchetypeDef.cs
│  │  ├─ AttackDef.cs
│  │  ├─ EnemyDef.cs
│  │  ├─ HeroDef.cs
│  │  ├─ ImplicitDef.cs
│  │  ├─ ItemDef_SO.cs
│  │  ├─ MapDef.cs
│  │  ├─ MonsterTagDefs.cs
│  │  ├─ RecipeDef.cs
│  │  ├─ ResearchNodeDef.cs
│  │  ├─ ResearchRequirement.cs
│  │  ├─ ResearchThemeDef.cs
│  │  ├─ ResearchTreeDef.cs
│  │  ├─ SkillModifierDef.cs
│  │  ├─ SkillModuleDef.cs
│  │  ├─ SocketTypedef.cs
│  │  └─ WaveDef.cs
│  ├─ DTO/
│  │  ├─ LootDTO.cs
│  │  ├─ ModulePartMapWrapper.cs
│  │  └─ ResearchSnapShot.cs
│  ├─ Enums/
│  │  ├─ AIPrio.cs
│  │  ├─ AnimationType.cs
│  │  ├─ DamageType.cs
│  │  ├─ Enemy.cs
│  │  ├─ HeroEnums.cs
│  │  ├─ InventroyType.cs
│  │  ├─ ItemRarity.cs
│  │  ├─ ItemType.cs
│  │  ├─ MapDifficulty.cs
│  │  ├─ ResearchUnlockTypes.cs
│  │  ├─ RuneColorType.cs
│  │  ├─ SkillEnums.cs
│  │  ├─ SocketType.cs
│  │  └─ UnitTeam.cs
│  └─ Structs/
│     ├─ EnemyStruct.cs
│     ├─ ItemStruct.cs
│     └─ WaveComposition.cs
├─ Editor/
│  ├─ HeroXpConfigEditor.cs
│  ├─ ImplicitGearConfigEditor.cs
│  ├─ ItemDefEdtitor.cs
│  ├─ ResearchNodeEditorWindow.cs
│  ├─ ResearchTreeeEditor.cs
│  ├─ RunForgeConfigEditor.cs
│  └─ SkillDataEditor.cs
├─ Systems/
│  ├─ _test/
│  │  ├─ dbg_LootRoller.cs
│  │  ├─ dbg_LootRules.cs
│  │  ├─ DebugCraftingRunner.cs
│  │  ├─ DebugEnemySpawner.cs
│  │  └─ demo_InvenotryBootstrap.cs
│  ├─ Crafting/
│  │  ├─ CraftingCatalog.cs
│  │  ├─ CraftingController.cs
│  │  └─ CraftingService.cs
│  ├─ Enemy/
│  │  ├─ EnemyController.cs
│  │  ├─ EnemyInstance.cs
│  │  └─ MonsterTagRegistry.cs
│  ├─ Heroes/
│  │  ├─ OrbitSystem/
│  │  ├─ HeroCatalog.cs
│  │  ├─ HeroController.cs
│  │  ├─ HeroInstance.cs
│  │  ├─ HeroLoadoutService.cs
│  │  └─ HeroProgressData.cs
│  ├─ Inventory/
│  │  ├─ DragDropService.cs
│  │  ├─ IInventoryDomain.cs
│  │  ├─ InvDnDProvider.cs
│  │  ├─ InventoryDef.cs
│  │  ├─ InventoryDomain.cs
│  │  ├─ InventoryInstance.cs
│  │  ├─ ItemStackRef.cs
│  │  ├─ MoveRequest.cs
│  │  ├─ Slot.cs
│  │  └─ TransactionResult.cs
│  ├─ Items/
│  │  ├─ Gear/
│  │  │  ├─ GearInstance.cs
│  │  │  ├─ GearModRegistry.cs
│  │  │  ├─ GearRoller.cs
│  │  │  └─ GearType.cs
│  │  ├─ ItemRegistry.cs
│  │  └─ ItemType.cs
│  ├─ Localization/
│  │  ├─ LocalizationDict.cs
│  │  └─ Localizationmanager.cs
│  ├─ Loot/
│  │  ├─ Models/
│  │  │  └─ LootModel.cs
│  │  ├─ LootBudgetCalculator.cs
│  │  ├─ LootBudgetModulator.cs
│  │  ├─ LootCube.cs
│  │  ├─ LootRoller.cs
│  │  ├─ LootRoller_old.cs
│  │  ├─ LootRulesService.cs
│  │  └─ UnluckyProtection.cs
│  ├─ Map/
│  │  ├─ Waves/
│  │  │  ├─ WaveLootContext.cs
│  │  │  └─ WaveManager.cs
│  │  └─ MapManager.cs
│  ├─ Research/
│  │  ├─ UI/
│  │  │  ├─ ResearchEdgeGraphic.cs
│  │  │  ├─ ResearchMapView.cs
│  │  │  └─ ResearchNodeWidget.cs
│  │  ├─ DevResearchFastForward.cs
│  │  ├─ ResearchService.cs
│  │  ├─ ResearchState.cs
│  │  ├─ ResearchTreeCompiler.cs
│  │  └─ ResearchUnlockRegistry.cs
│  ├─ Skills/
│  │  ├─ Effekte/
│  │  │  ├─ DamageImpact.cs
│  │  │  └─ TriggeSkillImpact.cs
│  │  ├─ ActiveStatusEffect.cs
│  │  ├─ BuffStatusEffect.cs
│  │  ├─ CombatCalculator.cs
│  │  ├─ DamageModifier.cs
│  │  ├─ DamagePacket.cs
│  │  ├─ DebuffStatusEffect.cs
│  │  ├─ DoTStatusEffect.cs
│  │  ├─ HitContext.cs
│  │  ├─ ProjectileController.cs
│  │  ├─ ResolvedSkill.cs
│  │  ├─ SkillExecuter.cs
│  │  ├─ SkillImpactBase.cs
│  │  ├─ SkillInstance.cs
│  │  ├─ SkillModifierStack.cs
│  │  ├─ SkillRegistry.cs
│  │  ├─ SkillResolveUtility.cs
│  │  └─ TagContext.cs
│  └─ Unit/
│     ├─ AiTargetSelector.cs
│     ├─ EffectReceiver.cs
│     ├─ IAttributeHolder.cs
│     ├─ IUnitController.cs
│     ├─ MoveAgent.cs
│     ├─ UnitLocator.cs
│     └─ UnitRegistry.cs
├─ UI/
│  ├─ HUD/
│  ├─ misc/
│  │  ├─ ClickableObject.cs
│  │  ├─ GhostOverlay.cs
│  │  ├─ IDockableView.cs
│  │  ├─ InGameUI.cs
│  │  ├─ UIDockingManager.cs
│  │  └─ UIEnums.cs
│  ├─ templates/
│  ├─ uss/
│  ├─ uxml/
│  ├─ CharacterCreationUI.cs
│  ├─ CraftingUI.cs
│  ├─ HeroSelectionUI.cs
│  ├─ InventoryView.cs
│  ├─ MainMenuUI.cs
│  ├─ MapRewardUI.cs
│  ├─ MapSelectionIUI.cs
│  ├─ RecipeDetailPanelView.cs
│  ├─ RecipeListView.cs
│  ├─ ResearchHUD.cs
│  └─ WaveRewardUI.cs
├─ utils/
│  ├─ DebugConfig.cs
│  └─ DebugManager.cs
└─ xTernal/
   └─ SaveGameFree/
      ├─ Documentation/
      ├─ Editor/
      │  └─ Tests/
      │     └─ SaveGameTests.cs
      ├─ Examples/
      │  ├─ Auto Save/
      │  │  └─ ExampleMoveObject.cs
      │  ├─ Save Custom/
      │  │  └─ ExampleSaveCustom.cs
      │  ├─ Save Position/
      │  │  └─ ExampleSavePosition.cs
      │  ├─ Save Rotation/
      │  │  └─ ExampleSaveRotation.cs
      │  ├─ Save Scale/
      │  │  └─ ExampleSaveScale.cs
      │  ├─ Save Web/
      │  │  └─ ExampleSaveWeb.cs
      │  └─ Shared/
      │     ├─ Prefabs/
      │     └─ Scripts/
      │        └─ SerializerDropdown.cs
      ├─ Plugins/
      ├─ PressKit/
      ├─ Scripts/
      │  ├─ Encoders/
      │  │  ├─ ISaveGameEncoder.cs
      │  │  └─ SaveGameSimpleEncoder.cs
      │  ├─ Serializers/
      │  │  ├─ ISaveGameSerializer.cs
      │  │  ├─ SaveGameBinarySerializer.cs
      │  │  ├─ SaveGameJsonSerializer.cs
      │  │  └─ SaveGameXmlSerializer.cs
      │  ├─ Types/
      │  │  ├─ Color32Save.cs
      │  │  ├─ ColorSave.cs
      │  │  ├─ MeshSave.cs
      │  │  ├─ QuaternionSave.cs
      │  │  ├─ Vector2Save.cs
      │  │  ├─ Vector3Save.cs
      │  │  └─ Vector4Save.cs
      │  ├─ SaveGame.cs
      │  ├─ SaveGameAuto.cs
      │  └─ SaveGameWeb.cs
      └─ Web/
```

