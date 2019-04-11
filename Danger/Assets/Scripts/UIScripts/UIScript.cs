using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;
using System.Threading.Tasks;
using TMPro;

public class UIScript : DangerGameObject
{
    static UIScript instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject StagePanel;
    public GameObject PlayerInfoPanel;

    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI CurrentName;
    public TextMeshProUGUI FreeArmies;

    public TextMeshProUGUI CurrentStage;
    public Button Stage1Button;
    public Button Stage2Button;
    public Button Stage3Button;




    private static void HighlightButton(Button obj, GameStage stage)
    {
        obj.colors = new ColorBlock()
        {
            normalColor = Color.green,
            disabledColor = Color.gray,
            pressedColor = Color.green,
            highlightedColor = Color.cyan,
            colorMultiplier = stage == GameManager.CurrentStage ? 1 : 0
        };
    }

    public void MoveToStage(int stage)
    {
        int current = (int)GameManager.CurrentStage;
        if(stage > current)
        { // the stage is next 
            GameManager.CurrentStage = (GameStage)stage;
        } else
        { // stage is not next, they either clicked backwards or.. looped round
            if(GameManager.CurrentStage == GameStage.Movement && ((GameStage)stage) == GameStage.Draft)
            { // they looped round, so start next player.
                GameManager.SwitchToNextPlayer();
            }
        }
        UpdateUI();
    }

    public void StartNextStage()
    {
        Debug.Log(GameManager.NextStage.ToString());
        if(GameManager.NextStage == GameStage.Draft)
        {
            GameManager.SwitchToNextPlayer();
        }
    }

    public override void Startup()
    {
        UpdateUI();
    }

    public static void UpdateUI()
    {
        instance.PlayerInfoPanel.SetActive(GameManager.CurrentStage != GameStage.NotStarted);
        instance.StagePanel.SetActive(GameManager.CurrentStage != GameStage.NotStarted);
        if (GameManager.CurrentStage == GameStage.NotStarted)
            return;
        instance.PlayerName.text = GameManager.CurrentPlayer.Name; // this would be different if multiplayer.
        instance.PlayerName.color = GameManager.CurrentPlayer.PlayerColor;
        instance.CurrentName.text = GameManager.CurrentPlayer.Name;
        instance.FreeArmies.text = GameManager.CurrentPlayer.ArmiesToGive.Count.ToString();
        instance.CurrentStage.text = GameManager.CurrentStage.ToString();
        HighlightButton(instance.Stage1Button, GameStage.Draft);
        HighlightButton(instance.Stage2Button, GameStage.Attack);
        HighlightButton(instance.Stage3Button, GameStage.Movement);
    }
}
