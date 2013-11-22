using UnityEngine;
using System.Collections;

public class MessageUI : MonoBehaviour
{
    public void SetMessage(string message)
    {
        guiText.text = message;
    }
}
