using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Navigation
{
    [SerializeField] private UIPair[] uipairs;

    private int currentPair; 
        
    public void UpdatePair(int index)
    {
        currentPair = index;
        for (int i = 0; i < uipairs.Length; i++)
        {
            if(currentPair == i)
            {
                uipairs[i].DisableButton();
                uipairs[i].EnablePanel();
                continue;
            }
            
            uipairs[i].EnableButton();
            uipairs[i].DisablePanel();
        }
    }
    
    [Serializable]
    class UIPair
    {
        public Button Button;
        public GameObject Panel;

        public void Enable()
        {
            Button.interactable = true;
            Panel.SetActive(true);
        }

        public void Disable()
        {
            Button.interactable = false;
            Panel.SetActive(false);
        }

        public void DisablePanel() => Panel.SetActive(false);
        public void EnablePanel() => Panel.SetActive(true);
        public void DisableButton() => Button.interactable = false;
        public void EnableButton() => Button.interactable = true;
    }
}
