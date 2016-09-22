using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Organism_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}
	public Dictionary<int, string> directions = new Dictionary<int, string>();

	public int hoursCount{ get; set;}

	// Use this for initialization
	void Start () {
		hoursCount = 0;

		Initialize ();
		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Organism");
		StartCoroutine (Movement ());
		StartCoroutine (StillHere ());
		StartCoroutine (PrepareEvolution ());
	}

	public void Initialize(){
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler> ();

		directions = new Dictionary<int, string> ();
		int i = 1;
		foreach (string dir in myCase.neighbours.Keys) {
			if (myCase.neighbours[dir].type == "Water" && myCase.neighbours[dir].caracs ["Organisms"] < 10) {
				directions.Add (i, dir);
				i++;
			}
		}

		myCase.caracs["Organisms"]++;
		if (!myCase.specialProperties ["Organism"]) {
			myCase.specialProperties ["Organism"] = true;
			myCase.descriptionBonus.Add ("Organisms", "Organisms");
		}
	}

	public IEnumerator PrepareEvolution(){
		yield return new WaitForSeconds (50 * myCase.timeM);
		hoursCount++;
		if (hoursCount < 72)
			StartCoroutine (PrepareEvolution ());
		else
			StartCoroutine (Evolve ());
	}
	public IEnumerator Evolve(){
		yield return new WaitForSeconds (50 * myCase.timeM + Random.Range (1f, -1f)); 
		StartCoroutine (Evolve ());
		if (!myCase.specialProperties ["Paused"]) {

			if (Random.Range (1, 101) <= 10) {
				myCase.myHandler.NewAttribute (myCase.gameObject, "Fish");
				MajCaracs ();
				Destroy (gameObject);
			}

		}

	}

	public IEnumerator Movement(){
		yield return new WaitForSeconds (100 * myCase.timeM + Random.Range (1f, -1f));
		StartCoroutine (Movement ());
		if (!myCase.specialProperties ["Paused"]) {

			int choice = Random.Range (0, directions.Values.Count + 1);
			if (choice != 0) {
				CaseHandler goal = myCase.neighbours[directions [Random.Range (1, directions.Values.Count + 1)]];
				MajCaracs ();

				if (goal.type == "Water" && goal.caracs ["Heat"] >= 45 && goal.caracs ["Heat"] <= 55
				    && goal.caracs ["Organisms"] < 10) {
					gameObject.transform.SetParent (goal.gameObject.transform);
					GetComponent<Transform> ().localPosition = new Vector3 (0, 0, 0);
					GetComponent<RectTransform> ().offsetMin = Vector2.zero;
					GetComponent<RectTransform> ().offsetMax = Vector2.zero;
					Initialize ();
					myCase.myHandler.OrganizeAttributes (myCase.gameObject);
				} else 
					Destroy (gameObject);
			} 

		}
	}

	public IEnumerator StillHere(){
		yield return new WaitForSeconds (1);
		StartCoroutine (StillHere ());
		if (myCase.caracs ["Heat"] < 45 || myCase.caracs ["Heat"] > 55) {
			MajCaracs ();
			Destroy (gameObject);
		}
	}

	public void MajCaracs(){
		myCase.caracs ["Organisms"]--;
		if (myCase.caracs ["Organisms"] == 0) {
			myCase.specialProperties ["Organism"] = false;
			myCase.descriptionBonus.Remove ("Organisms");
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
