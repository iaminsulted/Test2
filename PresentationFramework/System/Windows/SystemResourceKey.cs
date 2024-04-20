using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using MS.Internal.KnownBoxes;

namespace System.Windows
{
	// Token: 0x020003BA RID: 954
	[TypeConverter(typeof(SystemKeyConverter))]
	internal class SystemResourceKey : ResourceKey
	{
		// Token: 0x06002811 RID: 10257 RVA: 0x0019236C File Offset: 0x0019136C
		internal static short GetSystemResourceKeyIdFromBamlId(short bamlId, out bool isKey)
		{
			isKey = true;
			if (bamlId > 232 && bamlId < 464)
			{
				bamlId -= 232;
				isKey = false;
			}
			else if (bamlId > 464 && bamlId < 467)
			{
				bamlId -= 231;
			}
			else if (bamlId > 467 && bamlId < 470)
			{
				bamlId -= 234;
				isKey = false;
			}
			return bamlId;
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x001923D8 File Offset: 0x001913D8
		internal static short GetBamlIdBasedOnSystemResourceKeyId(Type targetType, string memberName)
		{
			short result = 0;
			bool flag = false;
			bool flag2 = true;
			SystemResourceKeyID systemResourceKeyID = SystemResourceKeyID.InternalSystemColorsStart;
			string text;
			if (memberName.EndsWith("Key", false, TypeConverterHelper.InvariantEnglishUS))
			{
				text = memberName.Remove(memberName.Length - 3);
				if (KnownTypes.Types[403] == targetType || KnownTypes.Types[669] == targetType || KnownTypes.Types[604] == targetType)
				{
					text = targetType.Name + text;
				}
				flag = true;
			}
			else
			{
				text = memberName;
			}
			try
			{
				systemResourceKeyID = (SystemResourceKeyID)Enum.Parse(typeof(SystemResourceKeyID), text);
			}
			catch (ArgumentException)
			{
				flag2 = false;
			}
			if (flag2)
			{
				if ((short)systemResourceKeyID > 233 && (short)systemResourceKeyID < 236)
				{
					if (flag)
					{
						result = -((short)systemResourceKeyID - 233 + 464);
					}
					else
					{
						result = -((short)systemResourceKeyID - 233 + 467);
					}
				}
				else if (flag)
				{
					result = -(short)systemResourceKeyID;
				}
				else
				{
					result = -((short)systemResourceKeyID + 232);
				}
			}
			return result;
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x001924F8 File Offset: 0x001914F8
		internal object Resource
		{
			get
			{
				switch (this._id)
				{
				case SystemResourceKeyID.ActiveBorderBrush:
					return SystemColors.ActiveBorderBrush;
				case SystemResourceKeyID.ActiveCaptionBrush:
					return SystemColors.ActiveCaptionBrush;
				case SystemResourceKeyID.ActiveCaptionTextBrush:
					return SystemColors.ActiveCaptionTextBrush;
				case SystemResourceKeyID.AppWorkspaceBrush:
					return SystemColors.AppWorkspaceBrush;
				case SystemResourceKeyID.ControlBrush:
					return SystemColors.ControlBrush;
				case SystemResourceKeyID.ControlDarkBrush:
					return SystemColors.ControlDarkBrush;
				case SystemResourceKeyID.ControlDarkDarkBrush:
					return SystemColors.ControlDarkDarkBrush;
				case SystemResourceKeyID.ControlLightBrush:
					return SystemColors.ControlLightBrush;
				case SystemResourceKeyID.ControlLightLightBrush:
					return SystemColors.ControlLightLightBrush;
				case SystemResourceKeyID.ControlTextBrush:
					return SystemColors.ControlTextBrush;
				case SystemResourceKeyID.DesktopBrush:
					return SystemColors.DesktopBrush;
				case SystemResourceKeyID.GradientActiveCaptionBrush:
					return SystemColors.GradientActiveCaptionBrush;
				case SystemResourceKeyID.GradientInactiveCaptionBrush:
					return SystemColors.GradientInactiveCaptionBrush;
				case SystemResourceKeyID.GrayTextBrush:
					return SystemColors.GrayTextBrush;
				case SystemResourceKeyID.HighlightBrush:
					return SystemColors.HighlightBrush;
				case SystemResourceKeyID.HighlightTextBrush:
					return SystemColors.HighlightTextBrush;
				case SystemResourceKeyID.HotTrackBrush:
					return SystemColors.HotTrackBrush;
				case SystemResourceKeyID.InactiveBorderBrush:
					return SystemColors.InactiveBorderBrush;
				case SystemResourceKeyID.InactiveCaptionBrush:
					return SystemColors.InactiveCaptionBrush;
				case SystemResourceKeyID.InactiveCaptionTextBrush:
					return SystemColors.InactiveCaptionTextBrush;
				case SystemResourceKeyID.InfoBrush:
					return SystemColors.InfoBrush;
				case SystemResourceKeyID.InfoTextBrush:
					return SystemColors.InfoTextBrush;
				case SystemResourceKeyID.MenuBrush:
					return SystemColors.MenuBrush;
				case SystemResourceKeyID.MenuBarBrush:
					return SystemColors.MenuBarBrush;
				case SystemResourceKeyID.MenuHighlightBrush:
					return SystemColors.MenuHighlightBrush;
				case SystemResourceKeyID.MenuTextBrush:
					return SystemColors.MenuTextBrush;
				case SystemResourceKeyID.ScrollBarBrush:
					return SystemColors.ScrollBarBrush;
				case SystemResourceKeyID.WindowBrush:
					return SystemColors.WindowBrush;
				case SystemResourceKeyID.WindowFrameBrush:
					return SystemColors.WindowFrameBrush;
				case SystemResourceKeyID.WindowTextBrush:
					return SystemColors.WindowTextBrush;
				case SystemResourceKeyID.ActiveBorderColor:
					return SystemColors.ActiveBorderColor;
				case SystemResourceKeyID.ActiveCaptionColor:
					return SystemColors.ActiveCaptionColor;
				case SystemResourceKeyID.ActiveCaptionTextColor:
					return SystemColors.ActiveCaptionTextColor;
				case SystemResourceKeyID.AppWorkspaceColor:
					return SystemColors.AppWorkspaceColor;
				case SystemResourceKeyID.ControlColor:
					return SystemColors.ControlColor;
				case SystemResourceKeyID.ControlDarkColor:
					return SystemColors.ControlDarkColor;
				case SystemResourceKeyID.ControlDarkDarkColor:
					return SystemColors.ControlDarkDarkColor;
				case SystemResourceKeyID.ControlLightColor:
					return SystemColors.ControlLightColor;
				case SystemResourceKeyID.ControlLightLightColor:
					return SystemColors.ControlLightLightColor;
				case SystemResourceKeyID.ControlTextColor:
					return SystemColors.ControlTextColor;
				case SystemResourceKeyID.DesktopColor:
					return SystemColors.DesktopColor;
				case SystemResourceKeyID.GradientActiveCaptionColor:
					return SystemColors.GradientActiveCaptionColor;
				case SystemResourceKeyID.GradientInactiveCaptionColor:
					return SystemColors.GradientInactiveCaptionColor;
				case SystemResourceKeyID.GrayTextColor:
					return SystemColors.GrayTextColor;
				case SystemResourceKeyID.HighlightColor:
					return SystemColors.HighlightColor;
				case SystemResourceKeyID.HighlightTextColor:
					return SystemColors.HighlightTextColor;
				case SystemResourceKeyID.HotTrackColor:
					return SystemColors.HotTrackColor;
				case SystemResourceKeyID.InactiveBorderColor:
					return SystemColors.InactiveBorderColor;
				case SystemResourceKeyID.InactiveCaptionColor:
					return SystemColors.InactiveCaptionColor;
				case SystemResourceKeyID.InactiveCaptionTextColor:
					return SystemColors.InactiveCaptionTextColor;
				case SystemResourceKeyID.InfoColor:
					return SystemColors.InfoColor;
				case SystemResourceKeyID.InfoTextColor:
					return SystemColors.InfoTextColor;
				case SystemResourceKeyID.MenuColor:
					return SystemColors.MenuColor;
				case SystemResourceKeyID.MenuBarColor:
					return SystemColors.MenuBarColor;
				case SystemResourceKeyID.MenuHighlightColor:
					return SystemColors.MenuHighlightColor;
				case SystemResourceKeyID.MenuTextColor:
					return SystemColors.MenuTextColor;
				case SystemResourceKeyID.ScrollBarColor:
					return SystemColors.ScrollBarColor;
				case SystemResourceKeyID.WindowColor:
					return SystemColors.WindowColor;
				case SystemResourceKeyID.WindowFrameColor:
					return SystemColors.WindowFrameColor;
				case SystemResourceKeyID.WindowTextColor:
					return SystemColors.WindowTextColor;
				case SystemResourceKeyID.CaptionFontSize:
					return SystemFonts.CaptionFontSize;
				case SystemResourceKeyID.CaptionFontFamily:
					return SystemFonts.CaptionFontFamily;
				case SystemResourceKeyID.CaptionFontStyle:
					return SystemFonts.CaptionFontStyle;
				case SystemResourceKeyID.CaptionFontWeight:
					return SystemFonts.CaptionFontWeight;
				case SystemResourceKeyID.CaptionFontTextDecorations:
					return SystemFonts.CaptionFontTextDecorations;
				case SystemResourceKeyID.SmallCaptionFontSize:
					return SystemFonts.SmallCaptionFontSize;
				case SystemResourceKeyID.SmallCaptionFontFamily:
					return SystemFonts.SmallCaptionFontFamily;
				case SystemResourceKeyID.SmallCaptionFontStyle:
					return SystemFonts.SmallCaptionFontStyle;
				case SystemResourceKeyID.SmallCaptionFontWeight:
					return SystemFonts.SmallCaptionFontWeight;
				case SystemResourceKeyID.SmallCaptionFontTextDecorations:
					return SystemFonts.SmallCaptionFontTextDecorations;
				case SystemResourceKeyID.MenuFontSize:
					return SystemFonts.MenuFontSize;
				case SystemResourceKeyID.MenuFontFamily:
					return SystemFonts.MenuFontFamily;
				case SystemResourceKeyID.MenuFontStyle:
					return SystemFonts.MenuFontStyle;
				case SystemResourceKeyID.MenuFontWeight:
					return SystemFonts.MenuFontWeight;
				case SystemResourceKeyID.MenuFontTextDecorations:
					return SystemFonts.MenuFontTextDecorations;
				case SystemResourceKeyID.StatusFontSize:
					return SystemFonts.StatusFontSize;
				case SystemResourceKeyID.StatusFontFamily:
					return SystemFonts.StatusFontFamily;
				case SystemResourceKeyID.StatusFontStyle:
					return SystemFonts.StatusFontStyle;
				case SystemResourceKeyID.StatusFontWeight:
					return SystemFonts.StatusFontWeight;
				case SystemResourceKeyID.StatusFontTextDecorations:
					return SystemFonts.StatusFontTextDecorations;
				case SystemResourceKeyID.MessageFontSize:
					return SystemFonts.MessageFontSize;
				case SystemResourceKeyID.MessageFontFamily:
					return SystemFonts.MessageFontFamily;
				case SystemResourceKeyID.MessageFontStyle:
					return SystemFonts.MessageFontStyle;
				case SystemResourceKeyID.MessageFontWeight:
					return SystemFonts.MessageFontWeight;
				case SystemResourceKeyID.MessageFontTextDecorations:
					return SystemFonts.MessageFontTextDecorations;
				case SystemResourceKeyID.IconFontSize:
					return SystemFonts.IconFontSize;
				case SystemResourceKeyID.IconFontFamily:
					return SystemFonts.IconFontFamily;
				case SystemResourceKeyID.IconFontStyle:
					return SystemFonts.IconFontStyle;
				case SystemResourceKeyID.IconFontWeight:
					return SystemFonts.IconFontWeight;
				case SystemResourceKeyID.IconFontTextDecorations:
					return SystemFonts.IconFontTextDecorations;
				case SystemResourceKeyID.ThinHorizontalBorderHeight:
					return SystemParameters.ThinHorizontalBorderHeight;
				case SystemResourceKeyID.ThinVerticalBorderWidth:
					return SystemParameters.ThinVerticalBorderWidth;
				case SystemResourceKeyID.CursorWidth:
					return SystemParameters.CursorWidth;
				case SystemResourceKeyID.CursorHeight:
					return SystemParameters.CursorHeight;
				case SystemResourceKeyID.ThickHorizontalBorderHeight:
					return SystemParameters.ThickHorizontalBorderHeight;
				case SystemResourceKeyID.ThickVerticalBorderWidth:
					return SystemParameters.ThickVerticalBorderWidth;
				case SystemResourceKeyID.FixedFrameHorizontalBorderHeight:
					return SystemParameters.FixedFrameHorizontalBorderHeight;
				case SystemResourceKeyID.FixedFrameVerticalBorderWidth:
					return SystemParameters.FixedFrameVerticalBorderWidth;
				case SystemResourceKeyID.FocusHorizontalBorderHeight:
					return SystemParameters.FocusHorizontalBorderHeight;
				case SystemResourceKeyID.FocusVerticalBorderWidth:
					return SystemParameters.FocusVerticalBorderWidth;
				case SystemResourceKeyID.FullPrimaryScreenWidth:
					return SystemParameters.FullPrimaryScreenWidth;
				case SystemResourceKeyID.FullPrimaryScreenHeight:
					return SystemParameters.FullPrimaryScreenHeight;
				case SystemResourceKeyID.HorizontalScrollBarButtonWidth:
					return SystemParameters.HorizontalScrollBarButtonWidth;
				case SystemResourceKeyID.HorizontalScrollBarHeight:
					return SystemParameters.HorizontalScrollBarHeight;
				case SystemResourceKeyID.HorizontalScrollBarThumbWidth:
					return SystemParameters.HorizontalScrollBarThumbWidth;
				case SystemResourceKeyID.IconWidth:
					return SystemParameters.IconWidth;
				case SystemResourceKeyID.IconHeight:
					return SystemParameters.IconHeight;
				case SystemResourceKeyID.IconGridWidth:
					return SystemParameters.IconGridWidth;
				case SystemResourceKeyID.IconGridHeight:
					return SystemParameters.IconGridHeight;
				case SystemResourceKeyID.MaximizedPrimaryScreenWidth:
					return SystemParameters.MaximizedPrimaryScreenWidth;
				case SystemResourceKeyID.MaximizedPrimaryScreenHeight:
					return SystemParameters.MaximizedPrimaryScreenHeight;
				case SystemResourceKeyID.MaximumWindowTrackWidth:
					return SystemParameters.MaximumWindowTrackWidth;
				case SystemResourceKeyID.MaximumWindowTrackHeight:
					return SystemParameters.MaximumWindowTrackHeight;
				case SystemResourceKeyID.MenuCheckmarkWidth:
					return SystemParameters.MenuCheckmarkWidth;
				case SystemResourceKeyID.MenuCheckmarkHeight:
					return SystemParameters.MenuCheckmarkHeight;
				case SystemResourceKeyID.MenuButtonWidth:
					return SystemParameters.MenuButtonWidth;
				case SystemResourceKeyID.MenuButtonHeight:
					return SystemParameters.MenuButtonHeight;
				case SystemResourceKeyID.MinimumWindowWidth:
					return SystemParameters.MinimumWindowWidth;
				case SystemResourceKeyID.MinimumWindowHeight:
					return SystemParameters.MinimumWindowHeight;
				case SystemResourceKeyID.MinimizedWindowWidth:
					return SystemParameters.MinimizedWindowWidth;
				case SystemResourceKeyID.MinimizedWindowHeight:
					return SystemParameters.MinimizedWindowHeight;
				case SystemResourceKeyID.MinimizedGridWidth:
					return SystemParameters.MinimizedGridWidth;
				case SystemResourceKeyID.MinimizedGridHeight:
					return SystemParameters.MinimizedGridHeight;
				case SystemResourceKeyID.MinimumWindowTrackWidth:
					return SystemParameters.MinimumWindowTrackWidth;
				case SystemResourceKeyID.MinimumWindowTrackHeight:
					return SystemParameters.MinimumWindowTrackHeight;
				case SystemResourceKeyID.PrimaryScreenWidth:
					return SystemParameters.PrimaryScreenWidth;
				case SystemResourceKeyID.PrimaryScreenHeight:
					return SystemParameters.PrimaryScreenHeight;
				case SystemResourceKeyID.WindowCaptionButtonWidth:
					return SystemParameters.WindowCaptionButtonWidth;
				case SystemResourceKeyID.WindowCaptionButtonHeight:
					return SystemParameters.WindowCaptionButtonHeight;
				case SystemResourceKeyID.ResizeFrameHorizontalBorderHeight:
					return SystemParameters.ResizeFrameHorizontalBorderHeight;
				case SystemResourceKeyID.ResizeFrameVerticalBorderWidth:
					return SystemParameters.ResizeFrameVerticalBorderWidth;
				case SystemResourceKeyID.SmallIconWidth:
					return SystemParameters.SmallIconWidth;
				case SystemResourceKeyID.SmallIconHeight:
					return SystemParameters.SmallIconHeight;
				case SystemResourceKeyID.SmallWindowCaptionButtonWidth:
					return SystemParameters.SmallWindowCaptionButtonWidth;
				case SystemResourceKeyID.SmallWindowCaptionButtonHeight:
					return SystemParameters.SmallWindowCaptionButtonHeight;
				case SystemResourceKeyID.VirtualScreenWidth:
					return SystemParameters.VirtualScreenWidth;
				case SystemResourceKeyID.VirtualScreenHeight:
					return SystemParameters.VirtualScreenHeight;
				case SystemResourceKeyID.VerticalScrollBarWidth:
					return SystemParameters.VerticalScrollBarWidth;
				case SystemResourceKeyID.VerticalScrollBarButtonHeight:
					return SystemParameters.VerticalScrollBarButtonHeight;
				case SystemResourceKeyID.WindowCaptionHeight:
					return SystemParameters.WindowCaptionHeight;
				case SystemResourceKeyID.KanjiWindowHeight:
					return SystemParameters.KanjiWindowHeight;
				case SystemResourceKeyID.MenuBarHeight:
					return SystemParameters.MenuBarHeight;
				case SystemResourceKeyID.SmallCaptionHeight:
					return SystemParameters.SmallCaptionHeight;
				case SystemResourceKeyID.VerticalScrollBarThumbHeight:
					return SystemParameters.VerticalScrollBarThumbHeight;
				case SystemResourceKeyID.IsImmEnabled:
					return BooleanBoxes.Box(SystemParameters.IsImmEnabled);
				case SystemResourceKeyID.IsMediaCenter:
					return BooleanBoxes.Box(SystemParameters.IsMediaCenter);
				case SystemResourceKeyID.IsMenuDropRightAligned:
					return BooleanBoxes.Box(SystemParameters.IsMenuDropRightAligned);
				case SystemResourceKeyID.IsMiddleEastEnabled:
					return BooleanBoxes.Box(SystemParameters.IsMiddleEastEnabled);
				case SystemResourceKeyID.IsMousePresent:
					return BooleanBoxes.Box(SystemParameters.IsMousePresent);
				case SystemResourceKeyID.IsMouseWheelPresent:
					return BooleanBoxes.Box(SystemParameters.IsMouseWheelPresent);
				case SystemResourceKeyID.IsPenWindows:
					return BooleanBoxes.Box(SystemParameters.IsPenWindows);
				case SystemResourceKeyID.IsRemotelyControlled:
					return BooleanBoxes.Box(SystemParameters.IsRemotelyControlled);
				case SystemResourceKeyID.IsRemoteSession:
					return BooleanBoxes.Box(SystemParameters.IsRemoteSession);
				case SystemResourceKeyID.ShowSounds:
					return BooleanBoxes.Box(SystemParameters.ShowSounds);
				case SystemResourceKeyID.IsSlowMachine:
					return BooleanBoxes.Box(SystemParameters.IsSlowMachine);
				case SystemResourceKeyID.SwapButtons:
					return BooleanBoxes.Box(SystemParameters.SwapButtons);
				case SystemResourceKeyID.IsTabletPC:
					return BooleanBoxes.Box(SystemParameters.IsTabletPC);
				case SystemResourceKeyID.VirtualScreenLeft:
					return SystemParameters.VirtualScreenLeft;
				case SystemResourceKeyID.VirtualScreenTop:
					return SystemParameters.VirtualScreenTop;
				case SystemResourceKeyID.FocusBorderWidth:
					return SystemParameters.FocusBorderWidth;
				case SystemResourceKeyID.FocusBorderHeight:
					return SystemParameters.FocusBorderHeight;
				case SystemResourceKeyID.HighContrast:
					return BooleanBoxes.Box(SystemParameters.HighContrast);
				case SystemResourceKeyID.DropShadow:
					return BooleanBoxes.Box(SystemParameters.DropShadow);
				case SystemResourceKeyID.FlatMenu:
					return BooleanBoxes.Box(SystemParameters.FlatMenu);
				case SystemResourceKeyID.WorkArea:
					return SystemParameters.WorkArea;
				case SystemResourceKeyID.IconHorizontalSpacing:
					return SystemParameters.IconHorizontalSpacing;
				case SystemResourceKeyID.IconVerticalSpacing:
					return SystemParameters.IconVerticalSpacing;
				case SystemResourceKeyID.IconTitleWrap:
					return SystemParameters.IconTitleWrap;
				case SystemResourceKeyID.KeyboardCues:
					return BooleanBoxes.Box(SystemParameters.KeyboardCues);
				case SystemResourceKeyID.KeyboardDelay:
					return SystemParameters.KeyboardDelay;
				case SystemResourceKeyID.KeyboardPreference:
					return BooleanBoxes.Box(SystemParameters.KeyboardPreference);
				case SystemResourceKeyID.KeyboardSpeed:
					return SystemParameters.KeyboardSpeed;
				case SystemResourceKeyID.SnapToDefaultButton:
					return BooleanBoxes.Box(SystemParameters.SnapToDefaultButton);
				case SystemResourceKeyID.WheelScrollLines:
					return SystemParameters.WheelScrollLines;
				case SystemResourceKeyID.MouseHoverTime:
					return SystemParameters.MouseHoverTime;
				case SystemResourceKeyID.MouseHoverHeight:
					return SystemParameters.MouseHoverHeight;
				case SystemResourceKeyID.MouseHoverWidth:
					return SystemParameters.MouseHoverWidth;
				case SystemResourceKeyID.MenuDropAlignment:
					return BooleanBoxes.Box(SystemParameters.MenuDropAlignment);
				case SystemResourceKeyID.MenuFade:
					return BooleanBoxes.Box(SystemParameters.MenuFade);
				case SystemResourceKeyID.MenuShowDelay:
					return SystemParameters.MenuShowDelay;
				case SystemResourceKeyID.ComboBoxAnimation:
					return BooleanBoxes.Box(SystemParameters.ComboBoxAnimation);
				case SystemResourceKeyID.ClientAreaAnimation:
					return BooleanBoxes.Box(SystemParameters.ClientAreaAnimation);
				case SystemResourceKeyID.CursorShadow:
					return BooleanBoxes.Box(SystemParameters.CursorShadow);
				case SystemResourceKeyID.GradientCaptions:
					return BooleanBoxes.Box(SystemParameters.GradientCaptions);
				case SystemResourceKeyID.HotTracking:
					return BooleanBoxes.Box(SystemParameters.HotTracking);
				case SystemResourceKeyID.ListBoxSmoothScrolling:
					return BooleanBoxes.Box(SystemParameters.ListBoxSmoothScrolling);
				case SystemResourceKeyID.MenuAnimation:
					return BooleanBoxes.Box(SystemParameters.MenuAnimation);
				case SystemResourceKeyID.SelectionFade:
					return BooleanBoxes.Box(SystemParameters.SelectionFade);
				case SystemResourceKeyID.StylusHotTracking:
					return BooleanBoxes.Box(SystemParameters.StylusHotTracking);
				case SystemResourceKeyID.ToolTipAnimation:
					return BooleanBoxes.Box(SystemParameters.ToolTipAnimation);
				case SystemResourceKeyID.ToolTipFade:
					return BooleanBoxes.Box(SystemParameters.ToolTipFade);
				case SystemResourceKeyID.UIEffects:
					return BooleanBoxes.Box(SystemParameters.UIEffects);
				case SystemResourceKeyID.MinimizeAnimation:
					return BooleanBoxes.Box(SystemParameters.MinimizeAnimation);
				case SystemResourceKeyID.Border:
					return SystemParameters.Border;
				case SystemResourceKeyID.CaretWidth:
					return SystemParameters.CaretWidth;
				case SystemResourceKeyID.ForegroundFlashCount:
					return SystemParameters.ForegroundFlashCount;
				case SystemResourceKeyID.DragFullWindows:
					return BooleanBoxes.Box(SystemParameters.DragFullWindows);
				case SystemResourceKeyID.BorderWidth:
					return SystemParameters.BorderWidth;
				case SystemResourceKeyID.ScrollWidth:
					return SystemParameters.ScrollWidth;
				case SystemResourceKeyID.ScrollHeight:
					return SystemParameters.ScrollHeight;
				case SystemResourceKeyID.CaptionWidth:
					return SystemParameters.CaptionWidth;
				case SystemResourceKeyID.CaptionHeight:
					return SystemParameters.CaptionHeight;
				case SystemResourceKeyID.SmallCaptionWidth:
					return SystemParameters.SmallCaptionWidth;
				case SystemResourceKeyID.MenuWidth:
					return SystemParameters.MenuWidth;
				case SystemResourceKeyID.MenuHeight:
					return SystemParameters.MenuHeight;
				case SystemResourceKeyID.ComboBoxPopupAnimation:
					return SystemParameters.ComboBoxPopupAnimation;
				case SystemResourceKeyID.MenuPopupAnimation:
					return SystemParameters.MenuPopupAnimation;
				case SystemResourceKeyID.ToolTipPopupAnimation:
					return SystemParameters.ToolTipPopupAnimation;
				case SystemResourceKeyID.PowerLineStatus:
					return SystemParameters.PowerLineStatus;
				case SystemResourceKeyID.InactiveSelectionHighlightBrush:
					return SystemColors.InactiveSelectionHighlightBrush;
				case SystemResourceKeyID.InactiveSelectionHighlightTextBrush:
					return SystemColors.InactiveSelectionHighlightTextBrush;
				}
				return null;
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x001930FC File Offset: 0x001920FC
		internal static ResourceKey GetResourceKey(short id)
		{
			switch (id)
			{
			case 1:
				return SystemColors.ActiveBorderBrushKey;
			case 2:
				return SystemColors.ActiveCaptionBrushKey;
			case 3:
				return SystemColors.ActiveCaptionTextBrushKey;
			case 4:
				return SystemColors.AppWorkspaceBrushKey;
			case 5:
				return SystemColors.ControlBrushKey;
			case 6:
				return SystemColors.ControlDarkBrushKey;
			case 7:
				return SystemColors.ControlDarkDarkBrushKey;
			case 8:
				return SystemColors.ControlLightBrushKey;
			case 9:
				return SystemColors.ControlLightLightBrushKey;
			case 10:
				return SystemColors.ControlTextBrushKey;
			case 11:
				return SystemColors.DesktopBrushKey;
			case 12:
				return SystemColors.GradientActiveCaptionBrushKey;
			case 13:
				return SystemColors.GradientInactiveCaptionBrushKey;
			case 14:
				return SystemColors.GrayTextBrushKey;
			case 15:
				return SystemColors.HighlightBrushKey;
			case 16:
				return SystemColors.HighlightTextBrushKey;
			case 17:
				return SystemColors.HotTrackBrushKey;
			case 18:
				return SystemColors.InactiveBorderBrushKey;
			case 19:
				return SystemColors.InactiveCaptionBrushKey;
			case 20:
				return SystemColors.InactiveCaptionTextBrushKey;
			case 21:
				return SystemColors.InfoBrushKey;
			case 22:
				return SystemColors.InfoTextBrushKey;
			case 23:
				return SystemColors.MenuBrushKey;
			case 24:
				return SystemColors.MenuBarBrushKey;
			case 25:
				return SystemColors.MenuHighlightBrushKey;
			case 26:
				return SystemColors.MenuTextBrushKey;
			case 27:
				return SystemColors.ScrollBarBrushKey;
			case 28:
				return SystemColors.WindowBrushKey;
			case 29:
				return SystemColors.WindowFrameBrushKey;
			case 30:
				return SystemColors.WindowTextBrushKey;
			case 31:
				return SystemColors.ActiveBorderColorKey;
			case 32:
				return SystemColors.ActiveCaptionColorKey;
			case 33:
				return SystemColors.ActiveCaptionTextColorKey;
			case 34:
				return SystemColors.AppWorkspaceColorKey;
			case 35:
				return SystemColors.ControlColorKey;
			case 36:
				return SystemColors.ControlDarkColorKey;
			case 37:
				return SystemColors.ControlDarkDarkColorKey;
			case 38:
				return SystemColors.ControlLightColorKey;
			case 39:
				return SystemColors.ControlLightLightColorKey;
			case 40:
				return SystemColors.ControlTextColorKey;
			case 41:
				return SystemColors.DesktopColorKey;
			case 42:
				return SystemColors.GradientActiveCaptionColorKey;
			case 43:
				return SystemColors.GradientInactiveCaptionColorKey;
			case 44:
				return SystemColors.GrayTextColorKey;
			case 45:
				return SystemColors.HighlightColorKey;
			case 46:
				return SystemColors.HighlightTextColorKey;
			case 47:
				return SystemColors.HotTrackColorKey;
			case 48:
				return SystemColors.InactiveBorderColorKey;
			case 49:
				return SystemColors.InactiveCaptionColorKey;
			case 50:
				return SystemColors.InactiveCaptionTextColorKey;
			case 51:
				return SystemColors.InfoColorKey;
			case 52:
				return SystemColors.InfoTextColorKey;
			case 53:
				return SystemColors.MenuColorKey;
			case 54:
				return SystemColors.MenuBarColorKey;
			case 55:
				return SystemColors.MenuHighlightColorKey;
			case 56:
				return SystemColors.MenuTextColorKey;
			case 57:
				return SystemColors.ScrollBarColorKey;
			case 58:
				return SystemColors.WindowColorKey;
			case 59:
				return SystemColors.WindowFrameColorKey;
			case 60:
				return SystemColors.WindowTextColorKey;
			case 63:
				return SystemFonts.CaptionFontSizeKey;
			case 64:
				return SystemFonts.CaptionFontFamilyKey;
			case 65:
				return SystemFonts.CaptionFontStyleKey;
			case 66:
				return SystemFonts.CaptionFontWeightKey;
			case 67:
				return SystemFonts.CaptionFontTextDecorationsKey;
			case 68:
				return SystemFonts.SmallCaptionFontSizeKey;
			case 69:
				return SystemFonts.SmallCaptionFontFamilyKey;
			case 70:
				return SystemFonts.SmallCaptionFontStyleKey;
			case 71:
				return SystemFonts.SmallCaptionFontWeightKey;
			case 72:
				return SystemFonts.SmallCaptionFontTextDecorationsKey;
			case 73:
				return SystemFonts.MenuFontSizeKey;
			case 74:
				return SystemFonts.MenuFontFamilyKey;
			case 75:
				return SystemFonts.MenuFontStyleKey;
			case 76:
				return SystemFonts.MenuFontWeightKey;
			case 77:
				return SystemFonts.MenuFontTextDecorationsKey;
			case 78:
				return SystemFonts.StatusFontSizeKey;
			case 79:
				return SystemFonts.StatusFontFamilyKey;
			case 80:
				return SystemFonts.StatusFontStyleKey;
			case 81:
				return SystemFonts.StatusFontWeightKey;
			case 82:
				return SystemFonts.StatusFontTextDecorationsKey;
			case 83:
				return SystemFonts.MessageFontSizeKey;
			case 84:
				return SystemFonts.MessageFontFamilyKey;
			case 85:
				return SystemFonts.MessageFontStyleKey;
			case 86:
				return SystemFonts.MessageFontWeightKey;
			case 87:
				return SystemFonts.MessageFontTextDecorationsKey;
			case 88:
				return SystemFonts.IconFontSizeKey;
			case 89:
				return SystemFonts.IconFontFamilyKey;
			case 90:
				return SystemFonts.IconFontStyleKey;
			case 91:
				return SystemFonts.IconFontWeightKey;
			case 92:
				return SystemFonts.IconFontTextDecorationsKey;
			case 95:
				return SystemParameters.ThinHorizontalBorderHeightKey;
			case 96:
				return SystemParameters.ThinVerticalBorderWidthKey;
			case 97:
				return SystemParameters.CursorWidthKey;
			case 98:
				return SystemParameters.CursorHeightKey;
			case 99:
				return SystemParameters.ThickHorizontalBorderHeightKey;
			case 100:
				return SystemParameters.ThickVerticalBorderWidthKey;
			case 101:
				return SystemParameters.FixedFrameHorizontalBorderHeightKey;
			case 102:
				return SystemParameters.FixedFrameVerticalBorderWidthKey;
			case 103:
				return SystemParameters.FocusHorizontalBorderHeightKey;
			case 104:
				return SystemParameters.FocusVerticalBorderWidthKey;
			case 105:
				return SystemParameters.FullPrimaryScreenWidthKey;
			case 106:
				return SystemParameters.FullPrimaryScreenHeightKey;
			case 107:
				return SystemParameters.HorizontalScrollBarButtonWidthKey;
			case 108:
				return SystemParameters.HorizontalScrollBarHeightKey;
			case 109:
				return SystemParameters.HorizontalScrollBarThumbWidthKey;
			case 110:
				return SystemParameters.IconWidthKey;
			case 111:
				return SystemParameters.IconHeightKey;
			case 112:
				return SystemParameters.IconGridWidthKey;
			case 113:
				return SystemParameters.IconGridHeightKey;
			case 114:
				return SystemParameters.MaximizedPrimaryScreenWidthKey;
			case 115:
				return SystemParameters.MaximizedPrimaryScreenHeightKey;
			case 116:
				return SystemParameters.MaximumWindowTrackWidthKey;
			case 117:
				return SystemParameters.MaximumWindowTrackHeightKey;
			case 118:
				return SystemParameters.MenuCheckmarkWidthKey;
			case 119:
				return SystemParameters.MenuCheckmarkHeightKey;
			case 120:
				return SystemParameters.MenuButtonWidthKey;
			case 121:
				return SystemParameters.MenuButtonHeightKey;
			case 122:
				return SystemParameters.MinimumWindowWidthKey;
			case 123:
				return SystemParameters.MinimumWindowHeightKey;
			case 124:
				return SystemParameters.MinimizedWindowWidthKey;
			case 125:
				return SystemParameters.MinimizedWindowHeightKey;
			case 126:
				return SystemParameters.MinimizedGridWidthKey;
			case 127:
				return SystemParameters.MinimizedGridHeightKey;
			case 128:
				return SystemParameters.MinimumWindowTrackWidthKey;
			case 129:
				return SystemParameters.MinimumWindowTrackHeightKey;
			case 130:
				return SystemParameters.PrimaryScreenWidthKey;
			case 131:
				return SystemParameters.PrimaryScreenHeightKey;
			case 132:
				return SystemParameters.WindowCaptionButtonWidthKey;
			case 133:
				return SystemParameters.WindowCaptionButtonHeightKey;
			case 134:
				return SystemParameters.ResizeFrameHorizontalBorderHeightKey;
			case 135:
				return SystemParameters.ResizeFrameVerticalBorderWidthKey;
			case 136:
				return SystemParameters.SmallIconWidthKey;
			case 137:
				return SystemParameters.SmallIconHeightKey;
			case 138:
				return SystemParameters.SmallWindowCaptionButtonWidthKey;
			case 139:
				return SystemParameters.SmallWindowCaptionButtonHeightKey;
			case 140:
				return SystemParameters.VirtualScreenWidthKey;
			case 141:
				return SystemParameters.VirtualScreenHeightKey;
			case 142:
				return SystemParameters.VerticalScrollBarWidthKey;
			case 143:
				return SystemParameters.VerticalScrollBarButtonHeightKey;
			case 144:
				return SystemParameters.WindowCaptionHeightKey;
			case 145:
				return SystemParameters.KanjiWindowHeightKey;
			case 146:
				return SystemParameters.MenuBarHeightKey;
			case 147:
				return SystemParameters.SmallCaptionHeightKey;
			case 148:
				return SystemParameters.VerticalScrollBarThumbHeightKey;
			case 149:
				return SystemParameters.IsImmEnabledKey;
			case 150:
				return SystemParameters.IsMediaCenterKey;
			case 151:
				return SystemParameters.IsMenuDropRightAlignedKey;
			case 152:
				return SystemParameters.IsMiddleEastEnabledKey;
			case 153:
				return SystemParameters.IsMousePresentKey;
			case 154:
				return SystemParameters.IsMouseWheelPresentKey;
			case 155:
				return SystemParameters.IsPenWindowsKey;
			case 156:
				return SystemParameters.IsRemotelyControlledKey;
			case 157:
				return SystemParameters.IsRemoteSessionKey;
			case 158:
				return SystemParameters.ShowSoundsKey;
			case 159:
				return SystemParameters.IsSlowMachineKey;
			case 160:
				return SystemParameters.SwapButtonsKey;
			case 161:
				return SystemParameters.IsTabletPCKey;
			case 162:
				return SystemParameters.VirtualScreenLeftKey;
			case 163:
				return SystemParameters.VirtualScreenTopKey;
			case 164:
				return SystemParameters.FocusBorderWidthKey;
			case 165:
				return SystemParameters.FocusBorderHeightKey;
			case 166:
				return SystemParameters.HighContrastKey;
			case 167:
				return SystemParameters.DropShadowKey;
			case 168:
				return SystemParameters.FlatMenuKey;
			case 169:
				return SystemParameters.WorkAreaKey;
			case 170:
				return SystemParameters.IconHorizontalSpacingKey;
			case 171:
				return SystemParameters.IconVerticalSpacingKey;
			case 172:
				return SystemParameters.IconTitleWrapKey;
			case 173:
				return SystemParameters.KeyboardCuesKey;
			case 174:
				return SystemParameters.KeyboardDelayKey;
			case 175:
				return SystemParameters.KeyboardPreferenceKey;
			case 176:
				return SystemParameters.KeyboardSpeedKey;
			case 177:
				return SystemParameters.SnapToDefaultButtonKey;
			case 178:
				return SystemParameters.WheelScrollLinesKey;
			case 179:
				return SystemParameters.MouseHoverTimeKey;
			case 180:
				return SystemParameters.MouseHoverHeightKey;
			case 181:
				return SystemParameters.MouseHoverWidthKey;
			case 182:
				return SystemParameters.MenuDropAlignmentKey;
			case 183:
				return SystemParameters.MenuFadeKey;
			case 184:
				return SystemParameters.MenuShowDelayKey;
			case 185:
				return SystemParameters.ComboBoxAnimationKey;
			case 186:
				return SystemParameters.ClientAreaAnimationKey;
			case 187:
				return SystemParameters.CursorShadowKey;
			case 188:
				return SystemParameters.GradientCaptionsKey;
			case 189:
				return SystemParameters.HotTrackingKey;
			case 190:
				return SystemParameters.ListBoxSmoothScrollingKey;
			case 191:
				return SystemParameters.MenuAnimationKey;
			case 192:
				return SystemParameters.SelectionFadeKey;
			case 193:
				return SystemParameters.StylusHotTrackingKey;
			case 194:
				return SystemParameters.ToolTipAnimationKey;
			case 195:
				return SystemParameters.ToolTipFadeKey;
			case 196:
				return SystemParameters.UIEffectsKey;
			case 197:
				return SystemParameters.MinimizeAnimationKey;
			case 198:
				return SystemParameters.BorderKey;
			case 199:
				return SystemParameters.CaretWidthKey;
			case 200:
				return SystemParameters.ForegroundFlashCountKey;
			case 201:
				return SystemParameters.DragFullWindowsKey;
			case 202:
				return SystemParameters.BorderWidthKey;
			case 203:
				return SystemParameters.ScrollWidthKey;
			case 204:
				return SystemParameters.ScrollHeightKey;
			case 205:
				return SystemParameters.CaptionWidthKey;
			case 206:
				return SystemParameters.CaptionHeightKey;
			case 207:
				return SystemParameters.SmallCaptionWidthKey;
			case 208:
				return SystemParameters.MenuWidthKey;
			case 209:
				return SystemParameters.MenuHeightKey;
			case 210:
				return SystemParameters.ComboBoxPopupAnimationKey;
			case 211:
				return SystemParameters.MenuPopupAnimationKey;
			case 212:
				return SystemParameters.ToolTipPopupAnimationKey;
			case 213:
				return SystemParameters.PowerLineStatusKey;
			case 215:
				return SystemParameters.FocusVisualStyleKey;
			case 216:
				return SystemParameters.NavigationChromeDownLevelStyleKey;
			case 217:
				return SystemParameters.NavigationChromeStyleKey;
			case 219:
				return MenuItem.SeparatorStyleKey;
			case 220:
				return GridView.GridViewScrollViewerStyleKey;
			case 221:
				return GridView.GridViewStyleKey;
			case 222:
				return GridView.GridViewItemContainerStyleKey;
			case 223:
				return StatusBar.SeparatorStyleKey;
			case 224:
				return ToolBar.ButtonStyleKey;
			case 225:
				return ToolBar.ToggleButtonStyleKey;
			case 226:
				return ToolBar.SeparatorStyleKey;
			case 227:
				return ToolBar.CheckBoxStyleKey;
			case 228:
				return ToolBar.RadioButtonStyleKey;
			case 229:
				return ToolBar.ComboBoxStyleKey;
			case 230:
				return ToolBar.TextBoxStyleKey;
			case 231:
				return ToolBar.MenuStyleKey;
			case 234:
				return SystemColors.InactiveSelectionHighlightBrushKey;
			case 235:
				return SystemColors.InactiveSelectionHighlightTextBrushKey;
			}
			return null;
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x00193A18 File Offset: 0x00192A18
		internal static ResourceKey GetSystemResourceKey(string keyName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(keyName);
			if (num <= 2045857616U)
			{
				if (num <= 705184480U)
				{
					if (num <= 461950501U)
					{
						if (num != 145556601U)
						{
							if (num == 461950501U)
							{
								if (keyName == "StatusBar.SeparatorStyleKey")
								{
									return SystemResourceKey.StatusBarSeparatorStyleKey;
								}
							}
						}
						else if (keyName == "DataGridComboBoxColumn.TextBlockComboBoxStyleKey")
						{
							return SystemResourceKey.DataGridComboBoxColumnTextBlockComboBoxStyleKey;
						}
					}
					else if (num != 493495474U)
					{
						if (num != 643203629U)
						{
							if (num == 705184480U)
							{
								if (keyName == "MenuItem.SeparatorStyleKey")
								{
									return SystemResourceKey.MenuItemSeparatorStyleKey;
								}
							}
						}
						else if (keyName == "ToolBar.RadioButtonStyleKey")
						{
							return SystemResourceKey.ToolBarRadioButtonStyleKey;
						}
					}
					else if (keyName == "DataGrid.FocusBorderBrushKey")
					{
						return SystemResourceKey.DataGridFocusBorderBrushKey;
					}
				}
				else if (num <= 1217509750U)
				{
					if (num != 1180275602U)
					{
						if (num == 1217509750U)
						{
							if (keyName == "SystemParameters.NavigationChromeStyleKey")
							{
								return SystemParameters.NavigationChromeStyleKey;
							}
						}
					}
					else if (keyName == "ToolBar.TextBoxStyleKey")
					{
						return SystemResourceKey.ToolBarTextBoxStyleKey;
					}
				}
				else if (num != 1247245809U)
				{
					if (num != 1661977326U)
					{
						if (num == 2045857616U)
						{
							if (keyName == "GridView.GridViewScrollViewerStyleKey")
							{
								return SystemResourceKey.GridViewScrollViewerStyleKey;
							}
						}
					}
					else if (keyName == "SystemParameters.FocusVisualStyleKey")
					{
						return SystemParameters.FocusVisualStyleKey;
					}
				}
				else if (keyName == "DataGridColumnHeader.ColumnHeaderDropSeparatorStyleKey")
				{
					return SystemResourceKey.DataGridColumnHeaderColumnHeaderDropSeparatorStyleKey;
				}
			}
			else if (num <= 2828802701U)
			{
				if (num <= 2379497950U)
				{
					if (num != 2175658447U)
					{
						if (num == 2379497950U)
						{
							if (keyName == "ToolBar.ToggleButtonStyleKey")
							{
								return SystemResourceKey.ToolBarToggleButtonStyleKey;
							}
						}
					}
					else if (keyName == "GridView.GridViewItemContainerStyleKey")
					{
						return SystemResourceKey.GridViewItemContainerStyleKey;
					}
				}
				else if (num != 2729588653U)
				{
					if (num != 2796646438U)
					{
						if (num == 2828802701U)
						{
							if (keyName == "ToolBar.SeparatorStyleKey")
							{
								return SystemResourceKey.ToolBarSeparatorStyleKey;
							}
						}
					}
					else if (keyName == "ToolBar.ButtonStyleKey")
					{
						return SystemResourceKey.ToolBarButtonStyleKey;
					}
				}
				else if (keyName == "ToolBar.MenuStyleKey")
				{
					return SystemResourceKey.ToolBarMenuStyleKey;
				}
			}
			else if (num <= 3227933789U)
			{
				if (num != 3124717228U)
				{
					if (num == 3227933789U)
					{
						if (keyName == "ToolBar.ComboBoxStyleKey")
						{
							return SystemResourceKey.ToolBarComboBoxStyleKey;
						}
					}
				}
				else if (keyName == "SystemParameters.NavigationChromeDownLevelStyleKey")
				{
					return SystemParameters.NavigationChromeDownLevelStyleKey;
				}
			}
			else if (num != 3609136063U)
			{
				if (num != 3884806943U)
				{
					if (num == 3928176313U)
					{
						if (keyName == "DataGridColumnHeader.ColumnFloatingHeaderStyleKey")
						{
							return SystemResourceKey.DataGridColumnHeaderColumnFloatingHeaderStyleKey;
						}
					}
				}
				else if (keyName == "GridView.GridViewStyleKey")
				{
					return SystemResourceKey.GridViewStyleKey;
				}
			}
			else if (keyName == "ToolBar.CheckBoxStyleKey")
			{
				return SystemResourceKey.ToolBarCheckBoxStyleKey;
			}
			return null;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x00193D7C File Offset: 0x00192D7C
		internal static object GetResource(short id)
		{
			if (SystemResourceKey._srk == null)
			{
				SystemResourceKey._srk = new SystemResourceKey((SystemResourceKeyID)id);
			}
			else
			{
				SystemResourceKey._srk._id = (SystemResourceKeyID)id;
			}
			return SystemResourceKey._srk.Resource;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x00193DB4 File Offset: 0x00192DB4
		internal SystemResourceKey(SystemResourceKeyID id)
		{
			this._id = id;
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x00193DC3 File Offset: 0x00192DC3
		internal SystemResourceKeyID InternalKey
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06002819 RID: 10265 RVA: 0x00109403 File Offset: 0x00108403
		public override Assembly Assembly
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x00193DCC File Offset: 0x00192DCC
		public override bool Equals(object o)
		{
			SystemResourceKey systemResourceKey = o as SystemResourceKey;
			return systemResourceKey != null && systemResourceKey._id == this._id;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x00193DC3 File Offset: 0x00192DC3
		public override int GetHashCode()
		{
			return (int)this._id;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x00193DF3 File Offset: 0x00192DF3
		public override string ToString()
		{
			return this._id.ToString();
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600281D RID: 10269 RVA: 0x00193E06 File Offset: 0x00192E06
		internal static ComponentResourceKey DataGridFocusBorderBrushKey
		{
			get
			{
				if (SystemResourceKey._focusBorderBrushKey == null)
				{
					SystemResourceKey._focusBorderBrushKey = new ComponentResourceKey(typeof(DataGrid), "FocusBorderBrushKey");
				}
				return SystemResourceKey._focusBorderBrushKey;
			}
		}

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600281E RID: 10270 RVA: 0x00193E2D File Offset: 0x00192E2D
		internal static ComponentResourceKey DataGridComboBoxColumnTextBlockComboBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._textBlockComboBoxStyleKey == null)
				{
					SystemResourceKey._textBlockComboBoxStyleKey = new ComponentResourceKey(typeof(DataGrid), "TextBlockComboBoxStyleKey");
				}
				return SystemResourceKey._textBlockComboBoxStyleKey;
			}
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600281F RID: 10271 RVA: 0x00193E54 File Offset: 0x00192E54
		internal static ResourceKey MenuItemSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._menuItemSeparatorStyleKey == null)
				{
					SystemResourceKey._menuItemSeparatorStyleKey = new SystemThemeKey(SystemResourceKeyID.MenuItemSeparatorStyle);
				}
				return SystemResourceKey._menuItemSeparatorStyleKey;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002820 RID: 10272 RVA: 0x00193E71 File Offset: 0x00192E71
		internal static ComponentResourceKey DataGridColumnHeaderColumnFloatingHeaderStyleKey
		{
			get
			{
				if (SystemResourceKey._columnFloatingHeaderStyleKey == null)
				{
					SystemResourceKey._columnFloatingHeaderStyleKey = new ComponentResourceKey(typeof(DataGrid), "ColumnFloatingHeaderStyleKey");
				}
				return SystemResourceKey._columnFloatingHeaderStyleKey;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002821 RID: 10273 RVA: 0x00193E98 File Offset: 0x00192E98
		internal static ComponentResourceKey DataGridColumnHeaderColumnHeaderDropSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._columnHeaderDropSeparatorStyleKey == null)
				{
					SystemResourceKey._columnHeaderDropSeparatorStyleKey = new ComponentResourceKey(typeof(DataGrid), "ColumnHeaderDropSeparatorStyleKey");
				}
				return SystemResourceKey._columnHeaderDropSeparatorStyleKey;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06002822 RID: 10274 RVA: 0x00193EBF File Offset: 0x00192EBF
		internal static ResourceKey GridViewItemContainerStyleKey
		{
			get
			{
				if (SystemResourceKey._gridViewItemContainerStyleKey == null)
				{
					SystemResourceKey._gridViewItemContainerStyleKey = new SystemThemeKey(SystemResourceKeyID.GridViewItemContainerStyle);
				}
				return SystemResourceKey._gridViewItemContainerStyleKey;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x00193EDC File Offset: 0x00192EDC
		internal static ResourceKey GridViewScrollViewerStyleKey
		{
			get
			{
				if (SystemResourceKey._scrollViewerStyleKey == null)
				{
					SystemResourceKey._scrollViewerStyleKey = new SystemThemeKey(SystemResourceKeyID.GridViewScrollViewerStyle);
				}
				return SystemResourceKey._scrollViewerStyleKey;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x00193EF9 File Offset: 0x00192EF9
		internal static ResourceKey GridViewStyleKey
		{
			get
			{
				if (SystemResourceKey._gridViewStyleKey == null)
				{
					SystemResourceKey._gridViewStyleKey = new SystemThemeKey(SystemResourceKeyID.GridViewStyle);
				}
				return SystemResourceKey._gridViewStyleKey;
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x00193F16 File Offset: 0x00192F16
		internal static ResourceKey StatusBarSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._statusBarSeparatorStyleKey == null)
				{
					SystemResourceKey._statusBarSeparatorStyleKey = new SystemThemeKey(SystemResourceKeyID.StatusBarSeparatorStyle);
				}
				return SystemResourceKey._statusBarSeparatorStyleKey;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x00193F33 File Offset: 0x00192F33
		internal static ResourceKey ToolBarButtonStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheButtonStyle == null)
				{
					SystemResourceKey._cacheButtonStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarButtonStyle);
				}
				return SystemResourceKey._cacheButtonStyle;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x00193F50 File Offset: 0x00192F50
		internal static ResourceKey ToolBarToggleButtonStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheToggleButtonStyle == null)
				{
					SystemResourceKey._cacheToggleButtonStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarToggleButtonStyle);
				}
				return SystemResourceKey._cacheToggleButtonStyle;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x00193F6D File Offset: 0x00192F6D
		internal static ResourceKey ToolBarSeparatorStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheSeparatorStyle == null)
				{
					SystemResourceKey._cacheSeparatorStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarSeparatorStyle);
				}
				return SystemResourceKey._cacheSeparatorStyle;
			}
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x00193F8A File Offset: 0x00192F8A
		internal static ResourceKey ToolBarCheckBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheCheckBoxStyle == null)
				{
					SystemResourceKey._cacheCheckBoxStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarCheckBoxStyle);
				}
				return SystemResourceKey._cacheCheckBoxStyle;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x00193FA7 File Offset: 0x00192FA7
		internal static ResourceKey ToolBarRadioButtonStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheRadioButtonStyle == null)
				{
					SystemResourceKey._cacheRadioButtonStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarRadioButtonStyle);
				}
				return SystemResourceKey._cacheRadioButtonStyle;
			}
		}

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x00193FC4 File Offset: 0x00192FC4
		internal static ResourceKey ToolBarComboBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheComboBoxStyle == null)
				{
					SystemResourceKey._cacheComboBoxStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarComboBoxStyle);
				}
				return SystemResourceKey._cacheComboBoxStyle;
			}
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x0600282C RID: 10284 RVA: 0x00193FE1 File Offset: 0x00192FE1
		internal static ResourceKey ToolBarTextBoxStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheTextBoxStyle == null)
				{
					SystemResourceKey._cacheTextBoxStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarTextBoxStyle);
				}
				return SystemResourceKey._cacheTextBoxStyle;
			}
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x00193FFE File Offset: 0x00192FFE
		internal static ResourceKey ToolBarMenuStyleKey
		{
			get
			{
				if (SystemResourceKey._cacheMenuStyle == null)
				{
					SystemResourceKey._cacheMenuStyle = new SystemThemeKey(SystemResourceKeyID.ToolBarMenuStyle);
				}
				return SystemResourceKey._cacheMenuStyle;
			}
		}

		// Token: 0x04001453 RID: 5203
		private const short SystemResourceKeyIDStart = 0;

		// Token: 0x04001454 RID: 5204
		private const short SystemResourceKeyIDEnd = 232;

		// Token: 0x04001455 RID: 5205
		private const short SystemResourceKeyIDExtendedStart = 233;

		// Token: 0x04001456 RID: 5206
		private const short SystemResourceKeyIDExtendedEnd = 236;

		// Token: 0x04001457 RID: 5207
		private const short SystemResourceKeyBAMLIDStart = 0;

		// Token: 0x04001458 RID: 5208
		private const short SystemResourceKeyBAMLIDEnd = 232;

		// Token: 0x04001459 RID: 5209
		private const short SystemResourceBAMLIDStart = 232;

		// Token: 0x0400145A RID: 5210
		private const short SystemResourceBAMLIDEnd = 464;

		// Token: 0x0400145B RID: 5211
		private const short SystemResourceKeyBAMLIDExtendedStart = 464;

		// Token: 0x0400145C RID: 5212
		private const short SystemResourceKeyBAMLIDExtendedEnd = 467;

		// Token: 0x0400145D RID: 5213
		private const short SystemResourceBAMLIDExtendedStart = 467;

		// Token: 0x0400145E RID: 5214
		private const short SystemResourceBAMLIDExtendedEnd = 470;

		// Token: 0x0400145F RID: 5215
		private static SystemThemeKey _cacheSeparatorStyle;

		// Token: 0x04001460 RID: 5216
		private static SystemThemeKey _cacheCheckBoxStyle;

		// Token: 0x04001461 RID: 5217
		private static SystemThemeKey _cacheToggleButtonStyle;

		// Token: 0x04001462 RID: 5218
		private static SystemThemeKey _cacheButtonStyle;

		// Token: 0x04001463 RID: 5219
		private static SystemThemeKey _cacheRadioButtonStyle;

		// Token: 0x04001464 RID: 5220
		private static SystemThemeKey _cacheComboBoxStyle;

		// Token: 0x04001465 RID: 5221
		private static SystemThemeKey _cacheTextBoxStyle;

		// Token: 0x04001466 RID: 5222
		private static SystemThemeKey _cacheMenuStyle;

		// Token: 0x04001467 RID: 5223
		private static ComponentResourceKey _focusBorderBrushKey;

		// Token: 0x04001468 RID: 5224
		private static ComponentResourceKey _textBlockComboBoxStyleKey;

		// Token: 0x04001469 RID: 5225
		private static SystemThemeKey _menuItemSeparatorStyleKey;

		// Token: 0x0400146A RID: 5226
		private static ComponentResourceKey _columnHeaderDropSeparatorStyleKey;

		// Token: 0x0400146B RID: 5227
		private static ComponentResourceKey _columnFloatingHeaderStyleKey;

		// Token: 0x0400146C RID: 5228
		private static SystemThemeKey _gridViewItemContainerStyleKey;

		// Token: 0x0400146D RID: 5229
		private static SystemThemeKey _scrollViewerStyleKey;

		// Token: 0x0400146E RID: 5230
		private static SystemThemeKey _gridViewStyleKey;

		// Token: 0x0400146F RID: 5231
		private static SystemThemeKey _statusBarSeparatorStyleKey;

		// Token: 0x04001470 RID: 5232
		private SystemResourceKeyID _id;

		// Token: 0x04001471 RID: 5233
		[ThreadStatic]
		private static SystemResourceKey _srk;
	}
}
