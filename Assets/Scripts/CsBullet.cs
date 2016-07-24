using UnityEngine;
using System.Collections;

public class CsBullet : MonoBehaviour {
    public float BulletSpeed = 0.3f;
	// Use this for initialization
	void Start () {
        if(gameObject.name == "Bullet(Clone)")
        {
            BulletSpeed = 0.15f;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.right * BulletSpeed);
        Destroy(gameObject, 4.0f);

    }
}
