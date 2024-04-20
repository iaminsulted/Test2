using System;
using System.Windows.Input;

namespace System.Windows.Documents
{
	// Token: 0x020005ED RID: 1517
	public static class EditingCommands
	{
		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x060049B5 RID: 18869 RVA: 0x00231176 File Offset: 0x00230176
		public static RoutedUICommand ToggleInsert
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleInsert, "ToggleInsert");
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x060049B6 RID: 18870 RVA: 0x00231187 File Offset: 0x00230187
		public static RoutedUICommand Delete
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._Delete, "Delete");
			}
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x060049B7 RID: 18871 RVA: 0x00231198 File Offset: 0x00230198
		public static RoutedUICommand Backspace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._Backspace, "Backspace");
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x002311A9 File Offset: 0x002301A9
		public static RoutedUICommand DeleteNextWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeleteNextWord, "DeleteNextWord");
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x002311BA File Offset: 0x002301BA
		public static RoutedUICommand DeletePreviousWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeletePreviousWord, "DeletePreviousWord");
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x002311CB File Offset: 0x002301CB
		public static RoutedUICommand EnterParagraphBreak
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._EnterParagraphBreak, "EnterParagraphBreak");
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x002311DC File Offset: 0x002301DC
		public static RoutedUICommand EnterLineBreak
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._EnterLineBreak, "EnterLineBreak");
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x060049BC RID: 18876 RVA: 0x002311ED File Offset: 0x002301ED
		public static RoutedUICommand TabForward
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._TabForward, "TabForward");
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x002311FE File Offset: 0x002301FE
		public static RoutedUICommand TabBackward
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._TabBackward, "TabBackward");
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x060049BE RID: 18878 RVA: 0x0023120F File Offset: 0x0023020F
		public static RoutedUICommand MoveRightByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveRightByCharacter, "MoveRightByCharacter");
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x00231220 File Offset: 0x00230220
		public static RoutedUICommand MoveLeftByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveLeftByCharacter, "MoveLeftByCharacter");
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x00231231 File Offset: 0x00230231
		public static RoutedUICommand MoveRightByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveRightByWord, "MoveRightByWord");
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x00231242 File Offset: 0x00230242
		public static RoutedUICommand MoveLeftByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveLeftByWord, "MoveLeftByWord");
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x060049C2 RID: 18882 RVA: 0x00231253 File Offset: 0x00230253
		public static RoutedUICommand MoveDownByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveDownByLine, "MoveDownByLine");
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x00231264 File Offset: 0x00230264
		public static RoutedUICommand MoveUpByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveUpByLine, "MoveUpByLine");
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00231275 File Offset: 0x00230275
		public static RoutedUICommand MoveDownByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveDownByParagraph, "MoveDownByParagraph");
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x00231286 File Offset: 0x00230286
		public static RoutedUICommand MoveUpByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveUpByParagraph, "MoveUpByParagraph");
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x00231297 File Offset: 0x00230297
		public static RoutedUICommand MoveDownByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveDownByPage, "MoveDownByPage");
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x060049C7 RID: 18887 RVA: 0x002312A8 File Offset: 0x002302A8
		public static RoutedUICommand MoveUpByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveUpByPage, "MoveUpByPage");
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x002312B9 File Offset: 0x002302B9
		public static RoutedUICommand MoveToLineStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToLineStart, "MoveToLineStart");
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060049C9 RID: 18889 RVA: 0x002312CA File Offset: 0x002302CA
		public static RoutedUICommand MoveToLineEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToLineEnd, "MoveToLineEnd");
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x002312DB File Offset: 0x002302DB
		public static RoutedUICommand MoveToDocumentStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToDocumentStart, "MoveToDocumentStart");
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x060049CB RID: 18891 RVA: 0x002312EC File Offset: 0x002302EC
		public static RoutedUICommand MoveToDocumentEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToDocumentEnd, "MoveToDocumentEnd");
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x060049CC RID: 18892 RVA: 0x002312FD File Offset: 0x002302FD
		public static RoutedUICommand SelectRightByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectRightByCharacter, "SelectRightByCharacter");
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x0023130E File Offset: 0x0023030E
		public static RoutedUICommand SelectLeftByCharacter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectLeftByCharacter, "SelectLeftByCharacter");
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x060049CE RID: 18894 RVA: 0x0023131F File Offset: 0x0023031F
		public static RoutedUICommand SelectRightByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectRightByWord, "SelectRightByWord");
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x060049CF RID: 18895 RVA: 0x00231330 File Offset: 0x00230330
		public static RoutedUICommand SelectLeftByWord
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectLeftByWord, "SelectLeftByWord");
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x00231341 File Offset: 0x00230341
		public static RoutedUICommand SelectDownByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectDownByLine, "SelectDownByLine");
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x060049D1 RID: 18897 RVA: 0x00231352 File Offset: 0x00230352
		public static RoutedUICommand SelectUpByLine
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectUpByLine, "SelectUpByLine");
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x060049D2 RID: 18898 RVA: 0x00231363 File Offset: 0x00230363
		public static RoutedUICommand SelectDownByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectDownByParagraph, "SelectDownByParagraph");
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x060049D3 RID: 18899 RVA: 0x00231374 File Offset: 0x00230374
		public static RoutedUICommand SelectUpByParagraph
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectUpByParagraph, "SelectUpByParagraph");
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x060049D4 RID: 18900 RVA: 0x00231385 File Offset: 0x00230385
		public static RoutedUICommand SelectDownByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectDownByPage, "SelectDownByPage");
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x060049D5 RID: 18901 RVA: 0x00231396 File Offset: 0x00230396
		public static RoutedUICommand SelectUpByPage
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectUpByPage, "SelectUpByPage");
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x060049D6 RID: 18902 RVA: 0x002313A7 File Offset: 0x002303A7
		public static RoutedUICommand SelectToLineStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToLineStart, "SelectToLineStart");
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x060049D7 RID: 18903 RVA: 0x002313B8 File Offset: 0x002303B8
		public static RoutedUICommand SelectToLineEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToLineEnd, "SelectToLineEnd");
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x060049D8 RID: 18904 RVA: 0x002313C9 File Offset: 0x002303C9
		public static RoutedUICommand SelectToDocumentStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToDocumentStart, "SelectToDocumentStart");
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x060049D9 RID: 18905 RVA: 0x002313DA File Offset: 0x002303DA
		public static RoutedUICommand SelectToDocumentEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToDocumentEnd, "SelectToDocumentEnd");
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x060049DA RID: 18906 RVA: 0x002313EB File Offset: 0x002303EB
		public static RoutedUICommand ToggleBold
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleBold, "ToggleBold");
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x060049DB RID: 18907 RVA: 0x002313FC File Offset: 0x002303FC
		public static RoutedUICommand ToggleItalic
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleItalic, "ToggleItalic");
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x060049DC RID: 18908 RVA: 0x0023140D File Offset: 0x0023040D
		public static RoutedUICommand ToggleUnderline
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleUnderline, "ToggleUnderline");
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x0023141E File Offset: 0x0023041E
		public static RoutedUICommand ToggleSubscript
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleSubscript, "ToggleSubscript");
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x0023142F File Offset: 0x0023042F
		public static RoutedUICommand ToggleSuperscript
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleSuperscript, "ToggleSuperscript");
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x00231440 File Offset: 0x00230440
		public static RoutedUICommand IncreaseFontSize
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._IncreaseFontSize, "IncreaseFontSize");
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x00231451 File Offset: 0x00230451
		public static RoutedUICommand DecreaseFontSize
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DecreaseFontSize, "DecreaseFontSize");
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x060049E1 RID: 18913 RVA: 0x00231462 File Offset: 0x00230462
		public static RoutedUICommand AlignLeft
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignLeft, "AlignLeft");
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x060049E2 RID: 18914 RVA: 0x00231473 File Offset: 0x00230473
		public static RoutedUICommand AlignCenter
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignCenter, "AlignCenter");
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x060049E3 RID: 18915 RVA: 0x00231484 File Offset: 0x00230484
		public static RoutedUICommand AlignRight
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignRight, "AlignRight");
			}
		}

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x00231495 File Offset: 0x00230495
		public static RoutedUICommand AlignJustify
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._AlignJustify, "AlignJustify");
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x002314A6 File Offset: 0x002304A6
		public static RoutedUICommand ToggleBullets
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleBullets, "ToggleBullets");
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x002314B7 File Offset: 0x002304B7
		public static RoutedUICommand ToggleNumbering
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleNumbering, "ToggleNumbering");
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x002314C8 File Offset: 0x002304C8
		public static RoutedUICommand IncreaseIndentation
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._IncreaseIndentation, "IncreaseIndentation");
			}
		}

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x002314D9 File Offset: 0x002304D9
		public static RoutedUICommand DecreaseIndentation
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DecreaseIndentation, "DecreaseIndentation");
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x060049E9 RID: 18921 RVA: 0x002314EA File Offset: 0x002304EA
		public static RoutedUICommand CorrectSpellingError
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._CorrectSpellingError, "CorrectSpellingError");
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x002314FB File Offset: 0x002304FB
		public static RoutedUICommand IgnoreSpellingError
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._IgnoreSpellingError, "IgnoreSpellingError");
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x060049EB RID: 18923 RVA: 0x0023150C File Offset: 0x0023050C
		internal static RoutedUICommand Space
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._Space, "Space");
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x0023151D File Offset: 0x0023051D
		internal static RoutedUICommand ShiftSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ShiftSpace, "ShiftSpace");
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060049ED RID: 18925 RVA: 0x0023152E File Offset: 0x0023052E
		internal static RoutedUICommand MoveToColumnStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToColumnStart, "MoveToColumnStart");
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x060049EE RID: 18926 RVA: 0x0023153F File Offset: 0x0023053F
		internal static RoutedUICommand MoveToColumnEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToColumnEnd, "MoveToColumnEnd");
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x060049EF RID: 18927 RVA: 0x00231550 File Offset: 0x00230550
		internal static RoutedUICommand MoveToWindowTop
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToWindowTop, "MoveToWindowTop");
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x060049F0 RID: 18928 RVA: 0x00231561 File Offset: 0x00230561
		internal static RoutedUICommand MoveToWindowBottom
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MoveToWindowBottom, "MoveToWindowBottom");
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x060049F1 RID: 18929 RVA: 0x00231572 File Offset: 0x00230572
		internal static RoutedUICommand SelectToColumnStart
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToColumnStart, "SelectToColumnStart");
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x00231583 File Offset: 0x00230583
		internal static RoutedUICommand SelectToColumnEnd
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToColumnEnd, "SelectToColumnEnd");
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x060049F3 RID: 18931 RVA: 0x00231594 File Offset: 0x00230594
		internal static RoutedUICommand SelectToWindowTop
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToWindowTop, "SelectToWindowTop");
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x002315A5 File Offset: 0x002305A5
		internal static RoutedUICommand SelectToWindowBottom
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SelectToWindowBottom, "SelectToWindowBottom");
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x060049F5 RID: 18933 RVA: 0x002315B6 File Offset: 0x002305B6
		internal static RoutedUICommand ResetFormat
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ResetFormat, "ResetFormat");
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x002315C7 File Offset: 0x002305C7
		internal static RoutedUICommand ToggleSpellCheck
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ToggleSpellCheck, "ToggleSpellCheck");
			}
		}

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060049F7 RID: 18935 RVA: 0x002315D8 File Offset: 0x002305D8
		internal static RoutedUICommand ApplyFontSize
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyFontSize, "ApplyFontSize");
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x002315E9 File Offset: 0x002305E9
		internal static RoutedUICommand ApplyFontFamily
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyFontFamily, "ApplyFontFamily");
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060049F9 RID: 18937 RVA: 0x002315FA File Offset: 0x002305FA
		internal static RoutedUICommand ApplyForeground
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyForeground, "ApplyForeground");
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x0023160B File Offset: 0x0023060B
		internal static RoutedUICommand ApplyBackground
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyBackground, "ApplyBackground");
			}
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060049FB RID: 18939 RVA: 0x0023161C File Offset: 0x0023061C
		internal static RoutedUICommand ApplyInlineFlowDirectionRTL
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyInlineFlowDirectionRTL, "ApplyInlineFlowDirectionRTL");
			}
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x0023162D File Offset: 0x0023062D
		internal static RoutedUICommand ApplyInlineFlowDirectionLTR
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyInlineFlowDirectionLTR, "ApplyInlineFlowDirectionLTR");
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060049FD RID: 18941 RVA: 0x0023163E File Offset: 0x0023063E
		internal static RoutedUICommand ApplySingleSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplySingleSpace, "ApplySingleSpace");
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x0023164F File Offset: 0x0023064F
		internal static RoutedUICommand ApplyOneAndAHalfSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyOneAndAHalfSpace, "ApplyOneAndAHalfSpace");
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060049FF RID: 18943 RVA: 0x00231660 File Offset: 0x00230660
		internal static RoutedUICommand ApplyDoubleSpace
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyDoubleSpace, "ApplyDoubleSpace");
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06004A00 RID: 18944 RVA: 0x00231671 File Offset: 0x00230671
		internal static RoutedUICommand ApplyParagraphFlowDirectionRTL
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyParagraphFlowDirectionRTL, "ApplyParagraphFlowDirectionRTL");
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06004A01 RID: 18945 RVA: 0x00231682 File Offset: 0x00230682
		internal static RoutedUICommand ApplyParagraphFlowDirectionLTR
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._ApplyParagraphFlowDirectionLTR, "ApplyParagraphFlowDirectionLTR");
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x06004A02 RID: 18946 RVA: 0x00231693 File Offset: 0x00230693
		internal static RoutedUICommand CopyFormat
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._CopyFormat, "CopyFormat");
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06004A03 RID: 18947 RVA: 0x002316A4 File Offset: 0x002306A4
		internal static RoutedUICommand PasteFormat
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._PasteFormat, "PasteFormat");
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x002316B5 File Offset: 0x002306B5
		internal static RoutedUICommand RemoveListMarkers
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._RemoveListMarkers, "RemoveListMarkers");
			}
		}

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06004A05 RID: 18949 RVA: 0x002316C6 File Offset: 0x002306C6
		internal static RoutedUICommand InsertTable
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._InsertTable, "InsertTable");
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06004A06 RID: 18950 RVA: 0x002316D7 File Offset: 0x002306D7
		internal static RoutedUICommand InsertRows
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._InsertRows, "InsertRows");
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x06004A07 RID: 18951 RVA: 0x002316E8 File Offset: 0x002306E8
		internal static RoutedUICommand InsertColumns
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._InsertColumns, "InsertColumns");
			}
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x06004A08 RID: 18952 RVA: 0x002316F9 File Offset: 0x002306F9
		internal static RoutedUICommand DeleteRows
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeleteRows, "DeleteRows");
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x06004A09 RID: 18953 RVA: 0x0023170A File Offset: 0x0023070A
		internal static RoutedUICommand DeleteColumns
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._DeleteColumns, "DeleteColumns");
			}
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06004A0A RID: 18954 RVA: 0x0023171B File Offset: 0x0023071B
		internal static RoutedUICommand MergeCells
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._MergeCells, "MergeCells");
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x06004A0B RID: 18955 RVA: 0x0023172C File Offset: 0x0023072C
		internal static RoutedUICommand SplitCell
		{
			get
			{
				return EditingCommands.EnsureCommand(ref EditingCommands._SplitCell, "SplitCell");
			}
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x00231740 File Offset: 0x00230740
		private static RoutedUICommand EnsureCommand(ref RoutedUICommand command, string commandPropertyName)
		{
			object synchronize = EditingCommands._synchronize;
			lock (synchronize)
			{
				if (command == null)
				{
					command = new RoutedUICommand(commandPropertyName, commandPropertyName, typeof(EditingCommands));
				}
			}
			return command;
		}

		// Token: 0x0400267A RID: 9850
		private static object _synchronize = new object();

		// Token: 0x0400267B RID: 9851
		private static RoutedUICommand _ToggleInsert;

		// Token: 0x0400267C RID: 9852
		private static RoutedUICommand _Delete;

		// Token: 0x0400267D RID: 9853
		private static RoutedUICommand _Backspace;

		// Token: 0x0400267E RID: 9854
		private static RoutedUICommand _DeleteNextWord;

		// Token: 0x0400267F RID: 9855
		private static RoutedUICommand _DeletePreviousWord;

		// Token: 0x04002680 RID: 9856
		private static RoutedUICommand _EnterParagraphBreak;

		// Token: 0x04002681 RID: 9857
		private static RoutedUICommand _EnterLineBreak;

		// Token: 0x04002682 RID: 9858
		private static RoutedUICommand _TabForward;

		// Token: 0x04002683 RID: 9859
		private static RoutedUICommand _TabBackward;

		// Token: 0x04002684 RID: 9860
		private static RoutedUICommand _Space;

		// Token: 0x04002685 RID: 9861
		private static RoutedUICommand _ShiftSpace;

		// Token: 0x04002686 RID: 9862
		private static RoutedUICommand _MoveRightByCharacter;

		// Token: 0x04002687 RID: 9863
		private static RoutedUICommand _MoveLeftByCharacter;

		// Token: 0x04002688 RID: 9864
		private static RoutedUICommand _MoveRightByWord;

		// Token: 0x04002689 RID: 9865
		private static RoutedUICommand _MoveLeftByWord;

		// Token: 0x0400268A RID: 9866
		private static RoutedUICommand _MoveDownByLine;

		// Token: 0x0400268B RID: 9867
		private static RoutedUICommand _MoveUpByLine;

		// Token: 0x0400268C RID: 9868
		private static RoutedUICommand _MoveDownByParagraph;

		// Token: 0x0400268D RID: 9869
		private static RoutedUICommand _MoveUpByParagraph;

		// Token: 0x0400268E RID: 9870
		private static RoutedUICommand _MoveDownByPage;

		// Token: 0x0400268F RID: 9871
		private static RoutedUICommand _MoveUpByPage;

		// Token: 0x04002690 RID: 9872
		private static RoutedUICommand _MoveToLineStart;

		// Token: 0x04002691 RID: 9873
		private static RoutedUICommand _MoveToLineEnd;

		// Token: 0x04002692 RID: 9874
		private static RoutedUICommand _MoveToColumnStart;

		// Token: 0x04002693 RID: 9875
		private static RoutedUICommand _MoveToColumnEnd;

		// Token: 0x04002694 RID: 9876
		private static RoutedUICommand _MoveToWindowTop;

		// Token: 0x04002695 RID: 9877
		private static RoutedUICommand _MoveToWindowBottom;

		// Token: 0x04002696 RID: 9878
		private static RoutedUICommand _MoveToDocumentStart;

		// Token: 0x04002697 RID: 9879
		private static RoutedUICommand _MoveToDocumentEnd;

		// Token: 0x04002698 RID: 9880
		private static RoutedUICommand _SelectRightByCharacter;

		// Token: 0x04002699 RID: 9881
		private static RoutedUICommand _SelectLeftByCharacter;

		// Token: 0x0400269A RID: 9882
		private static RoutedUICommand _SelectRightByWord;

		// Token: 0x0400269B RID: 9883
		private static RoutedUICommand _SelectLeftByWord;

		// Token: 0x0400269C RID: 9884
		private static RoutedUICommand _SelectDownByLine;

		// Token: 0x0400269D RID: 9885
		private static RoutedUICommand _SelectUpByLine;

		// Token: 0x0400269E RID: 9886
		private static RoutedUICommand _SelectDownByParagraph;

		// Token: 0x0400269F RID: 9887
		private static RoutedUICommand _SelectUpByParagraph;

		// Token: 0x040026A0 RID: 9888
		private static RoutedUICommand _SelectDownByPage;

		// Token: 0x040026A1 RID: 9889
		private static RoutedUICommand _SelectUpByPage;

		// Token: 0x040026A2 RID: 9890
		private static RoutedUICommand _SelectToLineStart;

		// Token: 0x040026A3 RID: 9891
		private static RoutedUICommand _SelectToLineEnd;

		// Token: 0x040026A4 RID: 9892
		private static RoutedUICommand _SelectToColumnStart;

		// Token: 0x040026A5 RID: 9893
		private static RoutedUICommand _SelectToColumnEnd;

		// Token: 0x040026A6 RID: 9894
		private static RoutedUICommand _SelectToWindowTop;

		// Token: 0x040026A7 RID: 9895
		private static RoutedUICommand _SelectToWindowBottom;

		// Token: 0x040026A8 RID: 9896
		private static RoutedUICommand _SelectToDocumentStart;

		// Token: 0x040026A9 RID: 9897
		private static RoutedUICommand _SelectToDocumentEnd;

		// Token: 0x040026AA RID: 9898
		private static RoutedUICommand _CopyFormat;

		// Token: 0x040026AB RID: 9899
		private static RoutedUICommand _PasteFormat;

		// Token: 0x040026AC RID: 9900
		private static RoutedUICommand _ResetFormat;

		// Token: 0x040026AD RID: 9901
		private static RoutedUICommand _ToggleBold;

		// Token: 0x040026AE RID: 9902
		private static RoutedUICommand _ToggleItalic;

		// Token: 0x040026AF RID: 9903
		private static RoutedUICommand _ToggleUnderline;

		// Token: 0x040026B0 RID: 9904
		private static RoutedUICommand _ToggleSubscript;

		// Token: 0x040026B1 RID: 9905
		private static RoutedUICommand _ToggleSuperscript;

		// Token: 0x040026B2 RID: 9906
		private static RoutedUICommand _IncreaseFontSize;

		// Token: 0x040026B3 RID: 9907
		private static RoutedUICommand _DecreaseFontSize;

		// Token: 0x040026B4 RID: 9908
		private static RoutedUICommand _ApplyFontSize;

		// Token: 0x040026B5 RID: 9909
		private static RoutedUICommand _ApplyFontFamily;

		// Token: 0x040026B6 RID: 9910
		private static RoutedUICommand _ApplyForeground;

		// Token: 0x040026B7 RID: 9911
		private static RoutedUICommand _ApplyBackground;

		// Token: 0x040026B8 RID: 9912
		private static RoutedUICommand _ToggleSpellCheck;

		// Token: 0x040026B9 RID: 9913
		private static RoutedUICommand _ApplyInlineFlowDirectionRTL;

		// Token: 0x040026BA RID: 9914
		private static RoutedUICommand _ApplyInlineFlowDirectionLTR;

		// Token: 0x040026BB RID: 9915
		private static RoutedUICommand _AlignLeft;

		// Token: 0x040026BC RID: 9916
		private static RoutedUICommand _AlignCenter;

		// Token: 0x040026BD RID: 9917
		private static RoutedUICommand _AlignRight;

		// Token: 0x040026BE RID: 9918
		private static RoutedUICommand _AlignJustify;

		// Token: 0x040026BF RID: 9919
		private static RoutedUICommand _ApplySingleSpace;

		// Token: 0x040026C0 RID: 9920
		private static RoutedUICommand _ApplyOneAndAHalfSpace;

		// Token: 0x040026C1 RID: 9921
		private static RoutedUICommand _ApplyDoubleSpace;

		// Token: 0x040026C2 RID: 9922
		private static RoutedUICommand _IncreaseIndentation;

		// Token: 0x040026C3 RID: 9923
		private static RoutedUICommand _DecreaseIndentation;

		// Token: 0x040026C4 RID: 9924
		private static RoutedUICommand _ApplyParagraphFlowDirectionRTL;

		// Token: 0x040026C5 RID: 9925
		private static RoutedUICommand _ApplyParagraphFlowDirectionLTR;

		// Token: 0x040026C6 RID: 9926
		private static RoutedUICommand _RemoveListMarkers;

		// Token: 0x040026C7 RID: 9927
		private static RoutedUICommand _ToggleBullets;

		// Token: 0x040026C8 RID: 9928
		private static RoutedUICommand _ToggleNumbering;

		// Token: 0x040026C9 RID: 9929
		private static RoutedUICommand _InsertTable;

		// Token: 0x040026CA RID: 9930
		private static RoutedUICommand _InsertRows;

		// Token: 0x040026CB RID: 9931
		private static RoutedUICommand _InsertColumns;

		// Token: 0x040026CC RID: 9932
		private static RoutedUICommand _DeleteRows;

		// Token: 0x040026CD RID: 9933
		private static RoutedUICommand _DeleteColumns;

		// Token: 0x040026CE RID: 9934
		private static RoutedUICommand _MergeCells;

		// Token: 0x040026CF RID: 9935
		private static RoutedUICommand _SplitCell;

		// Token: 0x040026D0 RID: 9936
		private static RoutedUICommand _CorrectSpellingError;

		// Token: 0x040026D1 RID: 9937
		private static RoutedUICommand _IgnoreSpellingError;
	}
}
