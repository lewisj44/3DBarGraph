using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBar : MonoBehaviour {

    public float sensitivity;
    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 movement;
    private bool isDragged;
    private float scale;
    private float _sensitivity;
    private bool maxReached;
    private bool minReached;

    private void Start()

    {
        sensitivity = 0.05f;
        movement = Vector3.zero;
    }

    private void Update()
    {
        _sensitivity = sensitivity;
        if (isDragged)
        {
            //mouseOffset = Input.mousePosition - mouseReference;
            movement.y = Input.GetAxis("Mouse Y");
            transform.localScale += movement;
            transform.localPosition =  new Vector3(transform.localPosition.x, transform.localScale.y / 2.0f, transform.localPosition.z);
            //mouseReference = Input.mousePosition;
            maxReached = Mathf.Abs(transform.localScale.y) >= 6.5f;
        }
        if(maxReached)
        {
            _sensitivity = 0;
            transform.localScale = transform.localScale.y > 0 ? new Vector3(1f, 6.5f, 1f) : new Vector3(1f, -6.5f, 1f);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2.0f, transform.localPosition.z);
            maxReached = false;
        }

    }

    private void OnMouseDown()
    {
        isDragged = true;
        //mouseReference = Input.mousePosition;
    }


    private void OnMouseUp()
    {
        isDragged = false;
    }


	
}
