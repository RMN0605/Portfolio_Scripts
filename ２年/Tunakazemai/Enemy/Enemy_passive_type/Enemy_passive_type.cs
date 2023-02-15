using UnityEngine;

public class Enemy_passive_type : Enemy_Base
{

    [SerializeField, Header("風の影響力")]
    public float wind_influence = 0;    //風に対する影響力

    [HideInInspector]
    public float wind_power = 0;    //現在の風力

    public Speed espeed;
    private float speed;
    void Start()
    {
        speed = espeed.speed;
    }
    private void FixedUpdate()
    {
        this.transform.Translate(Vector3.left * Time.deltaTime * speed);    //左に向かって直進するよ！
    }

    /// <summary>
    /// 飛来物が当たった際の処理
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D co)
    {
        if (co.gameObject.tag == "Player")
        {
            //Destroy(co.gameObject);
            GameOver();

        }
    }
}
