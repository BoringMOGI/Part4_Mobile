using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float moveSpeed;

    private void Update()
    {
        // �� �������� moveSpeed�� �ӵ��� ���ư���.
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("�÷��̾�� ����");
        Destroy(gameObject);
    }

    public void Shoot(Vector3 dir, float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        transform.rotation = Quaternion.LookRotation(dir);  // �ش� ������ �ٶ����.
    }
}
