using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapObjects : MonoBehaviour
{
    
    [HideInInspector] public             Vector2Int objectPos;
                      private readonly   int        _playerSpeed     = 5;
                      private readonly   int        _oneMoveDistance = 115;
                      Vector2Int checkPos = new Vector2Int();
    [HideInInspector] public             Vector3    pos              = Vector3.zero;
    
    private void Start()
    {
        pos = gameObject.transform.position;
        
    }
    
    public void MoveStone(PlayerDirection playerDirection)
    {
        GeneralManager.Instance.isPlay = false;
        do
        { 
            switch (playerDirection)
            {
                case PlayerDirection.Up:
                    checkPos = new Vector2Int(objectPos.x - 1, objectPos.y);
                    break;
                case PlayerDirection.Down:
                    checkPos = new Vector2Int(objectPos.x + 1, objectPos.y);
                    break;
                case PlayerDirection.Right:
                    checkPos = new Vector2Int(objectPos.x, objectPos.y + 1);
                    break;
                case PlayerDirection.Left:
                    checkPos = new Vector2Int(objectPos.x, objectPos.y - 1);
                    break;
            }
            
            var mapTile = StageManager.Instance.mapManager.mapTiles[checkPos.x, checkPos.y].GetComponent<MapTile>();
            //進行方向が道か確認 :壁なら抜ける || 進行方向にオブジェクトがあるか確認 :あるなら抜ける
            if (!(mapTile.tileId == TileTypeId.aisle_01 || mapTile.tileId == TileTypeId.aisle_02 || mapTile.tileId == TileTypeId.aisle_03) 
               || StageManager.Instance.mapManager.mapObjects[checkPos.x, checkPos.y] != null
               || mapTile.isRope)
            {
                GeneralManager.Instance.isPlay = true;
                break;
            }
            SetUpPlayerMove(playerDirection);
            //地面が氷床ならもう一度移動
        } while (StageManager.Instance.mapManager.mapTiles[checkPos.x, checkPos.y].GetComponent<MapTile>().tileId == TileTypeId.aisle_03);
        //StageManager.Instance.mapManager.SaveTurnData();
        //StageManager.Instance.mapManager.SaveObject();
    }

    private void SetUpPlayerMove(PlayerDirection playerDirection)
    {
        //前進できる？
        if (StageManager.Instance.mapManager.mapTiles[checkPos.x, checkPos.y].GetComponent<MapTile>().isAdvance)
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
            //岩が移動するため移動できるようにする
            StageManager.Instance.mapManager.mapTiles[objectPos.x, objectPos.y].GetComponent<MapTile>().isAdvance = true;
            StageManager.Instance.mapManager.mapObjects[objectPos.x, objectPos.y] = null;
            //座標を変更
            objectPos += moveDic;
            StageManager.Instance.mapManager.mapTiles[objectPos.x, objectPos.y].GetComponent<MapTile>().isAdvance = false;
            StageManager.Instance.mapManager.mapObjects[objectPos.x, objectPos.y] = this.gameObject;
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
    /// 実際に移動するコルーチン
    /// </summary>
    /// <param name="playerDirection">プレイヤーの移動する方向</param>
    /// <returns></returns>
    IEnumerator Moving(PlayerDirection playerDirection)
    {
        //前の場所にいたタイルの色を元に戻す
        var beforeMaptile = StageManager.Instance.mapManager.mapTiles[objectPos.x, objectPos.y].GetComponent<MapTile>();
        beforeMaptile.childSpriteRenderer.color = beforeMaptile.color;
        
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
            if (movedDistance >= _oneMoveDistance) break;
            yield return null;
        }
        StageManager.Instance.isPlayerMove = false;
        //移動の誤差を修正
        transform.position = pos;
        GeneralManager.Instance.isPlay = true;
        
        //今いる場所のタイルの色を赤にして反転できなくする
        var nowMaptile = StageManager.Instance.mapManager.mapTiles[objectPos.x, objectPos.y].GetComponent<MapTile>();
        nowMaptile.childSpriteRenderer.color = new Color(255, 0, 0, 0.4f);//赤
        yield break;
    }
    
}
