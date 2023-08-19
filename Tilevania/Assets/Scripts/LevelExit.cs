using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float slowMoFactor = .2f;

    bool levelDone = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!levelDone)
        {
            StartCoroutine(LevelComplete());
            levelDone = true;
        }
    }

    private IEnumerator LevelComplete()
    {
        Time.timeScale = slowMoFactor;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
