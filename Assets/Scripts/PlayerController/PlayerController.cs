using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float playerRunMaxSpeed;
    [SerializeField] private float playerWalkMaxSpeed;
    [SerializeField] private CurveField runAccelerationCurve;
    [SerializeField] private CurveField walkAccelerationCurve;
    [SerializeField] private float timeToMaxSpeed;
    [SerializeField] private float rotationSpeed;
    
    
    private Rigidbody _rb;

    private Vector3 _playerMoveInput;

    public enum PlayerState
    {
        Normal,
        Rolling,
        Attacking,
        Parrying,
        TakingDamage
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        if (_playerMoveInput == Vector3.zero) return;
        Vector3 relative = (transform.position + _playerMoveInput.ToIso()) - transform.position;
        Quaternion rot = Quaternion.LookRotation(relative, Vector3.up);
            
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + transform.forward * (_playerMoveInput.magnitude * playerRunMaxSpeed * Time.deltaTime));
    }

    #region Input Gathering

    public void OnMove(InputValue inputValue)
    {
        Vector3 inputs = new Vector3(inputValue.Get<Vector2>().x, 0, inputValue.Get<Vector2>().y);
        _playerMoveInput = inputs;
    }

    #endregion

}
