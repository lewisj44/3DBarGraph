using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class DBSwitcher : MonoBehaviour
{
    public VRTK_SnapDropZone dropZone;
    public GameObject snappedObject;
    public GraphManager manager;

    private bool loaded;
    // Use this for initialization
    void Start()
    {
        dropZone = GetComponent<VRTK_SnapDropZone>();
        manager = GameObject.Find("Graph").GetComponent<GraphManager>();

    }
	private void Update()
	{
        snappedObject = dropZone.GetCurrentSnappedObject();
        if (snappedObject == null && GameObject.Find("Graph/Floor") != null) manager.Delete();

	}


    private void OnTriggerExit()
    {
        if (GameObject.Find("Graph/Floor") == null && snappedObject != null)
        {
            manager.LoadGraph("testData" + snappedObject.GetComponent<DatabaseNum>().databaseNum); 
        }
       
       

    }


}
