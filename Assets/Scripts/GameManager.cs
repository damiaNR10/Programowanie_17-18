using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Główna klasa, która steruje grą oraz jej wszystkimi funkcjonalnościami
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// Czas gry
    /// </summary>
    [SerializeField]
    float GameDuration = 60f; // Czas gry (domyślnie 60s)

    private float remainingTime;
    /// <summary>
    /// odliczanie czasu rozgrywki
    /// </summary>
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
    /// <summary>
    /// zliczanie punktów
    /// </summary>
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


    /// <summary>
    /// Używane do inicjalzacji
    /// </summary>
    void Start () {

        Score = 0;

        RemainingTime = GameDuration;
        StartCoroutine(TimeCounterCoroutine());

        FindObjectOfType<BlockConnection>().OnConnection += UpdateScore; //klasa GameManager subskrybuje zdarzenie OnConnection w ramach klasy BlockConnection; dzięki temu połączeniu jest w stanie trackować wszystkie działania gracza, oraz odpowiednio naliczać punkty

    }


    /// <summary>
    /// korutyna (współprogram) wywoływana w ramach funkcji start, umożliwająca zliczanie czasu
    /// </summary>
    IEnumerator TimeCounterCoroutine() 
        {
            while(true)
            {
                RemainingTime -= 1f;
                yield return new WaitForSeconds(1f);
            }
        }
    /// <summary>
    /// funkcja odpowiedzialna za zapisanie stanu gry i przejście do nowej sceny
    /// </summary>
    private void OnGameEnded() 
        {
            PlayerPrefs.SetInt(PlayerPrefsConst.LastGameScore, Score); // zapisywanie poszczególnych wartości w ramach klasy PlayerPrefs do osobnej klasy

            var record = PlayerPrefs.GetInt(PlayerPrefsConst.RecordScore, 0);

            if (record < Score)
                PlayerPrefs.SetInt(PlayerPrefsConst.RecordScore, Score);

            FindObjectOfType<SceneChanger>().ChangeScene(SceneNames.Menu); // zapisywanie poszczególnych scen do osobnej klasy
    }
    /// <summary>
    /// naliczanie punktów: (ilośc połączonych elementów)^2
    /// </summary>
    private void UpdateScore(int length) 
    {
        Score += length * length;
    }
}
