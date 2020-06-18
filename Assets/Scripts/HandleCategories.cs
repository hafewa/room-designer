using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleCategories : MonoBehaviour
{
    public List<GameObject> categories = new List<GameObject>();
    public List<Button> categoryIcons = new List<Button>();
    
    private int _current;

    public void SetCategory(int index)
    {
        if (_current == index) return;
        
        _current = index;

        HighlightIcon(index);
        UpdateObjects(index);
    }

    private void UpdateObjects(int index)
    {
        for (var i = 0; i < categories.Count; i++)
        {
            var category = categories[i];

            category.SetActive(i == index);
        }
    }
    
    private void HighlightIcon(int index)
    {
        for (var i = 0; i < categoryIcons.Count; i++)
        {
            var categoryIcon = categoryIcons[i];
            var colors = categoryIcon.colors;
            var colorsNormalColor = colors.normalColor;
            if (i == index)
            {
                colors.normalColor = new Color(colorsNormalColor.r, colorsNormalColor.g, colorsNormalColor.b, 1);
            }
            else
            {
                colors.normalColor = new Color(colorsNormalColor.r, colorsNormalColor.g, colorsNormalColor.b, 0);
            }

            categoryIcon.colors = colors;
        }
    }
}
