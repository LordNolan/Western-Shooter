using UnityEngine;
using System.Collections;

public class HPBadgeUI : MonoBehaviour
{
    public Texture2D badge_full;
    public Texture2D badge_empty;
    
    private GUITexture badgeTexture;
    
    void Awake()
    {
        badgeTexture = GetComponent<GUITexture>();
    }
    
    public void Hit()
    {
        badgeTexture.texture = badge_empty;
    }
    
    public void Heal()
    {
        badgeTexture.texture = badge_full;
    }
}
