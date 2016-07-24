using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    public Sprite[] HeartSprites;
    public Image HeartUI;
    private PlayerMove player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
	}
	
	// Update is called once per frame
	void Update () {
        HeartUI.sprite = HeartSprites[player.curHealth];
	}
}
