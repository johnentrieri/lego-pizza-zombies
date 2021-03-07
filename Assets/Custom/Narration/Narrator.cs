using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrator : MonoBehaviour
{
    [SerializeField] AudioClip wizardTalkAudioClip;
    private AudioSource audioSource;
    private Text narrationText;
    private CanvasGroup canvasGroup;

    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        narrationText = GetComponentInChildren<Text>();
        canvasGroup = GetComponent<CanvasGroup>();

        Hide();

        // Intro Narration
        Narrate("I'll keep the pizza coming, head out and start delivering!", 3.0f, 1.0f);
        Narrate("Use your mouse to look around and W,A,S,D keys to move.", 5.0f, 5.0f);
        Narrate("Left Click or Press E to toss a pizza to a hungry customer.", 5.0f, 11.0f);
    }

    public void Narrate( string message, float duration, float delay) {        
        StartCoroutine( IssueNarration(message,duration, delay) );        
    }

    private IEnumerator IssueNarration(string message, float duration, float delay) {
        yield return new WaitForSeconds(delay);
        narrationText.text = message;        
        Show(); 
        audioSource.PlayOneShot(wizardTalkAudioClip);               
        yield return new WaitForSeconds(duration);
        Hide();
    }

    private void Show() {
        canvasGroup.alpha = 1.0f;
    }

    private void Hide() {
        canvasGroup.alpha = 0.0f;
    }
}
