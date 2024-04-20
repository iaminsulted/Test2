using System;
using System.ComponentModel;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200043A RID: 1082
	public sealed class SeekStoryboard : ControllableStoryboardAction
	{
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x001DB8B6 File Offset: 0x001DA8B6
		// (set) Token: 0x0600348E RID: 13454 RVA: 0x001DB8BE File Offset: 0x001DA8BE
		public TimeSpan Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"SeekStoryboard"
					}));
				}
				this._offset = value;
			}
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x001DB8ED File Offset: 0x001DA8ED
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeOffset()
		{
			return !TimeSpan.Zero.Equals(this._offset);
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003490 RID: 13456 RVA: 0x001DB902 File Offset: 0x001DA902
		// (set) Token: 0x06003491 RID: 13457 RVA: 0x001DB90C File Offset: 0x001DA90C
		[DefaultValue(TimeSeekOrigin.BeginTime)]
		public TimeSeekOrigin Origin
		{
			get
			{
				return this._origin;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"SeekStoryboard"
					}));
				}
				if (value == TimeSeekOrigin.BeginTime || value == TimeSeekOrigin.Duration)
				{
					this._origin = value;
					return;
				}
				throw new ArgumentException(SR.Get("Storyboard_UnrecognizedTimeSeekOrigin"));
			}
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x001DB95D File Offset: 0x001DA95D
		internal override void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
			if (containingFE != null)
			{
				storyboard.Seek(containingFE, this.Offset, this.Origin);
				return;
			}
			storyboard.Seek(containingFCE, this.Offset, this.Origin);
		}

		// Token: 0x04001C5A RID: 7258
		private TimeSpan _offset = TimeSpan.Zero;

		// Token: 0x04001C5B RID: 7259
		private TimeSeekOrigin _origin;
	}
}
