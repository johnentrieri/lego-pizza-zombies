using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] GameObject wizardBubble;
    [SerializeField] GameObject pizzaBubble;
    [SerializeField] float introDelay = 1.0f;  
    private TextMeshProUGUI wizardText;
    private TextMeshProUGUI pizzaText;

    void Start()
    {
        wizardText = wizardBubble.GetComponentInChildren<TextMeshProUGUI>();
        pizzaText = pizzaBubble.GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine( StartCutscene() );
    }

    void Update() {
        if ( Input.GetButtonDown("Toss") ) {
            LoadGameScene();
        }
    }

    private void LoadGameScene() {
        SceneManager.LoadScene("Game"); 
    }
    private IEnumerator StartCutscene() {
        yield return new WaitForSeconds(introDelay);

        PizzaSpeak(" Can't believe it's National Pizza Day already! ");
        yield return new WaitForSeconds(4.0f);

        PizzaSpeak(" Are you ready for the rush? ");
        yield return new WaitForSeconds(4.0f);
        
        Silence();
        yield return new WaitForSeconds(2.0f);
        
        WizardSpeak("  ...he's...  ");
        yield return new WaitForSeconds(2.0f);

        WizardSpeak("  ...he's a madman...  ");
        yield return new WaitForSeconds(3.0f);

        Silence();
        yield return new WaitForSeconds(2.0f);

        PizzaSpeak(" ... ");
        yield return new WaitForSeconds(2.0f);

        PizzaSpeak(" ...are you okay? ");
        yield return new WaitForSeconds(3.0f);

        Silence();
        yield return new WaitForSeconds(2.0f);

        WizardSpeak("  ...the boss...  ");
        yield return new WaitForSeconds(3.0f);

        WizardSpeak("  ...he changed the promise...  ");
        yield return new WaitForSeconds(3.0f);

        WizardSpeak("  ...instead of 30 minutes...  ");
        yield return new WaitForSeconds(4.0f);

        WizardSpeak("  ...28 minutes or less...  ");
        yield return new WaitForSeconds(4.0f);

        Silence();
        yield return new WaitForSeconds(0.5f);

        PizzaSpeak(" HE DID WHAT ?!?! ");
        yield return new WaitForSeconds(3.0f);

        PizzaSpeak(" ON NATIONAL PIZZA DAY ?!?! ");
        yield return new WaitForSeconds(3.0f);

        Silence();
        yield return new WaitForSeconds(2.0f);

        WizardSpeak("  ...there's no hope...  ");
        yield return new WaitForSeconds(3.0f);

        WizardSpeak("  ...the town...  ");
        yield return new WaitForSeconds(3.0f);

        WizardSpeak("  ...they'll eat you alive...  ");
        yield return new WaitForSeconds(5.0f);  

        LoadGameScene();    
    }

    private void WizardSpeak(string speech) {

        pizzaBubble.SetActive(false);
        wizardBubble.SetActive(false);

        wizardBubble.SetActive(true);
        wizardText.text = speech;
    }

    private void PizzaSpeak(string speech) {

        pizzaBubble.SetActive(false);
        wizardBubble.SetActive(false);

        pizzaBubble.SetActive(true);
        pizzaText.text = speech;
    }

    private void Silence() {

        pizzaBubble.SetActive(false);
        wizardBubble.SetActive(false);
    }

}
