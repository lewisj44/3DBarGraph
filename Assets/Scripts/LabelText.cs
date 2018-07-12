using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelText : MonoBehaviour
{
    public TextMesh dataText;
    public GameObject mainCamera;

    private GameObject cube;
    private RaycastHit hit;
    private Vector3 cameraRotation;


    void Start()
    {
        dataText = GetComponent<TextMesh>();
        mainCamera = GameObject.Find("[VRTK_SDKManager]/SDKSetups/Simulator/VRSimulatorCameraRig");
    }

    void Update()
    {
        //Rotate text to face main camera
        //cameraRotation.y = mainCamera.transform.rotation.y;
        //Debug.Log(cameraRotation.y);
        //transform.rotation = new Quaternion(0f, cameraRotation.y, 0f, 1f);
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y + 180f, transform.rotation.z, 1f);
    }
}
