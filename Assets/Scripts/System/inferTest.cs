using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic;
// using Microsoft.ML.Probabilistic.Compiler;

public class InferTest 
{
    // Start is called before the first frame update
    InferenceEngine engine;
    Variable<bool> coin3;

    public InferTest() { 
        engine = new InferenceEngine();
    }
    public Variable<bool> test()
    {
        return Variable.Bernoulli(0.5);  
    }

    // Update is called once per frame
    
}
