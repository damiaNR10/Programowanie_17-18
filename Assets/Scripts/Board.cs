using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Use this for initialization
	void Start () 
	{
		GenerateBoard ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void GenerateBoard() //funkcja generująca wszystkie bloki na planszy
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
	private Block GenerateBlock(int x, int y) //funkcja generująca pojedynczy blok na określonych pozycjach
	{
		var obj = Instantiate (BlockPrefab);
		obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one * BlockSize;

        var block = obj.GetComponent<Block>();
        block.Configure(x, y);
        return block;
	}
	public Vector2 GetBlockPosition(int x, int y) //funkcja określająca położenie na podstawie koordynatów
	{
		var basePosition = new Vector2 (
			x - Width / 2f + 0.5f,
			y - Height / 2f + 0.5f);
		return basePosition * GridSize;
	}
	public void RemoveBlocks(List<Block> connectedBlocks) //funkcja usuwająca wszystkie zadane bloki
	{
		connectedBlocks.ForEach (b => Blocks [b.X, b.Y] = null); // odnajdujemy wszystkie bloki > resetujemy i usuwamy
		connectedBlocks.ForEach (b => Destroy(b.gameObject)); // usuwamy dany block
	}
	public void RefreshBlocks() // funkcja odpowiedzialna za: usunięcie pustych miejsc w ramach planszy, przesunięcie wszystkich obiektów w dół, tak aby puste miejsca już nie występowały i utworzenie nowych obiektów w ramach pustych przestrzeni na górze planszy
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
