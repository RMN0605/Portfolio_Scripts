using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
                      private PlayerDirection _playerDirection;
                      private CharacterAnimationControl _characterAnimationControl;

                      private readonly int     _playerSpeed     = 5;
                      private readonly int     _oneMoveDistance = 115;
    [HideInInspector] public           Vector3 pos              = Vector3.zero;

    private void Awake()
    {
        _characterAnimationControl = GetComponent<CharacterAnimationControl>();
    }

    private void Start()
     {
         pos = gameObject.transform.position;
     }
    
    /// <summary>
    /// プレイヤーの配列座標の変更 :プレイヤーが移動する前に読み込まれる
    /// </summary>
    public void SetUpPlayerMove(PlayerDirection playerDirection)
    {
        //前進できる？
        if (StageManager.Instance.mapManager.CheckCanAdvance(playerDirection))
        {
            Vector2Int moveDic = Vector2Int.zero;
            switch (playerDirection)
            {
                case PlayerDirection.Up:
                    moveDic= new Vector2Int(-1, 0);
                    break;
                case PlayerDirection.Down:
                
                    moveDic= new Vector2Int(1, 0);
                    break;
                case PlayerDirection.Right:
                    moveDic= new Vector2Int(0, 1);
                    break;
                case PlayerDirection.Left:
                    moveDic= new Vector2Int(0, -1);
                    break;
            }
            //プレイヤー座標を変更
            StageManager.Instance.playerArrayPos += moveDic;
            //アニメーション開始
            _characterAnimationControl.SetActionMode(false, playerDirection);
            //移動開始
            StartCoroutine(Moving(playerDirection));
        }
        else
        {
            //進行方向が通れない場合はそのタイルがゴールか確認する
            StageManager.Instance.mapManager.CheckGoal(playerDirection);
        }
            
    }

    /// <summary>
    /// 岩が進行方向にある確認
    /// </summary>
    public bool CheckRock(PlayerDirection playerDirection)
    {
        Vector2Int checkPos = new Vector2Int();
        switch (playerDirection)
        {
            case PlayerDirection.Up:
                checkPos = new Vector2Int(StageManager.Instance.playerArrayPos.x - 1, StageManager.Instance.playerArrayPos.y);
                break;
            case PlayerDirection.Down:
                checkPos = new Vector2Int(StageManager.Instance.playerArrayPos.x + 1, StageManager.Instance.playerArrayPos.y);
                break;
            case PlayerDirection.Right:
                checkPos = new Vector2Int(StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y + 1);
                break;
            case PlayerDirection.Left:
                checkPos = new Vector2Int(StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y - 1);
                break;
        }

        if (StageManager.Instance.mapManager.mapObjects[checkPos.x, checkPos.y] != null)
        {
            var mapObject = StageManager.Instance.mapManager.mapObjects[checkPos.x, checkPos.y];
            if (mapObject.name == "Stone(Clone)")
            {
                mapObject.GetComponent<MapObjects>().MoveStone(playerDirection);
                return true;
            }
        }
        return false;
    }
    
    
    /// <summary>
    /// 実際に移動するコルーチン
    /// </summary>
    /// <param name="playerDirection">プレイヤーの移動する方向</param>
    /// <returns></returns>
    IEnumerator Moving(PlayerDirection playerDirection)
    {
        Vector3 movePos = new Vector3();
        //移動した距離
        float movedDistance = 0f;

        StageManager.Instance.isPlayerMove = true;
        //移動する方向からVector3を設定
        switch (playerDirection)
        {
            case PlayerDirection.Up:
                movePos = new Vector3(0, _playerSpeed, 0);
                pos.y += _oneMoveDistance;
                break;
            case PlayerDirection.Down:
                movePos = new Vector3(0, -_playerSpeed, 0);
                pos.y -= _oneMoveDistance;
                break;
            case PlayerDirection.Right:
                movePos = new Vector3(_playerSpeed, 0, 0);
                pos.x += _oneMoveDistance;
                break;
            case PlayerDirection.Left:
                movePos = new Vector3(-_playerSpeed, 0, 0);
                pos.x -= _oneMoveDistance;
                break;
        }
        //移動
        while (true)
        {
            transform.Translate(movePos);
            movedDistance += _playerSpeed;
            //目的地に移動したら抜ける
            if (movedDistance >= _oneMoveDistance)
            {
                //クリアしたか確認
                StageManager.Instance.mapManager.CheckClear();
                break;
            }
            yield return null;
        }
        StageManager.Instance.isPlayerMove = false;
        //移動の誤差を修正
        transform.position = pos;
        //地面に物が落ちていないか確認
        StageManager.Instance.mapManager.CheckUnderItem();
        _characterAnimationControl.SetActionMode(true);
        
        yield break;
    }
}
