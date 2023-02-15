using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_attack_type_body : Enemy_Base
{
    /// <summary>
    /// 飛来物が当たった際の処理
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerEnter2D(Collider2D co)
    {
        if (co.gameObject.tag == "Player")
        {
            //Destroy(co.gameObject);
            GameOver();
        }
    }
}
