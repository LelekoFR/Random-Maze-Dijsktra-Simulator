using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClicavel : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMapa mapa;



	void OnMouseUp() {

        Debug.Log(" "+tileX+","+tileY);
        //Quando clicar em um tile, a função de Dijkstra será chamada.
        //E criara um novo caminho até o tile alvo.
        mapa.Dijkstra(tileX, tileY);

	}
}
