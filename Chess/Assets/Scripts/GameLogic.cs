﻿using Assets.Classes;
using ChessLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    [SerializeField] Texture2D CursorHand;
    public ChessButton[] VisualBoard = new ChessButton[64];
    private ChessButton HandPiece;
    private ChessGame Game = new ChessGame();
    private ChessAI AI;
    private int[] possibleMoves;
    private bool vsComputer = true;
    public int computerLvl = 4;
    Button[] allButtons;


    void Start()
    {
        AI = new ChessAI(Game, PieceColor.Black, computerLvl);
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
        allButtons = GameObject.FindObjectsOfType<Button>();
        NewGame();
    }


    void DisableAllButtons()
    {
        foreach(var button in allButtons)
        {
            button.interactable = false;
        }
    }

    void EnableAllButtons()
    {
        foreach (var button in allButtons)
        {
            button.interactable = true;
        }
    }

    public void NewGame()
    {
        Cursor.SetCursor(CursorHand, new Vector2(10, 10), CursorMode.ForceSoftware);
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
                if (Game.board1d[index].pieceColor != PieceColor.Non)
                {
                    VisualBoard[index].Button.image.color = new Color(1f, 0f, 0f, 0.4f);
                    continue;
                }
                VisualBoard[index].Button.image.color = new Color(0f, 1f, 0f, 0.4f);
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
                if (Game.IsDoingCheck(Game.OtherColor(Game.turnColor)))
                {
                    ClearHandPiece();
                    ColorCheck();
                    DrawBoard();
                    if (Game.IsDoingCheckmate(Game.OtherColor(Game.turnColor)))
                    {
                        DisableBoard();
                        GameInfo.text = "Checkmate!!! " + Game.OtherColor(Game.turnColor) + " Won!!";
                        return;
                    }
                    GameInfo.text = Game.turnColor + " under Check.";
                    if (vsComputer)
                    {
                        StartCoroutine(ComputerTurn());
                    }
                    return;
                }
                DrawBoard();
                ClearHandPiece();
                if (vsComputer)
                {
                    StartCoroutine(ComputerTurn());
                }
            }
        }
    }


    void DisableBoard()
    {
        foreach(var button in VisualBoard)
        {
            button.Button.interactable = false;
        }
    }


    void EnableleBoard()
    {
        foreach (var button in VisualBoard)
        {
            button.Button.interactable = true;
        }
    }


    IEnumerator ComputerTurn()
    {
        DisableAllButtons();
        ChessMove computerMove = new ChessMove();
        var thread = new Thread(() => {
            computerMove = AI.MakeMove();    
            Game.MakeMove(computerMove.first, computerMove.last);
        });
        thread.Start();
        while (thread.IsAlive)
        {
            yield return null;
        }
        VisualBoard[computerMove.first].Button.image.color = Color.magenta;
        VisualBoard[computerMove.last].Button.image.color = Color.magenta;
        Wait();
    }

    async void Wait()
    {
        await Task.Delay(700);
        DrawBoard();
        ClearHandPiece();
        EnableAllButtons();
    }

    void ClearHandPiece()
    {
        foreach (var but in VisualBoard)
        {
            but.Button.image.color = Color.clear;
        }
        Cursor.SetCursor(CursorHand, new Vector2(10, 10), CursorMode.ForceSoftware);
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
        else
        {
            Cursor.SetCursor(CursorHand, new Vector2(10,10), CursorMode.ForceSoftware);
        }
    }

    public void CursorOutOfBoard()
    {
        Cursor.SetCursor(CursorHand, new Vector2(10, 10), CursorMode.ForceSoftware);
        if (HandPiece.CursorShape != null)
        {
            VisualBoard[HandPiece.tempIndex].Shape.sprite = HandPiece.Shape.sprite;
        }
    }

    public void BackMove()
    {
        EnableleBoard();
        Game.MoveBack();
        DrawBoard();
        foreach (var but in VisualBoard)
        {
            but.Button.image.color = Color.clear;
        }
        ColorCheck();
    }

    public void ForwardMove()
    {
        Game.MoveForward();
        DrawBoard();
        foreach (var but in VisualBoard)
        {
            but.Button.image.color = Color.clear;
        }
        ColorCheck();
        if (Game.IsDoingCheck(Game.OtherColor(Game.turnColor)))
        {
            if (Game.IsDoingCheckmate(Game.OtherColor(Game.turnColor)))
            {
                DisableBoard();
                GameInfo.text = "Checkmate!!! " + Game.OtherColor(Game.turnColor) + " Won!!";
                return;
            }
            GameInfo.text = Game.turnColor + " under Check.";
        }
    }
}
