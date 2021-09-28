using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Views;

namespace Code.Controllers
{
    internal sealed class PlayerHudController: IController, IInitialization
    {
        private readonly PlayerInitialization _initialization;
        
        private PlayerHudView _hud;
        
        private int _health;
        private int _armor;
        private int _ammo;
        private int _maxAmmo;

        public PlayerHudController(PlayerInitialization playerInitialization)
        {
            _initialization = playerInitialization;
        }

        public void Initialization()
        {
            _hud = _initialization.GetPlayerHud();
        }
    
        public void SetMaxAmmo(int maxAmmo)
        {
            if (_maxAmmo == maxAmmo) 
                return;
        
            _hud.AmmoText.text = $"Ammo: {_ammo}/{maxAmmo}";
            _maxAmmo = maxAmmo;
        }
    
        public void SetHealth(int health)
        {
            if (_health == health) 
                return;
        
            _hud.HealthText.text = $"Health: {health}";
            _health = health;
        }
    
        public void SetArmor(int armor)
        {
            if (armor == _armor)
                return;

            _hud.ArmorText.text = $"Armor: {armor}";
            _armor = armor;
        }
    
        public void SetAmmo(int ammo)
        {
            if (_ammo == ammo)
                return;

            _hud.AmmoText.text = $"Ammo: {ammo}/{_maxAmmo}";
            _ammo = ammo;
        }
    }
}