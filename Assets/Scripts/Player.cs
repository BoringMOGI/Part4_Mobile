using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float moveSpeed;

    void Update()
    {
        controller.Move(Vector3.down * 3f * Time.deltaTime);

        // �ȵ���̵� OS�� �ƴҶ� ���忡 ����.
#if UNITY_STANDALONE_WIN
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        MovePlayer(x, y);
#endif
    }

    public void MovePlayer(float inputX, float inputY)
    {
        Vector3 dir = new Vector3(inputX, 0, inputY);
        if (dir != Vector3.zero)
            controller.Move(dir * moveSpeed * Time.deltaTime);
    }
}
