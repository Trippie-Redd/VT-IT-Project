using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    Renderer material;
    float dissolve = 1;
    bool startDissolve = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!startDissolve) return;

        if(dissolve > 0.3)
            dissolve-=0.003f;
        else
            dissolve-=0.01f;
        material.material.SetFloat("_DissolveValue",dissolve);
    }

    public void StartDissolve()
    {
        startDissolve = true;
    }
}
