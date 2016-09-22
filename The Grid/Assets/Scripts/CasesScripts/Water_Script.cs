using UnityEngine;
using System.Collections;

public class Water_Script : Case {

	// Use this for initialization
	protected override void Start () {
		base.Start();

		myCase.specialProperties ["Solid"] = false;
		if (myCase.specialProperties ["Fire"])
			Destroy (myCase.gameObject.transform.Find ("Fire").gameObject);

		StartCoroutine (Humidify ());
		StartCoroutine (Corrode ());
		StartCoroutine (CreateOrganisms ());
	}

	public IEnumerator Humidify(){
		yield return new WaitForSeconds (50*myCase.timeM);
		StartCoroutine (Humidify ());
		if (!myCase.specialProperties ["Paused"]) {
			
			foreach (CaseHandler neigh in myCase.neighbours.Values) {
				if (neigh.type == "Void")
					neigh.ChangeParam ("Humidity", 10);
			}

		}
	}

	public IEnumerator Corrode(){
		yield return new WaitForSeconds (10*myCase.timeM);
		StartCoroutine (Corrode ());

		if (!myCase.specialProperties ["Paused"]) {

			foreach (CaseHandler neigh in myCase.neighbours.Values) {
				if (neigh.type == "Stone") {
					neigh.caracs ["Corrosion"] += 3;
					if (neigh.caracs ["Corrosion"] >= 100) {

						bool goToSand = false;
						foreach (CaseHandler neighbour in neigh.neighbours.Values)
							if (neighbour.type == "Water" && neighbour.caracs ["Pressure"] >= 70)
								goToSand = true;
						if (goToSand)
							neigh.myAnim.CrossFade ("Sand", 0f);
						else
							neigh.myAnim.CrossFade ("Void", 0f);
						myCase.myAnim.CrossFade ("Void", 0f); //become void again

					}
				}
			}

		}
	}

	public IEnumerator CreateOrganisms(){
		yield return new WaitForSeconds (50 * myCase.timeM + Random.Range (1f, -1f));
		StartCoroutine (CreateOrganisms ());
		if (!myCase.specialProperties ["Paused"]) {

			if (!myCase.specialProperties ["Organism"] && myCase.caracs ["Heat"] >= 45
				&& myCase.caracs ["Heat"] <= 50 && Random.Range (1, 101) <= 30
			    || myCase.specialProperties ["Organism"])
				myCase.myHandler.NewAttribute (myCase.gameObject, "Organism");
			
		}
	}

	void OnDestroy(){
		foreach (Transform child in myCase.transform)
			if (child.gameObject.name == "Organism")
				Destroy (child.gameObject);
	}
		
}
