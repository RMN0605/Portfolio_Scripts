using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField,Header("エリア格納")]
    private List<GameObject> _areaObj     = new List<GameObject>();
    [SerializeField,Header("全体の出現した敵")]
    public List<GameObject> AreaEnemyList = new List<GameObject>();
    
    private void Awake()
    {
        SetInstance();
    }
    
    //＝＝＝＝＝インスタンス＝＝＝＝＝//
    public static AreaManager Instance;
    private void SetInstance()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }
}
