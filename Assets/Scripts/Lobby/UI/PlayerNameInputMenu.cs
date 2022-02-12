using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class PlayerNameInputMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _playerName;
    [SerializeField]
    private Text _playerPassword;
    [SerializeField]
    private InputField _playerPasswordInputField;
    [SerializeField]
    private GameObject _playerPasswordConfirm;
    [SerializeField]
    private GameObject _playerEmail;
    [SerializeField]
    private InputField _playerPasswordConfirmInputField;
    [SerializeField]
    private InputField _playerEmailInputField;
    [SerializeField]
    private GameObject _errorMessageGameObject;

    // string url = "http://localhost:3000/enter";
    static string url = "http://ec2-54-92-242-20.compute-1.amazonaws.com:3000/enter";
    static string local_url = "http://localhost:3000/enter";
    static string access_type= "login";
    static string final_url = url + "?type=" + access_type;
    static bool errorMessageExists = false;
    static string errorMessage;
    static bool localTest = false;

    private RoomsCanvases _roomCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomCanvases = canvases;
    }
    const string playerNamePrefKey = "PlayerName";
  

    private void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
            PhotonNetwork.NickName = PlayerPrefs.GetString(playerNamePrefKey);
        }
        if(access_type == "login") {
            OnClick_SetLogin();
        } else {
            OnClick_SetSignup();
        }

        if(errorMessageExists) {
            setErrorMessage("");
        } else {
            disableErrorMessage();
        };
    }

    public void OnClick_SetPlayerName()
    {
        if (string.IsNullOrEmpty(_playerName.text) || string.IsNullOrEmpty(_playerPasswordInputField.text))
        {
            setErrorMessage("Please enter a name and password");
            Debug.LogError("Player Name or Password is null or empty");
            return;
        }

        if(access_type == "signup" && _playerPasswordInputField.text != _playerPasswordConfirmInputField.text) {
            setErrorMessage("Passwords do not match");
            // Debug.LogError("Passwords do not match");
            return;
        }

        StartCoroutine(SendRequest());

        // PhotonNetwork.NickName = _playerName.text;
        // Debug.Log("Player Name is "+ _playerName.text, this);

        // PlayerPrefs.SetString(playerNamePrefKey, _playerName.text);

        // _roomCanvases.AvatarSelectionCanvas.Show();
        // _roomCanvases.PlayerNameInputCanvas.Hide();
    }

    public void OnClick_SetLogin() {
        access_type = "login";
        // _playerPasswordConfirmInputField.enabled = false;
        // _playerEmailInputField.enabled = false;
        _playerPasswordConfirm.SetActive(false);
        _playerEmail.SetActive(false);
    }

    public void OnClick_SetSignup() {
        access_type = "signup";
        // _playerPasswordConfirmInputField.enabled = true;
        // _playerEmailInputField.enabled = true;
        _playerPasswordConfirm.SetActive(true);
        _playerEmail.SetActive(true);
    }

    public void setErrorMessage(string msg) {
        _errorMessageGameObject.SetActive(true);
        _errorMessageGameObject.GetComponentInChildren<Text>().text = msg;
        _errorMessageGameObject.GetComponentInChildren<Text>().color = Color.red;
    }

    public void disableErrorMessage() {
        _errorMessageGameObject.SetActive(false);
    }

    IEnumerator SendRequest() {
        WWWForm form = new WWWForm();
        form.AddField("name", _playerName.text);
        form.AddField("pw", _playerPasswordInputField.text);
        // form.AddField("pw_confirm", _playerPasswordConfirmInputField.text);
        form.AddField("email", _playerEmailInputField.text);

        final_url = url + "?type=" + access_type;
        if(localTest) final_url = local_url + "?type=" + access_type;

        UnityWebRequest uwr = UnityWebRequest.Post(final_url, form);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            JObject json = JObject.Parse(uwr.downloadHandler.text);
            Debug.Log("Result: " + json["result"]);
            if((bool) json["result"] == false) {
                setErrorMessage(json["message"].ToString());
                Debug.Log("Username Already Exists" + json["message"]);
            } else {
                PhotonNetwork.NickName = _playerName.text;
                Debug.Log("Player Name is "+ _playerName.text, this);

                PlayerPrefs.SetString(playerNamePrefKey, _playerName.text);

                _roomCanvases.AvatarSelectionCanvas.Show();
                _roomCanvases.PlayerNameInputCanvas.Hide();
            }
        }



        // List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        // // formData.Add(new MultipartFormDataSection("name"+_playerName.text+"&pw="+_playerPassword.text));
        // formData.Add(new MultipartFormDataSection("name", _playerName.text));
        // formData.Add(new MultipartFormDataSection("pw", _playerPassword.text));

        // UnityWebRequest www = UnityWebRequest.Post(url, formData);
        
        // yield return www.SendWebRequest();

        // if (www.result != UnityWebRequest.Result.Success)
        // {
        //     Debug.LogError(www.error);
        // } else {
        //     Debug.Log("Complete!");
        //     // JObject json = JObject.Parse(www.result);
        //     Debug.Log("Result:" + "\n" + www.result);
        //     connection = true;
        // }
    }

/*
    public void OnClick_JoinLobby()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        Debug.Log("Joining to Lobby...", this);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName, this);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        _roomCanvases.CreateOrJoinRoomCanvas.Show();
    }
*/
}
