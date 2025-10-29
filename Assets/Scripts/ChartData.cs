using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChartData
{
    public string nomeFase;
    public string nomeMusica;  // NOVO - nome da música
    public float duracaoTotal;
    public List<NotaData> notas = new List<NotaData>();
}

[System.Serializable]
public class NotaData
{
    public float tempo;
    public string tecla;
    
    public NotaData(float tempo, string tecla)
    {
        this.tempo = tempo;
        this.tecla = tecla;
    }
}