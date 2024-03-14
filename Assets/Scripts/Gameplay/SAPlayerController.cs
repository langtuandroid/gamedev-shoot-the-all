using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sounds;
using UnityEngine;

namespace Gameplay
{
    public class SAPlayerController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _shootParticle;
        [SerializeField] private GameObject _leftShoulder;
        [SerializeField] private GameObject _rightShoulder;
        [SerializeField] private GameObject _head;
        [SerializeField] private Transform _gunTransform;
        [SerializeField] private GameObject _gunVisual;
        [SerializeField] private Transform _playerModel;
        [SerializeField] private GameObject _armToRotate;
        [SerializeField] private float workingArmRotation;
        [SerializeField] private float _gunRotationOffset;
        [SerializeField] private Vector3 _gunRecoilAnimationOffset;
    
        private float _bodyRotationMin = 20f;
        private float _bodyRotationMax = 160f;
        private bool _isActive = true;
        private bool _isShooting;
        private bool _isLookAtGun = true;
        private bool _isXArmRotation = true;
        private bool _shootDelayPassed = true;
        private float _rotationX, _rotationY, _rotationZ;
        private List<Vector3> _listOfBulletPoints = new();
        private SABulletController _currentBullet;
        private Collider[] _collidersInsideBot;
        private Rigidbody[] _rbInBot;
        private BoxCollider[] _boxCollidersInsideBot;

        void Start()
        {
            _isActive = false;
            RemovePhysics();
            GetCurrentOffset();
            if (_armToRotate == null)
            {
                GetComponent<Animator>().enabled = true;
            }
            else
            {
                _rotationZ += workingArmRotation;
                _armToRotate.transform.localRotation = _isXArmRotation ? Quaternion.Euler(_rotationZ, _rotationY, _rotationX) : Quaternion.Euler(_rotationX, _rotationY, _rotationZ);
            }
        }
    
        void Update()
        {
            if (_isLookAtGun)
            {
                _leftShoulder.transform.LookAt(_gunTransform);
                _rightShoulder.transform.LookAt(_gunTransform);
                _head.transform.LookAt(_gunTransform);
                _gunTransform.transform.localRotation = Quaternion.Euler(0, 0, -_rotationZ);
                _playerModel.transform.localEulerAngles = new Vector3(0f, Math.Abs(_gunTransform.transform.localRotation.eulerAngles.z) * 0.9f, 0f);


                var tempWorkingArmRotation = Math.Abs(workingArmRotation);

                if (tempWorkingArmRotation > 360)
                {
                    if (tempWorkingArmRotation % 360f < 180)
                    {
                        tempWorkingArmRotation %= 180f;
                    }
                    else
                    {
                        tempWorkingArmRotation %= 360f;
                    }
                }
            
                float yRotation;

                if (tempWorkingArmRotation is >= 0 and < 180)
                {
                    yRotation = Mathf.Lerp(_bodyRotationMin, _bodyRotationMax, Mathf.InverseLerp(0f, 180f, tempWorkingArmRotation));
                }
                else
                {
                    yRotation = Mathf.Lerp(_bodyRotationMax, _bodyRotationMin, Mathf.InverseLerp(180f, 360f, tempWorkingArmRotation));
                }

                _playerModel.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            }

            if (_isActive)
            {
                if (Input.GetMouseButton(0))
                {
                    _rotationZ += Input.GetAxis("Mouse X");
                    _rotationZ -= Input.GetAxis("Mouse Y");
                    workingArmRotation += Input.GetAxis("Mouse X");
                    workingArmRotation -= Input.GetAxis("Mouse Y");
                    _armToRotate.transform.localRotation = _isXArmRotation ? Quaternion.Euler(_rotationZ, _rotationY, _rotationX) : Quaternion.Euler(_rotationX, _rotationY, _rotationZ);
                }
            }
        
            if (_isShooting && _currentBullet == null)
            {
                _isActive = true;
            }
        }

        public void Disable() => _isActive = false;
    
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                OnBotDeath();
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.CompareTag("Hit"))
            {
                OnBotDeath();
            }
        }

        private void RemovePhysics()
        {
            _collidersInsideBot = GetComponentsInChildren<Collider>();
            _rbInBot = GetComponentsInChildren<Rigidbody>();
            _boxCollidersInsideBot = GetComponentsInChildren<BoxCollider>();
        
            foreach (var bc in _boxCollidersInsideBot)
                bc.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

            foreach (var cl in _collidersInsideBot) cl.enabled = false;
            foreach (var rb in _rbInBot) rb.isKinematic = true;

            transform.GetComponent<CapsuleCollider>().enabled = true;
        }

        public void OnBotDeath()
        {
            _isShooting = false;
            _isActive = false;
            foreach (var c in _collidersInsideBot)
            {
                c.enabled = true;
            }

            foreach (var rb in _rbInBot)
            {
                rb.isKinematic = false;
            }

            transform.GetComponent<CapsuleCollider>().enabled = false;
            if (gameObject.CompareTag("Emeny"))
            {
                tag = "Untagged";
                SAGameManager.Instance.OnBotKilledAction();
            }
            else if (gameObject.CompareTag("Player")) SAGameManager.Instance.LevelFailedAction();

            _gunTransform.GetComponent<SARaycastReflectionWorker>().enabled = false;
            _gunTransform.GetComponent<LineRenderer>().enabled = false;
            _gunVisual.gameObject.SetActive(false);
            if (SAAudioManager.instance) SAAudioManager.instance.Play("Hit");
            enabled = false;
        }

        public bool Shoot()
        {
            if (_isActive && _shootDelayPassed)
            {
                _shootDelayPassed = false;
                StartCoroutine(ShootDelay());
                SpawnBullet();
                return true;
            }

            return false;
        }

        private IEnumerator ShootDelay()
        {
            yield return new WaitForSeconds(2);
            _shootDelayPassed = true;
        }

        private void SpawnBullet()
        {
            _shootParticle.Stop();
            _shootParticle.Play();
        
            if (_gunTransform.GetComponent<SARaycastReflectionWorker>().IsKillableOnShot)
            {
                OnBotDeath();
                Destroy(_currentBullet);
                return;
            }
            _listOfBulletPoints.Clear();
            Vector3[] temparray = new Vector3[_gunTransform.GetComponent<LineRenderer>().positionCount];
            _gunTransform.GetComponent<LineRenderer>().GetPositions(temparray);
            _listOfBulletPoints = temparray.ToList();
            _currentBullet = Instantiate(SAGameManager.Instance.GetBullet(), _listOfBulletPoints[0], Quaternion.identity);
            _currentBullet.SetPath(_listOfBulletPoints);
            if (_listOfBulletPoints.Count > 1)
            {
                _currentBullet.transform.LookAt(_listOfBulletPoints[1]);
            }
            _isShooting = true;

            if (SAAudioManager.instance)
            {
                SAAudioManager.instance.Play("Bullet");
            }
            GunAnimation();
        }

        private void GunAnimation()
        {
            _gunTransform.DOBlendableLocalMoveBy(_gunTransform.right * -_gunRotationOffset, 0.1f);
            _gunTransform.DOBlendableRotateBy(_gunRecoilAnimationOffset, 0.1f).OnComplete(() =>
            {
                _gunTransform.DOBlendableLocalMoveBy(_gunTransform.right * _gunRotationOffset, 0.2f);
                _gunTransform.DOBlendableRotateBy(-_gunRecoilAnimationOffset, 0.2f);
            });
        }
    
        public void BotWin()
        {
            _gunTransform.GetComponent<SARaycastReflectionWorker>().enabled = false;
            _gunTransform.GetComponent<LineRenderer>().enabled = false;
            _isActive = false;

        }

        private void GetCurrentOffset()
        {
            if (_isXArmRotation)
            {
                _rotationX = _armToRotate.transform.localEulerAngles.z;
                _rotationY = _armToRotate.transform.localEulerAngles.y;
                _rotationZ = _armToRotate.transform.localEulerAngles.x;
            }
            else
            {
                _rotationX = _armToRotate.transform.localEulerAngles.x;
                _rotationY = _armToRotate.transform.localEulerAngles.y;
                _rotationZ = _armToRotate.transform.localEulerAngles.z;
            }

            _isActive = true;
        }
    }
}
