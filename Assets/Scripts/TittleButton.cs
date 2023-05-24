using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleButton : MonoBehaviour
{
    // Tela do Mindwave
    public GameObject mind_obj;
    private bool mind_control = false;

    // Tela das configurações
    public GameObject config_obj;
    private bool config_control = false;

    // Tela das instruições
    public GameObject instr_obj;
    private bool instr_control = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void next_scene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void mind_open()
    {
        if(!mind_control)
        {
            mind_obj.SetActive(true);
            mind_control = true;
        }
        else
        {
            mind_obj.SetActive(false);
            mind_control = false;
        }
    }

    public void config_open()
    {
        if(!config_control)
        {
            config_obj.SetActive(true);
            config_control = true;
        }
        else
        {
            config_obj.SetActive(false);
            config_control = false;
        }
    }

    public void instr_open()
    {
        if(!instr_control)
        {
            instr_obj.SetActive(true);
            instr_control = true;
        }
        else
        {
            instr_obj.SetActive(false);
            instr_control = false;
        }
    }
}
