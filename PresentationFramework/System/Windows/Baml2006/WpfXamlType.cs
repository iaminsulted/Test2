using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x0200041D RID: 1053
	internal class WpfXamlType : XamlType
	{
		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06003289 RID: 12937 RVA: 0x001D1F26 File Offset: 0x001D0F26
		// (set) Token: 0x0600328A RID: 12938 RVA: 0x001D1F34 File Offset: 0x001D0F34
		private bool IsBamlScenario
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 1);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 1, value);
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x0600328B RID: 12939 RVA: 0x001D1F43 File Offset: 0x001D0F43
		// (set) Token: 0x0600328C RID: 12940 RVA: 0x001D1F51 File Offset: 0x001D0F51
		private bool UseV3Rules
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x0600328D RID: 12941 RVA: 0x001D1F60 File Offset: 0x001D0F60
		protected ConcurrentDictionary<string, XamlMember> Members
		{
			get
			{
				if (this._members == null)
				{
					this._members = new ConcurrentDictionary<string, XamlMember>(1, 11);
				}
				return this._members;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x0600328E RID: 12942 RVA: 0x001D1F7E File Offset: 0x001D0F7E
		protected ConcurrentDictionary<string, XamlMember> AttachableMembers
		{
			get
			{
				if (this._attachableMembers == null)
				{
					this._attachableMembers = new ConcurrentDictionary<string, XamlMember>(1, 11);
				}
				return this._attachableMembers;
			}
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x001D1F9C File Offset: 0x001D0F9C
		public WpfXamlType(Type type, XamlSchemaContext schema, bool isBamlScenario, bool useV3Rules) : base(type, schema)
		{
			this.IsBamlScenario = isBamlScenario;
			this.UseV3Rules = useV3Rules;
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x001D1FB8 File Offset: 0x001D0FB8
		protected override XamlMember LookupContentProperty()
		{
			XamlMember xamlMember = base.LookupContentProperty();
			WpfXamlMember wpfXamlMember = xamlMember as WpfXamlMember;
			if (wpfXamlMember != null)
			{
				xamlMember = wpfXamlMember.AsContentProperty;
			}
			return xamlMember;
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x001D1FE4 File Offset: 0x001D0FE4
		protected override bool LookupIsNameScope()
		{
			if (base.UnderlyingType == typeof(ResourceDictionary))
			{
				return false;
			}
			if (typeof(ResourceDictionary).IsAssignableFrom(base.UnderlyingType))
			{
				foreach (MethodInfo methodInfo in base.UnderlyingType.GetInterfaceMap(typeof(INameScope)).TargetMethods)
				{
					if (methodInfo.Name.Contains("RegisterName"))
					{
						return methodInfo.DeclaringType != typeof(ResourceDictionary);
					}
				}
				return false;
			}
			return base.LookupIsNameScope();
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x001D2080 File Offset: 0x001D1080
		private XamlMember FindMember(string name, bool isAttached, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = this.FindKnownMember(name, isAttached);
			if (xamlMember != null)
			{
				return xamlMember;
			}
			xamlMember = this.FindDependencyPropertyBackedProperty(name, isAttached, skipReadOnlyCheck);
			if (xamlMember != null)
			{
				return xamlMember;
			}
			xamlMember = this.FindRoutedEventBackedProperty(name, isAttached, skipReadOnlyCheck);
			if (xamlMember != null)
			{
				return xamlMember;
			}
			if (isAttached)
			{
				xamlMember = base.LookupAttachableMember(name);
			}
			else
			{
				xamlMember = base.LookupMember(name, skipReadOnlyCheck);
			}
			WpfKnownType wpfXamlType;
			if (xamlMember != null && (wpfXamlType = (xamlMember.DeclaringType as WpfKnownType)) != null)
			{
				XamlMember xamlMember2 = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttached);
				if (xamlMember2 != null)
				{
					return xamlMember2;
				}
			}
			return xamlMember;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x001D2114 File Offset: 0x001D1114
		protected override XamlMember LookupMember(string name, bool skipReadOnlyCheck)
		{
			return this.FindMember(name, false, skipReadOnlyCheck);
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x001D211F File Offset: 0x001D111F
		protected override XamlMember LookupAttachableMember(string name)
		{
			return this.FindMember(name, true, false);
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x001D212C File Offset: 0x001D112C
		protected override IEnumerable<XamlMember> LookupAllMembers()
		{
			List<XamlMember> list = new List<XamlMember>();
			foreach (XamlMember xamlMember in base.LookupAllMembers())
			{
				if (!(xamlMember is WpfXamlMember))
				{
					xamlMember = base.GetMember(xamlMember.Name);
				}
				list.Add(xamlMember);
			}
			return list;
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x001D2198 File Offset: 0x001D1198
		private XamlMember FindKnownMember(string name, bool isAttachable)
		{
			XamlType xamlType = this;
			if (this is WpfKnownType)
			{
				XamlMember xamlMember;
				for (;;)
				{
					xamlMember = WpfXamlType.FindKnownMember(xamlType as WpfXamlType, name, isAttachable);
					if (xamlMember != null)
					{
						break;
					}
					xamlType = xamlType.BaseType;
					if (!(xamlType != null))
					{
						goto IL_33;
					}
				}
				return xamlMember;
			}
			IL_33:
			return null;
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x001D21DC File Offset: 0x001D11DC
		private XamlMember FindRoutedEventBackedProperty(string name, bool isAttachable, bool skipReadOnlyCheck)
		{
			RoutedEvent routedEventFromName = EventManager.GetRoutedEventFromName(name, base.UnderlyingType);
			XamlMember xamlMember = null;
			if (routedEventFromName == null)
			{
				return xamlMember;
			}
			WpfXamlType wpfXamlType;
			if (this.IsBamlScenario)
			{
				wpfXamlType = (System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(routedEventFromName.OwnerType) as WpfXamlType);
			}
			else
			{
				wpfXamlType = (System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(routedEventFromName.OwnerType) as WpfXamlType);
			}
			if (wpfXamlType != null)
			{
				xamlMember = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttachable);
			}
			if (this.IsBamlScenario)
			{
				xamlMember = new WpfXamlMember(routedEventFromName, isAttachable);
			}
			else if (isAttachable)
			{
				xamlMember = this.GetAttachedRoutedEvent(name, routedEventFromName);
				if (xamlMember == null)
				{
					xamlMember = this.GetRoutedEvent(name, routedEventFromName, skipReadOnlyCheck);
				}
				if (xamlMember == null)
				{
					xamlMember = new WpfXamlMember(routedEventFromName, true);
				}
			}
			else
			{
				xamlMember = this.GetRoutedEvent(name, routedEventFromName, skipReadOnlyCheck);
				if (xamlMember == null)
				{
					xamlMember = this.GetAttachedRoutedEvent(name, routedEventFromName);
				}
				if (xamlMember == null)
				{
					xamlMember = new WpfXamlMember(routedEventFromName, false);
				}
			}
			if (this.Members.TryAdd(name, xamlMember))
			{
				return xamlMember;
			}
			return this.Members[name];
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x001D22D8 File Offset: 0x001D12D8
		private XamlMember FindDependencyPropertyBackedProperty(string name, bool isAttachable, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = null;
			DependencyProperty dependencyProperty;
			if ((dependencyProperty = DependencyProperty.FromName(name, base.UnderlyingType)) != null)
			{
				WpfXamlType wpfXamlType;
				if (this.IsBamlScenario)
				{
					wpfXamlType = (System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(dependencyProperty.OwnerType) as WpfXamlType);
				}
				else
				{
					wpfXamlType = (System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(dependencyProperty.OwnerType) as WpfXamlType);
				}
				if (wpfXamlType != null)
				{
					xamlMember = WpfXamlType.FindKnownMember(wpfXamlType, name, isAttachable);
				}
				if (xamlMember == null)
				{
					if (this.IsBamlScenario)
					{
						xamlMember = new WpfXamlMember(dependencyProperty, isAttachable);
					}
					else if (isAttachable)
					{
						xamlMember = this.GetAttachedDependencyProperty(name, dependencyProperty);
						if (xamlMember == null)
						{
							return null;
						}
					}
					else
					{
						xamlMember = this.GetRegularDependencyProperty(name, dependencyProperty, skipReadOnlyCheck);
						if (xamlMember == null)
						{
							xamlMember = this.GetAttachedDependencyProperty(name, dependencyProperty);
						}
						if (xamlMember == null)
						{
							xamlMember = new WpfXamlMember(dependencyProperty, false);
						}
					}
					return this.CacheAndReturnXamlMember(xamlMember);
				}
			}
			return xamlMember;
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x001D23AC File Offset: 0x001D13AC
		private XamlMember CacheAndReturnXamlMember(XamlMember xamlMember)
		{
			if (!xamlMember.IsAttachable || this.IsBamlScenario)
			{
				if (this.Members.TryAdd(xamlMember.Name, xamlMember))
				{
					return xamlMember;
				}
				return this.Members[xamlMember.Name];
			}
			else
			{
				if (this.AttachableMembers.TryAdd(xamlMember.Name, xamlMember))
				{
					return xamlMember;
				}
				return this.AttachableMembers[xamlMember.Name];
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x001D2418 File Offset: 0x001D1418
		private XamlMember GetAttachedRoutedEvent(string name, RoutedEvent re)
		{
			XamlMember xamlMember = base.LookupAttachableMember(name);
			if (xamlMember != null)
			{
				return new WpfXamlMember(re, (MethodInfo)xamlMember.UnderlyingMember, base.SchemaContext, this.UseV3Rules);
			}
			return null;
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x001D2458 File Offset: 0x001D1458
		private XamlMember GetRoutedEvent(string name, RoutedEvent re, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = base.LookupMember(name, skipReadOnlyCheck);
			if (xamlMember != null)
			{
				return new WpfXamlMember(re, (EventInfo)xamlMember.UnderlyingMember, base.SchemaContext, this.UseV3Rules);
			}
			return null;
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x001D2498 File Offset: 0x001D1498
		private XamlMember GetAttachedDependencyProperty(string name, DependencyProperty property)
		{
			XamlMember xamlMember = base.LookupAttachableMember(name);
			if (xamlMember != null)
			{
				return new WpfXamlMember(property, xamlMember.Invoker.UnderlyingGetter, xamlMember.Invoker.UnderlyingSetter, base.SchemaContext, this.UseV3Rules);
			}
			return null;
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x001D24E0 File Offset: 0x001D14E0
		private XamlMember GetRegularDependencyProperty(string name, DependencyProperty property, bool skipReadOnlyCheck)
		{
			XamlMember xamlMember = base.LookupMember(name, skipReadOnlyCheck);
			if (!(xamlMember != null))
			{
				return null;
			}
			PropertyInfo propertyInfo = xamlMember.UnderlyingMember as PropertyInfo;
			if (propertyInfo != null)
			{
				return new WpfXamlMember(property, propertyInfo, base.SchemaContext, this.UseV3Rules);
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x001D2530 File Offset: 0x001D1530
		private static XamlMember FindKnownMember(WpfXamlType wpfXamlType, string name, bool isAttachable)
		{
			XamlMember xamlMember = null;
			if (!isAttachable || wpfXamlType.IsBamlScenario)
			{
				if (wpfXamlType._members != null && wpfXamlType.Members.TryGetValue(name, out xamlMember))
				{
					return xamlMember;
				}
			}
			else if (wpfXamlType._attachableMembers != null && wpfXamlType.AttachableMembers.TryGetValue(name, out xamlMember))
			{
				return xamlMember;
			}
			WpfKnownType wpfKnownType = wpfXamlType as WpfKnownType;
			if (wpfKnownType != null)
			{
				if (!isAttachable || wpfXamlType.IsBamlScenario)
				{
					xamlMember = System.Windows.Markup.XamlReader.BamlSharedSchemaContext.CreateKnownMember(wpfXamlType.Name, name);
				}
				if (isAttachable || (xamlMember == null && wpfXamlType.IsBamlScenario))
				{
					xamlMember = System.Windows.Markup.XamlReader.BamlSharedSchemaContext.CreateKnownAttachableMember(wpfXamlType.Name, name);
				}
				if (xamlMember != null)
				{
					return wpfKnownType.CacheAndReturnXamlMember(xamlMember);
				}
			}
			return null;
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x001D25E4 File Offset: 0x001D15E4
		protected override XamlCollectionKind LookupCollectionKind()
		{
			if (!this.UseV3Rules)
			{
				return base.LookupCollectionKind();
			}
			if (base.UnderlyingType.IsArray)
			{
				return XamlCollectionKind.Array;
			}
			if (typeof(IDictionary).IsAssignableFrom(base.UnderlyingType))
			{
				return XamlCollectionKind.Dictionary;
			}
			if (typeof(IList).IsAssignableFrom(base.UnderlyingType))
			{
				return XamlCollectionKind.Collection;
			}
			if (typeof(DocumentReferenceCollection).IsAssignableFrom(base.UnderlyingType) || typeof(PageContentCollection).IsAssignableFrom(base.UnderlyingType))
			{
				return XamlCollectionKind.Collection;
			}
			if (typeof(ICollection<XmlNamespaceMapping>).IsAssignableFrom(base.UnderlyingType) && this.IsXmlNamespaceMappingCollection)
			{
				return XamlCollectionKind.Collection;
			}
			return XamlCollectionKind.None;
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060032A0 RID: 12960 RVA: 0x001D2696 File Offset: 0x001D1696
		private bool IsXmlNamespaceMappingCollection
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				return typeof(XmlNamespaceMappingCollection).IsAssignableFrom(base.UnderlyingType);
			}
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x001D26AD File Offset: 0x001D16AD
		internal XamlMember FindBaseXamlMember(string name, bool isAttachable)
		{
			if (isAttachable)
			{
				return base.LookupAttachableMember(name);
			}
			return base.LookupMember(name, true);
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x001D26C2 File Offset: 0x001D16C2
		internal static bool GetFlag(ref byte bitField, byte typeBit)
		{
			return (bitField & typeBit) > 0;
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x001D26CB File Offset: 0x001D16CB
		internal static void SetFlag(ref byte bitField, byte typeBit, bool value)
		{
			if (value)
			{
				bitField |= typeBit;
				return;
			}
			bitField &= ~typeBit;
		}

		// Token: 0x04001BF5 RID: 7157
		private const int ConcurrencyLevel = 1;

		// Token: 0x04001BF6 RID: 7158
		private const int Capacity = 11;

		// Token: 0x04001BF7 RID: 7159
		private ConcurrentDictionary<string, XamlMember> _attachableMembers;

		// Token: 0x04001BF8 RID: 7160
		private ConcurrentDictionary<string, XamlMember> _members;

		// Token: 0x04001BF9 RID: 7161
		protected byte _bitField;

		// Token: 0x02000AB7 RID: 2743
		[Flags]
		private enum BoolTypeBits
		{
			// Token: 0x04004633 RID: 17971
			BamlScenerio = 1,
			// Token: 0x04004634 RID: 17972
			V3Rules = 2
		}
	}
}
