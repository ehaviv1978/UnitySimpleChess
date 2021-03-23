using Assets.Classes;
using ChessLogic;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;
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
    [SerializeField] Image VsComputerCheck;
    [SerializeField] Text TextComputerLvl;
    [SerializeField] Image AnimationPlane;
    [SerializeField] Image ChessPieceAnimation;
    [SerializeField] Text TextSoundButton;
    public ChessButton[] VisualBoard = new ChessButton[64];
    private ChessButton HandPiece;
    private ChessGame Game = new ChessGame();
    private ChessAI AI;
    private int[] possibleMoves;
    private bool vsComputer = true;
    public int computerLvl = 2;
    public int maxComputerLvl = 2;
    Button[] allButtons;
    private Vector3 oldPosition;
    private Vector3 newPosition;
    private bool animating = false;
    private bool playSounds = false;


    void Start()
    {
        AI = new ChessAI(Game, computerLvl);
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
        playSounds = true;
        TextComputerLvl.text = computerLvl.ToString();
        AI.compLvl = computerLvl;
        ChessPieceAnimation.enabled = false;
    }


    // Disable or Enable all buttons in game
    void EnableAllButtons(bool isEnabled)
    {
        foreach (var button in allButtons)
        {
            button.interactable = isEnabled;
        }
    }

    public void NewGame()
    {
        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.click);
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
        BoardEnabled(true);

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
                VisualBoard[index].Button.image.color = new Color(0f, 1f, 1f, 0.4f);
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
            if (Game.board1d[button.index].pieceColor == PieceColor.Black && vsComputer) return;  //Don't allow to play black pieces if computer playing
            if (button.Shape.sprite == Empty || Game.board1d[button.index].pieceColor != Game.turnColor) return;  //Don't allow to pick up oponent pieces
            PickPieceInHand();
        }
        else
        {
            if (button.index == HandPiece.tempIndex)
            {
                if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.pieceMove);
                button.Shape.sprite = HandPiece.Shape.sprite;
                ClearHandPiece();
                ColorCheck();
                return;
            }
            if (Game.board1d[button.index].pieceColor == Game.board1d[HandPiece.tempIndex].pieceColor)
            {
                if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.pieceMove);
                ClearHandPiece();
                DrawBoard();
                PickPieceInHand();
                ColorCheck();
                return;
            }
            if (Array.Exists(possibleMoves, element => element == button.index)) //if clicked squre is in possible moves
            {
                Game.MakeMove(HandPiece.tempIndex, button.index);
                if (Game.IsDoingCheck(Game.turnColor))
                {
                    ClearHandPiece();
                    ColorCheck();
                    Game.MoveBack();
                    DrawBoard();
                    ColorCheck();
                    if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.error);
                    GameInfo.text = "Invalid Move";
                    return;
                }
                if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.pieceMove);
                if (Game.IsDoingCheck(Game.OtherColor(Game.turnColor)))
                {
                    ClearHandPiece();
                    ColorCheck();
                    DrawBoard();
                    if (Game.IsDoingCheckmate(Game.OtherColor(Game.turnColor)))
                    {
                        BoardEnabled(false);
                        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.win);
                        GameInfo.text = "Checkmate!!! " + Game.OtherColor(Game.turnColor) + " Won!!";
                        return;
                    }
                    if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.check);
                    GameInfo.text = Game.turnColor + " under Check.";
                    if (vsComputer && Game.turnColor == PieceColor.Black) ComputerTurn();// StartCoroutine(ComputerTurn());  // if computer playing make computer move


                    return;
                }
                var tempHistory = Game.moveHistory.ToList();
                if (Game.IsDraw(Game.turnColor))
                {
                    Game.moveHistory = tempHistory.ToList();
                    Draw();
                    return;
                }
                Game.moveHistory = tempHistory.ToList();
                DrawBoard();
                ClearHandPiece();
                if (vsComputer && Game.turnColor == PieceColor.Black) ComputerTurn();//StartCoroutine(ComputerTurn());  // if computer playing make computer move
                return;
            }
            // if clicked squre is not in posible move
            if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.error);
            GameInfo.text = "Invalid Move.";
        }
    }


    // If game end in draw
    private void Draw()
    {
        ClearHandPiece();
        DrawBoard();
        ColorCheck();
        BoardEnabled(false);
        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.lose);
        GameInfo.text = "Draw!";
    }


    //Disable or Enable all chess Board Squares/buttons
    void BoardEnabled(bool isEnabled)
    {
        foreach (var button in VisualBoard)
        {
            button.Button.interactable = isEnabled;
        }
    }


    /*IEnumerator ComputerTurn()
    {
        bool computerCapture = false;
        EnableAllButtons(false);
        GameInfo.text = "Computer thinking...";
        ChessMove computerMove = new ChessMove();
        var thread = new Thread(() =>
        {
            computerMove = AI.MakeMove();
            if (Game.board1d[computerMove.last].pieceColor != PieceColor.Non)
            {
                computerCapture = true;
            }
            Game.MakeMove(computerMove.first, computerMove.last);
        });
        thread.Start();
        while (thread.IsAlive)
        {
            yield return null;
        }
        if (computerCapture)
        {
            VisualBoard[computerMove.last].Button.image.color = Color.red;
        }
        oldPosition = new Vector3(((computerMove.first % 8) - 4) * 80 + 40, -(((computerMove.first / 8) - 4) * 80 + 40), 0);
        newPosition = new Vector3(((computerMove.last % 8) - 4) * 80 + 40, -(((computerMove.last / 8) - 4) * 80 + 40), 0);
        ChessPieceAnimation.transform.localPosition = oldPosition;
        ChessPieceAnimation.sprite = VisualBoard[computerMove.first].Shape.sprite;
        VisualBoard[computerMove.first].Shape.sprite = Empty;
        ChessPieceAnimation.enabled = true;
        animating = true;
    }*/


    void ComputerTurn()
    {
        EnableAllButtons(false);
        GameInfo.text = "Computer thinking...";
        var computerMove = AI.MakeMove();
        if (Game.board1d[computerMove.last].pieceColor != PieceColor.Non)
        {
            VisualBoard[computerMove.last].Button.image.color = Color.red;
        }
        Game.MakeMove(computerMove.first, computerMove.last);
        oldPosition = new Vector3(((computerMove.first % 8) - 4) * 80 + 40, -(((computerMove.first / 8) - 4) * 80 + 40), 0);
        newPosition = new Vector3(((computerMove.last % 8) - 4) * 80 + 40, -(((computerMove.last / 8) - 4) * 80 + 40), 0);
        ChessPieceAnimation.transform.localPosition = oldPosition;
        ChessPieceAnimation.sprite = VisualBoard[computerMove.first].Shape.sprite;
        VisualBoard[computerMove.first].Shape.sprite = Empty;
        ChessPieceAnimation.enabled = true;
        animating = true;
    }



    private void Update()
    {
        if (animating)
        {
            float step = 400 * Time.deltaTime; // calculate distance to move based on computer framerate
            ChessPieceAnimation.transform.localPosition = Vector3.MoveTowards(ChessPieceAnimation.transform.localPosition, newPosition, step);

            if (Vector3.Distance(ChessPieceAnimation.transform.localPosition, newPosition) < 0.001f)
            {
                animating=false;
                ChessPieceAnimation.enabled = false;
                CompliteComputerMove();
            }
        }    
    }


    void CompliteComputerMove()
    {
        var tempHistory = Game.moveHistory.ToList();
        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.pieceMove);
        DrawBoard();
        ClearHandPiece();
        EnableAllButtons(true);
        if (Game.IsDoingCheck(PieceColor.Black))
        {
            if (Game.IsDoingCheckmate(PieceColor.Black))
            {
                ColorCheck();
                BoardEnabled(false);
                if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.lose);
                GameInfo.text = "Checkmate!!! Computer Won!";
                return;
            }
            ColorCheck();
            if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.check);
            GameInfo.text = "Computer Check!";
        }
        else if (Game.IsDraw(PieceColor.White))
        {
            Game.moveHistory = tempHistory.ToList();
            Draw();
        }
        Game.moveHistory = tempHistory.ToList();
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
        if (Game.moveHistoryPointer == 0) return;

        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.softClick);
        BoardEnabled(true);
        Game.MoveBack();
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
                BoardEnabled(false);
                GameInfo.text = "Checkmate!!! " + Game.OtherColor(Game.turnColor) + " Won!!";
                return;
            }
            GameInfo.text = Game.turnColor + " under Check.";
        }
        if (vsComputer && Game.turnColor == PieceColor.Black)
        {
            GameInfo.text = "Press Play for computer to move.";
        }
    }

    public void ForwardMove()
    {
        if (Game.moveHistoryPointer +1 == Game.moveHistory.Count) return;

        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.softClick);
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
                BoardEnabled(false);
                GameInfo.text = "Checkmate!!! " + Game.OtherColor(Game.turnColor) + " Won!!";
                return;
            }
            GameInfo.text = Game.turnColor + " under Check.";
        }
        if (vsComputer && Game.turnColor == PieceColor.Black)
        {
            GameInfo.text = "Press Play for computer to move.";
        }
    }

    public void VsComputer()
    {
        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.click);
        vsComputer = !vsComputer;
        VsComputerCheck.enabled = !VsComputerCheck.enabled;
        
        if(vsComputer && Game.turnColor == PieceColor.Black && VisualBoard[0].Button.interactable)
        {
            ComputerTurn();
            //StartCoroutine(ComputerTurn());
        }
    }

    public void ButtonPlay()
    {
        if (vsComputer && Game.turnColor == PieceColor.Black && VisualBoard[0].Button.interactable)
        {
            if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.softClick);
            ComputerTurn();
            //StartCoroutine(ComputerTurn());
        }
    }

    public void LevelUp()
    {
        if (computerLvl < maxComputerLvl)
        {
            computerLvl++;
            if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.softClick);
            AI.compLvl = computerLvl;
            TextComputerLvl.text = AI.compLvl.ToString();
        }
    }

    public void LevelDown()
    {
        if (computerLvl > 1)
        {
            computerLvl--;
            if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.softClick);
            AI.compLvl = computerLvl;
            TextComputerLvl.text = AI.compLvl.ToString();
        }
    }

    public void SaveGame()
    {
        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.click);

        try
        {
            var gameToSave = new SavedChessGame(Game.turnColor, Game.moveHistory);
            string json = JsonConvert.SerializeObject(gameToSave, Formatting.Indented);
            File.WriteAllText(Application.persistentDataPath + "/saved_game.json", json);
            GameInfo.text = "Game Saved.";
        }
        catch
        {
            GameInfo.text = "Error saving game.";
            return;
        }
    }

    public void LoadGame()
    {
        SavedChessGame gameSaved;
        if (playSounds) SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.click);
        try
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/saved_game.json");
            gameSaved = JsonConvert.DeserializeObject<SavedChessGame>(json);
            GameInfo.text = json;
        }
        catch
        {
            GameInfo.text = "Error loading game.";
            return;
        }

        Game.moveHistory = gameSaved.moveHistory;
        Game.moveHistoryPointer = Game.moveHistory.Count;
        Game.MoveBack();
        Game.turnColor = gameSaved.turn;
        DrawBoard();
        ClearHandPiece();
        ColorCheck();
        if (gameSaved.turn == PieceColor.Black && vsComputer)
        {
            GameInfo.text = "Game Loaded. Press play for computer to move.";
        }
        else
        {
            GameInfo.text = "Game Loaded.";
        }
    }

    public void SoundOnOff()
    {
        playSounds = !playSounds;
        if (playSounds)
        {
            SoundManager.SoundInstance.Audio.PlayOneShot(SoundManager.SoundInstance.click);
            TextSoundButton.text = "Turn Sound OFF";
        }
        else
        {
            TextSoundButton.text = "Turn Sound ON";
        }
    }
}
