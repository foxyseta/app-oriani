using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraHandler : MonoBehaviour
{
    public GameObject cameraObject;
    [Header("Pan")]
    public float boundsXMin = -10f;
    public float boundsXMax = 5f;
    public float boundsYMin = -18f;
    public float boundsYMax = -4f;
    [Header("Zoom")]
    public float zoomSpeedTouch = 0.1f;
    public float zoomSpeedMouse = 0.5f;
    public float zoomBoundsMin = 10f;
    public float zoomBoundsMax = 85f;
    public float doubleInputMaxDelay = 0.5f;
    public int autoZoomSteps = 5;
    public float autoZoomDuration = 0.250f;

    private Camera cam;
    private Vector3 lastPanPosition;
    private float lastInputTime = -1;
    private int panFingerId; // touch mode only
    private bool wasZoomingLastFrame = false; // touch mode only
    private Vector2[] lastZoomPositions; // touch mode only
    private float originalOrthographicSize, targetOrthographicSize, sinceStartedAutoZooming = -1f;

    void Start()
    {
        cam = cameraObject.GetComponent<Camera>();
    }

    void Update()
    {
        // all WebGL players supported by Unity are on desktop anyway
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            HandleTouch();
        else
            HandleMouse();
        ApplyAutoZoom();
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {
            case 1: // Panning, double tap
                if (!EventSystem.current.IsPointerOverGameObject(0))
                {
                    wasZoomingLastFrame = false;
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        lastPanPosition = touch.position;
                        panFingerId = touch.fingerId;
                        if (lastInputTime >= 0 && Time.time - lastInputTime <= doubleInputMaxDelay)
                        {
                            AutoZoomCamera();
                            lastInputTime = -1;
                        }
                        else
                            lastInputTime = Time.time;
                    }
                    else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                        PanCamera(touch.position);
                }
                break;
            case 2: // Zooming
                if (!(EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1)))
                {
                    Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                    if (!wasZoomingLastFrame)
                        wasZoomingLastFrame = true;
                    else
                        ZoomCamera(Vector2.Distance(newPositions[0], newPositions[1]) -
                        Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]), zoomSpeedTouch);
                    lastZoomPositions = newPositions;
                }
                break;
            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            // Pan, double click
            if (Input.GetMouseButtonDown(0))
            {
                lastPanPosition = Input.mousePosition;
                if (lastInputTime >= 0 && Time.time - lastInputTime <= doubleInputMaxDelay)
                {
                    AutoZoomCamera();
                    lastInputTime = -1;
                }
                else
                    lastInputTime = Time.time;
            }
            else if (Input.GetMouseButton(0))
                PanCamera(Input.mousePosition);
            // Scroll
            ZoomCamera(Input.GetAxis("Mouse ScrollWheel"), zoomSpeedMouse);
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        cameraObject.transform.Translate(cam.ScreenToWorldPoint(lastPanPosition) - cam.ScreenToWorldPoint(newPanPosition), Space.World);
        Vector3 pos = cameraObject.transform.position;
        pos.x = Mathf.Clamp(cameraObject.transform.position.x, boundsXMin, boundsXMax);
        pos.y = Mathf.Clamp(cameraObject.transform.position.y, boundsYMin, boundsYMax);
        cameraObject.transform.position = pos;
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset != 0)
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), zoomBoundsMin, zoomBoundsMax);
    }

    void AutoZoomCamera()
    {
        originalOrthographicSize = cam.orthographicSize;
        float step = cam.orthographicSize == zoomBoundsMin ? autoZoomSteps : (float)Math.Floor((cam.orthographicSize - 1 - zoomBoundsMin) / (zoomBoundsMax - zoomBoundsMin) * autoZoomSteps);
        targetOrthographicSize = zoomBoundsMin + step / autoZoomSteps * (zoomBoundsMax - zoomBoundsMin);
        if (sinceStartedAutoZooming == -1f)
            sinceStartedAutoZooming = 0;
    }

    void ApplyAutoZoom()
    {
        if (sinceStartedAutoZooming != -1)
        {
            sinceStartedAutoZooming = Math.Min(sinceStartedAutoZooming + Time.deltaTime, autoZoomDuration);
            cam.orthographicSize = originalOrthographicSize + (targetOrthographicSize - originalOrthographicSize) * sinceStartedAutoZooming / autoZoomDuration;
            if (sinceStartedAutoZooming >= autoZoomDuration)
                sinceStartedAutoZooming = -1;
        }
    }

}