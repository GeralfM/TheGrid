using UnityEngine;
using System.Collections;

public class Water_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		StartCoroutine (humidify ());
	}

	public IEnumerator humidify(){
		yield return new WaitForSeconds (10);
		StartCoroutine (humidify ());
		foreach (CaseHandler neigh in myCase.neighbours.Values) {
			if (neigh.type == "Stone") {
				neigh.caracs ["Corrosion"] += 3;
				if (neigh.caracs ["Corrosion"] >= 100)
					neigh.myAnim.CrossFade ("Void", 0f);
			}
			else if (neigh.type == "Void")
				neigh.ChangeParam ("Humidity", 10);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
