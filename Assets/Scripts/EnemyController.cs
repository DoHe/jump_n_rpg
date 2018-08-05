using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed = 4f;
    public bool facingRight = false;

    private Rigidbody2D rb2d;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        float direction = facingRight ? 1 : -1;
        rb2d.velocity = new Vector2(direction * maxSpeed, rb2d.velocity.y);
    }

    public void Hit()
    {
        StartCoroutine("Disappear");
        Destroy(gameObject, 1f);
    }

    public IEnumerator Disappear()
    {
        Vector3 startScale = transform.localScale;
        float rate = 3f;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            transform.localScale = Vector3.Slerp(startScale, Vector3.zero, t);
            yield return null;
        }
    }
}

