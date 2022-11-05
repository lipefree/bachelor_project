using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInstantiation : MonoBehaviour
{

    //TODO: Should be moved to unit manager ? 
    private GameObject Menu;
    public GameObject MenuPrefab;

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
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.C)) { 

            // We don't want 2 menu at the time
            if(Menu) { 
                DestroyImmediate(Menu.gameObject, true);
            }

            Menu = Instantiate(MenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Find all UI elements for the menu and position them at the position of the mouse
            PlaceUI();

            if(OnTopOfNode()) { 
                Debug.Log("On top of Node !");
            } else { 
                Debug.Log("Not on top of Node ! :(");
            }
            // Only display create edge when we are on top of a node


        }
    }

    void PlaceUI() { 
        GameObject[] menuWindow = GameObject.FindGameObjectsWithTag("Menu");
         
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, null, out anchoredPos);
        foreach(GameObject obj in menuWindow) {
            obj.transform.position += new Vector3(anchoredPos.x, anchoredPos.y, 0);
        }
    }

    bool OnTopOfNode() { 
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        foreach(GameObject node in nodes) { 
            Debug.Log("node position : " + node.transform.position);
            Debug.Log("Mouse position : " + anchoredPos);
            if(Vector3.Distance(anchoredPos, node.transform.position) < 0.5) { //TODO: 0.5 is a guess, getting a proper function that check if we are on top of is preferable
                return true;
            }  
        }

        return false;
    }
}
