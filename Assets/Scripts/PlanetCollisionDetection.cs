using UnityEngine;

public class PlanetCollisionDetection : MonoBehaviour
{
    public GameObject hitPointPrefab;
    public float destroyDelay = 2;
    
    private void OnCollisionEnter(Collision other)
    {
        foreach (var contact in other.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            
            // Visualize the contact point
            if (!hitPointPrefab) return;
            var hitPoint = Instantiate(hitPointPrefab, contact.point,
                Quaternion.FromToRotation(Vector3.up, contact.normal), transform);
            Destroy(hitPoint, destroyDelay);
        }
    }
}
