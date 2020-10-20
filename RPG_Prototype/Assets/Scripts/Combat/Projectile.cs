using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 100f;
    [SerializeField] bool isHomingProjectile = true;
    Health target = null;
    float damage = 0;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
        StartCoroutine(DestroyIfNoCollisions());
    }

    //todo: might need to reconsider this depending on the type of projectile
    IEnumerator DestroyIfNoCollisions()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        if (isHomingProjectile)
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        target.TakeDamage(this.damage);
        Destroy(gameObject);
        
    }
}

