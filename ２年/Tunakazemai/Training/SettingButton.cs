using UnityEngine;
using UnityEngine.UI;
public class SettingButton : MonoBehaviour
{
    public SettingInformation menuInformation;

    /// <summary>
    /// 風向きの強さ変更
    /// </summary>
    public void SelectWindDirection()
    {
        if(this.tag == "Plus")
        {
            if (menuInformation.windDirection == 9)
                menuInformation.windDirection = 0;
            else
                menuInformation.windDirection++;

        }
        else if(this.tag == "Minus")
        {
            if (menuInformation.windDirection == 0)
                menuInformation.windDirection = 9;
            else
                menuInformation.windDirection--;
        }
        menuInformation.Direction();
    }

    /// <summary>
    /// 風向きの強さ変更
    /// </summary>
    public void SelectWindPower()
    {
        if (this.tag == "Plus")
        {
            if (menuInformation.windPower == 3)
                menuInformation.windPower = 0;
            else
                menuInformation.windPower++;

        }
        else if (this.tag == "Minus")
        {
            if (menuInformation.windPower == 0)
                menuInformation.windPower = 3;
            else
                menuInformation.windPower--;
        }
        menuInformation.Power();
    }

    /// <summary>
    /// 飛来物の種類の変更
    /// </summary>
    public void SelectEnemyType()
    {
        if (this.tag == "Plus")
        {
            if (menuInformation.enemyType == 2)
                menuInformation.enemyType = 0;
            else
                menuInformation.enemyType++;

        }
        else if (this.tag == "Minus")
        {
            if (menuInformation.enemyType == 0)
                menuInformation.enemyType = 2;
            else
                menuInformation.enemyType--;
        }
        menuInformation.Type();
    }

    /// <summary>
    /// 敵の速さの変更
    /// </summary>
    public void SelectEnemySpeed()
    {
        if (this.tag == "Plus")
        {
            if (menuInformation.enemySpeed == 3)
                menuInformation.enemySpeed = 0;
            else
                menuInformation.enemySpeed++;

        }
        else if (this.tag == "Minus")
        {
            if (menuInformation.enemySpeed == 0)
                menuInformation.enemySpeed = 3;
            else
                menuInformation.enemySpeed--;
        }
        menuInformation.Speed();
    }

    /// <summary>
    /// 敵の数の変更
    /// </summary>
    public void SelectEnemyNum()
    {
        if (this.tag == "Plus" && menuInformation.enemyNum2 > menuInformation.enemyNum)
        {
            if (menuInformation.enemyNum == 4)
                menuInformation.enemyNum = 4;
            else
                menuInformation.enemyNum++;

        }
        else if (this.tag == "Minus" && menuInformation.enemyNum2 >= menuInformation.enemyNum)
        {
            if (menuInformation.enemyNum == 0)
                menuInformation.enemyNum = 0;
            else
                menuInformation.enemyNum--;
        }
        menuInformation.Num();
    }
    public void SelectEnemyNum2()
    {
        if (this.tag == "Plus" && menuInformation.enemyNum2 >= menuInformation.enemyNum)
        {
            if (menuInformation.enemyNum2 == 4)
                menuInformation.enemyNum2 = 4;
            else
                menuInformation.enemyNum2++;

        }
        else if (this.tag == "Minus" && menuInformation.enemyNum2 > menuInformation.enemyNum)
        {
            if (menuInformation.enemyNum2 == 0)
                menuInformation.enemyNum2 = 0;
            else
                menuInformation.enemyNum2--;
        }
        menuInformation.Num2();
    }

    /// <summary>
    /// 敵の再出現時間の設定
    /// </summary>
    public void SelectEnemyRepopTime()
    {
        if (this.tag == "Plus" && menuInformation.enemyRepopTime2 > menuInformation.enemyRepopTime)
        {
            if (menuInformation.enemyRepopTime == 7)
                menuInformation.enemyRepopTime = 7;
            else
                menuInformation.enemyRepopTime++;

        }
        else if (this.tag == "Minus" && menuInformation.enemyRepopTime2 >= menuInformation.enemyRepopTime)
        {
            if (menuInformation.enemyRepopTime == 0)
                menuInformation.enemyRepopTime = 0;
            else
                menuInformation.enemyRepopTime--;
        }
        menuInformation.Repop_time();
    }
    public void SelectEnemyRepopTime2()
    {
        if (this.tag == "Plus" && menuInformation.enemyRepopTime2 >= menuInformation.enemyRepopTime)
        {
            if (menuInformation.enemyRepopTime2 == 7)
                menuInformation.enemyRepopTime2 = 7;
            else
                menuInformation.enemyRepopTime2++;

        }
        else if (this.tag == "Minus" && menuInformation.enemyRepopTime2 > menuInformation.enemyRepopTime)
        {
            if (menuInformation.enemyRepopTime2 == 0)
                menuInformation.enemyRepopTime2 = 0;
            else
                menuInformation.enemyRepopTime2--;
        }
        menuInformation.Repop_time2();
    }

    /// <summary>
    /// リセット
    /// </summary>
    public void SelectReset()
    {
        menuInformation.Reset();
    }

    private void Start()
    {
        SelectReset();
    }
}
