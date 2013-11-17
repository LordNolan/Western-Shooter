using UnityEngine;
using System.Collections;

public class Hitpoints : MonoBehaviour
{
    public int startingHP;
    public int HP { get; private set; }
    
    // Use this for initialization
    void Start()
    {
        HP = startingHP;
    }
    
    void TakeDamage(int amount)
    {
        if((HP -= amount) <= 0)
            Debug.Log(tag + " dead");
    }
}
