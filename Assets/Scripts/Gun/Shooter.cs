using NUnit.Framework;
using UnityEngine;

[ExecuteInEditMode]
public class Shooter : MonoBehaviour
{
    public Transform FirePoint;
 
    public void Shooting()
    {
        RaycastHit hit;

        if(Physics.Raycast(FirePoint.position , transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawRay(FirePoint.position , transform.TransformDirection(Vector3.forward)* hit.distance , Color.yellow);
            if(hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponent<Enemy>().OnHit();
            }
        }
        else
        {
            Debug.DrawRay(FirePoint.position , transform.TransformDirection(Vector3.forward)* 10.0f , Color.red);
        }
    }

}
