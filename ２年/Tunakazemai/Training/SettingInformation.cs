using UnityEngine;
using UnityEngine.UI;

public class SettingInformation : MonoBehaviour
{
    /// <summary>
    /// 風向き
    /// </summary>
    #region 風向きの変更
    [Header("風向き")]
    public Text widDirectionText;
    [HideInInspector]
    public int windDirection = 0;   

    /// <summary>
    /// テキストの代入
    /// </summary>
    public void Direction()
    {
        switch (windDirection)
        {
            case 0:
                widDirectionText.text = "無風";
                wind.windless();
                break;
            case 1:
                widDirectionText.text = "右向き";
                wind.rightwind();
                break;
            case 2:
                widDirectionText.text = "左向き";
                wind.leftwind();
                break;
            case 3:
                widDirectionText.text = "上向き";
                wind.upwind();
                break;
            case 4:
                widDirectionText.text = "下向き";
                wind.downwind();
                break;
            case 5:
                widDirectionText.text = "右上向き";
                wind.upperright();
                break;
            case 6:
                widDirectionText.text = "右下向き";
                wind.lowerright();
                break;
            case 7:
                widDirectionText.text = "左上向き";
                wind.upperleft();
                break;
            case 8:
                widDirectionText.text = "左下向き";
                wind.lowerleft();
                break;
            case 9:
                widDirectionText.text = "ランダム";
                wind.randomwind();
                break;
        }
    }
    #endregion

    /// <summary>
    /// 風向きの強さ
    /// </summary>
    #region 風向きの強さ変更
    [Header("風向きの強さ")]
    public Text windPowerText;
    [HideInInspector]
    public int windPower = 0;

    /// <summary>
    /// テキストの代入
    /// </summary>
    public void Power()
    {
        switch (windPower)
        {
            case 0:
                windPowerText.text = "デフォルト";
                wind.windvaluehold = 0;
                break;
            case 1:
                windPowerText.text = "強い";
                wind.windvaluehold = 0.3f;
                break;
            case 2:
                windPowerText.text = "弱い";
                wind.windvaluehold = 0.2f;
                break;
            case 3:
                windPowerText.text = "ランダム";
                wind.randomwind();
                break;
        }
    }
    #endregion

    /// <summary>
    /// 飛来物の種類
    /// </summary>
    #region 飛来物の種類の変更
    [Header("飛来物の種類")]
    public Text enemyTypeText;
    [HideInInspector]
    public int enemyType = 0;

    /// <summary>
    /// テキストの代入
    /// </summary>
    public void Type()
    {
        switch (enemyType)
        {
            case 0:
                enemyTypeText.text = "デフォルト";
                break;
            case 1:
                enemyTypeText.text = "攻撃のみ";
                break;
            case 2:
                enemyTypeText.text = "被攻撃のみ";
                break;
        }
    }
    #endregion

    /// <summary>
    /// 敵の速さ
    /// </summary>
    #region 敵の速さの変更
    [Header("敵の速さ")]
    public Text enemySpeedText;
    [HideInInspector]
    public int enemySpeed = 0;
    public static float _enemySpeed2 = 0;
    /// <summary>
    /// テキストの代入
    /// </summary>
    public void Speed()
    {
        switch (enemySpeed)
        {
            case 0:
                enemySpeedText.text = "デフォルト";
                _enemySpeed2 = 0;
                break;
            case 1:
                enemySpeedText.text = "早い";
                _enemySpeed2 = 6;

                break;
            case 2:
                enemySpeedText.text = "遅い";
                _enemySpeed2 = 2;
                break;
            case 3:
                enemySpeedText.text = "普通";
                _enemySpeed2 = 4;
                break;
        }
    }
    #endregion

    /// <summary>
    /// 敵の数
    /// </summary>
    #region 敵の数の変更
    [Header("敵の数")]
    public Text enemyNumText;
    [Header("敵の数2")]
    public Text enemyNumText2;
    [HideInInspector]
    public int enemyNum = 0;
    [HideInInspector]
    public int enemyNum2 = 0;

    /// <summary>
    /// テキストの代入
    /// </summary>
    public void Num()
    {
        switch (enemyNum)
        {
            case 0:
                enemyNumText.text = "1";
                break;
            case 1:
                enemyNumText.text = "2";
                break;
            case 2:
                enemyNumText.text = "3";
                break;
            case 3:
                enemyNumText.text = "4";
                break;
            case 4:
                enemyNumText.text = "5";
                break;
        }
    }
    public void Num2()
    {
        switch (enemyNum2)
        {
            case 0:
                enemyNumText2.text = "1";
                break;
            case 1:
                enemyNumText2.text = "2";
                break;
            case 2:
                enemyNumText2.text = "3";
                break;
            case 3:
                enemyNumText2.text = "4";
                break;
            case 4:
                enemyNumText2.text = "5";
                break;
        }
    }
    #endregion

    /// <summary>
    /// 敵の再出現時間
    /// </summary>
    /// 
    #region 敵の再出現時間の設定
    [Header("敵の再出現時間")]
    public Text enemyRepopTimeText;
    [Header("敵の再出現時間2")]
    public Text enemyRepopTimeText2;
    [HideInInspector]
    public int enemyRepopTime = 0;
    [HideInInspector]
    public int enemyRepopTime2 = 0;
    /// <summary>
    /// テキストの代入
    /// </summary>
    public void Repop_time()
    {
        switch (enemyRepopTime)
        {
            case 0:
                enemyRepopTimeText.text = "3";
                break;
            case 1:
                enemyRepopTimeText.text = "4";
                break;
            case 2:
                enemyRepopTimeText.text = "5";
                break;
            case 3:
                enemyRepopTimeText.text = "6";
                break;
            case 4:
                enemyRepopTimeText.text = "7";
                break;
            case 5:
                enemyRepopTimeText.text = "8";
                break;
            case 6:
                enemyRepopTimeText.text = "9";
                break;
            case 7:
                enemyRepopTimeText.text = "10";
                break;
        }
    }
    public void Repop_time2()
    {
        switch (enemyRepopTime2)
        {
            case 0:
                enemyRepopTimeText2.text = "3";
                break;
            case 1:
                enemyRepopTimeText2.text = "4";
                break;
            case 2:
                enemyRepopTimeText2.text = "5";
                break;
            case 3:
                enemyRepopTimeText2.text = "6";
                break;
            case 4:
                enemyRepopTimeText2.text = "7";
                break;
            case 5:
                enemyRepopTimeText2.text = "8";
                break;
            case 6:
                enemyRepopTimeText2.text = "9";
                break;
            case 7:
                enemyRepopTimeText2.text = "10";
                break;
        }
    }
    #endregion

    /// <summary>
    /// リセット
    /// </summary>

    public Wind wind;
    public void Reset()
    {
        #region 初期化
        windDirection = 0;
        windPower = 0;
        enemyType = 0;
        enemySpeed = 0;
        enemyNum = 0;
        enemyNum2 = 4;
        enemyRepopTime = 0;
        enemyRepopTime2 = 7;
        #endregion

        #region 反映
        Direction();
        Power();
        Type();
        Speed();
        Num();
        Repop_time();
        #endregion
    }
}
