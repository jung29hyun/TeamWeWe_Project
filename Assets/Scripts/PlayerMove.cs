using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 10;
    private Animator animator;
    private int stat, prev_stat;
    public int curHealth = 3;
    const int maxHealth = 5;
    static int tmp = 5;

    public GameObject Player;
    public GameObject[] Arrow;
    public Color color;
    private bool Blink;

    
    private float timer;
    private static int weapon = 0;
    [HideInInspector]
    public static int Player_Damage = 1;

    void Awake()
    {
        animator = GetComponent<Animator>();
        color = this.GetComponent<SpriteRenderer>().color;
        curHealth = tmp;
        timer = 0.0f;
        Blink = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        if (Input.GetButtonDown("Fire1"))  Shoot();
        
        if (Blink)  PlayerBlink();
    }
    void Move()
    {
        float amtMove = Speed * Time.deltaTime;
        float keyForward = Input.GetAxis("Vertical");
        float keySide = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Horizontal") && keySide > 0)
        {
            animator.SetTrigger("playerWalk_Right");
            stat = 3;
        }
        else if (Input.GetButtonDown("Horizontal") && keySide < 0)
        {
            animator.SetTrigger("playerWalk_Left");
            stat = 1;
        }

        else if (Input.GetButtonDown("Vertical") && keyForward > 0)
        {
            animator.SetTrigger("playerWalk_Back");
            stat = 4;
        }
        else if (Input.GetButtonDown("Vertical") && keyForward < 0)
        {
            animator.SetTrigger("playerWalk_Front");
            stat = 2;
        }

        transform.Translate(Vector2.up * amtMove * keyForward);
        transform.Translate(Vector2.right * amtMove * keySide);


        // 탈출 방지
        if (Player.transform.position.x < 0)
            Player.transform.localPosition = new Vector2(0, Player.transform.position.y);
        else if (Player.transform.position.x > 21)
            Player.transform.localPosition = new Vector2(21, Player.transform.position.y);

        if (Player.transform.position.y < 0)
            Player.transform.localPosition = new Vector2(Player.transform.position.x, 0);
        else if (Player.transform.position.y > 7)
            Player.transform.localPosition = new Vector2(Player.transform.position.x, 7);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Food")
        {
            other.gameObject.SetActive(false);

            curHealth++;
            curHealth++;

            if (curHealth > maxHealth)
                curHealth = maxHealth;
        }
        else if (other.tag == "Soda")
        {
            other.gameObject.SetActive(false);

            if (curHealth < maxHealth)
                curHealth++;
        }
        else if (other.tag == "Exit")
        {
            Invoke("Restart", 0.5f);
            tmp = curHealth;
            enabled = false;
        }
        else if ((other.tag == "Niddle" && Blink == false) || (other.tag == "BOSS" && Blink == false))
        {
            curHealth--;
            CheckIfGameOver();
            Blink = true;
        }
        else if ((other.tag == "Weapon"))
        {
            other.gameObject.SetActive(false);
            if (other.name == "Magic_Wand(Clone)")
            {
                weapon = 2;
                Player_Damage = 2;
            }
            else if (other.name == "Double_Shot(Clone)")
            {
                weapon = 1;
            }
            else if (other.name == "Heart_Staff(Clone)")
            {
                weapon = 4;
                Player_Damage = 999;
            }

        }


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Enemy" && Blink == false)
        {
            if (curHealth > 0)
            {
                curHealth--;
            }
            CheckIfGameOver();
            Blink = true;
        }
    }


    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void CheckIfGameOver()
    {
        if (curHealth <= 0)
        {
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }

    void Shoot()
    {
        if (weapon % 2 == 0)
        {

            Transform obj = Instantiate(Arrow[weapon], Player.transform.position, Quaternion.EulerRotation(0, 0, (stat - 1) * 1.5708f)) as Transform;
        }
        else if (weapon == 1)
        {
            Vector3 position = Player.transform.position;
            if (stat % 2 == 0)
            {
                position.x -= 0.2f;
                Transform obj = Instantiate(Arrow[weapon], position, Quaternion.EulerRotation(0, 0, (stat - 1) * 1.5708f)) as Transform;
                position.x += 0.4f;
                Transform obj2 = Instantiate(Arrow[weapon], position, Quaternion.EulerRotation(0, 0, (stat - 1) * 1.5708f)) as Transform;
                Debug.Log("Hello");
            }
            else if (stat % 2 != 0)
            {
                position.y -= 0.2f;
                Transform obj = Instantiate(Arrow[weapon], position, Quaternion.EulerRotation(0, 0, (stat - 1) * 1.5708f)) as Transform;
                position.y += 0.4f;
                Transform obj2 = Instantiate(Arrow[weapon], position, Quaternion.EulerRotation(0, 0, (stat - 1) * 1.5708f)) as Transform;
                Debug.Log("Hello");

            }
        }
    }

    void PlayerBlink()
    {
        timer += Time.deltaTime;

        if (timer > 2f)
        {
            gameObject.GetComponent<Renderer>().enabled = true;
            Blink = false;
            timer = 0;
        }
        else if (timer % 0.5f < 0.2f) // 여기 안들어왕 ㅠㅠㅠ
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = true;
        }

    }

}


