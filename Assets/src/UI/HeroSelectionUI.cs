using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Map;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{

/// <summary>
/// Manages the user interface for selecting heroes in the game.
/// </summary>
    public class HeroSelectionUI : IngameUI
    {
        [SerializeField]
        private List<string> availableHeroes;

        private List<string> selectedHeroes;

        private VisualElement heroRoot;
        private ScrollView heroContainer;
        private VisualElement heroDetails;
        private Button chooseHero;

        private string _pendingHero;

        private int maxSlots;

        private VisualElement Slot1;
        private VisualElement Slot2;
        private VisualElement Slot3;
        private VisualElement Slot4;

        private Button btnSlot1Select;
        private Button btnSlot2Select;
        private Button btnSlot3Select;
        private Button btnSlot4Select;

        private int currentSlot;

        private VisualElement imgSlot1Avatar;
        private VisualElement imgSlot2Avatar;
        private VisualElement imgSlot3Avatar;
        private VisualElement imgSlot4Avatar;

        private Button btnStartWave;
        private Button btnExitToHideout;

        private MapManager mapManager;


        protected override void Awake()
        {
            base.Awake();
            root = GetComponent<UIDocument>().rootVisualElement;

            heroContainer = root.Q<ScrollView>("HeroContainer");
            heroRoot = root.Q<VisualElement>("MainFrame");
            heroDetails = root.Q<VisualElement>("DetailsText");
            chooseHero = root.Q<Button>("ChooseHero");
            chooseHero.clicked += OnChooseHeroClicked;
            //hide herocontainer and Detials
            heroRoot.style.visibility = Visibility.Hidden;

            Slot1 = root.Q<VisualElement>("Slot1");
            Slot2 = root.Q<VisualElement>("Slot2");
            Slot3 = root.Q<VisualElement>("Slot3");
            Slot4 = root.Q<VisualElement>("Slot4");


            btnSlot1Select = root.Q<Button>("BtnSlot1");
            btnSlot1Select.clicked += () => OnSlotSelectClicked(1);

            btnSlot2Select = root.Q<Button>("BtnSlot2");
            btnSlot2Select.clicked += () => OnSlotSelectClicked(2);

            btnSlot3Select = root.Q<Button>("BtnSlot3");
            btnSlot3Select.clicked += () => OnSlotSelectClicked(3);

            btnSlot4Select = root.Q<Button>("BtnSlot4");
            btnSlot4Select.clicked += () => OnSlotSelectClicked(4);

            imgSlot1Avatar = root.Q<VisualElement>("IconSlot1");
            imgSlot2Avatar = root.Q<VisualElement>("IconSlot2");
            imgSlot3Avatar = root.Q<VisualElement>("IconSlot3");
            imgSlot4Avatar = root.Q<VisualElement>("IconSlot4");

            btnStartWave = root.Q<Button>("StartWave");
            btnStartWave.clicked += OnStartWaveClicked;

            btnExitToHideout = root.Q<Button>("Hideout");
            btnExitToHideout.clicked += OnExitToHideoutClicked;

        }

/// <summary>
/// Initializes the map manager and sets up the hero slots.
/// </summary>
/// <param name="mapMGR">The MapManager instance to initialize.</param>
        public void Init(MapManager mapMGR)
        {
            mapManager = mapMGR;
            maxSlots = mapManager.CurrentMap.heroSlots;

            var profile = GameManager.Instance.Profile;
            var roster = profile != null ? profile.GetUnlockedHeroes() : System.Array.Empty<string>();
            availableHeroes = new List<string>(roster); // UI-Liste fÃ¼llen


            selectedHeroes = new List<string>(new string[maxSlots]);

            // UI-Slots sichtbar/unsichtbar schalten
            Slot1.style.display = maxSlots >= 1 ? DisplayStyle.Flex : DisplayStyle.None;
            Slot2.style.display = maxSlots >= 2 ? DisplayStyle.Flex : DisplayStyle.None;
            Slot3.style.display = maxSlots >= 3 ? DisplayStyle.Flex : DisplayStyle.None;
            Slot4.style.display = maxSlots >= 4 ? DisplayStyle.Flex : DisplayStyle.None;

        }

        private void OnChooseHeroClicked()
        {
            // Beispiel: aktuellen Slot mit ausgewÃ¤hltem Hero befÃ¼llen
            if (currentSlot <= 0 || string.IsNullOrEmpty(_pendingHero)) return;

            if (selectedHeroes == null || selectedHeroes.Count < maxSlots)
            {
                selectedHeroes = new List<string>(new string[maxSlots]);
            }

            //Bei Wecshel alten hero wieder in die Auswahl geben
            var oldHero = selectedHeroes[currentSlot - 1];
            if (!string.IsNullOrEmpty(oldHero) && !availableHeroes.Contains(oldHero))
            {
                availableHeroes.Add(oldHero);
            }

            //neuen hero setzen
            selectedHeroes[currentSlot - 1] = _pendingHero;
            availableHeroes.Remove(_pendingHero);

            UpdateSlotVisual(currentSlot, _pendingHero);

            _pendingHero = null;
            heroRoot.style.visibility = Visibility.Hidden;
        }

        private void UpdateSlotVisual(int slot, string hero)
        {
            var target = slot switch
            {
                1 => imgSlot1Avatar,
                2 => imgSlot2Avatar,
                3 => imgSlot3Avatar,
                4 => imgSlot4Avatar,
                _ => null
            };

            if (target == null) return;

            if (string.IsNullOrEmpty(hero))
            {
                // leerer Slot â†’ grauer Platzhalter
                target.style.backgroundImage = null;
                target.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f);
            }
            else
            {
                // Held gesetzt â†’ bunte Debug-Farbe
                target.style.backgroundImage = null;
                target.style.backgroundColor = new Color(Random.value, Random.value, Random.value);
                // spÃ¤ter: statt Random.value ein echtes Bild
            }
        }

        private void FillHeroContainer()
        {
            heroContainer.Clear();

            //ToDO: substract the selectedHeroes form the abailable heroes

            foreach (var h in availableHeroes)
            {
                var btn = new Button { text = h }; // spÃ¤ter via Localization
                btn.clicked += () => OnHeroSelected(h);
                heroContainer.Add(btn);
            }
        }

        private void OnExitToHideoutClicked()
        {
            GameManager.Instance.ExitToHideout();
        }

        private void OnStartWaveClicked()
        {
            //ToDO: Give the map the selectedHeroes
            mapManager.SetSelectedHeroes(selectedHeroes);
            mapManager.StartWave();
            Show(false);
        }

        private void OnSlotSelectClicked(int slot)
        {
            currentSlot = slot;

            HighlightSlot(1, false);
            HighlightSlot(2, false);
            HighlightSlot(3, false);
            HighlightSlot(4, false);
            HighlightSlot(slot, true);

            heroRoot.style.visibility = Visibility.Visible;
            FillHeroContainer();
        }

        private void OnHeroSelected(string h)
        {
            _pendingHero = h;
            DebugManager.Log($"Show Details for {_pendingHero}", DebugManager.EDebugLevel.Test, "UI");

            heroDetails.Q<Label>("HeroName").text = h;
            //later: stats, Avatar, etc.
        }

        private void HighlightSlot(int slot, bool active)
        {
            var target = slot switch
            {
                1 => imgSlot1Avatar,
                2 => imgSlot2Avatar,
                3 => imgSlot3Avatar,
                4 => imgSlot4Avatar,
                _ => null
            };

            if (target == null) return;

            target.style.borderLeftColor = active ? new Color(1f, 0.84f, 0f) : new Color(0.4f, 0.4f, 0.4f);
            target.style.borderRightColor = target.style.borderLeftColor;
            target.style.borderTopColor = target.style.borderLeftColor;
            target.style.borderBottomColor = target.style.borderLeftColor;
        }
    }
}
