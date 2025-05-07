using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemies= new List<GameObject>();
    [SerializeField] private Transform player;

    public Transform Player => player;

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0)
        {
            enemies.RemoveAll(item => item == null);
        }
    }
}
