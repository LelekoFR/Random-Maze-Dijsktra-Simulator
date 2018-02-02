using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cada tile do nosso mapa sera representado por um nodo.
public class Nodo{
    
    //Arestas vai ser a ligação de cada tile.
    public List<Nodo> arestas;
    public int x;
    public int y;


    public Nodo(){
        arestas = new List<Nodo>();
    }

}