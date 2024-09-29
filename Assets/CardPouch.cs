using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardPouch : MonoBehaviour
{
    [SerializeField] private GameObject Owner;
    private List<Cards> Collection;
    private bool isThereNewCard;

    public GameObject Owner1
    {
        get => Owner;
        set => Owner = value;
    }

    public void deal(Cards cards)
    {
        Collection.Add(cards);
    }

    public void discard()
    {
        
    }

    public bool IsThereNewCard
    {
        get => isThereNewCard;
        set => isThereNewCard = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
