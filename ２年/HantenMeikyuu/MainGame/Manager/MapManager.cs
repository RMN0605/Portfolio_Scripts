using System;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
     private          MapData         _mapData = new MapData();       //ステージで使うMapのデータ
     public          StageObjectData stageObjectData;
     
     
                       public  readonly GameObject[,] mapTiles = new GameObject[7,8];   //マップの二次元配列
                       public           GameObject[,] mapObjects = new GameObject[7,8]; //マップにあるオブジェクトの二次元配列
     [HideInInspector] public           int           _nowDataCount;                    //現在読み込んでいるデータのリスト番号を取得
                       public           GameObject    ropeAnim;                         //ロープアニメーション

    #region ゲーム開始前に使う関数

        /// <summary> 
        /// 開始時にマップデータをステージのデータをもとに置き換え
        /// </summary>
        public void OnDataLoad()
        {
            _mapData = JsonUtility.FromJson<MapData>(StageManager.Instance.stageList[GeneralManager.Instance.selectStageNum]);
            RestTiles();
            LoadTileData();
            Debug.Log($"<color=blue>データをロードしたよ</color>");
        }

        /// <summary>
        /// タイルを二次元配列に置き換える
        /// </summary>
        public void SetTiles()
        {
            int tileCount = 0;
            for (int height = 0; height < 7; height++)
            {
                for (int width = 0; width < 8; width++)
                {
                    mapTiles[height, width] = StageManager.Instance.stageTiles[tileCount];
                    mapTiles[height, width].GetComponent<MapTile>().tilePos = new Vector2Int(height, width);
                    tileCount++;
                }
            }

        }

        /// <summary> 
        /// JsonからTileDataに戻す→プレイヤー座標の設定
        /// </summary>
        private void LoadTileData()
        {
            //MapTileと置き換えとTileMasterの設定
            foreach (var map in _mapData.tileChips.Select((mapChip, index) => new { mapChip, index }))
            {
                MapTile tileData = StageManager.Instance.stageTiles[map.index].GetComponent<MapTile>();
                //Idからタイルの画像の名前を取得
                string spriteName = ((TileTypeId)map.mapChip.tileId).ToString();
                //Texturesにある取得した名前のスプライトを取得
                Sprite tileSprite = Resources.Load<Sprite>("Textures/" + spriteName) as Sprite;
                
                //差し替え
                tileData.spriteRenderer.sprite = tileSprite;
                tileData.tileId = map.mapChip.tileId;
                tileData.isAdvance = map.mapChip.isAdvance;
                tileData.isInvert = map.mapChip.isInvert;
                tileData.isPlayer = map.mapChip.isPlayer;
                tileData.isRope = map.mapChip.isRope;
                tileData.isStone = map.mapChip.isStone;
                
                //差し替えたデータをもとにタイルマスターを設定
                tileData.GetComponent<TileMaster>().SetSprites(tileData.tileId);
            }
            
            //プレイヤーの初期値の設定
            for (int height = 0; height < 7; height++)
            {
                for (int width = 0; width < 8; width++)
                {
                    //プレイヤーの初期位置だった場合はステージマネージャーに登録する
                    if (mapTiles[height, width].GetComponent<MapTile>().isPlayer)
                        StageManager.Instance.playerArrayPos = new Vector2Int(height, width);
                }
            }
        }

        /// <summary>
        /// TileDataを初期化
        /// </summary>
        private void RestTiles()
        {
            foreach (var tile in StageManager.Instance.stageTiles)
            {
                MapTile mapTile = tile.GetComponent<MapTile>();
                mapTile.isAdvance = mapTile.isInvert = mapTile.isPlayer = 
                    mapTile.isRope = mapTile.isStone = false;
            }
        }

    #endregion
     
    #region ゲーム進行中に使う関数

        /// <summary>
        /// 前進できるか確認し結果を返す
        /// </summary>
        public bool CheckCanAdvance(PlayerDirection playerDirection)
        {
            //通ることができるか確認
            switch (playerDirection)
            {
                case PlayerDirection.Up:
                    return mapTiles[StageManager.Instance.playerArrayPos.x - 1, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>().isAdvance;
                case PlayerDirection.Down:
                    return mapTiles[StageManager.Instance.playerArrayPos.x + 1, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>().isAdvance;
                case PlayerDirection.Right:
                    return mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y + 1].GetComponent<MapTile>().isAdvance;
                case PlayerDirection.Left:
                    return mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y - 1].GetComponent<MapTile>().isAdvance;
                default: //それ以外はFalseを返す
                    return false;
            }
        }

        /// <summary>
        /// プレイヤーの位置がゴールか確認、そのあと進行方向にゴールがあるか確認
        /// </summary>
        public void CheckGoal(PlayerDirection playerDirection)
        {
            MapTile tile = null;

            //進行方向のタイルを取得
            switch (playerDirection)
            {
                case PlayerDirection.Up:
                    tile =  mapTiles[StageManager.Instance.playerArrayPos.x - 1, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>();
                    break;
                case PlayerDirection.Down:
                    tile = mapTiles[StageManager.Instance.playerArrayPos.x + 1, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>();
                    break;
                case PlayerDirection.Right:
                    tile = mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y + 1].GetComponent<MapTile>();
                    break;
                case PlayerDirection.Left:
                    tile = mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y - 1].GetComponent<MapTile>();
                    break;
            }
            //タイルはゴール？ && ロープは所持してる？
            if (!(tile.tileId == TileTypeId.goal_01 || tile.tileId == TileTypeId.goal_03) || !StageManager.Instance.isHaveRope) return;
            //ゴールにする
            tile.gameObject.GetComponent<TileMaster>().Hanten(StageManager.Instance.isHaveRope);
            StageManager.Instance.isHaveRope = false;
        }
        
        /// <summary>
        ///プレイヤーのいるタイルが氷床か確認する
        /// </summary>
        public bool CheckIceFloor()
        {
            return mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>().tileId == TileTypeId.aisle_03;
        }

        /// <summary>
        /// プレイヤーのいるタイルに何らかのアイテムが落ちているか確認
        /// </summary>
        public void CheckUnderItem()
        {
            //すでにロープを持っている場合は終了
            if (StageManager.Instance.isHaveRope) return;
            var obj = mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>();
            //プレイヤーの床にロープは落ちている？
            if (!obj.isRope) return;
            StageManager.Instance.isHaveRope = obj.isRope;
            //プレイヤーがロープを取得したときに呼ばれる
            StageManager.Instance.isHaveRope = true;
            //ロープアニメーション再生
            Instantiate (ropeAnim, StageManager.Instance.player.transform.localPosition, Quaternion.identity);
            obj.isRope = false;
            obj.child.SetActive(false);
        }

        /// <summary>
        /// クリアしていた場合はシーンを移動する
        /// </summary>
        public void CheckClear()
        {
            if (mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y].GetComponent<MapTile>().turnFaceType != TurnFaceType.Goal) return;
            {
                StageManager.Instance.clearCanvas.SetActive(true);
                GeneralManager.Instance.isPlay = false;
            }
        }

        /// <summary>
        /// 現在のターンデータをセーブ
        /// </summary>
        public void SaveTurnData()
        {
            //マップデータを読み込み
            foreach (var map in _mapData.tileChips.Select((mapChip, index) => new { mapChip, index }))
            {
                var tileData = StageManager.Instance.stageTiles[map.index].GetComponent<MapTile>();

                map.mapChip.turnFaceType = tileData.turnFaceType;
                map.mapChip.tileId = tileData.tileId;
                map.mapChip.isAdvance = tileData.isAdvance;
                map.mapChip.isInvert = tileData.isInvert;
                map.mapChip.isRope = tileData.isRope;
            }
            var json = JsonUtility.ToJson(_mapData, false);

            //ロードするのは初期データ？ :ちがければロードしたデータを削除
            for (var destroyDataNum = StageManager.Instance.saveStageData.Count - 1; destroyDataNum > _nowDataCount; destroyDataNum--)
            {
                //ロードしたセーブデータは削除
                StageManager.Instance.saveStageData.Remove(StageManager.Instance.saveStageData[destroyDataNum]);
                StageManager.Instance.savePlayerArray.Remove(StageManager.Instance.savePlayerArray[destroyDataNum]);
                StageManager.Instance.saveTurnNum.Remove(StageManager.Instance.saveTurnNum[destroyDataNum]); 
                StageManager.Instance.saveHantenNum.Remove(StageManager.Instance.saveHantenNum[destroyDataNum]); 
                StageManager.Instance.saveIsHaveRope.Remove(StageManager.Instance.saveIsHaveRope[destroyDataNum]);
                if(StageManager.Instance.saveStageObjectData.Count > 0) continue;
                StageManager.Instance.saveStageObjectData.Remove(StageManager.Instance.saveStageObjectData[destroyDataNum]);
            }

            #region セーブ

                //マップデータを保存
                StageManager.Instance.saveStageData.Add(json);
                //プレイヤー座標を保存
                StageManager.Instance.savePlayerArray.Add(StageManager.Instance.playerArrayPos);
                //プレイヤーの方向を保存
                StageManager.Instance.savePlayerDirections.Add(StageManager.Instance.player.GetComponent<Player>().playerDirection);
                //そのターンを保存
                StageManager.Instance.saveTurnNum.Add(StageManager.Instance.turnNum);
                //反転数を保存
                StageManager.Instance.saveHantenNum.Add(StageManager.Instance.hantenNum);
                //ロープの所持を保存
                StageManager.Instance.saveIsHaveRope.Add(StageManager.Instance.isHaveRope);

            #endregion

            _nowDataCount++;
        }

        public void SaveObject()
        {
            //マップオブジェクトデータを読み込み
            foreach (var map in stageObjectData.objectChips.Select((objectChip, index) => new { objectChip, index }))
            {
                if(StageManager.Instance.stageObject.Count <= map.index) break;
                var mapObject = StageManager.Instance.stageObject[map.index].gameObject.GetComponent<MapObjects>();
                map.objectChip.objectPos = mapObject.objectPos;
                map.objectChip.pos = StageManager.Instance.stageObject[map.index].gameObject.transform.position;
            }
            var objectJson = JsonUtility.ToJson(stageObjectData, false);
            //オブジェクトデータを保存
            StageManager.Instance.saveStageObjectData.Add(objectJson);
        }
        
        /// <summary>
        /// 一手前のデータをロード
        /// </summary>
        public void LoadTurnData()
        {
            Index loadDataNum = _nowDataCount;
            if(_nowDataCount >= 1) _nowDataCount--;
            _mapData = JsonUtility.FromJson<MapData>(StageManager.Instance.saveStageData[loadDataNum]);
            if (StageManager.Instance.saveStageObjectData.Count >= 1)
            {
                stageObjectData = JsonUtility.FromJson<StageObjectData>(StageManager.Instance.saveStageObjectData[loadDataNum]);  
            }
            
            #region ロード
                //二次配列のプレイヤー座標を読み込み
                StageManager.Instance.playerArrayPos = StageManager.Instance.savePlayerArray[loadDataNum];
                //プレイヤーのPositionを読み込み
                StageManager.Instance.player.transform.position = mapTiles[StageManager.Instance.playerArrayPos.x, StageManager.Instance.playerArrayPos.y].transform.position;
                StageManager.Instance.player.GetComponent<PlayerManager>().pos = StageManager.Instance.player.transform.position;
                //プレイヤーの方向,スプライトを読み込み
                StageManager.Instance.player.GetComponent<Player>().playerDirection = StageManager.Instance.savePlayerDirections[loadDataNum];
                StageManager.Instance.player.GetComponent<Player>().ChangePlayerSprite(StageManager.Instance.savePlayerDirections[loadDataNum]);
                //ターン数を読み込み
                StageManager.Instance.turnNum = StageManager.Instance.saveTurnNum[loadDataNum];
                //反転数を読み込み
                StageManager.Instance.hantenNum = StageManager.Instance.saveHantenNum[_nowDataCount];
                //ロープの所持を読み込み
                StageManager.Instance.isHaveRope = StageManager.Instance.saveIsHaveRope[loadDataNum];
                StageManager.Instance.uiManager.ChangeRopeUI();
                //マップデータを読み込み
                foreach (var map in _mapData.tileChips.Select((mapChip, index) => new { mapChip, index }))
                {
                    MapTile tileData = StageManager.Instance.stageTiles[map.index].GetComponent<MapTile>();
                    //Idからタイルの画像の名前を取得
                    string spriteName = ((TileTypeId)map.mapChip.tileId).ToString();
                    //Texturesにある取得した名前のスプライトを取得
                    Sprite tileSprite = Resources.Load<Sprite>("Textures/" + spriteName) as Sprite;

                    //差し替え
                    tileData.turnFaceType = map.mapChip.turnFaceType;
                    tileData.spriteRenderer.sprite = tileSprite;
                    tileData.tileId = map.mapChip.tileId;
                    tileData.isAdvance = map.mapChip.isAdvance;
                    tileData.isInvert = map.mapChip.isInvert;
                    tileData.isRope = map.mapChip.isRope;
                }

                if (StageManager.Instance.stageObject.Count > 0)
                {
                    //オブジェクトデータ読み込み
                    foreach (var map in stageObjectData.objectChips.Select((ObjectChip, index) => new { ObjectChip, index }))
                    {
                        if(StageManager.Instance.stageObject.Count <= map.index) break;
                        MapObjects mapObjects = StageManager.Instance.stageObject[map.index].GetComponent<MapObjects>();
                        StageManager.Instance.mapManager.mapObjects[mapObjects.objectPos.x, mapObjects.objectPos.y] = null;
                        mapObjects.objectPos = map.ObjectChip.objectPos;
                        StageManager.Instance.mapManager.mapObjects[mapObjects.objectPos.x, mapObjects.objectPos.y] = mapObjects.gameObject;
                        mapObjects.pos = map.ObjectChip.pos;
                        StageManager.Instance.stageObject[map.index].gameObject.transform.position = map.ObjectChip.pos;
                    }   
                }

            #endregion
        }
        
    #endregion

}
