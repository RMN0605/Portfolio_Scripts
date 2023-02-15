using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    private N_PlayerManager _nPlayerManager;

    private void Awake()
    {
        _nPlayerManager = _enemy.Player.GetComponent<N_PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nPlayerManager.TakeDamage(_enemy.AttackPower);
        }
    }
}
