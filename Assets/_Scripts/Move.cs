using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public HandController hand_controller;
    public GameObject Figures_Set;
    public GameObject Figures_Cut;
    public float min_hold;
	public float max_hold;
	public float min_object;
	public float max_object;

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;

    private float distance_object;
    private Transform[] figures;
    private bool find = false;
    private bool hold = false;
    private GameObject figure_move = null;
    private Color figure_move_color = Color.yellow;
    private GameObject handControllerCamera;

    private string left = "LEFT";
	private string right = "RIGHT";
	private int thumb = 0;
	private int index = 1;
	private int middle = 2;
	private int ring = 3;
	private int pinky = 4;



	void Start () {
        handControllerCamera = GameObject.Find("HandControllerCamera");
    }

	void Update () {
        if (handControllerCamera.transform.position.x == 0)
        {
            getFigureNear(Figures_Set);
            moveFigure(Figures_Set);
        }else if (handControllerCamera.transform.position.x == 12)
        {
            getFigureNear(Figures_Cut);
            moveFigure(Figures_Cut);
        }
    }

    void getFigureNear(GameObject Set)
    {
        figures = Set.GetComponentsInChildren<Transform>();
        for (int i = 1; i < figures.Length; i++)
        {

            distance_object = getObjectDistance(figures[i].gameObject, right, middle, min_object, max_object);
            if (distance_object == 0 && find == false)
            {
                //Debug.Log("Find: " + figures[i].gameObject.name);
                find = true;
                figure_move = figures[i].gameObject;
                figure_move_color = figure_move.GetComponent<Renderer>().material.color;
                figure_move.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                
            }
            else if (distance_object == 1 && figure_move == figures[i].gameObject)
            {
               
                //Debug.Log("No Find Nothing");
                find = false;

                if (figure_move != null)
                {
                    figure_move.gameObject.GetComponent<Renderer>().material.color = figure_move_color;
                }

                figure_move = null;
                   
            }
        }
    }
    void moveFigure(GameObject Set)
    {
        if(figure_move != null && isHand(right))
        {
            float distance_to_move = getFingerDistance(right, middle, ring, min_hold, max_hold);
            if(distance_to_move == 0)
            {
                GameObject figure_moving = GameObject.Find("Figure_Moving");
                figure_move.transform.SetParent(figure_moving.transform);
            }
            else if(distance_to_move == 1)
            {
                figure_move.transform.SetParent(Set.transform);
            }
            
        }
    }
    float normalized(Vector3 vector_1, Vector3 vectot_2, float min, float max)
    {
        float distance = (vector_1 - vectot_2).magnitude;
        float normalizedDistance = (distance - min) / (max - min);
        normalizedDistance = Mathf.Clamp01(normalizedDistance);
        return normalizedDistance;
    }
    float getFingerDistance(string type, int finger_1, int finger_2, float min, float max)
    {

        graphicsHands = hand_controller.GetAllGraphicsHands();
        physicsHands = hand_controller.GetAllPhysicsHands();

        if (graphicsHands.Length >= 1)
        {

            Vector3 firstFinger = new Vector3(0f, 0f, 0f);
            Vector3 secondFinger = new Vector3(0f, 0f, 0f);

            for (int i = 0; i < graphicsHands.Length; i++)
            {
                if (graphicsHands[i].IsLeft() && type.Equals(left))
                {
                    firstFinger = graphicsHands[i].fingers[finger_1].GetTipPosition();
                    secondFinger = graphicsHands[i].fingers[finger_2].GetTipPosition();
                }
                else if (graphicsHands[i].IsRight() && type.Equals(right))
                {
                    firstFinger = graphicsHands[i].fingers[finger_1].GetTipPosition();
                    secondFinger = graphicsHands[i].fingers[finger_2].GetTipPosition();
                }
            }

            return normalized(firstFinger, secondFinger, min, max);
        }
        return -5.0f;
    }
    float getObjectDistance(GameObject figure, string type1, int finger_1, float min, float max)
    {

        graphicsHands = hand_controller.GetAllGraphicsHands();
        physicsHands = hand_controller.GetAllPhysicsHands();

        if (graphicsHands.Length >= 1)
        {

            Vector3 fingerPosition = new Vector3();
            Vector3 figurePosition = figure.transform.position;

            for (int i = 0; i < graphicsHands.Length; i++)
            {
                if ((graphicsHands[i].IsRight() && type1.Equals(right)))
                {
                    fingerPosition = graphicsHands[i].fingers[finger_1].GetTipPosition();
                }
                else if ((graphicsHands[i].IsLeft() && type1.Equals(left)))
                {
                    fingerPosition = graphicsHands[i].fingers[finger_1].GetTipPosition();
                }
            }

            return normalized(fingerPosition, figurePosition, min_object, max_object);

        }
        return -5.0f;
    }
    bool isHand(string hand)
    {

        graphicsHands = hand_controller.GetAllGraphicsHands();
        physicsHands = hand_controller.GetAllPhysicsHands();

        if (graphicsHands.Length >= 1)
        {
            for (int i = 0; i < graphicsHands.Length; i++)
            {
                if (graphicsHands[i].IsLeft() && hand.Equals(left))
                {
                    return true;
                }
                else if (graphicsHands[i].IsRight() && hand.Equals(right))
                {
                    return true;
                }
            }
        }

        return false;
    }
}

