using System;

public class WeaponProgram
{
    class Weapon
    {
        private readonly int _damage;
        private int _bullets;
        private readonly int _bulletsForShot;

        public Weapon(int damage, int bullets, int bulletsForShot)
        {
            if (damage < 0)
                throw new System.ArgumentOutOfRangeException("Урон не может быть меньше 0!");

            if (bullets < 0)
                throw new System.ArgumentOutOfRangeException("Запась пуль не может быть меньше 0!");

            if (bulletsForShot < 0)
                throw new System.ArgumentOutOfRangeException("Трата пуль на выстрел не может быть меньше 0!");

            _damage = damage;
            _bullets = bullets;
            _bulletsForShot = bulletsForShot;
        }

        public void Shoot(Player player)
        {
            if (CanShoot() == false)
                throw new System.InvalidOperationException("Не хватает пуль для выстрела!");

            if (player.CanDamage())
                player.Damage(_damage);

            _bullets -= _bulletsForShot;
        }

        public bool CanShoot()
        {
            return _bullets >= _bulletsForShot;
        }
    }

    class Player
    {
        public Player(int health)
        {
            if (health < 1)
                throw new System.ArgumentOutOfRangeException("Невозможно создать игрока с количеством здоровья меньше 1!");

            Health = health;
            IsDead = false;
        }

        public int Health { get; private set; }
        public bool IsDead { get; private set; }

        public void Damage(int damage)
        {
            if (IsDead == true)
                throw new System.InvalidOperationException("Игрок мертв!");

            Health -= Math.Max(Health - damage, 0);

            if (Health == 0)
                Die();
        }

        private void Die()
        {
            IsDead = true;
        }
    }

    class Bot
    {
        private Weapon _weapon;

        public Bot(Weapon weapon)
        {
            if (weapon == null)
                throw new System.NullReferenceException("Оружие не может содержать null!");

            _weapon = weapon;
        }

        public void OnSeePlayer(Player player)
        {
            if (player == null)
                throw new System.NullReferenceException("Игрок не может быть null!");

            if (_weapon.CanShoot())
                _weapon.Shoot(player);
        }
    }
}
