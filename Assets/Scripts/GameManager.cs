using System;
using System.Globalization;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public GameObject obstaclePrefab;
    public Entity obstacleEntityPrefab;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;

    public float chanceToSpawn = 0.2f;
    public float minSpawnTimeout = 1f;
    public float maxSpawnTimeout = 3f;
    public float minObstacleSpeed = 3f;
    public float maxObstacleSpeed = 12f;
    public float newGameTimeInSeconds = 10f;
    
    public int[] playerScores;
    public Text[] playerText;
    public Text timeText;
    public Text restartText;
    
    public int lastUpdatedTime = -1;
    public bool gameOver = true;

    public float yBound = 14f;
    public float yUpperBoundObstacle = 11f;
    public float yLowerBoundObstacle = -9f;
    public float xBound = 29f;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }

        main = this;
        
        playerScores = new int[2];

        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings =
            GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        obstacleEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(obstaclePrefab, settings);
        
        TimeIsOut();

        Entity restart = entityManager.CreateEntity();
        entityManager.AddComponentData(restart, new WaitForRestartTag());
    }

    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    public void UpdateScoreText()
    {
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerText[i].text = playerScores[i].ToString();
        }
    }

    public void PlayerScored(int id)
    {
        playerScores[id]++;
        UpdateScoreText();
    }

    public void UpdateTime(float time)
    {
        int gotTime = Mathf.FloorToInt(time);
        if (gotTime != lastUpdatedTime && gotTime >= 0)
        {
            lastUpdatedTime = Mathf.FloorToInt(time);
            timeText.text = lastUpdatedTime.ToString();
        }
    }

    public void TimeIsOut()
    {
        gameOver = true;
        restartText.text = "Press 'R' to restart";
    }

    public void Restart()
    {
        gameOver = false;
        restartText.text = "";
        for (int i = 0; i < playerScores.Length; i++)
        {
            playerScores[i] = 0;
        }
        UpdateScoreText();
    }
    
    
}
