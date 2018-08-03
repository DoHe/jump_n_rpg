﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveForce = 365f;
    public float maxTorque = 120f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public float rotateLift = 5f;
    public float groundedRotateForce = 80f;
    public float airborneRotateForce = 40f;
    public SpriteRenderer bodyRenderer;
    public Sprite oneBiteSprite;
    public Sprite twoBiteSprite;

    private bool grounded = true;
    private bool facingRight = true;
    private bool jump = false;
    private bool rotate = false;
    private bool lying = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private LayerMask platformLayer;
    private int bites = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded && !lying)
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, Vector2.down, .5f, 1 << platformLayer);
        grounded = hit.collider != null;

        float horizontalAxis = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(horizontalAxis));

        if (horizontalAxis > 0 && !facingRight)
            Flip();
        else if (horizontalAxis < 0 && facingRight)
            Flip();

        float strength = grounded ? groundedRotateForce : airborneRotateForce;
        float rotation = facingRight ? -strength : strength;

        float fireAxis = Input.GetAxis("Fire1");
        if (grounded)
            rb2d.AddForce(Vector2.up * fireAxis * rotateLift);
        rb2d.AddTorque(fireAxis * rotation);
        if (Mathf.Abs(rb2d.angularVelocity) > maxTorque)
            rb2d.angularVelocity = maxTorque;
        rotate = false;

        lying = transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 315;
        if (lying)
        {
            return;
        }

        if (horizontalAxis * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * horizontalAxis * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
            grounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            bites += 1;
            switch (bites)
            {
                case 1:
                    bodyRenderer.sprite = oneBiteSprite;
                    break;
                case 2:
                    bodyRenderer.sprite = twoBiteSprite;
                    break;
                default:
                    Die();
                    break;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public IEnumerator RotateMe()
    {
        Quaternion startRotation = transform.rotation;
        int rotation = facingRight ? -90 : 90;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(new Vector3(0, 0, rotation));
        float rate = 3f;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
    }

    void Die()
    {
        Debug.Log("Died :(");
    }
}