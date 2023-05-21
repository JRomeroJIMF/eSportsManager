using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInformativoPartida : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void Entendido(GameObject prefab)
    {
        prefab.SetActive(false);
        
    }
}
