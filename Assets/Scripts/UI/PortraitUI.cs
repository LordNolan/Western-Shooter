using UnityEngine;
using System.Collections;

public class PortraitUI : MonoBehaviour
{
    public Texture2D hp4_c;
    public Texture2D hp4_l;
    public Texture2D hp4_r;
    
    public Texture2D hp3_c;
    public Texture2D hp3_l;
    public Texture2D hp3_r;
    
    public Texture2D hp2_c;
    public Texture2D hp2_l;
    public Texture2D hp2_r;
    
    public Texture2D hp1_c;
    public Texture2D hp1_l;
    public Texture2D hp1_r;
    
    public Texture2D rage_c;
    public Texture2D rage_l;
    public Texture2D rage_r;
    
    public Texture2D collectTexture;
    public Texture2D deadTexture;
    
    private Texture2D current_C;
    private Texture2D current_L;
    private Texture2D current_R;
    
    public int hp4Amount;
    public int hp3Amount;
    public int hp2Amount;
    public int hp1Amount;
    
    private double timer = 0;
    private double waitTime = 1;
    private double collectTimer = 0;
    private double collectWaitTime = .5;
    private GUITexture currentTexture;
    private p_State currentState;
    private bool enraged;
    private bool collecting;
    private int currentHP;
    enum p_State
    {
        forward,
        left,
        right
    }
    
    public void PlayerEnraged()
    {
        enraged = true;
        current_C = rage_c;
        current_L = rage_l;
        current_R = rage_r;
        ApplyTextureChange();
    }
    
    public void PlayerStopEnraged()
    {
        enraged = false;
        CheckForHPTextureChange(currentHP);
        ApplyTextureChange();
    }
    
    public void SetPlayerHitpoints(int HP)
    {
        currentHP = HP;
        collecting = false;
        if (!enraged) {
            CheckForHPTextureChange(HP);
            ApplyTextureChange();
        }
    }
    
    public void PickupItem()
    {
        collecting = true;
        current_C = collectTexture;
        current_L = collectTexture;
        current_R = collectTexture;
        ApplyTextureChange();
    }
    
    void ApplyTextureChange()
    {
        switch (currentState) {
            case p_State.forward:
                currentTexture.texture = current_C;
                break;
            case p_State.left:
                currentTexture.texture = current_L;
                break;
            case p_State.right:
                currentTexture.texture = current_R;
                break;
        }
    }
    
    void CheckForHPTextureChange(int hpLeft)
    {
        if (hpLeft < hp4Amount) {
            if (hpLeft < hp3Amount) {
                if (hpLeft < hp2Amount) {
                    if (hpLeft < hp1Amount) {
                        if (hpLeft <= 0) {
                            current_C = deadTexture;
                            current_L = deadTexture;
                            current_R = deadTexture;
                        }
                    } else {
                        current_C = hp1_c;
                        current_L = hp1_l;
                        current_R = hp1_r;
                    }
                } else {
                    current_C = hp2_c;
                    current_L = hp2_l;
                    current_R = hp2_r;
                }
            } else {
                current_C = hp3_c;
                current_L = hp3_l;
                current_R = hp3_r;
            }
        } else {
            current_C = hp4_c;
            current_L = hp4_l;
            current_R = hp4_r;
        }
    }
    
    void Awake()
    {
        currentTexture = GetComponent<GUITexture>();
        currentState = p_State.forward;
        current_C = hp4_c;
        current_L = hp4_l;
        current_R = hp4_r;
    }
    
    void Update()
    {
        if (collecting && (collectTimer += Time.deltaTime) > collectWaitTime) {
            collectTimer = 0;
            collecting = false;
            CheckForHPTextureChange(currentHP);
            ApplyTextureChange();
        }
        
        if ((timer += Time.deltaTime) > waitTime) {
            SwapTexture();
            timer = 0;
        }
    }
    
    void SwapTexture()
    {
        switch (currentState) {
            case p_State.forward:
                currentTexture.texture = current_C;
                currentState = p_State.left;
                break;
            case p_State.left:
                currentTexture.texture = current_L;
                currentState = p_State.right;
                break;
            case p_State.right:
                currentTexture.texture = current_R;
                currentState = p_State.forward;
                break;
        }
    }
}


/*
static final Image[][] headshots = { { hp1_c, hp1_l, hp1_r } ... };
static final int[] hpLimits = { 5, 4, 2, 1, 0 };

public void playerHit(int hpLeft) {
    int z = 0;
    while(hpLeft < hpLimits[z]) { z++; }
    current_C = headshots[z][0];
    current_L = headshots[z][1];
    current_R = headshots[z][2];
    ApplyTextureChange();
}
*/