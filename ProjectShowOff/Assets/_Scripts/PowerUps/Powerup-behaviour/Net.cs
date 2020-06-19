using Sirenix.OdinInspector;
using UnityEngine;

public class Net : MonoBehaviour
{
    [SerializeField] private float _netDuration = 5;
    private float _timer = 0;

    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _netSpawned = null;
    [FoldoutGroup("Sounds"), SerializeField] private AudioClip _thrashDestroyed = null;

    public GameObject _netEndHolder = null;
    public GameObject _netStart = null;
    public GameObject _netEnd = null;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.PlayOneShot(_netSpawned);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer > _netDuration)
        {
            Destroy(gameObject);
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("thrash"))
        {
            _audio.PlayOneShot(_thrashDestroyed);
            Destroy(collision.gameObject);
        }
    }
}
