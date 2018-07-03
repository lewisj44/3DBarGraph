using UnityEngine;
using System.Collections;

public class FreeRotate : MonoBehaviour
{

    public float sensitivity = 0.1f;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private bool isRotating;
    private float stopTime = 1f;
    public Vector3 rotationV;

    private void Start()
    {
        rotationV = Vector3.zero;
        rotation = Vector3.zero;

    }

    private void Update()
    {
        if (isRotating)
        {
            mouseOffset = (Input.mousePosition - mouseReference);
            rotation.x = (mouseOffset.y - mouseOffset.x) * sensitivity;
            transform.Rotate(rotation);
            rotationV = rotation;
            mouseReference = Input.mousePosition;
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
        StartCoroutine(RotateToStop());
    }

    private IEnumerator RotateToStop()
    {
        while (rotation != Vector3.zero)
        {
            rotation = Vector3.Lerp(rotation, Vector3.zero, stopTime * Time.deltaTime);
            transform.Rotate(rotation);
            rotationV = rotation;
            yield return null;
        }
    }
}