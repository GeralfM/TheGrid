using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeCase : StateMachineBehaviour {

	public string nameType;
	public int grad_heat;
	public bool flammable;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.gameObject.GetComponent<CaseHandler> ().SetType(nameType);
		animator.gameObject.GetComponent<CaseHandler> ().caracs ["Grad_Heat"] = grad_heat;
		animator.gameObject.GetComponent<CaseHandler> ().specialProperties ["Flammable"] = flammable;

		if (!new List<string>{ "Void" }.Contains (nameType)) {
			animator.gameObject.AddComponent (System.Type.GetType (nameType + "_Script"));
			animator.gameObject.GetComponent<CaseHandler> ().specialProperties ["Solid"] = true;
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		if (!new List<string>{ "Void" }.Contains (nameType))
			Destroy (animator.gameObject.GetComponent (System.Type.GetType (nameType + "_Script")));
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
