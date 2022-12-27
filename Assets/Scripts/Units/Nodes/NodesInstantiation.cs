using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesInstantiation : MonoBehaviour
{
    public GameObject nodePrefab; 
    // Start is called before the first frame update
    void Start()
    {
        // Create Node
        var node = Instantiate(nodePrefab, this.transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
