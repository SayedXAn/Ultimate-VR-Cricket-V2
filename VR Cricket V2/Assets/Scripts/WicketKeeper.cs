using UnityEngine;

public class WicketKeeper : MonoBehaviour
{
    public Bowler bowler;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ball") & !other.gameObject.GetComponent<Ball>().hitByBat)
        {
            Destroy(other.gameObject);
            bowler.ReadyToBall();
        }
    }
}
