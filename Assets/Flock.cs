using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Flock : MonoBehaviour
{
    //Referencia do Manager que controla esse script
    public FlockManager myManager;

    //a Velocidade do meu Peixe
    public float speed;

    //uma boleana para saber se esta virando ou não
    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //Pego o speed do Manager o maximo e o minimo e dou para minha variavel
        speed = Random.Range(myManager.minSpeed,
        myManager.maxSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        //Crio uma classe Bounds, declarando a posição do Manager e pegando o Limite 
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        //RayCast para detectar colisão
        RaycastHit hit = new RaycastHit();
        //Direção seria o transform do manager do Flock menos a minha posição
        Vector3 direction = myManager.transform.position - transform.position;
        
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        // Se não, caso tenha uma colisão a minha frente eu faço:
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            //Estou virando
            turning = true;
            //A direção sela o Reflect da minha posição e a colisão, assim mudando a trajetória 
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }else
        {
            //Não estou virando
            turning = false;
        }
        //Caso esteja virando
        if (turning)
        {
            //Minha rotação seja a direção proposta pela condições anteriores.
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //Caso o numero aleatorio seja menor que 10 de 0 a 100, eu mudo o meu speed
            //É uma condição de 1/10 ou 10%
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed,
                myManager.maxSpeed);
                //Caso o numero aleatorio seja menor que 20, eu chamo o metodo "ApplyRules"
                //é uma condição 1/5 ou 20%
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }

        //Avance na direção Z que seja na velocidade speed * time.deltatime
        transform.Translate(0, 0, Time.deltaTime * speed);
    }


    void ApplyRules()
    {
        //Array de gameobjects
        GameObject[] gos;
        //pego a referencia dos outros peixes e coloco no meu array.
        gos = myManager.allFish;
        //Calcula o centro do cardume mais proximo
        Vector3 vcentre = Vector3.zero;
        //Evita a colisão de todos os agentes
        Vector3 vavoid = Vector3.zero;
        //a velocidade do grupo
        float gSpeed = 0.01f;
        //distancia entre os outros peixes
        float nDistance;
        //o contador do grupo
        int groupSize = 0;
        //Faço uma busca de Gameobject do meu array gos, que seria as referencias do peixes
        foreach (GameObject go in gos)
        {
            //Caso não seja o mesmo gameobject, faça:
            if (go != this.gameObject)
            {
                //Pegue a distancia entre a posição do agente e da minha posição
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //Caso a distancia seja menor que a distancia do vizinho proposta pelo manager, eu faço :
                if (nDistance <= myManager.neighbourDistance)
                {
                    // o centro  sera centro + a posição daquele agente que esta bem proximo a mim ,proposto pelo if
                    vcentre += go.transform.position;
                    //contador do grupo  soma mais um
                    groupSize++;
                    //Caso a distancia seja menor que um 
                    if (nDistance < 1.0f)
                    {
                        //Somo a colisão com o a minha posição menos a posição do agente.
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //Pego o componente controlador do agente e declaro aqui.
                    Flock anotherFlock = go.GetComponent<Flock>();
                    //Pego o meu speed e somo mais o speed do outro agente.
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //Caso o contador do groupSize seja maior que Zero:
        if (groupSize > 0)
        {
            //Centro do Cardume sera dividido pelo contador e depois somado pelo a posição que vai pelo myManager menos a minha posição
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            //minha velocidade sera a velocidade de outros agentes dividido pelo contador
            speed = gSpeed / groupSize;
            //a direção sera o centro do cardume + a colisão da posição do cardume/agente proximo menos a minha posição
            Vector3 direction = (vcentre + vavoid) - transform.position;
            //Caso a direção seja diferente de 0, continuo na direção proposta 
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
    }
}
      


