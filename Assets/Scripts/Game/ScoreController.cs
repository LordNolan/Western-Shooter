using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour
{
    public int score { get; private set; }
    ScoreUI ui;

    void Start()
    {
        ui = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUI>();
    }

    public void AddScore(int amount)
    {
        score += amount;
        ui.UpdateScoreboard(score);
    }

    public void ResetScore()
    {
        score = 0;
    }
}
