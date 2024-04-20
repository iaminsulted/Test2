using System;

namespace MS.Internal.Data
{
	// Token: 0x0200023D RID: 573
	internal struct RBFinger<T>
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x00158004 File Offset: 0x00157004
		// (set) Token: 0x060015DB RID: 5595 RVA: 0x0015800C File Offset: 0x0015700C
		public RBNode<T> Node { readonly get; set; }

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x00158015 File Offset: 0x00157015
		// (set) Token: 0x060015DD RID: 5597 RVA: 0x0015801D File Offset: 0x0015701D
		public int Offset { readonly get; set; }

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x00158026 File Offset: 0x00157026
		// (set) Token: 0x060015DF RID: 5599 RVA: 0x0015802E File Offset: 0x0015702E
		public int Index { readonly get; set; }

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x00158037 File Offset: 0x00157037
		// (set) Token: 0x060015E1 RID: 5601 RVA: 0x0015803F File Offset: 0x0015703F
		public bool Found { readonly get; set; }

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x00158048 File Offset: 0x00157048
		public T Item
		{
			get
			{
				return this.Node.GetItemAt(this.Offset);
			}
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0015805B File Offset: 0x0015705B
		public void SetItem(T x)
		{
			this.Node.SetItemAt(this.Offset, x);
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x00158070 File Offset: 0x00157070
		public bool IsValid
		{
			get
			{
				return this.Node != null && this.Node.HasData;
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x00158087 File Offset: 0x00157087
		public static RBFinger<T>operator +(RBFinger<T> finger, int delta)
		{
			if (delta >= 0)
			{
				while (delta > 0)
				{
					if (!finger.IsValid)
					{
						break;
					}
					finger = ++finger;
					delta--;
				}
			}
			else
			{
				while (delta < 0 && finger.IsValid)
				{
					finger = --finger;
					delta++;
				}
			}
			return finger;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x001580C6 File Offset: 0x001570C6
		public static RBFinger<T>operator -(RBFinger<T> finger, int delta)
		{
			return finger + -delta;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x001580D0 File Offset: 0x001570D0
		public static int operator -(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index - f2.Index;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x001580E4 File Offset: 0x001570E4
		public static RBFinger<T>operator ++(RBFinger<T> finger)
		{
			finger.Offset++;
			finger.Index++;
			if (finger.Offset == finger.Node.Size)
			{
				finger.Node = finger.Node.GetSuccessor();
				finger.Offset = 0;
			}
			return finger;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00158140 File Offset: 0x00157140
		public static RBFinger<T>operator --(RBFinger<T> finger)
		{
			finger.Offset--;
			finger.Index--;
			if (finger.Offset < 0)
			{
				finger.Node = finger.Node.GetPredecessor();
				if (finger.Node != null)
				{
					finger.Offset = finger.Node.Size - 1;
				}
			}
			return finger;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x001581A7 File Offset: 0x001571A7
		public static bool operator <(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index < f2.Index;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x001581B9 File Offset: 0x001571B9
		public static bool operator >(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index > f2.Index;
		}
	}
}
