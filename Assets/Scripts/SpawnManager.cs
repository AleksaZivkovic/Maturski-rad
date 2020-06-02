using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public List<GameObject> ballPositions;
    public List<GameObject> balls;
    public List<int> drawnBalls;
    public int numberOfDrawnBalls = 0;
    public DatabaseManager databaseManager;
    public UIManager uiManager;

    void Start() {
        drawnBalls = new List<int>();
        StartCoroutine(drawNext());
    }

    IEnumerator drawNext() {
        yield return new WaitForSeconds(1f);
        drawNextNumber();
    }

    IEnumerator endRound() {
        yield return new WaitForSeconds(5f);
        List<int> jackpot = new List<int>();
        jackpot.Add(drawnBalls[15]);
        jackpot.Add(drawnBalls[18]);
        jackpot.Add(drawnBalls[21]);
        jackpot.Add(drawnBalls[24]);
        jackpot.Add(drawnBalls[34]);

        List<int> stars = new List<int>();
        stars.Add(drawnBalls[Random.Range(6, 35)]);
        stars.Add(drawnBalls[Random.Range(6, 35)]);

        databaseManager.pushRound(drawnBalls, stars, jackpot);
        uiManager.endRound();
    }

    void drawNextNumber() {
        bool chosen = false;

        while(!chosen) {
            bool valid = true;
            int random = Random.Range(1, 49);

            foreach(int i in drawnBalls) {
                if(i == random) {
                    valid = false;
                }
            }

            if(valid) {
                chosen = true;
                drawnBalls.Add(random);
                GameObject ball = Instantiate(balls[random - 1], ballPositions[numberOfDrawnBalls].transform);
                ball.GetComponent<SpriteRenderer>().enabled = true;
                numberOfDrawnBalls++;
            }
        }

        if(numberOfDrawnBalls < 35) {
            StartCoroutine(drawNext());
        } else {
            StartCoroutine(endRound());
        }
    }
}
