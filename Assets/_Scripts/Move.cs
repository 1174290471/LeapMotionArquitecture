using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public HandController hand_controller;
	public float min_hold;
	public float max_hold;
	public float min_object;
	public float max_object;
	public GameObject figures_set;

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;
	private HandModel hand_l;
	private HandModel hand_r;

	private string left = "LEFT";
	private string right = "RIGHT";
	private int thumb = 0;
	private int index = 1;
	private int middle = 2;
	private int ring = 3;
	private int pinky = 4;

	private float distance;
	private float distance_object;
	private Transform[] figures;
	private bool hold = false;

	void Start () {

	}

	void Update () {
		distance = getFingerDistance (right, thumb, index, min_hold, max_hold);
		if (distance == 0  && hold == false) {
			Debug.Log ("Join Distance: " + distance);
			holdFigure ();	
			hold = true;
		} else if (distance == 1) {
			Debug.Log ("Join Distance Exit: " + distance);
			hold = false;
		}
	}

	void holdFigure(){
		figures = figures_set.GetComponentsInChildren<Transform> ();
		foreach(Transform figure in figures){
			Debug.Log ("Figures");		
			distance_object = getObjectDistance(figure.gameObject,right,index,min_object,max_object);
			if (distance == 0) {
				Debug.Log ("Holding....");			
			} else {
				Debug.Log ("No Holding....");	
			}
		}
	}

	float getObjectDistance(GameObject figure, string type1, int finger_1,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 2) {

			Vector3 finger;
			Vector3 figurePosition = figure.transform.position;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if ((graphicsHands [i].IsRight() && type1.Equals(right))) {
					finger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
				}else if((graphicsHands [i].IsLeft() && type1.Equals(left))){
					finger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
				}
			}
				
			float distance = (finger - figurePosition).magnitude;
			float normalizedDistance = (distance - min) / (max - min);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			return normalizedDistance;

		}
		return -5.0f;
	}

	float getFingerDistance(string type,int finger_1,int finger_2,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

			Vector3 firstFinger;
			Vector3 secondFinger;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && type.Equals(left)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
					hand_l = graphicsHands [i];
				}else if (graphicsHands [i].IsRight() && type.Equals(right)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
					hand_r = graphicsHands [i];
				}
			}

			float distance = (firstFinger - secondFinger).magnitude;
			float normalizedDistance = (distance - min) / (max - min);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			return normalizedDistance;

		}
		return -5.0f;
	}
}

