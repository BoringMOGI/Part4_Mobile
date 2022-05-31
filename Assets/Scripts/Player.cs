using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform body;
    [SerializeField] float moveSpeed;

    void Update()
    {
        controller.Move(Vector3.down * 3f * Time.deltaTime);

        // 안드로이드 OS가 아닐때 빌드에 포함.
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

    Vector3 target;

    public void RotatePlayer(float inputX, float inputY)
    {
        Vector2 input = new Vector2(inputX, inputY);
        body.localRotation = Quaternion.FromToRotation(Vector3.forward, Vector2.zero - input);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.2f);
    }
}
