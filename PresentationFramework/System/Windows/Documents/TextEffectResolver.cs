using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020006AB RID: 1707
	public static class TextEffectResolver
	{
		// Token: 0x06005690 RID: 22160 RVA: 0x0026A700 File Offset: 0x00269700
		public static TextEffectTarget[] Resolve(TextPointer startPosition, TextPointer endPosition, TextEffect effect)
		{
			if (effect == null)
			{
				throw new ArgumentNullException("effect");
			}
			ValidationHelper.VerifyPositionPair(startPosition, endPosition);
			TextPointer textPointer = new TextPointer(startPosition);
			TextEffectResolver.MoveToFirstCharacterSymbol(textPointer);
			List<TextEffectTarget> list = new List<TextEffectTarget>();
			while (textPointer.CompareTo(endPosition) < 0)
			{
				TextEffect textEffect = effect.Clone();
				TextPointer textPointer2 = new TextPointer(textPointer);
				TextEffectResolver.MoveToFirstNonCharacterSymbol(textPointer2, endPosition);
				textPointer2 = (TextPointer)TextPointerBase.Min(textPointer2, endPosition);
				textEffect.PositionStart = textPointer.TextContainer.Start.GetOffsetToPosition(textPointer);
				textEffect.PositionCount = textPointer.GetOffsetToPosition(textPointer2);
				list.Add(new TextEffectTarget(textPointer.Parent, textEffect));
				textPointer = textPointer2;
				TextEffectResolver.MoveToFirstCharacterSymbol(textPointer);
			}
			return list.ToArray();
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x0026A7A7 File Offset: 0x002697A7
		private static void MoveToFirstCharacterSymbol(TextPointer navigator)
		{
			while (navigator.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text && navigator.MoveToNextContextPosition(LogicalDirection.Forward))
			{
			}
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x0026A7BC File Offset: 0x002697BC
		private static void MoveToFirstNonCharacterSymbol(TextPointer navigator, TextPointer stopHint)
		{
			while (navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text && navigator.CompareTo(stopHint) < 0 && navigator.MoveToNextContextPosition(LogicalDirection.Forward))
			{
			}
		}
	}
}
