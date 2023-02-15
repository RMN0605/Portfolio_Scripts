using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class SightChecker : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    private WaitForSeconds         _waitTime = new WaitForSeconds(0.2f); //視野角の計算をする秒数
    private ReactiveProperty<bool> isVisible = new ReactiveProperty<bool>();　//ターゲットが視野角内に入っているか

    [SerializeField] 
    private Transform _self;       //自身の目の位置

    private Transform _target;     //Trueにする条件（ターゲット）
    [SerializeField] 
    private float     _sightAngle; //視野角
    [SerializeField] 
    private float     _maxDistance; //視界の最大距離


    private void Awake()
    {
        isVisible.Skip(1).Subscribe(x => LockOnPlayer()).AddTo(this);
        _target = GameObject.Find("Player").gameObject.transform;
    }

    private void Start()
    {
        StartCoroutine(IsVisible());
    }

    private void OnEnable()
    {
        StartCoroutine(IsVisible());
    }
    
    private void Update()
    {
        LockOnPlayer();
    }

    /// <summary>
    /// ターゲットが見えているかどうか
    /// </summary>
    public IEnumerator IsVisible()
    {
        while (_enemy.CullentSituation.Value != Situation.Die)
        {
            // 自身の位置
            var selfPos = _self.position;
            // ターゲットの位置
            var targetPos = _target.position;

            // 自身の向き（正規化されたベクトル）
            var selfDir = _self.forward;
        
            // ターゲットまでの向きと距離計算
            var targetDir = targetPos - selfPos;
            var targetDistance = targetDir.magnitude;

            // cos(θ/2)を計算
            var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

            // 自身とターゲットへの向きの内積計算
            // ターゲットへの向きベクトルを正規化する必要があることに注意
            var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

            // 視界判定
            if (innerProduct > cosHalf && targetDistance < _maxDistance)
                isVisible.Value = true;
            else isVisible.Value = false;

            yield return _waitTime;
        }
        
    }

    /// <summary>
    /// プレイヤーを見る
    /// </summary>
    private void LockOnPlayer()
    {
        //範囲内に入ったらtargetを入れる
        if(isVisible.Value) _enemy.IsLockOn.Value = true;
        //範囲外に出たらtargetをNullにする
        else  _enemy.IsLockOn.Value = false;
    }
}