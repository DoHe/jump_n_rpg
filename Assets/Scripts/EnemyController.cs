using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed = 4f;

    private Rigidbody2D rb2d;
    private bool facingRight = false;
    private LayerMask platformLayer;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, .5f, 1 << platformLayer);
        if (hit.collider == null)
        {
            Flip();
        }
        float direction = facingRight ? 1 : -1;
        rb2d.velocity = new Vector2(direction * maxSpeed, rb2d.velocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
