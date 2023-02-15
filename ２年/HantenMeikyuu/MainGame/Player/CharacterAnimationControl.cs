using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationControl : MonoBehaviour
{
    [System.Serializable]
    public class CharacterAnimationSprites
    {
        [SerializeField]
        //  アニメーションするスプライト
        public List<Sprite> AnimationSprites = new List<Sprite>();
    }
    private PlayerDirection _playerDirection = PlayerDirection.Down;
    [SerializeField]
    private List<CharacterAnimationSprites> _animationSprites = new List<CharacterAnimationSprites>();
    private bool isSpriteChengeOn = false;
    [SerializeField]
    private float _waitTime = 0.01f;

    private Sprite _characterSprite = null;

    //  アイドリング状態フラグ
    private bool _isIdle = false;

    //  アイドリング状態のスプライトインデックス番号リスト
    private List<int> _idleIndexLists = new List<int>() { 0, 7, 14, 21 };
    //  現在再生中のスプライトインデックス
    private int _animationPoseIndex = 0;
    //  アイドリング状態のスプライトインデックス番号
    private const int _idleIndex = 1;
    //  最大インデックス数
    private const int _animationMaxIndex = 4;

    private List<int> _animationTable = new List<int>() { 1, 0, 1, 2 };

    private void Awake()
    {
        _characterSprite = GetComponent<SpriteRenderer>().sprite;
    }

    /// <summary>
    /// 移動モードの設定
    /// </summary>
    /// <param name="isIdle">アイドリングなら true</param>
    /// <param name="characterDirectionType">移動方向</param>
    public void SetActionMode(bool isIdle, PlayerDirection playerDirection = PlayerDirection.none)
    {
        _isIdle = isIdle;
        //  移動方向を指定されていたらその方向に向く
        if (PlayerDirection.none != playerDirection)
            _playerDirection = playerDirection;
        //  アイドリングなら停止画像に変更
        if (_isIdle)
        {
            _characterSprite = _animationSprites[(int)_playerDirection].AnimationSprites[_idleIndex];
        }
        //  アニメーション開始
        else
        {
            _animationPoseIndex = 0;
            _characterSprite = _animationSprites[(int)_playerDirection].AnimationSprites[_animationPoseIndex];
            StartCoroutine(CharacterAnimation());
        }
    }
    /// <summary>
    /// キャラクターアニメーションのコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator CharacterAnimation()
    {
        if (isSpriteChengeOn) { yield break; }
        isSpriteChengeOn = true;
        //  アイドリングになるまで繰り返す
        while (!_isIdle)
        {
            //  指定時間の待機
            yield return new WaitForSeconds(_waitTime);
            //  ポーズインデックスの加算
            _animationPoseIndex++;
            //  アニメーションテーブル数を超えないようにする
            _animationPoseIndex %= _animationMaxIndex;
            //  実際のインデックスを取得する
            var index = _animationTable[_animationPoseIndex];
            //  アニメーションのイメージを書き換える。
            _characterSprite = _animationSprites[(int)_playerDirection].AnimationSprites[index];
            GetComponent<SpriteRenderer>().sprite = _characterSprite;
        }
        isSpriteChengeOn = false;
    }
}
