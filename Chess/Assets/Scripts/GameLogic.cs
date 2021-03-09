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
        }

        NewBoard();
    }


    public void NewBoard()
    {
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);

        HandPiece = new ChessButton(chessButtonPrefub, 99);
        HandPiece.shape = Shape;
        HandPiece.shape.sprite = Empty;

        foreach (var button in ChessButtons)
        {
            if (button.index / 8 == 1)
            {
                button.piece.Color = ChessColor.Black;
                button.piece.Type = ChessPieceType.Pawn;
                button.shape.sprite = BlackPawn;
                button.cursorShape = CursorBlackPawn;
            }
            else if (button.index == 0 || button.index == 7)
            {
                button.piece.Color = ChessColor.Black;
                button.piece.Type = ChessPieceType.Rock;
                button.shape.sprite = BlackRock;
                button.cursorShape = CursorBlackRock;
            }
            else if (button.index == 1 || button.index == 6)
            {
                button.piece.Color = ChessColor.Black;
                button.piece.Type = ChessPieceType.Knight;
                button.shape.sprite = BlackKnight;
                button.cursorShape = CursorBlackKnight;
            }
            else if (button.index == 2 || button.index == 5)
            {
                button.piece.Color = ChessColor.Black;
                button.piece.Type = ChessPieceType.Bishop;
                button.shape.sprite = BlackBishop;
                button.cursorShape = CursorBlackBishop;
            }
            else if (button.index == 3)
            {
                button.piece.Color = ChessColor.Black;
                button.piece.Type = ChessPieceType.Queen;
                button.shape.sprite = BlackQueen;
                button.cursorShape = CursorBlackQueen;
            }
            else if (button.index == 4)
            {
                button.piece.Color = ChessColor.Black;
                button.piece.Type = ChessPieceType.King;
                button.shape.sprite = BlackKing;
                button.cursorShape = CursorBlackKing;
            }
            else if (button.index / 8 == 6)
            {
                button.piece.Color = ChessColor.White;
                button.piece.Type = ChessPieceType.Pawn;
                button.shape.sprite = WhitePawn;
                button.cursorShape = CursorWhitePawn;
            }
            else if (button.index == 56 || button.index == 63)
            {
                button.piece.Color = ChessColor.White;
                button.piece.Type = ChessPieceType.Rock;
                button.shape.sprite = WhiteRock;
                button.cursorShape = CursorWhiteRock;
            }
            else if (button.index == 57 || button.index == 62)
            {
                button.piece.Color = ChessColor.White;
                button.piece.Type = ChessPieceType.Knight;
                button.shape.sprite = WhiteKnight;
                button.cursorShape = CursorWhiteKnight;
            }
            else if (button.index == 58 || button.index == 61)
            {
                button.piece.Color = ChessColor.White;
                button.piece.Type = ChessPieceType.Bishop;
                button.shape.sprite = WhiteBishop;
                button.cursorShape = CursorWhiteBishop;
            }
            else if (button.index == 59)
            {
                button.piece.Color = ChessColor.White;
                button.piece.Type = ChessPieceType.Queen;
                button.shape.sprite = WhiteQueen;
                button.cursorShape = CursorWhiteQueen;
            }
            else if (button.index == 60)
            {
                button.piece.Color = ChessColor.White;
                button.piece.Type = ChessPieceType.King;
                button.shape.sprite = WhiteKing;
                button.cursorShape = CursorWhiteKing;
            }
            else
            {
                button.piece.Color = ChessColor.Non;
                button.piece.Type = ChessPieceType.Non;
                button.shape.sprite = Empty;
                button.cursorShape = null;
            }
        }
    }


    void ButtonClicked(ChessButton button)
    {
        Debug.Log("Button clicked = " + button.index);
        if (HandPiece.shape.sprite==Empty)
        {
            if (button.shape.sprite == Empty)
            {
                return;
            }
            else
            {
                HandPiece.piece = button.piece;
                HandPiece.cursorShape = button.cursorShape;
                HandPiece.shape.sprite = button.shape.sprite;
                HandPiece.tempIndex = button.index;
                Cursor.SetCursor(button.cursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
                button.piece.Type = ChessPieceType.Non;
                button.piece.Color = ChessColor.Non;
                button.cursorShape = null;
                button.shape.sprite = Empty;
            }
        }
        else
        {
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
            button.piece = HandPiece.piece;
            button.cursorShape = HandPiece.cursorShape;
            button.shape.sprite = HandPiece.shape.sprite;
            HandPiece.piece.Type = ChessPieceType.Non;
            HandPiece.piece.Color = ChessColor.Non;
            HandPiece.cursorShape = null;
            HandPiece.shape.sprite = Empty;

        }
    }
   
    public void CursorOverBoard()
    {
        if (HandPiece.cursorShape != null)
        {
            Cursor.SetCursor(HandPiece.cursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
            ChessButtons[HandPiece.tempIndex].shape.sprite = Empty;
        }
    }

    public void CursorOutOfBoard()
    {
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        if (HandPiece.cursorShape != null)
        {
            ChessButtons[HandPiece.tempIndex].shape.sprite = HandPiece.shape.sprite;
        }
    }
}
