using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioGestion : MonoBehaviour
{
    void Start()
    {
        //Audios por escena
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                AudioManagerScript.Instance.Reproducir(0, true, false);
                Debug.Log("TEST Audio : audio on");
                break;
            case 1:
                
                break;
            case 2:
                
                break;
            case 3:
                AudioManagerScript.Instance.Reproducir(1, true,false);
                break;
            case 4:
                AudioManagerScript.Instance.Reproducir(3, true,false);
                break;
                
        }
 
    }
    
}
