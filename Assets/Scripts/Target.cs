using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 传统推箱子的目标
public class Target : MonoBehaviour
{
    public TargetsManager targetsManager;

    private void Start()
    {
        targetsManager = this.GetComponentInParent<TargetsManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            collision.GetComponent<Rock>().RockOnTartget();
            targetsManager.ChangeDoorLight();
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
            targetsManager.ChangeDoorLight();
        }
    }


}
