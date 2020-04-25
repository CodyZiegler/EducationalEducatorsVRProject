using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using OVRTouchSample;
public class PlaceholderTarget : MonoBehaviour
{
    public Transform targetOne;
    public Transform targetTwo;
    private PlayerPackage _playerPackage;
    private float _targetTwoDistance;
    private float _targetOneDistance;
    //private float timer = 1f;
    private bool _targetTwoHit;
    private bool _targetOneHit;

    void Start()
    {
        _playerPackage = FindObjectOfType<PlayerPackage>();
        
    }

    void Update()
    {
        
        if (targetTwo == null) return;
            _targetTwoDistance = (transform.position - targetTwo.transform.position).magnitude;
            if (_targetTwoDistance < 3f && !_targetTwoHit)
            {
                _playerPackage.LoadNextScene();
                _targetTwoHit = true;
            }
        /*else
        {
            _targetOneDistance = (transform.position - targetOne.transform.position).magnitude;
            if (_targetOneDistance < 2f && !_targetOneHit)
            {
                _playerPackage.LoadNextScene();
                _targetOneHit = true;
            }
        }*/
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetOne.gameObject)
        {
            Debug.Log("Transisition Here");
            _playerPackage.LoadNextScene();
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Transisition Here");
            _playerPackage.LoadNextScene();
        }
    }*/
}