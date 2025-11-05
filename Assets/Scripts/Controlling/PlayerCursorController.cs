using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controlling
{
	public class PlayerCursorController : MonoBehaviour
	{
		private PlayerInput _playerInput;
		private RacerCursor _cursor;
		
		private void OnEnable()
		{
			if (_playerInput != null)
				SubscribeToInputActions(_playerInput);
		}

		private void OnDisable()
		{
			if (_playerInput != null)
				UnsubscribeFromInputActions(_playerInput);
		}

		public void ConnectToCursor(RacerCursor cursor)
		{
			_cursor = cursor;
		}

		public void DisconnectFromCursor()
		{
			_cursor = null;
		}
		
		public void ConnectToInput(PlayerInput input)
		{
			if (_playerInput is not null)
			{
				DisconnectFromInput();
				Debug.LogWarning("Controller with already connected input is connecting to another one. That's not intended behaviour as of writing this warning");
			}
			
			_playerInput = input;
			SubscribeToInputActions(input);
		}

		public void DisconnectFromInput()
		{
			UnsubscribeFromInputActions(_playerInput);
			_playerInput = null;
		}
		
		private void SubscribeToInputActions(PlayerInput input)
		{
			InputAction moveAction = input.actions["MoveCursor"];
			moveAction.performed += OnMoveCursor;
			moveAction.canceled += OnMoveCursor;
			
			InputAction placeAction = input.actions["PlaceItem"];
			placeAction.canceled += OnPlaceItem;
		}

		private void UnsubscribeFromInputActions(PlayerInput input)
		{
			InputAction moveAction = input.actions["MoveCursor"];
			moveAction.performed -= OnMoveCursor;
			moveAction.canceled -= OnMoveCursor;
			
			InputAction placeAction = input.actions["PlaceItem"];
			placeAction.performed -= OnPlaceItem;
		}


		private void OnPlaceItem(InputAction.CallbackContext context)
		{
			_cursor?.PlaceItem();
		}
		
		private void OnMoveCursor(InputAction.CallbackContext context)
		{
			Vector2 delta = context.ReadValue<Vector2>();
			print(delta);
			_cursor?.Move(delta);
		}
	}
}
