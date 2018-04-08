using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	public static InputHandler instance;
	void Awake()
	{
		if (instance == null)
			instance = this;
	}
	public void OnInputTaken(int pData)
	{
		if (!GameManager.instance.isAllowPlay)
			QuitGame ();
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);

		if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer && GameManager.instance.currTurnStatus != eTurnStatus.my)
			return;
		if (GameManager.instance.currGameMode == eGameMode.vCPU && GameManager.instance.currTurnStatus != eTurnStatus.my)
			return;
        BordManager.instace.OnInputByUser(pData);
		if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer)
			ConnectionManager.Instance.OnSendMeAnswer (pData+"");
	}
	public void OnInputTakenBYServer(int pData)
	{
		if (!GameManager.instance.isAllowPlay)
			QuitGame ();
		BordManager.instace.OnInputByUser(pData);
//		GameManager.instance.currTurnStatus = eTurnStatus.my;

	}
	void QuitGame()
	{
		Application.Quit ();
	}
}
