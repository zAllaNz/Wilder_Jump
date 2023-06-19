using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_data : MonoBehaviour
{
    private TittleButton tittle;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        tittle = GameObject.Find("TitleScreen").GetComponent<TittleButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tittle.on_end_edit)
        {
            name = tittle.name_player.text;
            tittle.on_end_edit = false;
        }
    }

    // Transformar em objeto persistente
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
