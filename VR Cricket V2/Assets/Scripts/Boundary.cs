using UnityEngine;

public class Boundary : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    ScoreManager scoreManager;
    void Start()
    {
        scoreManager = GameObject.FindWithTag("logics").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ball"))
        {            
            if (other.gameObject.GetComponent<Ball>().hitByBat && !other.gameObject.GetComponent<Ball>().hitBoundary)
            {
                other.gameObject.GetComponent<Ball>().hitBoundary = true;
                if(other.gameObject.GetComponent<Ball>().hitGround )
                {
                    //4
                    scoreManager.UpdateScore(4, 0);
                }
                else
                {
                    //6
                    scoreManager.UpdateScore(6, 0);
                }
            }

        }

    }
}
