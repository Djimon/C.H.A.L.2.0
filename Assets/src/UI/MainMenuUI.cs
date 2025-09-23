using CHAL.Core;
using CHAL.Data;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private string _startSceneName;

    public GameObject characterCreationMenue;

    private VisualElement root;

    private Button btnNew;
    private Button btnContinue;
    private Button btnOptions;
    private Button btnExit;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void Start()
    {
        if (GameManager.Instance?.Profile == null)
        {
            btnContinue.SetEnabled(false);
        }
        else 
        {
            btnContinue.SetEnabled(true);
        }
            
    }

    private void OnEnable()
    {

        btnNew = root.Q<Button>("NewGame");
        btnNew.clicked += OnStartBtnClicked;

        btnContinue = root.Q<Button>("Continue");
        btnContinue.clicked += OnContinueBtnClicked;

        btnOptions = root.Q<Button>("Options");
        btnOptions.clicked += OnOptoinsBtnClicked;

        btnExit = root.Q<Button>("Exit");
        btnExit.clicked += OnExitBtnClicked;
    }

    private void OnStartBtnClicked()
    {
        characterCreationMenue.SetActive(true);
    }

    private void OnContinueBtnClicked()
    {
        DebugManager.Log("Continue game", DebugManager.EDebugLevel.Test, "UI");
        GameManager.Instance.ContinueGame();
    }

    private void OnExitBtnClicked()
    {
        GameManager.Quit();
    }

    private void OnOptoinsBtnClicked()
    {
        DebugManager.Log("ToDo: optionen");
    }


}
