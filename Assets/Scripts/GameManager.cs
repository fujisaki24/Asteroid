using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainmenu_ui;
    public GameObject game_ui;
    public GameObject pause_ui;
    public GameObject gameover_ui;

    [Header("Game Logic")]
    public GameObject asteroid_spawner;
    public ParticleSystem playthrust;

    [Header("Misc")]
    public Player player;
    public int lives = 3;
    public float respawnRate = 3.0f;
    public int score = 0;
    private int highscore;

    public AudioSource source;
    public AudioClip explosion_clip;
    public AudioClip shoot_clip;
    public AudioClip hurt_clip;

    public TextMeshProUGUI life_text;
    public TextMeshProUGUI score_text;
    public TextMeshProUGUI finalscore;
    public TextMeshProUGUI highscore_text;


    private void Awake()
    {
        score_text.text = score.ToString();
        life_text.text = lives.ToString();

        mainmenu_ui.SetActive(true);
        game_ui.SetActive(false);
        pause_ui.SetActive(false);
        gameover_ui.SetActive(false);
        asteroid_spawner.SetActive(false);
        highscore = PlayerPrefs.GetInt("High Score");
        

    }

    private void Start()
    {
        playthrust.Play();
    }


    public void OnPlayBtn()
    {
        mainmenu_ui.SetActive(false);
        game_ui.SetActive(true);
        asteroid_spawner.SetActive(true);
        playthrust.Stop();
    }

    public void OnPauseExitBtn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void OnMenuExitBtn()
    {
        Application.Quit();
    }

    public void OnPauseBtn()
    {
        game_ui.SetActive(false);
        pause_ui.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnResumeBtn()
    {
        game_ui.SetActive(true);
        pause_ui.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void onRetryBtn()
    {
        this.lives = 3;
        this.score = 0;
        life_text.text = lives.ToString();
        score_text.text = score.ToString();
        game_ui.SetActive(true);
        gameover_ui.SetActive(false);
        asteroid_spawner.SetActive(true);
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        Respawn();
    }


    public void AsteroidHitSound()
    {
        source.PlayOneShot(explosion_clip, 0.5f);
    }

    public void PlayerShootSound()
    {
        source.PlayOneShot(shoot_clip, 0.5f);
    }

    public void PlayerHitSound()
    {
        source.PlayOneShot(hurt_clip, 0.5f);
    }

    public void AsteroidHitScore(float size)
    {
        if(size<0.3f)
        {
            score += 100;
        }
        else if(size<0.5f)
        {
            score += 50;
        }
        else
        {
            score += 25;
        }

        score_text.text = score.ToString();
    }


    public void PlayerDied()
    {
        this.lives--;
        life_text.text = lives.ToString();
        if(this.lives<=0)
        {
            //DebugRelife();
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnRate);
        }
    }

    void Respawn()
    {
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
    }

    void GameOver()
    {
        gameover_ui.SetActive(true);
        game_ui.SetActive(false);
        asteroid_spawner.SetActive(false);
        finalscore.text = "Score: " + score.ToString();
        if (score > highscore)
        {
            PlayerPrefs.SetInt("High Score", score);
        }
        highscore = PlayerPrefs.GetInt("High Score");
        highscore_text.text = "High Score: " + highscore.ToString();
        Debug.Log("Game Over!!!");
    }


}
