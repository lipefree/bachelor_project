using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.ML.Probabilistic.Models;

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

    public void requestProbaViz()
    {
        var nodes = getListNodes();
        var roots = Presenter.getRoots(getListEdges());
        var interNodes = nodes.Except(roots).ToList();

        Debug.Log("---ProbaViz---");
        //Observed value check
        foreach(var node in nodes) { 
            var nodeScript = node.GetComponent<NodesLogic>();
            if(nodeScript.isOberved()) { 
                Debug.Log(nodeScript.getVariableName() + " is observed");
            }
        }
        roots.ForEach(root => printRoot(root));
        interNodes.ForEach(interNode => printInterNode(interNode));
    }

    public void printRoot(GameObject root)
    {   
        
        var node = root.GetComponent<NodesLogic>();
        var probabilities = node.getProbabilities();

        Debug.Log(node.getVariableName() + ": P(0) = " + probabilities[0] + " | P(1) = " + probabilities[1] + ", Infer : " + engine.Infer(node.getInferProba()));
    }

    public void printInterNode(GameObject interNode)
    {
        var node = interNode.GetComponent<NodesLogic>();
        //var inferedProba = ... TODO: After definition is done, request inferengine 

        Debug.Log(node.getVariableName() + ", definition : " + node.getDefinition() + ", Infer : " + engine.Infer(node.getInferProba()));
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
