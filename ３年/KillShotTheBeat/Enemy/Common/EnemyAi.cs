using System;
using System.Collections;
using UnityEngine;

public enum MoveType
{
    Idle = 0,
    Walk,
    Attack,
    KnockBack,
    Die,
}

public class EnemyAi : MonoBehaviour
{
    #region 参照
    
    private Enemy _enemy;
    
    #endregion

    private bool _isIdle;
    private bool _isReturn;    //元の位置に移動
    private bool _isLockOn;　  //追従
    private bool _isRoaming;   //徘徊

    [HideInInspector]
    public  Vector3 RoamingNextPoint;
    [SerializeField,Header("追尾時に見失うか確認するまでの時間/秒")]
    private int     waitCount;

    private WaitForSeconds _waitSecond;
    private WaitForSeconds _waitRoamingSecond;
    
    public bool IsSearching;
    
    private void Awake()
    {
        _waitSecond        = new WaitForSeconds(0.2f);
        _enemy             = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        _enemy.CullentSituation.Value = Situation.Idle;
    }

    /// <summary>
    /// 待機
    /// </summary>
    /// <returns></returns>
    public IEnumerator Idle()
    {
        //１秒待機し次の行動を考える
        yield return _waitSecond;
        AiThink();
    }
    
    /// <summary>
    /// 元の位置に移動
    /// </summary>
    /// <returns></returns>
    public IEnumerator Return()
    {
        _enemy.NavMeshAgent.speed = _enemy.CullentSpeed / 10f;
        //歩くアニメーション再生
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Walk);
        //目的につくまで移動
        while(_enemy.CullentSituation.Value  == Situation.Return)
        {
            _enemy.NavMeshAgent.SetDestination(_enemy.MyStartPos);
            
            //スタート地点に戻るまで移動し続ける
            if (transform.position.x <= _enemy.MyStartPos.x + 1 && transform.position.x >= _enemy.MyStartPos.x - 1 
            &&  transform.position.z <= _enemy.MyStartPos.z + 1 && transform.position.z >= _enemy.MyStartPos.z - 1)
            {
                _enemy.CullentSituation.Value = Situation.Roaming;
                yield break;
            }
            yield return null;
        }
    }
    
    /// <summary>
    /// 追尾
    /// </summary>
    /// <returns></returns>
    public IEnumerator LockOn()
    {
        _enemy.NavMeshAgent.speed = _enemy.CullentSpeed / 10f;
        //歩くアニメーション再生
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Walk);
        //目的につくまで移動
        while(_enemy.CullentSituation.Value  == Situation.LockOn)
        {
            _enemy.NavMeshAgent.SetDestination(_enemy.Player.transform.position);
            yield return null;
        }
    }
    
    /// <summary>
    /// 徘徊
    /// </summary>
    /// <returns></returns>
    public IEnumerator Roaming()
    {
        IsSearching = false;
        //目的地を探す
        StartCoroutine(SearchPoint());
        //目的地が決まるまで処理を止める
        yield return new WaitUntil(() => IsSearching);
        
        _enemy.NavMeshAgent.speed = _enemy.CullentSpeed / 10f;
        //歩くアニメーション再生
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Walk);
        //目的地に着くまで移動
        do
        {
            _enemy.NavMeshAgent.SetDestination(RoamingNextPoint);
            yield return null;
        } while (!(transform.position.x <= RoamingNextPoint.x + 3 && transform.position.x >= RoamingNextPoint.x - 3
                && transform.position.z <= RoamingNextPoint.z + 3 && transform.position.z >= RoamingNextPoint.z - 3));
        
        //何もしないアニメーション再生
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Idle);
        _enemy.NavMeshAgent.speed = 0;
        yield return _waitRoamingSecond;
        _enemy.CullentSituation.Value = Situation.Idle;
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    /// <returns></returns>
    public IEnumerator Attack()
    {
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Attack);
        _enemy.NavMeshAgent.speed = 0;
        yield return null;
    }
    
    /// <summary>
    /// ノックバック
    /// </summary>
    /// <returns></returns>
    public IEnumerator KnockBack()
    {
        _enemy.NavMeshAgent.speed = 0;

        //ノックバックのアニメーション再生
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus, (int)MoveType.KnockBack);
        _enemy.Animator.Play("Zombie Reaction Hit", layer:default, 0.15f);
        yield break;
    }

    /// <summary>
    /// 死ぬ
    /// </summary>
    /// <returns></returns>
    public IEnumerator Die()
    {
        //移動停止
        _enemy.NavMeshAgent.speed = 0;
        _enemy.Animator.SetInteger(_enemy.CullentAnimStatus,(int)MoveType.Die);
        _enemy.MyArea.AreaEnemyList.Remove(gameObject);
        yield break;
    }

    /// <summary>
    /// Idle時に次の行動を考え選択する
    /// </summary>
    /// <returns></returns>
    private void AiThink()
    {
        //エリア外に出たら元の位置に戻る
        if (_enemy.IsOutOfArea) _enemy.CullentSituation.Value = Situation.Return;
        //そうでなければ徘徊を再開する
        else _enemy.CullentSituation.Value = Situation.Roaming;
    }

    /// <summary>
    /// 見失ってからプレイヤーを視野内に入れているかどうか
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckLockedPlayer()
    {
        for (int i = 0; i < waitCount; i++)
        {
            yield return _waitSecond;
            //見失っていなければそのまま追尾する
            if(_enemy.IsLockOn.Value) yield break;
        }
        //規定秒数立ってプレイヤーを見つけられていなければIdleに戻す
        _enemy.CullentSituation.Value = Situation.Idle;
    }
    
    /// <summary>
    /// 次の移動先を探すコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator SearchPoint()
    {
        var bounds = _enemy.MyArea.AreaBounds;
        Vector3 nextPoint;
        do
        {
            nextPoint = new Vector3(UnityEngine.Random.Range(-bounds.size.x / 2, bounds.size.x / 2), 
                70, UnityEngine.Random.Range(-bounds.size.z / 2, bounds.size.z / 2));
            nextPoint += _enemy.MyArea.transform.localPosition;
            _enemy.RayMachine.transform.position = nextPoint;
            _enemy.RayMachine.RayCast();
            yield return null;
        } while (!_enemy.RayMachine.IsNotObstacle || nextPoint == Vector3.zero);
        nextPoint.y = 1;
        RoamingNextPoint = nextPoint;
        IsSearching = true;
    }
}