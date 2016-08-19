using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElementHandler : MonoBehaviour {

	public Encyclopedia myEn { get; set;}
	public string nameType { get; set;}

	// Use this for initialization
	void Start () {
	
	}

	public void SelfDisplay(){
		GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/" + nameType);
	}

	public void DisplayInfo(){
		if (myEn.allTypes [nameType]) {
			myEn.nameElement.GetComponent<Text> ().text = nameType;
			myEn.visualElement.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/" + nameType);
			myEn.descriptionElement.GetComponent<Text> ().text = myEn.myDescr [nameType];
		} else {
			myEn.nameElement.GetComponent<Text> ().text = "Undiscovered";
			myEn.visualElement.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Sprites/Undiscovered");
			myEn.descriptionElement.GetComponent<Text> ().text = "You don't have discovered this element yet";
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
