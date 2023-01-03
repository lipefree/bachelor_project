using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class InterNodeMenu : MonoBehaviour
{

    private GameObject node;
    private RectTransform m_parent;

    public TMP_Text definitionText;

    public TMP_Text variableText;
    public GameObject parentButtonPrefab;
    private GameObject edge;
    public GameObject edgePrefab;
    private Vector2 anchoredPos;

    private InferPresenter Presenter;
    // Start is called before the first frame update
    void Start()
    {
        m_parent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RectTransform>();
        PlaceUi();
        Presenter = GameObject.FindGameObjectWithTag("Presenter").GetComponent<InferPresenter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNode(GameObject node)
    {
        this.node = node;
        variableText.text = node.GetComponent<NodesLogic>().getVariableName();
        definitionText.text = node.GetComponent<NodesLogic>().getDefinition();

    }


    public void setVariableName(string variableName)
    {   
        Assert.IsNotNull(node);
        node.GetComponent<NodesLogic>().setVariableName(variableName);
    }

    //TODO: Finish this design : What should the function receive ?
    public void setDefinition(string definition)
    {   
        Assert.IsNotNull(node);
        node.GetComponent<NodesLogic>().setDefinition(definition);
    } 

    public void setObservedValue(string value)
    {
        Assert.IsNotNull(node);
        node.GetComponent<NodesLogic>().setObservedValue(value);
    }  


    public void CreateEdge()
    {
        var (centerPosition, parentNode) = findNodeCenter();

        if (parentNode == null)
        {
            Debug.Log("Was not able to find a parent for the edge, can't create the edge");
            return;
        }

        edge = Instantiate(edgePrefab, centerPosition, Quaternion.identity, parentNode);

        //destroy parent (menu window in this case)
        Destroy(this.gameObject);

    }

    public void RequestParents()
    {   
        var parents = Presenter.getParents(this.node, getListEdges());
        printDebugList(parents);
        var first = parents.First().GetComponent<NodesLogic>().getVariableName();
        var parentButton = GameObject.FindGameObjectWithTag("RequestParentButton");
        var button = Instantiate(parentButtonPrefab, new Vector3(0,0,0), Quaternion.identity, parentButton.transform);
        GameObject.FindGameObjectWithTag("ParentButton").GetComponent<ParentButton>().setText(first);
    }

    public void receiveName(string name)
    {

    }

    private void printDebugList(List<GameObject> list)
    {   
        Debug.Log("Parent list : ");
        foreach(var elem in list)
        {
            Debug.Log(elem.GetComponent<NodesLogic>().getVariableName());
        }
    }

    private (Vector3, Transform) findNodeCenter()
    {
        Vector3 centerPosition = new Vector3();
        Transform parentNode = null;
        parentNode = node.transform;
        centerPosition = parentNode.position + new Vector3(0, 0, 1); // Centers it on the center of the node

        return (centerPosition, parentNode);
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void DestroyNode()
    {
        Assert.IsNotNull(node);
        Destroy(node);
        Destroy();
    }


    void PlaceUi()
    {
        GameObject[] menuWindow = GameObject.FindGameObjectsWithTag("NodeMenu");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, null, out anchoredPos);
        var offset = new Vector3(70, 50, 0);
        foreach(GameObject obj in menuWindow) {
            obj.transform.position += new Vector3(anchoredPos.x, anchoredPos.y, 0) + offset;
        }
    }

    private List<List<GameObject>> getListEdges()
    {
        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
        return edges.ToList().Select(edge => edge.GetComponent<EdgesLogic>().getNodesTransform()).Where(nodes => nodes != null).ToList();
    }
}
