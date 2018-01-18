using UnityEngine;
using System.Collections;

public class IntroEndBGM : MonoBehaviour
{
    public AudioClip BGM;
    public bool loop;
    public float volume;
    private AudioSource BGMSource;
    void Start()
    {

        BGMSource = gameObject.AddComponent<AudioSource>();
        BGMSource.clip = BGM;
        BGMSource.playOnAwake = true;
        BGMSource.loop = loop;
        BGMSource.volume = volume;
        BGMSource.Play();
    }
}
