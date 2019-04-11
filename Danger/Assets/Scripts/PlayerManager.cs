using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public GameObject PlayerInputPrefab;
    public GameObject PlayerListParent;

    public static List<Player> Players = new List<Player>();

    private static List<PlayerInput> AwaitingInput = new List<PlayerInput>();

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
	void Update () {
    }

    private static bool started = false;
    private static Dictionary<int, string> names; 
    public static void SetChosenPlayer(int number, string name)
    {
        Debug.Log("Setting " + number.ToString() + " to " + name);
        if(names.ContainsKey(number))
        {
            names[number] = name;
        } else
        {
            names.Add(number, name);
        }
        FinishedSettingName();
    }

    public static void FinishedSettingName()
    {
        int count = names.Values.Where(x => !string.IsNullOrEmpty(x)).Count();
        Debug.Log("Ran, count: " + count.ToString() + ", wanted: " + AwaitingInput.Count.ToString());
        if(count == AwaitingInput.Count)
        {
            if (started == true)
                return;
            started = true;
            Debug.Log("Starting");
            Players = new List<Player>();
            foreach(var keypair in names)
            {
                var nPl = new Player(keypair.Value);
                Players.Add(nPl);
            }
            SceneManager.LoadScene("Main");
            SceneManager.UnloadSceneAsync("Start"); // i'm aware this gives a warning
        }
    }

    public void SetNumPlayers(int value)
    {
        if (value == 0)
            return;
        value++; // value is the index of selected item, so we want +1
        PlayerListParent.SetActive(true);
        while(AwaitingInput.Count > 0)
        {
            var obj = AwaitingInput[0];
            Destroy(obj.gameObject);
            AwaitingInput.Remove(obj);
        }
        AwaitingInput = new List<PlayerInput>();
        names = new Dictionary<int, string>();
        for(int pNum = 0; pNum < value; pNum++)
        {
            var newObj = Instantiate(PlayerInputPrefab, PlayerListParent.transform);
            newObj.name = "Input#" + pNum.ToString();
            var playerInput = newObj.AddComponent<PlayerInput>();
            playerInput.Init(pNum);
            newObj.transform.localPosition = new Vector3(3, gameObject.GetComponent<RectTransform>().position.y - 75 - (35 * pNum));
            AwaitingInput.Add(playerInput);
        }
    }
}
