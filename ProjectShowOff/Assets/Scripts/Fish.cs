using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace old
{
    public class Fish : MonoBehaviour
    {
        public bool isProtected = false;

        private FishManager _fishManager;
        [SerializeField] private GameObject _protectionBubble;


        private void Start()
        {
            _protectionBubble = transform.Find("ProtectionBubble").gameObject;
            _protectionBubble.SetActive(false);

        }


        private void Update()
        {
            if (isProtected && _protectionBubble.activeSelf == false)
            {
                _protectionBubble.SetActive(true);
            }
            else if (!isProtected && _protectionBubble.activeSelf == true)
            {
                _protectionBubble.SetActive(false);
            }
        }


        private void Awake()
        {
            _fishManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FishManager>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("thrash"))
            {
                if (isProtected)
                {
                    Destroy(collision.gameObject);
                    isProtected = false;
                }
                else
                {
                    _fishManager.CheckFishCount();
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
