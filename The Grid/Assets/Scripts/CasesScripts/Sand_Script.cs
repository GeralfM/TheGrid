using UnityEngine;
using System.Collections;

public class Sand_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		StartCoroutine (IsSolid ());
	}

	public IEnumerator IsSolid(){
		myCase.specialProperties ["Solid"] = (myCase.caracs ["Humidity"] <= 70);
		yield return new WaitForSeconds (1);
		StartCoroutine (IsSolid ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
