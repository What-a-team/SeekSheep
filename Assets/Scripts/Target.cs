using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 传统推箱子的目标
public class Target : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            collision.GetComponent<Rock>().RockOnTartget();
            TargetsManager.instance.ChangeDoorLight();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            collision.GetComponent<Rock>().RockStayOnTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            collision.GetComponent<Rock>().RockLeaveTarget();
            TargetsManager.instance.ChangeDoorLight();
        }
    }


}
