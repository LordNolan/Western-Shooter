using UnityEngine;
using System.Collections;

public class EnemiesUI : MonoBehaviour
{
    public void SetEnemiesLeft(int amount)
    {
        guiText.text = "Enemies Left: " + amount;
    }
}
