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
                movePoint = hit.point;      // 레이가 충돌한 지점에 대한 위치.
                agent.SetDestination(movePoint);
            }
        }
    }
    Vector3 movePoint;

    private void KeyboardMove()
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
    public void RotatePlayer(float inputX, float inputY)
    {
        Vector3 dir = new Vector3(inputX, 0f, inputY);
        body.rotation = Quaternion.LookRotation(dir);       // 특정 방향으로 회전값을 줘라.
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(movePoint, 0.2f);
    }
#endif
}
