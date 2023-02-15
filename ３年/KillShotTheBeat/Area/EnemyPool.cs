using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class PoolClass
{
    public bool       IsUsedPrefab; //このプレハブが使われているか
    public GameObject Prefab;
}

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance = null;

    [Header("<プールの設定>"),SerializeField,Header("敵のストレージ")]
    private GameObject _storage;
    [SerializeField,Header("このポイントから出現する敵")]
    public List<GameObject> AreaSummonEnemys = new List<GameObject>();
    [Header("プールリスト"),SerializeField]
    public List<PoolClass> EnemyPoolList 　= new List<PoolClass>();
    


    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Start()
    {
        foreach(Transform childTransform in _storage.transform)
        {
            PoolClass newClass    = new PoolClass();
            newClass.Prefab       = childTransform.gameObject;
            EnemyPoolList.Add(newClass);
        }
    }

    /// <summary>
    /// 使われていない敵をステージに配置する関数
    /// </summary>
    public GameObject OrderEnemy(Vector3 summonArea)
    {
        bool       isOrderedFound = false;
        GameObject summonEnemy    = null;
        
        //プレハブ探しの旅スタート
        foreach (var poolClass in EnemyPoolList.Select((value, index) => new { value, index }))
        {
            //使われていないプレハブを見つけ方はこちらへ
            if (!poolClass.value.IsUsedPrefab)
            {
                summonEnemy    = poolClass.value.Prefab;
                poolClass.value.IsUsedPrefab = true;
                isOrderedFound = true;
                summonEnemy.GetComponent<Enemy>().MyPoolNum = poolClass.index;
                break;
            }
        }

        //プレハブを見つけられなかった方はこちらへ
        if (!isOrderedFound)
        {
            summonEnemy = AreaSummonEnemys.Count == 0
            ? Instantiate(AreaSummonEnemys[0].gameObject, summonArea, Quaternion.identity)
            : Instantiate(AreaSummonEnemys[UnityEngine.Random.Range(0, AreaSummonEnemys.Count - 1)].gameObject, summonArea,Quaternion.identity);
            
            //新たにプールに追加
            PoolClass newClass    = new PoolClass();
            newClass.Prefab       = summonEnemy;
            newClass.IsUsedPrefab = true;
            EnemyPoolList.Add(newClass);
            summonEnemy.GetComponent<Enemy>().MyPoolNum = EnemyPoolList.Count - 1;
            //ストレージに格納
            summonEnemy.transform.parent = _storage.transform;
        }
        
        //生成したオブジェクトが一方向向いていると不自然なためランダムに回転させる
        summonEnemy.transform.Rotate(0,Random.Range(0,360),0);
        summonEnemy.transform.position = summonArea;
        AreaManager.Instance.AreaEnemyList.Add(summonEnemy);

        return summonEnemy;
    }


}
