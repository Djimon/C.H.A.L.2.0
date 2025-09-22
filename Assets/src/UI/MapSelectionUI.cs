using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapSelectionUI : MonoBehaviour
{
    public VisualTreeAsset listItemTemplate; // dein UXML Template für EIN Item
    public List<ItemData> items;

    private ListView listView;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        listView = root.Q<ListView>("item-list");

        // --- makeItem: wie sieht ein Element aus ---
        listView.makeItem = () =>
        {
            // UXML-Kopie
            var element = listItemTemplate.CloneTree();
            return element;
        };

        // --- bindItem: Daten ins Template füllen ---
        listView.bindItem = (element, i) =>
        {
            var title = element.Q<Label>("item-title");
            var desc = element.Q<Label>("item-desc");
            var icon = element.Q<VisualElement>("item-icon");

            title.text = items[i].Title;
            desc.text = items[i].Description;
            if (items[i].Icon != null)
                icon.style.backgroundImage = new StyleBackground(items[i].Icon);
        };

        listView.itemsSource = items;
        listView.selectionType = SelectionType.Single;

        // --- Event: Auswahl ändern ---
        listView.selectionChanged += OnSelectionChange;
    }

    private void OnSelectionChange(IEnumerable<object> selectedItems)
    {
        foreach (var item in selectedItems)
        {
            Debug.Log("Ausgewählt: " + ((ItemData)item).Title);
        }
    }
}
