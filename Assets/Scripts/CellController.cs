using UnityEngine;
using System.Collections;

public class CellController : MonoBehaviour {

	public enum States {
		Dead, Living
	}

	public Material aliveMaterial;
	public Material deadMaterial;
	[HideInInspector] public Game gol;
	[HideInInspector] public int x, y;
	[HideInInspector] public CellController[] neighbours;
	[HideInInspector] public States state;
	private States nextState;
	private MeshRenderer meshRenderer;

	void Awake () {
		meshRenderer = GetComponent <MeshRenderer> ();
	}

	public void Init (Game game, int x, int y) {
		gol = game;
		transform.parent = game.transform;
		this.x = x;
		this.y = y;
	}

  	public void CellUpdate () {
		nextState = state;
		int livingCells = GetLivingCells ();
		if (state == States.Living) { 
			if (livingCells != 2 && livingCells != 3) 
				nextState = States.Dead;
		} else { 
			if (livingCells == 3) 
				nextState = States.Living;
		}
	}
		

	public void CellApplyUpdate () {
		state = nextState;
		UpdateMaterial ();
	}

	public void SetRandomState () {		
		state = (Random.Range (0, 2) == 0) ? States.Dead : States.Living;
		UpdateMaterial ();
	}

	private void UpdateMaterial () {		
		if (state == States.Living)
			meshRenderer.sharedMaterial = aliveMaterial;
		else
			meshRenderer.sharedMaterial = deadMaterial;
	}

	private int GetLivingCells () {		
		int ret = 0;
		for (int i = 0; i < neighbours.Length; i++) {
			if (neighbours[i] != null && neighbours [i].state == States.Living)
				ret++;
		}
		return ret;	
	}
}