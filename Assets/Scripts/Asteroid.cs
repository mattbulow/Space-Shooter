using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 20f;

    [SerializeField] private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private AudioSource _audioSourceExplosion;

    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null) { Debug.LogError("Enemy Script could not find SpawnManager script."); }
        _audioSourceExplosion = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
        if (_spawnManager == null) { Debug.LogError("Enemy Script could not find AudioSource component on self."); }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0,0, _rotationSpeed * Time.deltaTime);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Asteroid hit by: " + collision.gameObject.name);

        if (collision.tag == "Laser")
        {
            GameObject _explosion = Instantiate(_explosionPrefab,this.transform.position,Quaternion.identity);
            _audioSourceExplosion.Play();
            Destroy(collision.gameObject);
            Destroy(this.gameObject,1);
            Destroy(_explosion,2.6f);
            _spawnManager.StartSpawning();
        }
        // create explosion on asteroid (instantiate)
        //destroy explosion after 3sec
    }
}




