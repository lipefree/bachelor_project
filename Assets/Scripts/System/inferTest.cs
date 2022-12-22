using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ML.Probabilistic.Models;
using Microsoft.ML.Probabilistic;
// using Microsoft.ML.Probabilistic.Compiler;

public class inferTest : MonoBehaviour
{
    // Start is called before the first frame update
    InferenceEngine engine;
    Variable<bool> coin3;

    object test;

    void Start()
    {
        Variable<bool> coin = Variable.Bernoulli(0.5);
        Variable<bool> coin2 = Variable.Bernoulli(0.5);
        coin3 = coin & coin2;
        engine = new InferenceEngine();  

        test = engine.Infer(coin);
    }

    // Update is called once per frame
    void Update()
    {
        var test = new TreeNode(null , 0);
        Debug.Log("Probability both coins are heads: "+ test);
    }
}
