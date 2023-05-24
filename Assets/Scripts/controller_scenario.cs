using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller_scenario : MonoBehaviour
{
    // Váriaveis da plataforma
    public List<GameObject> platforms = new List<GameObject>();
    public GameObject finish_line;
    public List<Transform> atual_platforms = new List<Transform>();
    public float tam_rua = 32;

    // Váriaveis de ambiente
    public List<GameObject> scenario = new List<GameObject>();
    public List<Transform> atual_scenario = new List<Transform>();
    public int offset;
    public int offset_scenario;
    public float tam_sce = 150;
    private float ambient_x;
    private Transform player;
    private Transform point_plataforma_atual;
    private Transform point_finish_line;
    private Transform point_scenario_atual;
    private int index;
    private int index_scenario;
    private float debug = 0.0001f;
    public player_controller var;

    // Váriavel de Controle do ambiente
    public ui_controller ui;
    private string ambient = "florest";
    private bool troca_ambient = false;

    // Criando os obstáculos
    public GameObject[] obstacles; // Array contendo os obstáculos pré-selecionados
    public int[] spawnPoints; // Pontos de spawn para os obstáculos
    public float difficultyIncreaseRate = 10f; // Taxa de aumento da dificuldade
    public float initialSpawnDelay = 1f; // Tempo inicial entre o instanciamento de obstáculos
    public float spawnDelayReduction = 0.2f; // Redução do tempo entre o instanciamento de obstáculos ao aumentar a dificuldade
    private float currentSpawnDelay; // Tempo entre o instanciamento de obstáculos
    private int obstacles_z = 125;
    private int obstacles_number;
    private int obstacles_x;

    // Start is called before the first frame update
    void Start()
    {
        // Procurando a posição atual do player
        player = GameObject.FindGameObjectWithTag("Player").transform;
        var = GameObject.FindGameObjectWithTag("Player").GetComponent<player_controller>();

        // Instanciando as plataformas
        for(int i = 0; i < platforms.Count; i++)
        {
            Transform p = Instantiate(platforms[i], new Vector3(0,0,i * tam_rua), transform.rotation).transform;
            atual_platforms.Add(p);
            offset += 32;
        }

        // Instanciando o ambiente
        for(int i = 0; i < scenario.Count-3; i++)
        {
            Transform ambient_ = Instantiate(scenario[i], new Vector3(0,debug,i * tam_sce), transform.rotation).transform;
            ambient_.transform.name = "florest"+i.ToString();
            atual_scenario.Add(ambient_);
            offset_scenario += 150;
        }

        // Pegando a posi��o da plataforma atual onde o player est�
        point_plataforma_atual = atual_platforms[index].GetComponent<platforms>().point;
        point_scenario_atual = atual_scenario[index_scenario].GetComponent<platforms>().point;

        Transform finish = Instantiate(finish_line, new Vector3(0,debug,10000), transform.rotation).transform;
        finish.transform.Rotate(new Vector3(0, 90, 0));
        point_finish_line = finish.GetComponent<platforms>().point;

        // Define o tempo inicial entre o instanciamento de obstáculos
        currentSpawnDelay = initialSpawnDelay;

        // Inicia a chamada do método para instanciar obstáculos
        InvokeRepeating("SpawnObstacle", initialSpawnDelay, currentSpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        // Armazenando a diferen�a da posi��o entra player e plataforma atual **quando for zero a plataforma � destruida**
        float distance = player.position.z - point_plataforma_atual.position.z;
        float distance_finish = player.position.z - point_finish_line.position.z;
        float distance_scenario = player.position.z - point_scenario_atual.position.z;

        if (distance >= 5)
        {
            recycle(atual_platforms[index].gameObject,32);
            index++;

            if(index > atual_platforms.Count - 1)
            {
                index = 0;
            }

            point_plataforma_atual = atual_platforms[index].GetComponent<platforms>().point;
        }

        // Animação da linha de chegada
        if(distance_finish >= 0){
            var.finish_line_player();
        }

        // Teste da mudança de ambiente
        if(distance_scenario >= 25){
            if(!troca_ambient){
                recycle_scenario(atual_scenario[index_scenario].gameObject,145,ambient_x);
            }
            index_scenario++;

            if(index_scenario > atual_scenario.Count - 1)
            {
                index_scenario = 0;
            }
            point_scenario_atual = atual_scenario[index_scenario].GetComponent<platforms>().point;
        }

        if(index_scenario == 0 && troca_ambient)
        {
            troca_ambient = false;
        }

        switch(ambient)
        {
            case "florest":
                if(ui.distancia >= 100 && distance_scenario >= 25)
                {
                    if(index_scenario == 2)
                    {   
                        troca_ambient = true;
                        ambient_x = 10.7f;
                        for(int i = 3; i < scenario.Count; i++)
                        {
                            atual_scenario.RemoveAt(0);
                            Transform ambient_ = Instantiate(scenario[i], new Vector3(ambient_x,debug,offset_scenario-5), transform.rotation).transform;
                            ambient_.transform.name = "city"+(i-3).ToString();
                            atual_scenario.Add(ambient_);
                            offset_scenario += 145;
                        }  
                        ambient = "city";          
                    }
                }
            break;
        }
    }

    // Função para reciclagem das plataformas
    public void recycle(GameObject plataforma, int range)
    {
        plataforma.transform.position = new Vector3(0, 0, offset);
        offset += range;
    }

    // Função para reciclagem das plataformas
    public void recycle_scenario(GameObject plataforma, int range, float x)
    {
        plataforma.transform.position = new Vector3(x, debug, offset_scenario);
        offset_scenario += range;
    }

    void SpawnObstacle() {
        obstacles_number = Random.Range(1,1);
        obstacles_x = Random.Range(spawnPoints[0],spawnPoints[2]);
        // Verifica se existem pontos de spawn e obstáculos disponíveis
        if (spawnPoints.Length > 0 && obstacles.Length > 0 && obstacles_number == 1) {
            // Seleciona um obstáculo aleatoriamente
            GameObject selectedObstacle = obstacles[Random.Range(0, obstacles.Length)];

            // Seleciona um ponto de spawn aleatoriamente
            Transform selectedSpawnPoint = selectedObstacle.GetComponent<Transform>();

            // Instancia o obstáculo no ponto de spawn selecionado
            Instantiate(selectedObstacle, new Vector3(obstacles_x, selectedSpawnPoint.position.y, obstacles_z), selectedSpawnPoint.rotation);
        }
        else if(spawnPoints.Length > 0 && obstacles.Length > 0 && obstacles_number == 2)
        {
            if(obstacles_x == 2){
                obstacles_x--;
            }
            for(int i = 0; i < 2; i++)
            {
                GameObject selectedObstacle = obstacles[Random.Range(0, obstacles.Length)];

                // Seleciona um ponto de spawn aleatoriamente
                Transform selectedSpawnPoint = selectedObstacle.GetComponent<Transform>();

                // Instancia o obstáculo no ponto de spawn selecionado
                Instantiate(selectedObstacle, new Vector3(obstacles_x, selectedSpawnPoint.position.y, obstacles_z), selectedSpawnPoint.rotation);
                obstacles_x++;
            }
        }
        obstacles_z += 15;
    }

    void IncreaseDifficulty() {
        // Reduz o tempo entre o instanciamento de obstáculos para aumentar a dificuldade
        currentSpawnDelay -= spawnDelayReduction;

        // Verifica se o tempo entre o instanciamento de obstáculos é menor que 0.5f (mínimo)
        if (currentSpawnDelay < 0.5f) {
            currentSpawnDelay = 0.5f;
        }
    }

    // Chamado quando o jogador atinge um marco de dificuldade
    public void OnDifficultyIncrease() {
        // Aumenta a dificuldade chamando o método de aumento da dificuldade
        IncreaseDifficulty();
        // Cancela as chamadas repetidas do método de instanciar obstáculos
        CancelInvoke("SpawnObstacle");
        // Inicia novamente as chamadas repetidas com o novo tempo entre instanciamentos
        InvokeRepeating("SpawnObstacle", 0f, currentSpawnDelay);
    }
}