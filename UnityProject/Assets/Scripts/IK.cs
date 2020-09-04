using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IK : MonoBehaviour
{
    #region Variables
    private Animator _animator;

    private Vector3 _rightFootPosition, _leftFootPosition, _leftFootIkPosition, _rightFootIkPosition;
    private Quaternion _leftFootIkRotation, _rightFootIkRotation;
    private float _lastPelvisPositionY, _lastRightFootPositionY, _lastLeftFootPositionY;

    [Header("Feet Grounder")]
    public bool EnableFeetIk = true;
    [Range(0, 2)] [SerializeField] private float _heightFromGroundRaycast = 1.14f;
    [Range(0, 2)] [SerializeField] private float _raycastDownDistance = 1.5f;
    [SerializeField] private LayerMask _environmentLayer;
    [SerializeField] private float _pelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] private float _pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)] [SerializeField] private float _feetToIkPositionSpeed = 0.5f;

    public string LeftFootAnimVariableName = "LeftFootCurve";
    public string RightFootAnimVariableName = "RightFootCurve";

    public bool UseProIkFeature = false;
    public bool ShowSolverDebug = true;

    #endregion Variables

    private void Start()
    {
        _animator = GetComponent<Animator>();

    }

    /// <summary>
    /// We are updating the AdjustFeetTarget method and also find the position of each foot inside our Solver Position
    /// </summary>
    void FixedUpdate()
    {

        //FeetGrounding
        if (!EnableFeetIk)
        {
            return;
        }

        AdjustFeetTarget(ref _rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref _leftFootPosition, HumanBodyBones.LeftFoot);

        //find and raycast to the ground to find positions
        FeetPositionSolver(_rightFootPosition, ref _rightFootIkPosition, ref _rightFootIkRotation);
        FeetPositionSolver(_leftFootPosition, ref _leftFootIkPosition, ref _leftFootIkRotation);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        
        if (!EnableFeetIk)
        {
            return;
        }
        MovePelvisHeight();

        //Right foot ik position and rotation -- utilise the pro features in here
        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        if (UseProIkFeature)
        {
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _animator.GetFloat(RightFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.RightFoot, _rightFootIkPosition, _rightFootIkRotation, ref _lastRightFootPositionY);

        //Left foot ik position and rotation -- utilise the pro features in here
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

        if (UseProIkFeature)
        {
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat(LeftFootAnimVariableName));
        }

        MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, _leftFootIkPosition, _leftFootIkRotation, ref _lastLeftFootPositionY);
    }

    private void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY)
    {
        Vector3 targetIkPosition = _animator.GetIKPosition(foot);

        if (positionIkHolder != Vector3.zero)
        {
            targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
            positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, _feetToIkPositionSpeed);
            targetIkPosition.y += yVariable;

            lastFootPositionY = yVariable;
            targetIkPosition = transform.TransformPoint(targetIkPosition);

            _animator.SetIKRotation(foot, rotationIkHolder);
        }

        _animator.SetIKPosition(foot, targetIkPosition);
    }

    private void MovePelvisHeight()
    {
        if (_rightFootIkPosition == Vector3.zero || _leftFootIkPosition == Vector3.zero || _lastPelvisPositionY == 0)
        {
            _lastPelvisPositionY = _animator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = _leftFootIkPosition.y - transform.position.y;
        float rightOffsetPosition = _rightFootIkPosition.y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelivsPosition = _animator.bodyPosition + Vector3.up * totalOffset;

        newPelivsPosition.y = Mathf.Lerp(_lastPelvisPositionY, newPelivsPosition.y, _pelvisUpAndDownSpeed);

        _animator.bodyPosition = newPelivsPosition;
        _lastPelvisPositionY = _animator.bodyPosition.y;
    }

    /// <summary>
    /// We are locatio nthe feet position via a Raycast and than solving
    /// </summary>
    /// <param name="fromSkyPosition"></param>
    /// <param name="feetIkPositions"></param>
    /// <param name="feetIkRotations"></param>
    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPositions, ref Quaternion feetIkRotations)
    {
        //raycast handling section
        RaycastHit feetOutHit;

        if (ShowSolverDebug)
        {
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (_raycastDownDistance + _heightFromGroundRaycast), Color.yellow);
        }

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, _raycastDownDistance + _heightFromGroundRaycast, _environmentLayer))
        {
            //finding our feet ik positions from the sky position
            feetIkPositions = fromSkyPosition;
            feetIkPositions.y = feetOutHit.point.y + _pelvisOffset;
            feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;
            return;
        }

        feetIkPositions = Vector3.zero;
    }

    private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    {
        feetPositions = _animator.GetBoneTransform(foot).position;
        feetPositions.y = transform.position.y + _heightFromGroundRaycast;
    }
}
