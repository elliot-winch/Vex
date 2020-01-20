namespace Vex
{
    public class BuffHealth : GameAction
    {
        public Player Agent { get; private set; }
        public Player Target { get; set; }

        private Value mHealPercantage;

        private PercentageModifier mModifier;
        private int mHealingAmount;
        private int mRemovedAmount;

        public BuffHealth(Player player, Value healPercentage) : base()
        {
            Agent = player;
            mHealPercantage = healPercentage;
        }

        protected override void OnExecute()
        {
            int prevHealth = Target.Health.CurrentValue;

            mModifier = new PercentageModifier(mHealPercantage.CurrentValue, roundedUp: false);
            Target.Health.AddModifier(this, mModifier);

            mHealingAmount = Target.Health.CurrentValue - prevHealth;
        }

        protected override void OnRetract()
        {
            int prevHealth = Target.Health.CurrentValue;
            Target.Health.RemoveModifier(this, mModifier);
            mRemovedAmount = Target.Health.CurrentValue - prevHealth;
        }

        public override string ProduceExecuteLogString()
        {
            return string.Format("{0} has healed {1} for {2} ({3} %)", Agent.Name, Target.Name, mHealingAmount, mModifier.Percentage);
        }

        public override string ProduceRetractLogString()
        {
            return string.Format("{0} has stopped healing {1}. {1} loses {2} hit points. ({3} %)", Agent.Name, Target.Name, mRemovedAmount, mModifier.Percentage);
        }
    }
}
