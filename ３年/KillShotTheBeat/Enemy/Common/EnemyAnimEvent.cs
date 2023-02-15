using System;
using System.Collections;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;
    [SerializeField,Header("ノックバック後の待機時間")]
    private float _knockBackTime = 1;
    
    private N_PlayerManager _playerManager;
    WaitForSeconds waitAttacCoolTime;
    WaitForSeconds waitKnockBackTime;
    WaitForSeconds waitTime3;

    [SerializeField,Header("攻撃の当たり判定")]
    private GameObject _attackCollider;　//Robot > Character1_Reference >>> Character1_RightHand > AttackCollider を格納してください。

    [SerializeField,Header("爆発effectと爆発させる場所")]
    private ParticleSystem _explosionEffect;
    [SerializeField]
    private GameObject _explosionPos;
    
    [HideInInspector]
    public float FormerSpeed;
    
    private void Start()
    {
        _playerManager = _enemy.Player.GetComponent<N_PlayerManager>();
        waitAttacCoolTime = new WaitForSeconds(_enemy.AttackCoolTime);
        waitKnockBackTime = new WaitForSeconds(_knockBackTime);
    }

    /// <summary>
    /// SetActiveがfalseになったら呼ばれる
    /// </summary>
    private void OnDisable()
    {
        //テスト用
        if (AreaManager.Instance == null) return;
        
        //敵全体を保持しているリストから自身を消す
        AreaManager.Instance.AreaEnemyList.Remove(gameObject);
        //エリアが保持しているリストから自身を消す
        _enemy.MyArea.AreaEnemyList.Remove(gameObject);
        //自身を生成したポイントから生成している数を減らす
        _enemy.MyArea.CullentAreaEnemyNum--;
        //敵をすべて管理しているリストから消す
        AreaManager.Instance.AreaEnemyList.Remove(gameObject);
        //初期化する
        _enemy.MyArea = null;
        _enemy.CullentSituation.Value = Situation.None;
        //プールに自身が空いていることを伝える
        EnemyPool.Instance.EnemyPoolList[_enemy.MyPoolNum].IsUsedPrefab = false;
    }
    

    #region 攻撃＆攻撃のクールタイム


    /// <summary>
    /// 攻撃判定を表示
    /// </summary>
    public void ShowAttackObj()
    {
        _attackCollider.SetActive(true);
    }

    /// <summary>
    /// 攻撃判定を非表示
    /// </summary>
    public void HideAttackObj()
    {
        _attackCollider.SetActive(false);
        //状態をクールダウンにする
        _enemy.CullentSituation.Value = Situation.CoolDown;
    }

    //クールタイム関数
    public void AttackCoolTime()
    {
        StartCoroutine(CoolTimeCoroutine());
    }

    private IEnumerator CoolTimeCoroutine()
    {
        //アニメーション一時停止
        _enemy.Animator.speed = 0.5f;
        //待つよーん
        yield return waitAttacCoolTime;
        //アニメーション再生
        _enemy.Animator.speed = 1;
    }

    /// <summary>
    /// 攻撃アニメーション終了時に呼ばれる
    /// </summary>
    public void AttackAnimEnd()
    {
        if (_enemy.TargetInRange)
        {
            
            //攻撃対象がまだ範囲内に入っていれば再度攻撃する
            _enemy.CullentSituation.Value = Situation.Attack;
            _enemy.Animator.Play("Zombie Punching", 0, 0.09f);
        }
        //そうでなければ移動開始
        else
        {
            _enemy.CullentSituation.Value = Situation.LockOn;
        }
    }

    #endregion
    
    #region ノックバック

    /// <summary>
    /// ノックバック開始時に呼ばれる
    /// </summary>
    public void KnockBackStart()
    {
        if(_enemy.NavMeshAgent.speed != 0) _enemy.NavMeshAgent.speed = 0;
    }

    /// <summary>
    /// ノックバック終了時に呼ばれる
    /// </summary>
    public void KnockBackEnd()
    {
        StartCoroutine(KnockBackCoroutine());
    }

    /// <summary>
    /// ノックバックじのコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator KnockBackCoroutine()
    {
        yield return waitKnockBackTime;
        
        _enemy.NavMeshAgent.speed = 0;
        if (_enemy.TargetInRange)
        {
            _enemy.CullentSituation.Value = Situation.Attack;
            _enemy.Animator.Play("Zombie Punching", 0, 0.09f);
        }
        //そうでなければ移動を再開する
        else if (!_enemy.TargetInRange)
        {
            _enemy.CullentSituation.Value = Situation.LockOn;
            _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Walk);
        }
    }
    #endregion

    public void InstanceExplosionEffect()
    {
        _explosionEffect.Play();
    }

    public void SpeedDown()
    {
        var speed = FormerSpeed / 2.2f;
        _enemy.NavMeshAgent.speed = speed;
    }

    public void SpeedUp()
    {
        _enemy.NavMeshAgent.speed = FormerSpeed;
    }

    public void DestroyObj()
    {
        gameObject.SetActive(false);
    }
}
