using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{

    private GameObject Enemy;
    private GameObject Player;
    private GameObject Wall;
    public GameObject Bullet;

    private float distance;
    private float Min = 1.0f;
    private float sight = 5.0f; // 탐색 시야
    private float range = 0.0f;

    private float x, y;
    private float Angle;
    private float xLine, yLine;
    private float speed = 0.04f;
    private float xSpeed, ySpeed;
    private float timer = 1f;
    private bool valid;
    public int Health = 5;

    // Use this for initialization
    void Start()
    {
        Enemy = this.gameObject;
        Player = GameObject.Find("Player");

        if (Enemy.name == "Enemy2(Clone)")
        {
            Health = 10;
            speed = 0.02f;
        }
        else if (Enemy.name == "Enemy3(Clone)")
        {
            speed = 0.03f;
            sight = 7.0f;
            range = 4.0f;
        }
        else if (Enemy.name == "Enemy4(Clone)")
        {
            speed = 0.07f;
        }
    }
    
    void Update()
    {
        if (Health <= 0)
        {
            Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a -= Time.deltaTime * 2;

            gameObject.GetComponent<SpriteRenderer>().color = tmp;
            gameObject.GetComponent<BoxCollider>().enabled = false;

            Destroy(gameObject, 1.0f);

        }

        distance = Vector3.Distance(Player.transform.position, Enemy.transform.position);

        x = Player.transform.position.x - Enemy.transform.position.x;
        y = Player.transform.position.y - Enemy.transform.position.y;
        Angle = Mathf.Atan2(y, x);
        valid = EnemySearching();

        Debug.DrawRay(Enemy.transform.position, new Vector2(x, y), Color.red);


        if (distance <= sight && valid && Health > 0) // 시야 내  &&  겹치지 않게
        {
            if (distance >= Min && distance >= range)  // 겹치지 않게 
                EnemyFollow();
            else if (distance <= range)
                EnemyShoot();

        }
    }


    bool EnemySearching()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            if (hit.transform.tag == "Wall" || hit.transform.tag == "Niddle")
                return false;

            else
                return true;
        }
        else return true;


    }
    void EnemyShoot()
    {
        if (timer > 2)
        {

            Transform shoot = Instantiate(Bullet, Enemy.transform.position, Quaternion.EulerRotation(0, 0, Angle)) as Transform;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    void EnemyFollow()
    {
        xSpeed = x / (distance / speed);
        ySpeed = y / (distance / speed);

        transform.Translate(Vector2.right * xSpeed);
        transform.Translate(Vector2.up * ySpeed);
    }


    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Arrow" && Health > 0)
        {
            Destroy(coll.gameObject);
            Health -=  PlayerMove.Player_Damage;
        }
    }
}
