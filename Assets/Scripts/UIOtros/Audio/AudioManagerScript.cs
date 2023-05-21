using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance;
    
    [SerializeField] private AudioSource controlAudio;
    [SerializeField] private AudioSource controlAudioSFX;
    [SerializeField] private AudioClip[] sonidos;
    [SerializeField] private AudioClip[] sonidosSFX;
    

    void Awake()
    {
        //Patron Singleton
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this);
        }

    }
    
    public void Reproducir(int sonido, bool loop, bool esSFX)
    {

        if (esSFX)
        {
            Instance.controlAudioSFX.Stop();
            Instance.controlAudioSFX.clip = sonidosSFX[sonido];
            Instance.controlAudioSFX.Play();
            //si alguno tiene que hacer loop si que podemos poner controlAudio.loop = loop;
            
            Debug.Log("Test Audio: Audio SFX");
        }
        else
        {
            controlAudio.Stop();
            controlAudio.clip = sonidos[sonido];
            controlAudio.Play();
            controlAudio.loop = loop;
        }

    }
    
    public void BotonMute(bool noMute)
    {
        if (noMute)
        {
            controlAudio.mute = false;
            controlAudioSFX.mute = false;
        }
        else
        {
            controlAudio.mute = true;
            controlAudioSFX.mute = true;
        }
    }
     
}
