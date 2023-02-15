using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Last_Agaki : MonoBehaviour
{
//--------------------------------------------------------------------------------------
//参照系 

    Shot_Manager s_Manager;   //Shot_Managerを呼び出すためのものだよ
    GameObject HP;
    GameObject System;  //あがきを使ったときにしようするよ
    GameObject Player;

//--------------------------------------------------------------------------------------
//変数系

    public float interval = 22;  //発射間隔だよ
    private int launch_interval = 3;    //発射際に使うやつだよ。自由に調整できるよ
    float rotation_speed = 1f;  //回転する速度だよ  １０００にすると必殺技みたいになるよ（よけられると思うな）

    bool is_attack = false;   //攻撃を許可するか否かを決めるものだよ  trueになったら攻撃開始だ！
    bool rotation;  //回転方向を示すよ　左ならfalse,右ならtrue



//--------------------------------------------------------------------------------------
//あがき系

    public float agaki_time = 3; //あがきをする時間
    private float time_interval;    //あがきで受ける時間を格納するよ
    private float agaki_damage; //あがきで受けるダメージ
    bool set_hp;    //体力が１００に戻ったか見るよ

//--------------------------------------------------------------------------------------
//最初の準備

    private void Start()
    {
        s_Manager = GetComponent<Shot_Manager>();  //s_Managerにある変数を使えるようにするよ
        System = GameObject.Find("Game_Setting");   //あがきbool取得

        #region 使用者設定
        if (this.gameObject.CompareTag("Player_L1"))
        {
            Player = GameObject.Find("Player_L1");
            HP = GameObject.Find("Hit_Body_P1");
        }
        else if (s_Manager.player.gameObject.CompareTag("Player_L2"))
        {
            Player = GameObject.Find("Player_L2");
            HP = GameObject.Find("Hit_Body_P2");
        }
        #endregion

        agaki_damage = 10 / agaki_time;    //あがきで受けるダメージを計算
    }

//--------------------------------------------------------------------------------------
//発射処理

    public void Shot_agaki()
    {
        if (s_Manager.player.gameObject.CompareTag("Player_L1"))
        {
            if ((Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Button_R1")) && System.GetComponent<Setting>().last_of_agaki_check == false)
            {
                System.GetComponent<Setting>().last_of_agaki_check = true;
                set_hp = true;
                Player.GetComponent<Player_Manager_L>().used_agaki = true;
            }
        }
        if (s_Manager.player.gameObject.CompareTag("Player_L2"))
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Button_R2")) && System.GetComponent<Setting>().last_of_agaki_check == false)
            {
                Debug.Log("a");
                System.GetComponent<Setting>().last_of_agaki_check = true;
                set_hp = true;
                Player.GetComponent<Player_Manager_L>().used_agaki = true;
            }
        }
        if (is_attack == false && set_hp == true)
        {
            HP.GetComponent<Player_Manager_R>().m_Player_HP += 5;   //回復
            if(HP.GetComponent<Player_Manager_R>().m_Player_HP > 100)
            {
                HP.GetComponent<Player_Manager_R>().m_Player_HP = 100;
                is_attack = true;
            }
        }
        if (is_attack == true)
        {
            launch_interval++;
            //出る数を調整する処理
            if (launch_interval % interval == 0)
            {
                for (int i = 0; i < 4; i++)     //ここのiの数を変えると一度に出る弾の数が変わるよ
                {
                    Vector2 Vec = new Vector2(0.0f, 10f);
                    if (i == 0)
                    {
                        Vec = Quaternion.Euler(0, 0, 100f * launch_interval) * Vec;
                    }
                    else if (i == 1)
                    {
                        Vec = Quaternion.Euler(0, 0, -100f * launch_interval) * Vec;
                    }
                    Vec.Normalize();
                    Vec = Quaternion.Euler(0, 0, (360 / 5) * i) * Vec;  //どの位の角度で出すか計算するよ
                    Vec *= rotation_speed;
                    //３方向に出す処理
                    for (int bullet_counter = 0; bullet_counter < 9; bullet_counter++)
                    {
                        var a = Quaternion.Euler(0, 0, -Mathf.Atan2(Vec.x, Vec.y) * Mathf.Rad2Deg + (bullet_counter * 45));
                        var b = Instantiate(s_Manager.BulletList[8], transform.position, a);
                        b.transform.SetParent(s_Manager.prefab.transform);    //プレハブをここを親にして出すよ
                        b.GetComponent<Rigidbody2D>().velocity = Vec;
                    }
                    SoundManager.Instance.PlaySE(SE.Last_Agaki);
                }
                for (int i = 0; i < 8; i++)
                {
                    #region　弾の角度計算
                    //玉の角度
                    float Shot_Angle = i * 45;//変更時は22.5(全方向ではなく前だけの場合)
                    Vector3 Angle = transform.eulerAngles;
                    Angle.x = transform.rotation.x;
                    Angle.y = transform.rotation.y;
                    Angle.z = Shot_Angle;
                    #endregion
                    for (int j = 0; j < 2; j++)
                    {
                        #region 弾についているrotationにboolを代入する処理
                        if (!rotation)
                        {
                            rotation = true;    // 左向きだよ！
                        }
                        else if (rotation)
                        {
                            rotation = false;   // 右向きだよ！
                        }
                        #endregion
                        s_Manager.BulletList[9].GetComponent<Shot_Cross>().is_direction = rotation;
                        GameObject Shot = Instantiate(s_Manager.BulletList[9]) as GameObject;
                        Shot.transform.SetParent(s_Manager.prefab.transform);    //プレハブをここを親にして出すよ
                        Shot.transform.rotation = Quaternion.Euler(Angle);
                        Shot.transform.position = this.transform.position;
                    }
                }
                SoundManager.Instance.PlaySE(SE.Last_Agaki);
            }
        }
        if (is_attack == true)    //ダメージ(残りの秒数をゲージに反映させるための体力減少処理)
        {
            time_interval += Time.deltaTime;
            if (time_interval > 0.2f)
            {
                HP.GetComponent<Player_Manager_R>().m_Player_HP -= agaki_damage;
                Debug.Log(HP.GetComponent<Player_Manager_R>().m_Player_HP);
                time_interval = 0;
            }
        }
    }
}
