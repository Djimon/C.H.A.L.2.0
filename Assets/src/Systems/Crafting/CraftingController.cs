using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.Systems.Research;
using CHAL.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CHAL.Systems.Crafting.CraftingService;

namespace CHAL.Systems.Crafting
{

    public class CraftingController : MonoBehaviour
    {
        [Header("References")]
        public CraftingCatalog catalog;
        public InventoryDomain inv;
        public ResearchUnlockRegistry unlocks;

        [Header("UI")]
        public RecipeListView listView;
        public RecipeDetailPanel detailPanel;

        [Header("Inventories")]
        public string materialsInventoryId = "player:materials";
        public string outputInventoryId = "player:gear";

        private IWallet _wallet;

        // State
        private readonly List<RecipeDef> _visibleRecipes = new();
        private RecipeDef _selected;
        private RecipePreview _preview;

        // Logging
        private const string TAG = "Crafting";

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (inv != null) inv.OnSlotChanged += HandleSlotChanged;
            WireUI();
            RebuildRecipeList();
        }

        private void Start()
        {
            _wallet = GameManager.Instance.Profile;
        }

        private void OnDisable()
        {
            if (inv != null) inv.OnSlotChanged -= HandleSlotChanged;
            UnwireUI();
        }
        #endregion

        #region Wiring
        private void WireUI()
        {
            if (listView != null) listView.OnSelect += HandleSelectRecipe;
            if (detailPanel != null) detailPanel.OnCraftClicked += HandleCraftClicked;
        }

        private void UnwireUI()
        {
            if (listView != null) listView.OnSelect -= HandleSelectRecipe;
            if (detailPanel != null) detailPanel.OnCraftClicked -= HandleCraftClicked;
        }
        #endregion

        #region Build & Refresh
        private void RebuildRecipeList()
        {
            _visibleRecipes.Clear();

            if (catalog == null || catalog.recipes == null)
            {
                DebugManager.Warning(TAG, "Catalog is null/empty.");
                listView?.SetData(Array.Empty<RecipeDef>());
                detailPanel?.Clear();
                return;
            }

            foreach (var r in catalog.recipes)
            {
                if (r == null) continue;
                // Research-Gate: nur freigeschaltete Rezepte anzeigen
                if (unlocks != null && !unlocks.IsUnlockedRecipe(r.name)) // Annahme: recipeId == name oder passe hier auf deine ID an
                    continue;

                _visibleRecipes.Add(r);
            }

            listView?.SetData(_visibleRecipes);
            if (_visibleRecipes.Count > 0)
            {
                // Preselect erste Zeile
                HandleSelectRecipe(_visibleRecipes[0]);
            }
            else
            {
                _selected = null;
                detailPanel?.Clear();
            }
        }

        private void RefreshPreviewAndDetail()
        {
            if (_selected == null || inv == null || _wallet == null)
            {
                detailPanel?.Clear();
                return;
            }

            _preview = CraftingService.GetPreview(_selected, outputInventoryId, inv, _wallet);
            detailPanel?.Show(_selected, _preview, GetGoldNeed(_selected), _wallet.GetCurrency("gold"), CountMaterials(_selected));
        }
        #endregion

        #region Handlers
        private void HandleSelectRecipe(RecipeDef recipe)
        {
            _selected = recipe;
            RefreshPreviewAndDetail();
        }

        private void HandleCraftClicked()
        {
            if (_selected == null) return;
            if (inv == null || _wallet == null)
            {
                detailPanel?.ShowFail("Systeme nicht initialisiert.");
                return;
            }

            // Letzte Preview nutzen – Guard-Order ist im Service
            var ok = CraftingService.TryCraftToInventory(_selected, inv, _wallet, outputInventoryId, out var reason);
            if (!ok)
            {
                DebugManager.Info($"Craft fail: {reason}",TAG);
                detailPanel?.ShowFail(MapBlockerToText(_preview.blocker, reason));
                RefreshPreviewAndDetail(); // erneuern (Mengen/Gold)
                return;
            }

            DebugManager.Log($"Craft success: {_selected.outputItemId} x{_selected.outputCount}", DebugManager.EDebugLevel.Test, TAG);
            detailPanel?.ShowSuccess();
            RefreshPreviewAndDetail(); // Bestand geändert → UI updaten
                                       // Optional: SFX/VFX triggern
        }

        private void HandleSlotChanged(string instanceId, int slotIndex, ItemStack? newStack)
        {
            // Nur refreshen, wenn relevante Inventare betroffen sind
            if (instanceId == materialsInventoryId || instanceId == outputInventoryId)
            {
                RefreshPreviewAndDetail();
                // Für Badges in der Liste könntest du hier auch Previews für sichtbare Rezepte nachziehen.
            }
        }
        #endregion

        #region Helpers (read-only)
        private int GetGoldNeed(RecipeDef r)
        {
            if (r.currencyCosts == null) return 0;
            var sum = 0;
            foreach (var c in r.currencyCosts)
                if (!string.IsNullOrEmpty(c.currencyId) && c.currencyId == "gold")
                    sum += Mathf.Max(0, c.amount);
            return sum;
        }

        /// <summary>Ermittelt "have" je benötigtem MaterialId (für die Detailanzeige).</summary>
        private Dictionary<string, int> CountMaterials(RecipeDef r)
        {
            var result = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (r.inputs == null || inv == null) return result;

            foreach (var need in r.inputs)
            {
                if (string.IsNullOrEmpty(need.itemId)) continue;
                result[need.itemId] = 0;

                // konventionsbasierte Instanz ermitteln
                var t = ItemTypeUtils.FromId(need.itemId);
                string instId = null;
                switch (t)
                {
                    case ItemType.Remains: instId = "player_remains"; break;
                    case ItemType.Part: instId = "player_part"; break;
                    case ItemType.Rune: instId = "player_rune"; break;
                    case ItemType.Module: instId = "player_module"; break;
                    default: instId = null; break;
                }
                if (string.IsNullOrEmpty(instId) || !inv.HasInstance(instId)) continue;

                var inst = inv.GetInstance(instId);
                if (inst == null || inst.slots == null) continue;

                var total = 0;
                for (int i = 0; i < inst.slots.Length; i++)
                {
                    var st = inst.slots[i].stack;
                    if (st.HasValue && st.Value.itemID == need.itemId)
                        total += st.Value.count;
                }

                result[need.itemId] = total;
            }

            return result;
        }

        private static string MapBlockerToText(CraftBlocker blocker, string fallback)
        {
            switch (blocker)
            {
                case CraftBlocker.OutputInventoryFull: return "Kein Platz im Zielinventar.";
                case CraftBlocker.MissingMaterials: return "Materialien fehlen.";
                case CraftBlocker.NotEnoughCurrency: return "Gold reicht nicht.";
                case CraftBlocker.InvalidRefinement: return "Veredelung ungültig.";
                case CraftBlocker.None: return string.IsNullOrEmpty(fallback) ? "Unbekannter Fehler." : fallback;
                default: return string.IsNullOrEmpty(fallback) ? "Unbekannter Fehler." : fallback;
            }
        }
        #endregion

    }

}
