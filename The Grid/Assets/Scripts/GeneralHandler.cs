using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GeneralHandler : MonoBehaviour {

	public GameObject firstCase;
	public Text typeCase{ get; set;}
	public Text caraCase{ get; set;}
	public Dictionary<string,CaseHandler> myCases = new Dictionary<string, CaseHandler>(); 

	void Awake () {

		for (int i = 0; i < 12; i++) {
			for (int j = 0; j < 8; j++) {
				GameObject newCase = Instantiate (firstCase);
				newCase.transform.SetParent (GameObject.Find ("Background").transform);
				newCase.GetComponent<RectTransform> ().anchorMin = new Vector2 (i / 13f, j / 9f);
				newCase.GetComponent<RectTransform> ().anchorMax = new Vector2 ((i + 1) / 13f, (j + 1) / 9f);
				newCase.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				newCase.GetComponent<CaseHandler> ().hor = i;
				newCase.GetComponent<CaseHandler> ().ver = j;
				newCase.SetActive (true);
				myCases.Add (i + "" + j, newCase.GetComponent<CaseHandler>());
			}
		}
		for (int i = 0; i < 12; i++) {
			for (int j = 0; j < 8; j++) {
				for (int k = -1; k < 2; k ++)
					for (int m = -1; m < 2; m ++) {
						if ((k + m) % 2 != 0 && myCases.ContainsKey ((i + k).ToString () + (j + m).ToString ()))
							myCases [i + "" + j].neighbours.Add (k + "" + m, 
								myCases [(i + k).ToString () + (j + m).ToString ()]);
					}
			}
		}

		typeCase = GameObject.Find ("NameType").GetComponent<Text> ();
		caraCase = GameObject.Find ("NameCaracs").GetComponent<Text> ();

	}

	public void PrintInfos(string type, string descr){
		typeCase.text = type;
		caraCase.text = descr;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
