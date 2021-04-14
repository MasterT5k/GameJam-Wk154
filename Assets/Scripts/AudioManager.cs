using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get
        {
            if (_instance == null)
            {
                Debug.LogError("Audio Manager is NULL");
            }
            return _instance;
        } }

    private AudioSource _audioSource;


    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
    }

    public void PlayAudioClipOnce(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(clip);
    }
}
