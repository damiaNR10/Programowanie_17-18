using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa odpowiedzialna za generowanie planszy na której umieszczone zostaną bloki
/// </summary>
public class Board : MonoBehaviour {
	
	[SerializeField]
	GameObject BlockPrefab;

	[SerializeField]
	[Range(3,8)] //atrybut Range pozwalający limitować wartości w ramach danej zmiennej (nie działa z poziomu kodu), jest stosowany przede wszystkim w ramach inspektora Unity3D
	int Width = 5;

	[SerializeField]
	[Range(3,10)]
	int Height = 6;

	[SerializeField]
	[Range(0.5f, 5f)]
	float GridSize = 2f;

	[SerializeField]
	[Range(0.5f, 2f)]
	public float BlockSize = 1f;

   public  Block[,] Blocks { get; private set; }

	/// <summary>
    /// Inicjalizacja
    /// </summary>
	void Start () 
	{
		GenerateBoard ();
	}

    /// <summary>
    /// Aktualizacja jest wywoływana raz na klatkę, z każdą klatką gry następuje aktualizacja pozycji, wielkości i koloru bloku
    /// </summary>
    void Update () {
		
	}
    /// <summary>
    /// funkcja generująca wszystkie bloki na planszy
    /// </summary>
	private void GenerateBoard() 
	{
        Blocks = new Block[Width, Height];


        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Blocks[x, y] = GenerateBlock(x, y);
            }
                
        }
			
	}
    /// <summary>
    /// Funkcja generująca pojedynczy blok na określonych pozycjach
    /// </summary>
	private Block GenerateBlock(int x, int y) 
	{
		var obj = Instantiate (BlockPrefab);
		obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one * BlockSize;

        var block = obj.GetComponent<Block>();
        block.Configure(x, y);
        return block;
	}
    /// <summary>
    /// Funkcja określająca położenie na podstawie koordynatów
    /// </summary>
	public Vector2 GetBlockPosition(int x, int y) 
	{
		var basePosition = new Vector2 (
			x - Width / 2f + 0.5f,
			y - Height / 2f + 0.5f);
		return basePosition * GridSize;
	}
    /// <summary>
    /// funkcja usuwająca wszystkie zadane bloki
    /// </summary>
	public void RemoveBlocks(List<Block> connectedBlocks) 
	{
		connectedBlocks.ForEach (b => Blocks [b.X, b.Y] = null); // odnajdujemy wszystkie bloki > resetujemy i usuwamy
		connectedBlocks.ForEach (b => Destroy(b.gameObject)); // usuwamy dany block
	}
    /// <summary>
    /// funkcja odpowiedzialna za: usunięcie pustych miejsc w ramach planszy, przesunięcie wszystkich obiektów w dół, tak aby puste miejsca już nie występowały i utworzenie nowych obiektów w ramach pustych przestrzeni na górze planszy
    /// </summary>
	public void RefreshBlocks() 
	{
		for (int x = 0; x < Width; x++) // iteracja po szerokości planszy
		{
			int h = 0;
			for(int y=0; y < Height; y++) // iteracja po wysokości planszy
			{
				if (Blocks [x, y] == null) // sprawdzanie czy bloki nie są puste
					continue;

				Blocks[x, h] = Blocks[x, y];
				Blocks[x, h].Configure(x, h);

				h++;
			}

			for (int y = h; y < Height; y++) 
			{
				Blocks [x, y] = GenerateBlock (x, y);
				Blocks [x, y].PlaceOnTargetPosition (); // Nowe bloki bojawiają się w miejscach starych (usuniętych) bloków
			}

				
		}
	}

}
