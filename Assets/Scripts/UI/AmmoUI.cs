using UnityEngine;
using System.Collections;

public class AmmoUI : MonoBehaviour 
{   
    public void SetAmmoCount(int amount)
    {
        guiText.text = ": + amount";
    }
}
