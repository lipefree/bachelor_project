using System.Collections.Generic;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class InferPresenter { 
    InferProba inferEngine;
    UnityView view;

    public InferPresenter() { 
        inferEngine = new InferProba();
    }

    public void setView(UnityView view) { this.view = view;}

    public void updateNodes(List<(GameObject, arrowGeneration)> edges) { // This functions needs to be more precise maybe
        object newProba = inferEngine.getProba(edges);

        Debug.Log(newProba);
    }

}