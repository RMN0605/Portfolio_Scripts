using System.Collections;
using UnityEngine;

public class EditorMenu : MonoBehaviour
{
    private          GameObject _jsonMenuObject;          //Jsonメニューオブジェクト
    private          int        _openMenuPos;             //開くときの止まってほしい座標（ｘ）
    private          int        _closeMenuPos;            //閉じるときの座標（ｘ）
    private readonly int        _moveDistance    = 20;    //1フレームで動く距離（ｘ）
    private          bool       _isMenuOnOff     = false; //メニューを開いているか
    private          bool       _isMoving        = false; //メニュー画面を動かしているか
    
    private void Awake()
    {
        if(_jsonMenuObject == null)
            _jsonMenuObject = GameObject.Find("JsonMenu");
    }

    private void Start()
    {
        _openMenuPos = (int)_jsonMenuObject.transform.localPosition.x;
        _closeMenuPos = _openMenuPos -(_openMenuPos / 2);                  
    }
    
    /// <summary>
    /// メニューの開閉
    /// </summary>
    public void MenuOnOff()
    {
        //メニュー画面を動かしてる？
        if (!_isMoving)
        {
            //メニュー画面の移動を開始するよ
            _isMoving = true;
            //メニューを開いてる？
            if (!_isMenuOnOff)
            {
                //マネージャーにメニューを開くよーってやる
                EditorManager.Instance.isOpenedMenu = true;
                StartCoroutine(MenuOnOffMove(_closeMenuPos));
            }
            else
            {
                //マネージャーに閉じるよーってやる
                EditorManager.Instance.isOpenedMenu = false;
                StartCoroutine(MenuOnOffMove(_openMenuPos));
            }
        }
    }
    
    /// <summary>
    /// JsonMenuの開閉
    /// </summary>
    IEnumerator MenuOnOffMove(int goalPos)
    { 
        
        do
        {
            //メニューを開いてる？
            if (!_isMenuOnOff)
            {
                _jsonMenuObject.transform.Translate(_moveDistance,0,0);
            }
            else
            {
                _jsonMenuObject.transform.Translate(-_moveDistance,0,0);
            }

            //１フレーム待機
            yield return null;
            
            //_goalPosに着くまでループするよ
        } while (_jsonMenuObject.transform.localPosition.x != goalPos);
        
        //メニューの状態を変更する
        _isMenuOnOff = !_isMenuOnOff;

        //メニュー画面の移動を終了するよ
        _isMoving = false;
        yield break;
    }
}
