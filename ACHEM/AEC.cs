using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Assets.Scripts.NetworkClient.CommClasses;
using Newtonsoft.Json;
using UnityEngine;

public class AEC : MonoBehaviour
{
	private class AECEvent
	{
		private Action<string> callback;

		public string Message;

		public AECEvent(Action<string> callback, string message)
		{
			this.callback = callback;
			Message = message;
		}

		public void Fire()
		{
			callback(Message);
		}
	}

	protected List<Response> inputQ = new List<Response>();

	protected readonly object lockInputQ = new object();

	private List<AECEvent> eventQ = new List<AECEvent>();

	protected readonly object lockEventtQ = new object();

	private static JsonSerializerSettings serializersetting = new JsonSerializerSettings
	{
		DefaultValueHandling = DefaultValueHandling.Ignore,
		NullValueHandling = NullValueHandling.Ignore
	};

	private static JsonSerializerSettings deserializersetting = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.Auto,
		Binder = new TypeNameSerializationBinder()
	};

	public static bool IsLogEnabled;

	private static AEC instance;

	private int id;

	private string ip;

	private DateTime lastDisconnectMessageTimeStamp;

	private string disconnectMessage;

	private Socket socket;

	private int readBufferSize = 1024;

	private byte[] readBuffer;

	private int maxMessageLength = 2048;

	private MemoryStream messageStream;

	private List<byte[]> dataResponses;

	private static ManualResetEvent connectDone = new ManualResetEvent(initialState: false);

	public int ServerID => id;

	public int ID => id;

	public string IP => ip;

	public event Action<string> OnConnect;

	public event Action<string> OnDisconnect;

	public event Action<Response> ResponseReceived;

	public static AEC getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		lock (eventQ)
		{
			while (eventQ.Count > 0)
			{
				AECEvent aECEvent = eventQ[0];
				eventQ.RemoveAt(0);
				aECEvent.Fire();
			}
		}
		while (GetQueueSize() > 0)
		{
			Response response = GetResponse();
			if (response != null)
			{
				OnResponseReceived(response);
			}
		}
	}

	private int GetQueueSize()
	{
		lock (lockInputQ)
		{
			return inputQ.Count;
		}
	}

	private Response GetResponse()
	{
		lock (lockInputQ)
		{
			if (inputQ.Count > 0)
			{
				Response result = inputQ[0];
				inputQ.RemoveAt(0);
				return result;
			}
		}
		return null;
	}

	private void queueResponse(Response r)
	{
		lock (lockInputQ)
		{
			inputQ.Add(r);
		}
	}

	private void queueEvent(AECEvent evt)
	{
		if (DateTime.UtcNow.Subtract(lastDisconnectMessageTimeStamp) < new TimeSpan(0, 0, 1))
		{
			evt.Message = disconnectMessage;
			disconnectMessage = null;
			lastDisconnectMessageTimeStamp = DateTime.MinValue;
		}
		lock (eventQ)
		{
			eventQ.Add(evt);
		}
	}

	protected void callConnectDelegate(string message)
	{
		if (this.OnConnect != null)
		{
			this.OnConnect(message);
		}
	}

	protected void DisconnectCallback(string message)
	{
		if (this.OnDisconnect != null)
		{
			this.OnDisconnect(message);
		}
	}

	protected void OnResponseReceived(Response r)
	{
		if (this.ResponseReceived != null)
		{
			try
			{
				this.ResponseReceived(r);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	public void connect(string hostname, int port, int serverid)
	{
		if (socket != null && socket.Connected)
		{
			queueEvent(new AECEvent(callConnectDelegate, "Failure: Socket had already been connected."));
			return;
		}
		Debug.Log("CS AEC Init");
		readBuffer = new byte[readBufferSize];
		messageStream = new MemoryStream();
		dataResponses = new List<byte[]>();
		id = serverid;
		ip = hostname;
		try
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
			if (hostEntry.AddressList.Length == 0)
			{
				Debug.Log("Could not resolve server DNS name: AddressList Length zero");
				queueEvent(new AECEvent(callConnectDelegate, "Hostname could not be resolved. Please check your internet connection and try again."));
				return;
			}
			for (int i = 0; i < hostEntry.AddressList.Length; i++)
			{
				IPAddress iPAddress = hostEntry.AddressList[i];
				Debug.Log("Resolved address: " + iPAddress.AddressFamily.ToString() + " " + iPAddress.ToString() + ":" + port);
				if ((iPAddress.AddressFamily != AddressFamily.InterNetworkV6 || !Socket.OSSupportsIPv6) && (iPAddress.AddressFamily != AddressFamily.InterNetwork || !Socket.OSSupportsIPv4))
				{
					continue;
				}
				try
				{
					Debug.Log("Connecting to: " + iPAddress.AddressFamily.ToString() + " " + iPAddress.ToString() + ":" + port);
					socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					IPEndPoint remoteEP = new IPEndPoint(iPAddress, port);
					socket.BeginConnect(remoteEP, handleConnect, socket);
					connectDone.Reset();
					if (!connectDone.WaitOne(1000, exitContext: true))
					{
						throw new Exception("Connection timed out.");
					}
					socket.BeginReceive(readBuffer, 0, readBufferSize, SocketFlags.None, handleData, socket);
					queueEvent(new AECEvent(callConnectDelegate, "Success"));
					return;
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
					socket.Close();
					socket = null;
				}
			}
			Debug.Log("Error: Could not connect to resolved address.");
			queueEvent(new AECEvent(callConnectDelegate, "The game is unable to connect to the server. Please try again later!"));
		}
		catch (SocketException ex2)
		{
			if (ex2.ErrorCode == 11001)
			{
				queueEvent(new AECEvent(callConnectDelegate, "Hostname could not be resolved. Please check your internet connection and try again."));
				return;
			}
			Debug.LogException(ex2);
			queueEvent(new AECEvent(callConnectDelegate, "The game is unable to connect to the server. Please try again later!"));
		}
	}

	private void handleConnect(IAsyncResult ar)
	{
		try
		{
			((Socket)ar.AsyncState).EndConnect(ar);
			connectDone.Set();
		}
		catch (Exception ex)
		{
			if (!(ex is ObjectDisposedException))
			{
				Debug.LogException(ex);
			}
		}
	}

	public void close()
	{
		try
		{
			if (socket == null)
			{
				return;
			}
			if (socket.Connected)
			{
				try
				{
					socket.Shutdown(SocketShutdown.Both);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			socket.Close();
			socket = null;
		}
		catch (Exception exception2)
		{
			Debug.LogException(exception2);
		}
	}

	private void handleData(IAsyncResult ar)
	{
		int num = 0;
		try
		{
			if (ar.AsyncState is Socket socket)
			{
				num = socket.EndReceive(ar, out var errorCode);
				if (errorCode != 0)
				{
					Debug.LogException(new Exception("AQ3DException: Socket.EndReceive(): SocketError:" + errorCode));
				}
			}
		}
		catch (SocketException ex)
		{
			if (ex.ErrorCode != 10054)
			{
				Debug.LogError("Socket ErrorCode:" + ex.ErrorCode);
				Debug.LogException(ex);
			}
			queueEvent(new AECEvent(DisconnectCallback, "Connection to the server lost. Please check your internet connection and try again."));
			return;
		}
		catch (Exception ex2)
		{
			if (!(ex2 is ObjectDisposedException))
			{
				Debug.LogException(ex2);
			}
		}
		if (num <= 0)
		{
			Debug.Log("0 bytes received!");
			close();
			queueEvent(new AECEvent(DisconnectCallback, "Connection to the server lost. Please check your internet connection and try again."));
			return;
		}
		int num2 = 0;
		int num3 = 0;
		while ((num3 = Array.IndexOf(readBuffer, (byte)0, num3)) > -1 && num3 < num)
		{
			messageStream.Write(readBuffer, num2, num3 - num2);
			num3++;
			num2 = num3;
			dataResponses.Add(messageStream.ToArray());
			messageStream = new MemoryStream(maxMessageLength);
		}
		if (num2 < num)
		{
			messageStream.Write(readBuffer, num2, num - num2);
		}
		while (dataResponses.Count > 0)
		{
			byte[] data = dataResponses[0];
			dataResponses.RemoveAt(0);
			WrapAndQueueResponse(data);
		}
		this.socket.BeginReceive(readBuffer, 0, readBufferSize, SocketFlags.None, handleData, this.socket);
	}

	private void sendMessage(byte[] message)
	{
		byte[] array = new byte[message.Length + 1];
		Buffer.BlockCopy(message, 0, array, 0, message.Length);
		array[message.Length] = 0;
		if (socket == null)
		{
			Debug.LogError("Error: Attempting to send on null socket.");
			queueEvent(new AECEvent(DisconnectCallback, "Connection to the server lost. Please check your internet connection and try again."));
		}
		else if (!socket.Connected)
		{
			Debug.LogError("Error: Socket is not connected.");
			queueEvent(new AECEvent(DisconnectCallback, "Connection to the server lost. Please check your internet connection and try again."));
		}
		else
		{
			socket.BeginSend(array, 0, array.Length, SocketFlags.None, sendCallback, socket);
		}
	}

	private void sendCallback(IAsyncResult ar)
	{
		try
		{
			((Socket)ar.AsyncState).EndSend(ar);
		}
		catch (Exception ex)
		{
			if (!(ex is ObjectDisposedException))
			{
				Debug.LogException(ex);
			}
			queueEvent(new AECEvent(DisconnectCallback, "Connection to the server lost. Please check your internet connection and try again."));
		}
	}

	public void Disconnect()
	{
		close();
		queueEvent(new AECEvent(DisconnectCallback, "Disconnect"));
	}

	public void sendRequest(Request r)
	{
		string text = Serialize(r);
		if (IsLogEnabled)
		{
			Debug.Log("AEC >> " + text);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] array = new byte[bytes.Length + 2];
		array[0] = r.type;
		array[1] = r.cmd;
		Buffer.BlockCopy(bytes, 0, array, 2, bytes.Length);
		EncryptDecrypt(array);
		sendMessage(array);
	}

	private static void EncryptDecrypt(byte[] data)
	{
		byte[] array = new byte[3] { 250, 158, 179 };
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i] != array[i % array.Length])
			{
				data[i] ^= array[i % array.Length];
			}
		}
	}

	private T Deserialize<T>(string message)
	{
		return JsonConvert.DeserializeObject<T>(message, deserializersetting);
	}

	private string Serialize(object o)
	{
		return JsonConvert.SerializeObject(o, Formatting.None, serializersetting);
	}

	private void WrapAndQueueResponse(byte[] data)
	{
		EncryptDecrypt(data);
		byte num = data[0];
		byte b = data[1];
		Com.Type type = (Com.Type)num;
		string @string = Encoding.UTF8.GetString(data, 2, data.Length - 2);
		if (IsLogEnabled)
		{
			Debug.Log("AEC << " + type.ToString() + " " + b + " " + @string);
		}
		try
		{
			switch (type)
			{
			case Com.Type.ServerTools:
				switch (b)
				{
				case 11:
				{
					ResponseUpdatePosSpawn r58 = Deserialize<ResponseUpdatePosSpawn>(@string);
					queueResponse(r58);
					break;
				}
				case 12:
				{
					ResponseHijackSpawn r57 = Deserialize<ResponseHijackSpawn>(@string);
					queueResponse(r57);
					break;
				}
				case 13:
				{
					ResponseAddSpawn r56 = Deserialize<ResponseAddSpawn>(@string);
					queueResponse(r56);
					break;
				}
				case 14:
				{
					ResponseDeleteSpawn r55 = Deserialize<ResponseDeleteSpawn>(@string);
					queueResponse(r55);
					break;
				}
				case 15:
				{
					ResponseAddNPC r54 = Deserialize<ResponseAddNPC>(@string);
					queueResponse(r54);
					break;
				}
				case 16:
				{
					ResponseDeleteNPC r53 = Deserialize<ResponseDeleteNPC>(@string);
					queueResponse(r53);
					break;
				}
				case 17:
				{
					ResponseEditNPC r52 = Deserialize<ResponseEditNPC>(@string);
					queueResponse(r52);
					break;
				}
				case 18:
				{
					ResponseUpdatePathNPC r51 = Deserialize<ResponseUpdatePathNPC>(@string);
					queueResponse(r51);
					break;
				}
				case 19:
				{
					ResponseDeletePathNPC r50 = Deserialize<ResponseDeletePathNPC>(@string);
					queueResponse(r50);
					break;
				}
				case 21:
				{
					ResponseOpenInAdmin r49 = Deserialize<ResponseOpenInAdmin>(@string);
					queueResponse(r49);
					break;
				}
				case 24:
				{
					ResponseUpdateRequirements r48 = Deserialize<ResponseUpdateRequirements>(@string);
					queueResponse(r48);
					break;
				}
				case 26:
				{
					ResponseAddMapEntity r47 = Deserialize<ResponseAddMapEntity>(@string);
					queueResponse(r47);
					break;
				}
				case 30:
				{
					ResponseUpdateMapEntity r46 = Deserialize<ResponseUpdateMapEntity>(@string);
					queueResponse(r46);
					break;
				}
				case 28:
				{
					ResponseDeleteMapEntity r45 = Deserialize<ResponseDeleteMapEntity>(@string);
					queueResponse(r45);
					break;
				}
				case 34:
				{
					ResponseMapAssets r44 = Deserialize<ResponseMapAssets>(@string);
					queueResponse(r44);
					break;
				}
				case 36:
				{
					ResponseFavoriteMapAssets r43 = Deserialize<ResponseFavoriteMapAssets>(@string);
					queueResponse(r43);
					break;
				}
				}
				break;
			case Com.Type._DEBUG:
			{
				ResponseDebug r41 = Deserialize<ResponseDebug>(@string);
				queueResponse(r41);
				break;
			}
			case Com.Type.Login:
			{
				ResponseLogin r42 = Deserialize<ResponseLogin>(@string);
				queueResponse(r42);
				break;
			}
			case Com.Type.Area:
				switch (b)
				{
				case 1:
				{
					ResponseAreaJoin r39 = Deserialize<ResponseAreaJoin>(@string);
					queueResponse(r39);
					break;
				}
				case 2:
				{
					ResponseAreaRemove r38 = Deserialize<ResponseAreaRemove>(@string);
					queueResponse(r38);
					break;
				}
				case 3:
				{
					ResponseAreaList r37 = Deserialize<ResponseAreaList>(@string);
					queueResponse(r37);
					break;
				}
				case 4:
				{
					ResponseWarSync r36 = Deserialize<ResponseWarSync>(@string);
					queueResponse(r36);
					break;
				}
				case 5:
				{
					ResponseAreaMatchState r35 = Deserialize<ResponseAreaMatchState>(@string);
					queueResponse(r35);
					break;
				}
				case 7:
				{
					ResponseDynamicScale r34 = Deserialize<ResponseDynamicScale>(@string);
					queueResponse(r34);
					break;
				}
				case 8:
				{
					ResponseSpeedrun r33 = Deserialize<ResponseSpeedrun>(@string);
					queueResponse(r33);
					break;
				}
				case 9:
				{
					ResponseSpeedrun r32 = Deserialize<ResponseSpeedrun>(@string);
					queueResponse(r32);
					break;
				}
				}
				break;
			case Com.Type.Cell:
				switch (b)
				{
				case 1:
				{
					ResponseCellJoin r40 = Deserialize<ResponseCellJoin>(@string);
					queueResponse(r40);
					break;
				}
				case 2:
					queueResponse(Deserialize<ResponseCellAdd>(@string));
					break;
				case 3:
					queueResponse(Deserialize<ResponseCellRemove>(@string));
					break;
				case 4:
					queueResponse(Deserialize<ResponseCellTeleport>(@string));
					break;
				case 5:
					queueResponse(Deserialize<ResponseScoreSync>(@string));
					break;
				case 6:
					queueResponse(Deserialize<ResponseSoundTrackUpdate>(@string));
					break;
				case 7:
					queueResponse(Deserialize<ResponseCellTimerPause>(@string));
					break;
				case 8:
					queueResponse(Deserialize<ResponseCellTimerStart>(@string));
					break;
				case 9:
					queueResponse(Deserialize<ResponseCellTimerStop>(@string));
					break;
				case 10:
					queueResponse(Deserialize<ResponseCellTimerUnpause>(@string));
					break;
				}
				break;
			case Com.Type.NPC:
				switch (b)
				{
				case 7:
				{
					ResponseNPCPlayAnimation r14 = Deserialize<ResponseNPCPlayAnimation>(@string);
					queueResponse(r14);
					break;
				}
				case 8:
				{
					ResponseNPCBaited r13 = Deserialize<ResponseNPCBaited>(@string);
					queueResponse(r13);
					break;
				}
				case 9:
				{
					ResponseNPCMovementBehavior r12 = Deserialize<ResponseNPCMovementBehavior>(@string);
					queueResponse(r12);
					break;
				}
				case 1:
				{
					ResponseNPCDespawn r11 = Deserialize<ResponseNPCDespawn>(@string);
					queueResponse(r11);
					break;
				}
				case 4:
				{
					ResponseNPCDialogueEnd r10 = Deserialize<ResponseNPCDialogueEnd>(@string);
					queueResponse(r10);
					break;
				}
				case 10:
				{
					ResponseNPCNotify r9 = Deserialize<ResponseNPCNotify>(@string);
					queueResponse(r9);
					break;
				}
				case 2:
				{
					ResponseNPCSpawn r8 = Deserialize<ResponseNPCSpawn>(@string);
					queueResponse(r8);
					break;
				}
				case 6:
				{
					ResponseNPCTeamEvent r7 = Deserialize<ResponseNPCTeamEvent>(@string);
					queueResponse(r7);
					break;
				}
				case 5:
				{
					ResponseNPCTurnTo r6 = Deserialize<ResponseNPCTurnTo>(@string);
					queueResponse(r6);
					break;
				}
				}
				break;
			case Com.Type.SpellTemplates:
			{
				ResponseSpellTemplates r5 = Deserialize<ResponseSpellTemplates>(@string);
				queueResponse(r5);
				break;
			}
			case Com.Type.Channel:
				switch (b)
				{
				case 2:
				{
					ResponseChannelAdd r4 = Deserialize<ResponseChannelAdd>(@string);
					queueResponse(r4);
					break;
				}
				case 3:
				{
					ResponseChannelRemove r3 = Deserialize<ResponseChannelRemove>(@string);
					queueResponse(r3);
					break;
				}
				}
				break;
			case Com.Type.Chat:
			{
				ResponseChat r2 = Deserialize<ResponseChat>(@string);
				queueResponse(r2);
				break;
			}
			case Com.Type.Emote:
			{
				ResponseEmote r = Deserialize<ResponseEmote>(@string);
				queueResponse(r);
				break;
			}
			case Com.Type.Move:
				queueResponse(Deserialize<ResponseMovement>(@string));
				break;
			case Com.Type.NpcMove:
				switch (b)
				{
				case 1:
					queueResponse(Deserialize<ResponseMovePath>(@string));
					break;
				case 2:
					queueResponse(Deserialize<ResponsePathSpeed>(@string));
					break;
				case 3:
					queueResponse(Deserialize<ResponsePathStop>(@string));
					break;
				case 4:
					queueResponse(Deserialize<ResponseStopSync>(@string));
					break;
				}
				break;
			case Com.Type.Item:
				switch (b)
				{
				case 1:
				{
					ResponseItemEquip r110 = Deserialize<ResponseItemEquip>(@string);
					queueResponse(r110);
					break;
				}
				case 2:
				{
					ResponseItemUnequip r109 = Deserialize<ResponseItemUnequip>(@string);
					queueResponse(r109);
					break;
				}
				case 4:
				{
					ResponseItemRemove r108 = Deserialize<ResponseItemRemove>(@string);
					queueResponse(r108);
					break;
				}
				case 5:
				{
					ResponseItemAdd r107 = Deserialize<ResponseItemAdd>(@string);
					queueResponse(r107);
					break;
				}
				case 3:
				{
					ResponseItemUpdate r106 = Deserialize<ResponseItemUpdate>(@string);
					queueResponse(r106);
					break;
				}
				case 9:
				{
					ResponseOpenLootBox r105 = Deserialize<ResponseOpenLootBox>(@string);
					queueResponse(r105);
					break;
				}
				case 10:
				{
					ResponseItemDailyReward r104 = Deserialize<ResponseItemDailyReward>(@string);
					queueResponse(r104);
					break;
				}
				case 7:
				{
					ResponseItemLoad r103 = Deserialize<ResponseItemLoad>(@string);
					queueResponse(r103);
					break;
				}
				case 16:
				{
					ResponseInfusion r102 = Deserialize<ResponseInfusion>(@string);
					queueResponse(r102);
					break;
				}
				case 17:
				{
					ResponseExtract r101 = Deserialize<ResponseExtract>(@string);
					queueResponse(r101);
					break;
				}
				case 20:
				{
					ResponseItemModifierReroll r100 = Deserialize<ResponseItemModifierReroll>(@string);
					queueResponse(r100);
					break;
				}
				case 22:
				{
					ResponseItemModifierRerollConfirm r99 = Deserialize<ResponseItemModifierRerollConfirm>(@string);
					queueResponse(r99);
					break;
				}
				case 15:
				{
					ResponseItemLog r98 = Deserialize<ResponseItemLog>(@string);
					queueResponse(r98);
					break;
				}
				case 18:
				{
					ResponseItemInvReload r97 = Deserialize<ResponseItemInvReload>(@string);
					queueResponse(r97);
					break;
				}
				case 6:
				case 8:
				case 11:
				case 12:
				case 13:
				case 14:
				case 19:
				case 21:
					break;
				}
				break;
			case Com.Type.Trade:
				switch (b)
				{
				case 3:
				{
					ResponseShopLoad r96 = Deserialize<ResponseShopLoad>(@string);
					queueResponse(r96);
					break;
				}
				case 1:
				{
					ResponseTradeBuy r95 = Deserialize<ResponseTradeBuy>(@string);
					queueResponse(r95);
					break;
				}
				case 2:
				{
					ResponseTradeSell r94 = Deserialize<ResponseTradeSell>(@string);
					queueResponse(r94);
					break;
				}
				}
				break;
			case Com.Type.CombatClasses:
				switch (b)
				{
				case 1:
				{
					ResponseClassAdd r93 = Deserialize<ResponseClassAdd>(@string);
					queueResponse(r93);
					break;
				}
				case 2:
				{
					ResponseClassEquip r92 = Deserialize<ResponseClassEquip>(@string);
					queueResponse(r92);
					break;
				}
				case 3:
				{
					ResponseCrossSkillEquip r91 = Deserialize<ResponseCrossSkillEquip>(@string);
					queueResponse(r91);
					break;
				}
				case 4:
					queueResponse(Deserialize<ResponseSpellCooldowns>(@string));
					break;
				}
				break;
			case Com.Type.Combat:
				switch (b)
				{
				case 4:
				{
					ResponseCombatSpell r90 = Deserialize<ResponseCombatSpell>(@string);
					queueResponse(r90);
					break;
				}
				case 5:
				{
					ResponseCombatEffectPulse r89 = Deserialize<ResponseCombatEffectPulse>(@string);
					queueResponse(r89);
					break;
				}
				case 8:
				{
					ResponseMachineSpell r88 = Deserialize<ResponseMachineSpell>(@string);
					queueResponse(r88);
					break;
				}
				case 10:
				{
					ResponseResetCD r87 = Deserialize<ResponseResetCD>(@string);
					queueResponse(r87);
					break;
				}
				case 11:
				{
					ResponseAuraUpdate r86 = Deserialize<ResponseAuraUpdate>(@string);
					queueResponse(r86);
					break;
				}
				case 6:
				case 7:
				case 9:
					break;
				}
				break;
			case Com.Type.Quest:
				switch (b)
				{
				case 1:
				{
					ResponseQuestLoad r85 = Deserialize<ResponseQuestLoad>(@string);
					queueResponse(r85);
					break;
				}
				case 5:
				{
					ResponseQuestProgress r84 = Deserialize<ResponseQuestProgress>(@string);
					queueResponse(r84);
					break;
				}
				case 4:
				{
					ResponseQuestComplete r83 = Deserialize<ResponseQuestComplete>(@string);
					queueResponse(r83);
					break;
				}
				case 3:
				{
					ResponseQuestAbandon r82 = Deserialize<ResponseQuestAbandon>(@string);
					queueResponse(r82);
					break;
				}
				case 2:
				{
					ResponseQuestAccept r81 = Deserialize<ResponseQuestAccept>(@string);
					queueResponse(r81);
					break;
				}
				}
				break;
			case Com.Type.Loot:
				switch (b)
				{
				case 1:
				{
					ResponseLootAdd r80 = Deserialize<ResponseLootAdd>(@string);
					queueResponse(r80);
					break;
				}
				case 2:
				{
					ResponseLootRemoveItem r79 = Deserialize<ResponseLootRemoveItem>(@string);
					queueResponse(r79);
					break;
				}
				}
				break;
			case Com.Type.Entity:
				switch (b)
				{
				case 1:
				{
					ResponseEntityAddGoldXP r78 = Deserialize<ResponseEntityAddGoldXP>(@string);
					queueResponse(r78);
					break;
				}
				case 32:
				{
					ResponseGloryXpUpdate r77 = Deserialize<ResponseGloryXpUpdate>(@string);
					queueResponse(r77);
					break;
				}
				case 2:
				{
					ResponseEntityLevelUp r76 = Deserialize<ResponseEntityLevelUp>(@string);
					queueResponse(r76);
					break;
				}
				case 21:
				{
					ResponseEntityRankUp r75 = Deserialize<ResponseEntityRankUp>(@string);
					queueResponse(r75);
					break;
				}
				case 3:
				{
					ResponseEntityUpdate r74 = Deserialize<ResponseEntityUpdate>(@string);
					queueResponse(r74);
					break;
				}
				case 4:
				{
					ResponsePlayerRespawn r73 = Deserialize<ResponsePlayerRespawn>(@string);
					queueResponse(r73);
					break;
				}
				case 6:
				{
					ResponseEntityClass r72 = Deserialize<ResponseEntityClass>(@string);
					queueResponse(r72);
					break;
				}
				case 7:
					queueResponse(Deserialize<ResponseMCUpdate>(@string));
					break;
				case 8:
					queueResponse(Deserialize<ResponseGoldUpdate>(@string));
					break;
				case 9:
					queueResponse(Deserialize<ResponseBitFlagUpdate>(@string));
					break;
				case 10:
					queueResponse(Deserialize<ResponseEntityAssetOverride>(@string));
					break;
				case 11:
					queueResponse(Deserialize<ResponseEntityCustomize>(@string));
					break;
				case 24:
					queueResponse(Deserialize<ResponseEntityGender>(@string));
					break;
				case 12:
					queueResponse(Deserialize<ResponseDataSync>(@string));
					break;
				case 15:
					queueResponse(Deserialize<ResponseEntityAssetUpdate>(@string));
					break;
				case 16:
					queueResponse(Deserialize<ResponseEntityPortraitUpdate>(@string));
					break;
				case 17:
					queueResponse(Deserialize<ResponseEntityTitleUpdate>(@string));
					break;
				case 18:
					queueResponse(Deserialize<ResponseEntityLoadBadges>(@string));
					break;
				case 19:
					queueResponse(Deserialize<ResponsePetEquip>(@string));
					break;
				case 25:
					queueResponse(Deserialize<ResponsePetInteract>(@string));
					break;
				case 20:
					queueResponse(Deserialize<ResponsePetUnequip>(@string));
					break;
				case 22:
					queueResponse(Deserialize<ResponseAFK>(@string));
					break;
				case 23:
					lastDisconnectMessageTimeStamp = DateTime.UtcNow;
					disconnectMessage = Deserialize<ResponseServerDisconnect>(@string).message;
					break;
				case 26:
					queueResponse(Deserialize<ResponseEntityWeaponUpdate>(@string));
					break;
				case 27:
					queueResponse(Deserialize<ResponseEntityToolUpdate>(@string));
					break;
				case 28:
					queueResponse(Deserialize<ResponsePlayerTradeSkillAddXP>(@string));
					break;
				case 29:
					queueResponse(Deserialize<ResponsePlayerTradeSkillLevelUp>(@string));
					break;
				case 30:
					queueResponse(Deserialize<ResponseEntityCapstoneBarFill>(@string));
					break;
				case 31:
					queueResponse(Deserialize<ResponsePvpActionEquip>(@string));
					break;
				}
				break;
			case Com.Type.Machine:
				switch ((Com.CmdMachine)b)
				{
				case Com.CmdMachine.Update:
					queueResponse(Deserialize<ResponseMachineUpdate>(@string));
					break;
				case Com.CmdMachine.Cast:
					queueResponse(Deserialize<ResponseMachineCast>(@string));
					break;
				case Com.CmdMachine.Animation:
					queueResponse(Deserialize<ResponseMachinePlayAnimation>(@string));
					break;
				case Com.CmdMachine.AnimatorLayerWeight:
					queueResponse(Deserialize<ResponseMachineAnimatorLayerWeight>(@string));
					break;
				case Com.CmdMachine.AnimatorParameter:
					queueResponse(Deserialize<ResponseMachineAnimatorParameter>(@string));
					break;
				case Com.CmdMachine.AreaFlag:
					queueResponse(Deserialize<ResponseMachineAreaFlag>(@string));
					break;
				case Com.CmdMachine.CTState:
					queueResponse(Deserialize<ResponseMachineSetCTState>(@string));
					break;
				case Com.CmdMachine.Collision:
					queueResponse(Deserialize<ResponseMachineCollision>(@string));
					break;
				case Com.CmdMachine.HarpoonFire:
					queueResponse(Deserialize<ResponseMachineHarpoonFire>(@string));
					break;
				case Com.CmdMachine.HarpoonSync:
					queueResponse(Deserialize<ResponseMachineHarpoonSync>(@string));
					break;
				case Com.CmdMachine.ListenerUpdate:
					queueResponse(Deserialize<ResponseMachineListenerUpdate>(@string));
					break;
				case Com.CmdMachine.ResourceChannel:
					queueResponse(Deserialize<ResponseMachineResourceChannel>(@string));
					break;
				case Com.CmdMachine.ResourceDespawn:
					queueResponse(Deserialize<ResponseMachineResourceDespawn>(@string));
					break;
				case Com.CmdMachine.ResourceInteract:
					queueResponse(Deserialize<ResponseMachineResourceInteract>(@string));
					break;
				case Com.CmdMachine.ResourceInterrupt:
					queueResponse(Deserialize<ResponseMachineResourceInterrupt>(@string));
					break;
				case Com.CmdMachine.ResourceSpawn:
					queueResponse(Deserialize<ResponseMachineResourceSpawn>(@string));
					break;
				case Com.CmdMachine.ResourceUsageUpdate:
					queueResponse(Deserialize<ResponseMachineResourceUsageUpdate>(@string));
					break;
				case Com.CmdMachine.Click:
				case Com.CmdMachine.Enter:
				case Com.CmdMachine.Exit:
				case Com.CmdMachine.Trigger:
				case Com.CmdMachine.ResourceCollect:
				case Com.CmdMachine.ResourceDrop:
				case Com.CmdMachine.ResourceTrigger:
					break;
				}
				break;
			case Com.Type.PvP:
				switch (b)
				{
				case 1:
				{
					ResponsePvPToggle r71 = Deserialize<ResponsePvPToggle>(@string);
					queueResponse(r71);
					break;
				}
				case 2:
				{
					ResponsePvPDuelChallenge r70 = Deserialize<ResponsePvPDuelChallenge>(@string);
					queueResponse(r70);
					break;
				}
				case 4:
				{
					ResponsePvPDuelCountdown r69 = Deserialize<ResponsePvPDuelCountdown>(@string);
					queueResponse(r69);
					break;
				}
				case 5:
				{
					ResponsePvPDuelStart r68 = Deserialize<ResponsePvPDuelStart>(@string);
					queueResponse(r68);
					break;
				}
				case 6:
				{
					ResponsePvPDuelComplete r67 = Deserialize<ResponsePvPDuelComplete>(@string);
					queueResponse(r67);
					break;
				}
				case 8:
				{
					ResponsePvPScoreUpdate r66 = Deserialize<ResponsePvPScoreUpdate>(@string);
					queueResponse(r66);
					break;
				}
				case 9:
				{
					ResponseUpdateCapturePointStatus r65 = Deserialize<ResponseUpdateCapturePointStatus>(@string);
					queueResponse(r65);
					break;
				}
				case 10:
				{
					ResponsePvPCaptureBarUpdate r64 = Deserialize<ResponsePvPCaptureBarUpdate>(@string);
					queueResponse(r64);
					break;
				}
				case 15:
				{
					ResponsePvPTimerStart r63 = Deserialize<ResponsePvPTimerStart>(@string);
					queueResponse(r63);
					break;
				}
				case 12:
				{
					ResponsePvpMatchStart r62 = Deserialize<ResponsePvpMatchStart>(@string);
					queueResponse(r62);
					break;
				}
				case 13:
				{
					ResponsePvpMatchEnd r61 = Deserialize<ResponsePvpMatchEnd>(@string);
					queueResponse(r61);
					break;
				}
				case 11:
				{
					ResponsePvpMatchSoundTrackCountdown r60 = Deserialize<ResponsePvpMatchSoundTrackCountdown>(@string);
					queueResponse(r60);
					break;
				}
				}
				break;
			case Com.Type.Message:
			{
				ResponseMessageBox r59 = Deserialize<ResponseMessageBox>(@string);
				queueResponse(r59);
				break;
			}
			case Com.Type.Merge:
				if (b == 2)
				{
					ResponseMergeShopLoad r29 = Deserialize<ResponseMergeShopLoad>(@string);
					queueResponse(r29);
				}
				if (b == 6)
				{
					ResponseMergeAdd r30 = Deserialize<ResponseMergeAdd>(@string);
					queueResponse(r30);
				}
				if (b == 7)
				{
					ResponseMergeRemove r31 = Deserialize<ResponseMergeRemove>(@string);
					queueResponse(r31);
				}
				break;
			case Com.Type.Misc:
				switch (b)
				{
				case 1:
					queueResponse(Deserialize<ResponseNotify>(@string));
					break;
				case 4:
					queueResponse(Deserialize<ResponseEventDetails>(@string));
					break;
				}
				break;
			case Com.Type.Friend:
				switch (b)
				{
				case 6:
				{
					ResponseGoto r28 = Deserialize<ResponseGoto>(@string);
					queueResponse(r28);
					break;
				}
				case 4:
				{
					ResponseFriendList r27 = Deserialize<ResponseFriendList>(@string);
					queueResponse(r27);
					break;
				}
				case 1:
				{
					ResponseFriend r26 = Deserialize<ResponseFriend>(@string);
					queueResponse(r26);
					break;
				}
				case 2:
				{
					ResponseFriendAdd r25 = Deserialize<ResponseFriendAdd>(@string);
					queueResponse(r25);
					break;
				}
				case 5:
				{
					ResponseSummon r24 = Deserialize<ResponseSummon>(@string);
					queueResponse(r24);
					break;
				}
				case 7:
				{
					ResponseFriendAdded r23 = Deserialize<ResponseFriendAdded>(@string);
					queueResponse(r23);
					break;
				}
				}
				break;
			case Com.Type.Party:
				switch (b)
				{
				case 1:
				{
					ResponsePartyInvite r22 = Deserialize<ResponsePartyInvite>(@string);
					queueResponse(r22);
					break;
				}
				case 5:
				{
					ResponsePartyList r21 = Deserialize<ResponsePartyList>(@string);
					queueResponse(r21);
					break;
				}
				case 3:
				{
					ResponsePartyRemove r20 = Deserialize<ResponsePartyRemove>(@string);
					queueResponse(r20);
					break;
				}
				case 11:
				{
					ResponsePartyPrivacy r19 = Deserialize<ResponsePartyPrivacy>(@string);
					queueResponse(r19);
					break;
				}
				case 8:
					queueResponse(Deserialize<ResponseVoteKickStart>(@string));
					break;
				case 10:
					queueResponse(Deserialize<ResponseVoteKickEnd>(@string));
					break;
				case 12:
					queueResponse(Deserialize<ResponsePartyDisconnect>(@string));
					break;
				case 13:
					queueResponse(Deserialize<ResponsePartyReconnect>(@string));
					break;
				}
				break;
			case Com.Type.Bank:
				switch (b)
				{
				case 1:
				{
					ResponseBankItems r18 = Deserialize<ResponseBankItems>(@string);
					queueResponse(r18);
					break;
				}
				case 2:
				{
					ResponseItemTransfer r17 = Deserialize<ResponseItemTransfer>(@string);
					queueResponse(r17);
					break;
				}
				case 3:
				{
					ResponseBankPurchase r16 = Deserialize<ResponseBankPurchase>(@string);
					queueResponse(r16);
					break;
				}
				case 4:
				{
					ResponseAllBankItems r15 = Deserialize<ResponseAllBankItems>(@string);
					queueResponse(r15);
					break;
				}
				}
				break;
			case Com.Type.Disconnect:
				Disconnect();
				break;
			case Com.Type.Command:
				switch (b)
				{
				case 2:
					queueResponse(Deserialize<ResponseCommandDialog>(@string));
					break;
				case 4:
					queueResponse(Deserialize<ResponseCommandHelp>(@string));
					break;
				case 5:
					queueResponse(Deserialize<ResponseGetChatCommands>(@string));
					break;
				}
				break;
			case Com.Type.Admin:
				switch ((Com.CmdAdmin)b)
				{
				case Com.CmdAdmin.QSSet:
					queueResponse(Deserialize<ResponseAdminQSSet>(@string));
					break;
				case Com.CmdAdmin.Dialog:
					queueResponse(Deserialize<Response>(@string));
					break;
				case Com.CmdAdmin.ReloadData:
					queueResponse(Deserialize<Response>(@string));
					break;
				case Com.CmdAdmin.ReloadShop:
					queueResponse(Deserialize<Response>(@string));
					break;
				case Com.CmdAdmin.ReloadQuest:
					queueResponse(Deserialize<Response>(@string));
					break;
				case Com.CmdAdmin.ReloadMongo:
					queueResponse(Deserialize<ResponseMongoReload>(@string));
					break;
				case Com.CmdAdmin.QSInfo:
					queueResponse(Deserialize<Response>(@string));
					break;
				default:
					queueResponse(Deserialize<Response>(@string));
					break;
				}
				break;
			case Com.Type.AdWatch:
				queueResponse(Deserialize<ResponseAdWatch>(@string));
				break;
			case Com.Type.Player:
				switch ((Com.CmdPlayer)b)
				{
				case Com.CmdPlayer.ProductOfferSet:
					queueResponse(Deserialize<ResponseProductOfferSet>(@string));
					break;
				case Com.CmdPlayer.TimedChoice:
					queueResponse(Deserialize<ResponseTimedChoice>(@string));
					break;
				case Com.CmdPlayer.TimedChoiceCancel:
					queueResponse(Deserialize<ResponseTimedChoiceCancel>(@string));
					break;
				case Com.CmdPlayer.QueueStatus:
					queueResponse(Deserialize<ResponseQueueStatus>(@string));
					break;
				case Com.CmdPlayer.PvPStats:
					queueResponse(Deserialize<ResponsePlayerPvPStats>(@string));
					break;
				case Com.CmdPlayer.PvPRecords:
					queueResponse(Deserialize<ResponseUpdatePvPRecords>(@string));
					break;
				case Com.CmdPlayer.JoinQueue:
				case Com.CmdPlayer.LeaveQueue:
					break;
				}
				break;
			case Com.Type.SystemPerformance:
				if (b == 1)
				{
					queueResponse(Deserialize<ResponsePing>(@string));
				}
				break;
			case Com.Type.Guild:
				switch (b)
				{
				case 2:
					queueResponse(Deserialize<ResponseGuildInfo>(@string));
					break;
				case 15:
					queueResponse(Deserialize<ResponseGuildGoldXpInfo>(@string));
					break;
				case 16:
					queueResponse(Deserialize<ResponseGuildLeaderboardEntries>(@string));
					break;
				case 4:
					queueResponse(Deserialize<ResponseGuildInvite>(@string));
					break;
				case 5:
					queueResponse(Deserialize<ResponseEntityGuildUpdate>(@string));
					break;
				case 6:
					queueResponse(Deserialize<ResponseGuildChangeRole>(@string));
					break;
				case 3:
					queueResponse(Deserialize<ResponseLeaveGuild>(@string));
					break;
				case 7:
					queueResponse(Deserialize<ResponseGuildMemberStatus>(@string));
					break;
				case 8:
					queueResponse(Deserialize<ResponseUpdateGuildName>(@string));
					break;
				case 14:
					queueResponse(Deserialize<ResponseUpdateGuildTax>(@string));
					break;
				case 9:
					queueResponse(Deserialize<ResponseUpdateGuildTag>(@string));
					break;
				case 10:
					queueResponse(Deserialize<ResponseMOTDUpdate>(@string));
					break;
				}
				break;
			case Com.Type.DailyTask:
				if (b == 1)
				{
					queueResponse(Deserialize<ResponseDailyTaskInfo>(@string));
				}
				break;
			case Com.Type.ServerDailyTask:
				if (b == 1)
				{
					queueResponse(Deserialize<ResponseServerDailyTaskInitialize>(@string));
				}
				break;
			case Com.Type.Leaderboard:
				if (b == 1)
				{
					queueResponse(Deserialize<ResponseLeaderboard>(@string));
				}
				if (b == 2)
				{
					queueResponse(Deserialize<ResponsePlayerLeaderboardScore>(@string));
				}
				break;
			case Com.Type.DailyQuestReset:
				queueResponse(Deserialize<ResponseServerDailyQuest>(@string));
				break;
			case Com.Type.Resource:
				HandleResourceMessage(@string, (Com.CmdResource)b);
				break;
			case Com.Type.Audio:
				queueResponse(Deserialize<ResponsePlayAudioClip>(@string));
				break;
			case Com.Type.Sheathing:
				queueResponse(Deserialize<ResponseSheathing>(@string));
				break;
			case Com.Type.Housing:
				switch ((Com.CmdHousing)b)
				{
				case Com.CmdHousing.HouseJoin:
					queueResponse(Deserialize<ResponseHouseJoin>(@string));
					break;
				case Com.CmdHousing.HouseData:
					queueResponse(Deserialize<ResponseHouseData>(@string));
					break;
				case Com.CmdHousing.HouseAdd:
					queueResponse(Deserialize<ResponseHouseAdd>(@string));
					break;
				case Com.CmdHousing.HouseExit:
					queueResponse(Deserialize<ResponseHouseCommand>(@string));
					break;
				case Com.CmdHousing.HouseItemAdd:
					queueResponse(Deserialize<ResponseHouseItemAdd>(@string));
					break;
				case Com.CmdHousing.HouseItemMove:
					queueResponse(Deserialize<ResponseHouseItemMove>(@string));
					break;
				case Com.CmdHousing.HouseItemList:
					queueResponse(Deserialize<ResponseHouseItemList>(@string));
					break;
				case Com.CmdHousing.HouseItemRemove:
					queueResponse(Deserialize<ResponseHouseItemRemove>(@string));
					break;
				case Com.CmdHousing.HouseItemClearAll:
					queueResponse(Deserialize<ResponseHouseItemClearAll>(@string));
					break;
				case Com.CmdHousing.HouseSave:
				case Com.CmdHousing.HouseUpdate:
				case Com.CmdHousing.HouseSaveExit:
				case Com.CmdHousing.HouseForceExit:
				case Com.CmdHousing.HouseQuest:
					break;
				}
				break;
			case Com.Type.Mail:
				if (b == 1)
				{
					queueResponse(Deserialize<ResponseMail>(@string));
				}
				break;
			case Com.Type.Joinmychannels:
			case Com.Type.NPCTemplates:
			case Com.Type.Report:
			case Com.Type.SyncIgnore:
			case Com.Type.EndTransfer:
			case Com.Type.DirectCommand:
			case Com.Type.SuperClient:
			case Com.Type.ServerEvent:
				break;
			}
		}
		catch (Exception ex)
		{
			ErrorReporting.Instance.ReportError("Response Error", "The client has failed to process a message received from the server.", ex.Message, null, "Message: " + @string, ex);
		}
	}

	private void HandleResourceMessage(string message, Com.CmdResource cmd)
	{
		switch (cmd)
		{
		case Com.CmdResource.BobberDespawn:
			queueResponse(Deserialize<ResponseFishingBobberDespawn>(message));
			break;
		case Com.CmdResource.BobberSpawn:
			queueResponse(Deserialize<ResponseFishingBobberSpawn>(message));
			break;
		case Com.CmdResource.FishCatch:
			queueResponse(Deserialize<ResponseFishingFishCatch>(message));
			break;
		case Com.CmdResource.FishHook:
			queueResponse(Deserialize<ResponseFishingFishHook>(message));
			break;
		case Com.CmdResource.FishRelease:
			queueResponse(Deserialize<ResponseFishingFishRelease>(message));
			break;
		}
	}
}
