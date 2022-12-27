using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Jobs;
// using UniRx;
using UnityEngine;

public class NormalMenu : MonoBehaviour
{

    public GameObject nodeInstantiantionPrefab;
    public GameObject edgePrefab;
    private GameObject node;
    private GameObject edge;
    private Camera mainCamera;
    private float CameraZDistance;

    private RectTransform m_parent;

    private Vector2 anchoredPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance =
            mainCamera.WorldToScreenPoint(transform.position).z;

        m_parent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RectTransform>();

        PlaceUi();

    }

    public void CreateNode()
    {
        Vector3 NewWorldPosition = GetMousePosition();
        node = Instantiate(nodeInstantiantionPrefab, NewWorldPosition, Quaternion.identity);

        //destroy parent (menu windows in this case)
        Destroy(transform.gameObject);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 ScreenPosition =
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);

        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }

    private (Vector3, Transform) findNodeCenter()
    {
        Vector3 centerPosition = new Vector3();
        Vector3 UICenterPosition = mainCamera.ScreenToWorldPoint(this.transform.position);
        RaycastHit hit;
        Transform parentNode = null;
        if (Physics.Raycast(UICenterPosition, Vector3.forward, out hit, Mathf.Infinity) || Physics.Raycast(UICenterPosition, Vector3.back, out hit, Mathf.Infinity))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag.Equals("Node"))
                {
                    parentNode = hit.collider.gameObject.transform;
                    centerPosition = parentNode.position + new Vector3(0, 0, 1); // Centers it on the center of the node
                }
            }
        }

        return (centerPosition, parentNode);
    }

    private List<(GameObject, EdgesLogic)> edgesWithScripts()
    {
        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
        return edges.ToList().Select(edge => (edge, edge.GetComponent<EdgesLogic>())).ToList();
    }

    private List<List<GameObject>> getListNodes()
    {
        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
        return edges.ToList().Select(edge => edge.GetComponent<EdgesLogic>().getNodesTransform()).Where(nodes => nodes != null).ToList();
    }

    void PlaceUi()
    {
        GameObject[] menuWindow = GameObject.FindGameObjectsWithTag("Menu");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, null, out anchoredPos);
        var offset = new Vector3(70, 50, 0);
        foreach (GameObject obj in menuWindow)
        {
            obj.transform.position += new Vector3(anchoredPos.x, anchoredPos.y, 0) + offset;
        }
    }

}


