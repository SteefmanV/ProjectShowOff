using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AirTrap : MonoBehaviour
{
    private GameObject _trashObject = null;

    [SerializeField] private float _trapDurationSec = 5f;
    [SerializeField, Required] private GameObject _holdPosition = null;

    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _instantiated = null;
    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _trapped = null;
    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _trapDeactivated = null;
    private AudioSource _audio;

    private float _timer = 0f;
    private bool _hasTrash = false;
    private float _maxHeight = 6.5f;


    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.PlayOneShot(_instantiated);
    }


    private void Update()
    {
        if (_hasTrash)
        {
            _timer += Time.deltaTime;
            if (_timer > _trapDurationSec) destroyAndReset();

            transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        }

        if (transform.position.y >= _maxHeight) destroyAndReset();
    }


    // Update is called once per frame
    public void OnTriggerEnter(Collider pOther)
    {
        if (!_hasTrash)
        {
            if (pOther.gameObject.CompareTag("thrash"))
            {
                _hasTrash = true;

                _trashObject = pOther.transform.parent.gameObject;
                _trashObject.transform.parent = _holdPosition.transform;

                _trashObject.transform.localPosition = Vector3.zero;



                GetComponent<BoxCollider>().enabled = false;

                _audio.PlayOneShot(_trapped);
                pOther.gameObject.GetComponent<Thrash>().SetDisabled(true);
            }
        }
    }


    public void destroyAndReset()
    {
        if (_hasTrash)
        {
            _trashObject.transform.parent = null;
            _trashObject.GetComponentInChildren<Thrash>().SetDisabled(false);
        }

        _audio.PlayOneShot(_trapDeactivated);
        Destroy(gameObject);
    }
}
