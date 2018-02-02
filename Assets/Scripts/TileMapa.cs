using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class TileMapa : MonoBehaviour{

    public GameObject player;


    public TipoTile[] tipoTile;

    
    int[,] tiles;

    
    Nodo[,] grafo;

    
    public int mapSizeX;
    public int mapSizeY;


    void Start(){

        //Associando o mapa do player, a toda função TileMapa.
        player.GetComponent<Player>().mapa = this;
        gerarMapa();
        gerarGrafo();
        instanciarMapa();

    }


    //Reeniciar nivel.
    public void restartLevel(){
        
        SceneManager.LoadScene("Scene1");   
}


    void gerarMapa(){

        tiles = new int[mapSizeX, mapSizeY];

        //Função que gera um numero aleatorio.
        System.Random aleatorio = new System.Random();

        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){

                //num tem valor minimo como 0, e maximo como 4.
                int num = aleatorio.Next(0, 4);
                //Nosso vetor "escolha" fará com que os numeros fiquem entre 0 e 3.
                int[] escolha = new int[4] { 0, 1, 2, 3};
                //Nossa matriz tiles em cada posição x,y irá receber um numero aleatorio.
                tiles[x, y] = escolha[num];

            }
        }

        //Posiçao inicial do player sempre sera tile mato.
        tiles[0, 0] = 1;
    }





    float custoTile(int x, int y){
        TipoTile tt = tipoTile[tiles[x, y]];
        return tt.custoMov;
    }




    //criando uma matriz de grafos.
    void gerarGrafo(){

        grafo = new Nodo[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){

                grafo[x, y] = new Nodo();
                grafo[x, y].x = x;
                grafo[x, y].y = y;

            }

        }

        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){

                //Adcionando a esquerda.
                if(x > 0)
                    grafo[x, y].arestas.Add(grafo[x - 1, y]);

                //Adcionando a direita.
                if (x < mapSizeX - 1)
                    grafo[x, y].arestas.Add(grafo[x + 1, y]);

                //Adcionando para baixo.
                if (y > 0)
                    grafo[x, y].arestas.Add(grafo[x, y - 1]);

                //Adcionando para cima.
                if (y < mapSizeY - 1)
                    grafo[x, y].arestas.Add(grafo[x, y + 1]);

            }
        }
    }





    void instanciarMapa(){
        for (int x = 0; x < mapSizeX; x++){
            for (int y = 0; y < mapSizeY; y++){

                TipoTile tt = tipoTile[tiles[x, y]];

                GameObject map = (GameObject)Instantiate(tt.tilePrefab, new Vector3(x, y, 0), Quaternion.identity);

                TileClicavel tc = map.GetComponent<TileClicavel>();

                tc.tileX = x;
                tc.tileY = y;
                tc.mapa = this;

            }
        }
    }





    //Retorna a posição no mundo.
    public Vector3 coodernada(int x, int y){
        return new Vector3(x, y, 0);
    }





    public void Dijkstra(int x, int y){

        //Verificando se o tile é andavel
        TipoTile tt = tipoTile[tiles[x, y]];
        if (tt.andavel == false)
            return;


        //limpando os caminhos antigos
        player.GetComponent<Player>().caminhoAtual = null;


        //Estruturas usadas no Dijkstra.
        Dictionary<Nodo, float> dist = new Dictionary<Nodo, float>();
        Dictionary<Nodo, Nodo> anterior = new Dictionary<Nodo, Nodo>();


        List<Nodo> Nvisitados = new List<Nodo>();


        Nodo inicio = grafo[player.GetComponent<Player>().tileX, player.GetComponent<Player>().tileY];


        //Tile alvo;
        Nodo alvo = grafo[x, y];


        //Inicializando os "vetores" distancia e anterior.
        dist[inicio] = 0;
        anterior[inicio] = null;

        //inicializando todos os vertices a uma distancia inifinita.
        foreach (Nodo vert in grafo){
            if (vert != inicio){
                dist[vert] = Mathf.Infinity;
                anterior[vert] = null;
            }

            Nvisitados.Add(vert);
        }



        while (Nvisitados.Count > 0){

            //será o nodo não visitado com a menor distancia
            Nodo nodoMenorDist = null;
            
         
            foreach (Nodo possivelNodo in Nvisitados){
                if (nodoMenorDist == null || dist[possivelNodo] < dist[nodoMenorDist])
                    nodoMenorDist = possivelNodo;
            }


            if (nodoMenorDist == alvo){
                break;
            }


            Nvisitados.Remove(nodoMenorDist);

            //Relaxamento.
            //Para cada vertice nas adjascencias do nodo de menor distancia
            foreach (Nodo vert in nodoMenorDist.arestas){
               //"altura" irá receber o custo para chegar até o outro nodo.
                float altura = dist[nodoMenorDist] + custoTile(vert.x, vert.y);
                if (dist[vert]>altura){
                    dist[vert] = altura;
                    anterior[vert] = nodoMenorDist;
                }
            }
        }

        //Agora, o "vetor" anterior, descreve a rota de menor caminho do alvo até o inicio.


        if (anterior[alvo] == null){
            return; 
            //sem rota para o alvo.
        }

        //Será o caminho do player até o alvo.
        List<Nodo> caminhoAtualDijkstra = new List<Nodo>();

        Nodo atual = alvo;


        // percorrendo o "anterior" e adcionando para o caminho.
        while (atual != null){
            caminhoAtualDijkstra.Add(atual);
            atual = anterior[atual];
        }


        //Agora o caminho atual descreve uma rota do nosso alvo para nosso inicio.
        //Invertendo.
        caminhoAtualDijkstra.Reverse();

        //A cada chamada, o Dijkstra gerará um caminho para nosso player.
        //Atualizando o caminho atual do player.
        player.GetComponent<Player>().caminhoAtual = caminhoAtualDijkstra;

    }

}
