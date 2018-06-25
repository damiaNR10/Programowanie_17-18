using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Klasa odpowiedzialna za menu główne
/// </summary>
public class MenuUserInterface : MonoBehaviour {

    /// <summary>
    /// Aktyalny zdobyty wynik w grze
    /// </summary>
    [SerializeField]
    Number LastGameScore;
    /// <summary>
    /// Rekord
    /// </summary>
    [SerializeField]
    Number RecordScore;

    /// <summary>
    /// Inicjalizacja
    /// </summary>
    void Start () {
        LastGameScore.Value = PlayerPrefs.GetInt(PlayerPrefsConst.LastGameScore, 0);
        RecordScore.Value = PlayerPrefs.GetInt(PlayerPrefsConst.RecordScore, 0);

    }
    /// <summary>
    /// Zmiana sceny
    /// </summary>
    public void LoadGame() 
    {
        FindObjectOfType<SceneChanger>().ChangeScene(SceneNames.Game);
        SceneManager.LoadScene(SceneNames.Game);
    }

}
