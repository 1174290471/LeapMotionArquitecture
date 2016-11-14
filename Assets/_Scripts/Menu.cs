using UnityEngine;
using System.Collections;
using Leap;

public class Menu : MonoBehaviour {
	
	public HandController hand_controller;
	public GameObject FigureCut;
	public float min_menu;
	public float max_menu;
	public float min_btn;
	public float max_btn;

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;

	private GameObject figure_bkg;
	private GameObject figure_btn;
	private bool created = false;

	private float distance;
	private float distance_figure_btn;
	private bool isVisible = false;

	private GameObject camera_main;

	private string left = "LEFT";
	private string right = "RIGHT";
	private int thumb = 0;
	private int index = 1;
	private int middle = 2;
	private int ring = 3;
	private int pinky = 4;


	void Start () {
		camera_main = GameObject.Find ("Main_Camera");
        distance = -5f;
	}

	void Update () {
		displayMenu ();
	}

	void displayMenu(){
		if (isHand (left)) {
			distance = getFingerDistance (left, middle, ring, min_menu, max_menu);
			if (distance == 1) {
				showFingersIcons (true);
				isVisible = true;
				createFigures ();
			} else if (distance == 0 && isVisible == true) {
				showFingersIcons (false);
				isVisible = false;
			}
		}
	}
	void showFingersIcons(bool show){
		fingerIcon ("cubeButton", show);
		fingerIcon ("sphereButton", show);
		fingerIcon ("cylinderButton", show);
	}
	void fingerIcon(string figure_name,bool show){
		figure_bkg = GameObject.Find (figure_name).transform.GetChild(0).gameObject;
		figure_btn = GameObject.Find (figure_name).transform.GetChild(1).gameObject;
		figure_bkg.GetComponent<Renderer> ().enabled = show;
		figure_btn.GetComponent<Renderer> ().enabled = show;
	}		
	float getFingerDistance(string type,int finger_1,int finger_2,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {

			Vector3 firstFinger = new Vector3(0f,0f,0f);
			Vector3 secondFinger = new Vector3(0f, 0f, 0f);

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && type.Equals(left)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
				}else if (graphicsHands [i].IsRight() && type.Equals(right)) {
					firstFinger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
					secondFinger = graphicsHands [i].fingers [finger_2].GetTipPosition ();
				}
			}

            return normalized(firstFinger, secondFinger, min, max);
		}
		return -5.0f;
	}
	float getObjectDistance(string btn, string type1, int finger_1,float min,float max){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 2) {

			figure_btn = findButton (btn);
			Vector3 finger = new Vector3(0f,0f,0f);
			Vector3 btnPosition = figure_btn.transform.position; ;

			for (int i = 0; i < graphicsHands.Length; i++) {
				if ((graphicsHands [i].IsRight() && type1.Equals(right))) {
					finger = graphicsHands [i].fingers [finger_1].GetTipPosition ();
				}
			}

            return normalized(finger, btnPosition, min, max);
		}
		return -5.0f;
	}	
	GameObject findButton(string btn){
		return GameObject.Find (btn).transform.GetChild(1).gameObject;
	}
    void createFigures()
    {
        createFigure("cubeButton");
        createFigure("sphereButton");
        createFigure("cylinderButton");
    }
    void createFigure(string btn){
        GameObject HandControllerCamera = GameObject.Find("HandControllerCamera");
        distance_figure_btn = getObjectDistance (btn, right, index, min_btn, max_btn);
		if (distance_figure_btn == 0 && created == false) {
            HandControllerCamera.transform.position = new Vector3(12f, 0f, 0f);
            created = true;
			instanceFigure (btn);
		} else if(distance_figure_btn == 1)  {
			created = false;
		}
	}
	void instanceFigure (string figure){

        switch (figure){
		case "cubeButton": 
			GameObject cube_m = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube_m.gameObject.GetComponent<Renderer> ().material.color = Color.blue;
            cube_m.transform.position = FigureCut.transform.position;
            cube_m.transform.SetParent (FigureCut.transform);

            

                break;
		case "sphereButton": 
			GameObject sphere_m = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphere_m.gameObject.GetComponent<Renderer> ().material.color = Color.green;
			sphere_m.transform.position = FigureCut.transform.position;
            sphere_m.transform.SetParent(FigureCut.transform);

            break;
		case "cylinderButton":
			GameObject cylinder_m = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			cylinder_m.gameObject.GetComponent<Renderer> ().material.color = Color.red;
			cylinder_m.transform.position = FigureCut.transform.position;
            cylinder_m.transform.SetParent(FigureCut.transform);

            break;
		}

        Transform[] FiguresCuts;
        FiguresCuts = FigureCut.GetComponentsInChildren<Transform>();
        GameObject f = FiguresCuts[1].gameObject;
        Destroy(f, 1);

    }
	
	bool isHand(string hand){
		
		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if (graphicsHands.Length >= 1) {
			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsLeft() && hand.Equals(left)) {
					return true;
				}else if(graphicsHands [i].IsRight() && hand.Equals(right)){
					return true;
				}
			}
		}

		return false;
	}  
    float normalized(Vector3 vector_1, Vector3 vectot_2, float min,float max){
        float distance = (vector_1 - vectot_2).magnitude;
        float normalizedDistance = (distance - min) / (max - min);
        normalizedDistance = Mathf.Clamp01(normalizedDistance);
        return normalizedDistance;
    }
}
