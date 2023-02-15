using Unity.VisualScripting;
using UnityEngine;

public class Robot : Enemy
{
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TakeDamage(10);
        }
    }
    
    #endif
}
