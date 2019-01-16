using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public int scene; //manually input which scene to go to

    void OnMouseDown()
    {
        SceneManager.LoadScene(scene);
    }
}
