using UnityEngine;

public class MapTile : MonoBehaviour
{
   　private enum OnObjectType //配置されるオブジェクトのタイプ
      {
          Player = 0,
          Rope,
          Stone,
      }

    [HideInInspector] public  SpriteRenderer spriteRenderer;
    [HideInInspector] public SpriteRenderer childSpriteRenderer; //子オブジェクトのスプライトレンダラー
    [HideInInspector] public  Vector2Int     tilePos;
    [HideInInspector] public  Vector4        color = new Vector4();
    [HideInInspector] public  TurnFaceType   turnFaceType; //タイルの表裏
                      private OnObjectType   _onObjectType;
                      
                      public  TileTypeId     tileId;         //タイルID
                      public  bool           isAdvance;      //前進できるか
                      public  bool           _isInvert;       //反転できるか
                      private bool           _isPlayer;      //岩が配置されているか
                      private bool           _isRope;        //プレイヤーが配置されているか
                      private bool           _isStone;       //ロープが配置されているか
                      [HideInInspector] public  GameObject     child;

    public bool isInvert
    {
        get => _isInvert;
        set
        {
            _isInvert = value;
            if(!childSpriteRenderer)childSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (_isInvert || tileId == TileTypeId.goal_01 || tileId == TileTypeId.goal_02)
            {
                childSpriteRenderer.color = new Color(0, 255, 255, 0.4f); //シアン
                color = childSpriteRenderer.color;
            }
            else 
            {
                childSpriteRenderer.color = new Color(255, 0, 0, 0.4f);             //赤色
                color = childSpriteRenderer.color;
            }
        }
    }
    public bool isPlayer
    {
        get => _isPlayer;
        set
        {
            _isPlayer = value;
            SetOnObject(OnObjectType.Player, _isPlayer);
            //プレイヤーを配置する？
            if (_isPlayer)
            {
                StageManager.Instance.player = child;
                child.transform.parent = null;
            }
        }
    }

    public bool isRope
    {
        get => _isRope;
        set
        {
            _isRope = value;
            //isRopeはTrue？ && childはある？ && タグはRope？
            if(_isRope && child && child.CompareTag("Rope"))
                child.SetActive(true);
                
            if(_isRope)
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
        if (!isOnOff && child)
        {
            //デストロイするオブジェクトの名前の指定
            switch (onObjectType)
            {
                case OnObjectType.Player:
                    //同じだったら削除
                    if (child.name == "Player(Clone)")
                    {
                        Destroy(child);
                        child = null; 
                    }
                    break;
                case OnObjectType.Rope:
                    //同じだったら削除
                    if (child.name == "Rope(Clone)")
                    {
                        Destroy(child);
                        child = null; 
                    }
                    break;
                case OnObjectType.Stone:
                    //同じだったら削除
                    if (child.name == "Stone(Clone)")
                    {
                        Destroy(child);
                        child = null; 
                    }
                    break;
                default:
                    return;
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
            if (child != null) return;
            GameObject prefab = (GameObject)Instantiate(objectPrefab, transform.position, Quaternion.identity, transform);
            child = prefab;
            if (child.name == "Stone(Clone)")
            {
                child.GetComponent<MapObjects>().objectPos = tilePos;
                StageManager.Instance.mapManager.mapObjects[tilePos.x, tilePos.y] = child;
                child.transform.parent = null;
                child.transform.position = transform.position;
                StageManager.Instance.stageObject.Add(child);
                
                var beforeMaptile = StageManager.Instance.mapManager.mapTiles[tilePos.x, tilePos.y].GetComponent<MapTile>();
                beforeMaptile.childSpriteRenderer.color = new Color(255, 0, 0, 0.4f);//赤
                return;
            }
            
            child.transform.localPosition = Vector3.zero;
        }
    }
}
