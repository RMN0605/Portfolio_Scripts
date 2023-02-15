using UnityEngine;

public class SearchRayMachine : MonoBehaviour
{
    private float _rayDistance = 100;

    [HideInInspector]
    public bool IsNotObstacle; 
    
    /// <summary>
    /// レイキャスト飛ばすやーつ
    /// 敵の生成、敵の次の目的地の探索で使う。
    /// </summary>
    public void RayCast()
    {
        Vector3 rayPosition = transform.position;
        Ray     ray         = new Ray(rayPosition,  transform.forward);
        IsNotObstacle       =  false;
        
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                IsNotObstacle =  false;
            }
            else if (hit.collider.CompareTag("Area"))
            {
                IsNotObstacle =  true;
            }
            else
            {
                IsNotObstacle =  false;
            }
        }
    }
}
