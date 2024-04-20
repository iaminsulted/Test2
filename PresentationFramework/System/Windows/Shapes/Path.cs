using System;
using System.Windows.Media;
using MS.Internal.PresentationFramework;

namespace System.Windows.Shapes
{
	// Token: 0x020003FA RID: 1018
	public sealed class Path : Shape
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06002BE7 RID: 11239 RVA: 0x001A53A1 File Offset: 0x001A43A1
		// (set) Token: 0x06002BE8 RID: 11240 RVA: 0x001A53B3 File Offset: 0x001A43B3
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(Path.DataProperty);
			}
			set
			{
				base.SetValue(Path.DataProperty, value);
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06002BE9 RID: 11241 RVA: 0x001A53C4 File Offset: 0x001A43C4
		protected override Geometry DefiningGeometry
		{
			get
			{
				Geometry geometry = this.Data;
				if (geometry == null)
				{
					geometry = Geometry.Empty;
				}
				return geometry;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06002BEA RID: 11242 RVA: 0x001A519B File Offset: 0x001A419B
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x04001AFD RID: 6909
		[CommonDependencyProperty]
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(Path), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), null);
	}
}
