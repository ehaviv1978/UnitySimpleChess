using UnityEngine;

//Class for managing sound effects.
public class SoundManager : MonoBehaviour
{
    public AudioSource Audio;

    public AudioClip click;

    public AudioClip softClick;

    public AudioClip pieceMove;

    public AudioClip win;

    public AudioClip lose;

    public AudioClip error;

    public AudioClip check;

    public static SoundManager SoundInstance;

    private void Awake()
    {
        SoundInstance = this;
    }
}
