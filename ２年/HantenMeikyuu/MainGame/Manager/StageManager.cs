using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(MapManager))]
public class StageManager : MonoBehaviour
{
                      public static StageManager Instance;
    [HideInInspector] public        MapManager   mapManager;
    [HideInInspector] public        UIManager    uiManager;
                      public        GameObject   clearCanvas;
    
    [HideInInspector] public        List<string> stageList       = new List<string>();     //マップデータのリスト
    [HideInInspector] public        GameObject[] stageTiles;
    [HideInInspector] public        List<GameObject> stageObject = new List<GameObject>();
    
    
    [HideInInspector] public        GameObject   player;
                      public        int          turnNum         = 0;     //現在のターン数
                      private       int          _hantenNum;             //そのステージで使える残りの反転数
    [HideInInspector] public        bool         isPlayerMove    = false; //プレイヤーは移動中？
    [HideInInspector] public        Vector2Int   playerArrayPos;         //プレイヤーの二次元配列座標
                      private       bool         _isHaveRope;            //ロープを所持しているか

    #region セーブ関係

        [HideInInspector] public List<string>          saveStageData        = new List<string>();
        [HideInInspector] public List<string>          saveStageObjectData  = new List<string>();
        [HideInInspector] public List<Vector2Int>      savePlayerArray      = new List<Vector2Int>();
        [HideInInspector] public List<int>             saveTurnNum          = new List<int>();
        [HideInInspector] public List<int>             saveHantenNum        = new List<int>();
        [HideInInspector] public List<bool>            saveIsHaveRope       = new List<bool>();
        [HideInInspector] public List<PlayerDirection> savePlayerDirections = new List<PlayerDirection>();

    #endregion


    public bool isHaveRope
    {
        get => _isHaveRope;
        set
        {
            _isHaveRope = value;
            if(_isHaveRope)
                GeneralManager.Instance.soundManager.PlaySE(SoundManager.SeName.se_12);
            uiManager.ChangeRopeUI();
        }
    }

    public int  hantenNum
    {
        get => _hantenNum;
        set
        {
            _hantenNum = value;
            uiManager.hantensUI.hantenNum = _hantenNum;
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        mapManager = GetComponent<MapManager>();
        uiManager = GetComponent<UIManager>();

        //マップデータを読み込む
        SetMapData();
    }

    private void Start()
    {
        //反転数の読み込み
        hantenNum = GeneralManager.Instance.stageSetting.hantenNum[GeneralManager.Instance.selectStageNum];
        //読み込んだマップデータをもとに置き換える
        mapManager.SetTiles();
        mapManager.OnDataLoad();
        mapManager.stageObjectData = new StageObjectData();
        //スタート段階のマップデータを保存
        mapManager.SaveTurnData();
        mapManager.SaveObject();
        mapManager._nowDataCount = 0;
        //初期設定が全て終了したのでゲームを開始しますぅぅ
        GeneralManager.Instance.isPlay = true;
        GeneralManager.Instance.soundManager.PlayBGM((SoundManager.BgmName)BgmName.bgm_02);
        

    }

    /// <summary>
    /// マップデータを全てロードする
    /// </summary>
    private void SetMapData()
    {
        //MapDataから全てのステージ情報を読み取る
        var mapDatas = Resources.LoadAll<TextAsset>("MapData");
        //stageListに_mapChipの中に格納されているステージ情報をいれる
        foreach (var mapChip in mapDatas)
        {
            stageList.Add(mapChip.text);
        }
    }
}
