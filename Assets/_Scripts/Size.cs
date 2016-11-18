using UnityEngine;
using System.Collections;

public class Size : MonoBehaviour {

    public HandController hand_controller;
    public float min_size;
    public float max_size;
    public GameObject Figures_Cut;

    private HandModel[] graphicsHands;
    private HandModel[] physicsHands;
    private Transform[] figures;
    private float distance;
    private float current_x;
    private bool size_actived = false;
    private GameObject handControllerCamera;

    private string left = "LEFT";
    private string right = "RIGHT";
    private int thumb = 0;
    private int index = 1;
    private int middle = 2;
    private int ring = 3;
    private int pinky = 4;
    private int cut_table = 12;
    private int desing_table = 0;

    void Start () {
        handControllerCamera = GameObject.Find("HandControllerCamera");
    }
	
	void Update () {
        if(handControllerCamera.transform.position.x == cut_table)
        {
            changedSize();
        }   
	 
	}

    void changedSize()
    {
        if (isHand(right))
        {
            distance = getFingerDistance(right, thumb, index, min_size, max_size);
            if (distance == 0 && size_actived == false)
            {
                current_x = getCurrentPosition(right);
                //Debug.Log("Distance: 0");
                size_actived = true;
            }
            else if (distance == 1 && size_actived == true)
            {
                if (current_x < getCurrentPosition(right))
                {
                    figures = Figures_Cut.GetComponentsInChildren<Transform>();
                    if (figures.Length >= 2)
                    {
                        figures[1].gameObject.transform.localScale += new Vector3(-0.2f, -0.2f, -0.2f);
                    }
                    //Debug.Log("Small");
                }
                else
                {
                    figures = Figures_Cut.GetComponentsInChildren<Transform>();
                    if (figures.Length >= 2)
                    {
                        figures[1].gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                    }
                    //Debug.Log("Big");
                }
                //Debug.Log("Distance: 1");
                size_actived = false;
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
    float getCurrentPosition(string hand)
    {
        graphicsHands = hand_controller.GetAllGraphicsHands();
        physicsHands = hand_controller.GetAllPhysicsHands();

        if (graphicsHands.Length >= 1)
        {
            for (int i = 0; i < graphicsHands.Length; i++)
            {
                if (graphicsHands[i].IsLeft() && hand.Equals(left))
                {
                    return graphicsHands[i].fingers[index].GetTipPosition().x;
                }
                else if (graphicsHands[i].IsRight() && hand.Equals(right))
                {
                    return graphicsHands[i].fingers[index].GetTipPosition().x;
                }
            }
        }

        return -5f;
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
