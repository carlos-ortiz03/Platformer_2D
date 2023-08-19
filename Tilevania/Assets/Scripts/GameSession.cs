using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;

    int coins = 0;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = lives + "";
        scoreText.text = coins * 100 + "";
    }

    private void Update()
    {
        scoreText.text = coins * 100 + "";
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            ResetGameSession();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            livesText.text = lives + "";
        }
    }

    private void ResetGameSession()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    public void AddCoin()
    {
        coins += 1;
    }
}
