using UnityEngine;

public class Pitch : MonoBehaviour
{
    [SerializeField] PhysicsMaterial pitchMat;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ball")
        {
            if(collision.gameObject.GetComponent<Ball>().hitByBat)
            {
                GetComponent<BoxCollider>().material = null;
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            if (collision.gameObject.GetComponent<Ball>().hitByBat)
            {
                GetComponent<BoxCollider>().material = pitchMat;
            }
        }
    }
}
