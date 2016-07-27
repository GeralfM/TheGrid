using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GroundHandler : MonoBehaviour {

	public Image myImage { get; set;}
	public CaseHandler myCase { get; set;}

	public string type { get; set; }
	public Animator myAnim { get; set;}

	// Use this for initialization
	void Start () {
		myAnim = gameObject.GetComponent<Animator> ();
		myImage = gameObject.GetComponent<Image> ();
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler> ();
	}

	public void SetSprite(string newType){
		type = newType;
		if (newType == "none")
			myImage.enabled = false;
		else {
			myImage.enabled = true;
			myImage.sprite = Resources.Load<Sprite> ("Sprites/" + newType);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
