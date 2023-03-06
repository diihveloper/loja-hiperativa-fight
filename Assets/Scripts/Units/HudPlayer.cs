using System;
using TMPro;
using UnityEngine;

public class HudPlayer : MonoBehaviour
{
    [SerializeField] private FighterBase _fighterBase;

    [SerializeField] private RectTransform _healthBar;
    [SerializeField] private TextMeshProUGUI _nameText;
    private bool _hasPlayer = false;

    void FixedUpdate()
    {
        _healthBar.localScale = new Vector3(Math.Max(_fighterBase.Health / 100f, 0f), 1, 1);
    }

    public void SetPlayer(FighterBase player)
    {
        _hasPlayer = player != null;
        _fighterBase = player;
        _nameText.text = _hasPlayer ? _fighterBase.FighterSO.Name : "Sem Jogador";
    }
}