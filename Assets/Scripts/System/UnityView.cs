using System;
using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class UnityView 
{ 
    InferPresenter presenter;
    UnityEngine.GameObject debugObject;

    public UnityView(InferPresenter presenter) { 
        this.presenter = presenter;
        debugObject = UnityEngine.GameObject.FindGameObjectWithTag("DebugObject");
    }


    public void onNewEdge(List<(GameObject, arrowGeneration)> edges) { 
        presenter.updateNodes(edges);
    }
}
