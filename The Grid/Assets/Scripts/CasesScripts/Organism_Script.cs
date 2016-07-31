using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Organism_Script : MonoBehaviour {

	public GroundHandler myGround { get; set;}
	public List<string> directions = new List<string>();

	// Use this for initialization
	void Start () {
		myGround = gameObject.GetComponent<GroundHandler> ();
		foreach (string dir in myGround.myCase.neighbours.Keys)
			directions.Add (dir);
		directions.Add ("still");

		myGround.myCase.caracs.Add ("Organism", 1);
		myGround.myCase.descriptionBonus.Add ("Organisms", "Organism");

		StartCoroutine (Growth ());
		StartCoroutine (Movement ());

	}

	public IEnumerator Growth(){
		yield return new WaitForSeconds (50*myGround.myCase.timeM);
		StartCoroutine (Growth ());
		if (!myGround.myCase.specialProperties ["Paused"])
			myGround.myCase.caracs ["Organism"] = Mathf.Min (myGround.myCase.caracs ["Organism"] + 1, 10);
	}

	public IEnumerator Movement(){
		yield return new WaitForSeconds (101*myGround.myCase.timeM);
		StartCoroutine (Movement ());
		if (!myGround.myCase.specialProperties ["Paused"]) {
			
			Dictionary<string,int> division = new Dictionary<string, int> ();
			foreach (string dir in directions)
				division.Add (dir, 0);

			for (int i = 1; i <= myGround.myCase.caracs ["Organism"]; i++) {
				int test = Random.Range (1, division.Count + 1);
				int iter = 0;
				foreach (string dir in directions) {
					iter++;
					if (iter == test)
						division [dir]++;
				}

			}

			foreach (string dir in directions) {
				if (dir != "still" && division [dir] != 0) {
					myGround.myCase.caracs ["Organism"] -= division [dir];
					CaseHandler goal = myGround.myCase.neighbours [dir];
					if (goal.type == "Water" && goal.caracs ["Heat"] <= 55 && goal.caracs ["Heat"] >= 45) {
						int isNew = 0;
						if (goal.myGround.type == "none" && goal.caracs ["Heat"] <= 50) {
							goal.myGround.myAnim.CrossFade ("Organism", 0f);
							isNew = 1;
						}
						yield return new WaitForSeconds (0.1f);
						goal.caracs ["Organism"] += division [dir] - isNew;
					}
				}
			}

			if (myGround.myCase.caracs ["Organism"] == 0) {
				yield return new WaitForSeconds (0.5f);
				myGround.myAnim.CrossFade ("Void", 0f);
			}

		}
	}

	void OnDestroy(){
		myGround.myCase.caracs.Remove ("Organism");
		myGround.myCase.descriptionBonus.Remove ("Organisms");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
