using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPlatform : MonoBehaviour {
    public GraphManager manager;
    private float lowPos = -3.5f;
    private float highPos = -1f;
    private Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = transform.localPosition;
        manager = GameObject.Find("Graph").GetComponent<GraphManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (manager.transform.childCount >= 1 && System.Math.Abs(transform.localPosition.y - lowPos) > float.Epsilon)
        {
            pos = transform.localPosition;
            StopAllCoroutines();
            StartCoroutine(LowerPlatform());
        } 
        if (manager.transform.childCount < 1 && System.Math.Abs(transform.localPosition.y - highPos) > float.Epsilon){
            pos = transform.localPosition;
            StopAllCoroutines();
            StartCoroutine(RaisePlatform());
        } 

       
		
	}
    private IEnumerator LowerPlatform()
    {
        while (System.Math.Abs(pos.y - lowPos) > float.Epsilon)
        {
            pos.y = Mathf.Lerp(pos.y, lowPos, Time.deltaTime);
            transform.localPosition = pos;
            yield return null;
        }
    }


    private IEnumerator RaisePlatform()
    {
        while (System.Math.Abs(pos.y - highPos) > float.Epsilon)
        {
            pos.y = Mathf.Lerp(pos.y, highPos, Time.deltaTime);
            transform.localPosition = pos;
            yield return null;
        }
    }

}
