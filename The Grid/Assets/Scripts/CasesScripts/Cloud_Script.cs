using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cloud_Script : MonoBehaviour {

	public CaseHandler myCase { get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler>();
		myCase.specialProperties ["Cloud"] = true;
		myCase.specialProperties ["Day&NightEffects"] = false;

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Clouds");
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);

		StartCoroutine (Routine ());
	}

	public IEnumerator Routine(){
		if (Random.Range (1, 101) <= Mathf.Min ((120 - myCase.caracs ["Humidity"]), 100)) {
			yield return new WaitForSeconds (1f);
			myCase.ChangeParam ("Humidity", 20);
			Destroy (gameObject);
		}
		yield return new WaitForSeconds (50f*myCase.timeM);
		StartCoroutine (Routine ());
	}

	void OnDestroy(){
		myCase.specialProperties ["Cloud"] = false;
		myCase.specialProperties ["Day&NightEffects"] = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
