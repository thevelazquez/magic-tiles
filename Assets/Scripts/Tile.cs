using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class Tile : MonoBehaviour
{
    //public bool isActive = true;
    Color colorStart = Color.red;
    Color colorEnd = Color.yellow;
    Renderer rend;
    int iterations;
    bool isAlive;
    AudioSource makeSound;
    public AudioClip breakIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        iterations = 3;
        isAlive = true;
        makeSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [Button]
    public void Disappear() {
        isAlive = false;
        StartCoroutine(Animate());
    }
    
    IEnumerator Animate() {
        float timePassed = 0;
        float duration = .25f;
        iterations--;
        //Vector3 defaultScale = transform.localScale;
        while (timePassed < duration) {
            makeSound.PlayOneShot(breakIndicator,0.1f);
            float lerp = Mathf.PingPong(timePassed, duration) / duration;
            rend.material.color = Color.Lerp(colorStart, colorEnd, lerp);
            //transform.localScale = Vector3.Lerp(transform.localScale*hitScale, defaultScale, lerp);
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        if (iterations>0) {
            Disappear();
        } else {
            gameObject.SetActive(false);
        }
    }
    public bool GetIsAlive() {
        return isAlive;
    }
}
