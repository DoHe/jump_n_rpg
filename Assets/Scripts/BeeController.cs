using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : EnemyController
{
    public GameObject player;
    public float triggerDistance = 5.5f;
    public float deleteDistance = 8f;

    private bool seen = false;

    new void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (transform.position.x - player.transform.position.x < triggerDistance)
        {
            seen = true;
        }

        if (player.transform.position.x - transform.position.x > deleteDistance)
        {
            base.Hit();
        }
    }

    new void FixedUpdate()
    {
        if (seen)
        {
            base.FixedUpdate();
        }
    }
}
