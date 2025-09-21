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
        DebugManager.Log("Continue: optionen");
        SaveSystem.Load();

        SceneManager.LoadScene(_startSceneName);
    }

    private void OnExitBtnClicked()
    {
        Quit();
    }


    public static void Quit()
    {
        // Vor dem Beenden: persistente Saves / PlayerPrefs sichern
        try { SaveSystem.Save(GameManager.Instance.Profile); } catch { /* ignore */ }
        // Wenn du ein eigenes SaveSystem hast: SaveSystem.Flush(); etc.

        #if UNITY_EDITOR
                // Im Editor: Play Mode stoppen
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL
                // WebGL: kein echtes Quit möglich – zeig einen „Goodbye“-Screen oder navigier zur Startseite
                // Z.B.: ShowGoodbyeOverlay();  // Deine eigene Methode
        #else
                // Standalone (Win/Mac/Linux) & Android
                Application.Quit();
        #endif
         
    }

    private void OnOptoinsBtnClicked()
    {
        DebugManager.Log("ToDo: optionen");
    }


}
