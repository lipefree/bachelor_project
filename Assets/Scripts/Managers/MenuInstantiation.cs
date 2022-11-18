using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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


        }
    }

    void PlaceUI() { 

        //Center UI
        GameObject[] menuWindow = GameObject.FindGameObjectsWithTag("Menu");
         
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parent, Input.mousePosition, null, out anchoredPos); //write the output to anchoredPos
        foreach(GameObject obj in menuWindow) {
            obj.transform.position += new Vector3(anchoredPos.x, anchoredPos.y, 0);
        }

        //Deactivate button "Create edge" if we are not on top of a Node
        if(!OnTopOfNode()) { 
            Button edgeButton = GameObject.FindGameObjectWithTag("EdgeButton").GetComponent<Button>();
            edgeButton.enabled = false;
            edgeButton.GetComponent<Image>().color = Color.clear;
            var text = edgeButton.GetComponentInChildren<TextMeshProUGUI>();
            text.enabled = false;
        }

    }

    bool OnTopOfNode() { 
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(mousePosition, Vector3.forward, out hit, Mathf.Infinity) || Physics.Raycast(mousePosition, Vector3.back, out hit, Mathf.Infinity) ) { // foward is enough but we also check back because we may want another system for overlapping
            if(hit.collider != null) {
                if(hit.collider.gameObject.tag.Equals("Node")) {
                    return true;
                }
            }
        }

        return false;
    }
}
