using Assets.Classes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public Button chessButtonPrefub;
    public Image Shape;
    public GridLayoutGroup ChessBoard;
    public Sprite BlackPawn;
    public Sprite BlackRock;
    public Sprite BlackKing;
    public Sprite BlackQueen;
    public Sprite BlackKnight;
    public Sprite BlackBishop;
    public Sprite WhitePawn;
    public Sprite WhiteRock;
    public Sprite WhiteKing;
    public Sprite WhiteQueen;
    public Sprite WhiteKnight;
    public Sprite WhiteBishop;
    public Sprite Empty;
    public Texture2D CursorBlackRock;
    private List<ChessButton> ChessButtons = new List<ChessButton>();
    
    
    void Start()
    {

        for (int i = 0; i < 64; i++)
        {     
            var chessButton = Instantiate(chessButtonPrefub);
            ChessButtons.Add(new ChessButton(chessButton, i));
            chessButton.transform.SetParent(ChessBoard.transform);
        }

        foreach (var button in ChessButtons)
        {
            button.button.onClick.AddListener(() => ButtonClicked(button));
            button.shape = Instantiate(Shape);
            button.shape.transform.SetParent(button.button.transform);
            if (button.index/8 == 1)
            {
                button.shape.sprite = BlackPawn;
            } 
            else if (button.index / 8 == 6)
            {
                button.shape.sprite = WhitePawn;
            }
            else if (button.index == 0 || button.index== 7)
            {
                button.shape.sprite = BlackRock;
                button.cursorShape = CursorBlackRock;
            }
            else if (button.index == 1 || button.index == 6)
            {
                button.shape.sprite = BlackKnight;
            }
            else if (button.index == 2 || button.index == 5)
            {
                button.shape.sprite = BlackBishop;
            }
            else if (button.index == 3)
            {
                button.shape.sprite = BlackQueen;
            }
            else if (button.index == 4)
            {
                button.shape.sprite = BlackKing;
            }
            else if (button.index == 56 || button.index == 63)
            {
                button.shape.sprite = WhiteRock;
            }
            else if (button.index == 57 || button.index == 62)
            {
                button.shape.sprite = WhiteKnight;
            }
            else if (button.index == 58 || button.index == 61)
            {
                button.shape.sprite = WhiteBishop;
            }
            else if (button.index == 59)
            {
                button.shape.sprite = WhiteQueen;
            }
            else if (button.index == 60)
            {
                button.shape.sprite = WhiteKing;
            }
        }
    }


    void ButtonClicked(ChessButton button)
    {
        Debug.Log("Button clicked = " + button.index);
        if (button.shape.sprite != Empty)
        {
            Cursor.SetCursor(button.cursorShape, new Vector2(20,30), CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        }
    }
}
