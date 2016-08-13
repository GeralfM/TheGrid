using UnityEngine;
using System.Collections;

public class Stone_Script : Case {

	// Use this for initialization
	protected override void Start () {
		base.Start();

		goalTimes.Add ("Fire", 400f);

		myCase.specialProperties ["Solid"] = true;
		myCase.caracs.Add ("Corrosion", 0);
		myCase.descriptionBonus.Add ("Corrosion", "Corrosion");
	}

	void OnDestroy(){
		myCase.caracs.Remove ("Corrosion");
		myCase.descriptionBonus.Remove ("Corrosion");
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	
	}
}
