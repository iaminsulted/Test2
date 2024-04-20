using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Data
{
	// Token: 0x02000469 RID: 1129
	[MarkupExtensionReturnType(typeof(RelativeSource))]
	public class RelativeSource : MarkupExtension, ISupportInitialize
	{
		// Token: 0x06003A13 RID: 14867 RVA: 0x001EF91D File Offset: 0x001EE91D
		public RelativeSource()
		{
			this._mode = RelativeSourceMode.FindAncestor;
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x001EF933 File Offset: 0x001EE933
		public RelativeSource(RelativeSourceMode mode)
		{
			this.InitializeMode(mode);
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x001EF949 File Offset: 0x001EE949
		public RelativeSource(RelativeSourceMode mode, Type ancestorType, int ancestorLevel)
		{
			this.InitializeMode(mode);
			this.AncestorType = ancestorType;
			this.AncestorLevel = ancestorLevel;
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void ISupportInitialize.BeginInit()
		{
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x001EF970 File Offset: 0x001EE970
		void ISupportInitialize.EndInit()
		{
			if (this.IsUninitialized)
			{
				throw new InvalidOperationException(SR.Get("RelativeSourceNeedsMode"));
			}
			if (this._mode == RelativeSourceMode.FindAncestor && this.AncestorType == null)
			{
				throw new InvalidOperationException(SR.Get("RelativeSourceNeedsAncestorType"));
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x001EF9BC File Offset: 0x001EE9BC
		public static RelativeSource PreviousData
		{
			get
			{
				if (RelativeSource.s_previousData == null)
				{
					RelativeSource.s_previousData = new RelativeSource(RelativeSourceMode.PreviousData);
				}
				return RelativeSource.s_previousData;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06003A19 RID: 14873 RVA: 0x001EF9D5 File Offset: 0x001EE9D5
		public static RelativeSource TemplatedParent
		{
			get
			{
				if (RelativeSource.s_templatedParent == null)
				{
					RelativeSource.s_templatedParent = new RelativeSource(RelativeSourceMode.TemplatedParent);
				}
				return RelativeSource.s_templatedParent;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x001EF9EE File Offset: 0x001EE9EE
		public static RelativeSource Self
		{
			get
			{
				if (RelativeSource.s_self == null)
				{
					RelativeSource.s_self = new RelativeSource(RelativeSourceMode.Self);
				}
				return RelativeSource.s_self;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x001EFA07 File Offset: 0x001EEA07
		// (set) Token: 0x06003A1C RID: 14876 RVA: 0x001EFA0F File Offset: 0x001EEA0F
		[ConstructorArgument("mode")]
		public RelativeSourceMode Mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (this.IsUninitialized)
				{
					this.InitializeMode(value);
					return;
				}
				if (value != this._mode)
				{
					throw new InvalidOperationException(SR.Get("RelativeSourceModeIsImmutable"));
				}
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x001EFA3A File Offset: 0x001EEA3A
		// (set) Token: 0x06003A1E RID: 14878 RVA: 0x001EFA42 File Offset: 0x001EEA42
		public Type AncestorType
		{
			get
			{
				return this._ancestorType;
			}
			set
			{
				if (this.IsUninitialized)
				{
					this.AncestorLevel = 1;
				}
				if (this._mode != RelativeSourceMode.FindAncestor)
				{
					if (value != null)
					{
						throw new InvalidOperationException(SR.Get("RelativeSourceNotInFindAncestorMode"));
					}
				}
				else
				{
					this._ancestorType = value;
				}
			}
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x001EFA7C File Offset: 0x001EEA7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeAncestorType()
		{
			return this._mode == RelativeSourceMode.FindAncestor;
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x001EFA87 File Offset: 0x001EEA87
		// (set) Token: 0x06003A21 RID: 14881 RVA: 0x001EFA8F File Offset: 0x001EEA8F
		public int AncestorLevel
		{
			get
			{
				return this._ancestorLevel;
			}
			set
			{
				if (this._mode != RelativeSourceMode.FindAncestor)
				{
					if (value != 0)
					{
						throw new InvalidOperationException(SR.Get("RelativeSourceNotInFindAncestorMode"));
					}
				}
				else
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException(SR.Get("RelativeSourceInvalidAncestorLevel"));
					}
					this._ancestorLevel = value;
				}
			}
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x001EFA7C File Offset: 0x001EEA7C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeAncestorLevel()
		{
			return this._mode == RelativeSourceMode.FindAncestor;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x001EFAC8 File Offset: 0x001EEAC8
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this._mode == RelativeSourceMode.PreviousData)
			{
				return RelativeSource.PreviousData;
			}
			if (this._mode == RelativeSourceMode.Self)
			{
				return RelativeSource.Self;
			}
			if (this._mode == RelativeSourceMode.TemplatedParent)
			{
				return RelativeSource.TemplatedParent;
			}
			return this;
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06003A24 RID: 14884 RVA: 0x001EFAF7 File Offset: 0x001EEAF7
		private bool IsUninitialized
		{
			get
			{
				return this._ancestorLevel == -1;
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x001EFB04 File Offset: 0x001EEB04
		private void InitializeMode(RelativeSourceMode mode)
		{
			if (mode == RelativeSourceMode.FindAncestor)
			{
				this._ancestorLevel = 1;
				this._mode = mode;
				return;
			}
			if (mode == RelativeSourceMode.PreviousData || mode == RelativeSourceMode.Self || mode == RelativeSourceMode.TemplatedParent)
			{
				this._ancestorLevel = 0;
				this._mode = mode;
				return;
			}
			throw new ArgumentException(SR.Get("RelativeSourceModeInvalid"), "mode");
		}

		// Token: 0x04001D91 RID: 7569
		private RelativeSourceMode _mode;

		// Token: 0x04001D92 RID: 7570
		private Type _ancestorType;

		// Token: 0x04001D93 RID: 7571
		private int _ancestorLevel = -1;

		// Token: 0x04001D94 RID: 7572
		private static RelativeSource s_previousData;

		// Token: 0x04001D95 RID: 7573
		private static RelativeSource s_templatedParent;

		// Token: 0x04001D96 RID: 7574
		private static RelativeSource s_self;
	}
}
