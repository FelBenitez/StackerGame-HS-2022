using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scorer : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI beginText;
    public int counter;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
    }

    // Update is called once per frame
    public void increaseScore(int amount)
    {
        counter =amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        ScoreText.text = ""+counter;
    }

    public void StartText()
    {
        ScoreText.text = "Stacker";
    }

    public void startBeginText()
    {
        beginText.text = "Press S to begin";
    }

    public void deleteBegin()
    {
        beginText.text = " ";
    }

    public void playAgainText()
    {
        ScoreText.text = "Play again?";
        beginText.text = "Press A";
    }
}
