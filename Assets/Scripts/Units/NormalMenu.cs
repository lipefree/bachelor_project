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
    private GameObject node;
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


