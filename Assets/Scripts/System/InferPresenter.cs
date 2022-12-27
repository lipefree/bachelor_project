using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Probabilistic.Models;
using UnityEngine;

public class InferPresenter : MonoBehaviour
{ 
    InferProba inferEngine;
    UnityView view;

    Graph graph;

    void Start() 
    {

    }

    public InferPresenter() { 
        inferEngine = new InferProba();
    }

    public void setView(UnityView view) { this.view = view;}

    public void updateProba(Graph graph) { // This functions needs to be more precise maybe
        var newProba = inferEngine.getProba(graph);

        //Update Probaviz
        Debug.Log(newProba);
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

    //TODO: Cant put it here why ?
    // private List<List<GameObject>> getListNodes()
    // {
    //     GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
    //     return edges.ToList().Select(edge => edge.GetComponent<EdgesLogic>().getNodesTransform()).Where(nodes => nodes != null).ToList();
    // }

}