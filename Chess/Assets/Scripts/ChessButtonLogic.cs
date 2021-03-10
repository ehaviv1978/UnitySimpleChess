using UnityEngine;
using UnityEngine.UI;



public class ChessButtonLogic : MonoBehaviour
{
    Color tempColor = Color.clear;
    public void MouseIn()
    {
        //tempColor = gameObject.GetComponent<Image>().color;
        //gameObject.GetComponent<Image>().color = new Color(0, 0, 1, 0.2f);
    }

    public void MouseOut()
    {
        //gameObject.GetComponent<Image>().color = tempColor;
    }
}
