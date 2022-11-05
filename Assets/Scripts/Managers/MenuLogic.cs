using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{

    public GameObject nodePrefab;
    private GameObject node;
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
        
    }

    public void CreateNode() 
    { 

        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        Vector3 NewWorldPosition = 
            mainCamera.ScreenToWorldPoint(ScreenPosition);

        node = Instantiate(nodePrefab, NewWorldPosition, Quaternion.identity);

        //destroy parent
        Destroy(transform.parent.gameObject);
    }

    public void CreateEdge() 
    {   
        Vector3 NodeCenterPosition = new Vector3(0,0,0);
    }
}
