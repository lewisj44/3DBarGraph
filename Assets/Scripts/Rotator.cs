using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float sensitivity = 0.5f;
    public int axis;
    public bool flipped = false;

    private bool isRotating;
    private float stopTime = 1.5f;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isLocked = false;
    private bool reset = false;

    private void Start()
    {
        rotation = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) isLocked = !isLocked;
        if (Input.GetKey(KeyCode.R))
        {
            isRotating = false;
            rotation = Vector3.zero;
            StopCoroutine(RotateToStop());
            transform.root.rotation = Quaternion.Lerp(transform.root.rotation, Quaternion.identity, Time.deltaTime);
        }
        if (isRotating && !isLocked)
        {
            ///*
            mouseOffset = Input.mousePosition - mouseReference;
            if (axis == 0) rotation.x = -(mouseOffset.x + mouseOffset.y) * sensitivity;
            if (axis == 1) rotation.y = -mouseOffset.x * sensitivity;
            if (flipped) rotation.y = mouseOffset.x * sensitivity;
            if (axis == 2) rotation.z = -(mouseOffset.x + mouseOffset.y) * sensitivity;
            transform.root.Rotate(rotation);
            mouseReference = Input.mousePosition;
            //*/
            //VR 
            /*
            Vector2 movement = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            rotation.x = movement.x;
            rotation.y = movement.y;
            transform.parent.Rotate(rotation);
            */
        }
    }

    private void OnMouseDown()
    {

        isRotating = true;
        mouseReference = Input.mousePosition;
    }


    private void OnMouseUp()
    {
        isRotating = false;
        if (!reset) StartCoroutine(RotateToStop());

    }

    private IEnumerator RotateToStop()
    {
        while (rotation != Vector3.zero)
        {
            rotation = Vector3.Lerp(rotation, Vector3.zero, stopTime * Time.deltaTime);
            transform.root.Rotate(rotation);
            yield return null;
        }
    }
}