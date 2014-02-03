using UnityEngine;
using System.Collections;

public class ScoreValue : MonoBehaviour
{
    public int pointsValue;
    
    public void AwardPoints()
    {
        GameObject.FindWithTag("Global").GetComponent<ScoreController>().AddScore(pointsValue);
    }
}
