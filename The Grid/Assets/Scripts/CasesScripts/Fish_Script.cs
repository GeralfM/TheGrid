using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fish_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler> ();

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Fish");
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 1f);

		if (!myCase.specialProperties ["Fish"]) {
			myCase.caracs.Add ("Fish", 1);
			myCase.specialProperties ["Fish"] = true;
			myCase.descriptionBonus.Add ("Fishes", "Fish");
		} else if (myCase.caracs ["Fish"] < 5)
			myCase.caracs ["Fish"]++;
		else
			Destroy (gameObject);
	}

	void OnDestroy(){
		myCase.caracs ["Fish"]--;
		if (myCase.caracs ["Fish"] == 0) {
			myCase.specialProperties ["Fish"] = false;
			myCase.descriptionBonus.Remove ("Fish");
		}
	}

	// Update is called once per frame
	void Update () {
		if (myCase.type != "Water")
			Destroy (gameObject);
	}
}
