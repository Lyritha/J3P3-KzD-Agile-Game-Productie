using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Capital : MonoBehaviour
{
    [SerializeField]
    private CapitalItemDisplay capitalItemPrefab;
    [SerializeField]
    private RectTransform capitalParent;
    [SerializeField]
    private TMP_Text capitalNumber;

    private List<CapitalItemDisplay> capitalItemDisplay = new();

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

        foreach (CapitalItemDisplay item in capitalItemDisplay)
            if (item.Character != null) number += item.Character.baseKapitaal;

        capitalNumber.text = $"{number}";
    }

    private void AddCapitalItem(Personage character, int index)
    {
        CapitalItemDisplay display = Instantiate(capitalItemPrefab, capitalParent);
        capitalItemDisplay.Add(display);
        display.SetUI(character, index);
    }

    private void ClearCapitalItems()
    {
        foreach (Transform child in capitalParent)
            Destroy(child.gameObject);
    
        capitalItemDisplay.Clear();
    }

    public void ClearCapitalItem(Personage personage)
    {
        for (int i = 0;i < capitalItemDisplay.Count; i++)
        {
            CapitalItemDisplay item = capitalItemDisplay[i];

            bool isSame = item.Character == personage;
            if (isSame)
            {
                Destroy(item.gameObject);
                capitalItemDisplay.RemoveAt(i);
            }
        }

        RefreshUI();
    }
}
