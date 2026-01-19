using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.Systems.Codex;
using CHAL.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace CHAL.Systems.Crafting
{

/// <summary>
/// Manages crafting operations and interactions within the game.
/// Handles inventory, recipes, and UI elements related to crafting.
/// </summary>
    public class CraftingController : MonoBehaviour
    {
        [Header("References")]
        public CraftingCatalog catalog;
        public InventoryDomain inv;
        public CodexUnlockRegistry unlocks;

        [Header("UI")]
        public RecipeListView listView;
        public RecipeDetailPanel detailPanel;

        [Header("Inventories")]
        private HashSet<string> _relevantInvIds;


        private IWallet _wallet;

        // State
        private readonly List<RecipeDef> _visibleRecipes = new();
        private RecipeDef _selected;
        private CraftingService.RecipePreview _preview;

        // Logging
        private const string TAG = "Crafting";

        #region Unity Lifecycle
        private void OnEnable()
        {
            WireUI();
        }

        private void Awake()
        {
            _relevantInvIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            //if (!string.IsNullOrEmpty(outputInventoryId))
            //    _relevantInvIds.Add(outputInventoryId);
        }

        private void Start()
        {
            StartCoroutine(InitAfterOneFrame());

        }

        private IEnumerator InitAfterOneFrame()
        {
            yield return null;

            _wallet = GameManager.Instance != null ? GameManager.Instance.Profile : null;  

            if (_wallet == null)
            {
                DebugManager.Warning("Wallet is null. Crafting UI will not initialize.", "Crafting");
            }

            unlocks = GameManager.Instance.codexUnlocks;

            if( unlocks == null )
                DebugManager.Warning("No UnlockRegistry!", "Crafting");

            if (inv == null && GameManager.Instance != null)
            {
                inv = GameManager.Instance.Inventory; // <-- dein zentrales Inv-System
                DebugManager.Info(inv != null ? "InventoryDomain resolved from GameManager." : "InventoryDomain still null.", "Crafting");
            }

            if (inv == null)
            {
                DebugManager.Warning("InventoryDomain is null. Crafting UI will not initialize.", "Crafting");
                yield break;
            }

            inv.OnSlotChanged += HandleSlotChanged;
            RebuildRecipeList();
            RefreshPreviewAndDetail();
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
            if (listView != null) { listView.OnSelect += HandleSelectRecipe; DebugManager.Info("ListView wired", "Crafting"); }
            if (detailPanel != null) { detailPanel.OnCraftClicked += HandleCraftClicked; DebugManager.Info("Detail wired", "Crafting"); }
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

            if (inv == null)
            {
                detailPanel?.Clear();
                listView?.SetData(Array.Empty<RecipeDef>(), new Dictionary<RecipeDef, bool>());
                DebugManager.Warning("Rebuild skipped: InventoryDomain is null.", "Crafting");
                return;
            }

            if (catalog == null || catalog.recipes == null)
            {
                DebugManager.Warning("Catalog is null/empty.", "Crafting");
                listView?.SetData(Array.Empty<RecipeDef>(), new Dictionary<RecipeDef, bool>());
                detailPanel?.Clear();
                return;
            }

            foreach (var r in catalog.recipes)
            {
                if (r == null) continue;

                if (unlocks == null)
                    DebugManager.Warning("No UnlockRegistry!", "Crafting");

                DebugManager.Log($"{r.Id} unlokced? {unlocks.IsUnlockedRecipe(r.Id)}", DebugManager.EDebugLevel.Debug, "Crafting");
                // Research-Gate: nur freigeschaltete Rezepte anzeigen
                if (unlocks != null && !unlocks.IsUnlockedRecipe(r.Id))
                {   
                    continue;
                }

                


                _visibleRecipes.Add(r);
            }

            var craftableMap = new Dictionary<RecipeDef, bool>();
            if (inv != null && _wallet != null)
            {
                foreach (var r in _visibleRecipes)
                {
                    var outId = ResolveOutputInventoryId(r);
                    var p = (outId == null)
                        ? new CraftingService.RecipePreview(false, CraftBlocker.OutputInventoryFull, false, false, false)
                        : CraftingService.GetPreview(r, outId, inv, _wallet);
                    craftableMap[r] = p.canCraft;
                    DebugManager.DebugLog($"recipe: {r} can be crafted: {p.canCraft} (reason: {p.blocker})","Crafting");
                }
            }

            listView?.SetData(_visibleRecipes, craftableMap);

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

            _relevantInvIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (GameManager.Instance != null && _visibleRecipes != null)
            {
                foreach (var r in _visibleRecipes)
                {
                    // Output-Instanz tracken
                    var outId = ResolveOutputInventoryId(r);
                    if (!string.IsNullOrEmpty(outId)) _relevantInvIds.Add(outId);

                    // Input-Instanzen tracken
                    if (r?.inputs == null) continue;
                    foreach (var need in r.inputs)
                    {
                        if (string.IsNullOrEmpty(need.itemId)) continue;
                        if (GameManager.Instance.TryResolveByItemId(need.itemId, out var _type, out var instId)
                            && !string.IsNullOrEmpty(instId))
                        {
                            _relevantInvIds.Add(instId);
                        }
                    }
                }
            }

            DebugManager.Info( $"Visible recipes: {_visibleRecipes.Count}", "Crafting");
        }

        private void RefreshPreviewAndDetail()
        {
            if (_selected == null || inv == null || _wallet == null)
            {
                detailPanel?.Clear();
                DebugManager.Info($"null?: {_selected},{inv},{_wallet}", "Crafting");
                return;
            }

            var outId = ResolveOutputInventoryId(_selected);
            if (string.IsNullOrEmpty(outId))
            {
                detailPanel?.ShowFail("Ziel-Inventar unbekannt.");
                return;
            }
            _preview = CraftingService.GetPreview(_selected, outId, inv, _wallet);
            DebugManager.Info($"crafting preview {_selected.displayKey}: {_preview.blocker}","Crafting");
            detailPanel?.ShowRecipeDetails(_selected, _preview, GetGoldNeed(_selected), _wallet.GetCurrency("gold"), CountMaterials(_selected));
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

            var outId = ResolveOutputInventoryId(_selected);
            if (string.IsNullOrEmpty(outId))
            {
                detailPanel?.ShowFail("Ziel-Inventar unbekannt.");
                return;
            }

            // Letzte Preview nutzen â€“ Guard-Order ist im Service
            var ok = CraftingService.TryCraftToInventory(_selected, inv, _wallet, outId, out var reason);
            if (!ok)
            {
                DebugManager.Info($"Craft fail: {reason}",TAG);
                detailPanel?.ShowFail(MapBlockerToText(_preview.blocker, reason));
                RefreshPreviewAndDetail(); // erneuern (Mengen/Gold)
                return;
            }

            DebugManager.Log($"Craft success: {_selected.outputItemId} x{_selected.outputCount}", DebugManager.EDebugLevel.Test, TAG);
            detailPanel?.ShowSuccess();
            RefreshPreviewAndDetail(); // Bestand geÃ¤ndert â†’ UI updaten
            
            // TODO: SFX/VFX triggern
        }

        private void HandleSlotChanged(string instanceId, int slotIndex, ItemStackRef? newStack)
        {
            // Nur refreshen, wenn relevante Inventare betroffen sind
            if (_relevantInvIds != null && _relevantInvIds.Contains(instanceId))
            {
                RefreshPreviewAndDetail();
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

        private string ResolveOutputInventoryId(RecipeDef r)
        {
            var gm = GameManager.Instance;
            if (gm == null || r == null) return null;

            if (!gm.TryResolveByItemId(r.outputItemId, out var invType, out var instId) || string.IsNullOrEmpty(instId))
            {
                DebugManager.Error($"Crafting: Unable to resolve output inventory for '{r.outputItemId}'.", "Crafting");
                return null;
            }

            // stellt die Instanz sicher (Slots/Filter via InventoryDef)
            gm.EnsureInstance(instId, invType);
            return instId;
        }

        /// <summary>Ermittelt "have" je benÃ¶tigtem MaterialId (fÃ¼r die Detailanzeige).</summary>
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
                case CraftBlocker.InvalidRefinement: return "Veredelung ungÃ¼ltig.";
                case CraftBlocker.None: return string.IsNullOrEmpty(fallback) ? "Unbekannter Fehler." : fallback;
                default: return string.IsNullOrEmpty(fallback) ? "Unbekannter Fehler." : fallback;
            }
        }
        #endregion

    }

}
