using UnityEngine.UI;
using UnityEngine;
using ChessLogic;

namespace Assets.Classes
{
    class ChessButton
    {
        public Button Button;
        public Image Shape;
        public Texture2D CursorShape;
        public int tempIndex = 0;
        //public ChessPiece Piece;

        [SerializeField]
        private int m_index;
        public int index => m_index;

        public ChessButton(Button button, int index)
        {
            Button = button;
            m_index = index;
            CursorShape = null;
        }
    }
}
