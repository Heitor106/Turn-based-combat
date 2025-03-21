using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEditor.VersionControl;
using UnityEngine;
using Random = System.Random;

public class AttackPattern {

    public Random random = new Random();
    public object[] parameters = new object[3];

    public string Test()
    {
        return "test yeah";
    }
    public object[] EnemyAction(Unit Enemy, Unit Player)
    {
        switch (Enemy.UName)
        {
            case "The Rock":

            return RockAttack(Enemy, Player);
        }
        return null;
    }

    public object[] RockAttack(Unit enemy, Unit player)
    {
        if (((double)enemy.currentHP/enemy.maxHP) <= 0.2)
        {
            parameters[0] = "healing";
            parameters[1] = "Healing Potion";
            parameters[2] = enemy.healing;
        } else {
            parameters[0] = "attack";
            parameters[1] = enemy.weapons[0].name;
            parameters[2] = enemy.weapons[0]; 
        }
        return parameters;
    }
}
