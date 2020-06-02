using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour {
    public GameObject error1;
    public GameObject error2;
    public GameObject winningTicket;
    public GameObject lostTicket;
    public GameObject checkedTicket;
    public GameObject ticketID;
    public TextMeshProUGUI ticketIDText;
    public TextMeshProUGUI creditText;
    public DatabaseManager databaseManager;

    public void startRound() {
        databaseManager.CloseConnection();
        SceneManager.LoadScene("MainScene");
    }

    public void endRound() {
        databaseManager.CloseConnection();
        SceneManager.LoadScene("PreGameScene");
    }

    public void changeCredit(int credit) {
        creditText.text = credit.ToString();
    }

    public void displayError1() {
        error1.SetActive(true);
    }

    public void displayError2() {
        error2.SetActive(true);
    }

    public void displayWinningTicket() {
        winningTicket.SetActive(true);
    }

    public void displayLostTicket() {
        lostTicket.SetActive(true);
    }

    public void displayCheckedTicket() {
        checkedTicket.SetActive(true);
    }

    public void displayTicketID(int id) {
        ticketID.SetActive(true);
        ticketIDText.text = "Vas ID tiketa je " + id.ToString();
    }
}