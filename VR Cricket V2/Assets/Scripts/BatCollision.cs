using UnityEngine;
public class BatCollision : MonoBehaviour
{
    public GameObject LCon;
    public float powerMultiplier = 1.5f;
    private Vector3 lastPosition;
    private Vector3 batVelocity;

    void Update()
    {
        // Track bat velocity manually
        //transform.position = LCon.transform.position;
        //transform.rotation = LCon.transform.rotation;
        batVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Add bat�s velocity to the ball
                ballRb.linearVelocity += batVelocity * powerMultiplier;
            }
        }
    }
}
