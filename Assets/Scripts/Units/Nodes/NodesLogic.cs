using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class NodesLogic : MonoBehaviour
{
    private Camera mainCamera;
    private float CameraZDistance;

    // Start is called before the first frame update
    void Start()
    {   
        mainCamera = Camera.main;
        CameraZDistance = 
            mainCamera.WorldToScreenPoint(transform.position).z;

        // Represent incoming edges
        var entries = new ArrayList();
        // Represent leaving edges (Not sure if it will be used)
        var leaving = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDrag() {
        Debug.Log("Drag node");
        // Code for drag is from : https://www.youtube.com/watch?v=bK5kYjpqco0&ab_channel=Devsplorer
        transform.position = GetMousePosition();
        //Move the edges connected

    }

    void OnMouseDown() { 

        Debug.Log("Click on node");
        // Code for drag is from : https://www.youtube.com/watch?v=bK5kYjpqco0&ab_channel=Devsplorer
        transform.position = GetMousePosition();
        Vector3 NewWorldPosition = GetMousePosition();
        RaycastHit hit;
        if(Physics.Raycast(NewWorldPosition, Vector3.forward, out hit, Mathf.Infinity) || Physics.Raycast(NewWorldPosition, Vector3.back, out hit, Mathf.Infinity) ) {
            Debug.Log("raycast hit");
            if(hit.collider != null) { 
                if(hit.collider.gameObject.tag.Equals("Edge")) { 
                    Debug.Log("Found edge");
                }
            }
        } 
    }

    public Func<int> DistributionFunction() 
    {
        return () => 0;
    }

    private Vector3 GetMousePosition() { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }

}
