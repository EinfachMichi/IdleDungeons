using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameUIManager : MonoBehaviour, ITickable
{
    private void Start()
    {
        GameManager.Instance.AddToRegister(this);
        GameManager.OnCharacterUnlocked += CP_GenerateNewCard;
        
        CP_InitCards();
    }

    public void Tick()
    {
        UpdateUnits();   
    }

    private void UpdateUnits()
    {
        List<Unit> unitsToUpdate = new();
        unitsToUpdate.AddRange(GameManager.Instance.OwnedCharacters);
        foreach (Dungeon dungeon in GameManager.Instance.RunningDungeons)
        {
            unitsToUpdate.Add(dungeon.Enemy);
        }

        foreach (Unit unit in unitsToUpdate)
        {
            U_UpdateUnitUI(unit);
        }
        
        void U_UpdateUnitUI(Unit unit)
        {
            //if (unit == null) return;
            
            print($"Name: {unit.name}, " +
                  $"Level: {unit.Level}, " +
                  $"Health: {unit.Health}/{unit.MaxHealth}, " + 
                  $"Damage: {unit.Damage}"
            );
        }
    }

    #region Character Page CP_

    [Header("Character Page")] 
    [SerializeField] private CharacterCard CP_characterCardPrefab;
    [SerializeField] private Transform CP_cardParent;

    private List<CharacterCard> CP_activeCards = new();

    public void CP_InitCards()
    {
        int cardsToSpawn = GameManager.Instance.OwnedCharacterCount + 1;
        for (int i = 0; i < cardsToSpawn; i++)
        {
            CP_GenerateNewCard();
        }
    }
    
    public void CP_GenerateNewCard()
    {
        if(CP_activeCards.Count != 0)
        {
            CP_activeCards.Last().Unlock();
            CP_activeCards[CP_activeCards.Count - 1].Character = 
                GameManager.Instance.LatestCharacter;
        }
        
        CP_activeCards.Add(Instantiate(
                CP_characterCardPrefab,
                Vector3.zero, 
                Quaternion.identity,
                CP_cardParent
            )
        );
        
        CP_activeCards.Last().Lock();
    }
    
    #endregion
}
