using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ARPanelRotator : MonoBehaviour
{
    [Header("Parent Object")]
    [SerializeField] protected GameObject thisParentObject;
    [SerializeField] protected GameObject thisInnerObject;
    [SerializeField] protected GameObject ARCameraObject;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected VideoPlayer VideoPlayer;
    [SerializeField] protected TouchScript TouchScript;

    [Header("Zoom Smoothness")]
    [SerializeField] private float MinZoomFactor = 0.005f;
    [SerializeField] private float MaxZoomFactor = 0.025f;
    [SerializeField] private float zoomValue = 0f;
    [SerializeField] private float zoomSens = 1.5f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float currentDistance = 50f;
    [SerializeField] private float currentTargetDistance = 50f;
    [SerializeField] private float smoothing = 5f;

    [Header("Offset")]
    [SerializeField] private Vector3 RotationOffset = new Vector3(0, 180f, 0);

    [SerializeField] protected bool Activated = false;

    private void Start()
    {
        this.thisParentObject = this.gameObject;
        this.thisInnerObject = this.thisParentObject.transform.Find("BXImage").gameObject;
        this.VideoPlayer = this.thisParentObject.transform.Find("VideoPlayer").GetComponent<VideoPlayer>();
        this.Animator = this.thisParentObject.transform.Find("BXImage/MXImage").GetComponent<Animator>();
        this.ARCameraObject = GameObject.Find("ARCamera").gameObject;
        this.TouchScript = GameObject.Find("MainPanel").GetComponent<TouchScript>();
    }

    public void PlayBtn()
    {
        if (!this.Activated)
        {
            this.Activated = true;
            this.Animator.SetTrigger("Activate");
            this.VideoPlayer.Play();
        }
    }

    public void PauseBtn()
    {
        if (Activated)
        {
            this.Activated = false;
            this.Animator.SetTrigger("DeActivate");
            this.VideoPlayer.Stop();
        }
    }

    private void Update()
    {
        ParentResizer();

        if (this.TouchScript.Pressed)
        {
            if (TouchScript.TouchDist.y <= -0.1f)
            {
                Vector3 localRotation = this.thisInnerObject.transform.localEulerAngles;
                localRotation.x = (localRotation.x > 180) ? localRotation.x - 360 : localRotation.x;
                localRotation.x = Mathf.Max(localRotation.x - 1f, -30f);
                this.thisInnerObject.transform.localEulerAngles = localRotation;
            }
            else if (TouchScript.TouchDist.y >= 0.1f)
            {
                Vector3 localRotation = this.thisInnerObject.transform.localEulerAngles;
                localRotation.x = (localRotation.x > 180) ? localRotation.x - 360 : localRotation.x;
                localRotation.x = Mathf.Min(localRotation.x + 1f, 30f);
                this.thisInnerObject.transform.localEulerAngles = localRotation;
            }

            if (TouchScript.TouchDist.x <= -0.1f)
            {
                Vector3 localRotation = this.thisInnerObject.transform.localEulerAngles;
                localRotation.y = (localRotation.y > 180) ? localRotation.y - 360 : localRotation.y;
                localRotation.y = Mathf.Min(localRotation.y + 1f, 30f);
                this.thisInnerObject.transform.localEulerAngles = localRotation;
            }
            else if (TouchScript.TouchDist.x >= 0.1f)
            {
                Vector3 localRotation = this.thisInnerObject.transform.localEulerAngles;
                localRotation.y = (localRotation.y > 180) ? localRotation.y - 360 : localRotation.y;
                localRotation.y = Mathf.Max(localRotation.y - 1f, -30f);
                this.thisInnerObject.transform.localEulerAngles = localRotation;
            }
            return;
        }
        else
        {
            Vector3 localRotation = this.thisInnerObject.transform.localEulerAngles;
            localRotation.y = (localRotation.y > 180) ? localRotation.y - 360 : localRotation.y;
            localRotation.y = Mathf.MoveTowards(localRotation.y, 0f, 0.5f);
            this.thisInnerObject.transform.localEulerAngles = localRotation;

            Vector3 localRotation1 = this.thisInnerObject.transform.localEulerAngles;
            localRotation1.x = (localRotation1.x > 180) ? localRotation1.x - 360 : localRotation1.x;
            localRotation1.x = Mathf.MoveTowards(localRotation1.x, 0f, 0.5f);
            this.thisInnerObject.transform.localEulerAngles = localRotation1;
        }

        this.thisInnerObject.transform.LookAt(this.ARCameraObject.transform.position);
        this.thisInnerObject.transform.Rotate(RotationOffset);
    }

    private void ParentResizer()
    {
        if (Input.touchCount == 2)
        {
            Touch touch_0 = Input.GetTouch(0);
            Touch touch_1 = Input.GetTouch(1);

            if (touch_0.phase == TouchPhase.Moved && touch_1.phase == TouchPhase.Moved)
            {
                Vector2 firstTouchPrev = touch_0.position - touch_0.deltaPosition;
                Vector2 secondTouchPrev = touch_1.position - touch_1.deltaPosition;

                float touchPrevPos = (firstTouchPrev - secondTouchPrev).magnitude;
                float touchCurPos = (touch_0.position - touch_1.position).magnitude;

                if (touchPrevPos < touchCurPos)
                {
                    zoomValue = 0.1f * zoomSens;
                }
                else if (touchPrevPos > touchCurPos)
                {
                    zoomValue = -0.1f * zoomSens;
                }
            }
            else
            {
                zoomValue = 0f;
            }
        }
        else
        {
            zoomValue = 0f;
        }

        currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minDistance, maxDistance);
        float lerpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
        currentDistance = lerpedZoomValue;

        float scaleFactor = Mathf.Lerp(MinZoomFactor, MaxZoomFactor, (currentTargetDistance - minDistance) / (maxDistance - minDistance));
        this.gameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}
