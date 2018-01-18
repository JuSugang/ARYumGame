using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIClickSound : MonoBehaviour, IPointerDownHandler
{
    public AudioClip sound;

    private Button button {  get { return GetComponent<Button>(); } }
    private AudioSource source { get { return GetComponent<AudioSource>(); } }
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;
        button.onClick.AddListener(() => PlaySound());
    }
    void PlaySound()
    {
        //source.PlayOneShot(sound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        source.PlayOneShot(sound);
    }
}
