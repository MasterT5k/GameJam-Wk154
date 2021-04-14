using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private Vector3 _cameraOffset = Vector3.back;

    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _player.transform.position + _cameraOffset;
        if (transform.position.x < -17)
        {
            transform.position = new Vector3(-17, transform.position.y, _cameraOffset.z);
        }

        if (transform.position.y < -1)
        {
            transform.position = new Vector3(transform.position.x, -1, _cameraOffset.z);
        }
    }
}
