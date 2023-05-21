using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransicionEscenaScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AnimationClip animacion;
    [SerializeField] private Button boton;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        boton.onClick.AddListener(() =>
        {
            StartCoroutine(CambiarEscena());
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(CambiarEscena());
        }
    }

    IEnumerator CambiarEscena()
    {
        animator.SetTrigger("Iniciar");

        yield return new WaitForSeconds(animacion.length);

        SceneManager.LoadScene(3);
    }
}
