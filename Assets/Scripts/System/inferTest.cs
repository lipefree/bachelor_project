using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Infer;

public class inferTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Variable<bool> coin = Variable.Bernoulli(0.5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
