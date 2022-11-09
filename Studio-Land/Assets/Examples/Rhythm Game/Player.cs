using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Class <c>Player</c> that handles player input, score, and the score UI.
/// </summary>
public class Player : MonoBehaviour
{
    /* Fields for scoring */
    private int score;
    private int combo;
    private int maxCombo;

    /* External objects */
    private BeatController controller;
    private TMP_Text scoreText;

    void Awake()
    {
        score = 0;
        combo = 0;
        maxCombo = 0;
        controller = GameObject.Find("Beat Controller").GetComponent<BeatController>();
        scoreText = GameObject.Find("Score Text").GetComponent<TMP_Text>();
    }

    void Update()
    {
        updateText();
        if(controller.getCurrentlyLiveNotes().Count == 0)
            return;
        if(Input.GetKeyDown(KeyCode.A)){
            Note currNote = controller.getCurrentlyLiveNotes().Peek();
            float currPos = currNote.transform.position.y;
            Debug.Log(currPos);
            if(currPos > -2 - Note.DEFAULT_LEEWAY && currPos < -2 + Note.DEFAULT_LEEWAY){
                HitNote(currNote);
            }
        }
    }


    /* Helpers */
    
    // Increments score and destroys the hit note
    private void HitNote(Note note)
    {
        score += 1;
        combo += 1;
        Destroy(note.gameObject);
        controller.DequeueFrontNote();
        //controller.PlayHit();
    }

    private void updateText()
    {
        scoreText.text = $"Score: {score}\nCombo: {combo}\ncurBeat: {controller.songPositionInBeats}";
    }
    
    /* Getters and Setters */
    public int getScore() { return score; }
    public int getFinalScore() { return maxCombo == 0 ? score * combo : score * maxCombo; }
    public void resetPlayerCombo() { if (combo > maxCombo) maxCombo = combo; combo = 0; updateText(); }
}
