using UnityEngine;
using UnityEngine.Events;

namespace CGT
{
    public class NumericResource : MonoBehaviour, INumericResourceHandler, INumericResourceEventHandler, IHasNotes
    {
        [TextArea(3, 6)]
        [SerializeField] protected string _notes = string.Empty;
        [field: SerializeField] public float _maxValue = 10;
        [field: SerializeField] public float _currentValue = 10;
        [field: SerializeField] public float _minValue = 0;
        [field: SerializeField] public bool _resetToMaxOnAwake = true;

        public virtual string Notes { get { return _notes; } }
        public virtual float MaxValue
        {
            get { return _maxValue; }
            set
            {
                float prevVal = _maxValue;
                _maxValue = value;

                if (_maxValue > prevVal)
                {
                    MaxValueIncreased(this);
                }
                else if (_maxValue < prevVal)
                {
                    MaxValueDecreased(this);
                }

                _currentValue = WithinCurrentValueBounds(_currentValue);
            }
        }

        public event UnityAction<INumericResourceHandler> MaxValueIncreased = delegate { },
            MaxValueDecreased = delegate { };

        protected virtual float WithinCurrentValueBounds(float val)
        {
            float result = Mathf.Clamp(val, _minValue, _maxValue);
            return result;
        }

        public virtual float CurrentValue
        {
            get { return _currentValue; }
            set
            {
                float prevVal = _currentValue;
                _currentValue = WithinCurrentValueBounds(value);

                if (_currentValue > prevVal)
                {
                    CurrentValueIncreased(this);
                }
                else if (_currentValue < prevVal)
                {
                    CurrentValueDecreased(this);
                }

            }
        }

        public event UnityAction<INumericResourceHandler> CurrentValueIncreased = delegate { },
            CurrentValueDecreased = delegate { };

        public virtual float MinValue
        {
            get { return _minValue; }
            set
            {
                float prevVal = _minValue;
                _minValue = value;

                if (_minValue > prevVal)
                {
                    MinValueIncreased(this);
                }
                else if (_minValue < prevVal)
                {
                    MinValueDecreased(this);
                }

                _currentValue = WithinCurrentValueBounds(_currentValue);
            }
        }

        public event UnityAction<INumericResourceHandler> MinValueIncreased = delegate { },
            MinValueDecreased = delegate { };

        public virtual void Awake()
        {
            if (_resetToMaxOnAwake)
            {
                _currentValue = _maxValue;
            }
        }

    }
}