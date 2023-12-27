using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Ship1 : MonoBehaviour
{
    Vector2 initialPosition;

    Gun[] guns;

    float moveSpeed = 3;
    float speedMultiplier = 1;

    int hits = 1;

    bool moveUp;
    bool moveDown;
    bool moveLeft;
    bool moveRight;
    bool speedUp;

    bool shoot;

    GameObject shield;
    int powerUpGunLevel = 0;

    private void Awake()
    {
        //initialPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();
        guns = transform.GetComponentsInChildren<Gun>();
        foreach(Gun gun in guns)
        {
            gun.isActive = true;
            if (gun.powerUpLevelRequirement != 0)
            {
                gun.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveUp = Input.GetKey(KeyCode.UpArrow);
        moveDown = Input.GetKey(KeyCode.DownArrow);
        moveLeft = Input.GetKey(KeyCode.LeftArrow);
        moveRight = Input.GetKey(KeyCode.RightArrow);

        shoot = Input.GetMouseButtonDown(1);
        if (shoot)
        {
            shoot = false;
            foreach(Gun gun in guns)
            {
                if (gun.gameObject.activeSelf)
                {
                    gun.Shoot();

                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        float moveAmount = moveSpeed * speedMultiplier * Time.fixedDeltaTime;
        if (speedUp)
        {
            moveAmount *= 3;
        }
        Vector2 move = Vector2.zero;

        if(moveUp)
        {
            move.y += moveAmount;
        }

        if(moveDown)
        {
            move.y -= moveAmount;
        }

        if(moveLeft)
        {
            move.x -= moveAmount;
        }

        if (moveRight)
        {
            move.x += moveAmount;
        }

        pos += move;
        if (pos.x <= -8.5f)
        {
            pos.x = -8.5f;
        }
        if (pos.x >= 8.5f)
        {
            pos.x = 8.5f;
        }
        if (pos.y >= 4f)
        {
            pos.y = 4f;
        }
        if (pos.y <= -4f)
        {
            pos.y = -4f;
        }

        transform.position = pos;
    }

    void ActivateShield()
    {
        shield.SetActive(true);
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
    }

    bool HasShield()
    {
        return shield.activeSelf;
    }

    void AddGun()
    {
        powerUpGunLevel++;
        foreach(Gun gun in guns)
        {
            if (gun.powerUpLevelRequirement <= powerUpGunLevel)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }
        }
    }

    void SetSpeedMultiplier(float mult)
    {
        speedMultiplier = mult;
    }

    void ResetShip()
    {
        //transform.position = initialPosition;
        DeactivateShield();
        powerUpGunLevel = -1;
        AddGun();
        SetSpeedMultiplier(1);
        hits = 1;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null )
        {
            if (bullet.isEnemy)
            {
                Destroy(gameObject);
                Destroy(bullet.gameObject);
            }
        }
        Destructable destructable = collision.GetComponent<Destructable>();
        if (destructable != null)
        {
            if (HasShield())
            {
                DeactivateShield();
            }
            else
            {
                Destroy(gameObject);
            }
            Destroy(destructable.gameObject);
        }


        PowerUp powerup = collision.GetComponent<PowerUp>();
        {
            if (powerup)
            {
                if (powerup.activateShield)
                {
                    ActivateShield();
                }
                if (powerup.addGuns)
                {
                    AddGun();
                }
                if (powerup.increaseSpeed)
                {
                    SetSpeedMultiplier(speedMultiplier + 1);
                }
                Level.instance.AddScore(powerup.pointValue);
                Destroy(powerup.gameObject);
            }
        }
    }


}