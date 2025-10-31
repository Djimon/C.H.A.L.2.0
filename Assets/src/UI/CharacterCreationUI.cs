using CHAL.Core;
using CHAL.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CHAL.UI
{

    public class CharacterCreationUI : MonoBehaviour
    {

        [SerializeField]
        private string _startSceneName;

        private VisualElement root;

        private Button btnNewGame;
        private Button btnBack;
        private TextField name_input;

        private Color[] colors = new Color[1];


        private void Awake()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            colors[0] = new Color(50 / 255f, 50 / 255f, 180 / 255f);
        }

        private void OnEnable()
        {

            btnNewGame = root.Q<Button>("StartGame");
            btnNewGame.clicked += OnNewGameBtnClicked;

            btnBack = root.Q<Button>("Back");
            btnBack.clicked += OnBackBtnClicked;

            name_input = root.Q<TextField>("InputName");

        }

        private void OnNewGameBtnClicked()
        {
            PlayerProfile profile = new PlayerProfile();
            profile.InitializePlayer(name_input.text, colors);

            GameManager.Instance.StartNewGame(profile);

        }

        private void OnBackBtnClicked()
        {
            gameObject.SetActive(false);
        }
    }
}
