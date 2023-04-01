using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Dungeon/New ItemData")]
public abstract class ItemData : ScriptableObject
{
    public string Name;
    public ItemType ItemType;
}
