using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TilePicker : MonoBehaviour
{
    GameObject[] children = new GameObject[25];
    Tile childOne;
    GameObject child;
    public int countDownTime;
    public Text countDownDisplay;
    public Text gameStatus;
    public Text gameInstructions;
    int destroyIterations;
    public GameObject player;
    MovementInput playerScript;
    AudioSource makeSound;
    public AudioClip gameMusic;
    public AudioClip loseSound;
    public AudioClip winSound;
    public ParticleSystem winParticles;

    // Start is called before the first frame update
    void Start()
    {
        countDownTime = 10;
        destroyIterations = 4;
        for (int i = 0; i < 25; i++) {
            children[i] = transform.GetChild(i).gameObject;
        }
        playerScript = player.GetComponent<MovementInput>();
        makeSound = GetComponent<AudioSource>();
        makeSound.clip = gameMusic;
        makeSound.volume = 0.3f;
        StartCoroutine(gameStart());
        gameInstructions.text = "Use WASD to avoid the breaking tiles";
        winParticles.Stop();
    }

    IEnumerator gameStart() {
        yield return new WaitForSeconds(2);
        makeSound.Play();
        StartCoroutine(CountDown());
        gameInstructions.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    [Button]
    void destroyTiles() {
        for (int i=0; i<5; i++) {
            pickTile();
        }
    }
    void pickTile() {
        destroyTile(children[Random.Range(0,25)]);
    }
    void destroyTile(GameObject tile) {
        childOne = tile.GetComponent<Tile>();
        if (childOne.GetIsAlive()) {
            childOne.Disappear();
        } else {
            pickTile();
        }
    }

    IEnumerator CountDown() {
        while (countDownTime > 0) {
            countDownDisplay.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
            if (countDownTime == 8) {
                StartCoroutine(TileDestroyer());
            }
        }
        countDownDisplay.text = "Time's up!";
        gameStatus.text = "You win!";
        playerScript.Velocity = 0f;
        makeSound.Stop();
        makeSound.PlayOneShot(winSound);
        winParticles.Play();
    }
    IEnumerator TileDestroyer() {
        while (destroyIterations > 0) {
            destroyTiles();
            yield return new WaitForSeconds(2f);
            destroyIterations--;
        }
    }
    public void PlayLose() {
        makeSound.Stop();
        makeSound.PlayOneShot(loseSound);
    }
}
