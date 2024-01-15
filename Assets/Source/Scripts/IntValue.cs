#pragma warning disable 0660, 0661

using System;

namespace Zlodey
{
	public struct IntValue
	{
		public int Value;
		public Action<int> OnValueChange;

		public static IntValue operator +(IntValue a, int b)
		{
			a.Value += b;
			a.OnValueChange?.Invoke(a.Value);

			return a;
		}

		public static IntValue operator -(IntValue a, int b)
		{
			a.Value -= b;
			a.OnValueChange?.Invoke(a.Value);

			return a;
		}

		public static IntValue operator ++(IntValue a)
		{
			a.Value++;
			a.OnValueChange?.Invoke(a.Value);
			return a;
		}

		public static IntValue operator --(IntValue a)
		{
			a.Value--;
			a.OnValueChange?.Invoke(a.Value);
			return a;
		}

		public void Set(int count)
		{
			Value = count;
			OnValueChange?.Invoke(Value);
		}

		public static bool operator ==(IntValue a, int b)
		{
			return a.Value == b;
		}

		public static bool operator !=(IntValue a, int b)
		{
			return a.Value == b;
		}

		public static bool operator >=(IntValue a, int b)
		{
			return a.Value >= b;
		}
		public static bool operator <=(IntValue a, int b)
		{
			return a.Value <= b;
		}
	}
}