using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int totalRun = 0;
    private int wicket = 0;
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
        scoreBoardText.text = totalRun.ToString() + "-" + wicket.ToString();
    }

    public void ShowNotification(string str)
    {
        notiText.text = str;
        StartCoroutine(Notification());
    }


    IEnumerator Notification()
    {
        notiText.gameObject.SetActive(true);
        crowdAudio.volume = 0.7f;
        yield return new WaitForSeconds(3);
        notiText.gameObject.SetActive(false);
        crowdAudio.volume = 0.3f;
    }
}
