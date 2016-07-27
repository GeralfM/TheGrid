using UnityEngine;
using System.Collections;

public class Organism_Script : MonoBehaviour {

	public GroundHandler myGround { get; set;}

	public int numOrgs { get; set;}

	// Use this for initialization
	void Start () {
		myGround = gameObject.GetComponent<GroundHandler> ();
		numOrgs = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
