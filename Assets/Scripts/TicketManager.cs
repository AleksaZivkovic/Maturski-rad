using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour {
    public List<GameObject> balls;
    private List<int> selectedBalls;
    public DatabaseManager databaseManager;
    public UIManager UImanager;
    public CreditManager creditManager;
    public Round round;

    void Start() {
        selectedBalls = new List<int>();
        selectedBalls.Clear();
        selectedBalls.TrimExcess();
    }

    void OnApplicationQuit() {
        deleteTicket();
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

    public void pushTicket() {
        selectedBalls.TrimExcess();
        if(selectedBalls.Count == 6) {
            Ticket ticket = new Ticket();
            ticket.RoundNumber = round.RoundNumber;
            ticket.Numbers = selectedBalls;
            ticket.Checked = -1;

            if(creditManager.credit > 0) {
                ticket.Chip = creditManager.credit;
            } else {
                UImanager.displayError2();
                return;
            }

            databaseManager.pushTicket(ticket);

            foreach(GameObject ball in balls) {
                int index = getIndex(ball);

                foreach(int selected in selectedBalls)
                    if(selected == index) {
                        ball.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                    }
            }

            selectedBalls.Clear();
            selectedBalls.TrimExcess();
            UImanager.displayTicketID(databaseManager.getLastTicketID());
        } else {
            UImanager.displayError1();
        }
    }

    public void deleteTicket() {
        foreach(GameObject ball in balls) {
            int index = getIndex(ball);

            foreach(int selected in selectedBalls)
                if(selected == index) {
                    ball.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
                }
        }

        selectedBalls.Clear();
        selectedBalls.TrimExcess();
    }

    public void checkTicket() {
        databaseManager.checkTicketByID(creditManager.credit);
    }
}