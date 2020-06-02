using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour {
    public UIManager UImanager;
    public int credit = 0;

    public void add(int value) {
        int temp = credit * 10 + value;

        if(temp <= 400) {
            credit = temp;
            UImanager.changeCredit(credit);
        } else {
            UImanager.displayError2();
        }
    }

    public void deleteCredit() {
        credit = 0;
        UImanager.changeCredit(credit);
    }
}
