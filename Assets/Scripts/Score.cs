using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public static int score = 0;
    public static bool doNotScore = false;
    private Text scoreText;


    // Use this for initialization
    void Start () {
        score = 0;
        scoreText = gameObject.GetComponent<Text>();
        scoreText.text = "";
    }

    private void Update()
    {
        if (score != 0) UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = score.ToString();
    }
}
