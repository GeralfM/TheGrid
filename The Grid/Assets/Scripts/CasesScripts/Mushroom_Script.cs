using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mushroom_Script : MonoBehaviour {

	public CaseHandler myCase{ get; set;}

	// Use this for initialization
	void Start () {
		myCase = gameObject.transform.parent.gameObject.GetComponent<CaseHandler> ();
		myCase.specialProperties ["Mushroom"] = true;

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Sprites/Mushroom");
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 1f);

		myCase.caracs.Add ("Mushrooms", 1);
		myCase.descriptionBonus.Add ("Mushrooms", "Mushrooms");
		StartCoroutine (Routine ());
	}

	public IEnumerator Routine(){
		yield return new WaitForSeconds (50*myCase.timeM+Random.Range(-1f,1f));
		StartCoroutine (Routine ());
		if (!myCase.specialProperties ["Paused"]) {
			
			if (Random.Range (1, 101) <= 50)
				myCase.caracs["Mushrooms"] = Mathf.Min (myCase.caracs["Mushrooms"] + 1, 12);

		}
	}

	void OnDestroy(){
		myCase.specialProperties ["Mushroom"] = false;
		myCase.caracs.Remove ("Mushrooms");
		myCase.descriptionBonus.Remove ("Mushrooms");
	}

	// Update is called once per frame
	void Update () {
		if (myCase.caracs ["Humidity"] <= 80 || myCase.caracs ["Heat"] <= 30 || myCase.caracs ["Heat"] >= 40)
			Destroy (gameObject);
	}
}
