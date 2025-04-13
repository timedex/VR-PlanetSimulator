using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class HandGesturesController : MonoBehaviour
{
    [SerializeField] private Transform solarSystem;
    [SerializeField] private LayerMask interactableLayer;

    private XRHandSubsystem handSubsystem;

    private bool isMoving = false;
    private bool isScaling = false;

    private XRHand activeHand;
    private Vector3 moveOffset;

    private float initialScaleDistance;
    private Vector3 initialScale;

    void Start()
    {
        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    void Update()
    {
        if (handSubsystem == null || !handSubsystem.running) return;

        XRHand left = handSubsystem.leftHand;
        XRHand right = handSubsystem.rightHand;

        bool leftPinch = IsPinching(left);
        bool rightPinch = IsPinching(right);

        // if (leftPinch && rightPinch)
        // {
        //     HandleScaling(left, right);
        // }
        if (leftPinch ^ rightPinch) // Solo una mano
        {
            XRHand hand = leftPinch ? left : right;
            HandleMoving(hand);
        }
        else
        {
            isMoving = false;
            // isScaling = false;
        }
    }

    void HandleMoving(XRHand hand)
    {
        Vector3 pinchPos = GetJointPosition(hand, XRHandJointID.IndexTip);

        // Evitar conflictos con objetos interactivos
        if (Physics.CheckSphere(pinchPos, 0.02f, interactableLayer)) {
            isMoving = false;
            return;
        }

        if (!isMoving || !IsSameHand(activeHand, hand))
        {
            moveOffset = solarSystem.position - pinchPos;
            activeHand = hand;
            isMoving = true;
        }

        solarSystem.position = solarSystem.position + moveOffset * .01f; //pinchPos + moveOffset;
    }
    
    bool IsSameHand(XRHand a, XRHand b)
    {
        return a.handedness == b.handedness;
    }

    void HandleScaling(XRHand left, XRHand right)
    {
        Vector3 leftPos = GetJointPosition(left, XRHandJointID.IndexTip);
        Vector3 rightPos = GetJointPosition(right, XRHandJointID.IndexTip);

        float currentDistance = Vector3.Distance(leftPos, rightPos);

        if (!isScaling)
        {
            initialScaleDistance = currentDistance;
            initialScale = solarSystem.localScale;
            isScaling = true;
        }

        float scaleFactor = currentDistance / initialScaleDistance;
        solarSystem.localScale = initialScale * scaleFactor;
    }

    bool IsPinching(XRHand hand)
    {
        XRHandJoint index = hand.GetJoint(XRHandJointID.IndexTip);
        XRHandJoint thumb = hand.GetJoint(XRHandJointID.ThumbTip);

        if (!index.TryGetPose(out Pose indexPose) || !thumb.TryGetPose(out Pose thumbPose))
            return false;

        return Vector3.Distance(indexPose.position, thumbPose.position) < 0.025f;
    }

    Vector3 GetJointPosition(XRHand hand, XRHandJointID jointID)
    {
        var joint = hand.GetJoint(jointID);
        if (joint.TryGetPose(out Pose pose))
            return pose.position;
        return Vector3.zero;
    }
    /////////////////////////////////////////////////////////////////////////////77
    // [SerializeField] private Transform parentObjectTransform;
    // [SerializeField] private LayerMask interactableLayer;
    //
    // private XRHandSubsystem handSubsystem;
    // private bool isMoving = false;
    // private Vector3 handOffset;
    //
    // private bool isScaling = false;
    // private Vector3 initialScale;
    // private float initialDistance = 0f;
    //
    // void Start()
    // {
    //     handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
    // }
    //
    // void Update()
    // {
    //     if (handSubsystem == null || !handSubsystem.running) return;
    //
    //     XRHand leftHand = handSubsystem.leftHand;
    //     XRHand rightHand = handSubsystem.rightHand;
    //
    //     // Prioridad: mover con una sola mano (pinch)
    //     // if (IsPinching(leftHand) && IsPinching(rightHand))
    //     // {
    //     //     // isMoving = false;
    //     //     Vector3 leftPos = GetJointPosition(leftHand, XRHandJointID.IndexTip);
    //     //     Vector3 rightPos = GetJointPosition(rightHand, XRHandJointID.IndexTip);
    //     //
    //     //     float currentDistance = Vector3.Distance(leftPos, rightPos);
    //     //
    //     //     if (!isScaling)
    //     //     {
    //     //         isScaling = true;
    //     //         initialDistance = currentDistance;
    //     //         initialScale = parentObjectTransform.localScale;
    //     //     }
    //     //
    //     //     float scaleFactor = currentDistance / initialDistance;
    //     //     parentObjectTransform.localScale = initialScale * scaleFactor;
    //     // }
    //     if (IsPinching(leftHand) && !IsPinching(rightHand))
    //     {
    //         isScaling = false;
    //         HandleOneHandMovement(leftHand);
    //     }
    //     else if (IsPinching(rightHand) && !IsPinching(leftHand))
    //     {
    //         isScaling = false;
    //         HandleOneHandMovement(rightHand);
    //     }
    //     // else
    //     // {
    //     //     isScaling = false;
    //     //     // isMoving = false;
    //     // }
    //     // else if (isMoving)
    //     // {
    //     //     isMoving = false;
    //     // }
    //     // else
    //     // {
    //     //     isMoving = false;
    //     // }
    // }
    //
    // void HandleOneHandMovement(XRHand hand)
    // {
    //     Vector3 pinchPos = GetJointPosition(hand, XRHandJointID.IndexTip);
    //
    //     // Prevención de conflictos: no mover si el pinch está tocando algo con collider interactuable
    //     if (Physics.CheckSphere(pinchPos, 0.02f, interactableLayer))
    //     {
    //         print("interactableLayer: "+interactableLayer);
    //         isMoving = false;
    //         return;
    //     }
    //
    //     if (!isMoving)
    //     {
    //         isMoving = true;
    //         handOffset = parentObjectTransform.position - pinchPos;
    //     }
    //
    //     parentObjectTransform.position = pinchPos + handOffset;
    // }
    //
    // bool IsPinching(XRHand hand)
    // {
    //     var indexTip = hand.GetJoint(XRHandJointID.IndexTip);
    //     var thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
    //
    //     if (!indexTip.TryGetPose(out Pose indexPose) || !thumbTip.TryGetPose(out Pose thumbPose))
    //         return false;
    //
    //     float distance = Vector3.Distance(indexPose.position, thumbPose.position);
    //     return distance < 0.025f;
    // }
    //
    // Vector3 GetJointPosition(XRHand hand, XRHandJointID jointID)
    // {
    //     var joint = hand.GetJoint(jointID);
    //     if (joint.TryGetPose(out Pose pose))
    //         return pose.position;
    //     return Vector3.zero;
    // }
    
    //////////////////////////////////////////////////////////////////////////
    // [SerializeField] private Transform solarSystem;
    //
    // private XRHandSubsystem handSubsystem;
    //
    // private bool isScaling = false;
    // private float initialDistance = 0f;
    // private Vector3 initialScale;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     if (handSubsystem == null || !handSubsystem.running) return;
    //
    //     var leftHand = handSubsystem.leftHand;
    //     var rightHand = handSubsystem.rightHand;
    //
    //     if (IsPinching(leftHand) && IsPinching(rightHand))
    //     {
    //         Vector3 leftPos = GetJointPosition(leftHand, XRHandJointID.IndexTip);
    //         Vector3 rightPos = GetJointPosition(rightHand, XRHandJointID.IndexTip);
    //
    //         float currentDistance = Vector3.Distance(leftPos, rightPos);
    //
    //         if (!isScaling)
    //         {
    //             isScaling = true;
    //             initialDistance = currentDistance;
    //             initialScale = solarSystem.localScale;
    //         }
    //
    //         float scaleFactor = currentDistance / initialDistance;
    //         solarSystem.localScale = initialScale * scaleFactor;
    //     }
    //     else
    //     {
    //         isScaling = false;
    //     }
    // }
    //
    // bool IsPinching(XRHand hand)
    // {
    //     XRHandJoint indexTip = hand.GetJoint(XRHandJointID.IndexTip);
    //     XRHandJoint thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
    //
    //     if (!indexTip.TryGetPose(out Pose indexPose) || !thumbTip.TryGetPose(out Pose thumbPose))
    //         return false;
    //
    //     float pinchDistance = Vector3.Distance(indexPose.position, thumbPose.position);
    //     return pinchDistance < 0.025f;
    // }
    //
    // Vector3 GetJointPosition(XRHand hand, XRHandJointID jointID)
    // {
    //     XRHandJoint joint = hand.GetJoint(jointID);
    //     if (joint.TryGetPose(out Pose pose))
    //         return pose.position;
    //
    //     return Vector3.zero;
    // }
    //
    // public void LeftPinch()
    // {
    //         
    // }
    //
    // public void RightPinch()
    // {
    //     
    // }
}
