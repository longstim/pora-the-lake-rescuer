using UnityEngine;
using System.Collections;

public class Sampah{

	public string PackageName;
	public Vector3 Position;
    public RubbishController.RubbishSize Size;
	public float Rotation;
	public float RotationSpeed;

	public Sampah(){

	}

	public Sampah(string PackageName, RubbishController.RubbishSize Size, Vector3 Position, float Rotation, float RotationSpeed){
		this.PackageName = PackageName;
		this.Size = Size;
		this.Position = Position;
		this.Rotation = Rotation;
		this.RotationSpeed = RotationSpeed;
	}
}
