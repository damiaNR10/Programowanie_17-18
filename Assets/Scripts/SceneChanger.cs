using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))] // płynne przejście okien
public class SceneChanger : MonoBehaviour
{// klasa pozwalająca na dodanie efektu dotyczącego zmiany sceny (aktualny ekran staje się biały, natomiast po załadowaniu sceny następuje płynne przejście na właściwy obraz)
    [SerializeField]
    float FadeDuration = 1f;

    Image Blend;

    void Start()
    {
        Blend = CreateBlend();
        StartCoroutine(FadeInCoroutine()); //gdy załaduje się nowa scena zostanie uruchomiona korutyna FadeInCoroutine, której działanie jest analogiczne do działania FadeOutCoroutine z tą różnicą, że zmieniony zostaje poziom przezroczystości z wartości 1 do wartości 0
    }

    Image CreateBlend() // tworzenie blendy, czyli obiektu który będzie zajmował całą przestrzeń na ekranie i będzie odpowiedzialny za zmianę swojego stopnia przezroczystości
    {
        var obj = new GameObject();
        obj.transform.parent = transform;

        var rectTransform = obj.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero; // ustawienie blendy na środku, oraz tak, aby zajmował cały ekran

        obj.AddComponent<CanvasRenderer>(); //dodanie do obiektu odpowiedniego komponentu renderującego 

        var image = obj.AddComponent<Image>();
        image.color = Color.white; //dodanie do obiektu pustego obrazu o białym kolorze

        obj.SetActive(false); //wyłączenie aktywności obiektu tak, aby nie był widoczny

        return image;
    }

    public void ChangeScene(string name) //udostępnienie publicznej funkcji ChangeScene wraz z argumentem czyli nazwą sceny do załadowania, która odpowiedzialna jest za uruchomienie korutyny FadeOutCoroutine
    {
        StartCoroutine(FadeOutCoroutine(name));
    }

    IEnumerator FadeInCoroutine()
    {
        Blend.gameObject.SetActive(true);

        while (Blend.color.a > 0f)
        {
            Blend.color -= new Color(0f, 0f, 0f, 1f) / FadeDuration * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Blend.gameObject.SetActive(false);
    }

    IEnumerator FadeOutCoroutine(string sceneName)
    {

        Blend.gameObject.SetActive(true); //uruchomienie obiektu tak, aby był widoczny

        while (Blend.color.a > 1f)
        {
            Blend.color += new Color(0f, 0f, 0f, 1f) / FadeDuration * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }//manipulacja wartością przezroczystości tak długo, aż będzie ona równa 1 (biały obiekt będzie zasłaniał wszystkie pozostałe

        Blend.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName); //zmiana sceny
    }
}
