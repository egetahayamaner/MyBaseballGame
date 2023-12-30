/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameScore : MonoBehaviour
{
    public GameObject scoreText;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTheText();
    }
    public void UpdateTheText()
    {
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("Score", 0).ToString();
    }
    public void BlueTeamScores()
    {
        int x = PlayerPrefs.GetInt("BlueScore", 0);
        x++;
        PlayerPrefs.SetInt("BlueScore", x);
        scoreText.text = "Blue Team " + PlayerPrefs.GetInt("BlueScore", 0).ToString();
    }
    public void RedTeamScores()
    {
        int x = PlayerPrefs.GetInt("RedScore", 0);
        x++;
        PlayerPrefs.SetInt("RedScore", x);
        scoreText.text = "Red Team " + PlayerPrefs.GetInt("RedScore", 0).ToString();
    }
}
*/