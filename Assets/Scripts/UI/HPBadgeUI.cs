using UnityEngine;
using System.Collections;

public class HPBadgeUI : MonoBehaviour
{
    public Texture2D badge_full;
    public Texture2D badge_empty;
       
    bool badgeAlive = true;
    private GUITexture badgeTexture;
    
    void Awake()
    {
        badgeTexture = GetComponent<GUITexture>();
        badgeAlive = true;
    }
    
    public void Hit()
    {
        badgeTexture.texture = badge_empty;
        badgeAlive = false;
    }
    
    public void Heal()
    {
        badgeTexture.texture = badge_full;
        badgeAlive = true;
    }
}
