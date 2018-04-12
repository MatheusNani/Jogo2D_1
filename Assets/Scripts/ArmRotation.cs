using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{

    public int rotationOffset = 90;
    // Update is called once per frame
    void Update()
    {
        // subtracting the position of the player from the mouse position
        Vector3 diferrence = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diferrence.Normalize(); // normalizing the vector. Meaning that all the sum of the vector will be equal to 1

        /*
			Mathf.Atan2(diferrence.y, diferrence.x) Finding the Angle
			 * Mathf.Rad2Deg - Converte to degrees
		*/
        float rotaionZ = Mathf.Atan2(diferrence.y, diferrence.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotaionZ + rotationOffset);

    }
}
