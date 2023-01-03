using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.ML.Probabilistic.Models;
using System.Threading.Tasks;

public class ProbaViz : MonoBehaviour
{
    public GameObject presenterObject;

    private InferPresenter Presenter;
    private InferenceEngine engine;
    // Start is called before the first frame update
    void Start()
    {
        Presenter = presenterObject.GetComponent<InferPresenter>();
        engine = new InferenceEngine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void requestProbaViz()
    {   
        var nodes = getListNodes();
        var nodesScript = nodes.Select(node => node.GetComponent<NodesLogic>()).ToList();
        var roots = Presenter.getRoots(getListEdges());
        var rootsScript = roots.Select(node => node.GetComponent<NodesLogic>()).ToList();
        var interNodes = nodes.Except(roots).Select(interNode => interNode.GetComponent<NodesLogic>()).ToList();
        await printProbaViz(nodesScript, interNodes, rootsScript);
    }

    private void print(List<NodesLogic> nodes, List<NodesLogic> interNodes, List<NodesLogic> roots)
    {

        Debug.Log("---ProbaViz---");
        //Observed value check
        foreach(var node in nodes) { 
            if(node.isOberved()) { 
                Debug.Log(node.getVariableName() + " is observed");
            }
        }
        roots.ForEach(root => printRoot(root));
        interNodes.ForEach(interNode => printInterNode(interNode));
    }

    private async Task printProbaViz(List<NodesLogic> nodes, List<NodesLogic> interNodes, List<NodesLogic> roots){
        await Task.Run(() => print(nodes, interNodes, roots));
    }
    public void printRoot(NodesLogic root)
    {   
        var probabilities = root.getProbabilities();

        Debug.Log(root.getVariableName() + ": P(0) = " + probabilities[0] + " | P(1) = " + probabilities[1] + ", Infer : " + engine.Infer(root.getInferProba()));
    }

    public void printInterNode(NodesLogic interNode)
    {
        //var inferedProba = ... TODO: After definition is done, request inferengine 

        Debug.Log(interNode.getVariableName() + ", definition : " + interNode.getDefinition() + ", Infer : " + engine.Infer(interNode.getInferProba()));
    }

    private List<GameObject> getListNodes()
    {
        return GameObject.FindGameObjectsWithTag("Node").ToList();
    }

    private List<List<GameObject>> getListEdges()
    {
        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
        return edges.ToList().Select(edge => edge.GetComponent<EdgesLogic>().getNodesTransform()).Where(nodes => nodes != null).ToList();
    }
}
