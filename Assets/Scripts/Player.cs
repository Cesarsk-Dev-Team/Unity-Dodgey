using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public variables
    public static float scale = 3;
    public static float speed = 6f;
    public static float mapWidth = 2.36f;
    public static bool isInvincible = false;
    public static TrailRenderer trail;

    //private variables
    private Rigidbody2D rb;

    public static Player instance;
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public static Player GetInstance()
    {
        return instance;
    }

    void Start()
    {
        scale = 3;
        transform.localScale = new Vector3(scale,scale,1);
        speed = 6f;
        isInvincible = false;
        rb = GetComponent<Rigidbody2D>();
        Score.doNotScore = false;
        trail = GetComponent<TrailRenderer>();
        trail.sortingLayerName = "Background";
        trail.sortingOrder = 2;
	}

    void FixedUpdate()
    {
        //Touch Controls
        if (LeftInput.moveLeft && !RightInput.moveRight) MovePlayerToLeft();
        if (!LeftInput.moveLeft && RightInput.moveRight) MovePlayerToRight();

        //Keyboard Controls (to enable touch controls comment this statement)
        //MovePlayerWithKeyboard();
    }

    public static bool invertedControls = false;

    private void MovePlayerWithKeyboard()
    {
        float x = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * speed;
        if (invertedControls) x *= -1;
        Vector2 newPosition = rb.position + Vector2.right * x;
        newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);
        rb.MovePosition(newPosition);
    }

    private void MovePlayerToLeft()
    {
        //it runs on a fixed timer, and physics likes to do that
        float x = Time.fixedDeltaTime * speed;
        Vector2 newPosition = rb.position + Vector2.right * -Mathf.Abs(x);
        if (invertedControls) newPosition = rb.position + Vector2.right * Mathf.Abs(x);
        newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);
        rb.MovePosition(newPosition);
    }

    private void MovePlayerToRight()
    {
        //it runs on a fixed timer, and physics likes to do that
        float x = Time.fixedDeltaTime * speed;
        Vector2 newPosition = rb.position + Vector2.right * Mathf.Abs(x);
        if (invertedControls) newPosition = rb.position + Vector2.right * -Mathf.Abs(x);
        newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);
		rb.MovePosition(newPosition);
	}

    public void InvertControls()
    {
        StartCoroutine(InvertedControls());
    }

	public void MakeMeInvincible()
	{
        StartCoroutine(InvincibleTime());
	}

	public IEnumerator InvertedControls()
    {
        Player.invertedControls = true;
        Debug.Log("Controls Inverted");
        yield return new WaitForSeconds(5f);
        Debug.Log("Controls Restored");
        Player.invertedControls = false;
    }

	public IEnumerator InvincibleTime()
	{
        Debug.Log("You're invincible");
		//make me invincible
		Player.isInvincible = true;
		yield return new WaitForSeconds(5f);
		//make me vulnerable
		Player.isInvincible = false;
		Debug.Log("You're not invincible anymore");
	}
}
