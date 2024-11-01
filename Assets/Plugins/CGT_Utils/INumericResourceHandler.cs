using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CGT
{
    public interface INumericResourceHandler
    {
        float MaxValue { get; set; }
        float CurrentValue { get; set; }
        float MinValue { get; set; }
    }
}