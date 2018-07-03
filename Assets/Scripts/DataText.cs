using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataText : MonoBehaviour
{
    public TextMesh dataText;
    public Camera mainCamera;

    private float paddingHeight = 1;
    private GameObject cube;
    private RaycastHit hit;
    private Vector3 cameraRotation;
    private Vector3 textPosition;

    void Start()
    {
        textPosition = Vector3.zero;
        dataText = GetComponentInChildren<TextMesh>();
        mainCamera = Camera.main;

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
                    updateText();
                }
            }
        }
        updateText();
    }

    void updateText()
    {
        if (cube == null)
        {
            dataText.text = "";
            return;
        }

        dataText.text = "" + Mathf.Round(cube.transform.localScale.y * 100f);

        if ((cube.transform.lossyScale.y < 0 && paddingHeight > 0) || (cube.transform.lossyScale.y > 0 && paddingHeight < 0)) paddingHeight = -paddingHeight;
        //Update position
        textPosition.x = cube.transform.localPosition.x;
        textPosition.y = cube.transform.lossyScale.y + paddingHeight;
        textPosition.z = cube.transform.parent.transform.localPosition.z;
        transform.localPosition = textPosition;

        //Update rotation
        Quaternion newRotation = new Quaternion(-cube.transform.root.localRotation.x, cube.transform.root.localRotation.y, -cube.transform.root.localRotation.z, 1f);
        transform.localRotation = newRotation;

        //Rotate text to face main camera
        cameraRotation.y = mainCamera.transform.rotation.y;
        transform.rotation = new Quaternion(0f, cameraRotation.y, 0f, 1f); 
    }
}
