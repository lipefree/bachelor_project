using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class graph : MonoBehaviour
{

    List<GameObject> nodes;
    // Start is called before the first frame update
    void Start()
    {
        // nodes = new List<GameObject>();
        // GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");

        // foreach(GameObject edge in edges) { 
        //     var scrip = edge.GetComponent<arrowGeneration>();
        //     var (n1Transform, n2Transform) = scrip.getNodesTransform();
        //     var (n1, n2) = (n1Transform.gameObject, n2Transform.gameObject);

        //     if(!nodes.Contains(n1)){
        //         nodes.Add(n1);
        //     }

        //     if(!nodes.Contains(n2)){
        //         nodes.Add(n2);
        //     }
        // }

        // printNodes(nodes);
        // Debug.Log("Size of nodes is "+ nodes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void printNodes(List<GameObject> l) 
    {
        foreach(GameObject g in l) { 
            Debug.Log(g);
        }
    }
}
