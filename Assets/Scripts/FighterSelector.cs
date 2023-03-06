using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FighterSelector : MonoBehaviour
{
    [SerializeField] private GameObject _fighterAvatarPrefab;
    [SerializeField] private Transform _p1Position;
    [SerializeField] private Transform _p2Position;

    private List<FighterAvatar> _fighterAvatars;
    int _selectedFighterIndex = 0;

    public void SelectNextFighter()
    {
        _fighterAvatars[_selectedFighterIndex].Unhighlight();
        _selectedFighterIndex++;
        if (_selectedFighterIndex >= _fighterAvatars.Count)
        {
            _selectedFighterIndex = 0;
        }

        _fighterAvatars[_selectedFighterIndex].Highlight();
    }

    public void SelectPreviousFighter()
    {
        _fighterAvatars[_selectedFighterIndex].Unhighlight();
        _selectedFighterIndex--;
        if (_selectedFighterIndex < 0)
        {
            _selectedFighterIndex = _fighterAvatars.Count - 1;
        }

        _fighterAvatars[_selectedFighterIndex].Highlight();
    }

    public void SelectAboveFighter()
    {
        _fighterAvatars[_selectedFighterIndex].Unhighlight();
        _selectedFighterIndex -= 3;
        if (_selectedFighterIndex < 0)
        {
            _selectedFighterIndex = _fighterAvatars.Count - 1;
        }

        _fighterAvatars[_selectedFighterIndex].Highlight();
    }

    public void SelectBelowFighter()
    {
        _fighterAvatars[_selectedFighterIndex].Unhighlight();
        _selectedFighterIndex += 3;
        if (_selectedFighterIndex >= _fighterAvatars.Count)
        {
            _selectedFighterIndex = 0;
        }

        _fighterAvatars[_selectedFighterIndex].Highlight();
    }

    public void SelectFighter()
    {
        _fighterAvatars[_selectedFighterIndex].Select();
    }

    public void ConfirmSelection()
    {
        if (GameManager.Instance.Player1 == null || GameManager.Instance.Player2 == null)
        {
            SelectFighter();
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void RemoveSelection()
    {
        if (GameManager.Instance.Player2 != null)
        {
            foreach (var fighterAvatar in _fighterAvatars)
            {
                if (fighterAvatar.Fighter == GameManager.Instance.Player2)
                {
                    fighterAvatar.Unselect();
                }
            }
        }
        else if (GameManager.Instance.Player1 != null)
        {
            foreach (var fighterAvatar in _fighterAvatars)
            {
                if (fighterAvatar.Fighter == GameManager.Instance.Player1)
                {
                    fighterAvatar.Unselect();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.OnPlayer1Changed += GameManagerOnOnPlayer1Changed;
        GameManager.OnPlayer2Changed += GameManagerOnOnPlayer2Changed;
        GameManager.Instance.Reset();
        _fighterAvatars = new List<FighterAvatar>();
        foreach (var fighter in GameManager.Instance.Fighters)
        {
            var fighterAvatar = Instantiate(_fighterAvatarPrefab, transform).GetComponent<FighterAvatar>();
            fighterAvatar.SetFighter(fighter);
            _fighterAvatars.Add(fighterAvatar);
        }

        if (_fighterAvatars.Count > 0)
            _fighterAvatars[0].Highlight();
    }


    private void GameManagerOnOnPlayer1Changed(FighterBase obj)
    {
        if (obj != null)
        {
            var fighter = Instantiate(obj, _p1Position);
            fighter.transform.localPosition = Vector3.zero;
        }
        else if (_p1Position.childCount > 0)
        {
            DestroyImmediate(_p1Position.GetChild(0).gameObject);
        }

        UpdateEnimies();
    }

    private void GameManagerOnOnPlayer2Changed(FighterBase obj)
    {
        if (obj != null)
        {
            var fighter = Instantiate(obj, _p2Position);
            fighter.transform.localPosition = Vector3.zero;
        }
        else if (_p2Position.childCount > 0)
        {
            DestroyImmediate(_p2Position.GetChild(0).gameObject);
        }

        UpdateEnimies();
    }

    void UpdateEnimies()
    {
        if (_p1Position.childCount > 0 && _p2Position.childCount > 0)
        {
            var p1 = _p1Position.GetChild(0).GetComponent<FighterBase>();
            var p2 = _p2Position.GetChild(0).GetComponent<FighterBase>();
            p1.SetEnemy(p2.gameObject);
            p2.SetEnemy(p1.gameObject);
        }
        else
        {
            if (_p1Position.childCount > 0)
            {
                var p1 = _p1Position.GetChild(0).GetComponent<FighterBase>();
                p1.SetEnemy(null);
            }

            if (_p2Position.childCount > 0)
            {
                var p2 = _p2Position.GetChild(0).GetComponent<FighterBase>();
                p2.SetEnemy(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // select next fighter
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextFighter();
        }

        // select previous fighter
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousFighter();
        }

        // select above fighter
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectAboveFighter();
        }

        // select below fighter
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectBelowFighter();
        }

        // confirm selection
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelection();
        }

        // remove selection
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveSelection();
        }
    }

    private void OnDestroy()
    {
        GameManager.OnPlayer1Changed -= GameManagerOnOnPlayer1Changed;
        GameManager.OnPlayer2Changed -= GameManagerOnOnPlayer2Changed;
    }
}