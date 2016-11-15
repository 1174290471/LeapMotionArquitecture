using UnityEngine;
using System.Collections;

public class Cut : MonoBehaviour {

    public Material capMaterial;
	public HandController hand_controller;
    public GameObject FigureCut;
    public GameObject FigureSet;
    public float min_Cut;
	public float max_Cut;
    public float min_Cut_Done;
    public float max_Cut_Done;


    private HandModel[] graphicsHands;
	private HandModel[] physicsHands;

    private float distance;
    private Transform[] FiguresCuts;
    private GameObject figure_cut;

    private float distance_object;
    private bool held = false;

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
		cutFigure ();
        cutDone();
	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.forward * 5.0f);
		Gizmos.DrawLine(transform.position + -transform.up * 0.5f, transform.position + -transform.up * 0.5f + transform.forward * 5.0f);

		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.DrawLine(transform.position,  transform.position + -transform.up * 0.5f);

	}
	

	void cutFigure(){

		graphicsHands = hand_controller.GetAllGraphicsHands ();
		physicsHands = hand_controller.GetAllPhysicsHands ();

		if(graphicsHands.Length >= 1){

			for (int i = 0; i < graphicsHands.Length; i++) {
				if (graphicsHands [i].IsRight()) {

                    transform.position = graphicsHands [i].fingers [index].GetTipPosition ();
                    transform.rotation = graphicsHands [i].GetPalmRotation();

					distance = DistanceThumbIndex (right);

					cutting (distance);
				}
			}
		}

	}
	void cutting(float distance){
        FiguresCuts = FigureCut.GetComponentsInChildren<Transform> ();

		if(FiguresCuts.Length >= 2 && distance == 0){

			figure_cut = FiguresCuts[FiguresCuts.Length - 1].gameObject;
			GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(figure_cut, transform.position, transform.right, capMaterial);
			pieces [0].transform.localScale = figure_cut.transform.localScale;

			if(!pieces[1].GetComponent<Rigidbody>())
				pieces[1].AddComponent<Rigidbody>();

			Destroy(pieces[1], 1);
		}

	}
    float DistanceThumbIndex(string type)
    {

        graphicsHands = hand_controller.GetAllGraphicsHands();
        physicsHands = hand_controller.GetAllPhysicsHands();

        if (graphicsHands.Length >= 1)
        {

            Vector3 ThumbPosition = new Vector3(0f, 0f, 0f);
            Vector3 IndexPosition = new Vector3(0f, 0f, 0f);

            for (int i = 0; i < graphicsHands.Length; i++)
            {
                if (graphicsHands[i].IsLeft() && type.Equals(left))
                {
                    ThumbPosition = graphicsHands[i].fingers[0].GetBoneCenter(3);
                    IndexPosition = graphicsHands[i].fingers[1].GetJointPosition(1);
                }
                else if (graphicsHands[i].IsRight() && type.Equals(right))
                {
                    ThumbPosition = graphicsHands[i].fingers[0].GetBoneCenter(3);
                    IndexPosition = graphicsHands[i].fingers[1].GetJointPosition(1);
                }
            }

            return normalized(ThumbPosition, IndexPosition, min_Cut, max_Cut);
        }
        return -1.0f;
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
    void cutDone()
    {
        GameObject HandControllerCamera = GameObject.Find("HandControllerCamera");
        float distanceCutDone = getFingerDistance(left,index,thumb,min_Cut_Done,max_Cut_Done);
        if (distanceCutDone == 0 && HandControllerCamera.transform.position.x == 12 && isHand(left))
        {
            HandControllerCamera.transform.position = new Vector3(0f, 0f, 0f);
            Transform[] FiguresCuts = FigureCut.GetComponentsInChildren<Transform>();
            GameObject f = FiguresCuts[1].gameObject;
            f.transform.position = FigureSet.transform.position;
            f.transform.SetParent(FigureSet.transform);
            FiguresCuts = null;
        }
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
