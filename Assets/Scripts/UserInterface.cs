using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Klasa odpowiedzialna za aktualizowanie wartości na ekranie - pozostałego czasu, oraz ilości zdobytych punktów
/// </summary>
public class UserInterface : MonoBehaviour {


    /// <summary>
    /// referencje do obiektów w przestrzeni trójwymiarowej
    /// </summary>
    [SerializeField]
    Number TimeCounter;

    [SerializeField]
    Number ScoreCounter;


    /// <summary>
    /// Subskrypcja na dwóch zdarzeniach; dzięki temu można odseparować logikę aktualizacji interfejsu użytkownika od klasy GameManager 
    /// </summary>
    void Start () {
        
        FindObjectOfType<GameManager>().OnRemainingTimeChanged += time => 
            TimeCounter.Value = (int)time;

        FindObjectOfType<GameManager>().OnScoreChanged += score =>
            ScoreCounter.Value = (int)score;
    }
}
