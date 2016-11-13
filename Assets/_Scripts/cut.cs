using UnityEngine;
using System.Collections;

public class cut : MonoBehaviour {
	public Material capMaterial;
	public HandController hand_controller;
	public float min_Cut;
	public float max_Cut;
	public GameObject figures_set;

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;


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
	private bool held = false;
	private GameObject figure_cut;

	void Start () {
	
	}

	void Update () {
		cutFigure ();
	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);

	}

	float DistanceThumbIndex(string type){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

			Vector3 ThumbPosition;
			Vector3 IndexPosition;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && type.Equals(left)) {
					ThumbPosition = graphicsHands [i].fingers [0].GetBoneCenter(3);
					IndexPosition = graphicsHands [i].fingers [1].GetJointPosition(1);
				}else if (graphicsHands [i].IsRight() && type.Equals(right)) {
					ThumbPosition = graphicsHands [i].fingers [0].GetBoneCenter(3);
					IndexPosition = graphicsHands [i].fingers [1].GetJointPosition(1);
				}
			}

			float distance = (ThumbPosition - IndexPosition).magnitude;
			float normalizedDistance = (distance - min_Cut) / (max_Cut - min_Cut);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			//Debug.Log ("Cut Distance: " + normalizedDistance);
			return normalizedDistance;
		}
		return -1.0f;
	}

	void holdFigure(){
		figures = figures_set.GetComponentsInChildren<Transform> ();
		Debug.Log ("Length: " + figures.Length);
		for (int i = 1; i < figures.Length; i++){
			distance_object = getObjectDistance(figures[i].gameObject,right,index,0,0);
			if (distance_object == 0 && held == false) {
				held = true;
				Debug.Log ("Holding....");	
				figure_cut = figures [i].gameObject;
			} else if(distance_object == 1){
				held = false;
				Debug.Log ("No Holding....");	
			}
		}
	}

	float getObjectDistance(GameObject figure, string type1, int finger_1,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

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
			float normalizedDistance = (distance - 0) / (0 - 0);
			normalizedDistance = Mathf.Clamp01 (normalizedDistance);
			return normalizedDistance;

		}
		return -5.0f;
	}

	void cutFigure(){
		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if(graphicsHands.Length >= 1){

			Vector3 planePosition = new Vector3(0,0,0);
			Quaternion planeRotation = new Quaternion(0,0,0,0);

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsRight()) {
					planePosition = graphicsHands [i].fingers [index].GetTipPosition ();
					planeRotation = graphicsHands [i].GetPalmRotation();

					transform.position = planePosition;
					transform.rotation = planeRotation;

					distance = DistanceThumbIndex (right);

					cutting (distance);


				}
			}

		}
	}

	void cutting(float distance){
		figures = this.figures_set.GetComponentsInChildren<Transform> ();

		if(figures.Length >= 2 && distance == 0){
			figure_cut = figures [figures.Length - 1].gameObject;

			GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(figure_cut, transform.position, transform.right, capMaterial);
			pieces [0].transform.localScale = figure_cut.transform.localScale;

			if(!pieces[1].GetComponent<Rigidbody>())
				pieces[1].AddComponent<Rigidbody>();

			Destroy(pieces[1], 1);
		}
	}

}
