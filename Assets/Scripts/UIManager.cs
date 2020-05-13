using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour{
    public GameObject error1;

    public void displayError1() {
        error1.SetActive(true);
    }
}