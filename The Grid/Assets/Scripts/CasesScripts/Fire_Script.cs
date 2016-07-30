using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Fire_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}
	public List<string> fromTypes = new List<string>{ "Void", "Steam", "Stone" };

	// Use this for initialization
	void Start () {
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler>();
		myCase.specialProperties ["Fire"] = true;

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Fire");
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 0.75f);

		StartCoroutine (Routine ());
		StartCoroutine (StillHere ());
	}

	public IEnumerator Routine(){
		yield return new WaitForSeconds (150*myCase.timeM);
		StartCoroutine (Routine ());

		Dictionary<int, CaseHandler> candidates = new Dictionary<int, CaseHandler> ();
		int i = 0;
		foreach (CaseHandler neigh in myCase.neighbours.Values) {
			if (fromTypes.Contains(neigh.type) && !neigh.specialProperties ["Fire"]) {
				i++;
				candidates.Add (i, neigh);
			}
		}
		if (i > 0) {
			int goal = Random.Range (1, i + 1);
			candidates [goal].myHandler.NewAttribute (candidates [goal].gameObject, "Fire");
		}
	}

	public IEnumerator StillHere(){
		yield return new WaitForSeconds (1);
		StartCoroutine (StillHere ());
		if (!myCase.specialProperties["Flammable"])
			Destroy (gameObject);
	}

	void OnDestroy(){
		myCase.specialProperties ["Fire"] = false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
