using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Private Variables

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedUp = 8f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _laserTriplePrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _engineFire_Right;
    [SerializeField] private GameObject _engineFire_Left;
    [SerializeField] private UIManager _uiManager;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClipLaser;
    [SerializeField] private AudioClip _audioClipPowerUp;


    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField] private int _lives = 3;
    private bool _isDead = false;
    [SerializeField] private bool _isTripleShotActive = false;
    [SerializeField] private float _tripleShotCooldown = 4f;
    [SerializeField] private bool _isSpeedUpActive = false;
    [SerializeField] private float _speedUpCooldown = 4f;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private float _shieldCooldown = 4f;

    
    [SerializeField] private int _score;


    // Public Variables

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) { Debug.LogError("AudioSource Component on Player is NULL."); }
         // Set Player Position to (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _uiManager.SetLives(_lives);

        _engineFire_Right.SetActive(false);
        _engineFire_Left.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();

        fireLaser();
    }

    void fireLaser()
    {
        // create laser when spacebar is pressed
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            // use a cooldown for firing laser
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive)
            {
                // create triple laser
                Instantiate(_laserTriplePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                // create single laser
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            // play laser audio clip
            _audioSource.clip = _audioClipLaser;
            _audioSource.Play();
        }
    }

    void movePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float horizontalBound = 10.5f;
        float horizontalWarpHyst = 0.3f;
        float upperBound = 2f;
        float lowerBound = -3.8f;

        // Dont control position if out of bounds
        // if player is too far right warp to left side of screen
        if (transform.position.x > horizontalBound)
        {
            transform.position = new Vector3(-horizontalBound + horizontalWarpHyst, transform.position.y, 0);
        }
        // if player is too far left warp to right side of screen
        else if (transform.position.x < -horizontalBound)
        {
            transform.position = new Vector3(horizontalBound - horizontalWarpHyst, transform.position.y, 0);
        }

        // if player is too far up, zero out up input
        if (transform.position.y > upperBound && verticalInput > 0)
        {
            verticalInput = 0;
        }
        // if player is too far down, zero out down input
        else if (transform.position.y < lowerBound && verticalInput < 0)
        {
            verticalInput = 0;
        }

        float activeSpeed;
        if (_isSpeedUpActive) { activeSpeed = _speedUp; }
        else { activeSpeed = _speed; }

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * activeSpeed);
    }

    public void takeLife()
    {
        // if shield are active, remove shield and return
        if (_isShieldActive)
        {
            _shield.SetActive(false);
            _isShieldActive = false;
            return;
        }
        _lives--;
        _uiManager.SetLives(_lives);
        // check if dead
        if (_lives == 2)
        {
            _engineFire_Right.SetActive(true);
        } else if (_lives == 1)
        {
            _engineFire_Left.SetActive(true);
        }
        else if (_lives <= 0) 
        {
            _isDead = true;
            Destroy(this.gameObject);
        }
    }
    public bool IsDead() { return _isDead; }

    public void SetTripleShotActive() 
    { 
        _isTripleShotActive = true;
        StartCoroutine(TripleShotActive());
    }
    public void SetSpeedUpActive()
    {
        _isSpeedUpActive = true;
        StartCoroutine(SpeedUpActive());
    }

    public void SetShieldActive()
    {
        _isShieldActive = true;
        // create shield and attach to player as parent
        _shield.SetActive(true);
        StartCoroutine(ShieldActive());
    }
    IEnumerator TripleShotActive()
    {
        yield return new WaitForSeconds(_tripleShotCooldown);
        _isTripleShotActive = false;
    }
    IEnumerator SpeedUpActive()
    {
        yield return new WaitForSeconds(_speedUpCooldown);
        _isSpeedUpActive = false;
    }

    IEnumerator ShieldActive()
    {
        yield return new WaitForSeconds(_shieldCooldown);
        _isShieldActive = false;
        _shield.SetActive(false);
    }

    public void AddScore(int amount)
    {
        _score += amount;
        _uiManager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerUp")
        {
            // play powerup audio clip
            _audioSource.clip = _audioClipPowerUp;
            _audioSource.Play();
        }
    }
}

