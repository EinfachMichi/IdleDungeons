using UnityEngine;

public class GameUIManager : MonoBehaviour, ITickable
{
    [SerializeField] private Navigation navigation;
    [SerializeField] private TextDisplay textDisplay;

    private Page currentPage = Page.Characters;
    
    private void Start()
    {
        GameManager.Instance.AddToRegister(this);
        
        UpdateDisplaysPerTick();
        NavigationButtonClicked(0);
    }

    public void Tick()
    {
        UpdateDisplaysPerTick();
    }

    #region Buttons

    public void NavigationButtonClicked(int index)
    {
        navigation.UpdatePair(index);
        currentPage = (Page) index;
        textDisplay.UpdateHeader(currentPage);
    }

    #endregion

    #region Updates

    private void UpdateDisplaysPerTick()
    {
        textDisplay.UpdateGold(GameManager.Instance.Gold);
    }

    #endregion
}

public enum Page
{
    Characters = 0,
    Dungeons = 1,
    Inventory = 2,
    Profile = 3,
    Settings = 4
}