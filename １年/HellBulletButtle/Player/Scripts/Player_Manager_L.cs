//ル
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager_L : MonoBehaviour
{
    //--------------------------------------------------------------------------------------
    //参照系


    private Rigidbody2D Player_L;   //Rigidbody2D コンポーネントを格納する変数だよ
    private Rigidbody2D Reflect;    //Rigidbody2D コンポーネントを格納する変数だよ
    GameObject System;//勝敗を決める変数、スタートしているかを決める変数を見るものだよ

    //--------------------------------------------------------------------------------------
    //変数系

    public int Character; //キャラクター選択だよ　１，２，３が対応しているよ
    public float AttackPower_percent = 1;//キャラクターの攻撃力[%]
    public bool agaki = false;  //あがきが使えるかどうかを判定するよ
    public bool used_agaki = false; //キャラクターがアガキを使ったかどうか

    [SerializeField]
    Way s_Way;
    [SerializeField]
    Spiral s_Spiral;
    [SerializeField]
    Homing s_Homing;
    [SerializeField]
    Reflect s_Reflect;
    [SerializeField]
    Barrage s_Barrage;
    [SerializeField]
    Random_Homing s_Random_Homing;
    [SerializeField]
    Explosion s_Explosion;
    [SerializeField]
    Cross s_Cross;
    [SerializeField]
    Last_Agaki s_Agaki;
    //--------------------------------------------------------------------------------------
    //ゲームのスタート時の処理

    private void Awake()
    {
        if (gameObject.CompareTag("Player_L1"))
            Character = PanelSwitch.getA();
        else if (gameObject.CompareTag("Player_L2"))
            Character = PanelSwitch2.getB();
    }
    void Start()
    {
        //Rigidbody2D　コンポーネントを取得して変数　Player_L　に格納
        Player_L = GetComponent<Rigidbody2D>();
        //勝敗Bool取得
        System = GameObject.Find("Game_Setting");
    }

    //--------------------------------------------------------------------------------------
    // 繰り返す処理

    void Update()
    {

        //ゲームがスタートしていて、勝敗がついていなかった場合弾を打つことを許可
        if (System.GetComponent<Start_Timer>().is_start_chack && !System.GetComponent<Setting>().syouhai)
        {
            //プレイヤー１だった場合
            if (this.gameObject.CompareTag("Player_L1"))
            {
                if (Character == 1 || Character == 3)
                {
                    //キャラクターが使える４つの弾
                    if (used_agaki == false)
                    {
                        //１つ目　操作キー：Z　｜　操作ボタン：A
                        s_Cross.Shot8();//Cross
                        //２つ目　操作キー：X　｜　操作ボタン：B
                        s_Homing.Shot3();//Homing
                        //３つ目　操作キー：C　｜　操作ボタン：X
                        s_Explosion.Shot7();//Explosion
                        //４つ目　操作キー：V　｜　操作ボタン：Y
                        s_Barrage.Shot5();//Barrage
                    }
                    //必殺技
                    if (agaki == true)
                    {
                        s_Agaki.Shot_agaki();
                    }
                }
                else if (Character == 2 || Character == 4)
                {
                    //キャラクターが使える４つの弾
                    if (used_agaki == false)
                    {
                        //１つ目　操作キー：Z　｜　操作ボタン：A
                        s_Spiral.Shot2();//spiral
                        //２つ目　操作キー：X　｜　操作ボタン：B
                        s_Reflect.Shot4();//Reflect
                        //３つ目　操作キー：C　｜　操作ボタン：X
                        s_Way.Shot1();//Way
                        //４つ目　操作キー：V　｜　操作ボタン：Y
                        s_Random_Homing.Shot6();//Random_Homing
                    }
                    //必殺技
                    if (agaki == true)
                    {
                        s_Agaki.Shot_agaki();
                    }
                }
            }
            //プレイヤー２だった場合
            else if (this.gameObject.CompareTag("Player_L2"))
            {
                if (Character == 1 || Character == 3)
                {
                    //キャラクターが使える４つの弾
                    if (used_agaki == false)
                    {
                        //１つ目　操作キー：Z　｜　操作ボタン：A
                        s_Cross.Shot8();//Cross
                        //２つ目　操作キー：X　｜　操作ボタン：B
                        s_Homing.Shot3();//Homing
                        //３つ目　操作キー：C　｜　操作ボタン：X
                        s_Explosion.Shot7();//Explosion
                        //４つ目　操作キー：V　｜　操作ボタン：Y
                        s_Barrage.Shot5();//Barrage
                    }
                    //必殺技
                    if (agaki == true)
                    {
                        s_Agaki.Shot_agaki();
                    }
                }
                else if (Character == 2 || Character == 4)
                {
                    //キャラクターが使える４つの弾
                    if (used_agaki == false)
                    {
                        //１つ目　操作キー：Z　｜　操作ボタン：A
                        s_Spiral.Shot2();//spiral
                        //２つ目　操作キー：X　｜　操作ボタン：B
                        s_Reflect.Shot4();//Reflect
                        //３つ目　操作キー：C　｜　操作ボタン：X
                        s_Way.Shot1();//Way
                        //４つ目　操作キー：V　｜　操作ボタン：Y
                        s_Random_Homing.Shot6();//Random_Homing
                    }
                    //必殺技
                    if (agaki == true)
                    {
                        s_Agaki.Shot_agaki();
                    }
                }
            }
        }
    }
    //--------------------------------------------------------------------------------------
}