using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{

	public Text nameText;
	public Text levelText;
	public Slider hpSlider;
	public Text diceNumber;

	public void SetHUD(Unit unit)
	{
		nameText.text = unit.UName;
		levelText.text = "Lvl " + unit.level;
		hpSlider.maxValue = unit.maxHP;
		hpSlider.value = unit.currentHP;
		diceNumber.text = unit.currentDie.ToString();
	}

	public void SetHP(int hp)
	{
		hpSlider.value = hp;
	}

	public void SetDie(int die) { 
	
		diceNumber.text = die.ToString();

	}

}
