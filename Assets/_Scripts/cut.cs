using UnityEngine;
using System.Collections;

public class Cut : MonoBehaviour {

    public Material capMaterial;
	public HandController hand_controller;
    public GameObject figures_set;
    public float min_Cut;
	public float max_Cut;
	

	private HandModel[] graphicsHands;
	private HandModel[] physicsHands;

    private float distance;
    private Transform[] figures;
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
		figures = figures_set.GetComponentsInChildren<Transform> ();

		if(figures.Length >= 2 && distance == 0){

			figure_cut = figures [figures.Length - 1].gameObject;
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


    void holdFigure()
    {
        figures = figures_set.GetComponentsInChildren<Transform>();
        //Debug.Log("Length: " + figures.Length);
        for (int i = 1; i < figures.Length; i++)
        {
            distance_object = getObjectDistance(figures[i].gameObject, right, index, 0, 0);
            if (distance_object == 0 && held == false)
            {
                held = true;
                //Debug.Log("Holding....");
                figure_cut = figures[i].gameObject;
            }
            else if (distance_object == 1)
            {
                held = false;
                //Debug.Log("No Holding....");
            }
        }
    }
    float getObjectDistance(GameObject figure, string type1, int finger_1, float min, float max)
    {

        graphicsHands = hand_controller.GetAllGraphicsHands();
        physicsHands = hand_controller.GetAllPhysicsHands();

        if (graphicsHands.Length >= 1)
        {

            Vector3 finger = new Vector3(0f, 0f, 0f);
            Vector3 figurePosition = figure.transform.position;

            for (int i = 0; i < graphicsHands.Length; i++)
            {
                if ((graphicsHands[i].IsRight() && type1.Equals(right)))
                {
                    finger = graphicsHands[i].fingers[finger_1].GetTipPosition();
                }
                else if ((graphicsHands[i].IsLeft() && type1.Equals(left)))
                {
                    finger = graphicsHands[i].fingers[finger_1].GetTipPosition();
                }
            }

            float distance = (finger - figurePosition).magnitude;
            float normalizedDistance = (distance - 0) / (0 - 0);
            normalizedDistance = Mathf.Clamp01(normalizedDistance);
            return normalizedDistance;

        }
        return -5.0f;
    }

}
