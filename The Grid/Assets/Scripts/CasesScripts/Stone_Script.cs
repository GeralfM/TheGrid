using UnityEngine;
using System.Collections;

public class Stone_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.GetComponent<CaseHandler> ();
		myCase.caracs.Add ("Corrosion", 0);
		myCase.descriptionBonus.Add ("Corrosion", "Corrosion");
	}

	void OnDestroy(){
		myCase.caracs.Remove ("Corrosion");
		myCase.descriptionBonus.Remove ("Corrosion");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
