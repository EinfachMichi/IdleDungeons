using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextDisplay
{
    [SerializeField] private TextMeshProUGUI pageHeaderText;
    [SerializeField] private TextMeshProUGUI goldText;

    public void UpdateHeader(Page page) => pageHeaderText.text = page.ToString();
    public void UpdateGold(float value) => goldText.text = $"Gold: {value}";
}
