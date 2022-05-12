using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Requesting : MonoBehaviour
{
    public float scaleFactor;
    public GameObject[] objectArr;
    public GameObject[] rotationReferences;
    public GameObject[] prefabCarArray;
    public GameObject[] trafficLightsArray;
    public  List<GameObject> spawnedCarsList;
    public int amountOfCars;
    public  List<Vector3> targetPositions = new List<Vector3>();
    public string test;
    private bool carsSpawned=false;

    //rotate

    void Start() {

        StartCoroutine(loopRequests());
        //StartCoroutine(GetPositions());
        
        // string json = "{\"cars\":[{\"id\":0,\"x\":0,\"y\":0},{\"id\":1,\"x\":0,\"y\":0},{\"id\":2,\"x\":0,\"y\":0},{\"id\":3,\"x\":0,\"y\":0},{\"id\":4,\"x\":0,\"y\":0},{\"id\":5,\"x\":0,\"y\":0},{\"id\":6,\"x\":0,\"y\":0},{\"id\":7,\"x\":0,\"y\":0},{\"id\":8,\"x\":0,\"y\":0},{\"id\":9,\"x\":0,\"y\":0},{\"id\":10,\"x\":0,\"y\":0},{\"id\":11,\"x\":0,\"y\":0},{\"id\":12,\"x\":0,\"y\":0},{\"id\":13,\"x\":0,\"y\":0},{\"id\":14,\"x\":0,\"y\":0}],\"lights\":[{\"id\":0,\"state\":\"YELLOW_COLOR\"},{\"id\":1,\"state\":\"YELLOW_COLOR\"},{\"id\":2,\"state\":\"YELLOW_COLOR\"},{\"id\":3,\"state\":\"YELLOW_COLOR\"}]}";


        // Data infoSimul = JsonUtility.FromJson<Data>(json);

        // Debug.Log(infoSimul.cars.Length);
        // amountOfCars=infoSimul.cars.Length;
        // for(int car = 0; car<amountOfCars;car++){
        //       Debug.Log("Spawn");
        //       Instantiate(prefabCarList[0], new Vector3(0, 0, 0), Quaternion.identity);
        // }
        
    }
    
    IEnumerator loopRequests(){
        while(true){
            StartCoroutine(GetPositions());
            Debug.Log("---------Nuevo Request realizado-------------");
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator GetPositions() {
        float inicio = Time.time;
        
        //print("Haciendo request");
        Data infoSimul;
        //Hacemos request a server
        using(UnityWebRequest requestPositions = UnityWebRequest.Get("http://localhost:8080/")){
            yield return requestPositions.SendWebRequest();
            if (requestPositions.result != UnityWebRequest.Result.Success) {
                Debug.Log(requestPositions.error);
            }else{
                //Show results as text
                Debug.Log(requestPositions.downloadHandler.text);
                Debug.Log("Request succesful!");
            }
            infoSimul=JsonUtility.FromJson<Data>(requestPositions.downloadHandler.text);   //Mapeo de JSON
        }


        //int numObj=0;
        int numSpawnedCar = 0;
        //Debug.Log(positionArray.GetType());
        amountOfCars = infoSimul.cars.Length;
        
        Debug.Log("Total de carros:"+amountOfCars);

        //int roationPick=0;
        //Spawn de carros dinamicos
        if(carsSpawned==false){
            foreach(Car c in infoSimul.cars){
                //Debug.Log("Spawn");
                //Quaternion carRotation = Quaternion.Euler(0,-90,0); 
                Vector3 spawnCarro = new Vector3(c.x,c.z,c.y);
                // if(c.direction==0){
                //     newCar.transform.Rotate (0,-90,0); //ROTAR HACIA ARRIBA 
                // }else if(c.direction==1){
                //     newCar.transform.Rotate (0,90,0); //ROTAR HACIA ABAJO 
                // }else if(c.direction==2){
                //     newCar.transform.Rotate (0,0,0); //ROTAR HACIA DERECHA 
                // }else if(c.direction==3){
                //     newCar.transform.Rotate (0,-180,0); //ROTAR HACIA IZQUIERDA 
                // }
                // if(c.direction==0){
                //     roationPick=1; //ROTAR HACIA ARRIBA 
                // }else if(c.direction==1){
                //     roationPick=0;//ROTAR HACIA ABAJO 
                // }else if(c.direction==2){
                //     roationPick=2; //ROTAR HACIA DERECHA 
                // }else if(c.direction==3){
                //     roationPick=3; //ROTAR HACIA IZQUIERDA 
                // }

                if(numSpawnedCar == amountOfCars){
                    numSpawnedCar=0;
                }
                GameObject newCar = Instantiate(prefabCarArray[numSpawnedCar], spawnCarro, rotationReferences[c.direction].transform.rotation);
                newCar.name = "Car "+c.id;
                newCar.transform.localScale = new Vector3(0.5f, 0.5f,0.5f);
                
                //Debug.Log(c.direction);
                //Debug.Log("Carro "+c.id+" spawn en:"+spawnCarro+" con direccion "+ c.direction);
                spawnedCarsList.Add(newCar);
                numSpawnedCar++;
            }
            carsSpawned = true;
        }

        //Mover carros dinamicos
        Debug.Log("----------Movimiento----------");
        foreach(Car c in infoSimul.cars){
            Vector3 movePosition = new Vector3(c.x,c.z,c.y);
            spawnedCarsList[c.id].transform.position=movePosition;
            //Debug.Log("Carro "+c.id+" se movio a:"+movePosition + "con direccion "+ c.direction);
        }

        //Setup de luces

        foreach(TrafficLights luz in infoSimul.traffic_lights){
            Light componenteLuz = trafficLightsArray[luz.id].GetComponent<Light>();
            
            Color estadoSemaforo=Color.red;
            if(luz.state==0){
                estadoSemaforo=Color.red;
            }else if(luz.state==1){
                estadoSemaforo=Color.yellow;
            }else if(luz.state==2){
                estadoSemaforo=Color.green;
            }
            componenteLuz.color = estadoSemaforo;
            Debug.Log("Semaforo:"+ luz.id+" estado:"+luz.state);
        }
        // Mover carros existentes
        // foreach(Car c in infoSimul.cars){
        //     //float step = objectSpeed*Time.deltaTime;
        //     //Vector3 currentPos = objectArr[numObj].transform.position;
        //     Vector3 posObj = new Vector3(c.x,c.z,c.y);//invertimos z,y para que no se muevan verticalmente
        //     objectArr[numObj].transform.position=posObj; //Se teletransporta
        //     //targetPositions.Add(posObj);
        //     Debug.Log("Car:"+numObj+"moved to"+posObj);
        //     numObj++;
        // }
        
        
        
        // float total = Time.time - inicio;
        // print("tomo: " + total);
    }
    void Update() {
        // int numObj=1;
        // foreach(Vector3 a in targetPositions){
        //     float step = objectSpeed*Time.deltaTime;
        //     Vector3 currentPos = objectArr[numObj].transform.position;
        //     Vector3 posObj = new Vector3(a.x,a.y,a.z);
        //     objectArr[numObj].transform.position = Vector3.MoveTowards(currentPos,posObj,step);
        //     numObj++;
        // }
    }

}
