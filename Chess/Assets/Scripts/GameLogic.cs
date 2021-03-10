using Assets.Classes;
using ChessLogic;
using System;
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
    private int[] possibleMoves;


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
        foreach (var but in ChessButtons)
        {
            but.Button.image.color = Color.clear;
        }

        Game.NewBoard();
        DrawBoard();
    }

    void DrawBoard()
    {
        for (int i = 0; i < 64; i++)
        {
            if (Game.board1d[i].pieceColor == PieceColor.White)
            {
                switch (Game.board1d[i].pieceType)
                {
                    case PieceType.King:
                        ChessButtons[i].Shape.sprite = WhiteKing;
                        ChessButtons[i].CursorShape = CursorWhiteKing;
                        break;
                    case PieceType.King0:
                        ChessButtons[i].Shape.sprite = WhiteKing;
                        ChessButtons[i].CursorShape = CursorWhiteKing;
                        break;
                    case PieceType.Bishop:
                        ChessButtons[i].Shape.sprite = WhiteBishop;
                        ChessButtons[i].CursorShape = CursorWhiteBishop;
                        break;
                    case PieceType.Queen:
                        ChessButtons[i].Shape.sprite = WhiteQueen;
                        ChessButtons[i].CursorShape = CursorWhiteQueen;
                        break;
                    case PieceType.Pawn:
                        ChessButtons[i].Shape.sprite = WhitePawn;
                        ChessButtons[i].CursorShape = CursorWhitePawn;
                        break;
                    case PieceType.Rock:
                        ChessButtons[i].Shape.sprite = WhiteRock;
                        ChessButtons[i].CursorShape = CursorWhiteRock;
                        break;
                    case PieceType.Rock0:
                        ChessButtons[i].Shape.sprite = WhiteRock;
                        ChessButtons[i].CursorShape = CursorWhiteRock;
                        break;
                    case PieceType.Knight:
                        ChessButtons[i].Shape.sprite = WhiteKnight;
                        ChessButtons[i].CursorShape = CursorWhiteKnight;
                        break;
                }
            }
            else if (Game.board1d[i].pieceColor == PieceColor.Black)
            {
                switch (Game.board1d[i].pieceType)
                {
                    case PieceType.King:
                        ChessButtons[i].Shape.sprite = BlackKing;
                        ChessButtons[i].CursorShape = CursorBlackKing;
                        break;
                    case PieceType.King0:
                        ChessButtons[i].Shape.sprite = BlackKing;
                        ChessButtons[i].CursorShape = CursorBlackKing;
                        break;
                    case PieceType.Bishop:
                        ChessButtons[i].Shape.sprite = BlackBishop;
                        ChessButtons[i].CursorShape = CursorBlackBishop;
                        break;
                    case PieceType.Queen:
                        ChessButtons[i].Shape.sprite = BlackQueen;
                        ChessButtons[i].CursorShape = CursorBlackQueen;
                        break;
                    case PieceType.Pawn:
                        ChessButtons[i].Shape.sprite = BlackPawn;
                        ChessButtons[i].CursorShape = CursorBlackPawn;
                        break;
                    case PieceType.Rock:
                        ChessButtons[i].Shape.sprite = BlackRock;
                        ChessButtons[i].CursorShape = CursorBlackRock;
                        break;
                    case PieceType.Rock0:
                        ChessButtons[i].Shape.sprite = BlackRock;
                        ChessButtons[i].CursorShape = CursorBlackRock;
                        break;
                    case PieceType.Knight:
                        ChessButtons[i].Shape.sprite = BlackKnight;
                        ChessButtons[i].CursorShape = CursorBlackKnight;
                        break;
                }
            }
            else
            {
                ChessButtons[i].Shape.sprite = Empty;
                ChessButtons[i].CursorShape = null;
            }
        }
    }
    

    void ButtonClicked(ChessButton button)
    {
        Debug.Log("Button clicked = " + button.index);
        if (HandPiece.Shape.sprite==Empty)
        {
            if (button.Shape.sprite == Empty || Game.board1d[button.index].pieceColor!=Game.turnColor)
            {
                return;
            }
            else
            {
                possibleMoves = Game.possibleMoves(button.index).ToArray();
                foreach (int index in possibleMoves)
                {
                    ChessButtons[index].Button.image.color = Color.gray;
                }
                button.Button.image.color = Color.red;
                Cursor.SetCursor(button.CursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
                HandPiece.CursorShape = button.CursorShape;
                HandPiece.tempIndex = button.index;
                HandPiece.Shape.sprite = button.Shape.sprite;
                button.Shape.sprite = Empty;
            }
        }
        else
        {
            if (button.index == HandPiece.tempIndex)
            {
                button.Shape.sprite = HandPiece.Shape.sprite;               
            }
            else if (Array.Exists(possibleMoves, element => element == button.index))
            {
                Game.makeMove(HandPiece.tempIndex, button.index);
                DrawBoard();
            }
            else
            {
                return;
            }
            foreach (var but in ChessButtons)
            {
                but.Button.image.color = Color.clear;
            }
            Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
            HandPiece.Shape.sprite = Empty;
            HandPiece.CursorShape = null;
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
