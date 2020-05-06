using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour {
    public List<GameObject> balls;
    private List<int> selectedBalls;

    void Start() {
        selectedBalls = new List<int>();
        selectedBalls.Clear();
    }

    void OnApplicationQuit() {
        foreach(int index in selectedBalls) {
            selectedBalls.RemoveAt(selectedBalls.IndexOf(index));
        }
    }

    public void pressed(GameObject ball) {
        int index = getIndex(ball);
        if(!selected(index) && selectedBalls.Count < 6) {
            selectedBalls.Add(index);
            ball.transform.localScale += new Vector3(-0.2f, -0.2f, -0.2f);
        } else if(selected(index)) {
            ball.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            selectedBalls.RemoveAt(selectedBalls.IndexOf(index));
        }
    }

    bool selected(int index) {
        foreach(int selectedBall in selectedBalls) {
            if(selectedBall == index) {
                return true;
            }
        }

        return false;
    }

    int getIndex(GameObject ball) {
        int index = 0;

        if(ball.name.Length == 8) {
            index = (int)ball.name[ball.name.Length - 1] - (int)'0';
        } else {
            index = (int)ball.name[ball.name.Length - 2] - (int)'0';
            index *= 10;
            index += (int)ball.name[ball.name.Length - 1] - (int)'0';
        }

        return index;
    }
}
