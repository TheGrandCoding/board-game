using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;
using System.Threading.Tasks;

public class UIScript : DangerGameObject
{
    static UIScript instance;
    private void Awake()
    {
        instance = this;
    }
    // Main game UI
    public Text PlayerName;
    public Text UnitsRemain;
    public Text TerritoriesNum;
    public Button StartStage;
    public Button AttackStage;
    public Button RelocateStage;
    public static bool Started;

    static Player Current => GameManager.CurrentPlayer;

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
        if((GameStage)stage == GameStage.First)
        {
            GameManager.SwitchToNextPlayer();
        } else
        {
            GameManager.CurrentStage = (GameStage)stage;
        }
        UpdateUI();
    }

    public void StartNextStage()
    {
        Debug.Log(GameManager.NextStage.ToString());
        if(GameManager.NextStage == GameStage.First)
        {
            GameManager.SwitchToNextPlayer();
        }
    }

    public override void Startup()
    {
        Started = true;
        UpdateUI();
    }

    // Update is called once per frame
    public static void UpdateUI()
    {
        if(Started)
        {
            instance.PlayerName.text = Current.Name;
            instance.UnitsRemain.text = "Units: " + Current.ArmiesToGive.Count.ToString();
            instance.TerritoriesNum.text = "Terr: " +  Current.Territories.Count.ToString();
            HighlightButton(instance.StartStage, GameStage.First);
            HighlightButton(instance.AttackStage, GameStage.Attack);
            HighlightButton(instance.RelocateStage, GameStage.Movement);
        }
    }
}
