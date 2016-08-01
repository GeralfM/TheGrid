using UnityEngine;
using System.Collections;

public class Dirt_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	public bool maintain { get; set;}
	public float chrono { get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		maintain = false;
		chrono = 0;
		StartCoroutine (IsGrass ());
	}

	public IEnumerator IsGrass(){
		yield return new WaitForSeconds (1);
		StartCoroutine (IsGrass ());
		if (myCase.caracs ["Humidity"] > 30 && !myCase.specialProperties ["Grass"])
			maintain = true;
		else {
			maintain = false;
			chrono = 0f;
		}
	}

	public void GrassAppears(){
		myCase.myHandler.NewAttribute (myCase.gameObject, "Grass");
	}

	void OnDestroy(){
		if (myCase.specialProperties ["Grass"])
			Destroy (gameObject.transform.Find ("Grass").gameObject);
	}

	// Update is called once per frame
	void Update () {

		if (maintain && !myCase.specialProperties["Paused"]) { // grass growth
			chrono += Time.deltaTime;
			if (chrono >= 50f*myCase.timeM) {
				GrassAppears ();
				chrono = 0f;
			}
		}

	}

}
