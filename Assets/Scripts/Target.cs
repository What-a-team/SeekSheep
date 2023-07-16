using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ͳ�����ӵ�Ŀ��
public class Target : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            collision.GetComponent<Rock>().RockOnTartget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "rock")
        {
            collision.GetComponent<Rock>().RockLeaveTarget();
        }
    }


}
