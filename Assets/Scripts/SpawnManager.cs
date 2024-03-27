using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3 _spawnPosition;

    [SerializeField] private float _enemySpawnTime = 4f;
    [SerializeField] private float _spawnYPos = 6f; 
    [SerializeField] private float _spawnXBounds = 8f;

    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private GameObject _powerUpContainer;
    [SerializeField] private Player _player;

    public float getEnemySpawnYPos() {  return _spawnYPos; }

    // Start is called before the first frame update
    void Start()
    {
        _spawnPosition.y = _spawnYPos;
    }

    public void StartSpawning()
    {
        StartCoroutine(spawnEnemys());
        StartCoroutine(spawnPowerUps());
    }

    IEnumerator spawnEnemys()
    {
        // spawn enemys every 5 seconds, use a coroutine
        while (!_player.IsDead())
        {
            _spawnPosition.x = Random.Range(-_spawnXBounds, _spawnXBounds);
            GameObject enemy = Instantiate(_enemyPrefab, _spawnPosition, Quaternion.identity);     
            enemy.transform.parent = _enemyContainer.transform;
            //enemy.transform.parent = this.transform.transform.Find("EnemyContainer");

            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    IEnumerator spawnPowerUps()
    {
        while (!_player.IsDead()) 
        {
            //spawn random power up every 3-7 sec
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            _spawnPosition.x = Random.Range(-_spawnXBounds, _spawnXBounds);
            GameObject powerup = Instantiate(_powerUps[Random.Range(0,3)], _spawnPosition, Quaternion.identity);
            powerup.transform.parent = _powerUpContainer.transform;
        }

    }


}
