using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    public void Move(float move)
    {
        _anim.SetFloat("Movement", Mathf.Abs(move));
    }

    public void Jumping(bool jumping)
    {
        _anim.SetBool("Jumping", jumping);
    }

    public void Death()
    {
        _anim.SetTrigger("Death");
    }

    public void Restart()
    {
        _anim.SetTrigger("Restart");
    }
}
