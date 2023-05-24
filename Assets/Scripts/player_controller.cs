using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour {

    // Variáveis publicas
    public float vel_move = 6f;
    public float jumpForce = 8f;
    public float gravity = 20f;
    //padrão 5f
    public float vel = 5f;

    // Referência ao CharacterController
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    // Variáveis de animação
    public Animator anim_player;
    public bool player_dead = false;
    public LayerMask layer;
    public LayerMask layer_coletavel;
    public float raio_colisao;

    // Variáveis de controle do player
    private bool exec_jump = false;
    public int vida;
    public bool vida_add = false;
    public bool vida_remove = false;

    // Controle da Interface de Usuário
    private ui_controller ui_control;

    void Start () {
        controller = GetComponent<CharacterController>();
        anim_player = GetComponent<Animator>();
        ui_control = FindObjectOfType<ui_controller>();
        vida = 3;
    }

    void Update () {
        // Verifica se o jogador está no chão
        if (controller.isGrounded) {
            // Obtém o input do movimento horizontal (esquerda/direita)
            float horizontalInput = Input.GetAxis("Horizontal");

            // Cria um vetor de movimento com base no input
            moveDirection = new Vector3(horizontalInput, 0, 0);

            // Multiplica pelo valor da velocidade
            moveDirection *= vel_move;

            // Verifica se o jogador pressionou o botão de pulo
            if (Input.GetKeyDown(KeyCode.Space) && !exec_jump) {
                // Adiciona uma força vertical ao vetor de movimento
                moveDirection.y = jumpForce;
                anim_player.SetBool("jump",true);
                exec_jump = true;
            }
            else if (exec_jump)
            {
                exec_jump = false;
                anim_player.SetBool("jump",false);
            }
            
            if(Input.GetKeyDown(KeyCode.R)){
                ui_control.show_tela();
            }
        }

        // Aplica a gravidade ao vetor de movimento
        moveDirection.y -= gravity * Time.deltaTime;
        moveDirection.z = vel;

        // Move o jogador com base no vetor de movimento
        controller.Move(moveDirection * Time.deltaTime);

        colisao();
    }

    void colisao()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0,1f,0)), out hit, raio_colisao, layer) && !player_dead && vida <= 1)
        {
            vel = 0;
            vel_move = 0;
            jumpForce = 0;
            player_dead = true;
            vida_remove = true;
            anim_player.SetBool("death",true);
        }
        else if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0,1f,0)), out hit, raio_colisao, layer) && !player_dead)
        {
            vida_remove = true;
            Destroy(hit.transform.gameObject);
            anim_player.SetBool("hit",true);
            Invoke("hit_end",.23f);
        }

        RaycastHit coin;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + new Vector3(0,1f,0)), out coin, raio_colisao*2, layer_coletavel))
        {
            ui_control.add_coin();
            vida_add = true;
            Destroy(coin.transform.gameObject);
        }
    }

    // Animação de quando o player leva hit
    void hit_end()
    {
        anim_player.SetBool("hit",false);
    }


    // Animação de final do jogo
    public void finish_line_player()
    {
        vel = 0;
        vel_move = 0;
        jumpForce = 0;
        player_dead = true;
        anim_player.SetBool("finish",true);
    }
}