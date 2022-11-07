// Source : https://www.youtube.com/watch?v=8lXDLy24rJw

using System.Collections.Generic;
using UnityEngine;
using System;
 
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class arrowGeneration : MonoBehaviour
{
    public float stemLength;
    public float stemWidth;
    public float tipLength;
    public float tipWidth;

    private bool edgeBuilding;

    [System.NonSerialized]
    public List<Vector3> verticesList;
    [System.NonSerialized]
    public List<int> trianglesList;

    private Camera mainCamera;
    private float CameraZDistance;
 
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
    }
 
    void Update()
    {   
        if(edgeBuilding) { 
            transformEdge();
            GenerateArrow(new Vector3(0,0,0));
        }
        
    }

    void OnMouseDown()
    { 
        edgeBuilding = false;

    }

    // Transform the edge so that the tip of the arrow follows the mouse
    // TODO: This function should also be called when we move the node attached to the tip
    void transformEdge() 
    {
        //TODO: put this section elsewhere when test are finished

        //scale the edge
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float x = Math.Abs(mousePosition.x - transform.position.x);
        float y = Math.Abs(mousePosition.y - transform.position.y);

        double scale = Math.Sqrt(x*x + y*y);

        stemLength = (float)scale - tipLength;

        //rotate the edge
        Vector2 direction = mousePosition - transform.position;

        float angle = Vector2.SignedAngle(Vector2.right, direction);
        transform.eulerAngles = new Vector3 (0, 0, angle);

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

    private Vector3 GetMousePosition() 
    { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }
}