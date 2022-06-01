using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] CharacterController controller;
    [SerializeField] Transform body;
    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask groundMask;

    void Update()
    {
        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPositions(agent.path.corners);

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, float.MaxValue, groundMask))
            {
                movePoint = hit.point;      // ���̰� �浹�� ������ ���� ��ġ.
                agent.SetDestination(movePoint);
            }
        }
    }
    Vector3 movePoint;

    private void KeyboardMove()
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
    public void RotatePlayer(float inputX, float inputY)
    {
        Vector3 dir = new Vector3(inputX, 0f, inputY);
        body.rotation = Quaternion.LookRotation(dir);       // Ư�� �������� ȸ������ ���.
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(movePoint, 0.2f);
    }
#endif
}
