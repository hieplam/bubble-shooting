using UnityEngine;

public class ChainCollision : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col)
	{
		Chain.IsFired = !col.CompareTag("Wall");

		if (col.CompareTag("Ball"))
		{
			col.GetComponent<Ball>().Split();
			Chain.IsFired = false;

		}
	}

}
