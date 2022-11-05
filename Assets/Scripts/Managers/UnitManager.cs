using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: This script is currently unsused, consider removing it
public class UnitManager : MonoBehaviour
{   
    public GameObject nodePrefab;
    private GameObject node;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.C)){
            node = Instantiate(nodePrefab, new Vector3(0,0,0), Quaternion.identity); 
        }
    }
}
