using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float moveSpeed;

    private void Update()
    {
        // 내 정면으로 moveSpeed의 속도로 날아가라.
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("플레이어랑 맞음");
        Destroy(gameObject);
    }

    public void Shoot(Vector3 dir, float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        transform.rotation = Quaternion.LookRotation(dir);  // 해당 방향을 바라봐라.
    }
}
