using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour {


    public Text PlayerNum;
    private int PNumber {  get
        {
            return int.Parse(PlayerNum.text.Substring(1));
        } }
    public InputField PlayerField;
    public bool hasEnteredName;
    public string chosen;
    public string ChosenPlayerName {
        get
        {
            if (!hasEnteredName)
                return "";
            return PlayerField.text;
        } }

    void EnsureNoNulls()
    {
        if (PlayerNum == null)
            PlayerNum = this.gameObject.GetComponentInChildren<Text>();
        if (PlayerField == null)
            PlayerField = this.gameObject.GetComponentInChildren<InputField>();
    }


    public void Entered()
    {
        hasEnteredName = true;
        PlayerManager.SetChosenPlayer(PNumber, ChosenPlayerName);
    }

    // Use this for initialization
    void Start () {
        EnsureNoNulls();
        //PlayerNum.text = "P#";
	}

    public void Init(int number)
    {
        hasEnteredName = false;
        chosen = "";
        EnsureNoNulls();
        PlayerNum.text = "P" + number.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
