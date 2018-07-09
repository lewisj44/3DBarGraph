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

    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 movement;
    private bool isDragged;
    private float scale;
    private float _sensitivity;
    private bool maxReached;
    private bool minReached;
    private GraphManager manager;
    private DBConnection database;
    private bool isShiftDown;


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
        if(isShiftDown)
        {
            _sensitivity = sensitivity / 10f;
            isShiftDown = false;
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
            manager.data[X][Z] = Mathf.Round(transform.localScale.y * 10.00f);
            database.UpdateSale(X, Z, (int)manager.data[X][Z]);
        }
        if(maxReached)
        {
            _sensitivity = 0;
            transform.localScale = transform.localScale.y > 0 ? new Vector3(1f, 10.00f, 1f) : new Vector3(1f, -10.00f, 1f);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localScale.y / 2.00f, transform.localPosition.z);
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
