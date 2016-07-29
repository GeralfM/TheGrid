using UnityEngine;
using System.Collections;

public class Water_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		StartCoroutine (Humidify ());
		StartCoroutine (CreateOrganisms ());
	}

	public IEnumerator Humidify(){
		yield return new WaitForSeconds (50);
		StartCoroutine (Humidify ());
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

	public IEnumerator CreateOrganisms(){
		yield return new WaitForSeconds (50);
		StartCoroutine (CreateOrganisms ());
		if (myCase.myGround.type == "none" && myCase.caracs ["Heat"] >= 45 
			&& myCase.caracs ["Heat"] <= 50 && Random.Range (1, 101) <= 30)
			myCase.myGround.myAnim.CrossFade ("Organism", 0f);
	}

	void OnDestroy(){
		if (myCase.myGround.type == "Organism") {
			myCase.myGround.myAnim.CrossFade ("none",0f);
			Destroy (myCase.myGround.GetComponent<Organism_Script> ());
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
