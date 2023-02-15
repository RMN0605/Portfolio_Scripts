using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    public SettingInformation settingInformation;

    [SerializeField]
    public static List<Enemy_Data> EnemyList { get; private set; }

    //スコアによる飛来物スポーンレベル
    private int _scoreLevel = 1;

    [SerializeField, Header("配列数(初期値11)")]
    private int _elementNum = 11;

    [SerializeField, Header("難易度段階(M分け)")]
    private int[] _step = new int[11];

    [SerializeField, Header("難易度による頻度")]
    private float[] _appearanceTime = new float[11];

    [SerializeField, Header("難易度による出現数・最低値")]
    private int[] _enemyNumPerLMin = new int[11];

    [SerializeField, Header("難易度による出現数・最大値")]
    private int[] _enemyNumPerLMax = new int[11];

    private int rd;
    [SerializeField, Header("チュートリアルの場合はオンに")]
    private bool _isTutorial = false;
    [Header("トレーニングの場合はオンに")]
    public static bool _isTraining = false;
    public bool isTraining = false;
    /// <summary>
    /// スコアアタックかどうか
    /// </summary>
    public static bool scoreAttack = true;


    private void Awake()
    {
        _isTraining = isTraining;
        _elementNum -= 1;

        if(_isTutorial)
        {
            scoreAttack = false;
            _isTraining = false;
        }
        else if(_isTraining)
        {
            scoreAttack = false;
            _isTutorial = false;

        }
        else
        {
            scoreAttack = true;
        }

        //EnemyDataをロード
        Enemy_Data[] dataFiles = Resources.LoadAll<Enemy_Data>("EnemyData");
        EnemyList = new List<Enemy_Data>(dataFiles);

        if(scoreAttack || _isTraining)
        {
            //スコアアタック飛来物生成コルーチン開始
            StartCoroutine("ScoreAttackSpawn");
        }

    }

    private void Update()
    {
        if(scoreAttack)
        {
            ScoreAttackSpawnFunc();
        }
    }

    void ScoreAttackSpawnFunc()
    {
        //スコアレベルを増やしていく
        if (ScoreManager.Score > _step[_scoreLevel] && _scoreLevel < _elementNum)
        {
            _scoreLevel += 1;
        }

    }

    IEnumerator ScoreAttackSpawn()
    {
        while (true)
        {
            if (_isTraining)
                rd = Random.Range(settingInformation.enemyNum + 1,settingInformation.enemyNum2 + 1);
            else
                rd = Random.Range(_enemyNumPerLMin[_scoreLevel - 1], _enemyNumPerLMax[_scoreLevel - 1]);
            for (int i = 0; i < rd; i++)
            {
                    InstantiateEnemy(GetRandomEnemy());
            }

            //11段階目からは秒数ランダム
            if (_scoreLevel == 11)
            {
                if (_isTraining)
                {
                    yield return new WaitForSeconds(Random.Range(settingInformation.enemyRepopTime,settingInformation.enemyRepopTime2));
                }
                else
                    yield return new WaitForSeconds(Random.Range(2, _appearanceTime[_scoreLevel - 1]));
            }
            else
            {
                yield return new WaitForSeconds(_appearanceTime[_scoreLevel - 1]);
            }
        }
    }


    /// <summary>
    /// ランダムに飛来物データを取得
    /// </summary>
    /// <returns></returns>
    public Enemy_Data GetRandomEnemy()
    {
        if (_isTraining)
        {
            if (settingInformation.enemyType == 0)
            {
                return EnemyList[Random.Range(0, EnemyList.Count)];
            }
            else if (settingInformation.enemyType == 1)
            {
                return EnemyList[Random.Range(0, 1)];
            }
            else
            {
                return EnemyList[Random.Range(1, 2)];
            }
        }
        else
        {
            return EnemyList[Random.Range(0, EnemyList.Count)];
        } 
    }

    /// <summary>
    /// EnemyDataからエネミーを生成
    /// </summary>
    /// <param name="enemyData"></param>
    /// <returns>生成したオブジェクト</returns>
    public static GameObject InstantiateEnemy(Enemy_Data enemyData)
    {
        GameObject enemy = Instantiate(enemyData.Enemy_object, new Vector3(Random.Range(14,16), Random.Range(-4f, 4f), 2f), Quaternion.identity);
        return enemy;
    }

    /// <summary>
    /// 飛来物消去
    /// </summary>
    public void DestroyEnemy(GameObject obj)
    {
        Destroy(obj);
    }
}
