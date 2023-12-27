using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public GameObject explosion;

    bool canBeDestroyed = false;
    public int scoreValue = 100;



    // Start is called before the first frame update
    void Start()
    {
        Level.instance.AddDestructables();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -11)
        {
            DestroyDestructable();
        }

        if (transform.position.x < 8.7f && !canBeDestroyed)
        {
            canBeDestroyed = true;
            Gun[] guns = transform.GetComponentsInChildren<Gun>();
            foreach(Gun gun in guns)
            {
                gun.isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeDestroyed)
        {
            return;
        }
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (!bullet.isEnemy)
            {
                Level.instance.AddScore(scoreValue);
                Destroy(gameObject);
                DestroyDestructable();
                Destroy(bullet.gameObject);

            }
        }
    }

    private void OnDestroy()
    {
        Level.instance.RemoveDestructables();
    }


    void DestroyDestructable()
    {
        Instantiate(explosion,transform.position, Quaternion.identity);

        Level.instance.RemoveDestructables();
        Destroy(gameObject);

    }
}
