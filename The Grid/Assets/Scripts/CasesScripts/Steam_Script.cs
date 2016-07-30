using UnityEngine;
using System.Collections;

public class Steam_Script : MonoBehaviour {
	
	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		StartCoroutine (Humidify ());
	}

	public IEnumerator Humidify(){
		yield return new WaitForSeconds (50*myCase.timeM);
		StartCoroutine (Humidify ());
		foreach (CaseHandler neigh in myCase.neighbours.Values) {
			neigh.ChangeParam ("Humidity", 5);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
