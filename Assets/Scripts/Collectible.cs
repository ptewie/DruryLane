using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName = "Collectible")]
public class Collectible : ScriptableObject
{
    public string collectibleName;
    public GameObject collectiblePrefab;
    public int scoreValue;
}
