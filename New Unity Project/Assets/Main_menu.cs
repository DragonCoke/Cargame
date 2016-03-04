﻿using UnityEngine;
using System.Collections;

public class Main_menu : MonoBehaviour {

    public Cartype[] cars;
    public Maptype[] scenes;
    public GUIStyle Title;
    public GUIStyle TitleBig;
    public Texture background;
    public bool drawBackground;
    private Vector2 mousepos;
    private int selectedmap = -1;
    private int fadeintimer = 0;
    private int selectedcar = -1;
    private int fadeintimer2 = 0;
    private int playoffset = 0;

    [System.Serializable]
    public class Maptype
    {
        public string name;
        public string fileName;
        [HideInInspector]public float xoffset = 0;
    }

    [System.Serializable]
    public class Cartype
    {
        public string name;
        [HideInInspector]
        public float xoffset = 0;
    }

    public static float quadOut(float CurrentStep, float TotalSteps, float StartValue, float ValueChange)
    {
        return -ValueChange * (CurrentStep /= TotalSteps) * (CurrentStep - 2) + StartValue;
    }



    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 1920.0f, Screen.height / 1080.0f, 1));

        if (drawBackground)
        { GUI.DrawTexture(new Rect(0, 0, 1920, 1080), background); }

        GUI.Label(new Rect(50, 50, 1, 1), "CARGAME", TitleBig);
        Title.fontSize = 65;
        Title.normal.textColor = Color.black;
        GUI.Label(new Rect(700, 300, 1F, 1F), "Anton Bergåker", Title);
        Title.fontSize = 90;

        mousepos = Event.current.mousePosition;
        for (int i = 0; i < scenes.Length;i++ )
        {
            float off;
            if (i == selectedmap)
            { off = 1; }
            else
            { off = -1; }
            scenes[i].xoffset = Mathf.Clamp(scenes[i].xoffset + off, 0F, 60F);

            float col = scenes[i].xoffset / 59;
            Title.normal.textColor = new Color(col,col,col);

            float easescene = quadOut(scenes[i].xoffset, 60f, 0f, 60f);
            GUI.Label(new Rect(100+easescene, 450 + 120 * i, 1, 1), scenes[i].name, Title);
        }

        if (mousepos.x > 100 && mousepos.x < 650) //if mouse is in map area
        {
            int mouseval = (int)Mathf.Floor((mousepos.y - 450F) / 120F); //get mouse height to select map
            if (mouseval >= 0 && mouseval < scenes.Length) //if it's within bounds
            {
                if (Input.GetMouseButtonDown(0))
                {
                    selectedmap = mouseval;
                }
                if (mouseval != selectedmap)
                { scenes[mouseval].xoffset = Mathf.Clamp(scenes[mouseval].xoffset+2F,0F,60F);}
            }
        }

        if (selectedmap != -1)
        {
            fadeintimer = Mathf.Clamp(fadeintimer+1,0,60);
            float fadeinease = quadOut(fadeintimer, 60f, 0f, 60f);

            for (int i = 0; i < cars.Length; i++)
            {
                float caroffset = quadOut(cars[i].xoffset, 60f, 0f, 60f);
                float off;
                if (i == selectedcar)
                { off = 1; }
                else
                { off = -1; }
                cars[i].xoffset = Mathf.Clamp(cars[i].xoffset + off, 0F, 60F);
                float col = cars[i].xoffset / 59;

                //set correct color
                Title.normal.textColor = new Color(col, col, col,fadeintimer/60F);
                GUI.Label(new Rect(580 + caroffset+ fadeinease*2, 500 + 120 * i, 1, 1), cars[i].name, Title);
            }

            if (mousepos.x > 720 && mousepos.x < 1200)
            {
                int mouseval = (int)Mathf.Floor((mousepos.y - 500F) / 120F);
                if (mouseval >= 0 && mouseval < cars.Length)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        selectedcar = mouseval;
                    }
                    if (mouseval != selectedcar)
                    { cars[mouseval].xoffset = Mathf.Clamp(cars[mouseval].xoffset + 2F, 0F, 60F); }
                }
            }
        }

        if (selectedcar != -1)
        {
            fadeintimer2 = Mathf.Clamp(fadeintimer2 + 1, 0, 60);
            float fadeinease = quadOut(fadeintimer2, 60f, 0f, 60f);
            float playease = quadOut(playoffset, 60f, 0f, 60f);

            playoffset = Mathf.Clamp(playoffset -1, 0, 60);

            //set correct color
            float col = (float)playoffset / 59;
            Title.normal.textColor = new Color(col, col, col, fadeintimer2 / 60F);
            GUI.Label(new Rect(1100 + playease + fadeinease * 2, 550 , 1, 1), "Start", Title);
            

            if (mousepos.x > 1200 && mousepos.x < 1600)
            {
                int mouseval = (int)Mathf.Floor((mousepos.y - 550F) / 120F);
                if (mouseval == 0)
                {
                    if (Input.GetMouseButtonDown(0)) //move into the game
                    {
                        PlayerPrefs.SetInt("selectedcar", selectedcar);
                        Application.LoadLevel(scenes[selectedmap].fileName);
                    }
                playoffset = Mathf.Clamp(playoffset + 2, 0, 60); 
                }
            }
        }

     }

	
	// Update is called once per frame
	void Update () {
	
	}
}
