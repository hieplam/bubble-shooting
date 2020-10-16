using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject ballPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (playerPrefab == null )
        {
            Debug.Log("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.Log($"We are Instantiating LocalPlayer from {Application.loadedLevelName}");
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, -4.4f, 0f), Quaternion.identity, 0);
            PhotonNetwork.Instantiate(this.ballPrefab.name, new Vector3(2f, 3f, 0f), Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
