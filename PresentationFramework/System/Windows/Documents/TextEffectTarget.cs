using System;
using System.Windows.Media;
using MS.Internal.Text;

namespace System.Windows.Documents
{
	// Token: 0x020006AC RID: 1708
	public class TextEffectTarget
	{
		// Token: 0x06005693 RID: 22163 RVA: 0x0026A7DB File Offset: 0x002697DB
		internal TextEffectTarget(DependencyObject element, TextEffect effect)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (effect == null)
			{
				throw new ArgumentNullException("effect");
			}
			this._element = element;
			this._effect = effect;
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06005694 RID: 22164 RVA: 0x0026A80D File Offset: 0x0026980D
		public DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06005695 RID: 22165 RVA: 0x0026A815 File Offset: 0x00269815
		public TextEffect TextEffect
		{
			get
			{
				return this._effect;
			}
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0026A820 File Offset: 0x00269820
		public void Enable()
		{
			TextEffectCollection textEffectCollection = DynamicPropertyReader.GetTextEffects(this._element);
			if (textEffectCollection == null)
			{
				textEffectCollection = new TextEffectCollection();
				this._element.SetValue(TextElement.TextEffectsProperty, textEffectCollection);
			}
			for (int i = 0; i < textEffectCollection.Count; i++)
			{
				if (textEffectCollection[i] == this._effect)
				{
					return;
				}
			}
			textEffectCollection.Add(this._effect);
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x0026A880 File Offset: 0x00269880
		public void Disable()
		{
			TextEffectCollection textEffects = DynamicPropertyReader.GetTextEffects(this._element);
			if (textEffects != null)
			{
				for (int i = 0; i < textEffects.Count; i++)
				{
					if (textEffects[i] == this._effect)
					{
						textEffects.RemoveAt(i);
						return;
					}
				}
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06005698 RID: 22168 RVA: 0x0026A8C4 File Offset: 0x002698C4
		public bool IsEnabled
		{
			get
			{
				TextEffectCollection textEffects = DynamicPropertyReader.GetTextEffects(this._element);
				if (textEffects != null)
				{
					for (int i = 0; i < textEffects.Count; i++)
					{
						if (textEffects[i] == this._effect)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x04002FA1 RID: 12193
		private DependencyObject _element;

		// Token: 0x04002FA2 RID: 12194
		private TextEffect _effect;
	}
}
