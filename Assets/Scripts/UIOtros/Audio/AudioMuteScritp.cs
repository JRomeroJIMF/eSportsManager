using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMuteScritp : MonoBehaviour
{
    public void BotonMute(bool noMute)
    {
        if (noMute)
        {
            AudioManagerScript.Instance.BotonMute(true);
            Debug.Log("Test : audio con sonido");
        }
        else
        {
            AudioManagerScript.Instance.BotonMute(false);
            Debug.Log("Test : audio silenciado");
        }
    }
}
