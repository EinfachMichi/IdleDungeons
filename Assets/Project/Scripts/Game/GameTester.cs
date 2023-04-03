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
            GameManager.Instance.UnlockNewCharacter();
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

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D))
        {
            dungeonIndex--;
            if (dungeonIndex < 0) dungeonIndex = 0;
        }
        
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
        {
            characterIndex--;
            if (characterIndex < 0) characterIndex = 0;
        }
    }
}
