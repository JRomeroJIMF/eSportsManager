using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RellenarProgreso : MonoBehaviour
{
 
    public bool rellenar=true;
    public Image imagen;
    public Text progreso;
    
    private float valor =0f;
  
    void Update()
    {
        //Pasamos el texto a float (hasta la terminación en %)
        valor = float.Parse(progreso.text.TrimEnd('%'))/100;
        //Debug.Log(valor);
        
        if(rellenar)
        {
            imagen.fillAmount = valor;
            
            if(progreso)
            {
                progreso.text = (int)(imagen.fillAmount*100)+"%";
            }
			
            if(valor>1)
            {
						
                valor=0;
            }
        }
    }
	
	
}