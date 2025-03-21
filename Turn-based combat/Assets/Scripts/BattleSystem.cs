using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using Random = System.Random;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }
public enum BatleOptions { WEAPONS, SPELLS, CONSUMABLES, SKILLS}

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;
    public GameObject combatButtons;
    public GameObject choiceButton;
	static GameObject enemyOptions; 


    public Transform playerBattleStation;
	public Transform enemyBattleStation;
	public Transform playerHand;
	public Transform enemyHand;
	
	Unit playerUnit;
	Unit enemyUnit;

	AttackPattern attackPattern = new AttackPattern();
	
	public Text dialogueText;
	public Text[] buttonsText = new Text[4];

    public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;
	public BatleOptions option;
	public Random random = new Random();

// Start is called before the first frame update
    void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
	{
		GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = "A wild " + enemyUnit.UName + " approaches...";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	void PlayerTurn()
	{
        dialogueText.text = "Choose an action:";
        combatButtons.SetActive(true);
	}
	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN) { return; }

		option = BatleOptions.WEAPONS;

		ButtonsUpdate();

        dialogueText.text = "Choose an weapon:";

		combatButtons.SetActive(false);
		choiceButton.SetActive(true);
	}

    public void OnHealButton()
    {
		if (state != BattleState.PLAYERTURN) { return; }

        StartCoroutine(PlayerHeal());
    }

	public void ButtonsUpdate()
	{
		switch (option)
		{
			case BatleOptions.WEAPONS:

                foreach (var weapon in playerUnit.weapons)
                {
                    int i = 0;

                    buttonsText[i].text = weapon.name;

                    i++;
                }

				break;

			case BatleOptions.CONSUMABLES:


				break;

            case BatleOptions.SKILLS:

                break;

            case BatleOptions.SPELLS:

                break;        
		}
    }

	public void ButtonSelected(int index)
	{
        switch (option)
        {
            case BatleOptions.WEAPONS:

                StartCoroutine(PlayerAttack(playerUnit.weapons[index]));

                break;

            case BatleOptions.CONSUMABLES:


                break;

            case BatleOptions.SKILLS:

                break;

            case BatleOptions.SPELLS:

                break;
        }
    }

    public void button1()
    {
       ButtonSelected(0);
    }
    public void button2()
    {
        ButtonSelected(1);
    }
    public void button3()
    {
        ButtonSelected(2);
    }
    public void button4()
    {
        ButtonSelected(3);
    }

    IEnumerator PlayerAttack(GameObject weapon)
	{
        choiceButton.SetActive(false);
        
        GameObject weaponGO = Instantiate(weapon, playerHand);

        Weapon selectedWeapon = weaponGO.GetComponent<Weapon>();

        playerHUD.SetDie(playerUnit.setDice());

        if (playerUnit.currentDie > enemyUnit.armor)
		{
			int damage = anyDice(selectedWeapon.damage);

            bool isDead = enemyUnit.TakeDamage(damage);

            dialogueText.text = "You hit " + enemyUnit.UName + " with " + weapon.name + " for " + damage + " damage.";

            enemyHUD.SetHP(enemyUnit.currentHP);

            yield return new WaitForSeconds(2f);

            if (isDead)
			{
				state = BattleState.WON;
				EndBattle();
			}
			else
			{
				state = BattleState.ENEMYTURN;
				StartCoroutine(EnemyTurn());
			}

        } 
		else 
		{
            dialogueText.text = "The attack misses :(";

            yield return new WaitForSeconds(2f);

			state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        Destroy(weaponGO);
    }

	IEnumerator EnemyTurn()
	{
        object[] parameters = attackPattern.EnemyAction(enemyUnit, playerUnit);

        switch (parameters[0])
		{
			case "attack":

                dialogueText.text = enemyUnit.UName + " attacks " + playerUnit.UName + " with it's " + parameters[1];

                enemyHUD.SetDie(enemyUnit.setDice());

                GameObject weaponGO = Instantiate((GameObject)parameters[2], enemyHand.position, enemyHand.rotation * Quaternion.Euler(0f, 180f, 0f));
                Weapon selectedWeapon = weaponGO.GetComponent<Weapon>();

                yield return new WaitForSeconds(1f);

				if (enemyUnit.currentDie > playerUnit.armor)
				{
					int damage = anyDice(selectedWeapon.damage);
					bool isDead = playerUnit.TakeDamage(damage);

                    dialogueText.text = "The attack hits for " + damage + " damage.";

                    playerHUD.SetHP(playerUnit.currentHP);

                    if (isDead)
					{
                        state = BattleState.LOST;
                        EndBattle();
					}
                }
				else
				{
                    dialogueText.text = "The attack misses :)";

                }
                yield return new WaitForSeconds(2f);
				
				Destroy(weaponGO);

            break;

			case "healing":

				enemyUnit.Heal(enemyUnit.healing);
				enemyHUD.SetHP(enemyUnit.currentHP);

				dialogueText.text = enemyUnit.UName + " used " + parameters[1] + " healing for " + parameters[2] + " HP.";
				yield return new WaitForSeconds(2f);

            break;

        }

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	IEnumerator PlayerHeal()
	{
		playerUnit.Heal(playerUnit.healing);

		playerHUD.SetHP(playerUnit.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

    public int anyDice(int dice)
    {
        return random.Next(1, dice);
    }

}
