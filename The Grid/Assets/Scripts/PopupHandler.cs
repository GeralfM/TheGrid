using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopupHandler : MonoBehaviour {

	public List<string> infos = new List<string> ();

	public Text myText { get; set;}
	public RectTransform myTr { get; set;}

	public float goal { get; set;}
	public bool increase { get; set;}
	public bool move { get; set;}

	// Use this for initialization
	void Start () {
		myText = gameObject.GetComponentInChildren<Text> ();
		myTr = gameObject.GetComponent<RectTransform> ();
	}

	public void addInfo(string info){
		infos.Add (info);
		gameObject.transform.SetAsLastSibling ();
		StartCoroutine (delayAndDelete ());
	}

	public IEnumerator delayAndDelete(){
		MajText ();
		yield return new WaitForSeconds (5f);
		infos.RemoveAt (0);
		MajText ();
	}

	public void MajText(){
		myText.text = "";
		foreach(string info in infos)
			myText.text+=info+"\n";
		
		goal = infos.Count*3f/100f;
		move = true;
			
	}

	// Update is called once per frame
	void Update () {
		if (move) {
			if (goal > myTr.anchorMax.y) {
				myTr.anchorMax += new Vector2 (0, Time.deltaTime/5f);
				move = (goal > myTr.anchorMax.y);
			} else {
				myTr.anchorMax -= new Vector2 (0, Time.deltaTime/5f);
				move = (goal < myTr.anchorMax.y);
			}
			myTr.offsetMax = Vector2.zero;
		}
	}
}
