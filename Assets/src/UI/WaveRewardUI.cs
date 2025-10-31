using CHAL.Core;
using CHAL.Systems.Map;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{

    public class WaveRewardUI : IngameUI
    {

        private Button btnRetry;
        private Button btnNext;
        private Button btnHideout;

        private TextElement detailsText;

        private MapManager mapManager;

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

            mapManager = FindFirstObjectByType<MapManager>();
        }

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
            GameManager.Instance.ExitToHideout();
        }

        private void OnNexBtnClicked()
        {
            mapManager.NextWave();
        }

        private void OnRetryBtnClicked()
        {
            mapManager.StartWave();
        }
    }
}
