using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]

public class EdgesLogic : MonoBehaviour
{
    private Camera mainCamera;
    private float CameraZDistance;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance = 
            mainCamera.WorldToScreenPoint(transform.position).z;
        
    }

    // Update is called once per frame
    void Update()
    {   
        //Rotate the edge
        //source : https://gamedevbeginner.com/make-an-object-follow-the-mouse-in-unity-in-2d/
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3 (0, 0, angle);

        //scale the edge
        float x = Math.Abs(mousePosition.x - transform.position.x);
        float y = Math.Abs(mousePosition.y - transform.position.y);

        double scale = Math.Sqrt(x*x + y*y);

        Debug.Log("scale : " + scale);


        transform.localScale = new Vector3((float)scale, 1, 1); 
    }

    void OnMouseDrag() 
    {
        Debug.Log("detect drag from edge");
    }
    
    private Vector3 GetMousePosition() { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }
}

