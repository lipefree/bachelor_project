using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Microsoft.ML.Probabilistic.Models;
using System.Linq;

[RequireComponent(typeof(Collider))]
public class NodesLogic : MonoBehaviour
{
    private Camera mainCamera;
    private float CameraZDistance;
    public string variableName;

    public GameObject MenuPrefab;
    private GameObject Menu;
    public List<float> probabilities;
    public string definition;

    private Variable<bool> inferProbability;
    private InferPresenter Presenter;

    private bool observed;
    private bool observedValue;

    public bool prebuild;
    public List<NodesLogic> parents;
    private bool hasCustomDefinition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance =
            mainCamera.WorldToScreenPoint(transform.position).z;

        Presenter = GameObject.FindGameObjectWithTag("Presenter").GetComponent<InferPresenter>();

        if (!prebuild)
        {
            //defaults
            variableName = "var" + getListNodes().Count;
            probabilities = new List<float>() { 0.5f, 0.5f };
            observed = false;
            parents = new List<NodesLogic>();
            hasCustomDefinition = false;
            definition = "";
            CreateMenu();
            updateProba();
        }
    }

    void OnMouseDrag()
    {
        // Code for drag is from : https://www.youtube.com/watch?v=bK5kYjpqco0&ab_channel=Devsplorer
        transform.position = GetMousePosition();
    }

    void OnMouseDown()
    {
        //Open new menu
    }

    public void setVariableName(string variableName)
    {
        if (!validName(variableName))
        {
            return;
        }

        this.variableName = variableName;
        updateProba();
        notifyInterNodes();
    }

    public void setProbabilitie0(float proba)
    {
        probabilities[0] = proba;
        probabilities[1] = 1 - proba;
        updateProba();
    }

    public void setProbabilitie1(float proba)
    {
        probabilities[1] = proba;
        probabilities[0] = 1 - proba;
        updateProba();
    }

    public void setDefinition(string def)
    {
        try
        {
            interpret(def);
            this.hasCustomDefinition = true;
        }
        catch (Exception e)
        {
            Debug.Log("This definition is not accepted : " + e);
        }
    }

    public void interpret(string def)
    { 
        //Convert definition to infer.net
        var env2 = getListEdges()
            .SelectMany(x => x) //flatten
            .Distinct()
            .Select(node => (node.GetComponent<NodesLogic>().getInferProba(), node.GetComponent<NodesLogic>().getVariableName()))
            .ToList();
        
        var env = parents.Select(parent => (parent.getInferProba(), parent.getVariableName())).ToList();
        this.inferProbability = Presenter.interpret(def, env);
        this.definition = def;
    }

    public void setObservedValue(string value)
    {
        if (!(value.Equals("1") || value.Equals("0") || value.Equals("")))
        {
            Debug.Log("Observed value should be in format '1' or '0'");
        }

        if (value.Equals(""))
        {
            this.observed = false;
            this.inferProbability.ClearObservedValue();
        }
        else if (value.Equals("1"))
        {
            this.observed = true;
            this.inferProbability.ObservedValue = true;
        }
        else
        {
            this.observed = true;
            this.inferProbability.ObservedValue = false;
        }
    }

    public string getVariableName()
    {
        return this.variableName;
    }

    public string getDefinition()
    {
        return this.definition;
    }

    public List<float> getProbabilities()
    {
        return this.probabilities;
    }

    public Variable<bool> getInferProba()
    {
        return this.inferProbability;
    }

    public bool getObervedValue()
    {
        return observedValue;
    }

    public bool isOberved()
    {
        return observed;
    }

    public void updateParents(List<GameObject> parents)
    {   
        this.parents = parents.Select(parent => parent.GetComponent<NodesLogic>()).ToList();
        if (!this.hasCustomDefinition)
        {
            updateDefinitionOnUpdate();
            interpret(this.definition);
        }
    }

    private void updateDefinitionOnUpdate()
    {
        string updateRec(List<NodesLogic> parents)
        {
            if (parents.Count == 0)
            {
                return "";
            }
            else if (parents.Count == 1)
            {
                return parents.First().getVariableName();
            }

            return parents.First().getVariableName() + " | " + updateRec(parents.Skip(1).ToList());
        }

        this.definition = updateRec(parents);
    }

    void CreateMenu()
    {
        DestroyMenu();
        Menu = Instantiate(MenuPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        Menu.GetComponent<NodeMenu>().SetNode(this.gameObject);
    }

    void DestroyMenu()
    {
        foreach (var menu in GameObject.FindGameObjectsWithTag("NodeMenu"))
        {
            Destroy(menu);
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 ScreenPosition =
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);

        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }

    private void updateProba()
    {
        this.inferProbability = Variable.Bernoulli(probabilities[1]);
    }

    private List<List<GameObject>> getListEdges()
    {
        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
        return edges.ToList().Select(edge => edge.GetComponent<EdgesLogic>().getNodesTransform()).Where(nodes => nodes != null).ToList();
    }

    private List<GameObject> getListNodes()
    {
        return GameObject.FindGameObjectsWithTag("Node").ToList();
    }

    private bool validName(string name)
    {
        var letters = new List<string>(){
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
        };

        if (!letters.Contains(name.First().ToString()))
        {
            Debug.Log("Name must begin by a letter");
            return false;
        }

        if (name.Contains(" "))
        {
            Debug.Log("Spaces are not allowed in name");
            return false;
        }

        return true;
    }

    private void notifyInterNodes()
    {
        var edges = getListEdges();
        foreach(var node in Presenter.getInterNodes(edges))
        {
            var parents = Presenter.getParents(node, edges);
            node.GetComponent<NodesLogic>().updateParents(parents);
        }
    }

}
