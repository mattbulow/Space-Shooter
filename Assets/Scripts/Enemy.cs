using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    private SpawnManager _spawnManager;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSourceExplosion;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null ) { Debug.LogError("Enemy Script could not find SpawnManager script."); }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_spawnManager == null) { Debug.LogError("Enemy Script could not find Player script."); }
        _animator = this.GetComponent<Animator>();
        if (_spawnManager == null) { Debug.LogError("Enemy Script could not find animation component on self."); }
        _audioSourceExplosion = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        if (_spawnManager == null) { Debug.LogError("Enemy Script could not find AudioSource component on self."); }
    }

    // Update is called once per frame
    void Update()
    {

        // move enemy down
        this.transform.Translate(new Vector3(0, -_speed * Time.deltaTime, 0));

        // when enemy reaches lower bound, respawn at top of screen
        if (this.transform.position.y < -5.5)
        {
            this.transform.position = new Vector3(this.transform.position.x, _spawnManager.getEnemySpawnYPos(), 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy hit by: "+ other.gameObject.name);
        switch (other.tag)
        {
            
            // if Enemy collides with Player, destroy the Enemy
            case "Player":
                // damage player
                if (_player != null) { _player.takeLife(); }
                destroyEnemy();
                break;
            // if Enemy collides with laser, destroy the Enemy and laser
            case "Laser":
                Destroy(other.gameObject);
                if (_player != null)
                {
                    // Add to Player score
                    _player.AddScore(10);
                }
                destroyEnemy();
                break;
            default:
                break;
        }
    }

    void destroyEnemy()
    {
        _animator.SetTrigger("OnEnemyDeath");
        // need to wait unitl animation is finished before destroying self
        _speed = 0f;
        GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject,3f);
        _audioSourceExplosion.Play();
    }

}

