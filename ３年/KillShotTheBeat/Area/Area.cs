using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Area : MonoBehaviour
{
    #region 参照

    private BoxCollider      _boxCollider;
    public  SearchRayMachine SearchRay;
    [SerializeField]
    private EnemyPool        _enemyPool;
    
    #endregion
    
    private WaitForSeconds summonDiley = new WaitForSeconds(0.2f);
    [Space(10),SerializeField]
    private bool            _isAreaInPlayer;  //プレイヤーがエリア内にいるかどうか
    [SerializeField,Header("このポイントから出現した敵")]
    public List<GameObject> AreaEnemyList          = new List<GameObject>();

    [Header("現在生成している敵の数")]
    public  int             CullentAreaEnemyNum;
    [Header("このポイントが一度に召喚できる最大敵数")]
    public  int             SummonNum;
    [SerializeField,Header("このポイントが召喚する秒数の間隔")] 
    private float           _summonTimeSecond;
    [SerializeField,Header("このポイントが召喚できる最大敵数")]
    public  int             MaxAreaEnemyNum;
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        AreaBounds   = _boxCollider.bounds;
        _waitSummon  = new WaitUntil(() => _isSummonEnd);
    }

    private void Start()
    {
        StartCoroutine(Summon());
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _operation = false;
            _isAreaInPlayer = true;
        }
        else if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            if(enemy.MyArea == this)
                enemy.IsOutOfArea = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player")) _isAreaInPlayer = false;
        else if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            if(enemy.MyArea == this)
                enemy.IsOutOfArea = false;
        }
        if (!_operation)
        {
            _operation = true;
            StartCoroutine(Summon());
        }
    }

    private WaitForSeconds _onesecond = new WaitForSeconds(1);
    private bool           _operation; 　　　 //コルーチンが稼働できるか　/　複数呼ばないため
    private bool 　　　　　 _isCanGeneration; //生成できるか
    private bool           _isSummonEnd;
    private WaitUntil      _waitSummon;      //敵の生成が終わるまで待つ
    
    /// <summary>
    /// 敵の生成
    /// まず規定秒待機した後プレイヤーの位置を確認し生成を開始する
    /// </summary>
    /// <returns></returns>
    private IEnumerator Summon()
    {
        //三秒待機後プレイヤーがエリア内にいない場合敵の生成を開始する
        for (var time = 3; time >= 0; time--)
        {
            yield return _onesecond;
            if (_isAreaInPlayer)
            {
                _operation = false;
                yield break;
            }
        }

        while (!_isAreaInPlayer)
        {
            //規定数ユニットを生成
            for (int enemyNum = 1; enemyNum <= SummonNum; enemyNum++)
            {
                _isSummonEnd = false;
                //生成できる規定値に達した場合はおわる
                if (CullentAreaEnemyNum >= MaxAreaEnemyNum) break;
                if (_isAreaInPlayer)
                {
                    _operation = false;
                    yield break;
                }
                //生成
                StartCoroutine(SearchSummonPos());
                yield return _waitSummon;
                CullentAreaEnemyNum++;
            }
            yield return _onesecond;
        }
        _operation = false;
    }

    [HideInInspector] 
    public Bounds AreaBounds;
    private IEnumerator SearchSummonPos()
    {
        Vector3 summonArea;
        //生成位置をランダムに
        do
        {
            summonArea = new Vector3(UnityEngine.Random.Range(-AreaBounds.size.x / 2, AreaBounds.size.x / 2), 
                70, UnityEngine.Random.Range(-AreaBounds.size.z / 2, AreaBounds.size.z / 2));
            summonArea += transform.localPosition;
            SearchRay.transform.localPosition = summonArea;
            SearchRay.RayCast();
            yield return null;
        } while (!SearchRay.IsNotObstacle|| SearchRay.transform.position == Vector3.zero);

        //出す座標が決まったら高さを地面に合わせる
        summonArea.y = 0;

        //プレハブをオーダーするぜ！
        var callEnemy =  _enemyPool.OrderEnemy(summonArea);
        var enemyComponent = callEnemy.GetComponent<Enemy>();
        //生成した敵のエリアの設定
        enemyComponent.MyArea = this;
        AreaEnemyList.Add(callEnemy);
        
        //出勤
        callEnemy.SetActive(true);
        enemyComponent.CullentSituation.Value = Situation.Idle;
        enemyComponent.Animator.SetInteger(enemyComponent.CullentAnimStatus,(int)MoveType.Idle);
        //callEnemy.SituationAI(enemyComponent.CullentSituation);
        //生成が終わったことを伝える
        _isSummonEnd = true;
    }
}
