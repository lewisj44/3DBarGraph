using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelText : MonoBehaviour
{
    public TextMesh dataText;
    public Camera mainCamera;

    private GameObject cube;
    private RaycastHit hit;
    private Vector3 cameraRotation;
    private Vector3 textPosition;

    void Start()
    {
        textPosition = Vector3.zero;
        dataText = GetComponent<TextMesh>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        updateText();
    }

    void updateText()
    {
        
        //Rotate text to face main camera
        cameraRotation.y = mainCamera.transform.rotation.y;
        transform.rotation = new Quaternion(0f, cameraRotation.y, 0f, 1f);
    }
}
