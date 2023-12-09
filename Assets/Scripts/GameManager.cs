using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager gm;

    [Tooltip("If not set, the player will default to the gameObject tagged as Player.")]
    public GameObject player;

    public enum gameStates {Playing, Death, GameOver, BeatLevel};
    public gameStates gameState = gameStates.Playing;

    public int score = 0;
    public bool canBeatLevel = false;
    public int beatLevelScore = 0;

    public GameObject mainCanvas;
    public TextMeshProUGUI mainScoreDisplay;
    public GameObject gameOverCanvas;
    public TextMeshProUGUI gameOverScoreDisplay;

    [Tooltip("Only need to set if canBeatLevel is set to true.")]
    public GameObject beatLevelCanvas;

    public AudioSource backgroundMusic;
    public AudioClip gameOverSFX;

    [Tooltip("Only need to set if canBeatLevel is set to true.")]
    public AudioClip beatLevelSFX;

    private Health playerHealth;

    private bool passou = false;

    public GameObject winCanva;
    public TextMeshProUGUI txtWin;

    private int scoreWin = 3;

    void Start() {
        if (gm == null) {
            gm = gameObject.GetComponent<GameManager>();
        }

        if (player == null) {
            player = GameObject.FindWithTag("Player");
        }

        playerHealth = player.GetComponent<Health>();

        // setup score display
        Collect(0);

        // make other UI inactive
        gameOverCanvas.SetActive(false);
        winCanva.SetActive(false);
        if (canBeatLevel)
            beatLevelCanvas.SetActive(false);
    }

    void Update() {
        switch (gameState) {
            case gameStates.Playing:
                if (playerHealth.isAlive == false) {
                    // update gameState
                    gameState = gameStates.Death;

                    // set the end game score
                    gameOverScoreDisplay.text = mainScoreDisplay.text;

                    gameOverCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "GameOver";

                    // switch which GUI is showing		
                    mainCanvas.SetActive(false);
                    gameOverCanvas.SetActive(true);
                } else if (canBeatLevel && score >= beatLevelScore) {
                    // update gameState
                    gameState = gameStates.BeatLevel;

                    // hide the player so game doesn't continue playing
                    player.SetActive(false);

                    // switch which GUI is showing			
                    mainCanvas.SetActive(false);
                    beatLevelCanvas.SetActive(true);
                } else if (score >= scoreWin && SceneManager.GetActiveScene().name == "cena2") {
                    player.SetActive(false);
                 
                    // Mostra o Canvas da Vitória
                    if (winCanva != null) {
                        mainCanvas.SetActive(false);
                        winCanva.GetComponentInChildren<TextMeshProUGUI>().text = "You Win";
                        winCanva.SetActive(true);
                    }
                }
                break;
            case gameStates.Death:
                backgroundMusic.volume -= 0.01f;
                if (backgroundMusic.volume <= 0.0f) {
                    AudioSource.PlayClipAtPoint(gameOverSFX, gameObject.transform.position);
                    gameState = gameStates.GameOver;
                }
                break;
            case gameStates.BeatLevel:
                backgroundMusic.volume -= 0.01f;
                if (backgroundMusic.volume <= 0.0f) {
                    AudioSource.PlayClipAtPoint(beatLevelSFX, gameObject.transform.position);
                    gameState = gameStates.GameOver;
                }
                break;
            case gameStates.GameOver:
                // nothing
                break;
        }
    }

    void TrocarCena() {
        if (SceneManager.GetActiveScene().name != "cena2") {
            score = 0;
            SceneManager.LoadScene("cena2");
        }
    }

    public void Collect(int amount) {
        score += amount;
        if (canBeatLevel) {
            mainScoreDisplay.text = score.ToString() + " of " + beatLevelScore.ToString();
        } else {
            mainScoreDisplay.text = score.ToString();
        }

        if (score >= scoreWin && passou == false) {
             if (SceneManager.GetActiveScene().name != "cena2") {
                score = 0;
                SceneManager.LoadScene("cena2");
            }
        }
    }
}
