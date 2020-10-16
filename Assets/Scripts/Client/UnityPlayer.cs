using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnityPlayer : MonoBehaviourPun
{

    public float speed = 4f;

    public Rigidbody2D rb;

    private float movement = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        movement = Input.GetAxisRaw("Horizontal") * speed;
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        rb.MovePosition(rb.position + new Vector2(movement * Time.deltaTime, 0f));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ball"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }



}
