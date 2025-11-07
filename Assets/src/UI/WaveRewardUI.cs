using CHAL.Core;
using CHAL.Systems.Map;
using CHAL.Systems.Wave;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{

/// <summary>
/// Manages the user interface for wave rewards in the game.
/// </summary>
    public class WaveRewardUI : IngameUI
    {

        private Button btnRetry;
        private Button btnNext;
        private Button btnHideout;

        private TextElement detailsText;

        private Toggle _autoStartToggle;
        private Label _autoStartCountdown;

        private bool _snapshotAutoStartThisScreen = false;
        private bool _countdownStartedThisScreen = false;
        private Coroutine _autoStartCoroutine;

        private MapManager mapManager;
        private WaveManager waveManager;

        protected override void Awake()
        {
            base.Awake();
            root = GetComponent<UIDocument>().rootVisualElement;

            btnRetry = root.Q<Button>("Retry");
            btnRetry.clicked += OnRetryBtnClicked;

            btnNext = root.Q<Button>("Next");
            btnNext.clicked += OnNexBtnClicked;

            btnHideout = root.Q<Button>("Hideout");
            btnHideout.clicked += OnHideoutBtnClicked;

            detailsText = root.Q<Label>("WaveStatus");

            _autoStartToggle = root.Q<Toggle>("AutoStartToggle");
            _autoStartCountdown = root.Q<Label>("AutoStartCountdown");

            if (_autoStartToggle == null)
            {
                DebugManager.Warning("WaveRewardUI: 'AutoStartToggle' not found in UXML (check name=\"AutoStartToggle\")", "UI");
            }

            if (_autoStartCountdown == null)
            {
                DebugManager.Warning("WaveRewardUI: 'AutoStartCooldown' not found in UXML (check name=\"AutoStartToggle\")", "UI");
            }

            mapManager = FindFirstObjectByType<MapManager>();
            waveManager = FindFirstObjectByType<WaveManager>();


            // Toggle -> nur globalen Flag setzen; Countdown NIE hier starten
            _autoStartToggle.RegisterValueChangedCallback(evt =>
            {
                mapManager.SetAutoStartAllWaves(evt.newValue);

                // Wenn während eines laufenden Countdowns ausgeschaltet wird → abbrechen
                if (!evt.newValue && _autoStartCoroutine != null)
                {
                    StopCoroutine(_autoStartCoroutine);
                    _autoStartCoroutine = null;
                    HideCountdown();
                    DebugManager.Info("AutoStart countdown cancelled by user", "UI");
                }
            });
        }

        public override void Show(bool visible)
        { 
            base.Show(visible);


            if (visible)
            {
                // Snapshot des Flags NUR beim Öffnen
                _snapshotAutoStartThisScreen = mapManager.AutoStartAllWaves;
                _countdownStartedThisScreen = false;

                // UI-Toggle mit aktuellem Map-Flag synchronisieren (ohne Callback-Schleife)
                _autoStartToggle.SetValueWithoutNotify(mapManager.AutoStartAllWaves);

                // Countdown nur starten, wenn:
                // - Flag beim Öffnen true
                // - es eine nächste Wave gibt
                // - aktuelle Wave erfolgreich abgeschlossen wurde (falls du eine solche Info hast)
                var hasNextWave = mapManager.HasNextWave(); // stelle sicher, dass diese API existiert
                var lastWave = !hasNextWave;

                if (_snapshotAutoStartThisScreen && hasNextWave /* && success == true falls vorhanden */)
                {
                    StartAutoStartCountdown();
                }
                else
                {
                    HideCountdown();
                }
            }
            else
            {
                // Beim Schließen Sicherheit: Timer stoppen
                if (_autoStartCoroutine != null)
                {
                    StopCoroutine(_autoStartCoroutine);
                    _autoStartCoroutine = null;
                }
                HideCountdown();
            }

        }


        private void StartAutoStartCountdown()
        {
            if (_countdownStartedThisScreen) return; // Doppelstart verhindern

            _countdownStartedThisScreen = true;
            _autoStartCoroutine = StartCoroutine(AutoStartCountdownRoutine(5));
        }

        private IEnumerator AutoStartCountdownRoutine(int seconds)
        {
            for (int t = seconds; t > 0; t--)
            {
                ShowCountdown($"Starting next wave in {t}...");
                yield return new WaitForSeconds(1f);

                // Falls UI unterwegs geschlossen wurde oder Toggle deaktiviert: abbrechen
                if (!mapManager.AutoStartAllWaves)
                {
                    HideCountdown();
                    _autoStartCoroutine = null;
                    yield break;
                }
            }

            HideCountdown();
            _autoStartCoroutine = null;

            // Sicherstellen, dass immer noch AutoStart aktiv ist + es eine nächste Wave gibt
            if (mapManager.AutoStartAllWaves && mapManager.HasNextWave())
            {
                DebugManager.Info("AutoStart: starting next wave", "Wave");
                mapManager.NextWave(); // oder dein bestehender Weg "Next"-Button-Handler aufzurufen
            }
        }

        /// <summary>
        /// Updates the details text based on the success status.
        /// </summary>
        /// <param name="succeded">Indicates whether the operation was successful.</param>
        public void populateText(bool succeded)
        {
            detailsText.text = succeded ? "Successful!" : "Failed!";
            //failcolor: #9F0000 => 160,0,0
            //sucesscolor: #FFD31C => 255,211,28
            detailsText.style.color = succeded ? new Color(1f, 211f / 255, 28f / 255) : new Color(160f / 255, 0f, 0f);
            DebugManager.Log("Text updated");
        }

        private void OnHideoutBtnClicked()
        {
            if (_autoStartCoroutine != null)
            {
                StopCoroutine(_autoStartCoroutine);
                _autoStartCoroutine = null;
            }
            HideCountdown();

            GameManager.Instance.ExitToHideout();
        }

        private void OnNexBtnClicked()
        {
            if (_autoStartCoroutine != null)
            {
                StopCoroutine(_autoStartCoroutine);
                _autoStartCoroutine = null;
            }
            HideCountdown();

            mapManager.NextWave();
        }

        private void OnRetryBtnClicked()
        {
            if (_autoStartCoroutine != null)
            {
                StopCoroutine(_autoStartCoroutine);
                _autoStartCoroutine = null;
            }
            HideCountdown();

            mapManager.StartWave();
        }

        private void ShowCountdown(string text)
        {
            if (_autoStartCountdown != null)
            {
                _autoStartCountdown.style.display = DisplayStyle.Flex;
                _autoStartCountdown.text = text;
            }
        }

        private void HideCountdown()
        {
            if (_autoStartCountdown != null)
            {
                _autoStartCountdown.style.display = DisplayStyle.None;
                _autoStartCountdown.text = string.Empty;
            }
        }

    }
}

