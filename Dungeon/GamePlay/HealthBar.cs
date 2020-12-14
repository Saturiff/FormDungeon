using System.Windows.Forms;

namespace DungeonGame
{
    public class HealthBar : ProgressBar
    {
        public HealthBar() 
            => Maximum = CharacterBase.MaxHealth;

        public new int Value
        {
            get => base.Value;
            set
            {
                base.Value = (value < 0) ? 0 : value;
                this.SetState((value > 120) ? 1 : (value > 60) ? 3 : (value != 0) ? 2 : 0);
            }
        }
    }
}
