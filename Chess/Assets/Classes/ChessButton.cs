using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;


namespace Assets.Classes
{
    class ChessButton
    {
        public Button button;
        public Image shape;
        public Texture2D cursorShape;
        public int index;

        public ChessButton(Button button, int index)
        {
            this.button = button;
            this.index = index;
            cursorShape = null;
        }
    }
}
