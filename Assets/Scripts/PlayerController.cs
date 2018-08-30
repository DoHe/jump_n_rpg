using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveForce = 80f;
    public float minTorque = 50f;
    public float maxTorque = 120f;
    public float maxSpeed = 5f;
    public float jumpForce = 400f;
    public float rotateLift = 5f;
    public float groundedRotateForce = 80f;
    public float airborneRotateForce = 40f;
    public float enemyPushBackFoce = 300f;
    public SpriteRenderer bodyRenderer;
    public Sprite oneBiteSprite;
    public Sprite twoBiteSprite;
    public Sprite threeBiteSprite;

    private bool grounded = true;
    private bool facingRight = true;
    private bool jump = false;
    private bool rotate = false;
    private bool lying = false;
    private Animator anim;
    private Rigidbody2D rb2d;
    private LayerMask platformLayer;
    private int bites = 0;
    private GameController gameController;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        platformLayer = LayerMask.NameToLayer("Platform");
        gameController = GameObject.FindWithTag("GameMaster").GetComponent<GameController>();
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
        if (!GameController.gameRunning)
        {
            return;
        }
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
        if (!GameController.gameRunning)
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (Mathf.Abs(rb2d.angularVelocity) > minTorque && Input.GetAxis("Fire1") == 1f)
            {
                other.gameObject.GetComponent<EnemyController>().Hit();
                return;
            }
            Vector2 pushBackForce = transform.position.x > other.transform.position.x ? Vector2.right : Vector2.left;
            bites += 1;
            switch (bites)
            {
                case 1:
                    bodyRenderer.sprite = oneBiteSprite;
                    rb2d.AddForce(pushBackForce * enemyPushBackFoce);
                    break;
                case 2:
                    bodyRenderer.sprite = twoBiteSprite;
                    rb2d.AddForce(pushBackForce * enemyPushBackFoce);
                    break;
                default:
                    bodyRenderer.sprite = threeBiteSprite;
                    Die();
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameController.gameRunning && other.gameObject.CompareTag("Goal"))
        {
            gameController.EndLevel(gameObject, true);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Die()
    {
        gameController.EndLevel(gameObject, false);
        anim.SetTrigger("Die");
    }
}