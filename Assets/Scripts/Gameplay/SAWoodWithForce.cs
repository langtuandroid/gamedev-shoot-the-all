using UnityEngine;

namespace Gameplay
{
    public class SAWoodWithForce : MonoBehaviour
    {
        [SerializeField] public Vector3 _impulseDirection;
   
        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Bullet"))
            {
                for(int i=0;i<transform.childCount;i++)
                {
                    transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().isKinematic=false;
                    transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().
                        AddForce(_impulseDirection * 5000 * Time.deltaTime);
                }

                foreach (var bc in GetComponents<BoxCollider>())
                {
                    bc.enabled = false;
                }

                enabled = false;
            }
        }
    }
}
