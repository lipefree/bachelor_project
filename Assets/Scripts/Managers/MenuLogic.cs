using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Jobs;
// using UniRx;
using UnityEngine;

public class MenuLogic : MonoBehaviour
{

    public GameObject nodePrefab;
    public GameObject edgePrefab;
    private GameObject node;
    private GameObject edge;
    private Camera mainCamera;
    private float CameraZDistance;

    private UnityView view;

    private InferPresenter presenter;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance = 
            mainCamera.WorldToScreenPoint(transform.position).z;


        //Logic for communication between View and Infer.net
        presenter = new InferPresenter();
        view = new UnityView(presenter);
        presenter.setView(view);

    }
 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateNode() 
    { 

        Vector3 NewWorldPosition = GetMousePosition();

        node = Instantiate(nodePrefab, NewWorldPosition, Quaternion.identity);

        //destroy parent (menu windows in this case)
        Destroy(transform.parent.gameObject);
    }

    public async void CreateEdge() 
    {   
        //Center the begining of the edge according to the center of the node on top
        Vector3 NewWorldPosition = GetMousePosition();
        Vector3 UICenterPosition = mainCamera.ScreenToWorldPoint(this.transform.position);
        RaycastHit hit;
        Transform parentNode = null;
        if(Physics.Raycast(UICenterPosition, Vector3.forward, out hit, Mathf.Infinity) || Physics.Raycast(UICenterPosition, Vector3.back, out hit, Mathf.Infinity) ) {
            if(hit.collider != null) { 
                if(hit.collider.gameObject.tag.Equals("Node")) { 
                    parentNode = hit.collider.gameObject.transform;
                    NewWorldPosition = parentNode.position + new Vector3 (0,0,1); // Centers it on the center of the node
                }
            }
        } 

        edge = Instantiate(edgePrefab, NewWorldPosition, Quaternion.identity, parentNode);

        //destroy parent (menu window in this case)
        Destroy(transform.parent.gameObject);

        //Await call not to block the UI
        GameObject[] edges = GameObject.FindGameObjectsWithTag("Edge");
        var couple = edges.ToList().Select(edge => (edge, edge.GetComponent<arrowGeneration>())).ToList();
        await updateView(couple);
    }

    private async Task updateView(List<(GameObject, arrowGeneration)> edges) {
        await Task.Run(() => this.view.onNewEdge(edges));
    }

    private Vector3 GetMousePosition() { 
        Vector3 ScreenPosition = 
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraZDistance);
        
        return mainCamera.ScreenToWorldPoint(ScreenPosition);
    }
}


