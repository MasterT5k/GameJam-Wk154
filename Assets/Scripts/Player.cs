using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int _selectedType = -1;
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _jumpHeight = 20f;
    private Rigidbody2D _rigid;
    [SerializeField]
    private LayerMask _groundLayer = -1;
    private Challenge _currentChallenge;
    private bool _grounded;
    private bool _resetJumpNeeded;
    private bool _gameOver;
    [SerializeField]
    private int _deaths;
    private Vector3 _startPos;
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;

    public static event Action<int, Challenge> onCheckChallenge;
    public static event Action onRestart;

    private void OnEnable()
    {
        UIManager.onButtonClick += SetRoShamBo;
    }

    private void OnDisable()
    {
        UIManager.onButtonClick -= SetRoShamBo;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _startPos = transform.position;

        if (_rigid == null)
        {
            Debug.LogError("Rigidbody2D is NULL");
        }

        if (_playerAnim == null)
        {
            Debug.LogError("PlayerAnimation is NULL");
        }

        if (_playerSprite == null)
        {
            Debug.LogError("Player Sprite Renderer is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameOver == false)
        {
            Movement();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.PuaseMenu(true);
            }
        }
        else
        {
            _rigid.velocity = new Vector3(0, _rigid.velocity.y);
        }
    }

    void Movement()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (move > 0)
        {
            Flip(true);
        }
        else if (move < 0)
        {
            Flip(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _grounded = Grounded();
            if (_grounded == true)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, _jumpHeight);
                _resetJumpNeeded = true;
                _playerAnim.Jumping(true);
                StartCoroutine(ResetJumpRoutine());
            }
        }

        _rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);

        if (_rigid.velocity.y == 0)
        {
            _playerAnim.Jumping(false);
        }

        _playerAnim.Move(move);
    }

    bool Grounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, 1.25f, _groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        if (hitInfo.collider != null)
        {
            Debug.Log("Hit: " + hitInfo.collider.name);
            if (_resetJumpNeeded == false)
            {
                return true;
            }
        }
        return false;
    }

    void Flip(bool facingRight)
    {
        if (facingRight == true)
        {
            _playerSprite.flipX = false;
        }
        else if (facingRight == false)
        {
            _playerSprite.flipX = true;
        }
    }

    public void SetRoShamBo(int selectedType)
    {
        _selectedType = selectedType;
        onCheckChallenge?.Invoke(_selectedType, _currentChallenge);
    }

    public void SetCurrentChallenge(Challenge currentChallenge)
    {
        if (_currentChallenge != currentChallenge)
        {
            _currentChallenge = currentChallenge;
        }
        else
        {
            Debug.Log("Challenge already Set");
        }
    }

    IEnumerator ResetJumpRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _resetJumpNeeded = false;
    }

    public void GameOver(bool hitBounds)
    {
        _playerAnim.Death();
        _gameOver = true;
        if (hitBounds == true)
        {
            StartCoroutine(GameOverCoroutine(0f));
        }
        else
        {
            StartCoroutine(GameOverCoroutine(2f));
        }
    }

    public void ReachedEnd()
    {
        _playerAnim.Move(0);
        _gameOver = true;
        UIManager.Instance.YouWin();
        UIManager.Instance.SetDeathsText(_deaths);
    }

    private IEnumerator GameOverCoroutine(float delay)
    {
        _deaths++;
        yield return new WaitForSeconds(delay);
        _playerAnim.Restart();
        transform.position = _startPos;
        _gameOver = false;
        _selectedType = -1;
        _currentChallenge = null;
        onRestart?.Invoke();
    }
}
