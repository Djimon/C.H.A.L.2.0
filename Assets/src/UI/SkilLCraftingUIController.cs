using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Crafting;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static CHAL.Systems.Crafting.CraftingService;

namespace CHAL.Systems.UI
{
    public class SkillModuleCraftingPanel : IngameUI
    {
        //private VisualElement root;

        // UI References
        private ScrollView _moduleListScroll;
        private VisualElement _moduleIcon;
        private Label _moduleName;
        private Label _moduleBaseInfo;
        private Label _moduleTags;

        private DropdownField _coreDropdown;
        private SliderInt _tierSlider;
        private Label _tierValue;

        private VisualElement _materialsList;
        private Label _goldLabel;
        private Button _craftButton;
        private Label _failLabel;
        private Button _exitButton;

        // Runtime state
        private ItemDef _selectedModule;
        private ItemDef _selectedCore;
        private int _selectedTier = 1;
        private SkillModuleCraftPreview _currentPreview;

        private InventoryDomain _inv;
        private IWallet _wallet;

        // Mapping Dropdown-Text -> Core-ItemDef
        private readonly Dictionary<string, ItemDef> _coreChoices = new();

        protected override void Awake()
        {
            // Basisklasse: setzt root = UIDocument.rootVisualElement
            // und root.style.display = DisplayStyle.None
            base.Awake();

            if (root == null)
            {
                Debug.LogError("[SkillModuleCraftingPanel] root is null (IngameUI failed to init UIDocument).");
                enabled = false;
                return;
            }

            // 2) Services vom GameManager holen
            var gm = GameManager.Instance;
            if (gm == null)
            {
                Debug.LogError("[SkillModuleCraftingPanel] GameManager.Instance is null.");
                enabled = false;
                return;
            }

            _inv = gm.Inventory;
            _wallet = gm.Profile; // PlayerProfile : IWallet

            if (_inv == null || _wallet == null)
            {
                Debug.LogError("[SkillModuleCraftingPanel] Inventory or Wallet missing (GameManager.Inventory / Profile).");
                enabled = false;
                return;
            }

            // 3) UI binden + Liste bauen + Events hooken
            BindUI();
            BuildModuleList();
            HookEvents();

            // WICHTIG: NICHT mehr mit root.visible arbeiten.
            // Sichtbarkeit geht nur über IngameUI.Show(bool) / ToggleUI().
        }

        private void BindUI()
        {
            _moduleListScroll = root.Q<ScrollView>("module-list-scroll");
            _moduleIcon = root.Q<VisualElement>("module-icon");
            _moduleName = root.Q<Label>("module-name");
            _moduleBaseInfo = root.Q<Label>("module-base-info");
            _moduleTags = root.Q<Label>("module-tags");

            _coreDropdown = root.Q<DropdownField>("core-dropdown");
            _tierSlider = root.Q<SliderInt>("tier-slider");
            _tierValue = root.Q<Label>("tier-value");

            _materialsList = root.Q<VisualElement>("materials-list");
            _goldLabel = root.Q<Label>("gold-label");
            _craftButton = root.Q<Button>("craft-btn");
            _failLabel = root.Q<Label>("fail-label");
            _exitButton = root.Q<Button>("exit");

            if (_tierSlider != null)
            {
                int minTier = 1;
                int maxTier = 1;

                var gm = GameManager.Instance;
                var cfg = gm != null ? gm.BalanceConfig : null;
                var costs = cfg != null ? cfg.skillSettings.skillModuleCosts.TierBasedCosts : null;

                if (costs != null && costs.Count > 0)
                {
                    maxTier = minTier;
                    for (int i = 0; i < costs.Count; i++)
                    {
                        var entry = costs[i];
                        if (entry.tier > maxTier)
                            maxTier = entry.tier;
                    }
                }

                _tierSlider.lowValue = minTier;
                _tierSlider.highValue = maxTier;

                // initial clamp + Label setzen
                _selectedTier = Mathf.Clamp(_tierSlider.value, minTier, maxTier);
                _tierSlider.value = _selectedTier;

                if (_tierValue != null)
                    _tierValue.text = _selectedTier.ToString();
            }
        }

        private void BuildModuleList()
        {
            _moduleListScroll.Clear();

            var modules = GetAllSkillModuleItems();

            foreach (var item in modules)
            {
                var row = new Label(item.name); // TODO: eigenes Row-Template benutzen
                row.AddToClassList("list-entry");
                row.userData = item;

                row.RegisterCallback<ClickEvent>(_ =>
                {
                    SelectModule((ItemDef)row.userData);
                });

                _moduleListScroll.Add(row);
            }

            // Default-Auswahl
            if (modules.Count > 0)
                SelectModule(modules[0]);
        }

        private List<ItemDef> GetAllSkillModuleItems()
        {
            var result = new List<ItemDef>();

            var registry = ItemRegistry.Instance;
            if (registry == null) return result;

            foreach (var item in registry.GetAllItemsByType("module"))
            {
                if (item == null) continue;

                // Filter: nur Module mit SkillDef
                if (ItemTypeUtils.FromId(item.itemId) != ItemType.Module)
                    continue;

                if (item.moduleData == null || item.moduleData.skillDef == null)
                    continue;

                result.Add(item);
            }

            return result;
        }

        private void HookEvents()
        {
            if (_tierSlider != null)
            {
                _tierSlider.RegisterValueChangedCallback(evt =>
                {
                    _selectedTier = evt.newValue;
                    _tierValue.text = _selectedTier.ToString();
                    RefreshPreview();
                });
            }

            if (_coreDropdown != null)
            {
                _coreDropdown.RegisterValueChangedCallback(evt =>
                {
                    _selectedCore = ResolveCoreFromDropdownValue(evt.newValue);
                    RefreshPreview();
                });
            }

            if (_craftButton != null)
            {
                _craftButton.clicked += OnCraftClicked;
            }

            if (_exitButton != null)
            {
                _exitButton.clicked += () =>
                {
                    Show(false);
                };
            }
        }

        private void SelectModule(ItemDef moduleItem)
        {
            _selectedModule = moduleItem;
            _moduleName.text = moduleItem.name;
            _moduleBaseInfo.text = moduleItem.itemId;
            _moduleTags.text = moduleItem.moduleData.skillDef.SkillId;

            BuildCoreDropdownForModule(moduleItem);
            RefreshPreview();
        }

        private void BuildCoreDropdownForModule(ItemDef moduleItem)
        {
            _coreChoices.Clear();

            var cores = GetAllCoreItemsForSkill(moduleItem.moduleData.skillDef, out var defaultCoreItem);

            var options = new List<string>();
            foreach (var c in cores)
            {
                // schöneres Label: bevorzugt displayName, sonst SO-Name
                var label = !string.IsNullOrEmpty(c.name) ? c.name : c.name;
                options.Add(label);
                _coreChoices[label] = c;
            }

            _coreDropdown.choices = options;

            // Default-Core setzen
            if (defaultCoreItem != null)
            {
                var label = defaultCoreItem.itemId;

                _coreDropdown.value = label;
                _selectedCore = defaultCoreItem;
            }
            else if (cores.Count > 0)
            {
                var first = cores[0];
                var label = !string.IsNullOrEmpty(first.itemId) ? first.itemId : first.name;
                _coreDropdown.value = label;
                _selectedCore = first;
            }
            else
            {
                _coreDropdown.value = string.Empty;
                _selectedCore = null;
            }
        }

        private List<ItemDef> GetAllCoreItemsForSkill(SkillModuleDef skillDef, out ItemDef defaultCoreItem)
        {
            defaultCoreItem = null;
            var result = new List<ItemDef>();

            var registry = ItemRegistry.Instance;
            if (registry == null) return result;

            // Helper lokal: spiegelt exakt die Service-Regel
            bool IsCoreAllowed(CoreType coreType)
            {
                if (coreType == skillDef.defaultCore)
                    return true;

                var allowed = skillDef.changeCoreTypesAllowed;
                if (allowed == null || allowed.Count == 0)
                    return false;

                return allowed.Contains(coreType);
            }

            foreach (var item in registry.GetAllItemsByType("core"))
            {
                if (item == null) continue;
                if (ItemTypeUtils.FromId(item.itemId) != ItemType.Core) continue;
                if (item.coreData == null) continue;

                var coreType = item.coreData.coreType;

                // nur erlaubte Cores aufnehmen
                if (!IsCoreAllowed(coreType))
                    continue;

                result.Add(item);

                if (coreType == skillDef.defaultCore)
                    defaultCoreItem = item;
            }

            return result;
        }

        private ItemDef ResolveCoreFromDropdownValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return _selectedCore;

            if (_coreChoices.TryGetValue(value, out var def))
                return def;

            return _selectedCore;
        }

        private void RefreshPreview()
        {
            _failLabel.text = string.Empty;
            _craftButton.SetEnabled(false);

            if (_selectedModule == null || _selectedCore == null)
                return;

            var preview = CraftingService.PreviewSkillModuleCraft(
                _selectedModule,
                _selectedTier,
                _selectedCore,
                _inv,
                _wallet,
                "player_module");

            _currentPreview = preview;

            RenderPreview(preview);
        }

        private void RenderPreview(SkillModuleCraftPreview preview)
        {
            _materialsList.Clear();

            foreach (var mat in preview.materials)
            {
                var label = new Label($"{mat.playerAmount}/{mat.required} {mat.itemId}");
                if (mat.playerAmount < mat.required)
                    label.AddToClassList("cost-missing");
                _materialsList.Add(label);
            }

            _goldLabel.text = $"{preview.goldCost} G";

            _craftButton.SetEnabled(preview.canCraft);

            if (!preview.canCraft)
            {
                _failLabel.text = preview.blocker.ToString();
            }
        }

        private void OnCraftClicked()
        {
            if (_selectedModule == null || _selectedCore == null)
                return;

            if (!CraftingService.TryCraftSkillModuleToInventory(
                    _selectedModule,
                    _selectedTier,
                    _selectedCore,
                    _inv,
                    _wallet,
                    "player_module",
                    out var fail))
            {
                _failLabel.text = fail;
                RefreshPreview();
                return;
            }

            // Erfolg
            _failLabel.text = "Crafted.";
            RefreshPreview();
        }
    }
}
