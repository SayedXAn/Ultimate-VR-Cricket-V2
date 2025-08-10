using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

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


    //private int totalBall = 0; //let's keep the TotalBallCount and TotalRunCount in only the scoreManager script
    private int overBall = 0;
    private int currentOverType = 0;
    private bool readyToBall = true;
    private bool inningsEnded = true;
    private int lastOverType = -1;

    [Header("Variation")]
    public float angleVariation = 5f;
    [SerializeField] Animator bowlerAnimator;
    public ScoreManager scoreManager;
    public ControllerAnimator conAnim;

    private void Start()
    {
        //InvokeRepeating("StartBowling", 1f, 8f);
        //StartNewOver();
        //ReadyToBall();
        InningsEnd();

    }

    private void Update()
    {
        if(!readyToBall && inningsEnded && conAnim.pressed)
        {
            inningsEnded = false;
            ReadyToBall();
        }
    }

    public void Bowl()
    {
        dropTarget.transform.position = new Vector3(Random.Range(480f, 490f), dropTarget.transform.position.y, dropTarget.transform.position.z);
        GameObject ball = Instantiate(ballPrefab, bowlingPoint.transform.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        //int tempBallType = ball.GetComponent<Ball>().ballType;
        ball.GetComponent<Ball>().ballType = currentOverType;
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
        
        if(currentOverType == 0)
        {
            timeToDrop = Random.Range(0.75f, 1.0f);
        }
        else
        {
            timeToDrop = Random.Range(1.1f, 1.3f);
        }
        
        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPos, timeToDrop);
        rb.linearVelocity = velocity;
        readyToBall = false;
        //totalBall++;
        overBall++;
        scoreManager.UpdateBall(/*totalBall*/);
        
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
        if(scoreManager.CheckIfInningsIsOver())
        {
            InningsEnd();
            return;
        }
        bowlerAnimator.enabled = true;
        if (overBall == 6)
        {
            //OverEnd
            StartNewOver();
            //InningsEnd();
        }
        else
        {
            GetComponent<Animator>().applyRootMotion = true;
            GetComponent<Animator>().Play(animationName, -1, 0f);
        }
    }

    public void InningsEnd()
    {
        string tempStr = "You scored: " + scoreManager.GetTotalRun().ToString() + "\nPress the 'Trigger button to play again";
        //scoreManager.ShowNotification(tempStr);
        scoreManager.NotificationOnInningsEnd(tempStr);
        scoreManager.ResetTotalBall();
        scoreManager.ResetTotalRun();

    }

    public void StartNewOver()
    {
        overBall = 0;
        currentOverType = Random.Range(0, 3);
        while(currentOverType == lastOverType)
        {
            currentOverType = Random.Range(0, 3);
        }
        lastOverType = currentOverType;
        string tempText = "---";
        if(currentOverType == 0)
        {
            tempText = "Fast Ball";
        }
        else if(currentOverType == 1)
        {
            tempText = "Leg Spin";
        }
        else if (currentOverType == 2)
        {
            tempText = "Off Spin";
        }
        scoreManager.ShowNotification(tempText);
    }

    public void ReadyToBall()
    {
        if(!readyToBall)
        {
            readyToBall = true;
            overBall = 0;
            StartNewOver();
            StartCoroutine(DelayBeforeBowling());
        }

    }

    IEnumerator DelayBeforeBowling()
    {
        StopCoroutine(DelayBeforeBowling());
        yield return new WaitForSeconds(2);
        StartBowling();
    }
}