using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Challenge : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _platforms = null;
    [SerializeField]
    private AudioClip _loseClip = null;
    [SerializeField]
    private AudioClip _supspenceClip = null;
    [SerializeField]
    private AudioClip _winClip = null;
    private Collider2D _collider;
    private int _neededType = -1;
    private bool _typeSelected = false;
    private bool _isTypeCorrect = false;

    private void OnEnable()
    {
        Player.onCheckChallenge += OnSelectType;
        Player.onRestart += OnRestart;
    }

    private void OnDisable()
    {
        Player.onCheckChallenge -= OnSelectType;
        Player.onRestart -= OnRestart;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _platforms.Length; i++)
        {
            _platforms[i].SetActive(false);
        }

        _neededType = Random.Range(0, 3);

        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogError("Collider is NULL");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.SetCurrentChallenge(this);
                AudioManager.Instance.PlayAudioClipOnce(_supspenceClip);
            }
            UIManager.Instance.ActivateSelectionObjs(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isTypeCorrect == true && _typeSelected == true)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.SetCurrentChallenge(null);
                    UIManager.Instance.ResetButtons();
                    Debug.Log("Challenge Passed");
                    AudioManager.Instance.PlayAudioClipOnce(_winClip);
                    _collider.enabled = false;

                    for (int i = 0; i < _platforms.Length; i++)
                    {
                        _platforms[i].SetActive(true);
                    }
                }
            }
            else if (_isTypeCorrect == false && _typeSelected == true)
            {
                other.GetComponent<Player>().GameOver(false);
                AudioManager.Instance.PlayAudioClipOnce(_loseClip);
            }

            UIManager.Instance.ActivateSelectionObjs(false);
        }
    }

    public void OnSelectType(int selectedType, Challenge challenge)
    {
        if (challenge == this)
        {
            _typeSelected = true;

            if (_neededType == selectedType)
            {
                _isTypeCorrect = true;
            }
            else
            {
                Debug.Log("You Picked Wrong!");
            }
        }
    }

    public void OnRestart()
    {
        for (int i = 0; i < _platforms.Length; i++)
        {
            _platforms[i].SetActive(false);
        }
        _typeSelected = false;
        _isTypeCorrect = false;
        _collider.enabled = true;
    }
}
