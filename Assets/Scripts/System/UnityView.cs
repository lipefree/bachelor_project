using System;
using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class UnityView 
{ 
    InferPresenter presenter;
    public UnityView(InferPresenter presenter) { 
        this.presenter = presenter;
    }

    public void onNewEdge(List<List<GameObject>> nodes) { 
        presenter.updateNodes(nodes);
    }
}
