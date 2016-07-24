using UnityEngine;
using System.Collections;

public class BossMove : MonoBehaviour {
    private GameObject Boss;
    private GameObject Player;
    public GameObject RedLine;
    public GameObject BossBullet;
    public GameObject exit;
    public int BossHealth;

    private float tmp;

    private float timer;
    private float timer2 = 0;
    private float Distance;
    private float x, y;
    private float tempX, tempY;
    private float Min = 1.0f;
    private float speed = 0.03f;
    private float RushSpeed = 0.5f;
    private float xSpeed, ySpeed;
    private bool valid, valid2;
    private float Angle;
    private float BulletForward;

    void Start () {
        Boss = this.gameObject;
        BossHealth = BoardManager.Stage + 1;
        Player = GameObject.Find("Player");
        
        timer = 0;
        valid = true;
        valid2 = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (BossHealth > 0)
        {
            timer += Time.deltaTime;

            if (timer <= 5) // 5초까지 
            {
                Follow();
                tmp = Distance / 10.0f + 1;
            }
            else if (timer <= 10) // 10초까지
            {
                Rush();
            }
            else if (timer <= 15) // 15초까지
            {
                Follow();
            }
            else
            {
                BossShoot();
            }
        }
        else
        {
            timer += Time.deltaTime;
            FadeAway();
        }

	
	}

    void Follow()
    {
        Distance = Vector3.Distance(Player.transform.position, Boss.transform.position);

        tempX = Player.transform.position.x;
        tempY = Player.transform.position.y;

        if (tempX > 19.5)
            tempX = 19.5f;
        else if (tempX < 1.5)
            tempX = 1.5f;

        if (tempY > 5.5)
            tempY = 5.5f;
        else if (tempY < 1.5)
            tempY = 1.5f;

        x = tempX - Boss.transform.position.x;
        y = tempY - Boss.transform.position.y;

        xSpeed = x / (Distance / speed);
        ySpeed = y / (Distance / speed);

        // 이동!
        transform.Translate(Vector2.right * xSpeed);
        transform.Translate(Vector2.up * ySpeed);
    }

    void Rush()
    {
        if(timer <= 6) // 5~6
        {  // 각잡고
           

            tempX = Player.transform.position.x;
            tempY = Player.transform.position.y;

            if (tempX > 19.5)
                tempX = 19.5f;
            else if (tempX < 1.5)
                tempX = 1.5f;

            if (tempY > 5.5)
                tempY = 5.5f;
            else if (tempY < 1.5)
                tempY = 1.5f;

            Distance = Vector3.Distance(new Vector2(tempX, tempY), Boss.transform.position);

            x = tempX - Boss.transform.position.x;
            y = tempY - Boss.transform.position.y;

            xSpeed = x / (Distance / RushSpeed);
            ySpeed = y / (Distance / RushSpeed);

            Angle = Mathf.Atan2(y,x);// * Mathf.Rad2Deg;
        }
        else if (timer <= 8) // 6~8
        {
            DrawRed(); // 경고하고 
        }
        else if( timer > 8) // 움직여! 8~10
        {
            valid = true;

            Distance = Vector3.Distance(new Vector2(tempX, tempY), Boss.transform.position);
            if (Distance > Min)
            {
                transform.Translate(Vector2.right * xSpeed);
                transform.Translate(Vector2.up * ySpeed);
            }
        }
    }

    void BossShoot()
    {
        if (valid2 == true)
        {
            if (timer >= 17)
            {
                for (int i = 0; i < 8; i++)
                {
                    GameObject Instance = (GameObject)Instantiate(BossBullet, Boss.transform.position, Quaternion.EulerRotation(0, 0, 0 + i * 0.785398f));
                }
                valid2 = false;
            }
        }
        else if(timer >= 18)
        {
            timer = 0;
            valid2 = true;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Arrow" && BossHealth > 0)
        {
            Destroy(coll.gameObject);
            BossHealth -= PlayerMove.Player_Damage;
        }
    }
    void FadeAway()
    {
        

        Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
        tmp.a -= Time.deltaTime;
        gameObject.GetComponent<SpriteRenderer>().color = tmp;
        
        if (tmp.a < 0)
        {
            Instantiate(exit, new Vector3(Boss.transform.position.x, Boss.transform.position.y, 0f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
    void DrawRed() // 6에 진입
    {
        RedLine.transform.localScale = new Vector3(Distance / 6.0f, 3, 1);
        if (timer <= 6.5 && valid == true)
        {
            GameObject Instance = (GameObject)Instantiate(RedLine, new Vector2(Boss.transform.position.x + Mathf.Cos(Angle) * 4.3f, Boss.transform.position.y+Mathf.Sin(Angle) * 4.3f), Quaternion.EulerRotation(0,0,Angle));
            valid = false;
            Destroy(Instance, 0.5f);
        }
        else if (timer >= 6.5 && timer < 7 && valid == false)
        {
            valid = true;
        }
        else if (timer >= 7 && valid == true)
        {
            GameObject Instance = (GameObject)Instantiate(RedLine, new Vector2(Boss.transform.position.x + Mathf.Cos(Angle) * 4.3f, Boss.transform.position.y + Mathf.Sin(Angle) * 4.3f), Quaternion.EulerRotation(0, 0, Angle));
            Destroy(Instance, 0.5f);
            valid = false;
        }
    }
}
