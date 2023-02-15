using UnityEngine;
using UniRx;
using UnityEngine.Serialization;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField]
    protected EnemyId  Id;                               //敵ID
    protected int      MaxHp;                            //最大体力
    private   int      _cullentHp;                       //現在の体力
    public    int      AttackPower { get; private set; } //攻撃力
    private   float    _attackCoolTime;                   //攻撃のクールタイム
    private   float    _cullentSpeed;                     //現在のスピード
    private   int      _getScore;                         //倒したらもらえるスコア
    
    public int CullentHp
    {
        get => _cullentHp;
        set
        {
            _cullentHp = value;
        }
    }
    
    public int  GetScore        => _getScore;
    
    public float AttackCoolTime => _attackCoolTime;

    public float CullentSpeed
    {
        get => _cullentSpeed;
        set => _cullentSpeed = value;
    }
    public virtual void Awake()
    {
        //必要なデータを集める
        var dataBase = Resources.Load<EnemyDataBase> ("EnemyDataBase").EnemyStatusList[(int)Id];
        
        //代入
        MaxHp        　 = dataBase.Hp;
        _cullentHp    　= MaxHp;
        AttackPower  　 = dataBase.AttackPower;
        _attackCoolTime = dataBase.CoolTime;
        _cullentSpeed   = dataBase.Speed;
        _getScore     　= dataBase.Score;
    }
}