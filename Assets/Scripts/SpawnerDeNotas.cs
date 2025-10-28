using UnityEngine;

public class SpawnerDeNotas : MonoBehaviour
{
    public GameObject notaPrefab;
    public float alturaSpawn = 6f;
    public float intervaloEntreNotas = 1f;
    
    // MUDADO PARA 5 POSIÇÕES
    private float[] posicoesX = new float[] { -3f, -1.5f, 0f, 1.5f, 3f };
    private string[] tags = new string[] { "NotaA", "NotaS", "NotaJ", "NotaK", "NotaL" };
    private Color[] cores;
    
    private float tempoProximaNota = 0f;
    
    void Start()
    {
        // 5 CORES DIFERENTES
        cores = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };
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
        // MUDADO PARA 5 NOTAS
        int indiceAleatorio = Random.Range(0, 5);
        
        Vector3 posicao = new Vector3(posicoesX[indiceAleatorio], alturaSpawn, 0);
        
        GameObject nota = Instantiate(notaPrefab, posicao, Quaternion.identity);
        
        nota.tag = tags[indiceAleatorio];
        nota.name = tags[indiceAleatorio];
        
        SpriteRenderer sprite = nota.GetComponent<SpriteRenderer>();
        sprite.color = cores[indiceAleatorio];
    }
}