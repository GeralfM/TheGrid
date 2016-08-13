using UnityEngine;
using System.Collections;

public class Dirt_Script : Case {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		myCase.specialProperties ["Solid"] = true;

		goalTimes.Add ("Fire", 150f);
		goalTimes.Add ("Grass", 50f);
		maintainTimes.Add ("Grass", false);
		chronoTimes.Add ("Grass", 0f);

		StartCoroutine (IsGrass ());
		StartCoroutine (IsMushroom ());
	}

	public IEnumerator IsGrass(){
		yield return new WaitForSeconds (1);
		StartCoroutine (IsGrass ());
		if (myCase.caracs ["Humidity"] > 30 && !myCase.specialProperties ["Grass"])
			maintainTimes["Grass"] = true;
		else {
			maintainTimes["Grass"] = false;
			chronoTimes["Grass"] = 0f;
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
	protected override void Update () {
		base.Update ();

		if (maintainTimes["Grass"] && !myCase.specialProperties["Paused"]) { // grass growth
			chronoTimes["Grass"] += Time.deltaTime;
			if (chronoTimes["Grass"] >= goalTimes["Grass"]*myCase.timeM) {
				GrassAppears ();
				chronoTimes["Grass"] = 0f;
			}
		}

	}

}
