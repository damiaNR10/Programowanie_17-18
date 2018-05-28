using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    [SerializeField]
    float GameDuration = 60f; // Czas gry (domyślnie 60s)

    private float remainingTime;
    public float RemainingTime
    {
        get { return remainingTime;  }
        set
        {
            remainingTime = value;

            if (remainingTime < 0f) // Jeśli czas gry < 0 = zakończenie gry
                OnGameEnded();

            if (OnRemainingTimeChanged != null)
                OnRemainingTimeChanged.Invoke(remainingTime);
        }
    }


    public event Action<float> OnRemainingTimeChanged;

    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(score);
        }
    }
        public event Action<int> OnScoreChanged;


    // Use this for initialization
    void Start () {

        Score = 0;

        RemainingTime = GameDuration;
        StartCoroutine(TimeCounterCoroutine());

        FindObjectOfType<BlockConnection>().OnConnection += UpdateScore;

    }



    IEnumerator TimeCounterCoroutine()
        {
            while(true)
            {
                RemainingTime -= 1f;
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnGameEnded()
        {
            SceneManager.LoadScene("menu");
        }

    private void UpdateScore(int length)
    {
        Score += length * length;
    }
}
