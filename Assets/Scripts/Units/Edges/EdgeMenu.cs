using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class EdgeMenu : MonoBehaviour
{   
    private RectTransform m_parent;
    private InferPresenter Presenter;
    private GameObject edge;
    private Vector2 anchoredPos;
    void Start()
    {
        m_parent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RectTransform>();
        PlaceUi();
        Presenter = GameObject.FindGameObjectWithTag("Presenter").GetComponent<InferPresenter>();
    }

    public void SetEdge(GameObject edge)
    {
        this.edge = edge;
    }

    public void DestroyEdge()
    {   
        edge.GetComponent<EdgesLogic>().Destroy();
        Destroy(this.gameObject);
    }

    void PlaceUi()
    {
        GameObject[] menuWindow = GameObject.FindGameObjectsWithTag("EdgeMenu");
        Debug.Log("Moving " + menuWindow.ToList().Count);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, null, out anchoredPos);
        foreach(GameObject obj in menuWindow) {
            obj.transform.position += new Vector3(anchoredPos.x, anchoredPos.y, 0);
        }
    }
}