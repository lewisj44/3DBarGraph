using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour {
    
    public float rows;
    public float columns;
    public GameObject _cube;
    public GameObject _base;
    public GameObject _dataText;
    public GameObject _xRotator;
    public GameObject _zRotator;
    public GameObject _labelCanvas;
    public GameObject _label;
    public Material[] colors;


    private float xOffset = 0.5f;
    private float yOffset = 0.5f;
    private GameObject[][] graph;

    private void Start()
    {
        //Initialze graph array
        int rowSize = (int)rows;
        int colSize = (int)columns;
        graph = new GameObject[rowSize][];
        for (int i = 0; i < graph.Length; i++)
        {
            graph[i] = new GameObject[colSize];
        }
        //Instatiate graph objects
        InstantiateBars();
        InstantiateGrid();
        InstantiateRotator(); //x
        InstantiateLabels();
    }

    private void InstantiateBars()
    {
        //indexes for storing graph bars in graph array
        int rowIndex = 0;
        int colIndex = 0;

        for (float y = (-rows / 2f); y < rows / 2f; y++)
        {
            //Normalize index to start at 0 
            rowIndex = (int)(y + rows / 2f);

            //Create row object to hold bars
            GameObject row = new GameObject("*Row " + (y + yOffset));
            row.transform.position = new Vector3(0f, 0f, y + yOffset);
            row.transform.parent = this.transform;

            colIndex = 0;
            for (float x = (-columns / 2); x < columns / 2; x++)
            {
                //Normalize index to start at 0 
                colIndex = (int)(x + columns / 2f);

                //Instantiate bar and set row object as parent
                GameObject bar = Instantiate(_cube);
                bar.transform.parent = row.transform;

                //Set a random height for bar
                float yScale = Random.Range(-5f, 5f);
                if(Mathf.Abs(yScale - 0f) < float.Epsilon) yScale = Random.Range(0, 5f);
                bar.transform.localScale = new Vector3(1f,yScale , 1f);

                //Position bar in x-axis. Z positon is set by parent Row object
                bar.transform.localPosition = new Vector3(x + xOffset, bar.transform.localScale.y / 2.0f, 0f);

                //Set bar color
                Material newMaterial = colors[Random.Range(0, colors.Length)];
                Renderer rend = bar.GetComponent<Renderer>();
                if (rend != null) rend.material = newMaterial;

                //Add bar to graph array
                graph[rowIndex][colIndex] = bar;
                colIndex++;
            }
            rowIndex++;
        }
    }


    private void InstantiateRotator(int axis = -1, bool flip = false)
    {   //0 == x-axis, 1 == y-axis, 2 == z-axis
        switch (axis)
        {
            case 0:
                GameObject xRot1 = Instantiate(_xRotator, new Vector3((columns + 2f) / 2f, 0f, 0f), Quaternion.identity) as GameObject;
                xRot1.transform.parent = this.transform;
                xRot1.transform.Rotate(new Vector3(90f, 90f, 0f));
                xRot1.GetComponent<Rotator>().axis = axis;

                GameObject xRot2 = Instantiate(_xRotator, new Vector3(-(columns + 2f) / 2f, 0f, 0f), Quaternion.identity) as GameObject;
                xRot2.transform.parent = this.transform;
                xRot2.transform.Rotate(new Vector3(90f, 90f, 0f));
                xRot2.GetComponent<Rotator>().axis = axis;
                break;

            case 1:
                GameObject graphBase = Instantiate(_base, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                graphBase.transform.localScale = new Vector3((columns + 2) / 10f, 1f, (rows + 2) / 10f);
                graphBase.transform.SetParent(this.transform);
                graphBase.GetComponent<Rotator>().axis = axis;
                graphBase.GetComponent<Rotator>().flipped = flip;
                if (flip) graphBase.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.right);
                else graphBase.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
                break;

            case 2:
                GameObject zRot1 = Instantiate(_zRotator, new Vector3(0f, 0f, (rows + 2f) / 2f), Quaternion.identity) as GameObject;
                zRot1.transform.parent = this.transform;
                zRot1.transform.Rotate(new Vector3(90f, 0f, 0f));
                zRot1.GetComponent<Rotator>().axis = axis;

                GameObject zRot2 = Instantiate(_zRotator, new Vector3(0f, 0f, -(rows + 2f) / 2f), Quaternion.identity) as GameObject;
                zRot2.transform.parent = this.transform;
                zRot2.transform.Rotate(new Vector3(90f, 0f, 0f));
                zRot2.GetComponent<Rotator>().axis = axis;
                break;
            default:
                InstantiateRotator(0);
                InstantiateRotator(1);
                InstantiateRotator(1, true);
                InstantiateRotator(2);
                break;

        }
    }

    private void InstantiateGrid()
    {
        //Create GridFloor parent obejct at origin
        GameObject grid = new GameObject("Floor");
        grid.transform.parent = this.transform;
        grid.transform.localPosition = new Vector3(0f, 0f, 0f);

        //Gridlines along x-axis
        for (float x = (-rows / 2); x < rows / 2; x++)
        {
            GameObject rowLine = new GameObject("rowLine " + x);
            rowLine.AddComponent<LineRenderer>();
            rowLine.transform.parent = grid.transform;
            rowLine.transform.localScale = new Vector3(1f, 1f, columns + 2f);
            rowLine.transform.localPosition = new Vector3(-rowLine.transform.localScale.z / 2f, 0f, x + xOffset);
            rowLine.transform.Rotate(Vector3.up * 90f);
            SetLineMaterial(rowLine);
            rowLine.transform.parent = grid.transform;
        }

        //Gridlines along z-axis
        for (float y = (-columns / 2); y < columns / 2; y++)
        {
            GameObject colLine = new GameObject("colLine " + y);
            colLine.AddComponent<LineRenderer>();
            colLine.transform.parent = grid.transform;
            colLine.transform.localScale = new Vector3(1f, 1f, rows + 2f);
            colLine.transform.localPosition = new Vector3(y + yOffset, 0f, -colLine.transform.localScale.z / 2f);
            SetLineMaterial(colLine);
            colLine.transform.parent = grid.transform;
        }
    }

    private void SetLineMaterial(GameObject obj)
    {
        Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        obj.GetComponent<LineRenderer>().useWorldSpace = false;
        obj.GetComponent<LineRenderer>().material = whiteDiffuseMat;
        obj.GetComponent<LineRenderer>().startColor = Color.white;
        obj.GetComponent<LineRenderer>().endColor = Color.white;
        obj.GetComponent<LineRenderer>().startWidth = 0.01f;
        obj.GetComponent<LineRenderer>().endWidth = 0.01f;
    }

    private void InstantiateLabels()
    {
        //Label for bar data
        GameObject dataText = Instantiate(_dataText, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        dataText.transform.parent = this.transform;

        //Canvas object to hold graph axis labels
        GameObject labelCanvas = Instantiate(_labelCanvas, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        labelCanvas.transform.parent = this.transform;

        //Label for x-axis
        GameObject xlabelTitle = Instantiate(_label, new Vector3(0f, 0f, -(rows + 6f) / 2f), Quaternion.identity) as GameObject;
        xlabelTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        xlabelTitle.transform.parent = labelCanvas.transform;
        TextMesh xlabelTitleText = xlabelTitle.GetComponent<TextMesh>();
        xlabelTitleText.text = "Title 1";
        xlabelTitleText.fontSize = 40;

        //Label for y-axis
        GameObject ylabelTitle = Instantiate(_label, new Vector3((columns + 6f) / 2f, 0f, 0f), Quaternion.identity) as GameObject;
        ylabelTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ylabelTitle.transform.parent = labelCanvas.transform;
        TextMesh ylabelTitleText = ylabelTitle.GetComponent<TextMesh>();
        ylabelTitleText.text = "Title 2";
        ylabelTitleText.fontSize = 40;

        //Labels for x-axis grid
        for (float x = (-columns / 2); x < columns / 2; x++)
        {
            GameObject label = Instantiate(_label, new Vector3((x + xOffset), 0f, -(rows + 3f)/2f), Quaternion.identity) as GameObject;
            label.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            label.transform.parent = labelCanvas.transform;
            TextMesh labelText = label.GetComponent<TextMesh>();
            labelText.text = "Grp";
        }

        //Labels for y-axis grid
        for (float y = (-rows / 2); y < rows / 2; y++)
        {
            GameObject label = Instantiate(_label, new Vector3((columns + 3f) / 2f, 0f, (y + yOffset)), Quaternion.identity) as GameObject;
            label.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            label.transform.parent = labelCanvas.transform;
            TextMesh labelText = label.GetComponent<TextMesh>();
            labelText.text = "Ctg";
        }
    }

}
