using System;
using System.Collections.Generic;
using System.IO;

namespace System.Windows.Documents
{
	// Token: 0x0200062F RID: 1583
	internal interface ITextRange
	{
		// Token: 0x06004E74 RID: 20084
		bool Contains(ITextPointer position);

		// Token: 0x06004E75 RID: 20085
		void Select(ITextPointer position1, ITextPointer position2);

		// Token: 0x06004E76 RID: 20086
		void SelectWord(ITextPointer position);

		// Token: 0x06004E77 RID: 20087
		void SelectParagraph(ITextPointer position);

		// Token: 0x06004E78 RID: 20088
		void ApplyTypingHeuristics(bool overType);

		// Token: 0x06004E79 RID: 20089
		object GetPropertyValue(DependencyProperty formattingProperty);

		// Token: 0x06004E7A RID: 20090
		UIElement GetUIElementSelected();

		// Token: 0x06004E7B RID: 20091
		bool CanSave(string dataFormat);

		// Token: 0x06004E7C RID: 20092
		void Save(Stream stream, string dataFormat);

		// Token: 0x06004E7D RID: 20093
		void Save(Stream stream, string dataFormat, bool preserveTextElements);

		// Token: 0x06004E7E RID: 20094
		void BeginChange();

		// Token: 0x06004E7F RID: 20095
		void BeginChangeNoUndo();

		// Token: 0x06004E80 RID: 20096
		void EndChange();

		// Token: 0x06004E81 RID: 20097
		void EndChange(bool disableScroll, bool skipEvents);

		// Token: 0x06004E82 RID: 20098
		IDisposable DeclareChangeBlock();

		// Token: 0x06004E83 RID: 20099
		IDisposable DeclareChangeBlock(bool disableScroll);

		// Token: 0x06004E84 RID: 20100
		void NotifyChanged(bool disableScroll, bool skipEvents);

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06004E85 RID: 20101
		bool IgnoreTextUnitBoundaries { get; }

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06004E86 RID: 20102
		ITextPointer Start { get; }

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06004E87 RID: 20103
		ITextPointer End { get; }

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06004E88 RID: 20104
		bool IsEmpty { get; }

		// Token: 0x17001231 RID: 4657
		// (get) Token: 0x06004E89 RID: 20105
		List<TextSegment> TextSegments { get; }

		// Token: 0x17001232 RID: 4658
		// (get) Token: 0x06004E8A RID: 20106
		bool HasConcreteTextContainer { get; }

		// Token: 0x17001233 RID: 4659
		// (get) Token: 0x06004E8B RID: 20107
		// (set) Token: 0x06004E8C RID: 20108
		string Text { get; set; }

		// Token: 0x17001234 RID: 4660
		// (get) Token: 0x06004E8D RID: 20109
		string Xml { get; }

		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06004E8E RID: 20110
		bool IsTableCellRange { get; }

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06004E8F RID: 20111
		int ChangeBlockLevel { get; }

		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06004E90 RID: 20112
		// (remove) Token: 0x06004E91 RID: 20113
		event EventHandler Changed;

		// Token: 0x06004E92 RID: 20114
		void FireChanged();

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06004E93 RID: 20115
		// (set) Token: 0x06004E94 RID: 20116
		uint _ContentGeneration { get; set; }

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x06004E95 RID: 20117
		// (set) Token: 0x06004E96 RID: 20118
		bool _IsTableCellRange { get; set; }

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x06004E97 RID: 20119
		// (set) Token: 0x06004E98 RID: 20120
		List<TextSegment> _TextSegments { get; set; }

		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06004E99 RID: 20121
		// (set) Token: 0x06004E9A RID: 20122
		int _ChangeBlockLevel { get; set; }

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06004E9B RID: 20123
		// (set) Token: 0x06004E9C RID: 20124
		ChangeBlockUndoRecord _ChangeBlockUndoRecord { get; set; }

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06004E9D RID: 20125
		// (set) Token: 0x06004E9E RID: 20126
		bool _IsChanged { get; set; }
	}
}
