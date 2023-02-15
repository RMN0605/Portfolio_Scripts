using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HantenUI : MonoBehaviour
{
                     private int  _hantenNum;
    [SerializeField] private Text _text;
                     private Color _color;
    public int hantenNum
    {
        get => _hantenNum;
        set
        {
            _hantenNum = value;
            _text.text = _hantenNum.ToString();
            
            //反転回数が少なくなったら色を変えてお知らせ　:反転数は1よりも多い？
            _color = _hantenNum <= 1 ? Color.red : Color.white;
            //テキストの色は同じ？
            if(_text.color != _color) _text.color = _color;
        }
    }

}
