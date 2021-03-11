using Assets.Classes;
using ChessLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [SerializeField] Text GameInfo;
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
    public ChessButton[] VisualBoard = new ChessButton[64];
    private ChessButton HandPiece;
    private ChessGame Game = new ChessGame();
    private int[] possibleMoves;


    void Start()
    {
        for (int i = 0; i < 64; i++)
        {     
            var chessButton = Instantiate(chessButtonPrefub);
            chessButton.transform.SetParent(ChessBoard.transform);
            VisualBoard[i]=(new ChessButton(chessButton, i));
            var temp = i;
            VisualBoard[i].Button.onClick.AddListener(() => ButtonClicked(VisualBoard[temp]));
            VisualBoard[i].Shape = Instantiate(Shape);
            VisualBoard[i].Shape.transform.SetParent(VisualBoard[i].Button.transform);
        }

        NewGame();
    }


    public void NewGame()
    {
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);

        HandPiece = new ChessButton(chessButtonPrefub, 99);
        HandPiece.Shape = Shape;
        HandPiece.Shape.sprite = Empty;
        foreach (var but in VisualBoard)
        {
            but.Button.image.color = Color.clear;
        }

        Game.NewBoard();
        DrawBoard();

        GameInfo.text = "White move first";
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
                        VisualBoard[i].Shape.sprite = WhiteKing;
                        VisualBoard[i].CursorShape = CursorWhiteKing;
                        break;
                    case PieceType.King0:
                        VisualBoard[i].Shape.sprite = WhiteKing;
                        VisualBoard[i].CursorShape = CursorWhiteKing;
                        break;
                    case PieceType.Bishop:
                        VisualBoard[i].Shape.sprite = WhiteBishop;
                        VisualBoard[i].CursorShape = CursorWhiteBishop;
                        break;
                    case PieceType.Queen:
                        VisualBoard[i].Shape.sprite = WhiteQueen;
                        VisualBoard[i].CursorShape = CursorWhiteQueen;
                        break;
                    case PieceType.Pawn:
                        VisualBoard[i].Shape.sprite = WhitePawn;
                        VisualBoard[i].CursorShape = CursorWhitePawn;
                        break;
                    case PieceType.Rock:
                        VisualBoard[i].Shape.sprite = WhiteRock;
                        VisualBoard[i].CursorShape = CursorWhiteRock;
                        break;
                    case PieceType.Rock0:
                        VisualBoard[i].Shape.sprite = WhiteRock;
                        VisualBoard[i].CursorShape = CursorWhiteRock;
                        break;
                    case PieceType.Knight:
                        VisualBoard[i].Shape.sprite = WhiteKnight;
                        VisualBoard[i].CursorShape = CursorWhiteKnight;
                        break;
                }
            }
            else if (Game.board1d[i].pieceColor == PieceColor.Black)
            {
                switch (Game.board1d[i].pieceType)
                {
                    case PieceType.King:
                        VisualBoard[i].Shape.sprite = BlackKing;
                        VisualBoard[i].CursorShape = CursorBlackKing;
                        break;
                    case PieceType.King0:
                        VisualBoard[i].Shape.sprite = BlackKing;
                        VisualBoard[i].CursorShape = CursorBlackKing;
                        break;
                    case PieceType.Bishop:
                        VisualBoard[i].Shape.sprite = BlackBishop;
                        VisualBoard[i].CursorShape = CursorBlackBishop;
                        break;
                    case PieceType.Queen:
                        VisualBoard[i].Shape.sprite = BlackQueen;
                        VisualBoard[i].CursorShape = CursorBlackQueen;
                        break;
                    case PieceType.Pawn:
                        VisualBoard[i].Shape.sprite = BlackPawn;
                        VisualBoard[i].CursorShape = CursorBlackPawn;
                        break;
                    case PieceType.Rock:
                        VisualBoard[i].Shape.sprite = BlackRock;
                        VisualBoard[i].CursorShape = CursorBlackRock;
                        break;
                    case PieceType.Rock0:
                        VisualBoard[i].Shape.sprite = BlackRock;
                        VisualBoard[i].CursorShape = CursorBlackRock;
                        break;
                    case PieceType.Knight:
                        VisualBoard[i].Shape.sprite = BlackKnight;
                        VisualBoard[i].CursorShape = CursorBlackKnight;
                        break;
                }
            }
            else
            {
                VisualBoard[i].Shape.sprite = Empty;
                VisualBoard[i].CursorShape = null;
            }
        }
        GameInfo.text = Game.turnColor + " turn";
    }


    void ButtonClicked(ChessButton button)
    {
        void PickPieceInHand()
        {
            possibleMoves = Game.PossibleMoves(button.index).ToArray();
            foreach (int index in possibleMoves)
            {
                VisualBoard[index].Button.image.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
            }
            button.Button.image.color = new Color(0, 0, 1, 0.6f);
            Cursor.SetCursor(button.CursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
            HandPiece.CursorShape = button.CursorShape;
            HandPiece.tempIndex = button.index;
            HandPiece.Shape.sprite = button.Shape.sprite;
            button.Shape.sprite = Empty;
        }

        if (HandPiece.Shape.sprite==Empty)
        {
            if (button.Shape.sprite == Empty || Game.board1d[button.index].pieceColor!=Game.turnColor)
            {
                return;
            }
            else
            {
                if (!Game.IsDoingCheck(Game.OtherColor(Game.turnColor)))
                {
                    foreach (var but in VisualBoard)
                    {
                        but.Button.image.color = Color.clear;
                    }
                }
                PickPieceInHand();
            }
        }
        else
        {
            if (button.index == HandPiece.tempIndex)
            {
                button.Shape.sprite = HandPiece.Shape.sprite;
                ClearHandPiece();
                ColorCheck();
                return;
            }
            else if (Game.board1d[button.index].pieceColor == Game.board1d[HandPiece.tempIndex].pieceColor)
            {
                ClearHandPiece();
                DrawBoard();
                PickPieceInHand();
                ColorCheck();
                return;
            }
            else if (Array.Exists(possibleMoves, element => element == button.index))
            {
                Game.MakeMove(HandPiece.tempIndex, button.index);
                if (Game.IsDoingCheck(Game.turnColor))
                {
                    ClearHandPiece();
                    ColorCheck();
                    Game.MoveBack();
                    DrawBoard();
                    ColorCheck();
                    GameInfo.text = "Invalid Move";
                    return;
                }
                else if (Game.IsDoingCheck(Game.OtherColor(Game.turnColor)))
                {
                    ClearHandPiece();
                    ColorCheck();
                    DrawBoard();
                    GameInfo.text = Game.turnColor + " under Check.";
                    return;
                }
                DrawBoard();
                ClearHandPiece();
            }
        }
    }
   

    void ClearHandPiece()
    {
        foreach (var but in VisualBoard)
        {
            but.Button.image.color = Color.clear;
        }
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        HandPiece.Shape.sprite = Empty;
        HandPiece.CursorShape = null;
    }

    void ColorCheck()
    {
        for (int first = 0; first < 64; first++)
        {
            if (Game.board1d[first].pieceType ==PieceType.King || Game.board1d[first].pieceType == PieceType.King0)
            {
                for (int second = 0; second < 64; second++)
                {
                    if (Game.IsValidMove(second, first))
                    {
                        VisualBoard[first].Button.image.color = Color.red;
                        VisualBoard[second].Button.image.color = Color.red;
                    }
                }
            }
        }
    }

    public void CursorOverBoard()
    {
        if (HandPiece.CursorShape != null)
        {
            Cursor.SetCursor(HandPiece.CursorShape, new Vector2(20, 30), CursorMode.ForceSoftware);
            VisualBoard[HandPiece.tempIndex].Shape.sprite = Empty;
        }
    }

    public void CursorOutOfBoard()
    {
        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);
        if (HandPiece.CursorShape != null)
        {
            VisualBoard[HandPiece.tempIndex].Shape.sprite = HandPiece.Shape.sprite;
        }
    }

    public void BackMove()
    {
        Game.MoveBack();
        DrawBoard();
    }

    public void ForwardMove()
    {
        Game.MoveForward();
        DrawBoard();
    }
}
