using UnityEngine;
using System.Collections;

public class Dirt_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	public bool maintain { get; set;}
	public float chrono { get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		myCase.specialProperties ["Solid"] = true;
		maintain = false;
		chrono = 0;
		StartCoroutine (IsGrass ());
		StartCoroutine (IsMushroom ());
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

	public IEnumerator IsMushroom(){
		yield return new WaitForSeconds (50*myCase.timeM+Random.Range(-1f,1f));
		StartCoroutine (IsMushroom ());
		if (!myCase.specialProperties ["Paused"]) {
			
			if (myCase.caracs ["Humidity"] > 80 && myCase.caracs ["Heat"] > 30 && Random.Range(1,101)<=50
			   && myCase.caracs ["Heat"] < 40 && !myCase.specialProperties ["Mushroom"])
				myCase.myHandler.NewAttribute (myCase.gameObject, "Mushroom");
			
		}
	}

	public void GrassAppears(){
		myCase.myHandler.NewAttribute (myCase.gameObject, "Grass");
	}

	void OnDestroy(){
		if (myCase.specialProperties ["Grass"])
			Destroy (gameObject.transform.Find ("Grass").gameObject);
		if (myCase.specialProperties ["Mushroom"])
			Destroy (gameObject.transform.Find ("Mushroom").gameObject);
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
