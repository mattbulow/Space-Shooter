using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _gameOverText;
    private Color32 _gameOverColor;
    [SerializeField] private Text _restartText;
    [SerializeField] GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameOverColor = Color.white;
        _restartText.gameObject.SetActive(false);
    }

    public void UpdateScore(int new_score)
    {

        _scoreText.text = "Score: " + new_score;
    }

    public void SetLives(int lives)
    {
        _livesImage.GetComponent<Image>().sprite = _livesSprites[Math.Clamp(lives,0,3)];
        if (lives <= 0 ) {
            _gameOverText.gameObject.SetActive(true);
            StartCoroutine(flashGameOver());
            _restartText.gameObject.SetActive(true);
            _gameManager.SetGameOver();
        }
    }

    IEnumerator flashGameOver()
    {
        bool increase = false;
        float flashStep_sec = (1 / 256 / 2);  // (period_sec / 256 steps / 2( for complete period)
        while (true)
        {
            if (_gameOverColor.a == 0) { increase = true; }
            else if (_gameOverColor.a == 255) { increase = false; }

            if (increase) { _gameOverColor.a += 1; }
            else { _gameOverColor.a -= 1; }
            _gameOverText.color = _gameOverColor;

            yield return new WaitForSeconds(flashStep_sec);
        } 
    }
}
