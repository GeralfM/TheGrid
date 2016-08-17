using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Cloud_Script : MonoBehaviour {

	public CaseHandler myCase { get; set;}

	// Use this for initialization
	void Start () {
		Initialize ();

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Cloud");
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);

		StartCoroutine (Routine ());
		StartCoroutine (Move (25f));
	}

	public void Initialize(){
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler>();
		myCase.specialProperties ["Cloud"] = true;
		myCase.specialProperties ["Day&NightEffects"] = false;
	}

	public IEnumerator Routine(){
		if (Random.Range (1, 101) <=
			Mathf.Min ((120 - myCase.caracs ["Humidity"]), 100) && !myCase.specialProperties["Paused"]) {
			yield return new WaitForSeconds (1f);
			myCase.ChangeParam ("Humidity", 20);
			Destroy (gameObject);
		}
		yield return new WaitForSeconds (50f*myCase.timeM);
		StartCoroutine (Routine ());
	}

	public IEnumerator Move(float delay){
		yield return new WaitForSeconds (delay * myCase.timeM + Random.Range (-0.5f, 0.5f));
		if (myCase.caracs["Wind"]!=-1 && !myCase.specialProperties ["Paused"]) {

			if (myCase.windGoal.specialProperties ["Cloud"] == false) {
				myCase.specialProperties ["Cloud"] = false;
				myCase.specialProperties ["Day&NightEffects"] = true;

				gameObject.transform.SetParent (myCase.windGoal.gameObject.transform);

				RectTransform rec = GetComponent<RectTransform> ();
				rec.localPosition = new Vector3 (0, 0, 0);
				rec.offsetMin = Vector2.zero;
				rec.offsetMax = Vector2.zero;

				Initialize ();
				myCase.myHandler.OrganizeAttributes (myCase.gameObject);
			} 
	
		}
		yield return new WaitForSeconds (50f*myCase.timeM+Random.Range(-1f,1f));
		StartCoroutine (Move (0f));
	}

	void OnDestroy(){
		myCase.specialProperties ["Cloud"] = false;
		myCase.specialProperties ["Day&NightEffects"] = true;
	}
		
	
	// Update is called once per frame
	void Update () {
	
	}
}
