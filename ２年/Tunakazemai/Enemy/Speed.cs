using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public float speed;
    private void Awake()
    {
        if (!Enemy_Manager._isTraining || SettingInformation._enemySpeed2 == 0)
            speed = Random.Range(2, 6);
        else
            speed = SettingInformation._enemySpeed2;
    }
}
