using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Windows.Media.Animation
{
	// Token: 0x02000436 RID: 1078
	public class ThicknessKeyFrameCollection : Freezable, IList, ICollection, IEnumerable
	{
		// Token: 0x06003466 RID: 13414 RVA: 0x001DB36E File Offset: 0x001DA36E
		public ThicknessKeyFrameCollection()
		{
			this._keyFrames = new List<ThicknessKeyFrame>(2);
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x001DB382 File Offset: 0x001DA382
		public static ThicknessKeyFrameCollection Empty
		{
			get
			{
				if (ThicknessKeyFrameCollection.s_emptyCollection == null)
				{
					ThicknessKeyFrameCollection thicknessKeyFrameCollection = new ThicknessKeyFrameCollection();
					thicknessKeyFrameCollection._keyFrames = new List<ThicknessKeyFrame>(0);
					thicknessKeyFrameCollection.Freeze();
					ThicknessKeyFrameCollection.s_emptyCollection = thicknessKeyFrameCollection;
				}
				return ThicknessKeyFrameCollection.s_emptyCollection;
			}
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x001DB3AC File Offset: 0x001DA3AC
		public new ThicknessKeyFrameCollection Clone()
		{
			return (ThicknessKeyFrameCollection)base.Clone();
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x001DB3B9 File Offset: 0x001DA3B9
		protected override Freezable CreateInstanceCore()
		{
			return new ThicknessKeyFrameCollection();
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x001DB3C0 File Offset: 0x001DA3C0
		protected override void CloneCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.CloneCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].Clone();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x001DB42C File Offset: 0x001DA42C
		protected override void CloneCurrentValueCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.CloneCurrentValueCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].CloneCurrentValue();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x001DB498 File Offset: 0x001DA498
		protected override void GetAsFrozenCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.GetAsFrozenCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].GetAsFrozen();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x001DB504 File Offset: 0x001DA504
		protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.GetCurrentValueAsFrozenCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].GetCurrentValueAsFrozen();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x001DB570 File Offset: 0x001DA570
		protected override bool FreezeCore(bool isChecking)
		{
			bool flag = base.FreezeCore(isChecking);
			int num = 0;
			while (num < this._keyFrames.Count && flag)
			{
				flag &= Freezable.Freeze(this._keyFrames[num], isChecking);
				num++;
			}
			return flag;
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x001DB5B5 File Offset: 0x001DA5B5
		public IEnumerator GetEnumerator()
		{
			base.ReadPreamble();
			return this._keyFrames.GetEnumerator();
		}

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06003470 RID: 13424 RVA: 0x001DB5CD File Offset: 0x001DA5CD
		public int Count
		{
			get
			{
				base.ReadPreamble();
				return this._keyFrames.Count;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003471 RID: 13425 RVA: 0x001DB5E0 File Offset: 0x001DA5E0
		public bool IsSynchronized
		{
			get
			{
				base.ReadPreamble();
				return base.IsFrozen || base.Dispatcher != null;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06003472 RID: 13426 RVA: 0x001DB5FB File Offset: 0x001DA5FB
		public object SyncRoot
		{
			get
			{
				base.ReadPreamble();
				return ((ICollection)this._keyFrames).SyncRoot;
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x001DB60E File Offset: 0x001DA60E
		void ICollection.CopyTo(Array array, int index)
		{
			base.ReadPreamble();
			((ICollection)this._keyFrames).CopyTo(array, index);
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x001DB623 File Offset: 0x001DA623
		public void CopyTo(ThicknessKeyFrame[] array, int index)
		{
			base.ReadPreamble();
			this._keyFrames.CopyTo(array, index);
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x001DB638 File Offset: 0x001DA638
		int IList.Add(object keyFrame)
		{
			return this.Add((ThicknessKeyFrame)keyFrame);
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x001DB646 File Offset: 0x001DA646
		public int Add(ThicknessKeyFrame keyFrame)
		{
			if (keyFrame == null)
			{
				throw new ArgumentNullException("keyFrame");
			}
			base.WritePreamble();
			base.OnFreezablePropertyChanged(null, keyFrame);
			this._keyFrames.Add(keyFrame);
			base.WritePostscript();
			return this._keyFrames.Count - 1;
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x001DB684 File Offset: 0x001DA684
		public void Clear()
		{
			base.WritePreamble();
			if (this._keyFrames.Count > 0)
			{
				for (int i = 0; i < this._keyFrames.Count; i++)
				{
					base.OnFreezablePropertyChanged(this._keyFrames[i], null);
				}
				this._keyFrames.Clear();
				base.WritePostscript();
			}
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x001DB6DF File Offset: 0x001DA6DF
		bool IList.Contains(object keyFrame)
		{
			return this.Contains((ThicknessKeyFrame)keyFrame);
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x001DB6ED File Offset: 0x001DA6ED
		public bool Contains(ThicknessKeyFrame keyFrame)
		{
			base.ReadPreamble();
			return this._keyFrames.Contains(keyFrame);
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x001DB701 File Offset: 0x001DA701
		int IList.IndexOf(object keyFrame)
		{
			return this.IndexOf((ThicknessKeyFrame)keyFrame);
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x001DB70F File Offset: 0x001DA70F
		public int IndexOf(ThicknessKeyFrame keyFrame)
		{
			base.ReadPreamble();
			return this._keyFrames.IndexOf(keyFrame);
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x001DB723 File Offset: 0x001DA723
		void IList.Insert(int index, object keyFrame)
		{
			this.Insert(index, (ThicknessKeyFrame)keyFrame);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x001DB732 File Offset: 0x001DA732
		public void Insert(int index, ThicknessKeyFrame keyFrame)
		{
			if (keyFrame == null)
			{
				throw new ArgumentNullException("keyFrame");
			}
			base.WritePreamble();
			base.OnFreezablePropertyChanged(null, keyFrame);
			this._keyFrames.Insert(index, keyFrame);
			base.WritePostscript();
		}

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x0600347E RID: 13438 RVA: 0x001DB763 File Offset: 0x001DA763
		public bool IsFixedSize
		{
			get
			{
				base.ReadPreamble();
				return base.IsFrozen;
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600347F RID: 13439 RVA: 0x001DB763 File Offset: 0x001DA763
		public bool IsReadOnly
		{
			get
			{
				base.ReadPreamble();
				return base.IsFrozen;
			}
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x001DB771 File Offset: 0x001DA771
		void IList.Remove(object keyFrame)
		{
			this.Remove((ThicknessKeyFrame)keyFrame);
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x001DB77F File Offset: 0x001DA77F
		public void Remove(ThicknessKeyFrame keyFrame)
		{
			base.WritePreamble();
			if (this._keyFrames.Contains(keyFrame))
			{
				base.OnFreezablePropertyChanged(keyFrame, null);
				this._keyFrames.Remove(keyFrame);
				base.WritePostscript();
			}
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x001DB7B0 File Offset: 0x001DA7B0
		public void RemoveAt(int index)
		{
			base.WritePreamble();
			base.OnFreezablePropertyChanged(this._keyFrames[index], null);
			this._keyFrames.RemoveAt(index);
			base.WritePostscript();
		}

		// Token: 0x17000B1A RID: 2842
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (ThicknessKeyFrame)value;
			}
		}

		// Token: 0x17000B1B RID: 2843
		public ThicknessKeyFrame this[int index]
		{
			get
			{
				base.ReadPreamble();
				return this._keyFrames[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "ThicknessKeyFrameCollection[{0}]", index));
				}
				base.WritePreamble();
				if (value != this._keyFrames[index])
				{
					base.OnFreezablePropertyChanged(this._keyFrames[index], value);
					this._keyFrames[index] = value;
					base.WritePostscript();
				}
			}
		}

		// Token: 0x04001C58 RID: 7256
		private List<ThicknessKeyFrame> _keyFrames;

		// Token: 0x04001C59 RID: 7257
		private static ThicknessKeyFrameCollection s_emptyCollection;
	}
}
