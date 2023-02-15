using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerDirection  //移動する方向
{
    Up = 0,
    Down,
    Right,
    Left,
    none,
}
public class Player : MonoBehaviour
{
    private PlayerManager   _playerManager;
    private SpriteRenderer  _playerSprite;
    
    [HideInInspector] public PlayerDirection playerDirection = PlayerDirection.Down;  //プレイヤーの向いている方向
    [SerializeField]  private List<Sprite> spriteLists       = new List<Sprite>();    //上下左右のスプライト

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        _playerSprite = GetComponent<SpriteRenderer>();
        
        //エディターならプレイヤーのスクリプトを停止させる
        if (SceneManager.GetActiveScene().name == "MapEditorScene")
        {
            _playerManager.enabled = false;
            this.enabled = false;
        }
    }

    private void Update()
    {
        #region 移動
        
            #region 斜め禁止
            
                if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.D)))
                    return;
                if ((Input.GetKey(KeyCode.W)) && (Input.GetKey(KeyCode.A)))
                    return;
                if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.D)))
                    return;
                if ((Input.GetKey(KeyCode.S)) && (Input.GetKey(KeyCode.A)))
                    return;
                
            #endregion

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Move(PlayerDirection.Up);
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                Move(PlayerDirection.Down);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                Move(PlayerDirection.Left);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                Move(PlayerDirection.Right);

        #endregion
    }
    
    /// <summary>
    /// 各移動処理
    /// </summary>
    /// <param name="playerDic">進む方向</param>
    private void Move(PlayerDirection playerDic)
    {
        //プレイヤーが移動中？ && 行動してもいい？
        if (StageManager.Instance.isPlayerMove || !GeneralManager.Instance.isPlay) return;
        //ターンを進める
        StageManager.Instance.turnNum++;
        
        Vector2Int beforePos;
        //プレイヤーの向きを変更
        playerDirection = playerDic;
        //プレイヤーの向きにあったスプライトに変更
        ChangePlayerSprite(playerDic);
        //進行方向に岩があるか確認 :ある場合は移動を中断して岩を動かす
        if(_playerManager.CheckRock(playerDic)) return;
        do
        {
            
            //移動前に座標を取得
            beforePos = StageManager.Instance.playerArrayPos;
            _playerManager.SetUpPlayerMove(playerDic);
            //移動後の座標が変わっていなければそのまま終了する
            if (beforePos == StageManager.Instance.playerArrayPos) return;
            //地面が氷床ならもう一度移動
        } while (StageManager.Instance.mapManager.CheckIceFloor());
        //プレイヤーの向きにあったスプライトに変更
        ChangePlayerSprite(playerDic);
        
        //前の場所にいたタイルの色を元に戻す
        var beforeMaptile = StageManager.Instance.mapManager.mapTiles[beforePos.x, beforePos.y].GetComponent<MapTile>();
        beforeMaptile.childSpriteRenderer.color = beforeMaptile.color;
        //今いる場所のタイルの色を赤にして反転できなくする
        var nowMaptile = StageManager.Instance.mapManager.mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>();
        nowMaptile.childSpriteRenderer.color = new Color(255, 0, 0, 0.4f);//赤
    }

    /// <summary>
    /// スプライトの変更
    /// </summary>
    /// <param name="playerDic"></param>
    public void ChangePlayerSprite(PlayerDirection playerDic)
    {
        if(_playerSprite.sprite != spriteLists[(int)playerDic])
            _playerSprite.sprite = spriteLists[(int)playerDic];
    }
}
