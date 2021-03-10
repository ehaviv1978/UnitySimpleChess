using Assets.Classes;
using ChessLogic;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] Button chessButtonPrefub;
    [SerializeField] Image Shape;
    [SerializeField] GridLayoutGroup ChessBoard;
    [SerializeField] Sprite BlackPawn;
    [SerializeField] Sprite BlackRock;
    [SerializeField] Sprite BlackKing;
    [SerializeField] Sprite BlackQueen;
    [SerializeField] Sprite BlackKnight;
    [SerializeField] Sprite BlackBishop;
    [SerializeField] Sprite WhitePawn;
    [SerializeField] Sprite WhiteRock;
    [SerializeField] Sprite WhiteKing;
    [SerializeField] Sprite WhiteQueen;
    [SerializeField] Sprite WhiteKnight;
    [SerializeField] Sprite WhiteBishop;
    [SerializeField] Sprite Empty;
    [SerializeField] Texture2D CursorBlackRock;
    [SerializeField] Texture2D CursorBlackKing;
    [SerializeField] Texture2D CursorBlackQueen;
    [SerializeField] Texture2D CursorBlackBishop;
    [SerializeField] Texture2D CursorBlackKnight;
    [SerializeField] Texture2D CursorBlackPawn;
    [SerializeField] Texture2D CursorWhiteRock;
    [SerializeField] Texture2D CursorWhiteKing;
    [SerializeField] Texture2D CursorWhiteQueen;
    [SerializeField] Texture2D CursorWhiteBishop;
    [SerializeField] Texture2D CursorWhiteKnight;
    [SerializeField] Texture2D CursorWhitePawn;
    private List<ChessButton> ChessButtons = new List<ChessButton>();
    private ChessButton HandPiece;
    private ChessGame Game = new ChessGame();


    void Start()
    {
        for (int i = 0; i < 64; i++)
        {     
            var chessButton = Instantiate(chessButtonPrefub);
            chessButton.transform.SetParent(ChessBoard.transform);
            ChessButtons.Add(new ChessButton(chessButton, i));
            var temp = i;
            ChessButtons[i].Button.onClick.AddListener(() => ButtonClicked(ChessButtons[temp]));
            ChessButtons[i].Shape = Instantiate(Shape);
            ChessButtons[i].Shape.transform.SetParent(ChessButtons[i].Button.transform);
        }

        NewBoard();
    }


    public void NewBoard()
    {
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);

        HandPiece = new ChessButton(chessButtonPrefub, 99);
        HandPiece.Shape = Shape;
        HandPiece.Shape.sprite = Empty;

        foreach (var button in ChessButtons)
        {
            if (button.index / 8 == 1)
            {
                button.Piece.Color = PieceColor.Black;
                button.Piece.Type = PieceType.Pawn;
                button.Shape.sprite = BlackPawn;
                button.CursorShape = CursorBlackPawn;
            }
            else if (button.index == 0 || button.index == 7)
            {
                button.Piece.Color = PieceColor.Black;
                button.Piece.Type = PieceType.Rock0;
                button.Shape.sprite = BlackRock;
                button.CursorShape = CursorBlackRock;
            }
            else if (button.index == 1 || button.index == 6)
            {
                button.Piece.Color = PieceColor.Black;
                button.Piece.Type = PieceType.Knight;
                button.Shape.sprite = BlackKnight;
                button.CursorShape = CursorBlackKnight;
            }
            else if (button.index == 2 || button.index == 5)
            {
                button.Piece.Color = PieceColor.Black;
                button.Piece.Type = PieceType.Bishop;
                button.Shape.sprite = BlackBishop;
                button.CursorShape = CursorBlackBishop;
            }
            else if (button.index == 3)
            {
                button.Piece.Color = PieceColor.Black;
                button.Piece.Type = PieceType.Queen;
                button.Shape.sprite = BlackQueen;
                button.CursorShape = CursorBlackQueen;
            }
            else if (button.index == 4)
            {
                button.Piece.Color = PieceColor.Black;
                button.Piece.Type = PieceType.King0;
                button.Shape.sprite = BlackKing;
                button.CursorShape = CursorBlackKing;
            }
            else if (button.index / 8 == 6)
            {
                button.Piece.Color = PieceColor.White;
                button.Piece.Type = PieceType.Pawn;
                button.Shape.sprite = WhitePawn;
                button.CursorShape = CursorWhitePawn;
            }
            else if (button.index == 56 || button.index == 63)
            {
                button.Piece.Color = PieceColor.White;
                button.Piece.Type = PieceType.Rock0;
                button.Shape.sprite = WhiteRock;
                button.CursorShape = CursorWhiteRock;
            }
            else if (button.index == 57 || button.index == 62)
            {
                button.Piece.Color = PieceColor.White;
                button.Piece.Type = PieceType.Knight;
                button.Shape.sprite = WhiteKnight;
                button.CursorShape = CursorWhiteKnight;
            }
            else if (button.index == 58 || button.index == 61)
            {
                button.Piece.Color = PieceColor.White;
                button.Piece.Type = PieceType.Bishop;
                button.Shape.sprite = WhiteBishop;
                button.CursorShape = CursorWhiteBishop;
            }
            else if (button.index == 59)
            {
                button.Piece.Color = PieceColor.White;
                button.Piece.Type = PieceType.Queen;
                button.Shape.sprite = WhiteQueen;
                button.CursorShape = CursorWhiteQueen;
            }
            else if (button.index == 60)
            {
                button.Piece.Color = PieceColor.White;
                button.Piece.Type = PieceType.King0;
                button.Shape.sprite = WhiteKing;
                button.CursorShape = CursorWhiteKing;
            }
            else
            {
                button.Piece.Color = PieceColor.Non;
                button.Piece.Type = PieceType.Non;
                button.Shape.sprite = Empty;
                button.CursorShape = null;
            }
        }
    }


    void ButtonClicked(ChessButton button)
    {
        Debug.Log("Button clicked = " + button.index);
        if (HandPiece.Shape.sprite==Empty)
        {
            if (button.Shape.sprite == Empty)
            {
                return;
            }
            else
            {
                HandPiece.Piece = button.Piece;
                HandPiece.CursorShape = button.CursorShape;
                HandPiece.Shape.sprite = button.Shape.sprite;
                HandPiece.tempIndex = button.index;
                Cursor.SetCursor(button.CursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
                button.Piece.Type = PieceType.Non;
                button.Piece.Color = PieceColor.Non;
                button.CursorShape = null;
                button.Shape.sprite = Empty;
            }
        }
        else
        {
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
            button.Piece = HandPiece.Piece;
            button.CursorShape = HandPiece.CursorShape;
            button.Shape.sprite = HandPiece.Shape.sprite;
            HandPiece.Piece.Type = PieceType.Non;
            HandPiece.Piece.Color = PieceColor.Non;
            HandPiece.CursorShape = null;
            HandPiece.Shape.sprite = Empty;

        }
    }
   
    public void CursorOverBoard()
    {
        if (HandPiece.CursorShape != null)
        {
            Cursor.SetCursor(HandPiece.CursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
            ChessButtons[HandPiece.tempIndex].Shape.sprite = Empty;
        }
    }

    public void CursorOutOfBoard()
    {
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        if (HandPiece.CursorShape != null)
        {
            ChessButtons[HandPiece.tempIndex].Shape.sprite = HandPiece.Shape.sprite;
        }
    }
}
