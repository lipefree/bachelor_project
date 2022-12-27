using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class UnityView 
{ 
    InferPresenter presenter;
    public UnityView(InferPresenter presenter) { 
        this.presenter = presenter;
    }

    public void onNewEdge(Graph graph) { 
        presenter.updateProba(graph);
    }
}
