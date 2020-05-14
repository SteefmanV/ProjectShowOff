using Sirenix.OdinInspector;
using UnityEngine;

public class SmallBubbleGunPowerUp : MonoBehaviour
{
    [ReadOnly] public bool smallBubbleGunPowerUpActive = false;

    [SerializeField] private GameObject _player = null;
    [SerializeField] private GameObject _bubbleProjectile = null;
    [SerializeField] private LayerMask _mask;

    private Vector3 _playerCurrentPosition;
    private Collider[] _fishInRange;
    private int _createdBubbleProjectiles;
    private float _shootrange = 10f;
    private GameObject _newProjectile = null;
    

    private void Update()
    {
        _playerCurrentPosition = _player.transform.position;

        if (smallBubbleGunPowerUpActive)
        {
            _fishInRange = Physics.OverlapSphere(_playerCurrentPosition, _shootrange, _mask);
            createBullet();
        }

    }


    public void createBullet()
    {
        foreach(Collider Fish in _fishInRange)
        {
            if (_createdBubbleProjectiles <= _fishInRange.Length-1)
            {
                _newProjectile = Instantiate(_bubbleProjectile, _playerCurrentPosition, Quaternion.identity);
                _newProjectile.GetComponent<SmallBubbleProjectile>().target = Fish.gameObject;
                _newProjectile = null;
                _createdBubbleProjectiles++;
            }
        }
    }


    public void SetUp()
    {
        smallBubbleGunPowerUpActive = true;

        
    } 


    public void Land()
    {
        smallBubbleGunPowerUpActive = false;
    }
}
