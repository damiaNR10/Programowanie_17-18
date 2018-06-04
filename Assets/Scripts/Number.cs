using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TextDirection { Left, Middle, Right}

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

    private void RefreshNumber() //funkcja odświeżająca interfejs
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

    private void RemoveDigits() //usunięcie liczby
    {
        DigitsObject.ForEach(number => Destroy(number));
        DigitsObject.Clear();
    }

    private Vector3 CalculatePosition(int index, int numberOfDigits) // obliczanie pozycji danej cyfry, obliczana na podstawie 3 wartości: index danej wartości w ramach całej liczby, łączna liczba cyfr w wyświetlanej liczbie, sposoby wyświetlania (lewa, prawa, środek)
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

    private GameObject CreateDigit(Vector3 position, int value) 
    {
        var digit = new GameObject(); // tworzy nowy obiekt
        digit.transform.parent = transform; 
        digit.transform.localPosition = position; // ustawia stworzonemu obiektowi pozycję

        var sprite = Sprites[value];
        digit.AddComponent<SpriteRenderer>().sprite = sprite; // ustawienie odopwiedniego Sprite'u (wyświetlanego obrazka)

        return digit; // zwrócenie tego obiektu po to, aby zapisać go w tablicy cyfr
    }

}
