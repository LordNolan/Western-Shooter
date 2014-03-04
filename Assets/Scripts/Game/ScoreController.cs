using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour
{
    public int score { get; private set; }
    public float multiplierTime;

    int scoreMulti = 1;
    int killCounter = 0;
    float currentTime;
    bool isMulti = false;

    ScoreUI ui;

    void Start()
    {
        ui = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<ScoreUI>();
    }

    public void AddScore(int amount)
    {
        score += scoreMulti * amount;
        ui.UpdateScoreboard(score);
        isMulti = true;
        currentTime = 0;
        killCounter++;
        RecalculateMulti();
    }

    void RecalculateMulti()
    {
        scoreMulti = Mathf.FloorToInt(killCounter / 3) + 1;
        Debug.Log(scoreMulti);
    }

    public void ResetScore()
    {
        score = 0;
        scoreMulti = 1;
    }

    void Update()
    {
        if (isMulti && ((currentTime += Time.deltaTime) >= multiplierTime)) {
            scoreMulti = 1;
            currentTime = 0;
            killCounter = 0;
            isMulti = false;
        }
    }
}

/*
 * score multiplier
 * on first kill, start one second timer.
 * if timer expires, reset kill counter.
 * if another kill occurs during timer, timer resets and kill count increments
 * for every 3 kills in kill counter, multiplier increased by 1
 */