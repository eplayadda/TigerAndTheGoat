using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tg_FightAI : MonoBehaviour
{
	public static Tg_FightAI instance;
	BordManager bordManager;
	GameManager gameManager;
	List <int> aiMove = new List<int> ();

	void Start ()
	{
		if (instance == null)
			instance = this;
		bordManager = BordManager.instace;
		gameManager = GameManager.instance;
		//Debug.Log ("Chadnan");
	}

	public List<int> GetTigerNextMove ()
	{
		int indexTo = -1;
		int indexFrom = -1;
		List <int> tempTo = new List<int> ();
		List <int> tempFrom = new List<int> ();
		aiMove.Clear ();
		tempTo.Clear ();
		tempFrom.Clear ();
		foreach (TGNode node in bordManager.allTgNodes) {
			if (node.currNodeHolder == eNodeHolder.tiger) {
				foreach (BranchTGNode brNodes in node.branchTgNodes) {
					if (brNodes != null) {
						if (brNodes.firstLayerNode.currNodeHolder == eNodeHolder.goat && brNodes.secondLayerNode != null &&
						    brNodes.secondLayerNode.currNodeHolder == eNodeHolder.none) {
							indexTo = brNodes.secondLayerNode.ID;
							indexFrom = node.ID;
							break;
						} else if (brNodes.firstLayerNode.currNodeHolder == eNodeHolder.none) {
							tempTo.Add (brNodes.firstLayerNode.ID);
							tempFrom.Add (node.ID);
						}
					}
				}
				if (indexTo > 0)
					break;
				
			}
		}
		if (indexTo < 0) {
			int a = UnityEngine.Random.Range (0, tempTo.Count);
			try {
				indexTo = tempTo [a];
				indexFrom = tempFrom [a];
			} catch (Exception e) {
			}
		}
		//	if (tempTo.Count > 0) {
		aiMove.Add (indexFrom);
		aiMove.Add (indexTo);
		//	}
		return aiMove;
	}

	public List<int> GetGoatNextMove ()
	{
		int indexTo = -1;
		int indexFrom = -1;
		List <int> tempTo = new List<int> ();
		List <int> tempFrom = new List<int> ();
		aiMove.Clear ();
		tempTo.Clear ();
		tempFrom.Clear ();

		List <TGNode> cornnerNode = new List<TGNode> ();
		List <TGNode> emptyNode = new List<TGNode> ();
		List<TGNode> nearTiger = new List<TGNode> ();

		if (bordManager.noOfGoat < gameManager.totalNoOfGoat) {
			// If Goat is less then total number of Gaot allowed
			foreach (TGNode item in bordManager.allTgNodes) {
				if (item.currNodeHolder == eNodeHolder.none) {
					emptyNode.Add (item);
					if (item.isCornner)
						cornnerNode.Add (item);
				}
			}
			foreach (TGNode item in cornnerNode) {
				tempTo.Add (item.ID);
			}
			if (tempTo.Count > 0) {
				return AiGoatMove (tempTo);
			}
			tempTo.Clear ();
			foreach (TGNode item in emptyNode) {
				if (SafeForGoat (item))
					tempTo.Add (item.ID);
			}
			if (tempTo.Count > 0) {
				return AiGoatMove (tempTo);
			}
			tempTo.Clear ();
			foreach (TGNode item in emptyNode) {
				tempTo.Add (item.ID);
			}
			if (tempTo.Count > 0) {
				return AiGoatMove (tempTo);
			}
		} else {
			// If tota; number of Goat used

			// Checking Goat which is near by Tiger 
			foreach (TGNode item in bordManager.allTgNodes) {
				if (item.currNodeHolder == eNodeHolder.none)
					emptyNode.Add (item);
				if (item.currNodeHolder == eNodeHolder.goat) {
					foreach (BranchTGNode brItem in item.branchTgNodes) {
						if (brItem.firstLayerNode.currNodeHolder == eNodeHolder.tiger) {
							nearTiger.Add (item);
						}
					}
				}
			}
			foreach (TGNode item in nearTiger) {
				foreach (BranchTGNode br in item.branchTgNodes) {
					if (br.firstLayerNode.currNodeHolder == eNodeHolder.none) {
						tempTo.Add (br.firstLayerNode.ID);
						tempFrom.Add (item.ID);
					}
				}
			}
			if (tempTo.Count > 0) {
				Debug.Log ("fjgh");
				return AiGoatMove (tempTo, tempFrom);
			}
			tempTo.Clear ();
			tempFrom.Clear ();
			foreach (TGNode item in bordManager.allTgNodes) {
				if (item.currNodeHolder == eNodeHolder.goat) {
					foreach (BranchTGNode brItem in item.branchTgNodes) {
						if (brItem.firstLayerNode.currNodeHolder == eNodeHolder.none) {
							tempTo.Add (brItem.firstLayerNode.ID);
							tempFrom.Add (item.ID);
						}
					}
				}
			}
			if (tempTo.Count > 0) {
				Debug.Log ("fjgh");
				return AiGoatMove (tempTo, tempFrom);
			}
		}


		return aiMove;
	}

	List<int> AiGoatMove (List<int> pDataTo, List<int> pDataFrom = null)
	{
		int a = UnityEngine.Random.Range (0, pDataTo.Count);
		aiMove.Add (pDataTo [a]);
		if (pDataFrom != null)
			aiMove.Add (pDataFrom [a]);
		return aiMove;
	}

	bool SafeForGoat (TGNode node)
	{
		bool isSafe = true;
		foreach (BranchTGNode item in node.branchTgNodes) {
			if (item.firstLayerNode.currNodeHolder == eNodeHolder.tiger) {
				isSafe = false;
				break;
			}
		}
		return isSafe;
	}

}
