using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    #region Singleton

    public static GameCore Instance;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetupNeighbours()
    {
        List<Tile> tileList = FindObjectsOfType<Tile>().ToList();
        tileList.ForEach(x => x.DetectNeighbours());
    }

    #endregion

    #region Variables

    public PlayerController Player;

    #endregion
}
