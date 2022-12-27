using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class MenuInstantiation : MonoBehaviour
{

    //TODO: Should be moved to unit manager ? 
    private GameObject Menu;
    public GameObject MenuPrefab;

    public GameObject NodeMenuPrefab;

    private RectTransform m_parent;

    private Vector2 anchoredPos;
    void Start()
    {
        //We have to find the main canvas but we can't link real object to prefabs, so we have to find it at the instantiation 
        m_parent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.C))
        {

            // We don't want 2 menu at the time
            destroyMenus();

            var checkNode = OnTopOfNode();

            if (checkNode.Item1)
            {
                Menu = InstantiateObject(NodeMenuPrefab, "NodeMenu", new Vector3(0, 0, 0), true);
                Menu.GetComponent<NodeMenu>().SetNode(checkNode.Item2);
            }
            else
            {
                Menu = InstantiateObject(MenuPrefab, "Menu", new Vector3(0, 0, 0), false);
            }
        }
    }

    GameObject InstantiateObject(GameObject prefab, string tag, Vector3 offset, bool onNode)
    {
        return Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    (bool, GameObject) OnTopOfNode()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mousePosition, Vector3.forward, out hit, Mathf.Infinity) || Physics.Raycast(mousePosition, Vector3.back, out hit, Mathf.Infinity))
        { // foward is enough but we also check back because we may want another system for overlapping
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag.Equals("Node"))
                {
                    return (true, hit.collider.gameObject);
                }
            }
        }
        return (false, null);
    }

    private void destroyMenus()
    {
        var tags = new List<string>() { "Menu", "NodeMenu" };

        var listToDelete = tags.Select(tag => GameObject.FindGameObjectsWithTag(tag)).ToList();
        
        listToDelete.ForEach(menus =>
            menus.ToList()
                .ForEach(menu => Destroy(menu)));
    }
}
