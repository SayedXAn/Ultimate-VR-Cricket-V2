using System.Collections;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool hitByBat = false;
    public bool hitGround = false;
    public bool hitBoundary = false;
    public float bounceMultiplier = 500f;

    //private Vector3 ballVelocity;

    public TrailRenderer trail;


    //public float speed = 0;
    //public float spinTorque = 5f;
   // public Vector3 spinAxis = Vector3.zero;

    //Vector3 lastPosition = Vector3.zero;
    private bool hasBounced = false;
    public float deviationValue = 13f;
    private Rigidbody rb;
    ScoreManager scoreManager;
    Bowler bowler;

    public AudioClip[] SFX;
    public AudioSource audioSource;

    public float groundCheckDistance = 0.01f;
    public LayerMask groundLayer;
    public TMP_Text debugText;

    public int ballType = 0; //0=pace ball 1=leg spin 2=offspin

    public GameObject hitMarkerPrefab;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GameObject.FindWithTag("audiosource").GetComponent<AudioSource>();
        scoreManager = GameObject.FindWithTag("logics").GetComponent<ScoreManager>();
        bowler = GameObject.FindWithTag("bowler").GetComponent<Bowler>();
        debugText = GameObject.FindWithTag("debugtext").GetComponent<TMP_Text>();
        //StartCoroutine(CountDownTimer());
    }
    //void FixedUpdate()
    //{
    //    //speed = (transform.position - lastPosition).magnitude;
    //    //lastPosition = transform.position;
    //}    

    void Update()
    {
        Debug.Log(rb.linearVelocity.magnitude);
        if (rb.linearVelocity.magnitude < 0.75f && !rb.isKinematic)
        {
            StartCoroutine(SmallDelay());
        }
        CheckGroundContactWithRaycast();

    }
    IEnumerator SmallDelay()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(1);        
        bowler.ReadyToBall();
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bat"))
        {
            hitByBat = true;
            PlaySFX(2);
            trail.emitting = true;
            debugText.text = "Bat hit";

            if (hitMarkerPrefab != null && collision.contactCount > 0)
            {
                ContactPoint contact = collision.contacts[0];
                Vector3 hitPosition = contact.point;
                Vector3 normal = contact.normal;
                Quaternion rotation = Quaternion.LookRotation(normal, Vector3.up);
                GameObject marker = Instantiate(hitMarkerPrefab, hitPosition, rotation);
                marker.transform.localScale = Vector3.one * 0.01f;
                marker.transform.SetParent(collision.transform);

                Destroy(marker, 5f);
            }
        }
        if(collision.gameObject.CompareTag("stump"))
        {
            //out
            PlaySFX(1);
            scoreManager.UpdateScore(0, 1);
            bowler.ReadyToBall();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("pitch") && !hitByBat && !hasBounced)
        {
            hasBounced = true;
            PlaySFX(0);
            // Apply bounce deviation (side movement)
            if(ballType == 1)
            {
                Vector3 velocity = rb.linearVelocity;
                float deviation = Random.Range(0.3f, deviationValue);
                velocity += transform.forward * deviation;
                rb.linearVelocity = velocity;
            }
            else if(ballType == 2)
            {
                Vector3 velocity = rb.linearVelocity;
                float deviation = Random.Range(-deviationValue, -0.3f);
                velocity += transform.forward * deviation;
                rb.linearVelocity = velocity;
            }
        }
        
    }

    public void PlaySFX(int index)
    {
        audioSource.clip = SFX[index];
        audioSource.Play();
    }


    //IEnumerator CountDownTimer()
    //{
    //    yield return new WaitForSeconds(20f);
    //    Destroy(gameObject);
    //}

    void CheckGroundContactWithRaycast()
    {
        if (hitByBat && !hitGround)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, groundCheckDistance, groundLayer))
            {
                hitGround = true;
                Debug.DrawRay(transform.position, GetComponent<Rigidbody>().angularVelocity.normalized * 0.5f, Color.red);
                Debug.Log("Ball has hit the ground.");
                debugText.text = debugText.text + "\nGround hit korse";
            }
        }
    }

}
