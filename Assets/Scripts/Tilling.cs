using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Tilling : MonoBehaviour {

    //the offset so that we don't get any  weird errors
    public int offsetX = 2;

    //there are used for checking if we need to instantiate stuff
    public bool hasARightBubby = false;
    public bool hasALeftBubby = false;

    // used if the object is not tilable
    public bool reverseScale = false;

    //the width of our element
    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    
    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;        
        //Debug.Log(spriteWidth);
	}
	
	// Update is called once per frame
	void Update () {
        //does it still need buddies ? if not do nothing
        if (hasALeftBubby == false || hasARightBubby == false)
        {
            // calculate the camera extend (half the width) of what the camera can see in world coordinates   
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //calculate the x position where the camera can see the edge of the sprites (element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x + spriteWidth / 2) + camHorizontalExtend;

            //checking if we can see the edge of the element and then calling MakeNewBuddy if we can 
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBubby == false)
            {
                MakeNewBuddy(1);
                hasARightBubby = true;

            }else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBubby == false){
                MakeNewBuddy(-1);
                hasALeftBubby = true;
            }

         }
	}


    void MakeNewBuddy(int rightOrLeft)
    {
        // calculating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // if not tilable let's reverse the x size of our object to get rid of ugly seams
        if(reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }
        newBuddy.parent = myTransform.parent;
        if(rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tilling>().hasALeftBubby = true;
        } else
        {
            newBuddy.GetComponent<Tilling>().hasARightBubby = true;
        }
    }
}
