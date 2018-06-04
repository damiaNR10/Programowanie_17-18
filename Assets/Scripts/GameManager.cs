using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//klasa zarządzająca wszystkimi najważniejszymi rzeczami w ramach gry
public class GameManager : MonoBehaviour {


    [SerializeField]
    float GameDuration = 60f; // Czas gry (domyślnie 60s)

    private float remainingTime;
    public float RemainingTime  //odliczanie czasu rozgrywki
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
    public int Score //zliczanie punktów
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

        FindObjectOfType<BlockConnection>().OnConnection += UpdateScore; //klasa GameManager subskrybuje zdarzenie OnConnection w ramach klasy BlockConnection; dzięki temu połączeniu jest w stanie trackować wszystkie działania gracza, oraz odpowiednio naliczać punkty

    }



    IEnumerator TimeCounterCoroutine() //korutyna (współprogram) wywoływana w ramach funkcji start, umożliwająca zliczanie czasu
        {
            while(true)
            {
                RemainingTime -= 1f;
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnGameEnded() // funkcja odpowiedzialna za zapisanie stanu gry i przejście do nowej sceny
        {
            PlayerPrefs.SetInt(PlayerPrefsConst.LastGameScore, Score); // zapisywanie poszczególnych wartości w ramach klasy PlayerPrefs do osobnej klasy

            var record = PlayerPrefs.GetInt(PlayerPrefsConst.RecordScore, 0);

            if (record < Score)
                PlayerPrefs.SetInt(PlayerPrefsConst.RecordScore, Score);

            FindObjectOfType<SceneChanger>().ChangeScene(SceneNames.Menu); // zapisywanie poszczególnych scen do osobnej klasy
    }

    private void UpdateScore(int length) // naliczanie punktów: (ilośc połączonych elementów)^2
    {
        Score += length * length;
    }
}
