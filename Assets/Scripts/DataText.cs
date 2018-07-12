using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataText : MonoBehaviour
{
    public TextMesh dataText;
    public GameObject mainCamera;

    private float paddingHeight = 1;
    private GameObject cube;
    private RaycastHit hit;
    private Vector3 cameraRotation;
    private Vector3 textPosition;


    void Start()
    {
        textPosition = Vector3.zero;
        dataText = GetComponentInChildren<TextMesh>();
        mainCamera = GameObject.Find("[VRTK_SDKManager]/SDKSetups/Simulator/VRSimulatorCameraRig");

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Cube")
                {
                    cube = hit.collider.gameObject;
                    //UpdateText();
                }
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        if (cube == null)
        {
            dataText.text = "";
            return;
        }

        dataText.text = ""  + (cube.GetComponent<DragBar>().Value)
                            + "\n " + cube.GetComponent<DragBar>().Year + ", " + cube.GetComponent<DragBar>().Quarter;
                          
        if ((cube.transform.localScale.y < 0 && paddingHeight > 0) || (cube.transform.localScale.y > 0 && paddingHeight < 0)) paddingHeight = -paddingHeight;
        //Update position
        textPosition.x = cube.transform.localPosition.x;
        textPosition.y = cube.transform.localScale.y + paddingHeight;
        textPosition.z = cube.transform.parent.transform.localPosition.z;
        transform.localPosition = textPosition;

        //Update rotation
        Quaternion newRotation = new Quaternion(-cube.transform.root.localRotation.x, cube.transform.root.localRotation.y, -cube.transform.root.localRotation.z, 1f);
        transform.localRotation = newRotation;

        //Rotate text to face main camera
        //cameraRotation.y = mainCamera.transform.rotation.y;
        //Debug.Log(cameraRotation.y);
        //transform.rotation = new Quaternion(0f, cameraRotation.y, 0f, 1f);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 180f, transform.rotation.z, 1f);

    }
}