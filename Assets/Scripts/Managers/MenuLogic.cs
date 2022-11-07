using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{

    public GameObject nodePrefab;
    public GameObject edgePrefab;
    private GameObject node;
    private GameObject edge;
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

        Vector3 NewWorldPosition = GetMousePosition();

        node = Instantiate(nodePrefab, NewWorldPosition, Quaternion.identity);

        //destroy parent (menu windows in this case)
        Destroy(transform.parent.gameObject);
    }

    public void CreateEdge() 
    {   

        Vector3 NewWorldPosition = GetMousePosition();

        edge = Instantiate(edgePrefab, NewWorldPosition, Quaternion.identity);

        //destroy parent (menu windows in this case)
        Destroy(transform.parent.gameObject);
    }

    private Vector3 GetMousePosition() { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }
}
