using UnityEngine;
using System.Collections;

public class Ice_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		myCase.specialProperties ["Solid"] = true;
		StartCoroutine (Freeze ());
	}

	public IEnumerator Freeze(){
		yield return new WaitForSeconds (1);
		StartCoroutine (Freeze ());
		foreach (CaseHandler neigh in myCase.neighbours.Values)
			if (neigh.type == "Water" && neigh.caracs ["Heat"] <= 10) {
				neigh.ChangeParam ("Heat", 5-neigh.caracs ["Heat"]);
				neigh.myAnim.CrossFade ("Ice",0f);
			}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
