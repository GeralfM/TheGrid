using UnityEngine;
using System.Collections;

public class Steam_Script : Case {
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		myCase.specialProperties ["Solid"] = false;
		StartCoroutine (Humidify ());
	}

	public IEnumerator Humidify(){
		yield return new WaitForSeconds (50*myCase.timeM);
		StartCoroutine (Humidify ());
		if (!myCase.specialProperties ["Paused"]) {
			
			foreach (CaseHandler neigh in myCase.neighbours.Values)
				neigh.ChangeParam ("Humidity", 5);

		}
	}
		
}
