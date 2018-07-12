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
    private bool isLocked = true;


    private void Start()

    {
        database = transform.root.GetComponent<DBConnection>();
        manager = transform.root.GetComponent<GraphManager>();
        movement = Vector3.zero;
        minReached = false;
        maxReached = false;
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.M)) isLocked = !isLocked;
        isShiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isCtrlDown = (Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightApple)) || (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl));
        if(isShiftDown)
        {
            _sensitivity = sensitivity / 10f;
            if(isCtrlDown) _sensitivity /= 10f;
        }
        else _sensitivity = sensitivity;
        if (isDragged && !isLocked)
        {
            manager.StopAllCoroutines();
            //mouseOffset = Input.mousePosition - mouseReference;
            movement.y = Input.GetAxis("Mouse Y") * _sensitivity;
            transform.localScale += movement;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2.00f, transform.localPosition.z);
            //mouseReference = Input.mousePosition;
            maxReached = transform.localScale.y >= 10f;
            minReached = transform.localScale.y <= 0.1f;
            if (maxReached)
            {
                _sensitivity = 0;
                transform.localScale = new Vector3(transform.localScale.x, 10.00f, transform.localScale.z);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2.00f, transform.localPosition.z);
                Value = 100f;
                manager.data[X][Z] = Value;
                maxReached = false;
            }

            if (minReached)
            {
                _sensitivity = 0;
                transform.localScale = new Vector3(transform.localScale.x, 0.01f, transform.localScale.z);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2.00f, transform.localPosition.z);
                Value = 0f;
                manager.data[X][Z] = Value;
                database.UpdateSale(X, Z, Value);
                minReached = false;
            }
            else
            {
                Value = Utility.Truncate(transform.localScale.y * 10.00f, 2);
                manager.data[X][Z] = Value;
            }

            database.UpdateSale(X, Z, Value);

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
