using System;

public class WeaponProgram
{
    class Weapon
    {
        private int _damage;
        private int _bullets;

        public void Fire(Player player)
        {
            if (_damage < 0)
                throw new Exception("Damage is less than 0");

            if (_bullets <= 0)
                return

            player.TakeDamage(_damage);
            _bullets -= 1;
        }
    }

    class Player
    {
        public int Health { get; private set; }

        public void TakeDamage(int damage)
        {
            if (Health <= 0)
                return

            Health -= Math.Max(Health - damage, 0);
        }
    }

    class Bot
    {
        private Weapon _weapon;

        public void OnSeePlayer(Player player)
        {
            Weapon.Fire(player);
        }
    }
}
