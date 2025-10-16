using UnityEngine;

public class SpawnerDeNotas : MonoBehaviour
{
    public GameObject notaPrefab;
    public float alturaSpawn = 6f;
    public float intervaloEntreNotas = 1f;
    
    private float[] posicoesX = new float[] { -2f, -0.7f, 0.7f, 2f };
    private string[] tags = new string[] { "NotaA", "NotaS", "NotaJ", "NotaK" };
    private Color[] cores;
    
    private float tempoProximaNota = 0f;
    
    void Start()
    {
        cores = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
        tempoProximaNota = intervaloEntreNotas;
    }
    
    void Update()
    {
        tempoProximaNota -= Time.deltaTime;
        
        if (tempoProximaNota <= 0)
        {
            SpawnarNota();
            tempoProximaNota = intervaloEntreNotas;
        }
    }
    
    void SpawnarNota()
    {
        int indiceAleatorio = Random.Range(0, 4);
        
        Vector3 posicao = new Vector3(posicoesX[indiceAleatorio], alturaSpawn, 0);
        
        GameObject nota = Instantiate(notaPrefab, posicao, Quaternion.identity);
        
        nota.tag = tags[indiceAleatorio];
        nota.name = tags[indiceAleatorio];
        
        SpriteRenderer sprite = nota.GetComponent<SpriteRenderer>();
        sprite.color = cores[indiceAleatorio];
    }
}