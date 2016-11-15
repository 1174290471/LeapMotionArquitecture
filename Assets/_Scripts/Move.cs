using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public HandController hand_controller;
    public GameObject Figures_Set;
    public float min_hold;
	public float max_hold;
	public float min_object;
	public float max_object;

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;

    private float distance;
    private float distance_object;
    private Transform[] figures;
    private bool find = false;
    private GameObject figure_move = null;
    private Color figure_move_color = Color.yellow;

    private string left = "LEFT";
	private string right = "RIGHT";
	private int thumb = 0;
	private int index = 1;
	private int middle = 2;
	private int ring = 3;
	private int pinky = 4;



	void Start () {

	}

	void Update () {
        getFigureNear();
	}

    void getFigureNear()
    {
        figures = Figures_Set.GetComponentsInChildren<Transform>();
        for (int i = 1; i < figures.Length; i++)
        {

            distance_object = getObjectDistance(figures[i].gameObject, right, index, min_object, max_object);
            if (distance_object == 0 && find == false)
            {
                Debug.Log("Find: " + figures[i].gameObject.name);
                find = true;
                figure_move = figures[i].gameObject;
                figure_move_color = figure_move.GetComponent<Renderer>().material.color;
                figure_move.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                Debug.Log("Changed Color: " + figures[i].gameObject.name);
            }
            else if (distance_object == 1 && figure_move == figures[i].gameObject)
            {
                Debug.Log("No Find Nothing");
                find = false;
                
                if (figure_move != null)
                {
                    figure_move.gameObject.GetComponent<Renderer>().material.color = figure_move_color;
                }

                figure_move = null;
            }
        }
    }
    void colorFigureMove()
    {

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
}

