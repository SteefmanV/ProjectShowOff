using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Thrash : MonoBehaviour
{
    public bool disabled { get; set; } = false;

    [ProgressBar(0, 100, ColorMember = "GetHealthBarColor")]
    public float health = 100;

    [Header("Thrash Settings")]
    [SerializeField] private ItemCollectionManager.Item trashType = ItemCollectionManager.Item.bottle;
    [SerializeField] private float _startFallSpeed = 1;

    [SerializeField, Tooltip("This is minimum movespeed,  % of the _startFallSpeed ")]
    private float _minForcePercentage = 0.2f;

    [SerializeField] private GameObject _mainObject = null;

    [SerializeField] private Rigidbody _rb = null;
    private ItemCollectionManager _powerUpManager;
    private ScoreManager _scoreManager;


    private void Awake()
    {
        _rb = _mainObject.GetComponent<Rigidbody>();
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _powerUpManager = gameManager.GetComponent<ItemCollectionManager>();
        _scoreManager = gameManager.GetComponent<ScoreManager>();
    }


    private void Start()
    {
        _rb.AddForce(new Vector3(0, _startFallSpeed, 0), ForceMode.Force);
    }


    private void Update()
    {
        if (disabled) { return; }

        Vector3 minimumForce = new Vector3(0, _startFallSpeed * _minForcePercentage, 0);

        if (_rb.velocity.magnitude < minimumForce.magnitude)
        {
            _rb.velocity = minimumForce;
        }

        if (health <= 0) Destroy(_mainObject);
    }


    public void GetCollected()
    {
        _powerUpManager.CollectedItem(trashType);
        _scoreManager.ThrashDestroyed();
        Destroy(_mainObject);
    }

    
    public void SetDisabled(bool pActive)
    {
        disabled = pActive;

        if(disabled)
        {
            GetComponentInParent<BoxCollider>().enabled = false;
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
        }
        else
        {
            GetComponentInParent<BoxCollider>().enabled = true;
            _rb.isKinematic = false;
            _rb.AddForce(new Vector3(0, _startFallSpeed, 0), ForceMode.Force);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (disabled) return;
        Debug.Log("other: " + other.gameObject.tag);

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Enabled: " + enabled);
            if (enabled)
            {
                Debug.Log("Collided with player");
                GetCollected();
            }
        }
    }


    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
}
