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

    public object getProba(List<(GameObject, arrowGeneration)> edges) 
    {
        //Get all informations to perform calculations

        var nodes = getListNodes(edges); // Change to graph
        
        var roots = getRoots(nodes);
        var notRoots = nodes.SelectMany(x => x).Except(roots).ToList();

        printListList(nodes);
        Debug.Log("Number of roots :" + roots);
        notRoots.ForEach(nt => Debug.Log("Size of parents is : " + getParents(nodes, nt).Count()));

        //Calculate proba for each nonRoots
        var probas = notRoots.Select(nt => createInference(getParents(nodes, nt), nt)).ToList();

        probas.ForEach(p => Debug.Log("proba is : " + engine.Infer(p))); //< -

        //Still prototyping, we just return something for MVP
        return engine.Infer(proba); // <- 
    }

    private List<List<GameObject>> getListNodes(List<(GameObject, arrowGeneration)> edges)
    {
        var nodes = new List<List<GameObject>>();
        
        foreach((GameObject, arrowGeneration) couple in edges) { 
            var scrip = couple.Item2;
            var nodesTransform = scrip.getNodesTransform();
            //There is one unattached edge that will be null
            if(!nodesTransform.Contains(null)) {
                nodes.Add(nodesTransform.Select(nt => nt.gameObject).ToList());
            }
        }

        return nodes;
    }

    private Variable<bool> createInference(List<GameObject> parents, GameObject node) 
    {
        return parents.Aggregate(Variable.Bernoulli(0), (z, next) => z | Variable.Bernoulli(0.5));
    }

    private List<GameObject> getRoots(List<List<GameObject>> pairs) 
    {
        var listNodes = pairs.SelectMany(x => x).Distinct().ToList(); //get all unique node
        return listNodes.Where(node => pairs.Aggregate(true, (z, next) => next[1] == node? z && false : z && true)).ToList();
    }

    private List<GameObject> getParents(List<List<GameObject>> pairs, GameObject node) 
    {
        return pairs.Where(pair => pair[1] == node).Select(pair => pair[0]).ToList();
    }

    //debug tools
    private void printArray(GameObject[] edges) 
    {
        edges.ToList().ForEach(edge => Debug.Log(edge + "\n"));
    }

    private void printListList(List<List<GameObject>> nodes)
    {
        nodes.ForEach(node => Debug.Log("[" + node[0] + "," + node[1]+ "], "));
    }
}