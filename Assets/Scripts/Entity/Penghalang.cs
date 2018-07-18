using UnityEngine;
using System.Collections;

public class Penghalang{

	public string PackageName;
	public Vector3 Position;
	public string Name;
	public float Rotation;
	public float RotationSpeed;

	public Penghalang(){
	
	}

    public Penghalang(string PackageName, string Name, Vector3 Position, float Rotation, float RotationSpeed)
    {
		this.PackageName = PackageName;
		this.Name = Name;
		this.Position = new Vector3(Position.x,Position.y,0);
		this.Rotation = Rotation;
		this.RotationSpeed = RotationSpeed;
	}
}
