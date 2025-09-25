using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Map;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MapSelectionUI : IngameUI
{
    [SerializeField]
    private string mapSceneName = "04_Map";
    [SerializeField]
    private List<MapDef> availableMaps;

    private MapDef _selectedMap;

    private Button btnStartMap;
    private Button btnExitMenu;
    private TextElement detailsText;

    protected override void Awake()
    {
        base.Awake();   
        root = GetComponent<UIDocument>().rootVisualElement;

        // Beispiel: baue Buttons für jede Map
        var container = root.Q<VisualElement>("MapList");
        container.Clear();

        foreach (var map in availableMaps)
        {
            var btn = new Button { text = map.displayNameKey }; // später via Localization
            btn.clicked += () => OnMapSelected(map);
            container.Add(btn);
        }

        btnStartMap = root.Q<Button>("StartMap");
        btnStartMap.clicked += OnStartMapBtnClicked;

        btnExitMenu = root.Q<Button>("Exit");
        btnExitMenu.clicked += OnExitMenuBtnClicked;

        detailsText = root.Q<Label>("Details");

        //später: Buttons um schwierigkeit zu ändern, sobald diese freiheschaltet wurden
    }

    private void OnExitMenuBtnClicked()
    {
        Show(false);
    }

    private void OnMapSelected(MapDef map)
    {
        _selectedMap = map;
        detailsText.text = $"{map.displayNameKey} (Monster-Level {map.baseLevel})";
    }

    private void OnStartMapBtnClicked()
    {
        if (_selectedMap == null)
        {
            DebugManager.Warning("No map selected!", "UI");
            return;
        }

        GameManager.Instance.StartMap(mapSceneName,_selectedMap); // zentral!
    }
}
