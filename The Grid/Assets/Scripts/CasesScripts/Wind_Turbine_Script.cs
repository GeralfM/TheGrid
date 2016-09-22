using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wind_Turbine_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	public float diamondModifier{ get; set;}

	// Use this for initialization
	public void Start () {
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler> ();
		myCase.myHandler.ChangeEnergy (-5f);
		myCase.specialProperties ["Building"] = true;

		diamondModifier = 1f;

		StartCoroutine (ProduceEnergy ());
		StartCoroutine (StillHere ());
	}

	public IEnumerator ProduceEnergy(){
		yield return new WaitForSeconds (1f*myCase.timeM);
		StartCoroutine (ProduceEnergy ());
		if (!myCase.specialProperties ["Paused"]) {

			gameObject.GetComponentInChildren<Animator> ().speed = myCase.caracs ["WindPower"] / 3f;
			myCase.myHandler.ChangeEnergy (0.2f / 50 * diamondModifier * myCase.caracs ["WindPower"]);

		}
	}

	public IEnumerator StillHere(){
		yield return new WaitForSeconds (1);
		StartCoroutine (StillHere ());
		if (myCase.type == "Diamond") {
			gameObject.GetComponentInChildren<Animator> ().CrossFade ("Diamond_Wind_Turbine", 0f);
			myCase.myHandler.encyclopedia.GetComponent<Encyclopedia> ().CheckDiscovered("Diamond_Wind_Turbine");
			diamondModifier = 0.5f;
		}
		if (!new List<string>{"Dirt","Stone","Diamond"}.Contains(myCase.type))
			Destroy (gameObject);
	}

	void OnDestroy(){
		myCase.specialProperties ["Building"] = false;
	}
		
	// Update is called once per frame
	public void Update () {
	}
}
