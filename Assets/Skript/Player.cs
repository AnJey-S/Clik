using UnityEngine;

public class Player : MonoBehaviour
{
    // ----------------------------------------------------------------
    //
    // Этот класс представляет игрока в бою. Он хранит информацию о здоровье, блоке и отравлении игрока, а также методы для получения урона, получения блока и проверки отравления.
    // Он взаимодействует с GameManager для получения текущего здоровья игрока и нанесения урона. Он также обрабатывает смерть игрока, выводя сообщение и уничтожая объект игрока.
    //
    // ----------------------------------------------------------------
    // Методы:
    //    - GainBlock(int amount): Этот метод увеличивает блок игрока на указанное количество.
    //    - PoisonCheck(): Этот метод проверяет, отравлен ли игрок, и если да, то наносит ему урон от отравления и уменьшает количество оставшихся ходов отравления.
    //    - AddPoison(int stacks): Этот метод добавляет указанное количество стаков отравления игроку.
    //    - TakeDamage(int damage): Этот метод наносит игроку урон, учитывая его текущий блок. Он уменьшает блок на количество урона и наносит оставшийся урон здоровью игрока через GameManager.
    //    - Death(): Этот метод вызывается, когда здоровье игрока достигает нуля или ниже. Он выводит сообщение о смерти и уничтожает объект игрока.
    // Поля:
    //    - block: Текущее количество блока игрока, который уменьшает входящий урон.
    //    - poisonedTime: Количество оставшихся ходов отравления, которые будут наносить урон игроку в начале каждого его хода.
    //    - Health: Свойство, возвращающее текущее здоровье игрока через GameManager.
    //    - Block: Свойство для доступа и изменения количества блока игрока.
    // ----------------------------------------------------------------
    private int block;
    public int poisonedTime = 0;
    public int Block { get => block; set => block = value; }

    public void GainBlock(int amount)
    {
        block += amount;
    }
    public void PoisonCheck()
    {
        if (poisonedTime > 0)
        {
            TakeDamage(poisonedTime);
            poisonedTime--;
        }
    }
    public void AddPoison(int stacks)
    {
        poisonedTime += stacks;
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(0, damage - block);
        block = Mathf.Max(0, block - damage);
        GameManager.Instance.DamagePlayer(finalDamage);
    }
    public int Health => GameManager.Instance.playerHP;
    public void Death()
    {
        Debug.Log("Упс! Вы умерли");
        Destroy(gameObject);
        // открыть какое-нибудь окно поражения
    }
}