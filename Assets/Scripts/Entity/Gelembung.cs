using UnityEngine;
using System.Collections;

public class Gelembung{
	
    public string PackageName;
	public BubbleType Type;
    public bool Define;

	public Gelembung(){
	
	}

	public Gelembung(string PackageName, BubbleType Type){
		this.PackageName = PackageName;
		this.Type = Type;
        this.Define = true;
	}

    public Gelembung(string PackageName, BubbleType Type,bool Define)
    {
        this.PackageName = PackageName;
        this.Type = Type;
        this.Define = Define;
    }
}