using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    FighterBase _player1;
    FighterBase _player2;
    public static GameManager Instance { get; private set; }
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<FighterBase> OnPlayer1Changed;
    public static event Action<FighterBase> OnPlayer2Changed;


    public GameState CurrentGameState { get; private set; }

    [SerializeField] private FighterBase[] _fighters;

    public FighterBase[] Fighters => _fighters;
    public FighterBase Player1 => _player1;
    public FighterBase Player2 => _player2;


    public void SetPlayer1(FighterBase player1)
    {
        if (_player1 != player1)
        {
            _player1 = player1;
            OnPlayer1Changed?.Invoke(_player1);
        }
    }

    public void SetPlayer2(FighterBase player2)
    {
        if (_player2 != player2)
        {
            _player2 = player2;
            OnPlayer2Changed?.Invoke(_player2);
        }
    }


    public void SetGameState(GameState gameState)
    {
        CurrentGameState = gameState;
        OnGameStateChanged?.Invoke(CurrentGameState);
    }

    public void Reset()
    {
        SetPlayer1(null);
        SetPlayer2(null);
        SetGameState(GameState.MainMenu);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}

public enum GameState
{
    MainMenu,
    SelectFighter,
    InGame,
    GameOver,
    Victory
}