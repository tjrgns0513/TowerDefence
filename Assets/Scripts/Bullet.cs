using System.Collections;
using UnityEngine;
using static ObjectPoolManager;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public Transform targetTr;
    Enemy enemyTarget;
    public float speed = 30f;


    public void Init(Transform target)
    {
        SetTarget(target);
    }

    public void SetTarget(Transform _target)
    {
        targetTr = _target;
        enemyTarget = _target.GetComponent<Enemy>();
    }

    void Update()
    {
        if (targetTr == null || targetTr.gameObject == null || enemyTarget.isDead || enemyTarget.gameObject == null)
        {
            DisposeBullet();
            return;
        }

        Seek();
    }

    private void Seek()
    {
        Vector3 dir = targetTr.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(targetTr);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy hitEnemy = other.gameObject.GetComponent<Enemy>();
            if (hitEnemy != null && !hitEnemy.isDead)
            {
                HitTarget(hitEnemy);
            }
            else
            {
                DisposeBullet();
            }
        }
    }


    //�Ѿ��� Ÿ�ٿ� �¾�����
    void HitTarget(Enemy target)
    {
        //impactEffect ��ƼŬ��ġ�� �Ѿ���ġ�� �޾Ƽ� ����
        GameObject effectObj = ObjectPoolManager.Instance.GetObjectFromPool(ObjectPoolManager.PoolObjectType.ImpactEffect);
        BulletImpactEffect impactEffect = effectObj.GetComponent<BulletImpactEffect>();
        effectObj.transform.position = transform.position;
        effectObj.transform.rotation = transform.rotation;

        if(!target.isDead)
        {
            target.Die();
        }

        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolObjectType.Bullet);
    }

    void DisposeBullet()
    {
        ObjectPoolManager.Instance.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolObjectType.Bullet);
    }
}
