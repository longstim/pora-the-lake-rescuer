using UnityEngine;
using System.Collections;

public class CommunicationData {

	private SceneName _sender;

	public SceneName Sender {
		get {
			return _sender;
		}
		set {
			_sender = value;
		}
	}

	private SceneName _reciever;

	public SceneName Reciever {
		get {
			return _reciever;
		}
	}

	private object _data;

	public object Data {
		get {
			return _data;
		}
	}

	public CommunicationData(SceneName sender, SceneName reciever, object data)
	{
		this._sender = sender;
		this._reciever = reciever;
		this._data = data;
	}
}
