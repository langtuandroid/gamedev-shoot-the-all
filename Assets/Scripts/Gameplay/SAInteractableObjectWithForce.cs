using UnityEngine;

namespace Gameplay
{
    public class SAInteractableObjectWithForce : MonoBehaviour
    {
        [SerializeField] private Vector3 _forceDirection;
   
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                GetComponent<Rigidbody>().AddForce(_forceDirection * 5000 * Time.deltaTime, ForceMode.Force);
            }
        }
    }
}
