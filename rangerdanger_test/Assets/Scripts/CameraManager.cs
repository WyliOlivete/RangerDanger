using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CameraMode
{
    Pan,
    Rotate
}
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform mainField;
    [SerializeField]
    private Image cameraModeImage;
    [SerializeField]
    private Sprite cameraPan, cameraRotate;

    private Vector2 startPosition, dragStartPosition, dragNewPosition, finger0Position;
    private float rotationX, fingersDistance;
    private bool isZooming;
    private CameraMode cameraMode;
    private int cameraModeIndex;

    private void Update()
    {
        if (Input.touchCount == 0)
        {
            Mathf.Lerp(rotationX, 0, Time.deltaTime);
            if (isZooming)
                isZooming = false;
        }
        else if (Input.touchCount == 1)
        {
            if (!isZooming && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                switch (cameraMode)
                {
                    case CameraMode.Pan:
                        {
                            Camera.main.transform.position -= (Vector3)Input.GetTouch(0).deltaPosition / Screen.dpi * 2f;
                            Vector3 camPos = Camera.main.transform.position;
                            camPos.x = Mathf.Clamp(camPos.x, -5, 5);
                            camPos.y = Mathf.Clamp(camPos.y, 25 - 4, 25 + 4);
                            Camera.main.transform.position = camPos;
                        }
                        break;
                    case CameraMode.Rotate:
                        {
                            Vector2 newPosition = GetWorldPosition(Input.mousePosition);
                            Vector2 positionDifference = newPosition - startPosition;
                            rotationX = positionDifference.x * Screen.dpi * Mathf.Deg2Rad;
                        }
                        break;
                    default:
                        break;
                }
            }
            startPosition = GetWorldPosition(Input.mousePosition);
        }
        else if (Input.touchCount == 2)
        {
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                isZooming = true;
                dragNewPosition = GetWorldPosition(Input.GetTouch(1).position);
                Vector2 positionDifference = dragNewPosition - dragStartPosition;
                //if (Vector2.Distance(dragNewPosition, finger0Position) < fingersDistance)
                //    Camera.main.orthographicSize += positionDifference.magnitude;
                //else
                //    Camera.main.orthographicSize -= positionDifference.magnitude;
                //Vector2 midPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position + Input.GetTouch(1).position) * 0.5f;
                if (Vector2.Distance(dragNewPosition, finger0Position) < fingersDistance)
                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize + positionDifference.magnitude, Time.deltaTime * Screen.dpi);
                else
                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Camera.main.orthographicSize - positionDifference.magnitude, Time.deltaTime * Screen.dpi);
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1, 6);
                fingersDistance = Vector2.Distance(dragNewPosition, finger0Position);
            }
            dragStartPosition = GetWorldPosition(Input.GetTouch(1).position);
            finger0Position = GetWorldPosition(Input.GetTouch(0).position);
        }
        mainField.Rotate(Vector3.up, -rotationX);
    }
    private Vector2 GetWorldPosition(Vector3 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos);
    }
    public void CameraModeToggle()
    {
        cameraModeIndex++;
        if (cameraModeIndex >= System.Enum.GetValues(typeof(CameraMode)).Length)
            cameraModeIndex = 0;
        cameraMode = (CameraMode)cameraModeIndex;
        switch (cameraMode)
        {
            case CameraMode.Pan:
                cameraModeImage.sprite = cameraPan;
                break;
            case CameraMode.Rotate:
                cameraModeImage.sprite = cameraRotate;
                break;
            default:
                break;
        }
    }
    public void CameraReset()
    {
        Camera.main.orthographicSize = 6;
        Camera.main.transform.position = new Vector3(0, 25, -24);
    }
}