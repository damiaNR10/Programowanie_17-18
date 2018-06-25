using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

/// <summary>
/// Klasa odpowiedzialna za łączenie bloków
/// </summary>

public class BlockConnection : MonoBehaviour {
    /// <summary>
    /// lista aktualnie połączonych bloków
    /// </summary>
    List<Block> ConnectedBlocks = new List<Block>();
    /// <summary>
    /// aktualnie używany kolor
    /// </summary>
    private BlockColor? CurrentColor; 
	private Board Board;
	private LineRenderer LineRenderer;
    /// <summary>
    /// informuje o ilości połączonych bloków
    /// </summary>
	public event Action<int> OnConnection; 

	void Awake ()
	{
		Board = FindObjectOfType<Board>();
		LineRenderer = GetComponent<LineRenderer> ();
	}

    void Start() {

    }
    void Update() {
		if (Input.GetMouseButtonUp (0))
			FinishConnection ();
    }

    /// <summary>
    ///  funkcja odpowiedzialna za łączenie poszczególnych elementów planszy
    /// </summary>
    public void Connect(Block block)
    {
		if (!Input.GetMouseButton(0))
            return; // sprawdzenie czy naciśnięty jest LPM, lub (w wersji mobilnej) czy gracz dotyka ekranu

        if (ConnectedBlocks.Contains(block))
            return; // sprawdzenie czy dany blok nie jest już zawarty w ramach ścieżki

        if (!CurrentColor.HasValue)
            CurrentColor = block.Color;

        if (CurrentColor != block.Color)
            return; // sprawdzenie czy dany blok ma poprawne kolory

        if (ConnectedBlocks.Count() >= 1 && !ConnectedBlocks.Last().IsNeighbour(block))
            return; // sprawdzenie czy dany blok jest sądziadem ostatnio połączonego bloku

        block.IsConnected = true; // aktualizacja wartości IsConnected
        ConnectedBlocks.Add(block); //dodanie bloku do odpowiedniej listy

		RefreshConnector();
    }
    /// <summary>
    /// funkcja wyświetlana w momencie, gdy użytkownik przestawał dotykać ekran
    /// </summary>
    private void FinishConnection()
    {

        ConnectedBlocks
            .ForEach(block => block.IsConnected = false);

		if (ConnectedBlocks.Count >= 3) // sprawdzenie czy gracz połączył przynajmniej 3 bloki
		{
			if (OnConnection != null)
				OnConnection.Invoke (ConnectedBlocks.Count);
			Board.RemoveBlocks (ConnectedBlocks); // usuwanie poszczególnych bloków
			Board.RefreshBlocks (); // przesunięcie poszczególnych elementów na planszy
		}

		
		ConnectedBlocks.Clear ();

        CurrentColor = null;
		RefreshConnector();
    }
    /// <summary>
    /// funkcja odświażająca konektor, czyli wizualnie reprezentowała połączone elementy gry
    /// </summary>

    private void RefreshConnector() 
	{
		var points = ConnectedBlocks
			.Select(block => Board.GetBlockPosition(block.X, block.Y))
			.Select(position => (Vector3)position + Vector3.back * 2f)
			.ToArray();

		LineRenderer.positionCount = points.Length;
		LineRenderer.SetPositions(points);
	}
}
