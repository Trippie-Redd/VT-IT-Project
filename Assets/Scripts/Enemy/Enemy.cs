using UnityEngine;

[RequireComponent(typeof(CapsuleCollider), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    int hp =10;

    public void OnHit()
    {
    
        hp--;
        if(hp>0) return;
        // Starta animation
        GetComponent<Animator>().SetBool("Dead",true);
        // Starta dissolve
        GetComponentInChildren<DissolveScript>().StartDissolve();
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(gameObject,5);
    }
}
