using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [SerializeField] float knockbackTime = 0.1f;
    [SerializeField] float knockBackScale = 10.0f;
    Rigidbody rb;

    public void BlockParryKnockback()
    {
        rb.velocity = -gameObject.transform.forward * knockBackScale;
        StartCoroutine(KnockbackTime());
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    IEnumerator KnockbackTime()
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.velocity = Vector3.zero;
    }
}
