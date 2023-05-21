using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceScript : MonoBehaviour
{
    //NO USADO -> AudioManagerScript
    private AudioSource controlAudio; 
    
    [SerializeField] private AudioClip[] sonidos; 
    
    private void Awake() // docs.unity3d.com/es/2019.4/Manual/ExecutionOrder.html 
    {
        controlAudio=GetComponent<AudioSource>();   // componente GameObject previamente añadido 

    } 
    public void Reproducir(int sonido, bool loop)
    {
        controlAudio.Stop();
        controlAudio.clip = sonidos[sonido];
        controlAudio.Play();
        controlAudio.loop = loop;

    }

    public void ReproducirLoop(int sonido, float volume)
    {
        for (int i = 0; i < 10; i++)
        {
            controlAudio.PlayOneShot(sonidos[sonido], volume);
        }
        
    }

    public void BotonMute(bool noMute)
    {
        if (noMute)
        {
            controlAudio.volume = 1;
        }
        else
        {
            controlAudio.volume = 0;
        }
    }
}
