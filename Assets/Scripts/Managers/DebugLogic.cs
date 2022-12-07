using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void writeDebug(object text) {
        Debug.Log("Inference for proba is : " + text);
    }
}
