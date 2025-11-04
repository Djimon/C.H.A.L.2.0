using CHAL.Core;
using CHAL.Systems.Map;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{

/// <summary>
/// Manages the user interface for map rewards in the game.
/// Inherits from IngameUI to provide additional functionality.
/// </summary>
    public class MapRewardUI : IngameUI
    {
        private Button btnRetry;
        private Button btnHideout;

        private TextElement detailsText;

        private MapManager mapManager;

        protected override void Awake()
        {
            base.Awake();

            btnRetry = root.Q<Button>("Retry");
            btnRetry.clicked += OnRetryBtnClicked;

            btnHideout = root.Q<Button>("Hideout");
            btnHideout.clicked += OnHideoutBtnClicked;


            detailsText = root.Q<Label>("MapStatus");

            mapManager = FindFirstObjectByType<MapManager>();
        }

/// <summary>
/// Populates the text based on the success status.
/// </summary>
/// <param name="succeded">Indicates whether the operation was successful.</param>
        public void populateText(bool succeded)
        {
            detailsText.text = succeded ? "Successful!" : "Failed!";
            //failcolor: #9F0000 => 160,0,0
            //sucesscolor: #FFD31C => 255,211,28
            detailsText.style.color = succeded ? new Color(1f, 211f / 255, 28f / 255) : new Color(160f / 255, 0f, 0f);
        }

        private void OnHideoutBtnClicked()
        {
            GameManager.Instance.ExitToHideout();
        }

        private void OnRetryBtnClicked()
        {
            mapManager.ResetWave();
        }
    }
}
