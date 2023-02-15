using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    public List<GameObject> Enemys = new List<GameObject>();
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy")) Enemys.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy")) Enemys.Remove(other.gameObject);
    }
}
