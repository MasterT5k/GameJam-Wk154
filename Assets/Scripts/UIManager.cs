using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UI Manager is NULL");
            }
            return _instance;
        }
    }

    [SerializeField]
    private Text _selectText = null;
    [SerializeField]
    private Text _chosenText = null;
    [SerializeField]
    private GameObject _pausePanel = null;
    [SerializeField]
    private GameObject _winPanel = null;
    [SerializeField]
    private Text _deathText = null;
    [SerializeField]
    private AudioClip _buttonClick = null;
    [SerializeField]
    private Button[] _buttons = null;
    private GameObject _buttonGroup;
    private int _selectedType = -1;

    public static event Action<int> onButtonClick;

    private void OnEnable()
    {
        Player.onRestart += ResetButtons;
    }

    private void OnDisable()
    {
        Player.onRestart -= ResetButtons;
    }

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _pausePanel.SetActive(false);
        _winPanel.SetActive(false);
        _selectText.gameObject.SetActive(false);
        _chosenText.gameObject.SetActive(false);
        _buttonGroup = _buttons[0].transform.parent.gameObject;
        if (_buttonGroup != null)
        {
            _buttonGroup.SetActive(false);
        }
    }

    public void ButtonClick(int selectedType)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (i == selectedType)
            {
                _buttons[i].image.color = Color.red;
            }
            _buttons[i].interactable = false;
        }
        _selectText.gameObject.SetActive(false);
        _chosenText.gameObject.SetActive(true);
        onButtonClick?.Invoke(selectedType);
        AudioManager.Instance.PlayAudioClipOnce(_buttonClick);
    }

    public void ResetButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = true;
            _buttons[i].image.color = Color.white;
        }
        _chosenText.gameObject.SetActive(false);
        _selectedType = -1;
    }

    public void ActivateSelectionObjs(bool activate)
    {
        if (activate == true)
        {
            if (_selectedType > -1 && _selectedType < 3)
            {
                _chosenText.gameObject.SetActive(true);
            }
            else
            {
                _selectText.gameObject.SetActive(true);
            }
            _buttonGroup.SetActive(true);
        }
        else
        {
            _chosenText.gameObject.SetActive(false);
            _selectText.gameObject.SetActive(false);
            _buttonGroup.SetActive(false);
        }
    }

    public void YouWin()
    {
        _winPanel.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        //EditorApplication.ExitPlaymode();
    }

    public void SetDeathsText(int deaths)
    {
        _deathText.text = "Deaths: " + deaths;
    }

    public void PuaseMenu(bool pause)
    {
        if (pause == true)
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
