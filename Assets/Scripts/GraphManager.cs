using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour 
{    
    public float rows;
    public float columns;
    public float[][] data;
    public float scaleFactor;
    public float dragSensitivity;
    public float rotateSensitivity;

    public GameObject _cube;
    public GameObject _base;
    public GameObject _dataText;
    public GameObject _xRotator;
    public GameObject _zRotator;
    public GameObject _labelCanvas;
    public GameObject _label;
    public Material[] colors;



    private float xOffset;
    private float zOffset;
    private bool barLoaded = false;
    private GameObject[][] graph;
    private DBConnection database;
    private int switchCount;

	private void Start()
	{
        
       // LoadGraph();
	}




	private void InstantiateBars()
    {
        //Indexes for array storage
        int rowIndex = 0;
        int colIndex = 0;

        for (float z = (-rows / 2f); z < rows / 2f; z++)
        {
            //Normalize index to start at 0 
            rowIndex = (int)(z + rows / 2f);

            //Create row object to hold bars
            GameObject row = new GameObject("*Row " + (z + zOffset));
            row.transform.localPosition = new Vector3(0f, 0f, z + zOffset);
            row.transform.parent = this.transform;

            colIndex = 0;
            for (float x = (-columns / 2); x < columns / 2; x++)
            {
                barLoaded = false;
                //Normalize index to start at 0 
                colIndex = (int)(x + columns / 2f);

                //Instantiate bar and set row object as parent
                GameObject bar = Instantiate(_cube);
                bar.transform.parent = row.transform;

                //Set bar values
                bar.GetComponent<DragBar>().sensitivity = dragSensitivity;
                bar.GetComponent<DragBar>().X = colIndex;
                bar.GetComponent<DragBar>().Z = rowIndex;
                bar.GetComponent<DragBar>().Year = "" + (colIndex + database.GetStartYear());
                bar.GetComponent<DragBar>().Quarter = "Q" + (rowIndex + 1);
                bar.GetComponent<DragBar>().Value = (float)database.GetSale((database.GetStartYear() + colIndex), rowIndex);

                //Set bar color
                Renderer rend = bar.GetComponent<Renderer>();
                rend.material = colors[colIndex % 6];

                float yScale = bar.GetComponent<DragBar>().Value/10f;
                Vector3 targetScale = new Vector3(0.75f, yScale, 0.75f);
                Vector3 currentScale = new Vector3(0.75f, bar.transform.localScale.y, 0.75f);
                //Add bar to array
                graph[colIndex][rowIndex] = bar;
                data[colIndex][rowIndex] = bar.GetComponent<DragBar>().Value;
                colIndex++;

                //bar.transform.localScale = targetScale;
                //Position bar in x-axis. Z positon is set by parent Row object
                //bar.transform.localPosition = new Vector3(x + xOffset, yScale / 2.00f, 0f);
                if(bar != null)StartCoroutine(LoadBarData(bar, currentScale, targetScale, x));



               

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
                xRot1.GetComponent<Rotator>().sensitivity = rotateSensitivity;


                GameObject xRot2 = Instantiate(_xRotator, new Vector3(-(columns + 2f) / 2f, 0f, 0f), Quaternion.identity) as GameObject;
                xRot2.transform.parent = this.transform;
                xRot2.transform.Rotate(new Vector3(90f, 90f, 0f));
                xRot2.GetComponent<Rotator>().axis = axis;
                xRot2.GetComponent<Rotator>().sensitivity = rotateSensitivity;
                break;

            case 1:
                GameObject graphBase = Instantiate(_base, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
                graphBase.transform.localScale = new Vector3((columns + 2) / 10f, 1f, (rows + 2) / 10f);
                graphBase.transform.SetParent(this.transform);
                graphBase.GetComponent<Rotator>().axis = axis;
                graphBase.GetComponent<Rotator>().sensitivity = rotateSensitivity;
                graphBase.GetComponent<Rotator>().flipped = flip;
                if (flip) graphBase.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.right);
                else graphBase.transform.localRotation = Quaternion.AngleAxis(180f, Vector3.up);
                break;

            case 2:
                GameObject zRot1 = Instantiate(_zRotator, new Vector3(0f, 0f, (rows + 2f) / 2f), Quaternion.identity) as GameObject;
                zRot1.transform.parent = this.transform;
                zRot1.transform.Rotate(new Vector3(90f, 0f, 0f));
                zRot1.GetComponent<Rotator>().axis = axis;
                zRot1.GetComponent<Rotator>().sensitivity = rotateSensitivity;

                GameObject zRot2 = Instantiate(_zRotator, new Vector3(0f, 0f, -(rows + 2f) / 2f), Quaternion.identity) as GameObject;
                zRot2.transform.parent = this.transform;
                zRot2.transform.Rotate(new Vector3(90f, 0f, 0f));
                zRot2.GetComponent<Rotator>().axis = axis;
                zRot2.GetComponent<Rotator>().sensitivity = rotateSensitivity;
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
        //grid.transform.localPosition = new Vector3(0f, 0f, 0f);

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
        for (float z = (-columns / 2); z < columns / 2; z++)
        {
            GameObject colLine = new GameObject("colLine " + z);
            colLine.AddComponent<LineRenderer>();
            colLine.transform.parent = grid.transform;
            colLine.transform.localScale = new Vector3(1f, 1f, rows + 2f);
            colLine.transform.localPosition = new Vector3(z + zOffset, 0f, -colLine.transform.localScale.z / 2f);
            SetLineMaterial(colLine);
            colLine.transform.parent = grid.transform;
        }
    }

    //Set color of gridLines
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
        dataText.transform.SetParent(this.transform);

        //Canvas object to hold graph axis labels
        GameObject labelCanvas = Instantiate(_labelCanvas, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        labelCanvas.transform.SetParent(this.transform);

        //Label for x-axis
        GameObject xlabelTitle = Instantiate(_label, new Vector3(0f, 0f, -(rows + 6f) / 2f), Quaternion.identity) as GameObject;
        xlabelTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        xlabelTitle.transform.SetParent(labelCanvas.transform);
        TextMesh xlabelTitleText = xlabelTitle.GetComponent<TextMesh>();
        xlabelTitleText.text = "Year";
        xlabelTitleText.fontSize = 40;

        //Label for x-axis
        GameObject xlabelTitle2 = Instantiate(_label, new Vector3(0f, 0f, (rows + 6f) / 2f), Quaternion.identity) as GameObject;
        xlabelTitle2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        xlabelTitle2.transform.SetParent(labelCanvas.transform);
        TextMesh xlabelTitleText2 = xlabelTitle2.GetComponent<TextMesh>();
        xlabelTitleText2.text = "Year";
        xlabelTitleText2.fontSize = 40;

        //Label for y-axis
        GameObject ylabelTitle = Instantiate(_label, new Vector3(-(columns + 6f) / 2f, 0f, 0f), Quaternion.identity) as GameObject;
        ylabelTitle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ylabelTitle.transform.SetParent(labelCanvas.transform);
        TextMesh ylabelTitleText = ylabelTitle.GetComponent<TextMesh>();
        ylabelTitleText.text = "Quarter";
        ylabelTitleText.fontSize = 40;

        //Label for y-axis
        GameObject ylabelTitle2 = Instantiate(_label, new Vector3((columns + 6f) / 2f, 0f, 0f), Quaternion.identity) as GameObject;
        ylabelTitle2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ylabelTitle2.transform.SetParent(labelCanvas.transform);
        TextMesh ylabelTitleText2 = ylabelTitle2.GetComponent<TextMesh>();
        ylabelTitleText2.text = "Quarter";
        ylabelTitleText2.fontSize = 40;

        //Labels for x-axis grid
        for (float x = (-columns / 2); x < columns / 2; x++)
        {
            int year = (int)(x + columns / 2f) + database.GetStartYear();

            GameObject label = Instantiate(_label, new Vector3((x + xOffset), 0f, -(rows + 3f)/2f), Quaternion.identity) as GameObject;
            label.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            label.transform.SetParent(labelCanvas.transform);
            TextMesh labelText = label.GetComponent<TextMesh>();
            labelText.text = year + "";

            GameObject label2 = Instantiate(_label, new Vector3((x + xOffset), 0f, (rows + 3f) / 2f), Quaternion.identity) as GameObject;
            label2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            label2.transform.SetParent(labelCanvas.transform);
            TextMesh labelText2 = label2.GetComponent<TextMesh>();
            labelText2.text = year + "";

        }

        //Labels for y-axis grid
        for (float z = (-rows / 2); z < rows / 2; z++)
        {
            int quarter = (int)(z + rows / 2f) + 1;

            GameObject label = Instantiate(_label, new Vector3(-(columns + 3f) / 2f, 0f, (z + zOffset)), Quaternion.identity) as GameObject;
            label.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            label.transform.SetParent(labelCanvas.transform);
            TextMesh labelText = label.GetComponent<TextMesh>();
            labelText.text = "Q" + quarter;

            GameObject label2 = Instantiate(_label, new Vector3((columns + 3f) / 2f, 0f, (z + zOffset)), Quaternion.identity) as GameObject;
            label2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            label2.transform.SetParent(labelCanvas.transform);
            TextMesh labelText2 = label2.GetComponent<TextMesh>();
            labelText2.text = "Q" + quarter;
        }
    }

    private IEnumerator LoadBarData(GameObject bar, Vector3 curScale, Vector3 targetScale, float xPos)
    {
        while (!barLoaded)
        {
            if (bar != null)
            {
                curScale.y = Mathf.Lerp(curScale.y, targetScale.y, Time.deltaTime);
                bar.transform.localScale = curScale;
                //Position bar in x-axis. Z positon is set by parent Row object
                bar.transform.localPosition = new Vector3(xPos + xOffset, curScale.y / 2.00f, 0f);
                barLoaded = (curScale.Equals(targetScale));
            }
            yield return null;
        }
    }

	public void LoadGraph(string dbName = null)
    {
        if (GameObject.Find("Graph/Floor") != null) return;
        Debug.Log("CHILD COUNT" + transform.childCount);
        //Connect database and get graph dimsensions
        database = GetComponent<DBConnection>();
        database.LoadData(dbName);
        rows = database.GetNumQuarters();
        columns = database.GetNumYears();

        //Get position offset
        xOffset = transform.localScale.x / 2f;
        zOffset = transform.localScale.z / 2f;

        //Initialize data and graph array
        int rowSize = (int)rows;
        int colSize = (int)columns;
        graph = new GameObject[colSize][];
        data = new float[colSize][];
        for (int i = 0; i < colSize; i++)
        {
            graph[i] = new GameObject[rowSize];
            data[i] = new float[rowSize];
        }

        //Instatiate graph objects
        InstantiateBars();
        InstantiateGrid();
        InstantiateRotator(); 
        InstantiateLabels();
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }


    public void Delete()
    {
        if (transform.childCount > 0){

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
                transform.localScale = new Vector3(1f, 1f, 1f);
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
