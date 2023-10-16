using UnityEngine;
using System.Collections;

public class CombatScript : MonoBehaviour
{
    public Entity player;
    public Entity enemy;

    private bool isPlayerTurn = true;

    void Start()
    {
        player = new Entity("Player", 100, 20, 5);
        enemy = new Entity("Enemy", 50, 5, 10);
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            // For simplicity, the player will attack automatically each turn
            player.Attack(enemy);
            isPlayerTurn = false;
        }
        else
        {
            enemy.Attack(player);
            isPlayerTurn = true;
        }

        // Check for end of combat
        if (!player.IsAlive())
        {
            Debug.Log("Player is defeated!");
            // Implement game over logic here
        }
        else if (!enemy.IsAlive())
        {
            Debug.Log("Enemy is defeated!");
            // Implement victory logic here
        }
    }

    public class Entity
    {
        public string name;
        public int hp;
        public int attack;
        public int defense;

        public Entity(string name, int hp, int attack, int defense)
        {
            this.name = name;
            this.hp = hp;
            this.attack = attack;
            this.defense = defense;
        }

        public bool IsAlive()
        {
            return hp > 0;
        }

        public void TakeDamage(int damage)
        {
            hp -= damage;
            hp = Mathf.Max(0, hp);
            Debug.Log(name + " has " + hp + " HP left.");
        }

        public void Attack(Entity target)
        {
            int damage = Mathf.Clamp(attack - target.defense + Random.Range(-5, 6), 1, int.MaxValue);
            Debug.Log(name + " attacks " + target.name + " for " + damage + " damage.");
            target.TakeDamage(damage);
        }
    }
}
