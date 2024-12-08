using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    PrefabBullet bullet;
    EnemyType attackType;

    public void Shoot(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Pistol:
                    ShootPistol(transform.position);
                break;
            case EnemyType.Pump:
                    ShootPump(transform.position);
                break;
            case EnemyType.Submachine:
                    ShootSubmachine(transform.position);
                break;
            default:
                break;
        }
    }

    void ShootPistol(Vector3 origin)
    {
        PrefabBullet enemyBullet = Instantiate(bullet, origin, transform.rotation);
        enemyBullet.isFromPlayer = false;
    }

    void ShootPump(Vector3 origin)
    {
        for (int i = 0; i < 3; i++)
        {
            PrefabBullet enemyBullet = Instantiate(bullet, origin, transform.rotation);
            enemyBullet.isFromPlayer = false;
            enemyBullet.transform.Rotate(new Vector3(0, 0, -30));
            enemyBullet.transform.Rotate(new Vector3(0, 0, 30 * i));
        }
    }

    void ShootSubmachine(Vector3 origin)
    {
        PrefabBullet enemyBullet = Instantiate(bullet, origin, transform.rotation);
        enemyBullet.isFromPlayer = false;
        enemyBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)));
    }
}
