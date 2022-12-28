using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class InferPresenter : MonoBehaviour
{ 
    InferProba inferEngine;

    Graph graph;

    List<List<GameObject>> edges;

    void Start() 
    {
        edges = new List<List<GameObject>>();
        inferEngine = new InferProba();
    }

    //TODO: This overload only exist because we can't see EdgeLogic from here.
    public async void updateProba(List<List<GameObject>> edges)
    {
        var graph = new Graph(edges);
        this.graph = graph;

        var newProbaTask = getProba(graph);

        var newProba = await newProbaTask;

        //update Probaviz
    }

    private async Task<Graph> getProba(Graph graph)
    {
        return await Task.Run(() => inferEngine.getProba(graph));
    }

    public void updateProba()
    {
        var newProba = inferEngine.getProba(this.graph);

        // Update ProbaViz
    }

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

    public bool checkDefinition(GameObject node, string definition, List<List<GameObject>> edges)
    {
        

        return false;
    }

    //TODO: Cant put it here why ?
    // private List<List<GameObject>> getListNodes()
    // {
    //     GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
    //     return edges.ToList().Select(edge => edge.GetComponent<EdgesLogic>().getNodesTransform()).Where(nodes => nodes != null).ToList();
    // }

}