using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBar : MonoBehaviour 
{
    public float sensitivity;
    public int X { get; set; }
    public int Z { get; set; }
    public string Quarter { get; set; }
    public string Year { get; set; }
    public float Value { get; set; }

    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 movement;
    private GraphManager manager;
    private DBConnection database;
    private bool isDragged;
    private float scale;
    private float _sensitivity;
    private bool maxReached;
    private bool minReached;
    private bool isShiftDown;
    private bool isCtrlDown;


    private void Start()

    {
        database = transform.root.GetComponent<DBConnection>();
        manager = transform.root.GetComponent<GraphManager>();
        sensitivity = 0.5f;
        movement = Vector3.zero;
    }

    private void Update()
    {
        isShiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isCtrlDown = (Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightApple))  || (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        if(isShiftDown)
        {
            _sensitivity = sensitivity / 10f;
            if(isCtrlDown) _sensitivity /= 10f;
        }
        else _sensitivity = sensitivity;
        if (isDragged)
        {
            //mouseOffset = Input.mousePosition - mouseReference;
            movement.y = Input.GetAxis("Mouse Y") * _sensitivity;
            transform.localScale += movement;
            transform.localPosition =  new Vector3(transform.localPosition.x, transform.localScale.y / 2.00f, transform.localPosition.z);
            //mouseReference = Input.mousePosition;
            maxReached = Mathf.Abs(transform.localScale.y) >= 10f;
            manager.data[X][Z] = Utility.Truncate(transform.localScale.y * 10.00f, 2);
            Debug.Log(manager.data[X][Z]);
            database.UpdateSale(X, Z, manager.data[X][Z]);
        }
        if(maxReached)
        {
            _sensitivity = 0;
            transform.localScale = transform.localScale.y > 0 ? new Vector3(1f, 10.00f, 1f) : new Vector3(1f, -10.00f, 1f);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2.00f, transform.localPosition.z);
            maxReached = false;
        }
        Value = manager.data[X][Z];
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
