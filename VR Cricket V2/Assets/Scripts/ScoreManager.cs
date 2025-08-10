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
    public void UpdateScore(int run, int wick)
    {
        totalRun += run;
        wicket += wick;
        if(run == 6)
        {
            ShowNotification("6");
        }
        else if(run == 4)
        {
            ShowNotification("4");
        }
        else if (run == 0 && wick == 1)
        {
            ShowNotification("Out!");
        }
        scoreBoardText.text = totalRun.ToString() + "-" + wicket.ToString() + "\n" + (totalBall/6) +"."+ (totalBall%6);
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

    public void NotificationOnInningsEnd(string str)
    {
        notiText.text = str;
        notiText.gameObject.SetActive(true);
        notiText.GetComponent<Animator>().enabled = false;
    }
}
