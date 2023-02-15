using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region BGM
    public enum BgmName    //BGMの種類
    {
        //以後追加
        bgm_01 = 0,
        bgm_02,
        bgm_03,
        bgm_04,
        Silent = 999,
    }
    [HideInInspector]
    public BgmName bgm;
    private       AudioSource     _bgmSource;
    public        List<BgmStatus> bgmClips;
    private int[]                 _bgmNumber;   //BgmNameの項目数の取得
    private int                   _currentBgmIndex = 999;  //現在選ばれているBGM番号

    [System.Serializable]
    public struct BgmStatus  //リスト情報
    {
        [Header("名前")]
        public BgmName Name;
        [Header("音量"), Range(0, 1)]
        public float Volume;
        [Header("BGMデータ")]
        public AudioClip BgmData; //BGM一覧
    }



    /// <summary>
    /// 再生関数
    /// </summary>
    /// <param name="bgmName">選択したBGM</param>
    public void PlayBGM(BgmName bgmName, bool loopFlg = true)
    {
        
        int index = (int)bgmName;    //選択されたBGM番号を格納
        _currentBgmIndex = index;    //選択されたBGM番号を再生する為の変数に格納
        if (index == 999) //無音にするときのやつ
        {
            Debug.LogWarning("無音になったよ");
            StopBGM();  //他のBGMをすべてストップさせるよ
            return;
        }
        #region エラー回避用
        if (index < 0 || _bgmNumber.Length <= index)  //選択されたBGM番号があるか確認：数字でPlayBGMを呼び出された際のエラー回避
        {
            Debug.LogWarning("検索できなかったよ");
            return;
        }
        else if (_bgmSource.clip != null && _bgmSource.clip == bgmClips[index].BgmData) // 同じBGMの場合は何もしない
        {

            Debug.LogWarning("BGMが同じだったよ");
            _bgmSource.Stop();
            _bgmSource.Play();
            return;
        }
        #endregion
        else if (!_bgmSource.isPlaying)  //再生されていなかったら
        {
            _bgmSource.clip = bgmClips[index].BgmData;    //再生するBGMを選択
            _bgmSource.volume = bgmClips[index].Volume;  //音量を調整するよー
            _bgmSource.Play();    //再生するよー
            return;
        }
        StopBGM();
    }

    /// <summary>
    /// BGM停止関数
    /// </summary>
    public void StopBGM()
    {
        _bgmSource.Stop();    //すべてのBGMを停止
        return;
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        _bgmSource.Stop(); //BGMを一時停止～
    }

    /// <summary>
    /// 止めたBGMを再開する関数
    /// </summary>
    public void ResumeBGM()
    {
        _bgmSource.Play(); //止めたBGMを再生～
    }

    #endregion

    #region  SE

    public enum SeName
    {
        se_01 = 0,
        se_01_2,
        se_02,
        se_03,
        se_04,
        se_05,
        se_06,
        se_07,
        se_08,
        se_09,
        se_10,
        se_11,
        se_12,
        se_13,
        se_14
    }

    private       AudioSource    _seSource;
    public        List<SeStatus> seClips;
    private int[]                _seNumber;   //SeNameの項目数の取得
    [System.Serializable]
    public struct SeStatus  //リスト情報
    {
        [Header("名前")]
        public SeName Name;
        [Header("音量"), Range(0, 1)]
        public float Volume;
        [Header("SEデータ")]
        public AudioClip SeData; //BGM一覧
    }

    /// <summary>
    /// 再生関数
    /// </summary>
    /// <param name="seName">選択したSE</param>
    public void PlaySE(SeName seName)
    {
        int index = (int)seName;    //選択されたSE番号を格納
        if (index < 0 || _seNumber.Length <= index)  //選択されたSE番号があるか確認：数字でPlaySEを呼び出された際のエラー回避
        {
            Debug.LogWarning("検索できなかったよ");
            return;
        }
        _seSource.clip = seClips[index].SeData;    //再生するSEを選択
        _seSource.volume = seClips[index].Volume;  //音量を調整するよー
        _seSource.Play();
        return;
    }

    /// <summary>
    /// SE停止関数
    /// </summary>
    public void StopSE()
    {

        _seSource.Stop();    //すべてのSEを停止

        return;
    }

    #endregion

    private void Awake()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _seSource = gameObject.AddComponent<AudioSource>();

        string[] BGM = System.Enum.GetNames(typeof(BgmName));    //string[]→int[]に変換
        _bgmNumber = new int[BGM.Length];    //intに変換

        string[] SE = System.Enum.GetNames(typeof(SeName));    //string[]→int[]に変換
        _seNumber = new int[SE.Length];    //intに変換
    }
}
