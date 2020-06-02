using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    public TicketManager ticketManager;
    public DatabaseManager databaseManager;
    public Round round;

    void Start() {
        StartCoroutine(setRoundNumber());
    }

    IEnumerator setRoundNumber() {
        yield return new WaitForEndOfFrame();
        round.RoundNumber = databaseManager.getLastRoundNumber();
    }

}