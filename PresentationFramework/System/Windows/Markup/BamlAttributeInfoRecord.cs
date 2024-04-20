using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x020004B7 RID: 1207
	internal class BamlAttributeInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x06003DC5 RID: 15813 RVA: 0x001FD73B File Offset: 0x001FC73B
		internal BamlAttributeInfoRecord()
		{
			base.Pin();
			this.AttributeUsage = BamlAttributeUsage.Default;
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x001FD750 File Offset: 0x001FC750
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			this.OwnerTypeId = bamlBinaryReader.ReadInt16();
			this.AttributeUsage = (BamlAttributeUsage)bamlBinaryReader.ReadByte();
			this.Name = bamlBinaryReader.ReadString();
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x001FD782 File Offset: 0x001FC782
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(this.OwnerTypeId);
			bamlBinaryWriter.Write((byte)this.AttributeUsage);
			bamlBinaryWriter.Write(this.Name);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x001FD7B8 File Offset: 0x001FC7B8
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)record;
			bamlAttributeInfoRecord._ownerId = this._ownerId;
			bamlAttributeInfoRecord._attributeId = this._attributeId;
			bamlAttributeInfoRecord._name = this._name;
			bamlAttributeInfoRecord._ownerType = this._ownerType;
			bamlAttributeInfoRecord._Event = this._Event;
			bamlAttributeInfoRecord._dp = this._dp;
			bamlAttributeInfoRecord._ei = this._ei;
			bamlAttributeInfoRecord._pi = this._pi;
			bamlAttributeInfoRecord._smi = this._smi;
			bamlAttributeInfoRecord._gmi = this._gmi;
			bamlAttributeInfoRecord._dpOrMiOrPi = this._dpOrMiOrPi;
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x001FD855 File Offset: 0x001FC855
		// (set) Token: 0x06003DCA RID: 15818 RVA: 0x001FD85D File Offset: 0x001FC85D
		internal short OwnerTypeId
		{
			get
			{
				return this._ownerId;
			}
			set
			{
				this._ownerId = value;
			}
		}

		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06003DCC RID: 15820 RVA: 0x001FD86F File Offset: 0x001FC86F
		// (set) Token: 0x06003DCB RID: 15819 RVA: 0x001FD866 File Offset: 0x001FC866
		internal short AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x001FD877 File Offset: 0x001FC877
		// (set) Token: 0x06003DCE RID: 15822 RVA: 0x001FD87F File Offset: 0x001FC87F
		internal string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x001FD888 File Offset: 0x001FC888
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.AttributeInfo;
			}
		}

		// Token: 0x06003DD0 RID: 15824 RVA: 0x001FD88C File Offset: 0x001FC88C
		internal Type GetPropertyType()
		{
			DependencyProperty dp = this.DP;
			Type result;
			if (dp == null)
			{
				MethodInfo attachedPropertySetter = this.AttachedPropertySetter;
				if (attachedPropertySetter == null)
				{
					result = this.PropInfo.PropertyType;
				}
				else
				{
					result = attachedPropertySetter.GetParameters()[1].ParameterType;
				}
			}
			else
			{
				result = dp.PropertyType;
			}
			return result;
		}

		// Token: 0x06003DD1 RID: 15825 RVA: 0x001FD8DC File Offset: 0x001FC8DC
		internal void SetPropertyMember(object propertyMember)
		{
			if (this.PropertyMember == null)
			{
				this.PropertyMember = propertyMember;
				return;
			}
			object[] array = this.PropertyMember as object[];
			if (array == null)
			{
				array = new object[3];
				array[0] = this.PropertyMember;
				array[1] = propertyMember;
				return;
			}
			array[2] = propertyMember;
		}

		// Token: 0x06003DD2 RID: 15826 RVA: 0x001FD924 File Offset: 0x001FC924
		internal object GetPropertyMember(bool onlyPropInfo)
		{
			if (this.PropertyMember == null || this.PropertyMember is MemberInfo || KnownTypes.Types[136].IsAssignableFrom(this.PropertyMember.GetType()))
			{
				if (onlyPropInfo)
				{
					return this.PropInfo;
				}
				return this.PropertyMember;
			}
			else
			{
				object[] array = (object[])this.PropertyMember;
				if (!onlyPropInfo)
				{
					return array[0];
				}
				if (array[0] is PropertyInfo)
				{
					return (PropertyInfo)array[0];
				}
				if (array[1] is PropertyInfo)
				{
					return (PropertyInfo)array[1];
				}
				return array[2] as PropertyInfo;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x001FD9B9 File Offset: 0x001FC9B9
		// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x001FD9C1 File Offset: 0x001FC9C1
		internal object PropertyMember
		{
			get
			{
				return this._dpOrMiOrPi;
			}
			set
			{
				this._dpOrMiOrPi = value;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x001FD9CA File Offset: 0x001FC9CA
		// (set) Token: 0x06003DD6 RID: 15830 RVA: 0x001FD9D2 File Offset: 0x001FC9D2
		internal Type OwnerType
		{
			get
			{
				return this._ownerType;
			}
			set
			{
				this._ownerType = value;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x001FD9DB File Offset: 0x001FC9DB
		// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x001FD9E3 File Offset: 0x001FC9E3
		internal RoutedEvent Event
		{
			get
			{
				return this._Event;
			}
			set
			{
				this._Event = value;
			}
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x001FD9EC File Offset: 0x001FC9EC
		// (set) Token: 0x06003DDA RID: 15834 RVA: 0x001FDA08 File Offset: 0x001FCA08
		internal DependencyProperty DP
		{
			get
			{
				if (this._dp != null)
				{
					return this._dp;
				}
				return this._dpOrMiOrPi as DependencyProperty;
			}
			set
			{
				this._dp = value;
				if (this._dp != null)
				{
					this._name = this._dp.Name;
				}
			}
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x001FDA2A File Offset: 0x001FCA2A
		// (set) Token: 0x06003DDC RID: 15836 RVA: 0x001FDA32 File Offset: 0x001FCA32
		internal MethodInfo AttachedPropertySetter
		{
			get
			{
				return this._smi;
			}
			set
			{
				this._smi = value;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003DDD RID: 15837 RVA: 0x001FDA3B File Offset: 0x001FCA3B
		// (set) Token: 0x06003DDE RID: 15838 RVA: 0x001FDA43 File Offset: 0x001FCA43
		internal MethodInfo AttachedPropertyGetter
		{
			get
			{
				return this._gmi;
			}
			set
			{
				this._gmi = value;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003DDF RID: 15839 RVA: 0x001FDA4C File Offset: 0x001FCA4C
		// (set) Token: 0x06003DE0 RID: 15840 RVA: 0x001FDA54 File Offset: 0x001FCA54
		internal EventInfo EventInfo
		{
			get
			{
				return this._ei;
			}
			set
			{
				this._ei = value;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003DE1 RID: 15841 RVA: 0x001FDA5D File Offset: 0x001FCA5D
		// (set) Token: 0x06003DE2 RID: 15842 RVA: 0x001FDA65 File Offset: 0x001FCA65
		internal PropertyInfo PropInfo
		{
			get
			{
				return this._pi;
			}
			set
			{
				this._pi = value;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x001FDA6E File Offset: 0x001FCA6E
		// (set) Token: 0x06003DE4 RID: 15844 RVA: 0x001FDA86 File Offset: 0x001FCA86
		internal bool IsInternal
		{
			get
			{
				return this._flags[BamlAttributeInfoRecord._isInternalSection] == 1;
			}
			set
			{
				this._flags[BamlAttributeInfoRecord._isInternalSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003DE5 RID: 15845 RVA: 0x001FDA9F File Offset: 0x001FCA9F
		// (set) Token: 0x06003DE6 RID: 15846 RVA: 0x001FDAB2 File Offset: 0x001FCAB2
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return (BamlAttributeUsage)this._flags[BamlAttributeInfoRecord._attributeUsageSection];
			}
			set
			{
				this._flags[BamlAttributeInfoRecord._attributeUsageSection] = (int)value;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003DE7 RID: 15847 RVA: 0x001FDAC5 File Offset: 0x001FCAC5
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlAttributeInfoRecord._attributeUsageSection;
			}
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x001FDACC File Offset: 0x001FCACC
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} owner={1} attr({2}) is '{3}'", new object[]
			{
				this.RecordType,
				BamlRecord.GetTypeName((int)this.OwnerTypeId),
				this.AttributeId,
				this._name
			});
		}

		// Token: 0x04001EEE RID: 7918
		private static BitVector32.Section _isInternalSection = BitVector32.CreateSection(1, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001EEF RID: 7919
		private static BitVector32.Section _attributeUsageSection = BitVector32.CreateSection(3, BamlAttributeInfoRecord._isInternalSection);

		// Token: 0x04001EF0 RID: 7920
		private short _ownerId;

		// Token: 0x04001EF1 RID: 7921
		private short _attributeId;

		// Token: 0x04001EF2 RID: 7922
		private string _name;

		// Token: 0x04001EF3 RID: 7923
		private Type _ownerType;

		// Token: 0x04001EF4 RID: 7924
		private RoutedEvent _Event;

		// Token: 0x04001EF5 RID: 7925
		private DependencyProperty _dp;

		// Token: 0x04001EF6 RID: 7926
		private EventInfo _ei;

		// Token: 0x04001EF7 RID: 7927
		private PropertyInfo _pi;

		// Token: 0x04001EF8 RID: 7928
		private MethodInfo _smi;

		// Token: 0x04001EF9 RID: 7929
		private MethodInfo _gmi;

		// Token: 0x04001EFA RID: 7930
		private object _dpOrMiOrPi;
	}
}
