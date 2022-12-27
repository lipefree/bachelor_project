using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

class InferProba
{
    InferenceEngine engine;
    Variable<bool> proba;
    public InferProba() {
        engine = new InferenceEngine(); // <-
        proba = Variable.Bernoulli(0.5);
    }

    public Graph getProba(Graph graph) 
    {
        
        var roots = graph.getRoots();

        roots.ForEach(root => propagate(root));

        Debug.Log("finished calculation");
        return graph;
    }

    private Variable<bool> createInference(List<GameObject> parents, GameObject node) 
    {
        return parents.Aggregate(Variable.Bernoulli(0), (z, next) => z | Variable.Bernoulli(0.5));
    }

    private void propagate(TreeNode node) {
        if(node == null) {
            return;
        }

        node.getChildren().ForEach(child => {
            child.setTransform(node.getTransform());
        });

        node.getChildren().ForEach(child => {
            propagate(child);
        });
        
    }

    //debug tools
    private void printArray(GameObject[] edges) 
    {
        edges.ToList().ForEach(edge => Debug.Log(edge + "\n"));
    }

    private void printListList(List<List<GameObject>> nodes)
    {
        // nodes.ForEach(node => Debug.Log("[" + node[0] + "," + node[1]+ "], "));

        var node = new TreeNode(null, 0);
    }

}