using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : EnemyController
{
    private LayerMask platformLayer;
    private float timeSinceLastMiss = 0.5f;

    new void Start()
    {
        base.Start();
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    new void FixedUpdate()
    {
        timeSinceLastMiss += Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, .5f, 1 << platformLayer);
        if (timeSinceLastMiss > 0.5f)
        {
            if (hit.collider == null)
            {
                Flip();
                timeSinceLastMiss = 0;
            }
        }
        base.FixedUpdate();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
