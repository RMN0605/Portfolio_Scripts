using UnityEngine;

public class Enemy_attack_type : Enemy_Base
{
    public Speed espeed;
    private float speed;
    [SerializeField, Header("旋回速度")]
    public float rotate_speed;  //読んで字のごとく
    [SerializeField, Header("旋回最大量")]
    private float rotate_max_quantity;  //Playerとの距離が最大量以下だった場合旋回を開始
    [SerializeField, Header("旋回最小量")]
    private float rotate_min_quantity;  //Playerとの角度が一定だった場合旋回停止

    [HideInInspector]
    public bool is_tracking = false;   //追尾するか否か    旋回開始時とオブジェクトが持つ当たり判定で使用
    [HideInInspector]
    public bool rotate = false;     //左ならfalse　右ならtrue  旋回開始時とオブジェクトが持つ当たり判定で使用
    [HideInInspector]
    public float angle;    //実際のPlayerとの角度が格納されるよ
    [HideInInspector]
    public float angle_with_player;
    [HideInInspector]
    public GameObject player;  //Playerの角度を入れるよ

    private Transform my_transform; //Angleの計算時に使用、自分の角度を計算に使おうとするとエラーを吐くため用意

    private void Start()
    {
        speed = espeed.speed;
        my_transform = this.gameObject.transform;   //Angle計算時使用
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        //Debug.Log(angle);
        this.transform.Translate(Vector3.left * Time.deltaTime * speed);    //左に向かって直進するよ！
        angle = get_angle(this.transform.position, player.transform.position);
        if (is_tracking)
        {
            swing_process();
        }
    }

    /// <summary>
    /// 実際に旋回する際の判定、実行処理
    /// </summary>
    private void swing_process()
    {
        if (angle < rotate_max_quantity && angle > rotate_min_quantity)     //最大＞現在のAngle＞最小　だった場合
        {
            if (!rotate)    //左に旋回するよ
            {
                transform.Rotate(new Vector3(0, 0, rotate_speed));
            }
            else if (rotate)    //右に旋回するよ
            {

                transform.Rotate(new Vector3(0, 0, -rotate_speed));//1フレームごとに1度加算
            }
        }
    }

    /// <summary>
    /// PlayerとEnemyとの角度を算出、計算
    /// </summary>
    /// <param name="my_transform">Enemyの座標</param>
    /// <param name="player">Playerの座標</param>
    /// <returns></returns>
    float get_angle(Vector2 my_transform, Vector2 player)
    {

        Vector2 dt = player - my_transform;
        Vector2 my = -this.transform.right;
        float degree = Vector2.Angle(dt, my);
        return degree;
    }

}