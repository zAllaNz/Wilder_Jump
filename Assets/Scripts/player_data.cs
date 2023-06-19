using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_data : MonoBehaviour
{
    private TittleButton tittle;
    private string name;

    // Acessando as variáveis do script do mind wave
    private mind_wave mind;
    public string cena_gameplay;
    private bool cena_certa = false;

    public string file_name = @"";
    private string cena_atual;


    // Start is called before the first frame update
    void Start()
    {
        tittle = GameObject.Find("TitleScreen").GetComponent<TittleButton>();
        mind = GameObject.FindWithTag("Mind").GetComponent<mind_wave>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cena_certa)
        {
            cena_atual = SceneManager.GetActiveScene().name;
        }

        if (tittle.on_end_edit)
        {
            name = tittle.name_player.text;
            tittle.on_end_edit = false;
        }

        if (cena_atual == cena_gameplay && !cena_certa)
        {
            StartCoroutine(MakeData(Create_File()));
            cena_certa = true;
        }
    }

    // Transformar em objeto persistente
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Configurando o diretório e o nome do arquivo
    public string Create_File()
    {
        file_name += @"C:\Users\USER\Downloads\Wilder_Jump\Data\" + name.ToString() + ".csv";
        string file = Headers(file_name);
        return file;
    }
    public string Headers(string FileName)
    {
        string file = FileName;
        string[] HeaderText = {"Atencion Level, Meditation Level, Blink, Life, Distance, Speed"};
        File.WriteAllLines(file, HeaderText);
        return file;
    }

    IEnumerator MakeData(string file)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (Time.timeScale != 0)
            {
                float attention = mind_wave.sAttention;
                float meditation = mind_wave.sMeditation;
                DataRecord(file, attention, meditation);
            }
        }
    }
    public void DataRecord(string file, float AT, float ML)
    {
        string DataForWrite = Convert.ToString(AT) + "," + Convert.ToString(ML) + "\n";
        File.AppendAllText(file, DataForWrite);
    }
}
