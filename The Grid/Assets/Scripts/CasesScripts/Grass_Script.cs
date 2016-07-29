using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Grass_Script : MonoBehaviour {

	public CaseHandler myCase { get; set;}
	public List<CaseHandler> couldBurn = new List<CaseHandler> ();

	public bool maintain { get; set;}
	public float chrono { get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler>();
		myCase.specialProperties ["Grass"] = true;
		maintain = false;
		chrono = 0;

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Grass");
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 1f);

		StartCoroutine (IsItBurning());
	}

	public IEnumerator IsItBurning(){

		yield return new WaitForSeconds (1);
		StartCoroutine (IsItBurning ());

		couldBurn = new List<CaseHandler> ();
		foreach (CaseHandler neigh in myCase.neighbours.Values)
			couldBurn.Add (neigh);
		couldBurn.Add (myCase);

		bool burning = false;
		foreach (CaseHandler risk in couldBurn)
			if (risk.specialProperties ["Fire"])
				burning = true;
		if (burning == true)
			maintain = true;
		else {
			maintain = false;
			chrono = 0;
		}
	}

	void OnDestroy(){
		myCase.specialProperties ["Grass"] = false;
	}

	// Update is called once per frame
	void Update () {
	
		if (maintain) { // grass burns
			chrono += Time.deltaTime;
			if (chrono >= 25f)
				Destroy(gameObject);
		}

	}
}
