using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    private Enemy_Manager enemy_manager;

    [SerializeField, Header("このスクリプトのデータ")]
    private Enemy_Data enemy_data;



    public Enemy_Data Enemy_Data => Enemy_Data;

    public void GameOver()
    {
        if(Enemy_Manager.scoreAttack)
        {
            TunakazeGameManager.GameOverFunc();
        }
        else if(Enemy_Manager._isTraining)
        {
            TunakazeGameManager.practiceGameOver = true;
            Destroy(gameObject);
        }

    }


}
