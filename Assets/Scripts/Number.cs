using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TextDirection { Left, Middle, Right}
/// <summary>
/// Klasa odpowiedzialna za, wyświetlani w postaci liczbowej (0-9) czasu rozgrywki oraz ilość zdobytych punktów.
/// </summary>
public class Number : MonoBehaviour {
    //skrypt pobiera łacznie 10 grafik, reprezentujących cyfry 0-9
    [SerializeField]
    //pole przechowujące listę spritów
    Sprite[] Sprites;

    [SerializeField]
    float GridSize = 1f;

    [SerializeField]
    TextDirection TextDirection;

     //wartość, która powinna zostać wyświetlana
    private int _value = 0;
    public int Value {
        get { return _value; }
        set {
            _value = value;
            RefreshNumber();
        }
    }

    List<GameObject> DigitsObject = new List<GameObject>();
    /// <summary>
    /// funkcja odświeżająca interfejs
    /// </summary>
    private void RefreshNumber() 
    {
        RemoveDigits(); // usunięcie wszystkich obiektów jakie składały się na daną aktualnie wyświetlaną liczbę

        var digits = Value
            .ToString()
            .Select(c => int.Parse(c.ToString()))
            .ToArray(); // dzielenie podanej wartości na poszczególne cyfry

        for (int i = 0; i < digits.Count(); i++) // wyświetlenie cyfr
        {
            var position = CalculatePosition(i, digits.Count());
            var digit = CreateDigit(position, digits[i]);
            DigitsObject.Add(digit);
        }
    }
    /// <summary>
    /// usunięcie liczby
    /// </summary>
    private void RemoveDigits() 
    {
        DigitsObject.ForEach(number => Destroy(number));
        DigitsObject.Clear();
    }

    /// <summary>
    /// obliczanie pozycji danej cyfry, obliczana na podstawie 3 wartości: index danej wartości w ramach całej liczby, łączna liczba cyfr w wyświetlanej liczbie, sposoby wyświetlania (lewa, prawa, środek)
    /// </summary>
    private Vector3 CalculatePosition(int index, int numberOfDigits) 
    {
        float result = 0;

        if (TextDirection == TextDirection.Left)
            result = index;
        else if (TextDirection == TextDirection.Middle)
            result = index - numberOfDigits / 2f + 0.5f;
        else if (TextDirection == TextDirection.Right)
            result = index - numberOfDigits + 1;

        return Vector3.right * result * GridSize;

    }
    /// <summary>
    /// Nowe obiekty
    /// </summary>
    /// <param name="position">Ustawia stworzonemu obektowi pozycję</param>
    /// <param name="value">ustawienie odopwiedniego Sprite'u (wyświetlanego obrazka)</param>
    /// <returns>Zwrócenie obektu po to,aby zapisać go w tablicy cyfr</returns>
    private GameObject CreateDigit(Vector3 position, int value) 
    {
        var digit = new GameObject(); // tworzy nowy obiekt
        digit.transform.parent = transform; 
        digit.transform.localPosition = position;

        var sprite = Sprites[value];
        digit.AddComponent<SpriteRenderer>().sprite = sprite; 

        return digit;
    }

}
