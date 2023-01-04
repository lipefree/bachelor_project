using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class InferPresenter : MonoBehaviour
{ 
    Graph graph;

    public bool isNodeRoot(GameObject node, List<List<GameObject>> edges)
    {   
        var graph = new Graph(edges);
        return graph.isNodeRoot(node, edges);
    }

    public List<GameObject> getParents(GameObject node, List<List<GameObject>> edges)
    {
        var graph = new Graph(edges);
        return graph.getParents(node, edges);
    }

    public List<GameObject> getRoots(List<List<GameObject>> edges)
    {
        return new Graph(edges).getRoots(edges);
    }

    public bool remainUncyclic(List<List<GameObject>> edges)
    {   
        var graph = new Graph(edges);
        if(!graph.isCyclic()) {
            this.graph = graph;
            return true;
        } else { 
            return false;
        }
    }

    public Variable<bool> interpret(string definition, List<(Variable<bool>, string)> env)
    {
        var def = new Definition(definition);
        return def.interpret(env);
    }
}