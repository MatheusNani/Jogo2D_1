using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public Camera mainCam;
    private float shakeAmount = 0;


    private void Awake()
    {
        if(mainCam == null)
        {
            mainCam = Camera.main;
        }    
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        // começa a chamar o metodo BeginShake com 0(zero sec) a cada 0.01 sec chama novamente
        InvokeRepeating("DoShake", 0,0.01f);
        Invoke("StopShake", length);
    }

	private void DoShake()
    {
        if(shakeAmount > 0)
        {
            // posição da camera antes de aplicar o efeito SHAKE
            Vector3 camPos = mainCam.transform.position; 

            // Calculo do efeito SHAKE no X e Y Axes.
            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
            
            //Aplica o Efeito
            camPos.x += offsetX;
            camPos.y += offsetY;

            mainCam.transform.position = camPos;

        }
    }
	
    private void StopShake()
    {
        CancelInvoke("DoShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
