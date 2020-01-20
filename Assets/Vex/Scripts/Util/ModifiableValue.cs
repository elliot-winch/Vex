using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vex
{
    public abstract class Modifier
    {
        private int mPriority = 1;

        public Modifier(int priority)
        {
            mPriority = priority;
        }

        public abstract int ApplyModifier(int value);
    }

    public class LumpSumModifier : Modifier
    {
        public int Amount { get; private set; }
    
        public LumpSumModifier(int modifier, int priority = 0) : base(priority)
        {
            Amount = modifier;
        }

        public override int ApplyModifier(int value)
        {
            return Amount + value;
        }
    }

    public class PercentageModifier : Modifier
    {
        public float Percentage { get; private set; }
        private bool mRoundedUp;

        public PercentageModifier(float modifier, bool roundedUp, int priority = 1) : base(priority)
        {
            mRoundedUp = roundedUp;
            Percentage = modifier;
        }

        public override int ApplyModifier(int value)
        {
            float newFloatVal = Percentage * value;
            if (mRoundedUp)
            {
                return Mathf.CeilToInt(newFloatVal);
            }
            else
            {
                return Mathf.FloorToInt(newFloatVal);
            }
        }
    }

    [Serializable]
    public class Value
    {
        private List<GameAction> mActionStatus = new List<GameAction>();
        private List<Modifier> mModifiers = new List<Modifier>();
        private Action<int> mOnValueChanged;

        [SerializeField]
        private int mOriginalValue;
        [SerializeField]
        private int mCurrentValue;

        public int OriginalValue => mOriginalValue;
        public int CurrentValue => mCurrentValue;

        public Value(int baseValue)
        {
            mOriginalValue = baseValue;
            mCurrentValue = OriginalValue;
        }

        public void AddModifier(GameAction action, Modifier modifier)
        {
            mActionStatus.Add(action);
            mModifiers.Add(modifier);

            CalculateCurrent();
            mOnValueChanged?.Invoke(CurrentValue);
        }

        public void RemoveModifier(GameAction action, Modifier modifier)
        {
            mActionStatus.Remove(action);
            mModifiers.Remove(modifier);

            CalculateCurrent();
            mOnValueChanged?.Invoke(CurrentValue);
        }

        public void Subscribe(Action<int> subscription)
        {
            if (subscription != null)
            {
                subscription(CurrentValue);
                mOnValueChanged += subscription;
            }
        }

        public void Unsubscribe(Action<int> subscription)
        {
            mOnValueChanged -= subscription;
        }

        private void CalculateCurrent()
        {
            int newValue = OriginalValue;

            foreach (var modifier in mModifiers)
            {
                newValue = modifier.ApplyModifier(newValue);
            }

            mCurrentValue = newValue;
        }

        public override string ToString()
        {
            return string.Format("Current: {0}; Base: {1}; ModCount: {2}", CurrentValue, OriginalValue, mModifiers.Count);
        }
    }
}