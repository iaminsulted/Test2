using System;

namespace System.Windows
{
	// Token: 0x020003A5 RID: 933
	internal struct ChildValueLookup
	{
		// Token: 0x06002611 RID: 9745 RVA: 0x0018B0BC File Offset: 0x0018A0BC
		public override bool Equals(object value)
		{
			if (value is ChildValueLookup)
			{
				ChildValueLookup childValueLookup = (ChildValueLookup)value;
				if (this.LookupType == childValueLookup.LookupType && this.Property == childValueLookup.Property && this.Value == childValueLookup.Value)
				{
					if (this.Conditions == null && childValueLookup.Conditions == null)
					{
						return true;
					}
					if (this.Conditions == null || childValueLookup.Conditions == null)
					{
						return false;
					}
					if (this.Conditions.Length == childValueLookup.Conditions.Length)
					{
						for (int i = 0; i < this.Conditions.Length; i++)
						{
							if (!this.Conditions[i].TypeSpecificEquals(childValueLookup.Conditions[i]))
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x0018B175 File Offset: 0x0018A175
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x0018B187 File Offset: 0x0018A187
		public static bool operator ==(ChildValueLookup value1, ChildValueLookup value2)
		{
			return value1.Equals(value2);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x0018B19C File Offset: 0x0018A19C
		public static bool operator !=(ChildValueLookup value1, ChildValueLookup value2)
		{
			return !value1.Equals(value2);
		}

		// Token: 0x040011CF RID: 4559
		internal ValueLookupType LookupType;

		// Token: 0x040011D0 RID: 4560
		internal TriggerCondition[] Conditions;

		// Token: 0x040011D1 RID: 4561
		internal DependencyProperty Property;

		// Token: 0x040011D2 RID: 4562
		internal object Value;
	}
}
