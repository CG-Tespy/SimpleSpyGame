using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CGT
{
    public interface INumericResourceEventHandler
    {
        event UnityAction<INumericResourceHandler> MaxValueIncreased;
        event UnityAction<INumericResourceHandler> MaxValueDecreased;

        event UnityAction<INumericResourceHandler> CurrentValueIncreased;
        event UnityAction<INumericResourceHandler> CurrentValueDecreased;

        event UnityAction<INumericResourceHandler> MinValueIncreased;
        event UnityAction<INumericResourceHandler> MinValueDecreased;
    }
}