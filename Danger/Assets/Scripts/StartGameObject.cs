using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DangerGameObject : MonoBehaviour
{
    void Start()
    {
        GameManager.Ready += GameReady;
    }

    void GameReady(object sender, GameManager.ReadyEventArgs e)
    {
        GameManager.Ready -= GameReady;
        Startup();
    }
    /// <summary>
    /// Only triggered when the GameManager has finished
    /// </summary>
    public abstract void Startup();
}
