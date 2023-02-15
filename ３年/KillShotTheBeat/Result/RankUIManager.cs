using System.Collections.Generic;
using UnityEngine;

public class RankUIManager : MonoBehaviour
{
    [SerializeField]
    private ResultGameManager resultGameManager;

    private int nowScore;
    
    private int _sRank = 40000;
    private int _aRank = 30000;
    private int _bRank = 20000;
    //private int _cRank = 50000;

    [SerializeField] 
    private List<GameObject> _rankObjectList = new List<GameObject>();

    private void Awake()
    {
        nowScore = GameManager.instance.Score;
    }

    private void Start()
    {
        Transform _rank = null;
        if(nowScore >= _sRank)
        {
            _rankObjectList[0].SetActive(true);
            _rank = _rankObjectList[0].transform;
        }
        else if(nowScore >= _aRank)
        {
            _rankObjectList[1].SetActive(true);
            _rank = _rankObjectList[1].transform;
        }
        else if(nowScore >= _bRank)
        {
            _rankObjectList[2].SetActive(true);
            _rank = _rankObjectList[2].transform;
        }
        else
        {
            _rankObjectList[3].SetActive(true);
            _rank = _rankObjectList[3].transform;
        }
        resultGameManager.Rank = _rank;
    }
}
