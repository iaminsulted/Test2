using System;
using System.Globalization;
using System.Text;

namespace MS.Internal.Xaml.Context
{
	// Token: 0x02000335 RID: 821
	internal class XamlContextStack<T> where T : XamlFrame
	{
		// Token: 0x06001EF4 RID: 7924 RVA: 0x001706F8 File Offset: 0x0016F6F8
		public XamlContextStack(Func<T> creationDelegate)
		{
			this._creationDelegate = creationDelegate;
			this.Grow();
			this._depth = 0;
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x00170714 File Offset: 0x0016F714
		public XamlContextStack(XamlContextStack<T> source, bool copy)
		{
			this._creationDelegate = source._creationDelegate;
			this._depth = source.Depth;
			if (!copy)
			{
				this._currentFrame = source.CurrentFrame;
				return;
			}
			T t = source.CurrentFrame;
			T t2 = default(T);
			while (t != null)
			{
				T t3 = (T)((object)t.Clone());
				if (this._currentFrame == null)
				{
					this._currentFrame = t3;
				}
				if (t2 != null)
				{
					t2.Previous = t3;
				}
				t2 = t3;
				t = (T)((object)t.Previous);
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x001707BC File Offset: 0x0016F7BC
		private void Grow()
		{
			T currentFrame = this._currentFrame;
			this._currentFrame = this._creationDelegate();
			this._currentFrame.Previous = currentFrame;
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x001707F7 File Offset: 0x0016F7F7
		public T CurrentFrame
		{
			get
			{
				return this._currentFrame;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001EF8 RID: 7928 RVA: 0x001707FF File Offset: 0x0016F7FF
		public T PreviousFrame
		{
			get
			{
				return (T)((object)this._currentFrame.Previous);
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x00170816 File Offset: 0x0016F816
		public T PreviousPreviousFrame
		{
			get
			{
				return (T)((object)this._currentFrame.Previous.Previous);
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x00170834 File Offset: 0x0016F834
		public T GetFrame(int depth)
		{
			T t = this._currentFrame;
			while (t.Depth > depth)
			{
				t = (T)((object)t.Previous);
			}
			return t;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0017086C File Offset: 0x0016F86C
		public void PushScope()
		{
			if (this._recycledFrame == null)
			{
				this.Grow();
			}
			else
			{
				T currentFrame = this._currentFrame;
				this._currentFrame = this._recycledFrame;
				this._recycledFrame = (T)((object)this._recycledFrame.Previous);
				this._currentFrame.Previous = currentFrame;
			}
			this._depth++;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x001708E0 File Offset: 0x0016F8E0
		public void PopScope()
		{
			this._depth--;
			T currentFrame = this._currentFrame;
			this._currentFrame = (T)((object)this._currentFrame.Previous);
			currentFrame.Previous = this._recycledFrame;
			this._recycledFrame = currentFrame;
			currentFrame.Reset();
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001EFD RID: 7933 RVA: 0x00170945 File Offset: 0x0016F945
		// (set) Token: 0x06001EFE RID: 7934 RVA: 0x0017094D File Offset: 0x0016F94D
		public int Depth
		{
			get
			{
				return this._depth;
			}
			set
			{
				this._depth = value;
			}
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x00170956 File Offset: 0x0016F956
		public void Trim()
		{
			this._recycledFrame = default(T);
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06001F00 RID: 7936 RVA: 0x00170964 File Offset: 0x0016F964
		public string Frames
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				T currentFrame = this._currentFrame;
				stringBuilder.AppendLine("Stack: " + ((this._currentFrame == null) ? -1 : (this._currentFrame.Depth + 1)).ToString(CultureInfo.InvariantCulture) + " frames");
				this.ShowFrame(stringBuilder, this._currentFrame);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x001709D8 File Offset: 0x0016F9D8
		private void ShowFrame(StringBuilder sb, T iteratorFrame)
		{
			if (iteratorFrame == null)
			{
				return;
			}
			if (iteratorFrame.Previous != null)
			{
				this.ShowFrame(sb, (T)((object)iteratorFrame.Previous));
			}
			sb.AppendLine("  " + iteratorFrame.Depth.ToString() + " " + iteratorFrame.ToString());
		}

		// Token: 0x04000F5E RID: 3934
		private int _depth;

		// Token: 0x04000F5F RID: 3935
		private T _currentFrame;

		// Token: 0x04000F60 RID: 3936
		private T _recycledFrame;

		// Token: 0x04000F61 RID: 3937
		private Func<T> _creationDelegate;
	}
}
