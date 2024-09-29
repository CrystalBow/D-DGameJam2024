using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardAnimation : MonoBehaviour
{
    private Image image;
    [SerializeField] private List<Sprite> cards = new List<Sprite>();
    [SerializeField] private Sprite cardBack;
    [SerializeField] private int card_id;
    private bool animating = false;
    private bool showingBack = true;
    private float time;

    void drawAnimation(int id, float duration, float holdTime) {
        time += Time.deltaTime;

        if (time < duration) {
            transform.localScale = new Vector3(1 - time/duration, 1, 1);
        }
        else if (time >= duration && time < duration * 2) {
            //Swap the new card in
            if (showingBack) {
                showingBack = false;
                image.sprite = cards[id];
            }

            transform.localScale = new Vector3(time/duration - 1, 1, 1);
        }
        else if (time >= 2 * duration + holdTime) {
            reset();
        }
    }

    void reset() {
        time = 0;
        showingBack = true;
        image.sprite = cardBack;
        animating = false;
        transform.localScale = Vector3.one;
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            drawAnimation(card_id, 0.2f, 1);
        }
    }

    void StartAnimation(int card_id)
    {
        animating = true;
        this.card_id = card_id;
    }
}
