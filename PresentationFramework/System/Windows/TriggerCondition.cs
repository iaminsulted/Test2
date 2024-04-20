using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows
{
	// Token: 0x020003A6 RID: 934
	internal struct TriggerCondition
	{
		// Token: 0x06002615 RID: 9749 RVA: 0x0018B1B4 File Offset: 0x0018A1B4
		internal TriggerCondition(DependencyProperty dp, LogicalOp logicalOp, object value, string sourceName)
		{
			this.Property = dp;
			this.Binding = null;
			this.LogicalOp = logicalOp;
			this.Value = value;
			this.SourceName = sourceName;
			this.SourceChildIndex = 0;
			this.BindingValueCache = new BindingValueCache(null, null);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x0018B1EE File Offset: 0x0018A1EE
		internal TriggerCondition(BindingBase binding, LogicalOp logicalOp, object value)
		{
			this = new TriggerCondition(binding, logicalOp, value, "~Self");
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x0018B1FE File Offset: 0x0018A1FE
		internal TriggerCondition(BindingBase binding, LogicalOp logicalOp, object value, string sourceName)
		{
			this.Property = null;
			this.Binding = binding;
			this.LogicalOp = logicalOp;
			this.Value = value;
			this.SourceName = sourceName;
			this.SourceChildIndex = 0;
			this.BindingValueCache = new BindingValueCache(null, null);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x0018B238 File Offset: 0x0018A238
		internal bool Match(object state)
		{
			return this.Match(state, this.Value);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x0018B247 File Offset: 0x0018A247
		private bool Match(object state, object referenceValue)
		{
			if (this.LogicalOp == LogicalOp.Equals)
			{
				return object.Equals(state, referenceValue);
			}
			return !object.Equals(state, referenceValue);
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x0018B264 File Offset: 0x0018A264
		internal bool ConvertAndMatch(object state)
		{
			object obj = this.Value;
			string text = obj as string;
			Type type = (state != null) ? state.GetType() : null;
			if (text != null && type != null && type != typeof(string))
			{
				BindingValueCache bindingValueCache = this.BindingValueCache;
				Type bindingValueType = bindingValueCache.BindingValueType;
				object obj2 = bindingValueCache.ValueAsBindingValueType;
				if (type != bindingValueType)
				{
					obj2 = obj;
					TypeConverter converter = DefaultValueConverter.GetConverter(type);
					if (converter != null && converter.CanConvertFrom(typeof(string)))
					{
						try
						{
							obj2 = converter.ConvertFromString(null, TypeConverterHelper.InvariantEnglishUS, text);
						}
						catch (Exception ex)
						{
							if (CriticalExceptions.IsCriticalApplicationException(ex))
							{
								throw;
							}
						}
						catch
						{
						}
					}
					bindingValueCache = new BindingValueCache(type, obj2);
					this.BindingValueCache = bindingValueCache;
				}
				obj = obj2;
			}
			return this.Match(state, obj);
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x0018B34C File Offset: 0x0018A34C
		internal bool TypeSpecificEquals(TriggerCondition value)
		{
			return this.Property == value.Property && this.Binding == value.Binding && this.LogicalOp == value.LogicalOp && this.Value == value.Value && this.SourceName == value.SourceName;
		}

		// Token: 0x040011D3 RID: 4563
		internal readonly DependencyProperty Property;

		// Token: 0x040011D4 RID: 4564
		internal readonly BindingBase Binding;

		// Token: 0x040011D5 RID: 4565
		internal readonly LogicalOp LogicalOp;

		// Token: 0x040011D6 RID: 4566
		internal readonly object Value;

		// Token: 0x040011D7 RID: 4567
		internal readonly string SourceName;

		// Token: 0x040011D8 RID: 4568
		internal int SourceChildIndex;

		// Token: 0x040011D9 RID: 4569
		internal BindingValueCache BindingValueCache;
	}
}
