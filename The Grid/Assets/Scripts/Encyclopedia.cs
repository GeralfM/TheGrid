using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Encyclopedia : MonoBehaviour {

	public GameObject firstElement;
	public Dictionary<string, string> myDescr = new Dictionary<string, string>();

	public GameObject nameElement;
	public GameObject visualElement;
	public GameObject descriptionElement;

	public Dictionary<string,bool> allTypes = new Dictionary<string, bool>();
	public List<ElementHandler> myElements = new List<ElementHandler>();

	public PopupHandler myPopup { get; set;}

	void Awake(){

		myPopup = GameObject.Find ("PopupInfo").GetComponent<PopupHandler> ();

		if (!(Application.platform == RuntimePlatform.WindowsPlayer)) {
			string[] descr = Resources.Load<TextAsset> ("Files/Descriptions").text.Split 
				(new string[]{ "===" + System.Environment.NewLine }, System.StringSplitOptions.None);
			for (int k = 0; k < descr.Length; k += 2)
				myDescr.Add (descr [k].Replace (System.Environment.NewLine, ""), descr [k + 1]);
		} else {
			string[] descr = Resources.Load<TextAsset> ("Files/Descriptions").text.Split 
				(new string[]{ "===\n" }, System.StringSplitOptions.None);
			for (int k = 0; k < descr.Length; k += 2)
				myDescr.Add (descr [k].Replace ("\n", ""), descr [k + 1]);
		}
			
		int numBlocs = 18;
		int i; int j; int count = 0;
		string[] types = new string[] {"Carbon", "Cloud", "Diamond", "Diamond_Wind_Turbine", 
			"Dirt", "Fire", "Fish", "Grass",
			"Ice", "Lava", "Mushroom", "Organism", "Sand", 
			"Steam", "Stone", "Void", "Water", "Wind_Turbine"};
		foreach (string aType in types)
			allTypes.Add (aType, false);
		allTypes ["Void"] = true;

		do {
			i = count / 10;
			j = count % 10;
			GameObject newElement = Instantiate (firstElement);
			newElement.transform.SetParent (GameObject.Find ("Encyclopedia").transform);
			newElement.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, 0);
			newElement.GetComponent<RectTransform> ().anchorMin = new Vector2 (0.15f + 0.075f * i, 0.9f - 0.095f * j);
			newElement.GetComponent<RectTransform> ().anchorMax = new Vector2 (0.2f + 0.075f * i, 0.975f - 0.095f * j);
			newElement.GetComponent<RectTransform> ().offsetMin = Vector2.zero;
			newElement.GetComponent<RectTransform> ().offsetMax = Vector2.zero;
			newElement.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

			newElement.GetComponent<ElementHandler>().myEn = this;
			newElement.GetComponent<ElementHandler>().nameType = types[count];

			myElements.Add(newElement.GetComponent<ElementHandler>());
			count++;
		} while(count < numBlocs);

		this.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
	}

	public void CheckDiscovered(string type){
		if (!allTypes [type]) {
			myPopup.addInfo (type + " has been discovered !");
			allTypes [type] = true;
		}
	}

	public void MajIcons(){
		foreach (ElementHandler anElem in myElements)
			if (allTypes [anElem.nameType])
				anElem.SelfDisplay ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
