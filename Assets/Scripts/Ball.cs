using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    public float speed = 30;
    private Rigidbody2D rigidBody;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.right * speed;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        //Left or Right Paddle
        if((collision.gameObject.name == "LeftPaddle") || 
            (collision.gameObject.name == "RightPaddle"))
        {
            HandlePaddleHit(collision);
        }

        //Wall Bottom or Top
        if ((collision.gameObject.name == "WallBottom") ||
            (collision.gameObject.name == "WallTop"))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.wallBloop);
        }

        //Left or Right Goal
        if ((collision.gameObject.name == "LeftGoal") ||
            (collision.gameObject.name == "RightGoal"))
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.goalBloop);

            //Update Score UI
            if(collision.gameObject.name == "LeftGoal")
            {
                IncreaseTextUIScore("RightScoreUI");
            }
            if (collision.gameObject.name == "RightGoal")
            {
                IncreaseTextUIScore("LeftScoreUI");
            }

            transform.position = new Vector2(0, 0);
        }

    }

    void HandlePaddleHit(Collision2D collision)
    {
        float y = BallHitPaddleWhere(transform.position,
            collision.transform.position,
            collision.collider.bounds.size.y);

        Vector2 dir = new Vector2();

        if(collision.gameObject.name == "LeftPaddle")
        {
            dir = new Vector2(1, y).normalized;
        }
        if (collision.gameObject.name == "RightPaddle")
        {
            dir = new Vector2(-1, y).normalized;
        }

        rigidBody.velocity = dir * speed;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.hitPaddleBloop);
    }

    float BallHitPaddleWhere(Vector2 ball, Vector2 paddle,  float paddleHeight)
    {
        return (ball.y - paddle.y) / paddleHeight;
    }

    void IncreaseTextUIScore(string textUIName)
    {
        var textUIComp = GameObject.Find(textUIName).GetComponent<Text>();

        int score = int.Parse(textUIComp.text);
        score++;
        textUIComp.text = score.ToString();
    }
}
