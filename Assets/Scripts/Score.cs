using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


public class Score : MonoBehaviour
{

    // Variables
    public double ScoreTotal = 0;
    private int spentScore = 0;
    private Text TextScoreText;

    // References
    [SerializeField] private Text Reward_Display;

    private void Start()
    {
        TextScoreText = gameObject.GetComponent<Text>();

        TextScoreText.text = $"Score: {ScoreTotal}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore(int increase) // Increase the score by amount parsed
    {
        ScoreTotal += increase;
        spentScore -= increase;
        TextScoreText.text = $"Score: {ScoreTotal}";
        if (spentScore < 50)
        {
            Reward_Display.text = $"100%";
        }
        else if (spentScore < 100)
        {
            Reward_Display.text = $"80%";
        }
        else if (spentScore < 150)
        {
            Reward_Display.text = $"60%";
        }
        else
        {
            Reward_Display.text = $"40%";
        }
    }
    public void DecreaseScore(int decrease) // Increase the score by amount parsed
    {
        ScoreTotal -= decrease;
        spentScore += decrease;
        TextScoreText.text = $"Score: {ScoreTotal}";
        if (spentScore < 50)
        {
            Reward_Display.text = $"100%";
        }
        else if (spentScore < 100)
        {
            Reward_Display.text = $"80%";
        }
        else if (spentScore < 150)
        {
            Reward_Display.text = $"60%";
        }
        else
        {
            Reward_Display.text = $"40%";
        }
    }

    public void RewardScore(int reward) // Increase the score during gameplay at a rate based on how much they spent on upgrades
    {
        if (spentScore < 50)
        {
            ScoreTotal += reward;
        }
        else if (spentScore < 100)
        {
            ScoreTotal += reward * 0.8;
        }
        else if (spentScore < 150)
        {
            ScoreTotal += reward * 0.6;
        }
        else
        {
            ScoreTotal += reward * 0.4;
        }


        TextScoreText.text = $"Score: {ScoreTotal}";
    }

    // Health Upgrades
    public void UpgradeHealth(int health)
    {
    
    }

    public void setScore(int score)
    {
        ScoreTotal = score;
        TextScoreText.text = $"Score: {ScoreTotal}";
    }

    public void Clean()
    {
        ScoreTotal = 0;
        spentScore = 0;
        TextScoreText.text = $"Score: {ScoreTotal}";
        Reward_Display.text = $"100%";



    }

    
}
