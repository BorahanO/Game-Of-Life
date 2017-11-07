using UnityEngine;
using System.Collections;
using System;

public class Game : MonoBehaviour {

	public enum States {
		Running
	}

	public CellController cellPrefab;
	public float cellUpdateTime = 0.1f; 
	[HideInInspector] public CellController[,] cells;
	[HideInInspector] public States state;
	[HideInInspector] public int cellHeight; 
	[HideInInspector] public int cellWidth; 
	private Action cellUpdates; 
	private Action cellApplyUpdates; 
	private IEnumerator coroutine; 

	void Start () {		
		Init (23, 23); 
		Run (); 
	}

	public void Init (int x, int y) {		
		if (cells != null) {
			for (int i = 0; i < cellHeight; i++) {
				for (int j = 0; j < cellWidth; j++) {
					GameObject.Destroy (cells [i, j].gameObject);
				}
			}
		}			
		cellUpdates = null;
		cellApplyUpdates = null;
		coroutine = null;
		cellHeight = x;
		cellWidth = y;
		SpawnCells (cellHeight, cellWidth);
	}
		
	public void UpdateCells () {		
		cellUpdates ();
		cellApplyUpdates ();
	}

	public void SpawnCells (int x, int y) {		
		cells = new CellController[x, y]; 
		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {
				CellController c = Instantiate (cellPrefab, new Vector3 ((float)i, (float)j, 0f), Quaternion.identity) as CellController; 
				cells [i, j] = c;
				c.Init (this, i, j); 
				c.SetRandomState (); 
				cellUpdates += c.CellUpdate;
				cellApplyUpdates += c.CellApplyUpdate;
			}	
		}

		for (int i = 0; i < x; i++) {			
			for (int j = 0; j < y; j++) {
				cells [i, j].neighbours = GetNeighbours (i, j);
			}
		}
	}
		
	public CellController[] GetNeighbours (int x, int y) {		
		CellController[] result = new CellController[8];
		result[0] = cells[x, (y + 1) % cellWidth];
		result[1] = cells[(x + 1) % cellHeight, (y + 1) % cellWidth]; 
		result[2] = cells[(x + 1) % cellHeight, y % cellWidth]; 
		result[3] = cells[(x + 1) % cellHeight, (cellWidth + y - 1) % cellWidth]; 
		result[4] = cells[x % cellHeight, (cellWidth + y - 1) % cellWidth]; 
		result[5] = cells[(cellHeight + x - 1) % cellHeight, (cellWidth + y - 1) % cellWidth]; 
		result[6] = cells[(cellHeight + x - 1) % cellHeight, y % cellWidth]; 
		result[7] = cells[(cellHeight + x - 1) % cellHeight, (y + 1) % cellWidth]; 
		return result;
	}
		
	public void Run () {		
		state = States.Running;
		if (coroutine != null)
			StopCoroutine (coroutine);
		coroutine = RunCoroutine ();
		StartCoroutine (coroutine);
	}

	private IEnumerator RunCoroutine () {		
		while (state == States.Running) { 
			UpdateCells (); 
			yield return new WaitForSeconds (cellUpdateTime); 
		}
	}
}