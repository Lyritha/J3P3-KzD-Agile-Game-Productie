using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CapitalDisplay : MonoBehaviour
{
    public List<CapitalItemDisplay> CapitalItemDisplay { get; private set; } = new();

    [SerializeField]
    private CapitalItemDisplay capitalItemPrefab;
    [SerializeField]
    private RectTransform capitalParent;
    [SerializeField]
    private TMP_Text capitalNumber;

    public void AddCapitalItems(List<Personage> characters) => AddCapitalItems(characters.ToArray());
    public void AddCapitalItems(Personage[] characters)
    {
        ClearCapitalItems();
        for (int i = 0; i < characters.Length; i++)
        {
            Personage character = characters[i];
            AddCapitalItem(character, i + 1);

            if (character != null) character.OnChanged -= RefreshUI;
            character.OnChanged += RefreshUI;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        int number = 0;

        foreach (CapitalItemDisplay item in CapitalItemDisplay)
            if (item.Character != null) number += item.Character.baseKapitaal;

        capitalNumber.text = $"{number}.";
    }

    private void AddCapitalItem(Personage character, int index)
    {
        CapitalItemDisplay display = Instantiate(capitalItemPrefab, capitalParent);
        CapitalItemDisplay.Add(display);
        display.SetUI(character, index);
    }

    private void ClearCapitalItems()
    {
        foreach (Transform child in capitalParent)
            Destroy(child.gameObject);
    
        CapitalItemDisplay.Clear();
    }


}
