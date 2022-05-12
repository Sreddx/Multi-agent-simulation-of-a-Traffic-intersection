using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carrito : MonoBehaviour
{
    
    NavMeshAgent agent;
    public Transform objetivo;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.velocity


        // PARA DESPLAZAMIENTO SUAVE - 
        // - calcular distancia entre puntos 
        // Vector3.Distance();
        // - dividir distancia entre frecuencia de request
        // - actualizar velocidad de agente 
        // agent.velocity = ;

        
        agent.destination = Vector3.zero;
    }

    void Update()
    {
        agent.destination = objetivo.position;
    }
}
