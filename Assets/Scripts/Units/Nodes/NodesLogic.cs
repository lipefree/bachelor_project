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
    private string variableName;

    public GameObject MenuPrefab;
    private GameObject Menu;
    private List<float> probabilities;
    private string definition;
    private InferPresenter Presenter;

    // Start is called before the first frame update
    void Start()
    {   
        mainCamera = Camera.main;
        CameraZDistance = 
            mainCamera.WorldToScreenPoint(transform.position).z;

        Presenter = GameObject.FindGameObjectWithTag("Presenter").GetComponent<InferPresenter>();

        //defaults
        variableName = "variable " + getListNodes().Count;
        probabilities = new List<float>(){0.5f, 0.5f};
        CreateMenu();

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDrag() {
        // Code for drag is from : https://www.youtube.com/watch?v=bK5kYjpqco0&ab_channel=Devsplorer
        transform.position = GetMousePosition();
    }

    void OnMouseDown() {
        //Open new menu
    }

    public void setVariableName(string variableName) {
        this.variableName = variableName;
        updateProba();
    }

    public string getVariableName()
    {   
        return this.variableName;
    }

    //TODO: Finish design this : How should we store probabilities ?
    public void setProbabilitie0(float proba)
    {
        probabilities[0] = proba;

        updateProba();
    }

    public void setProbabilitie1(float proba)
    {
        probabilities[1] = proba;

        Presenter.updateProba();
    }

    public void setDefinition(string def)
    {
        this.definition = def;
    }

    public string getDefinition()
    {
        return this.definition;
    }

    public List<float> getProbabilities()
    {
        return this.probabilities;
    }

    void CreateMenu()
    {   
        DestroyMenu();
        Menu = Instantiate(MenuPrefab, new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        Menu.GetComponent<NodeMenu>().SetNode(this.gameObject);
    }

    void DestroyMenu()
    {
        foreach(var menu in GameObject.FindGameObjectsWithTag("NodeMenu")) {
            Destroy(menu);
        }
    }

    public Variable<bool> DistributionFunction() 
    {
        return Variable.Bernoulli(0.5);
    }

    private Vector3 GetMousePosition() { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }

    //TODO: Presenter can't see EdgeLogic namespace for some reasons
    private void updateProba()
    {
        Presenter.updateProba(getListEdges());
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

}
