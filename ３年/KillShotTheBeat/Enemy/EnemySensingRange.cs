using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySensingRange : MonoBehaviour
{
    private SphereCollider _sphereCollider;
    
    [SerializeField]
    private int _range = 5;
    
    public List<GameObject> _nearEnemy = new List<GameObject>();
    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        //感知範囲の設定
        _sphereCollider.radius = _range;
    }

    /// <summary>
    /// 範囲内の敵に攻撃を指示
    /// </summary>
    public void EnemySensing()
    {
        foreach (var enemy in _nearEnemy)
        {
            enemy.transform.parent.GetComponent<Robot>().CullentSituation.Value = Situation.LockOn;
        }
    }

    /// <summary>
    /// 範囲内の敵のListから自信を削除
    /// </summary>
    public void EnemyRemoveSensing()
    {
        foreach (var enemy in _nearEnemy)
        {
            enemy.GetComponent<EnemySensingRange>()._nearEnemy.Remove(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyRange"))
        {
            _nearEnemy.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider  other)
    {
        if (other.gameObject.CompareTag("EnemyRange"))
        {
            _nearEnemy.Remove(other.gameObject);
        }
    }
}
