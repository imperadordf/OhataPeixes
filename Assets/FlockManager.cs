using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlockManager : MonoBehaviour
{
    //O prefab do Peixe
    public GameObject fishPrefab;
    //Numeros de Peixes
    public int numFish = 20;
    //o Array de Peixes
    public GameObject[] allFish;
    //limite para os Peixes poderem seguir
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    //A posição que os peixes seguiram
    public Vector3 goalPos;
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed; // a Velocidade Minima dos peixes
    [Range(0.0f, 5.0f)]
    public float maxSpeed; // A velocidade Maxima dos peixes
    [Range(1.0f, 10.0f)]
    public float neighbourDistance; // A distancia entre os vizinhos
    [Range(0.0f, 5.0f)]
    public float rotationSpeed; // a velocidade de rotação
    //RANGE, determina o valor minimo e maximo.


    void Start()
    {
        //todos os pexeis  sera um novo gameobject que seria o Peixe
        allFish = new GameObject[numFish];
        //a Criação de todos os peixes, criado até o maximo proposto 
        for (int i = 0; i < numFish; i++)
        {
            //Spawnar na posição limite + a  posição do Manager
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
            //Instanciando o Peixe na posição proposta a cima.
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //Declarando o Manager para o Agente, para poder ter a referencia de outros peixes e do manager.
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        // a posição inicial sera essa posição do Manager.
        goalPos = this.transform.position;
    }


    void Update()
    {
        //goalPos = this.transform.position;
        //a posição do GoalPos, sera a posição do Manager
        goalPos = this.transform.position;
        //Caso o Numero aleatorio seja menor que 10, faça :
        //A posição para todos os Cardume, sera a nova posição aleatoria defininando pelos limites.
        if (Random.Range(0, 100) < 10)
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
    }

}