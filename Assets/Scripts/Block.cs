using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BlockColor { Red, Green, Blue, Yellow, Magenta, Gray} //deklaracja możliwych kolorów dla bloków 

[System.Serializable]
class BlockType //klasa służąca do mapowania wartości pola enumerowanego BlockColor na odpowiedni Sprite
{
    public BlockColor Color;
    public Sprite Sprite;
}

public class Block : MonoBehaviour
{

    [SerializeField]
    BlockType[] BlockTypes;

    public int X { get; private set; } 
    public int Y { get; private set; } //położenie bloków na planszy

    public BlockColor Color { get; private set; }

    public bool IsConnected; //czy blok jest "połączony" (jest w składzie ścieżki)

    private Vector3 TargetPosition;
    private Board Board; //referencje do klas Board i BlockConnection
    private BlockConnection BlockConnection;

    private SpriteRenderer SpriteRenderer;


    private void Awake()
    {
        Board = FindObjectOfType<Board>();
        BlockConnection = FindObjectOfType<BlockConnection>();

        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {

        Color = GetRandomColor(); //losowany jest jeden z kolorów i przypisany zostaje danemu blokowi
        SetSprite();

    }

    // Update is called once per frame
    void Update()
    {
        //z każdą klatkągry następuje aktualizacja pozycji, wielkości i koloru bloku
        UpdatePosition(); 
        UpdateScale();
        UpdateColor();
    }

    private void UpdatePosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, Time.deltaTime * 5f);
    }

    private void UpdateScale()
    {
        var targetScale = IsConnected ? 0.8f : 1f; //decyzja na temat skali i Coloru (poniżej) podejmowana zostaje na podstawie tego czy dany blok jest połączony z innymi
        targetScale *= Board.BlockSize;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale * Vector3.one, Time.deltaTime * 5f);
    }

    private void UpdateColor()
    {
        var targetColor = IsConnected ? new Color(1f, 1f, 1f, 0.8f) : UnityEngine.Color.white;
        SpriteRenderer.color = UnityEngine.Color.Lerp(SpriteRenderer.color, targetColor, Time.deltaTime * 5f);
    }

    public void Configure(int x, int y) //funkcja odpowiedzialna za przypisanie poszczególnych właściwości
    {
        X = x;
        Y = y;

        TargetPosition = Board.GetBlockPosition(x, y);
        IsConnected = false;
    }

	public void PlaceOnTargetPosition() //funkcja służąca do ustawienia bloku na określonej pozycji z pominięciem animacji
	{
		transform.localPosition = TargetPosition;
		transform.localScale = Vector3.zero;
		SpriteRenderer.color = new Color(1f, 1f, 1f, 0f);
	}

    public static BlockColor GetRandomColor()
    {
        var values = System.Enum.GetValues(typeof(BlockColor));
        var index = Random.Range(0, values.Length);

        return (BlockColor)values.GetValue(index);
    }

    private void SetSprite()
    {
        var sprite = BlockTypes.First(type => type.Color == Color).Sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void ResetColor()
    {
        Color = GetRandomColor();
        SetSprite();
    }

    public bool IsNeighbour(Block other) //funkcja pozwalająca określić czy dane 2 bloki są ze sobą sąsiadami; użyta zostaje wartośc bezwzględna
    {
        if (Mathf.Abs(X - other.X) > 1f) 
            return false;

        if (Mathf.Abs(Y - other.Y) > 1f)
            return false;

        return true;
    }
		
    private void OnMouseEnter()
    {
		ConnectBlock();
    }
	private void OnMouseDown()
	{
		ConnectBlock();
	}
	private void ConnectBlock()
	{
		BlockConnection.Connect (this);
	}

}