using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Case : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	public Dictionary<string,float> goalTimes = new Dictionary<string, float> ();
	public Dictionary<string,bool> maintainTimes = new Dictionary<string, bool> ();
	public Dictionary<string,float> chronoTimes = new Dictionary<string, float> ();

	// Use this for initialization
	protected virtual void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		if(myCase.specialProperties["Flammable"])
			chronoTimes.Add ("Fire", 0f);
	}

	// Update is called once per frame
	protected virtual void Update () {

		if (myCase.specialProperties ["Flammable"])
			if (myCase.specialProperties ["Fire"] && !myCase.specialProperties ["Paused"]) {
				chronoTimes ["Fire"] += Time.deltaTime;
				if (chronoTimes ["Fire"] >= goalTimes ["Fire"] * myCase.timeM) {
					if (myCase.type != "Carbon") {
						myCase.myAnim.CrossFade ("Carbon", 0f);
						Destroy (myCase.gameObject.transform.Find ("Fire").gameObject);
					} else
						myCase.myAnim.CrossFade ("Void", 0f);
				}
			}
			
	}

}
