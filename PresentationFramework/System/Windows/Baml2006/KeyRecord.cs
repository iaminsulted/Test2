using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x02000402 RID: 1026
	[DebuggerDisplay("{DebuggerString}")]
	internal class KeyRecord
	{
		// Token: 0x06002C40 RID: 11328 RVA: 0x001A65B8 File Offset: 0x001A55B8
		public KeyRecord(bool shared, bool sharedSet, int valuePosition, Type keyType) : this(shared, sharedSet, valuePosition)
		{
			this._data = keyType;
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x001A65B8 File Offset: 0x001A55B8
		public KeyRecord(bool shared, bool sharedSet, int valuePosition, string keyString) : this(shared, sharedSet, valuePosition)
		{
			this._data = keyString;
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x001A65CB File Offset: 0x001A55CB
		public KeyRecord(bool shared, bool sharedSet, int valuePosition, XamlSchemaContext context) : this(shared, sharedSet, valuePosition)
		{
			this._data = new XamlNodeList(context, 8);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x001A65E4 File Offset: 0x001A55E4
		private KeyRecord(bool shared, bool sharedSet, int valuePosition)
		{
			this._shared = shared;
			this._sharedSet = sharedSet;
			this.ValuePosition = (long)valuePosition;
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x001A6602 File Offset: 0x001A5602
		public bool Shared
		{
			get
			{
				return this._shared;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x001A660A File Offset: 0x001A560A
		public bool SharedSet
		{
			get
			{
				return this._sharedSet;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06002C46 RID: 11334 RVA: 0x001A6612 File Offset: 0x001A5612
		// (set) Token: 0x06002C47 RID: 11335 RVA: 0x001A661A File Offset: 0x001A561A
		public long ValuePosition { get; set; }

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06002C48 RID: 11336 RVA: 0x001A6623 File Offset: 0x001A5623
		// (set) Token: 0x06002C49 RID: 11337 RVA: 0x001A662B File Offset: 0x001A562B
		public int ValueSize { get; set; }

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06002C4A RID: 11338 RVA: 0x001A6634 File Offset: 0x001A5634
		// (set) Token: 0x06002C4B RID: 11339 RVA: 0x001A663C File Offset: 0x001A563C
		public byte Flags { get; set; }

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06002C4C RID: 11340 RVA: 0x001A6645 File Offset: 0x001A5645
		public List<object> StaticResources
		{
			get
			{
				if (this._resources == null)
				{
					this._resources = new List<object>();
				}
				return this._resources;
			}
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06002C4D RID: 11341 RVA: 0x001A6660 File Offset: 0x001A5660
		public bool HasStaticResources
		{
			get
			{
				return this._resources != null && this._resources.Count > 0;
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002C4E RID: 11342 RVA: 0x001A667A File Offset: 0x001A567A
		public StaticResource LastStaticResource
		{
			get
			{
				return this.StaticResources[this.StaticResources.Count - 1] as StaticResource;
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06002C4F RID: 11343 RVA: 0x001A6699 File Offset: 0x001A5699
		public string KeyString
		{
			get
			{
				return this._data as string;
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002C50 RID: 11344 RVA: 0x001A66A6 File Offset: 0x001A56A6
		public Type KeyType
		{
			get
			{
				return this._data as Type;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x001A66B3 File Offset: 0x001A56B3
		public XamlNodeList KeyNodeList
		{
			get
			{
				return this._data as XamlNodeList;
			}
		}

		// Token: 0x04001B1C RID: 6940
		private List<object> _resources;

		// Token: 0x04001B1D RID: 6941
		private object _data;

		// Token: 0x04001B1E RID: 6942
		private bool _shared;

		// Token: 0x04001B1F RID: 6943
		private bool _sharedSet;
	}
}
