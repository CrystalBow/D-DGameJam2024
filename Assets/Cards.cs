using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Random = System.Random;

public class Cards 
{
    private static List<String> CardTypes = new List<string>();
    private int ID;

    public int ID1 => ID;

    public string Name1 => Name;

    private string Name;
    private Random _random = new Random();
    
    public Cards()
    {
        birthCheck();
        int val = _random.Next(0, 1000);
        ID = val % 11;
        Name = CardTypes[ID];
    }
    public void birthCheck()
    {
        if (CardTypes.Count == 0)
        {
            CardTypes.Add("Icarus");
            CardTypes.Add("The Mist");
            CardTypes.Add("The Cat");
            CardTypes.Add("The Boulder");
            CardTypes.Add("The Journey");
            CardTypes.Add("Contingency");
            CardTypes.Add("The Fates");
            CardTypes.Add("The Void");
            CardTypes.Add("The Idiot");
            CardTypes.Add("The Jester");
            CardTypes.Add("The Spire");
        } 
    }
}
