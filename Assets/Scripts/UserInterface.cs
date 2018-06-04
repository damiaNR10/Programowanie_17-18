using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {
    // klasa odpowiedzialna za aktualizowanie wartości na ekranie - pozostałego czasu, oraz ilości zdobytych punktów

    //referencje do obiektów w przestrzeni trójwymiarowej
    [SerializeField]
    Number TimeCounter;

    [SerializeField]
    Number ScoreCounter;


	// Use this for initialization
	void Start () {
        //sybskrypcja na dwóch zdarzeniach; dzięki temu można odseparować logikę aktualizacji interfejsu użytkownika od klasy GameManager 
        FindObjectOfType<GameManager>().OnRemainingTimeChanged += time =>
            TimeCounter.Value = (int)time;

        FindObjectOfType<GameManager>().OnScoreChanged += score =>
            ScoreCounter.Value = (int)score;
    }
}
