using UnityEngine;

public partial class Player : OccupyingGamePiece
{
    [Header("Player Stats")]
    public int actionsPerTurn = 2;

    public int baseDefense = 10;
    public int baseMaxHealth = 10;

    public int CurrentHealth { get; private set; }

    public ModifiableValue Defense { get; private set; }
    public ModifiableValue MaxHealth { get; private set; }

    void InitStats()
    {
        Defense = new ModifiableValue(baseDefense);
        MaxHealth = new ModifiableValue(baseMaxHealth);

        CurrentHealth = MaxHealth.CurrentValue;
    }

    public virtual void AdjustHealth(int amount)
    {
        if(CurrentHealth + amount < MaxHealth.CurrentValue)
        {
            CurrentHealth += amount;
        }
    }
}
