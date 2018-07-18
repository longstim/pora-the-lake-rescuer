using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneCommunication {

	private SceneName _name;

	private static List<CommunicationData> _messages = new List<CommunicationData>();

	public SceneCommunication(SceneName name)
	{
		this._name = name;
	}

	public void SendMessage(SceneName sceneName, object data)
	{
		_messages.Add(new CommunicationData(this._name,sceneName,data));
	}

	public List<CommunicationData> RetrieveMessages()
	{
		List<CommunicationData> data = new List<CommunicationData>();
		for (int i=0; i<_messages.Count; i++) {
			CommunicationData message = (CommunicationData)_messages[i];
			if(message.Reciever == this._name)
			{
				data.Add(message);
				//_messages.RemoveAt(i);
			}
		}
		_messages.Clear ();
		return data;
	}
}
