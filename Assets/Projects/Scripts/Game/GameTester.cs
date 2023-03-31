using UnityEngine;

public class GameTester : MonoBehaviour
{
    private int characterIndex;
    private int dungeonIndex;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Add new Character
            GameManager.Instance.C_UnlockNewCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.EnterDungeon(dungeonIndex, characterIndex);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            dungeonIndex++;
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            characterIndex++;
        }
    }
}
