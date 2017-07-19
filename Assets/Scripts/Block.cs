﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    string color = "white";
    public Color whiteColor, greenColor, redColor, yellowColor, 
        purpleColor, blueColor, orangeColor, indigoColor, rainbowColor;

    //Private Vars
    private AudioSource[] sounds;

    //Const vars
    private const float GRAVITY_SCALE_FACTOR = 180f;

    private void Start()
    {
        if (!MenuManager.isImpossibleMode) GetComponent<Rigidbody2D>().gravityScale += Time.timeSinceLevelLoad / GRAVITY_SCALE_FACTOR;
        else GetComponent<Rigidbody2D>().gravityScale += ((Time.timeSinceLevelLoad / GRAVITY_SCALE_FACTOR) / 1.5f);
        PickBlock();
        sounds = AudioManager.GetInstance().GetSounds();
	}

    // Update is called once per frame
    void Update () {
		if(transform.position.y < -2)
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //score++
        if (collision.name == "Player")
        {
            Score.score++;
            sounds[2].Play();
            if (Score.score % 15 == 0)
            {
                if (!MenuManager.isImpossibleMode)
                {
                    GameManager.level += 1;
                    FindObjectOfType<GameManager>().SwitchLevel();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            switch(color)
            {
                case "white":
                    //FindObjectOfType is not very performant and safe, it should be replaced
                    //Game Over
                    if (!Player.isInvincible)
                    {
                        //To enable Death just un-comment these statements
                        sounds[4].Play();
                        GameManager.isRedEnabled = false;
                        GameManager.isPurpleEnabled = false;
                        GameManager.isYellowEnabled = false;
                        GameManager.isOrangeEnabled = false;
                        GameManager.isGreenEnabled = false;
                        GameManager.isIndigoEnabled = false;
                        GameManager.isBlueEnabled = false;
                        GameManager.isRainbowEnabled = false;
                        GameManager.GetInstance().EndGame();
                        Score.doNotScore = true;
                    }
                    GameManager.SetHighscore();
                    break;

                case "red":
                    //slow down player
                    RedBlock(collision);
                    break;

                case "yellow":
                    YellowBlock(collision);
                    break;

                case "rainbow":
                    RainbowBlock(collision);
                    break;

                case "green":
                    GreenBlock(collision);
                    break;

                case "indigo":
                    IndigoBlock(collision);
                    break;

                case "blue":
                    BlueBlock(collision);
                    break;

                case "orange":
                    OrangeBlock(collision);
                    break;

                case "purple":
                    PurpleBlock(collision);
                    break;

                default:
                    //FindObjectOfType is not very performant and safe, it should be replaced
                    //Game Over
                    FindObjectOfType<GameManager>().EndGame();
                    Score.doNotScore = true;
                    break;
            }
        }
    }

    private void PickBlock()
    {
        //7/10 white block - 3/10 colored block
        int random = (int)Random.Range(1f, 10f);
        if (random > 7)
        {
            color = "white";
            // 3/22 every color except for rainbow (1/22)
            int randomColor = (int)Random.Range(1f, 22f);

            if (randomColor == 1 && GameManager.isRainbowEnabled) color = "rainbow";
            else if (randomColor >= 2 && randomColor <= 4 && GameManager.isRedEnabled) color = "red";
            else if (randomColor >= 5 && randomColor <= 7 && GameManager.isYellowEnabled) color = "yellow";
            else if (randomColor >= 8 && randomColor <= 10 && GameManager.isIndigoEnabled) color = "indigo";
            else if (randomColor >= 11 && randomColor <= 13 && GameManager.isPurpleEnabled) color = "purple";
            else if (randomColor >= 14 && randomColor <= 16 && GameManager.isBlueEnabled) color = "blue";
            else if (randomColor >= 17 && randomColor <= 19 && GameManager.isOrangeEnabled) color = "orange";
            else if (randomColor >= 20 && randomColor <= 22 && GameManager.isGreenEnabled) color = "green";
        }

        switch(color)  
        {
            //high probability white
            case "white":
                color = "white";
                GetComponent<SpriteRenderer>().color = whiteColor;
                break;

            case "red":
                color = "red";
                GetComponent<SpriteRenderer>().color = redColor;
                break;

            case "yellow":
                color = "yellow";
                GetComponent<SpriteRenderer>().color = yellowColor;
                break;

            case "green":
                color = "green";
                GetComponent<SpriteRenderer>().color = greenColor;
                break;

            case "blue":
                color = "blue";
                GetComponent<SpriteRenderer>().color = blueColor;
                break;

            case "indigo":
                color = "indigo";
                GetComponent<SpriteRenderer>().color = indigoColor;
                break;

            case "rainbow":
                color = "rainbow";
                GetComponent<SpriteRenderer>().color = rainbowColor;
                GetComponent<TrailRenderer>().enabled = true;
                float trailWidth = 0.15f * transform.localScale.x;
                GetComponent<TrailRenderer>().widthMultiplier = 1;
				GetComponent<TrailRenderer>().widthCurve = AnimationCurve.Linear(0f, trailWidth, 1f, trailWidth);
				break;

            case "orange":
                color = "orange";
                GetComponent<SpriteRenderer>().color = orangeColor;
                break;

            case "purple":
                color = "purple";
                GetComponent<SpriteRenderer>().color = purpleColor;
                break;

            default:
                color = "white";
                GetComponent<SpriteRenderer>().color = whiteColor;
                break;
        }
    }

    //Indigo Block: Fast Motion
    private void IndigoBlock(Collision2D collision) 
    {
        sounds[3].Play();
        GetComponent<Collider2D>().enabled = false;
		foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;
		{
            Debug.Log("IndigoBlock: speed up time");
            GameManager.GetInstance().StartFastMotion();
        }
    }
    //Orange Block: Slow Motion
    private void OrangeBlock(Collision2D collision) 
    {
        sounds[3].Play();
        GetComponent<Collider2D>().enabled = false;
		foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;
		{
            Debug.Log("OrangeBlock: slow down time");
            GameManager.GetInstance().StartSlowMotion();
        }
    }

    //Rainbow Block: Invincibility
    private void RainbowBlock(Collision2D collision) 
    {
        sounds[3].Play();
        Debug.Log("RainbowBlock: invincible");
		foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;
        Player.GetInstance().MakeMeInvincible();
    }

    //Green Block: Invert controls
    private void GreenBlock(Collision2D collision) 
    {
        sounds[3].Play();
        Debug.Log("GreenBlock: inverted controls");
		foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;
		Player.GetInstance().InvertControls();
    }

	//Purple Block: Size Increase
	private void PurpleBlock(Collision2D collision)
	{
        sounds[3].Play();
        foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;

        //Increasing Size
        if (Player.scale < 5.5)
		{
            if (sizeLock == false)
            {
				Debug.Log("PurpleBlock: increasing size");
				StartCoroutine(LockSize());
                Player.scale += 0.5f;
				float trailWidth = 0.185f * Player.scale;
                Player.trail.widthCurve = AnimationCurve.Linear(0f, trailWidth, 1f, trailWidth);
			}
		}
		collision.gameObject.GetComponent<Transform>().localScale = new Vector3(Player.scale, Player.scale, 1);
	}

	//Blue Block: Size Decrease
	private void BlueBlock(Collision2D collision)
	{
        sounds[3].Play();
        foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;

        //Decreasing Size
        if (Player.scale > 1f)
		{
			if (sizeLock == false)
			{
				Debug.Log("BlueBlock: decreasing size");
				StartCoroutine(LockSize());
				Player.scale -= 0.5f;
                //equation: widthCurve = 0.185 * scale
                float trailWidth = 0.185f * Player.scale;
                Player.trail.widthCurve = AnimationCurve.Linear(0f, trailWidth, 1f, trailWidth);
			}
		}
		collision.gameObject.GetComponent<Transform>().localScale = new Vector3(Player.scale, Player.scale, 1);
	}

	//Red Block: Speed Decrease
	private void RedBlock(Collision2D collision)
	{
        sounds[3].Play();
        //Red Block: Decrease Player speed of a factor X
        foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;
		if (Player.speed >= 3f) 	
        {
			if (speedLock == false)
			{
				Debug.Log("RedBlock: decreasing speed");
				StartCoroutine(LockSpeed());
				Player.speed -= 0.5f;
			}
        }
	}
	//Yellow Block: Speed Increase
	private void YellowBlock(Collision2D collision)
	{
        sounds[3].Play();
        //Yellow Block: Increase Player speed of a factor X
        foreach (Renderer s in GetComponentsInChildren<Renderer>()) s.enabled = false;
		if (Player.speed <= 10f) 
        {
            if (speedLock == false)
            {
				Debug.Log("YellowBlock: increasing speed");
				StartCoroutine(LockSpeed());
                Player.speed += 0.5f;
			}
        }
	}

	bool sizeLock = false;
	bool speedLock = false;

	IEnumerator LockSize() {
        sizeLock = true;
        GetComponents<Collider2D>()[0].enabled = false;
        Debug.Log("LockSize Acquired");
        yield return new WaitForSeconds(1f);
        Debug.Log("LockSize Released");
        sizeLock = false;
		GetComponents<Collider2D>()[0].enabled = true;
	}

    IEnumerator LockSpeed() {
        speedLock = true;
		GetComponents<Collider2D>()[0].enabled = false;
		Debug.Log("LockSpeed Acquired");
		yield return new WaitForSeconds(1f);
		Debug.Log("LockSpeed Released");
		speedLock = false;
		GetComponents<Collider2D>()[0].enabled = true;
	}
}
