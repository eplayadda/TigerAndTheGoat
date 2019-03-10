using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eNodeHolder
{
	none = 0,
	tiger,
	goat}
;

public class TGNode : MonoBehaviour
{
	public int ID;
	public bool isCornner;
	public List <BranchTGNode> branchTgNodes;
	public eNodeHolder currNodeHolder;
	public Image nodeHolderSprint;

	public void SetNodeHolderSprint ()
	{
		Invoke ("SetImage", .001f);
	}

	void SetImage ()
	{
		nodeHolderSprint.GetComponent<Image> ().preserveAspect = true;
		if (currNodeHolder == eNodeHolder.goat) {
			nodeHolderSprint.sprite = BordManager.instace.goatTexture;
			nodeHolderSprint.enabled = true;
            nodeHolderSprint.transform.localScale = new Vector3(1.8f,1.8f,1.8f);
		} else if (currNodeHolder == eNodeHolder.tiger) {
			nodeHolderSprint.enabled = true;
			nodeHolderSprint.sprite = BordManager.instace.tigerTexture;
            nodeHolderSprint.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else {
			nodeHolderSprint.enabled = false;
            nodeHolderSprint.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}

[System.Serializable]
public class BranchTGNode
{
	public TGNode firstLayerNode;
	public TGNode secondLayerNode;
}