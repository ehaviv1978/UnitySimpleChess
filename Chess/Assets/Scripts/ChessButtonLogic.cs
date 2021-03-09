using UnityEngine;
using UnityEngine.UI;



public class ChessButtonLogic : MonoBehaviour
{
    public void MouseIn()
    {
        gameObject.GetComponent<Image>().color = new Color(0, 0, 1, 0.2f);
    }

    public void MouseOut()
    {
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
}
