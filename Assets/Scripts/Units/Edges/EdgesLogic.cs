// Source : https://www.youtube.com/watch?v=8lXDLy24rJw

using System.Collections.Generic;
using UnityEngine;
using System;
 
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class EdgesLogic : MonoBehaviour
{
    public float stemLength;
    public float stemWidth;
    public float tipLength;
    public float tipWidth;
    public bool edgeBuilding;

    [System.NonSerialized]
    public List<Vector3> verticesList;
    [System.NonSerialized]
    public List<int> trianglesList;

    private Camera mainCamera;
    private float CameraZDistance;

    public BoxCollider boxCollider;

    private Transform nodeTipTransform;
    private Vector3 tipOriginPosition;
    private Transform nodeBaseTransform;
    private Vector3 baseOriginPosition;
    Mesh mesh;
 
    void Start()
    {
        //make sure Mesh Renderer has a material
        mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = mesh;

        mainCamera = Camera.main;
        CameraZDistance = 
            mainCamera.WorldToScreenPoint(transform.position).z;

        edgeBuilding = true;

        nodeBaseTransform = transform.parent.transform;
        baseOriginPosition = nodeBaseTransform.position;
        nodeTipTransform = null;
    }
 
    void Update()
    {   
        if(edgeBuilding) { 
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transformEdge(mousePosition, 0, 10);
        } else { 
            // Follow both nodes (tip and base)
            transformEdge(nodeTipTransform.position, -nodeTipTransform.localScale.x/2, 0);
        }
    }

    void OnMouseDown()
    {   
        Vector3 NewWorldPosition = GetMousePosition();
        RaycastHit hit;
   
        if(Physics.Raycast(NewWorldPosition, Vector3.forward, out hit, Mathf.Infinity) || Physics.Raycast(NewWorldPosition, Vector3.back, out hit, Mathf.Infinity) ) {
            if(hit.collider != null) { 
                if(hit.collider.gameObject.tag.Equals("Node")) { 
                    //adjust the tip to point the center of the node
                    nodeTipTransform = hit.collider.gameObject.transform;
                    tipOriginPosition = nodeTipTransform.position;
                    edgeBuilding = false;
                    //Extra update in case we are in between updates
                    transformEdge(nodeTipTransform.position, -nodeTipTransform.localScale.x/2, 10); //Adjust to point just the outside of the node
                    this.transform.position = nodeBaseTransform.position;
                }
            }
        } 
    }

    // Transform the edge so that the tip of the arrow follows the mouse
    void transformEdge(Vector3 goalPosition, float adjustStemLength, int boxSize) 
    {
        //TODO: put this section elsewhere when test are finished

        //scale the edge
        float x = Math.Abs(goalPosition.x - transform.position.x);
        float y = Math.Abs(goalPosition.y - transform.position.y);

        double scale = Math.Sqrt(x*x + y*y);

        stemLength = (float)scale - tipLength + adjustStemLength;

        float nodeSize = 1f; 
        boxCollider.size = new Vector3(stemLength + tipLength , stemWidth, boxSize);
        boxCollider.center = new Vector3(Math.Abs(stemLength + tipLength + nodeSize)/2, 0, 0);

        //rotate the edge
        Vector2 direction = goalPosition - transform.position;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3 (0, 0, angle);
        GenerateArrow(new Vector3(0,0,0));
    }
 
    //arrow is generated starting at Vector3.zero
    //arrow is generated facing right, towards radian 0.
    void GenerateArrow(Vector3 stemOrigin)
    {
        //setup
        verticesList = new List<Vector3>();
        trianglesList = new List<int>();
 
        //stem setup
        float stemHalfWidth = stemWidth/2f;
        //Stem points
        verticesList.Add(stemOrigin+(stemHalfWidth*Vector3.down));
        verticesList.Add(stemOrigin+(stemHalfWidth*Vector3.up));
        verticesList.Add(verticesList[0]+(stemLength*Vector3.right));
        verticesList.Add(verticesList[1]+(stemLength*Vector3.right));
 
        //Stem triangles
        trianglesList.Add(0);
        trianglesList.Add(1);
        trianglesList.Add(3);
 
        trianglesList.Add(0);
        trianglesList.Add(3);
        trianglesList.Add(2);
        
        //tip setup
        Vector3 tipOrigin = stemOrigin + stemLength*Vector3.right;
        float tipHalfWidth = tipWidth/2;
 
        //tip points
        verticesList.Add(tipOrigin+(tipHalfWidth*Vector3.up));
        verticesList.Add(tipOrigin+(tipHalfWidth*Vector3.down));
        verticesList.Add(tipOrigin+(tipLength*Vector3.right));
 
        //tip triangle
        trianglesList.Add(4);
        trianglesList.Add(6);
        trianglesList.Add(5);
 
        //assign lists to mesh.
        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
    }
    public List<GameObject> getNodesTransform() {
        if(nodeTipTransform == null) {
            return null;
        } else { 
            return new List<GameObject>{nodeBaseTransform.gameObject, nodeTipTransform.gameObject};;
        }
    }
    
    private Vector3 GetMousePosition() 
    { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }
}