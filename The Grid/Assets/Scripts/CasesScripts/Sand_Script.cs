using UnityEngine;
using System.Collections;

public class Sand_Script : Case {

	// Use this for initialization
	protected override void Start () {
		base.Start();
		goalTimes.Add ("Fire", 100f);
		StartCoroutine (IsSolid ());
	}

	public IEnumerator IsSolid(){
		myCase.specialProperties ["Solid"] = (myCase.caracs ["Humidity"] <= 70);
		yield return new WaitForSeconds (1);
		StartCoroutine (IsSolid ());
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	
	}
}
