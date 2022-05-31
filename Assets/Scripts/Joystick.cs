using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Camera cam;
    [SerializeField] RectTransform panel;
    [SerializeField] RectTransform stick;

    [Header("Events")]
    [SerializeField] UnityEvent<float, float> OnMoveStick;

    Vector2 originPos;  // ��ƽ�� ���� ��ġ.
    float maxDistance;  // �ִ�� ������ �� �ִ� �Ÿ�.
    bool isOperate;     // ���� ���̴�.

    private void Start()
    {
        originPos = stick.localPosition;
        maxDistance = panel.rect.width / 2f;
    }
    private void Update()
    {
        if (isOperate == false)
            return;

        // Canvas�� ��尡 ScreenSpace-Camera�̱� ������ ���� �ػ󵵶� ���̰� �����.
        // ���� �ش� ĵ������ ��ġ�� ī�޶��� ���� ���� ���콺 ��ġ�� �����Ѵ�.
        Vector2 mousePos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, Input.mousePosition, cam, out mousePos);

        // �ִ� �Ÿ� ����.
        float currentDistance = Vector2.Distance(originPos, mousePos);
        if(currentDistance > maxDistance)
        {
            // �Ÿ��� ���� ���Ⱚ�� ���̰� �߻��ϱ� ������ "����ȭ" �����ش�.
            Vector2 dir = (mousePos - originPos).normalized;
            mousePos = originPos + (dir * maxDistance);
        }

        stick.localPosition = mousePos;

        // �̺�Ʈ �߻�. ���� ������ ������ �����Ѵ�.
        Vector2 moveDir = (mousePos - originPos).normalized;
        OnMoveStick?.Invoke(moveDir.x, moveDir.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isOperate = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isOperate = false;
        stick.localPosition = originPos;
    }       
}
