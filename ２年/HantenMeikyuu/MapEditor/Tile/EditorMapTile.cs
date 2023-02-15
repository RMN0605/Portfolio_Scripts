using UnityEngine;

public class EditorMapTile : MonoBehaviour
{
    private enum OnObjectType
    {
      Player = 0,
      Rope,
      Stone,
    }

                      private OnObjectType   _onObjectType;
    [HideInInspector] public  SpriteRenderer spriteRenderer;
    
                      public  TileTypeId     tileId;         //タイルID
                      public  bool           isAdvance;      //前進できるか
                      public  bool           isInvert;       //反転できるか
                      private bool           _isPlayer;      //岩が配置されているか
                      private bool           _isRope;        //プレイヤーが配置されているか
                      private bool           _isStone;       //ロープが配置されているか
                      private GameObject     _child;

    public bool isPlayer
    {
        get => _isPlayer;
        set
        {
            _isPlayer = value;
            SetOnObject(OnObjectType.Player, _isPlayer);
            //プレイヤーを配置する？ && プレイヤーは配置されていない？ : プレイヤーが複数存在しないようにする
            if (_isPlayer &&　!EditorManager.Instance.player)
            {
                EditorManager.Instance.player = this;
            }
            //プレイヤーを配置する？ && すでに置かれているプレイヤーは自身じゃない？
            else if (_isPlayer && EditorManager.Instance.player != this)
            {
                EditorManager.Instance.player.isPlayer = false;
                EditorManager.Instance.player = this;
            }
        }
    }

    public bool isRope
    {
        get => _isRope;
        set
        {
            _isRope = value;
            SetOnObject(OnObjectType.Rope,_isRope);
        }
    }

    public bool isStone
    {
        get => _isStone;
        set
        {
            _isStone = value;
            SetOnObject(OnObjectType.Stone,_isStone);
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 設置と削除
    /// </summary>
    private void SetOnObject(OnObjectType onObjectType,bool isOnOff)
    {
        //設置しない？ && すでに設置されてあるオブジェクトがある？
        if (!isOnOff && _child)
        {
            //デストロイするオブジェクトの名前の指定
            switch (onObjectType)
            {
                case OnObjectType.Player:
                    //同じだったら削除
                    if (_child.name == "Player(Clone)")
                    {
                        Destroy(_child);
                        _child = null; 
                    }
                    break;
                case OnObjectType.Rope:
                    //同じだったら削除
                    if (_child.name == "Rope(Clone)")
                    {
                        Destroy(_child);
                        _child = null; 
                    }
                    break;
                case OnObjectType.Stone:
                    //同じだったら削除
                    if (_child.name == "Stone(Clone)")
                    {
                        Destroy(_child);
                        _child = null; 
                    }
                    break;
            }

        }
        //オブジェクトを設置する？
        else if (isOnOff)
        {
            string objectName = onObjectType.ToString();
            //プレハブを探す
            GameObject objectPrefab = (GameObject)Resources.Load("Prefabs/" + objectName);
            //プレハブはなかった？
            if (objectPrefab == null)
            {
                Debug.LogError($"<color=yellow>Prefabs/{objectName}がないよ</color>");
                return;
            }
        
            //すでに置かれているものはない？
            if (_child != null) return;
            GameObject prefab = (GameObject)Instantiate(objectPrefab, transform.position, Quaternion.identity, transform);
            _child = prefab;
            _child.transform.localPosition = Vector3.zero;
        }
    }
}
