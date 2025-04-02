using System.Collections.Generic;
using UnityEngine;

public class BallGoalDetection : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        // Call goal method based on ball name
        switch (collision.gameObject.name)
        {
            case "ball_1_goalTrigger":
                GameObject.Find("GameManager").GetComponent<GameManager>().Ball_1_Goal();
                break;
            case "ball_2_goalTrigger":
                GameObject.Find("GameManager").GetComponent<GameManager>().Ball_2_Goal();
                break;
        }
    }


}



