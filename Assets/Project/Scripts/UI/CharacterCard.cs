using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : MonoBehaviour, ITickable
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject unlockedPanel;
    [SerializeField] private GameObject lockedPanel;

    public Character Character
    {
        get => character;
        set => character = value;
    }
    
    private Character character;
    private bool isLocked;

    private void Start()
    {
        GameManager.Instance.AddToRegister(this);
    }

    public void Tick()
    {
        if (isLocked) return;
        
        UpdateInfos();   
    }
    
    public void UpdateInfos()
    {
        nameText.text = $"Name: {character.Name}";
        levelText.text = $"Level: {character.Level}";
    }

    public void Unlock()
    {
        isLocked = false;
        unlockedPanel.SetActive(true);
        lockedPanel.SetActive(false);
    }

    public void Lock()
    {
        isLocked = true;
        unlockedPanel.SetActive(false);
        lockedPanel.SetActive(true);
    }
}
