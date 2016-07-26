﻿using UnityEngine;
using System.Collections;

public class ChangeCase : StateMachineBehaviour {

	public string nameType;
	public int grad_heat;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<CaseHandler> ().SetType(nameType);
		animator.gameObject.GetComponent<CaseHandler> ().caracs ["Grad_Heat"] = grad_heat;

		switch (nameType) {
		case "Water":
			animator.gameObject.AddComponent<Water_Script> ();
			break;
		case "Stone":
			animator.gameObject.AddComponent<Stone_Script> ();
			break;
		default:
			break;
		}

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		switch (nameType) {
		case "Water":
			Destroy(animator.gameObject.GetComponent<Water_Script> ());
			break;
		case "Stone":
			Destroy(animator.gameObject.GetComponent<Stone_Script> ());
			break;
		default:
			break;
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
