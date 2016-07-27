using UnityEngine;
using System.Collections;

public class CursorHandler : MonoBehaviour {

	public string cursorState { get; set;}
	public bool isLeft { get; set;}

	// Use this for initialization
	void Start () {
		cursorState = "none";
	}

	public void SetState(string state){
		cursorState = state;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
			isLeft=true;
		if(Input.GetMouseButtonDown(1))
			isLeft=false;
	}
}
