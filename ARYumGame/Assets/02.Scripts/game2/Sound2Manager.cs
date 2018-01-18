using UnityEngine;
using System.Collections;

public class Sound2Manager : MonoBehaviour
{
    public static Sound2Manager instance;
    public AudioClip eatbad;
    public AudioClip eatgood;
    public AudioClip BGM;
    private AudioSource eatBadSource;
    private AudioSource eatGoodSource;
    private AudioSource BGMSource;


    void Awake() //다른 곳에서도 인스턴스를 접근할 수 있도록 static instance를 만들어 놓습니다.
    {
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
    }

    void Start()
    {
        eatBadSource = gameObject.AddComponent<AudioSource>();
        eatBadSource.clip = eatbad;
        eatBadSource.playOnAwake = false;

        eatGoodSource = gameObject.AddComponent<AudioSource>();
        eatGoodSource.clip = eatgood;
        eatGoodSource.playOnAwake = false;
        eatGoodSource.volume = 0.7f;

        BGMSource = gameObject.AddComponent<AudioSource>();
        BGMSource.clip = BGM;
        BGMSource.playOnAwake = true;
        BGMSource.loop = true;
        BGMSource.volume = 0.35f;
        BGMSource.Play();
    }
    public void EatBadSound()
    {
        eatBadSource.PlayOneShot(eatbad);
    }
    public void EatGoodSound()
    {
        eatGoodSource.PlayOneShot(eatgood);
    }
    public void PlayBGM()
    {
        BGMSource.Play();
    }
    public void PauseBGM()
    {
        BGMSource.Pause();
    }
}
