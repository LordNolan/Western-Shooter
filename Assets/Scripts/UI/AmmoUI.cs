using UnityEngine;
using System.Collections;

public class AmmoUI : MonoBehaviour 
{
    public void AmmoSpent(int newAmount)
    {
          guiText.text = ": " + newAmount;
    }
}
