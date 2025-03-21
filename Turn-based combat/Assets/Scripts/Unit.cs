using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Unit : MonoBehaviour
{
    public Random random = new Random();

    public string UName;
	public int level;
	public int maxHP;
	public int currentHP;
	public int currentDie;
	public int armor;

	public int healing;
    public GameObject[] weapons = new GameObject[4];
    public GameObject[] consumables = new GameObject[4];
    public GameObject[] skills = new GameObject[4];
    public GameObject[] spells = new GameObject[4];

    public bool TakeDamage(int dmg)
	{
			currentHP -= dmg;

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}

	public int setDice()
	{
		return currentDie = random.Next(1,20);

	}

}
