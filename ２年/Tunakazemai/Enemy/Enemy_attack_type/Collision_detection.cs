using UnityEngine;

public class Collision_detection : MonoBehaviour
{
	private Enemy_attack_type Eat;  //Enemy_attack_typeの頭文字の略
	private bool reaction = false;  //判定内に一度でもプレイヤーが入ったか判定 

	private void Start()
    {
		Eat = transform.root.gameObject.GetComponent<Enemy_attack_type>();
	}

    protected virtual void OnTriggerStay2D(Collider2D co)
	{
		if (co.gameObject.tag == "Player" && Eat.is_tracking == false)
		{
			Eat.is_tracking = true;     //追尾するよ

			if (Eat.player.transform.position.y < transform.root.gameObject.transform.position.y && !reaction)
			{
				Eat.rotate = false;
			}
			
			if (Eat.player.transform.position.y > transform.root.gameObject.transform.position.y && !reaction)
			{
				Eat.rotate = true;
			}
		}
	}
}
