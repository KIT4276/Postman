using System;

public interface IHealth
{
    float Current { get; set; }
    float Max { get; set; }

    event Action HealthChanged;

    void ChangeHealth(float health);
}
