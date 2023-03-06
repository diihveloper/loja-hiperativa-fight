using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Transform _player1Position;

    [SerializeField] private Transform _player2Position;
    private FighterBase _player1;
    private FighterBase _player2;

    [SerializeField] private HudPlayer _hudPlayer1;
    [SerializeField] private HudPlayer _hudPlayer2;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _winnerText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _remainingTime = 60f;
    private bool _gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.Player1 == null || GameManager.Instance.Player2 == null)
        {
            Debug.LogError("Player 1 or Player 2 is null");
            SceneManager.LoadScene("SelectFighters");
            return;
        }

        _remainingTime = 60f;
        SetupPlayers();
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SelectFighters");
    }

    void SetupPlayers()
    {
        _player1 = Instantiate(GameManager.Instance.Player1, _player1Position.position, Quaternion.identity);
        _player2 = Instantiate(GameManager.Instance.Player2, _player2Position.position, Quaternion.identity);

        _player1.SetEnemy(_player2.gameObject);
        _player2.SetEnemy(_player1.gameObject);

        _player1.gameObject.AddComponent<PlayerController>();
        _player2.gameObject.AddComponent<IaController>();

        _hudPlayer1.SetPlayer(_player1);
        _hudPlayer2.SetPlayer(_player2);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (!_gameOver)
        {
            _remainingTime = Mathf.Max(_remainingTime - Time.deltaTime, 0);
            _timerText.text = _remainingTime.ToString("0");

            if (HasWinner())
            {
                var winner = GetWinner();
                FinishGame(winner);
            }
        }
    }

    bool HasWinner()
    {
        if (_remainingTime <= 0)
        {
            return true;
        }

        return _player1.IsDead || _player2.IsDead;
    }

    FighterBase GetWinner()
    {
        if (_player1.IsDead)
        {
            return _player2;
        }

        if (_player2.IsDead)
        {
            return _player1;
        }

        return _player1.Health > _player2.Health ? _player1 : _player2;
    }

    void FinishGame(FighterBase winner)
    {
        _gameOverPanel.SetActive(true);
        _winnerText.text = winner.FighterSO.Name + " Venceu!";
        _gameOver = true;
    }
}