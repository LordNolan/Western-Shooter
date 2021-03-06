﻿using UnityEngine;
using System.Collections;

public class LoadingBehavior : MonoBehaviour
{	
    bool loadingDone = false;
    
    void Update()
    {
        // if not done loading and world gen complete, show everything and hide load elements including camera
        if (!loadingDone && GlobalParams.IsWorldGenComplete()) {
            loadingDone = true;
            camera.enabled = false;                                       // disable our camera
            transform.GetChild(0).guiTexture.enabled = false;             // remove "Loading..."
            GetComponent<AudioListener>().enabled = false;                // turn off our audiolistener
            GameObject.FindWithTag("Floor").SendMessage("ShowElement");   // show game world floor
            GameObject.FindWithTag("UI").BroadcastMessage("ShowElement"); // show HUD
        }
    }
    
    public void ShowLoadingScreen()
    {
        loadingDone = false;
        camera.enabled = true;
        transform.GetChild(0).guiTexture.enabled = true;
        GetComponent<AudioListener>().enabled = true;
        GameObject.FindWithTag("Floor").SendMessage("HideElement");
        GameObject.FindWithTag("UI").BroadcastMessage("HideElement");
    }
}
