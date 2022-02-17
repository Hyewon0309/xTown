using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;
public class BasketBallJoin : MonoBehaviourPunCallbacks
{
	/* public fields */
	[SerializeField]
	private GameObject _joinPanel;

	[SerializeField]
    private GameObject _mainCanvas;
	[SerializeField]
	private CamManager _camManager;
	private byte _maxPlayersPerRoom = 1;
		//[SerializeField]
		//private Text _playerNumText;
		//private int _currentPlayers = 0;
	private int _currentRoomNum = 0;
    private string _sceneName = "BasketBall";

		/* private fields */

		// required so different version users cannot play together
	private string _gameVersion = "0";
	private bool isBasketBallConnecting = false;


		/* Monobehaviour callbacks */

	void Awake()
	{
		_joinPanel.SetActive(false);
		PhotonNetwork.AutomaticallySyncScene = true;
	}
    private void OnTriggerEnter(Collider other)
        {
			if (other.tag == "Player")
			{
					_joinPanel.SetActive(true);
					isBasketBallConnecting = true;

			}
			//_playerNumText.text = _currentPlayers+"/2 players waiting...";
		}

	/* public connection management methods */

	public void Connect()
	{
		_joinPanel.SetActive(false);
		_mainCanvas.SetActive(false);
		_camManager.enabled = false;
		PlayerPrefs.SetString("PastScene", "MainRoom");
        PhotonNetwork.LeaveRoom();
	}
		/* Pun Callbacks */
        /*
		public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				Debug.Log("Yacht/PhotonLauncher: OnConnectedToMaster");
				PhotonNetwork.JoinRandomRoom();
			}
		}*/
		/*public override void OnJoinedLobby()
		{
			if (isYachtConnecting)
			{
				//if(_currentPlayers%2 == 1)
				//{
				RoomOptions options = new RoomOptions();
				options.BroadcastPropsChangeToAll = true;
				options.MaxPlayers = 2;
				PhotonNetwork.JoinOrCreateRoom(_sceneName, options, TypedLobby.Default);
				//}
				//else if(_currentPlayers%2 == 0)
				//{
				//	PhotonNetwork.JoinRoom(_sceneName);
				//}
			}
		}*/
	public override void OnCreatedRoom()
	{
		if(isBasketBallConnecting)
		{
			Debug.Log("BasketBallJoin: Joined Room");
			PhotonNetwork.LoadLevel("Basketball");
		}
	}
}
