using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNameUI : MonoBehaviour
{
    [SerializeField] private Text   _text;
    
                     private string _stageName; //ステージ名が入る


     private void OnEnable()
     {
         ChengeName();
     }

     private void ChengeName()
    {
    //チュートリアル？
    if (GeneralManager.Instance.selectStageNum == 0) _stageName = "チュートリアル";
    else
    {
        string stageNum = null;
        switch (GeneralManager.Instance.selectStageNum)
        {
            case 1:
                stageNum = "一";
                break;
            case 2:
                stageNum = "二";
                break;
            case 3:
                stageNum = "三";
                break;
            case 4:
                stageNum = "四";
                break;
            case 5:
                stageNum = "五";
                break;
            case 6:
                stageNum = "六";
                break;
            case 7:
                stageNum = "七";
                break;
            case 8:
                stageNum = "八";
                break;
            case 9:
                stageNum = "九";
                break;
            case 10:
                stageNum = "十";
                break;
            default:
                stageNum = "EX";
                break;
        }
        _stageName = "第" + stageNum + "階層";
    }
    //名前の置き換え
    _text.text = _stageName;
    }
}
