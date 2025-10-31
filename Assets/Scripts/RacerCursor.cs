using System;
using UnityEngine;

public class RacerCursor : MonoBehaviour
{
	[SerializeField] private GameObject _ItemPrefab;
	
	public bool _IsItemPlaced;
	public Action OnItemPlaced;
	
	public void Move(Vector2 move)
	{
		transform.position += (Vector3)move;
	}

	public void PlaceItem()
	{
		GameObject item = Instantiate(_ItemPrefab, transform.position, Quaternion.identity);

		_IsItemPlaced = true;
		OnItemPlaced?.Invoke();	
		gameObject.SetActive(false);
	}
}