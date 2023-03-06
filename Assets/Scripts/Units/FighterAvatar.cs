using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FighterAvatar : MonoBehaviour
{
    private FighterBase _fighter;
    private FighterSO _fighterSo;
    [SerializeField] private Image _avatar;
    [SerializeField] private TextMeshProUGUI _name;
    private Outline _outline;
    private bool _isSelected;
    public FighterBase Fighter => _fighter;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void SetHighlightColor()
    {
        _outline.effectColor =
            _isSelected ? Color.green : GameManager.Instance.Player1 != null ? Color.red : Color.blue;
    }

    public void Highlight()
    {
        _outline.enabled = true;
        SetHighlightColor();
    }

    public void Unhighlight()
    {
        _outline.enabled = false || _isSelected;
    }

    public void Select()
    {
        if (GameManager.Instance.Player1 == null)
        {
            GameManager.Instance.SetPlayer1(_fighter);
            _isSelected = true;
            Highlight();
        }
        else if (GameManager.Instance.Player2 == null)
        {
            GameManager.Instance.SetPlayer2(_fighter);
            _isSelected = true;
            Highlight();
        }
    }

    public void Unselect()
    {
        if (GameManager.Instance.Player1 == _fighter)
        {
            GameManager.Instance.SetPlayer1(null);
        }
        else if (GameManager.Instance.Player2 == _fighter)
        {
            GameManager.Instance.SetPlayer2(null);
        }

        _isSelected = false;
        Unhighlight();
    }

    public void SetFighter(FighterBase fighter)
    {
        _fighter = fighter;
        _fighterSo = fighter.FighterSO;
        _name.text = _fighterSo.Name;
        _avatar.sprite = _fighterSo.Avatar;
    }
}