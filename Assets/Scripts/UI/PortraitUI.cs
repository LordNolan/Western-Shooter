using UnityEngine;
using System.Collections;

public class PortraitUI : MonoBehaviour
{
    public Texture2D p_forward;
    public Texture2D p_left;
    public Texture2D p_right;
    
    p_State currentState;
    enum p_State
    {
        forward,
        left,
        right
    }
    
    private GUITexture p_texture;
    
    void Awake()
    {
        p_texture = GetComponent<GUITexture>();
        currentState = p_State.forward;
    }
    
    private double timer = 0;
    private double waitTime = 1;
    void Update()
    {
        if ((timer += Time.deltaTime) > waitTime) {
            SwapTexture();
            timer = 0;
        }
    }
    
    void SwapTexture()
    {
        switch (currentState) {
            case p_State.forward:
                p_texture.texture = p_left;
                currentState = p_State.left;
                break;
            case p_State.left:
                p_texture.texture = p_right;
                currentState = p_State.right;
                break;
            case p_State.right:
                p_texture.texture = p_forward;
                currentState = p_State.forward;
                break;
        }
    }
}
