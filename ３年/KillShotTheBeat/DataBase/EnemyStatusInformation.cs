using UnityEngine;

[System.Serializable]
    public class EnemyStatusInformation
    {
        public string      Name;        //名前
        public GameObject  Prefab;      //プレハブ
        public AttackRange Range;       //攻撃距離
        public int         Hp;          //ヘルスポイント
        public int         AttackPower; //攻撃力
        public float       CoolTime;    //攻撃後のクールタイム
        public float       Speed;       //移動速度
        public int         Score;       //貰えるスコア
    }

    /// <summary>
    /// 攻撃範囲の区分
    /// </summary>
    public enum AttackRange
    {
        None = 0,
        Short,
        Long
    }

    /// <summary>
    /// 現在の状態
    /// </summary>
    public enum Situation
    {
        None = 0,
        Idle,
        Return,       //元の位置に移動
        LockOn,　  //追従
        Roaming,   //徘徊
        Attack,    //攻撃
        KnockBack, //ノックバック
        CoolDown,  //攻撃のクールタイム
        Die = 999, 
    }