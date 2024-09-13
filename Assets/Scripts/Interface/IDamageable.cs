using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public bool HasTakenDamage { get; set; }

    public void Damage(float value);
}
