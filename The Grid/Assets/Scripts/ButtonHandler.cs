using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHandler : MonoBehaviour {

	public Image myImage { get; set;}

	// Use this for initialization
	void Start () {
		myImage = gameObject.GetComponent<Image> ();
	}

	public void SetSelected(bool itIs){
		if (itIs)
			myImage.color = new Color (1, 1, 1, 1);
		else
			myImage.color = new Color (1, 1, 1, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
