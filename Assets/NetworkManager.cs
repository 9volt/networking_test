using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	private float btnX;
	private float btnY;
	private float btnW;
	private float btnH;
	private string game_name = "thenmal_network_test";
	private bool refreshing = false;
	private float end_time;
	private HostData[] servers = new HostData[0];
	public GameObject mage;
	public GameObject spawn;

	// Use this for initialization
	void Start () {
		btnX = Screen.width * 0.05f;
		btnY = Screen.width * 0.05f;
		btnW = Screen.width * 0.1f;
		btnH = Screen.width * 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(refreshing){
			if(MasterServer.PollHostList().Length > 0){
				refreshing = false;
				Debug.Log(MasterServer.PollHostList().Length);
				servers = MasterServer.PollHostList();
			} else if(Time.time > end_time){
				Debug.Log("No server found");
				refreshing = false;
			}
		}
	}

	void StartServer(){
		Network.InitializeServer(4, 25025, !Network.HavePublicAddress());
		MasterServer.RegisterHost(game_name, "Thenmal Networking Test", "DO NOT EAT");
	}

	void OnMasterServerEvent(MasterServerEvent mse){
		if(mse == MasterServerEvent.RegistrationSucceeded){
			Debug.Log ("Server succesfully registered");
		}
	}

	void RefreshHostList(){
		MasterServer.RequestHostList(game_name);
		refreshing = true;
		end_time = Time.time + 10f;
		MasterServer.PollHostList();
	}

	void OnServerInitialized(){
		Debug.Log("Server initialized");
		SpawnPlayer();
	}

	void OnPlayerConnected(){
		Debug.Log("A Player Joined");
	}

	void SpawnPlayer(){
		Network.Instantiate(mage, spawn.transform.position, spawn.transform.rotation, 0);
	}

	void OnConnectedToServer(){
		SpawnPlayer();
	}

	void OnGUI(){
		if(!Network.isClient && !Network.isServer){
			if(GUI.Button(new Rect(btnX, btnY, btnW, btnH), "Start Server")){
				Debug.Log("Starting Server");
				StartServer();
			}
			if(GUI.Button(new Rect(btnX, btnY * 1.2f + btnH, btnW, btnH), "Refresh Hosts")){
				Debug.Log("Refreshing Hosts");
				RefreshHostList();
			}
			for(int i = 0; i < servers.Length; i++){
				if(GUI.Button(new Rect(btnX * 1.5f + btnW, btnY * 1.2f + btnH + (btnH * i), btnW * 3f, btnH), string.Join(".", servers[i].ip) + " : " + servers[i].connectedPlayers)){
					Network.Connect(servers[i]);
				}
			}
		}
	}
}
