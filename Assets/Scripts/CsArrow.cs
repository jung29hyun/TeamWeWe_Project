using UnityEngine;
using System.Collections;

public class CsArrow : MonoBehaviour
{

    public float ArrowSpeed = 0.3f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * ArrowSpeed);
        Destroy(gameObject, 2.0f);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Wall" )
        {
            Destroy(gameObject);

        }
    }
}