using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {

    public GameObject xLine;
    public GameObject zLine;

    private GameObject cube;
    private RaycastHit hit;
    private Vector3 cameraRotation;
    private Vector3 xLinePosition;
    private Vector3 zLinePosition;


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Cube")
                {
                    DisplayLines(true);
                    cube = hit.collider.gameObject;
                    UpdateLine();
                }
            }
        }
        UpdateLine();

    }
    void UpdateLine()
    {
        if (cube == null )
        {
            DisplayLines(false);
            return;
        }
        if (xLine.transform.localPosition.y < 0 || zLine.transform.localPosition.y < 0) DisplayLines(false);
        else DisplayLines(true);

        xLinePosition.x = xLine.transform.localPosition.x;
        xLinePosition.y = cube.transform.lossyScale.y;
        xLinePosition.z = cube.transform.parent.transform.localPosition.z;
        xLine.transform.localPosition = xLinePosition;

        zLinePosition.x = cube.transform.localPosition.x;
        zLinePosition.y = cube.transform.lossyScale.y;
        zLinePosition.z = zLine.transform.localPosition.z;
        zLine.transform.localPosition = zLinePosition;

        xLine.transform.localScale = new Vector3(xLine.transform.localScale.x, xLine.transform.localScale.y, cube.transform.localPosition.x + 3.5f);
        zLine.transform.localScale = new Vector3(zLine.transform.localScale.x, zLine.transform.localScale.y, cube.transform.parent.transform.localPosition.z + +(-3.5f));
    }

    void DisplayLines(bool display)
    {
        xLine.SetActive(display);
        zLine.SetActive(display);
    }

}

