using System;
using UniRx;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimEvent))]
[RequireComponent(typeof(EnemyAi))]
[RequireComponent(typeof(SightChecker))]
public class Enemy : EnemyStatus
{
    #region 参照
    
    [HideInInspector]
    public  NavMeshAgent     NavMeshAgent;
    [HideInInspector]
    public  Animator         Animator;
    [HideInInspector]
    public  EnemyAi          Ai;
    public  Area             MyArea;     //どのばしょから生成されたか格納しておく
    public  SearchRayMachine RayMachine;
    [HideInInspector]
    public  GameObject       Player;     //プレイヤーが格納される
    [SerializeField]
    private GameObject      _rayObject;
    [HideInInspector]
    public int              MyPoolNum;
    public EnemySensingRange SensingRange;
    
    #endregion
    
    public          ReactiveProperty<Situation> CullentSituation  = new ReactiveProperty<Situation>();
    public readonly int                         CullentAnimStatus = Animator.StringToHash("NowStatus");
    public          ReactiveProperty<bool>      IsLockOn          = new ReactiveProperty<bool>();
    public          bool                        IsOutOfArea; //エリア外に出たかどうか
    public          UnityEvent                  OnHurt            = new UnityEvent();

    [Header("攻撃対象")]
    public          GameObject                  TargetInRange;  //攻撃範囲内に入ったプレイヤーが格納される
    [HideInInspector]
    public          Vector3                     MyStartPos;//生成された最初の場所　/ 元の位置に戻る際に使う


    [Header("/////下記よりプランナー設定よろ/////")]
    [Space(10)]
    [SerializeField,Header("射程範囲の長さ")]
    private float _rayDistance = 10;
    [SerializeField,Header("Rayを飛ばす時間")]
    private float _rayTime = 0.05f;

    public override void Awake()
    {
        base.Awake();
        SetUp();
    }

    public void Start()
    {
        CullentSituation.Value = Situation.Idle;
        StartCoroutine(EnemyRayCoroutine());
    }

    private void OnEnable()
    {
        base.Awake();
        SetUp();
        CullentSituation.Value = Situation.None;
        CullentSituation.Value = Situation.Idle;
        StartCoroutine(EnemyRayCoroutine());
    }

    /// <summary>
    /// AIの挙動を制御する
    /// </summary>
    /// <param name="situation"></param>
    public void SituationAI(Situation situation)
    {
        //死んだ後に状態が変化した場合の予防処理
        if(!gameObject.activeSelf) return;

        switch (situation)
        {
            case Situation.Idle://待機
                NavMeshAgent.speed = 0;
                Animator.SetInteger(CullentAnimStatus,(int)MoveType.Idle);
                StartCoroutine(Ai.Idle());
                break;
            case Situation.Return://元の位置に移動
                StartCoroutine(Ai.Return());
                break;
            case Situation.LockOn://追尾
                StartCoroutine(Ai.LockOn());
                break;
            case Situation.Roaming://徘徊
                StartCoroutine(Ai.Roaming());
                break;
            case Situation.Attack://攻撃
                StartCoroutine(Ai.Attack());
                break;
            case Situation.KnockBack:
                StartCoroutine(Ai.KnockBack());
                break;
            case Situation.Die:
                StartCoroutine(Ai.Die());
                break;
        }
    }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void TakeDamage(int damage = 0)
    {
        OnHurt.Invoke();
        //攻撃力０以下は何もしない～
        if(damage <= 0 ) return;
        //攻撃クールタイム中はアニメーション速度が０になっているため１（通常速度）に戻す
        if (Animator.speed == 0) Animator.speed = 1;
        
        //ダメージを受ける
        CullentHp -= damage;
        //体力が０より多ければノックバックする
        if (0 < CullentHp)
        {
            if(Animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie Reaction Hit"))
                Animator.Play("Zombie Reaction Hit", layer:default, 0.15f);
            Animator.SetInteger(CullentAnimStatus,(int)MoveType.KnockBack);
            CullentSituation.Value = Situation.KnockBack;
        }
        //体力が０以下になった場合は死ぬアニメーションを流し消す
        else
        {
            //死んだらゲージが少し上がる
            PlayerGauge.instance.UpGauge();
            //周りの敵を感知させる
            SensingRange.EnemySensing();
            SensingRange.EnemyRemoveSensing();
            CullentSituation.Value = Situation.Die;
            NavMeshAgent.speed = 0;
            Animator.SetInteger(CullentAnimStatus,(int)MoveType.Die);
            GameManager.instance.AddKillEnemyValue();
        }
    }

    /// <summary>
    /// 視野内にプレイヤーが入ったら追尾を開始する
    /// </summary>
    private void CheckLockOn()
    {
        if (CullentSituation.Value == Situation.LockOn && !IsLockOn.Value) StartCoroutine(Ai.CheckLockedPlayer());
        if (IsLockOn.Value) CullentSituation.Value = Situation.LockOn;
    }
    
    
    #region Ray

    /// <summary>
    /// Rayを規定時間ごとに飛ばすコルーチン
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyRayCoroutine()
    {
        var waitTime = new WaitForSeconds(_rayTime);
        while (CullentSituation.Value != Situation.Die)
        {
            RayCast();
            yield return waitTime;
        }
    }
    
    
    /// <summary>
    /// レイキャスト飛ばすやーつ
    /// </summary>
    private void RayCast()
    {
        Vector3 rayPosition = _rayObject.transform.position;
        Ray ray = new Ray(rayPosition,  _rayObject.transform.forward);
        Debug.DrawRay(rayPosition, transform.forward * _rayDistance, Color.red, 0.5f, true);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, _rayDistance))
        {
            //攻撃範囲に入ったオブジェクトがプレイヤーだったら臨戦態勢になるぜ　ちがければ動くぜ
            if (hit.collider.CompareTag("Player"))
            {
                TargetInRange = hit.transform.gameObject;
                if(!(CullentSituation.Value == Situation.CoolDown || CullentSituation.Value == Situation.KnockBack))
                    CullentSituation.Value = Situation.Attack;
            }
            //対象が範囲外に行ったらNullに戻す
            else TargetInRange = null;
        }
        //対象が範囲外に行ったらNullに戻す
        else TargetInRange = null;
    }

    #endregion
    
    
    /// <summary>
    /// 初期設定
    /// </summary>
    private void SetUp()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator     = GetComponent<Animator>();
        Ai           = GetComponent<EnemyAi>();
        NavMeshAgent.speed        = CullentSpeed / 10f;
        NavMeshAgent.angularSpeed = CullentSpeed * 10;
        NavMeshAgent.acceleration = CullentSpeed * 2;
        
        var aiEvent = GetComponent<EnemyAnimEvent>();

        aiEvent.FormerSpeed = NavMeshAgent.speed;
        
        MyStartPos = transform.localPosition;
        IsLockOn.Skip(1).Subscribe(x => CheckLockOn()).AddTo(this);
        CullentSituation.Skip(1).Subscribe(x => SituationAI(x)).AddTo(this);
        Player = GameObject.Find("Player");
    }
}
