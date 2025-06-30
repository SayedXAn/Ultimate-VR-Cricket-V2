using UnityEngine;

public class Bowler : MonoBehaviour
{
    [Header("Bowling Setup")]
    public GameObject ballPrefab;
    public GameObject bowlingPoint;
    public Transform dropTarget;         // First bounce point
    public float timeToDrop = 1.2f;      // Time to first bounce
    public float deliveryInterval = 2f;
    public GameObject initPos;
    public string animationName = "Bowling_Anim";
    public float minDropTarget;
    public float maxDropTarget;

    [Header("Variation")]
    public float angleVariation = 5f;

    private void Start()
    {        
        InvokeRepeating("StartBowling", 1f, 8f);
    }

    public void Bowl()
    {
        dropTarget.transform.position = new Vector3(Random.Range(480f, 490f), dropTarget.transform.position.y, dropTarget.transform.position.z);
        GameObject ball = Instantiate(ballPrefab, bowlingPoint.transform.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Ball prefab must have a Rigidbody.");
            return;
        }

        // Direction variation
        Vector3 targetPos = dropTarget.position;
        float angleOffset = Random.Range(-angleVariation, angleVariation);
        Vector3 lateralOffset = Quaternion.Euler(0, angleOffset, 0) * (targetPos - transform.position);
        targetPos = transform.position + lateralOffset;

        // Compute velocity
        timeToDrop = Random.Range(0.75f, 1.3f);
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPos, timeToDrop);
        rb.linearVelocity = velocity;
    }

    Vector3 CalculateLaunchVelocity(Vector3 origin, Vector3 target, float time)
    {
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);

        float gravity = Mathf.Abs(Physics.gravity.y);
        float vx = toTargetXZ.magnitude / time;
        float vy = (toTarget.y + 0.5f * gravity * time * time) / time;

        Vector3 result = toTargetXZ.normalized * vx;
        result.y = vy;

        return result;
    }

    public void ResetBowler()
    {
        GetComponent<Animator>().applyRootMotion = false;
        transform.position = initPos.transform.position;
    }

    public void StartBowling()
    {
        GetComponent<Animator>().applyRootMotion = true;
        GetComponent<Animator>().Play(animationName, -1, 0f);

    }


    public void LegSpinBowl()
    {
        dropTarget.transform.position = new Vector3(Random.Range(minDropTarget, maxDropTarget), dropTarget.transform.position.y, dropTarget.transform.position.z);
        GameObject ball = Instantiate(ballPrefab, bowlingPoint.transform.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Ball prefab must have a Rigidbody.");
            return;
        }
        Vector3 targetPos = dropTarget.position;
        float angleOffset = Random.Range(0f, angleVariation);
        Vector3 lateralOffset = Quaternion.Euler(0, angleOffset, 0) * (targetPos - transform.position);
        targetPos = transform.position + lateralOffset;

        timeToDrop = Random.Range(1.1f, 1.3f);
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPos, timeToDrop);
        rb.linearVelocity = velocity;
    }

    public void OffSpinBowl()
    {
        dropTarget.transform.position = new Vector3(Random.Range(26f, 34f), dropTarget.transform.position.y, dropTarget.transform.position.z);
        GameObject ball = Instantiate(ballPrefab, bowlingPoint.transform.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Ball prefab must have a Rigidbody.");
            return;
        }

        // Direction variation
        Vector3 targetPos = dropTarget.position;
        float angleOffset = Random.Range(-angleVariation, angleVariation);
        Vector3 lateralOffset = Quaternion.Euler(0, angleOffset, 0) * (targetPos - transform.position);
        targetPos = transform.position + lateralOffset;

        // Compute velocity
        timeToDrop = Random.Range(1.1f, 1.3f);
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPos, timeToDrop);
        rb.linearVelocity = velocity;

    }

}