using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// deklaracja możliwych kolorów dla bloków 
/// </summary>
public enum BlockColor { Red, Green, Blue, Yellow, Magenta, Gray}
/// <summary>
/// Klasa służąca do mapowania wartości pola enumerowanego BlockColor na odpowiedni Sprite
/// </summary>
[System.Serializable]
class BlockType 
{
    public BlockColor Color;
    public Sprite Sprite;
}
/// <summary>
/// Klasa Block odpowiedzialna jest za wyświetlanie bloków na planszy.
/// </summary>
public class Block : MonoBehaviour
{

    [SerializeField]
    BlockType[] BlockTypes;

    public int X { get; private set; } 
    public int Y { get; private set; } //położenie bloków na planszy

    public BlockColor Color { get; private set; }
    /// <summary>
    /// czy blok jest "połączony" (jest w składzie ścieżki)
    /// </summary>
    public bool IsConnected; 

    private Vector3 TargetPosition;
    private Board Board; 
    private BlockConnection BlockConnection;

    private SpriteRenderer SpriteRenderer;


    private void Awake()
    {
        Board = FindObjectOfType<Board>();
        BlockConnection = FindObjectOfType<BlockConnection>();

        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Inicjalizacja. Losowany jest jeden z kolorów i przypisany zostaje danemu blokowi
    /// </summary>
    void Start()
    {

        Color = GetRandomColor(); 
        SetSprite();

    }

    /// <summary>
    /// Aktualizacja jest wywoływana raz na klatkę, z każdą klatką gry następuje aktualizacja pozycji, wielkości i koloru bloku
    /// </summary>
    void Update()
    {
        UpdatePosition(); 
        UpdateScale();
        UpdateColor();
    }
    /// <summary>
    /// Funkcja odpowiadająca za akutalizowanie pozycji
    /// </summary>
    private void UpdatePosition()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, Time.deltaTime * 5f);
    }
    /// <summary>
    /// decyzja na temat skali i Coloru (poniżej) podejmowana zostaje na podstawie tego czy dany blok jest połączony z innymi
    /// </summary>
    private void UpdateScale()
    {
        var targetScale = IsConnected ? 0.8f : 1f; 
        targetScale *= Board.BlockSize;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale * Vector3.one, Time.deltaTime * 5f);
    }
    /// <summary>
    /// Zmiana koloru zaznaczonego bloku
    /// </summary>
    private void UpdateColor()
    {
        var targetColor = IsConnected ? new Color(1f, 1f, 1f, 0.8f) : UnityEngine.Color.white;
        SpriteRenderer.color = UnityEngine.Color.Lerp(SpriteRenderer.color, targetColor, Time.deltaTime * 5f);
    }
    /// <summary>
    /// funkcja odpowiedzialna za przypisanie poszczególnych właściwości
    /// </summary>
    public void Configure(int x, int y)
    {
        X = x;
        Y = y;

        TargetPosition = Board.GetBlockPosition(x, y);
        IsConnected = false;
    }
    /// <summary>
    ///  funkcja służąca do ustawienia bloku na określonej pozycji z pominięciem animacji
    /// </summary>
	public void PlaceOnTargetPosition()
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
    /// <summary>
    /// Resetowanie koloru
    /// </summary>
    public void ResetColor()
    {
        Color = GetRandomColor();
        SetSprite();
    }
    ///<summary> 
    /// funkcja pozwalająca określić czy dane 2 bloki są ze sobą sąsiadami; użyta zostaje wartośc bezwzględna
    ///</summary>
    public bool IsNeighbour(Block other) 
    {
        if (Mathf.Abs(X - other.X) > 1f) 
            return false;

        if (Mathf.Abs(Y - other.Y) > 1f)
            return false;

        return true;
    }
	/// <summary>
    /// Funkcja odpowiedzialna za naciśnięcie przycisku
    /// </summary>
    private void OnMouseEnter()
    {
		ConnectBlock();
    }
    /// <summary>
    /// Funkcja odpowiedzialna za puszczenie wcześniej naciśniętego przycisku
    /// </summary>
	private void OnMouseDown()
	{
		ConnectBlock();
	}
    /// <summary>
    /// Funkcja odpowiedzialna połączone bloki
    /// </summary>
	private void ConnectBlock()
	{
		BlockConnection.Connect (this);
	}

}