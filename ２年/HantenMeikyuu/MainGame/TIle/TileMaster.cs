using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnFaceType　//現在のタイルの状態（表裏/ゴール）
{
    Front = 0,
    Back,
    Goal
}

public class TileMaster : MonoBehaviour
{
    private MapTile      _mapTile;
    private SpriteRenderer _spriteRenderer;
    
    public  List<Sprite> spriteLists = new List<Sprite>(); //タイルの画像（表裏/ゴール）
    private Sprite       _first;                           //表の画像
    private Sprite       _second;                          //裏の画像
    private Sprite       _third;                           //ゴールの画像

    private void Awake()
    {
        _mapTile = GetComponent<MapTile>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeTile()
    {
        //反転できる？
        if (_mapTile.isInvert || _mapTile.turnFaceType == TurnFaceType.Back && spriteLists[(int)TurnFaceType.Front].name == "goal_01")
        {
            StartCoroutine("Kaiten");
        }
    }
    private IEnumerator Kaiten()
    {
        GeneralManager.Instance.isPlay = false;
        for (int i = 0; i < 180; i += 6)
        {
            transform.Rotate(0,6,0);
            //見えないところで反転するよ
            if (i == 90 || i == -90)
            {
                Hanten();
                GeneralManager.Instance.isPlay = true;
            }
            yield return null;
        }
    }
    
    /// <summary>
    /// タイルの反転処理
    /// </summary>
    public void Hanten(bool isHaveRope = false)
    {
        //裏のスプライトはある？ :ない場合は反転できないタイルである
        if (spriteLists.Count < 2) return;
        //タイルは表？ && 裏のスプライトはgoal_02？ && ロープはもってる？
        if (_mapTile.turnFaceType == TurnFaceType.Front && spriteLists[(int)TurnFaceType.Back].name == TileTypeId.goal_02.ToString() && isHaveRope)
        {
            ChangeClearGoal();
        }
        //タイルは反転できる？
        else if (_mapTile.isInvert)
        {
            //スプライトを裏のスプライトに変更
            _mapTile.spriteRenderer.sprite = spriteLists[(int)TurnFaceType.Back];
            //タイルを裏判定にする
            _mapTile.turnFaceType = TurnFaceType.Back;
            //反転できないようにする :ゴールは例外とする
            _mapTile.isInvert = false;
            //タイルの状態を変更
            ChangeTileType();
        }
        //タイルは裏？ && 表のスプライトはgoal_01？
        else if (_mapTile.turnFaceType == TurnFaceType.Back && spriteLists[(int)TurnFaceType.Front].name == "goal_01")
        {
            //タイルを表にする
            _mapTile.turnFaceType = TurnFaceType.Front;
            //スプライトを表のスプライトに変更
            _mapTile.spriteRenderer.sprite = spriteLists[(int)TurnFaceType.Front];
            _mapTile.tileId = TileTypeId.goal_01;
        }
    }

    /// <summary>
    /// clear条件を満たしたゴールに変更
    /// </summary>
    private void ChangeClearGoal()
    {
        //スプライトをクリア条件を満たしたゴールのスプライトに変更
        _mapTile.spriteRenderer.sprite = spriteLists[(int)TurnFaceType.Goal]; 
        //タイルをゴールにする
        _mapTile.turnFaceType = TurnFaceType.Goal;
        //通れるようにする
        _mapTile.isAdvance = true;
        //反転できないようにする
        _mapTile.isInvert = false;
        GeneralManager.Instance.soundManager.PlaySE(SoundManager.SeName.se_09);
    }
    
    /// <summary>
    /// タイルの状態を変更 :状態→tileId(タイルのID),isAdvance(進めるか),isInvert(反転できるか)
    /// </summary>
    private void ChangeTileType()
    {
        _mapTile.tileId    = _packLists[(int)_mapTile.tileId].tileTypeId;
        _mapTile.isAdvance = _packLists[(int)_mapTile.tileId].istileAdvance;
    }

    private class IdPack // タイルごとの通れる通れないの判定
    {
        public TileTypeId  tileTypeId;
        public bool 　　　 istileAdvance;

        public IdPack(TileTypeId tileType, bool isAdvance)
        {
            tileTypeId = tileType;
            istileAdvance = isAdvance;
        }
    }
    private readonly List<IdPack> _packLists = new List<IdPack>()
    {
        new IdPack(TileTypeId.invisible,false),
        new IdPack(TileTypeId.wall_02,true),
        new IdPack(TileTypeId.invisible,true),
        new IdPack(TileTypeId.wall_03,true),
        new IdPack(TileTypeId.aisle_02,false),
        new IdPack(TileTypeId.invisible,false),
        new IdPack(TileTypeId.aisle_03,false),
        new IdPack(TileTypeId.goal_02,false),
        new IdPack(TileTypeId.goal_02,false),
        new IdPack(TileTypeId.goal_01,false),
        new IdPack(TileTypeId.invisible,false),
        new IdPack(TileTypeId.invisible,false),
        new IdPack(TileTypeId.invisible,false),
        new IdPack(TileTypeId.statue_11,false),
        new IdPack(TileTypeId.statue_12,false),
        new IdPack(TileTypeId.statue_13,false),
        new IdPack(TileTypeId.statue_14,false),
        new IdPack(TileTypeId.statue_01,false),
        new IdPack(TileTypeId.statue_02,false),
        new IdPack(TileTypeId.statue_03,false),
        new IdPack(TileTypeId.statue_04,false),
    };
    
    #region スプライトの初期設定関係
        
        /// <summary>
        /// 表、裏、ゴールのスプライトをListに登録
        /// </summary>
        private void InitSprite(Sprite first, Sprite second = null, Sprite third = null)
        {
            //表の画像を登録
            spriteLists.Add(first);
            //secondはある？
            if (null != second)
                spriteLists.Add(second);
            //thirdはある？
            if (null != third)
                spriteLists.Add(third);
            //表のスプライトを表示
            _mapTile.spriteRenderer.sprite = spriteLists[(int)TurnFaceType.Front];
        }

        /// <summary>
        /// 表裏/ゴールのスプライトをセット
        /// </summary>
        public void SetSprites(TileTypeId tileId)
        {
            string tileSpriteName = ""; //検索するスプライトの名前
            //表のスプライトを登録
            _first = Resources.Load<Sprite>("Textures/" + tileId.ToString()) as Sprite;
            //裏のスプライトの名前を検索
            tileSpriteName = SearchSpriteName(tileId);
            //裏スプライトはある？
            if (tileSpriteName != null)
            {
                _second = Resources.Load<Sprite>("Textures/" + tileSpriteName) as Sprite;
                
                //このタイルはゴール？ :ゴールならスプライトを登録する
                if (tileId == TileTypeId.goal_01)
                {
                    _third = Resources.Load<Sprite>("Textures/" + TileTypeId.goal_03.ToString()) as Sprite;
                }
            }
            
            InitSprite(_first,_second,_third);
        }

        /// <summary>
        /// スプライトの名前検索
        /// </summary>
        private string SearchSpriteName(TileTypeId tileId)
        {
            switch (tileId)
            {
                #region 道の場合

                    case TileTypeId.aisle_01: //反転できる床
                        return TileTypeId.wall_02.ToString();
                    case TileTypeId.aisle_03: //氷床
                        return TileTypeId.wall_03.ToString();

                #endregion
                
                #region 壁の場合

                    case TileTypeId.wall_01: //反転できる壁
                        return TileTypeId.aisle_02.ToString();
                    case TileTypeId.wall_03: //氷の壁
                        return TileTypeId.aisle_03.ToString();

                #endregion

                #region 石像の場合

                case TileTypeId.statue_01: //石像:上
                        return TileTypeId.statue_11.ToString();
                case TileTypeId.statue_02: //石像:下
                        return TileTypeId.statue_12.ToString();
                case TileTypeId.statue_03: //石像:右
                        return TileTypeId.statue_13.ToString();
                case TileTypeId.statue_04: //石像:左
                        return TileTypeId.statue_14.ToString();

                #endregion
                
                #region 壊れた石像の場合

                case TileTypeId.statue_11: //壊れた石像:上
                        return TileTypeId.statue_01.ToString();
                case TileTypeId.statue_12: //壊れた石像:下
                        return TileTypeId.statue_02.ToString();
                case TileTypeId.statue_13: //壊れた石像:右
                        return TileTypeId.statue_03.ToString();
                case TileTypeId.statue_14: //壊れた石像:左
                        return TileTypeId.statue_04.ToString();

                #endregion

                #region ゴールの場合

                    case TileTypeId.goal_01: //ゴール:裏
                        return TileTypeId.goal_02.ToString();

                #endregion
            }
            
            return null;
        }
            
    #endregion
    
}
