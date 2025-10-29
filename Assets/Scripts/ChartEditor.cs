using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ChartEditor : MonoBehaviour
{
    public Text textoStatus;
    public Text textoTimer;
    public InputField inputNomeFase;
    public Text textoContador;
    public Dropdown dropdownMusicas;
    public Text textoContagem;
    
    private ChartData chartAtual;
    private bool gravando = false;
    private float tempoInicio;
    private float tempoDecorrido;
    
    private AudioSource audioSource;
    private List<string> nomesMusicas = new List<string>();
    
    void Start()
    {
        chartAtual = new ChartData();
        
        // Adiciona AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        
        CarregarListaDeMusicas();
        AtualizarStatus("Selecione uma música e pressione ESPAÇO");
        
        if (textoContagem != null)
        {
            textoContagem.gameObject.SetActive(false);
        }
    }
    
    void CarregarListaDeMusicas()
    {
        nomesMusicas.Clear();
        
        // Carrega todas as músicas da pasta Resources/Music
        AudioClip[] musicas = Resources.LoadAll<AudioClip>("Music");
        
        if (dropdownMusicas != null)
        {
            dropdownMusicas.ClearOptions();
            
            if (musicas.Length == 0)
            {
                dropdownMusicas.options.Add(new Dropdown.OptionData("Nenhuma música encontrada"));
                AtualizarStatus("AVISO: Coloque músicas em Assets/Resources/Music/");
            }
            else
            {
                foreach (AudioClip musica in musicas)
                {
                    nomesMusicas.Add(musica.name);
                    dropdownMusicas.options.Add(new Dropdown.OptionData(musica.name));
                }
                
                dropdownMusicas.RefreshShownValue();
                AtualizarStatus("Músicas carregadas! Selecione uma e pressione ESPAÇO.");
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gravando && !audioSource.isPlaying)
            {
                StartCoroutine(IniciarComContagem());
            }
        }
        
        if (gravando)
        {
            tempoDecorrido = Time.time - tempoInicio;
            AtualizarTimer();
            GravarInputs();
            
            // Para automaticamente quando a música acabar
            if (!audioSource.isPlaying)
            {
                PararGravacao();
            }
        }
    }
    
    IEnumerator IniciarComContagem()
    {
        // Carrega a música selecionada
        if (nomesMusicas.Count == 0)
        {
            AtualizarStatus("ERRO: Nenhuma música disponível!");
            yield break;
        }
        
        string musicaSelecionada = nomesMusicas[dropdownMusicas.value];
        AudioClip clip = Resources.Load<AudioClip>("Music/" + musicaSelecionada);
        
        if (clip == null)
        {
            AtualizarStatus("ERRO: Não foi possível carregar a música!");
            yield break;
        }
        
        audioSource.clip = clip;
        
        // Mostra contagem regressiva
        if (textoContagem != null)
        {
            textoContagem.gameObject.SetActive(true);
            
            for (int i = 5; i >= 1; i--)
            {
                textoContagem.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            
            textoContagem.text = "GO!";
            yield return new WaitForSeconds(0.5f);
            
            textoContagem.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        
        IniciarGravacao();
    }
    
    void IniciarGravacao()
    {
        gravando = true;
        tempoInicio = Time.time;
        chartAtual.notas.Clear();
        
        // Toca a música
        audioSource.Play();
        
        AtualizarStatus("GRAVANDO! Aperte A, S, J, K, L no ritmo!");
    }
    
    void PararGravacao()
    {
        gravando = false;
        audioSource.Stop();
        chartAtual.duracaoTotal = tempoDecorrido;
        AtualizarStatus("Gravação finalizada! " + chartAtual.notas.Count + " notas gravadas.");
    }
    
    void GravarInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AdicionarNota("A");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AdicionarNota("S");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AdicionarNota("J");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            AdicionarNota("K");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AdicionarNota("L");
        }
    }
    
    void AdicionarNota(string tecla)
    {
        float tempo = Time.time - tempoInicio;
        chartAtual.notas.Add(new NotaData(tempo, tecla));
        AtualizarContador();
        Debug.Log("Nota " + tecla + " gravada em " + tempo.ToString("F2") + "s");
    }
    
    void AtualizarTimer()
    {
        if (textoTimer != null)
        {
            int minutos = Mathf.FloorToInt(tempoDecorrido / 60);
            int segundos = Mathf.FloorToInt(tempoDecorrido % 60);
            textoTimer.text = string.Format("Tempo: {0:00}:{1:00}", minutos, segundos);
        }
    }
    
    void AtualizarStatus(string mensagem)
    {
        if (textoStatus != null)
        {
            textoStatus.text = mensagem;
        }
        Debug.Log(mensagem);
    }
    
    void AtualizarContador()
    {
        if (textoContador != null)
        {
            textoContador.text = "Notas: " + chartAtual.notas.Count;
        }
    }
    
    public void SalvarChart()
    {
        if (chartAtual.notas.Count == 0)
        {
            AtualizarStatus("ERRO: Nenhuma nota gravada!");
            return;
        }
        
        string nomeFase = inputNomeFase != null ? inputNomeFase.text : "Fase_Teste";
        
        if (string.IsNullOrEmpty(nomeFase))
        {
            nomeFase = "Fase_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }
        
        chartAtual.nomeFase = nomeFase;
        
        // Salva também o nome da música
        if (nomesMusicas.Count > 0)
        {
            chartAtual.nomeMusica = nomesMusicas[dropdownMusicas.value];
        }
        
        string json = JsonUtility.ToJson(chartAtual, true);
        
        string caminho = Application.dataPath + "/Resources/Charts/" + nomeFase + ".json";
        
        // Cria a pasta se não existir
        string pasta = Path.GetDirectoryName(caminho);
        if (!Directory.Exists(pasta))
        {
            Directory.CreateDirectory(pasta);
        }
        
        File.WriteAllText(caminho, json);
        
        AtualizarStatus("Chart salvo: " + nomeFase + ".json (" + chartAtual.notas.Count + " notas)");
        
        Debug.Log("Chart salvo em: " + caminho);
    }
    
    public void LimparChart()
    {
        chartAtual = new ChartData();
        gravando = false;
        audioSource.Stop();
        
        if (textoContagem != null)
        {
            textoContagem.gameObject.SetActive(false);
        }
        
        AtualizarStatus("Chart limpo! Selecione uma música e pressione ESPAÇO.");
        AtualizarContador();
    }
    
    public void PararGravacaoManual()
    {
        if (gravando)
        {
            PararGravacao();
        }
    }
}