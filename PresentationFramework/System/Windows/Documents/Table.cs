using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200068F RID: 1679
	[ContentProperty("RowGroups")]
	public class Table : Block, IAddChild, IAcceptInsertion
	{
		// Token: 0x0600536B RID: 21355 RVA: 0x0025C454 File Offset: 0x0025B454
		static Table()
		{
			Block.MarginProperty.OverrideMetadata(typeof(Table), new FrameworkPropertyMetadata(new Thickness(double.NaN)));
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x0025C4D0 File Offset: 0x0025B4D0
		public Table()
		{
			this.PrivateInitialize();
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x0025C4E0 File Offset: 0x0025B4E0
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TableRowGroup tableRowGroup = value as TableRowGroup;
			if (tableRowGroup != null)
			{
				this.RowGroups.Add(tableRowGroup);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRowGroup)
			}), "value");
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x0025C542 File Offset: 0x0025B542
		public override void BeginInit()
		{
			base.BeginInit();
			this._initializing = true;
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x0025C551 File Offset: 0x0025B551
		public override void EndInit()
		{
			this._initializing = false;
			this.OnStructureChanged();
			base.EndInit();
		}

		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06005371 RID: 21361 RVA: 0x0025C566 File Offset: 0x0025B566
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new Table.TableChildrenCollectionEnumeratorSimple(this);
			}
		}

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06005372 RID: 21362 RVA: 0x0025C56E File Offset: 0x0025B56E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableColumnCollection Columns
		{
			get
			{
				return this._columns;
			}
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x0025C576 File Offset: 0x0025B576
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColumns()
		{
			return this.Columns.Count > 0;
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06005374 RID: 21364 RVA: 0x0025C586 File Offset: 0x0025B586
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableRowGroupCollection RowGroups
		{
			get
			{
				return this._rowGroups;
			}
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06005375 RID: 21365 RVA: 0x0025C58E File Offset: 0x0025B58E
		// (set) Token: 0x06005376 RID: 21366 RVA: 0x0025C5A0 File Offset: 0x0025B5A0
		[TypeConverter(typeof(LengthConverter))]
		public double CellSpacing
		{
			get
			{
				return (double)base.GetValue(Table.CellSpacingProperty);
			}
			set
			{
				base.SetValue(Table.CellSpacingProperty, value);
			}
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x0025C5B3 File Offset: 0x0025B5B3
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TableAutomationPeer(this);
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06005378 RID: 21368 RVA: 0x0025C5BB File Offset: 0x0025B5BB
		internal double InternalCellSpacing
		{
			get
			{
				return Math.Max(this.CellSpacing, 0.0);
			}
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06005379 RID: 21369 RVA: 0x0025C5D1 File Offset: 0x0025B5D1
		// (set) Token: 0x0600537A RID: 21370 RVA: 0x0025C5D9 File Offset: 0x0025B5D9
		int IAcceptInsertion.InsertionIndex
		{
			get
			{
				return this.InsertionIndex;
			}
			set
			{
				this.InsertionIndex = value;
			}
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x0600537B RID: 21371 RVA: 0x0025C5E2 File Offset: 0x0025B5E2
		// (set) Token: 0x0600537C RID: 21372 RVA: 0x0025C5EA File Offset: 0x0025B5EA
		internal int InsertionIndex
		{
			get
			{
				return this._rowGroupInsertionIndex;
			}
			set
			{
				this._rowGroupInsertionIndex = value;
			}
		}

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x0600537D RID: 21373 RVA: 0x0025C5F3 File Offset: 0x0025B5F3
		internal int ColumnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x0025C5FB File Offset: 0x0025B5FB
		internal void EnsureColumnCount(int columnCount)
		{
			if (this._columnCount < columnCount)
			{
				this._columnCount = columnCount;
			}
		}

		// Token: 0x0600537F RID: 21375 RVA: 0x0025C610 File Offset: 0x0025B610
		internal void OnStructureChanged()
		{
			if (!this._initializing)
			{
				if (this.TableStructureChanged != null)
				{
					this.TableStructureChanged(this, EventArgs.Empty);
				}
				this.ValidateStructure();
				TableAutomationPeer tableAutomationPeer = ContentElementAutomationPeer.FromElement(this) as TableAutomationPeer;
				if (tableAutomationPeer != null)
				{
					tableAutomationPeer.OnStructureInvalidated();
				}
			}
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x0025C65C File Offset: 0x0025B65C
		internal void ValidateStructure()
		{
			if (!this._initializing)
			{
				this._columnCount = 0;
				for (int i = 0; i < this._rowGroups.Count; i++)
				{
					this._rowGroups[i].ValidateStructure();
				}
				this._version++;
			}
		}

		// Token: 0x06005381 RID: 21377 RVA: 0x0025C6AD File Offset: 0x0025B6AD
		internal void InvalidateColumns()
		{
			base.NotifyTypographicPropertyChanged(true, true, null);
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x0025C6B8 File Offset: 0x0025B6B8
		internal bool IsFirstNonEmptyRowGroup(int rowGroupIndex)
		{
			for (rowGroupIndex--; rowGroupIndex >= 0; rowGroupIndex--)
			{
				if (this.RowGroups[rowGroupIndex].Rows.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x0025C6E6 File Offset: 0x0025B6E6
		internal bool IsLastNonEmptyRowGroup(int rowGroupIndex)
		{
			for (rowGroupIndex++; rowGroupIndex < this.RowGroups.Count; rowGroupIndex++)
			{
				if (this.RowGroups[rowGroupIndex].Rows.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06005384 RID: 21380 RVA: 0x0025C720 File Offset: 0x0025B720
		// (remove) Token: 0x06005385 RID: 21381 RVA: 0x0025C758 File Offset: 0x0025B758
		internal event EventHandler TableStructureChanged;

		// Token: 0x06005386 RID: 21382 RVA: 0x0025C78D File Offset: 0x0025B78D
		private void PrivateInitialize()
		{
			this._columns = new TableColumnCollection(this);
			this._rowGroups = new TableRowGroupCollection(this);
			this._rowGroupInsertionIndex = -1;
		}

		// Token: 0x06005387 RID: 21383 RVA: 0x0025C7B0 File Offset: 0x0025B7B0
		private static bool IsValidCellSpacing(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && num >= 0.0 && num <= num2;
		}

		// Token: 0x04002EDE RID: 11998
		private TableColumnCollection _columns;

		// Token: 0x04002EDF RID: 11999
		private TableRowGroupCollection _rowGroups;

		// Token: 0x04002EE0 RID: 12000
		private int _rowGroupInsertionIndex;

		// Token: 0x04002EE1 RID: 12001
		private const double c_defaultCellSpacing = 2.0;

		// Token: 0x04002EE2 RID: 12002
		private int _columnCount;

		// Token: 0x04002EE3 RID: 12003
		private int _version;

		// Token: 0x04002EE4 RID: 12004
		private bool _initializing;

		// Token: 0x04002EE5 RID: 12005
		public static readonly DependencyProperty CellSpacingProperty = DependencyProperty.Register("CellSpacing", typeof(double), typeof(Table), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Table.IsValidCellSpacing));

		// Token: 0x02000B5C RID: 2908
		private class TableChildrenCollectionEnumeratorSimple : IEnumerator, ICloneable
		{
			// Token: 0x06008DBA RID: 36282 RVA: 0x0033ED00 File Offset: 0x0033DD00
			internal TableChildrenCollectionEnumeratorSimple(Table table)
			{
				this._table = table;
				this._version = this._table._version;
				this._columns = ((IEnumerable)this._table._columns).GetEnumerator();
				this._rowGroups = ((IEnumerable)this._table._rowGroups).GetEnumerator();
			}

			// Token: 0x06008DBB RID: 36283 RVA: 0x0033ED57 File Offset: 0x0033DD57
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x06008DBC RID: 36284 RVA: 0x0033ED60 File Offset: 0x0033DD60
			public bool MoveNext()
			{
				if (this._version != this._table._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				if (this._currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.Columns && this._currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.RowGroups)
				{
					this._currentChildType++;
				}
				object obj = null;
				while (this._currentChildType < Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.AfterLast)
				{
					Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes currentChildType = this._currentChildType;
					if (currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.Columns)
					{
						if (currentChildType == Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.RowGroups)
						{
							if (this._rowGroups.MoveNext())
							{
								obj = this._rowGroups.Current;
							}
						}
					}
					else if (this._columns.MoveNext())
					{
						obj = this._columns.Current;
					}
					if (obj != null)
					{
						this._currentChild = obj;
						break;
					}
					this._currentChildType++;
				}
				return this._currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.AfterLast;
			}

			// Token: 0x17001EF9 RID: 7929
			// (get) Token: 0x06008DBD RID: 36285 RVA: 0x0033EE28 File Offset: 0x0033DE28
			public object Current
			{
				get
				{
					if (this._currentChildType == Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.BeforeFirst)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					if (this._currentChildType == Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.AfterLast)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
					}
					return this._currentChild;
				}
			}

			// Token: 0x06008DBE RID: 36286 RVA: 0x0033EE64 File Offset: 0x0033DE64
			public void Reset()
			{
				if (this._version != this._table._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this._columns.Reset();
				this._rowGroups.Reset();
				this._currentChildType = Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.BeforeFirst;
				this._currentChild = null;
			}

			// Token: 0x040048A8 RID: 18600
			private Table _table;

			// Token: 0x040048A9 RID: 18601
			private int _version;

			// Token: 0x040048AA RID: 18602
			private IEnumerator _columns;

			// Token: 0x040048AB RID: 18603
			private IEnumerator _rowGroups;

			// Token: 0x040048AC RID: 18604
			private Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes _currentChildType;

			// Token: 0x040048AD RID: 18605
			private object _currentChild;

			// Token: 0x02000C90 RID: 3216
			private enum ChildrenTypes
			{
				// Token: 0x04004FC9 RID: 20425
				BeforeFirst,
				// Token: 0x04004FCA RID: 20426
				Columns,
				// Token: 0x04004FCB RID: 20427
				RowGroups,
				// Token: 0x04004FCC RID: 20428
				AfterLast
			}
		}
	}
}
