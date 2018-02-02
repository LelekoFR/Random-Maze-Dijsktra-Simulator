using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int tileX;
	public int tileY;
    public TileMapa mapa;
    
    //No começo, nosso caminho será null, pois ainda não existe um caminho a ser seguido.
    //Caminho que será gerado pelo Djikstra.
    public List<Nodo> caminhoAtual = null;



    //A cada click em um tile, a função Djikstra sera chamada, como tambem a função update.
    void Update(){

        desenharCaminho();
    }



    public void desenharCaminho(){

        if (caminhoAtual != null){

            int i = 0;

            
            while (i < caminhoAtual.Count - 1){
                
                Vector3 inicio = mapa.coodernada(caminhoAtual[i].x, caminhoAtual[i].y) + new Vector3(0, 0, -0.75f);
                Vector3 fim = mapa.coodernada(caminhoAtual[i + 1].x, caminhoAtual[i + 1].y) + new Vector3(0, 0, -0.75f);

                DrawLine(inicio, fim, Color.blue, 1f);

                i++;
            }
        }
    }



    public void Mover(){

        if (caminhoAtual == null)
            return;
        

        //Removendo o nodo atual da lista.
        caminhoAtual.RemoveAt(0);

        //Atualizando a posição no mundo
        tileX = caminhoAtual[0].x;
        tileY = caminhoAtual[0].y;

        //Mudando a posição para o proximo zero.
        transform.position = mapa.coodernada(caminhoAtual[0].x, caminhoAtual[0].y);

     
        if (caminhoAtual.Count == 1){
            //Sobrou so um tile em nosso caminho
            //Limpando o caminho
            caminhoAtual = null;

        }
    }



   
    public void DrawLine(Vector3 inicio, Vector3 fim, Color cor, float duracao){

        GameObject linha = new GameObject();
        linha.transform.position = inicio;
        linha.AddComponent<LineRenderer>();
        LineRenderer renderizar = linha.GetComponent<LineRenderer>();
        renderizar.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        renderizar.SetColors(cor, cor);
        renderizar.SetWidth(0.160f, 0.160f);
        renderizar.SetPosition(0, inicio);
        renderizar.SetPosition(1, fim);
        GameObject.Destroy(linha, duracao);

    }

}
