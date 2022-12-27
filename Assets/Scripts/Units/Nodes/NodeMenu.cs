using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class NodeMenu : MonoBehaviour
{   
    private GameObject node;

    public TMP_Text variableText;
    public TMP_Text p0Text;
    public TMP_Text p1Text;
    public string variableName;
    public List<float> probabilities;
    private RectTransform m_parent;
    private Vector2 anchoredPos;
    public GameObject edgePrefab;
    private GameObject edge;

    // Start is called before the first frame update
    void Start()
    {
        m_parent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RectTransform>();
        PlaceUi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNode(GameObject node)
    {
        this.node = node;
        variableText.text = node.GetComponent<NodesLogic>().getVariableName();

        Debug.Log("Name is : "  + node.GetComponent<NodesLogic>().getVariableName());
        var probabilities = node.GetComponent<NodesLogic>().getProbabilities();
        p0Text.text = probabilities[0].ToString();
        p1Text.text = probabilities[1].ToString();
    }


    public void setVariableName(string variableName)
    {   
        Assert.IsNotNull(node);
        node.GetComponent<NodesLogic>().setVariableName(variableName);
    }

    //TODO: Finish this design : What should the function receive ?
    public void setProbabilitie0(string proba)
    {   
        Assert.IsNotNull(node);
        float probaF = float.Parse(proba, CultureInfo.InvariantCulture.NumberFormat);
        node.GetComponent<NodesLogic>().setProbabilitie0(probaF);
    }   

    public void setProbabilitie1(string proba)
    {   
        Assert.IsNotNull(node);
        float probaF = float.Parse(proba, CultureInfo.InvariantCulture.NumberFormat);
        node.GetComponent<NodesLogic>().setProbabilitie1(probaF);
    }  

    public async void CreateEdge()
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

    void PlaceUi()
    {
        GameObject[] menuWindow = GameObject.FindGameObjectsWithTag("NodeMenu");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, null, out anchoredPos);
        var offset = new Vector3(70, 50, 0);
        foreach(GameObject obj in menuWindow) {
            obj.transform.position += new Vector3(anchoredPos.x, anchoredPos.y, 0) + offset;
        }
    }
}
