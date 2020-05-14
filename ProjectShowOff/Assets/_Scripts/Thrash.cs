using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Thrash : MonoBehaviour
{
    [ProgressBar(0, 100, ColorMember = "GetHealthBarColor")]
    public float health = 100;

    [Header("Thrash Settings")]
    [SerializeField] private ItemCollectionManager.Item trashType = ItemCollectionManager.Item.bottle;
    [SerializeField] private float _startFallSpeed = 1;

    [SerializeField, Tooltip("This is minimum movespeed,  % of the _startFallSpeed ")]
    private float _minForcePercentage = 0.2f;

    private Rigidbody _rb = null;
    private ItemCollectionManager _powerUpManager;
    private ScoreManager _scoreManager;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
        Vector3 minimumForce = new Vector3(0, _startFallSpeed * _minForcePercentage, 0);

        if (_rb.velocity.magnitude < minimumForce.magnitude)
        {
            _rb.velocity = minimumForce;
        }

        if(health <= 0) Destroy(gameObject);
    }


    public void GetCollected()
    {
        _powerUpManager.CollectedItem(trashType);
        _scoreManager.ThrashDestroyed();
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (enabled)
            {
               GetCollected();
            }
        }
    }


    private Color GetHealthBarColor(float value)
    {
        return Color.Lerp(Color.red, Color.green, Mathf.Pow(value / 100f, 2));
    }
}
