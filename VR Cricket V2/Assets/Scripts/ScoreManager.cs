using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int totalRun = 0;
    private int wicket = 0;
    private int totalBall = 0;
    [SerializeField] int totalBallToPlay = 6; //THis is where we set the over limit
    public TMP_Text scoreBoardText;
    public TMP_Text notiText;
    public AudioSource crowdAudio;

    public GameObject bells;
    public GameObject groundedBells;
    //private void Update()
    //{
    //    Debug.Log("notiiii  "+ notiText.transform.position);
    //}
    public void UpdateScore(int run, int wick)
    {
        totalRun += run;
        wicket += wick;
        
        if (run == 0 && wick == 1)
        {
            ShowNotification("Out!");
            StartCoroutine(BellsGrounded());
        }
        else
        {
            ShowNotification(run.ToString());
        }
        scoreBoardText.text = totalRun.ToString() + "-" + wicket.ToString() + "\n" + (totalBall/6) +"."+ (totalBall%6);
    }
    public void ResetScoreBoard()
    {
        totalRun = 0;
        wicket = 0;
        scoreBoardText.text = "0-0\n0.0";
    }

    public void UpdateBall(/*int ttlBall*/)
    {
        //totalBall = ttlBall;
        totalBall++;
        scoreBoardText.text = totalRun.ToString() + "-" + wicket.ToString() + "\n" + (totalBall / 6) + "." + (totalBall % 6);
    }

    public void ShowNotification(string str)
    {
        notiText.text = str;
        notiText.fontSize = 12f;
        StartCoroutine(Notification());
    }

    public int GetTotalRun()
    {
        return totalRun;
    }

    public void ResetTotalRun()
    {
        totalRun = 0;
    }

    public int GetTotalBall()
    {
        return totalBall;
    }

    public void ResetTotalBall()
    {
        totalBall = 0;
    }

    public int GetOverLimit()
    {
        return totalBallToPlay;
    }

    public bool CheckIfInningsIsOver()
    {
        return totalBall == totalBallToPlay;
    }


    IEnumerator Notification()
    {
        notiText.gameObject.SetActive(true);
        notiText.GetComponent<Animator>().enabled = true;
        crowdAudio.volume = 0.7f;
        yield return new WaitForSeconds(3);
        notiText.gameObject.SetActive(false);
        crowdAudio.volume = 0.3f;
    }

    public void NotificationOnInningsEnd(string str, float fontSize)
    {
        StopAllCoroutines();
        notiText.text = str;
        notiText.fontSize = fontSize;
        notiText.gameObject.SetActive(true);        
        notiText.GetComponent<Animator>().enabled = false;
        notiText.transform.position = new Vector3(627.80f, 28.96f, 500.47f);


    }
    IEnumerator BellsGrounded()
    {
        bells.SetActive(false);
        groundedBells.SetActive(true);
        yield return new WaitForSeconds(2f);
        bells.SetActive(true);
        groundedBells.SetActive(false);
    }
}
