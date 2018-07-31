﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    //public Transform groundCheck;

    private bool grounded = true;
    private bool facingRight = true;
    private bool jump = false;
    private bool lying = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown("Jump") && grounded && !lying)
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1") && grounded)
        {
            StartCoroutine("RotateMe");
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(h));

        lying = transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 315;
        if (lying)
        {
            return;
        }

        if (h * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
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
        float rate = 1.0f / 0.5f;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * rate;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }
    }
}