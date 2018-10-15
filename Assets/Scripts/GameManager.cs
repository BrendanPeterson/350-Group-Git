using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one gameManager in scene.");
        }
        else
        {
            instance = this;
        }

    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(isActive);
    }

    #region Player Registration

    private const string PLAYER_ID_PREFIX = "Player ";

    //Creates a dictioniary with the IDS of players connected to the network
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    //Gets each players net id and registers it in the dictionary
    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    //removes player from dictionary on disconnect
    //called in PlayerNetworking class
    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    //Gets a specific player with an ID
    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    //Not important just shows Dictionary on  game screen
    /*void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/

    #endregion
}
