using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpPlayer : MonoBehaviour
{
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private Transform pickUpParent;
    [SerializeField] private float playerSpeed = 3f;
    private Stack<Pickup> _pickups = new Stack<Pickup>();
    private Rigidbody _rigidbody;
    private PlayerControllerActions _playerInputActions;
    private Vector3 _playerMovementDirection;
    private Vector2 _PlayerInputDirection;
    private bool isTouching;

    private Vector3 _currentPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInputActions = new PlayerControllerActions();

        _playerInputActions.Player.Move.performed += context => _playerMovementDirection = context.ReadValue<Vector3>();
        _playerInputActions.Player.Move.canceled += context => _playerMovementDirection = Vector3.zero;


        _playerInputActions.Player.TouchPosition.performed +=
            context => _PlayerInputDirection = context.ReadValue<Vector2>();

        _playerInputActions.Player.TouchInput.performed += context => isTouching = true;
        _playerInputActions.Player.TouchInput.canceled += context => isTouching = false;
    }

    private void Start()
    {
        _currentPosition = _rigidbody.position;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    private void Update()
    {
        var maxXValue = Mathf.Clamp(_rigidbody.position.x, -5, 5);
        Vector3 currentPosition = new Vector3(maxXValue, _currentPosition.y, _currentPosition.z);

        if (_playerMovementDirection != Vector3.zero)
            _rigidbody.MovePosition(currentPosition + _playerMovementDirection * playerSpeed * Time.deltaTime);


        if (_PlayerInputDirection.x > 500f && isTouching)
            _rigidbody.MovePosition(currentPosition + Vector3.right * playerSpeed * Time.deltaTime);
        else if (_PlayerInputDirection.x < 500f && isTouching)
            _rigidbody.MovePosition(currentPosition + Vector3.left * playerSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            if (_pickups.Count == 0)
            {
                ChangeParentAndPositionOfPickUp(other, 0);
                AddPickUpItemToStack(other);
            }
            else
            {
                ChangeParentAndPositionOfPickUp(other, _pickups.Count + 1);
                AddPickUpItemToStack(other);
            }
        }

        if (other.CompareTag("Drop"))
        {
            if (_pickups.Count > 0)
            {
                other.GetComponent<Pickup>().DropPickUp();
                DropPickUpItemToStack(other);
            }
        }
    }

    private void ChangeParentAndPositionOfPickUp(Collider other, float offSet)
    {
        Transform _pickUpTransform = other.transform;
        _pickUpTransform.SetParent(pickUpParent);
        _pickUpTransform.position = pickUpTransform.position + new Vector3(0, offSet, 0);
    }

    private void AddPickUpItemToStack(Collider other)
    {
        Pickup currentPickup = other.GetComponent<Pickup>();
        currentPickup.SetKinematic();
        _pickups.Push(currentPickup);
    }

    private void DropPickUpItemToStack(Collider other)
    {
        var lastPickUpItem = _pickups.Peek();
        lastPickUpItem.DropPickUp();
        
        _pickups.Pop();
    }
}