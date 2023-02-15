using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MapData
{
    private readonly int  _mapChipMaxCount = 56; //タイルの最大数
    
    [Serializable]
    public class TileChip
    {
        public TurnFaceType turnFaceType; //タイルの表裏
        public TileTypeId   tileId;       //タイルID
        public bool         isAdvance;    //前進できるか
        public bool         isInvert;     //反転できるか
        public bool         isRope;       //ロープを生成するか
        public bool         isStone;      //岩を生成するか
        public bool         isPlayer;     //プレイヤーを生成するか

        //初期化
        public TileChip()
        {
            turnFaceType = TurnFaceType.Front;
            tileId = TileTypeId.invisible;
            isAdvance = false;
            isInvert = false;
            isRope = false;
            isStone = false;
            isPlayer = false;
        }
    }

    public List<TileChip> tileChips = new List<TileChip>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MapData()
    {
        //全てのタイル情報をTileChipsに書き込む
        foreach (var num in Enumerable.Range(0, _mapChipMaxCount))
        {
            tileChips.Add(new MapData.TileChip());
        }
    }
}
