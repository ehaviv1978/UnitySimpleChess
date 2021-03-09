using UnityEngine.UI;
using UnityEngine;
using ChessLogic;

namespace Assets.Classes
{
    class ChessButton
    {
        public Button button;
        public Image shape;
        public Texture2D cursorShape;
        public int tempIndex =0;
        public ChessPiece piece;

        [UnityEngine.SerializeField]
        private int m_index;
        public int index => m_index;

        public ChessButton(Button button, int index)
        {
            this.button = button;
            m_index = index;
            cursorShape = null;
            piece = new ChessPiece();
        }
    }
}
