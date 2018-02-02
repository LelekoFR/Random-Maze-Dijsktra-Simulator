using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Faz com que os atributos da classe apareçam no inspector.
//Possibilita a criação de objetos da mesma classe.
[System.Serializable]

public class TipoTile {

    
	public string nome;
	public GameObject tilePrefab;
    public float custoMov;
    public bool andavel = true;

}
