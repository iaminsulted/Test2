using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Converters;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xaml;
using System.Xaml.Schema;
using System.Xml.Serialization;
using MS.Internal.Markup;

namespace System.Windows.Baml2006
{
	// Token: 0x02000414 RID: 1044
	internal class WpfSharedBamlSchemaContext : XamlSchemaContext
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x001ABA54 File Offset: 0x001AAA54
		private WpfKnownMember CreateKnownMember(short bamlNumber)
		{
			switch (bamlNumber)
			{
			case 1:
				return this.Create_BamlProperty_AccessText_Text();
			case 2:
				return this.Create_BamlProperty_BeginStoryboard_Storyboard();
			case 3:
				return this.Create_BamlProperty_BitmapEffectGroup_Children();
			case 4:
				return this.Create_BamlProperty_Border_Background();
			case 5:
				return this.Create_BamlProperty_Border_BorderBrush();
			case 6:
				return this.Create_BamlProperty_Border_BorderThickness();
			case 7:
				return this.Create_BamlProperty_ButtonBase_Command();
			case 8:
				return this.Create_BamlProperty_ButtonBase_CommandParameter();
			case 9:
				return this.Create_BamlProperty_ButtonBase_CommandTarget();
			case 10:
				return this.Create_BamlProperty_ButtonBase_IsPressed();
			case 11:
				return this.Create_BamlProperty_ColumnDefinition_MaxWidth();
			case 12:
				return this.Create_BamlProperty_ColumnDefinition_MinWidth();
			case 13:
				return this.Create_BamlProperty_ColumnDefinition_Width();
			case 14:
				return this.Create_BamlProperty_ContentControl_Content();
			case 15:
				return this.Create_BamlProperty_ContentControl_ContentTemplate();
			case 16:
				return this.Create_BamlProperty_ContentControl_ContentTemplateSelector();
			case 17:
				return this.Create_BamlProperty_ContentControl_HasContent();
			case 18:
				return this.Create_BamlProperty_ContentElement_Focusable();
			case 19:
				return this.Create_BamlProperty_ContentPresenter_Content();
			case 20:
				return this.Create_BamlProperty_ContentPresenter_ContentSource();
			case 21:
				return this.Create_BamlProperty_ContentPresenter_ContentTemplate();
			case 22:
				return this.Create_BamlProperty_ContentPresenter_ContentTemplateSelector();
			case 23:
				return this.Create_BamlProperty_ContentPresenter_RecognizesAccessKey();
			case 24:
				return this.Create_BamlProperty_Control_Background();
			case 25:
				return this.Create_BamlProperty_Control_BorderBrush();
			case 26:
				return this.Create_BamlProperty_Control_BorderThickness();
			case 27:
				return this.Create_BamlProperty_Control_FontFamily();
			case 28:
				return this.Create_BamlProperty_Control_FontSize();
			case 29:
				return this.Create_BamlProperty_Control_FontStretch();
			case 30:
				return this.Create_BamlProperty_Control_FontStyle();
			case 31:
				return this.Create_BamlProperty_Control_FontWeight();
			case 32:
				return this.Create_BamlProperty_Control_Foreground();
			case 33:
				return this.Create_BamlProperty_Control_HorizontalContentAlignment();
			case 34:
				return this.Create_BamlProperty_Control_IsTabStop();
			case 35:
				return this.Create_BamlProperty_Control_Padding();
			case 36:
				return this.Create_BamlProperty_Control_TabIndex();
			case 37:
				return this.Create_BamlProperty_Control_Template();
			case 38:
				return this.Create_BamlProperty_Control_VerticalContentAlignment();
			case 39:
				return this.Create_BamlProperty_DockPanel_Dock();
			case 40:
				return this.Create_BamlProperty_DockPanel_LastChildFill();
			case 41:
				return this.Create_BamlProperty_DocumentViewerBase_Document();
			case 42:
				return this.Create_BamlProperty_DrawingGroup_Children();
			case 43:
				return this.Create_BamlProperty_FlowDocumentReader_Document();
			case 44:
				return this.Create_BamlProperty_FlowDocumentScrollViewer_Document();
			case 45:
				return this.Create_BamlProperty_FrameworkContentElement_Style();
			case 46:
				return this.Create_BamlProperty_FrameworkElement_FlowDirection();
			case 47:
				return this.Create_BamlProperty_FrameworkElement_Height();
			case 48:
				return this.Create_BamlProperty_FrameworkElement_HorizontalAlignment();
			case 49:
				return this.Create_BamlProperty_FrameworkElement_Margin();
			case 50:
				return this.Create_BamlProperty_FrameworkElement_MaxHeight();
			case 51:
				return this.Create_BamlProperty_FrameworkElement_MaxWidth();
			case 52:
				return this.Create_BamlProperty_FrameworkElement_MinHeight();
			case 53:
				return this.Create_BamlProperty_FrameworkElement_MinWidth();
			case 54:
				return this.Create_BamlProperty_FrameworkElement_Name();
			case 55:
				return this.Create_BamlProperty_FrameworkElement_Style();
			case 56:
				return this.Create_BamlProperty_FrameworkElement_VerticalAlignment();
			case 57:
				return this.Create_BamlProperty_FrameworkElement_Width();
			case 58:
				return this.Create_BamlProperty_GeneralTransformGroup_Children();
			case 59:
				return this.Create_BamlProperty_GeometryGroup_Children();
			case 60:
				return this.Create_BamlProperty_GradientBrush_GradientStops();
			case 61:
				return this.Create_BamlProperty_Grid_Column();
			case 62:
				return this.Create_BamlProperty_Grid_ColumnSpan();
			case 63:
				return this.Create_BamlProperty_Grid_Row();
			case 64:
				return this.Create_BamlProperty_Grid_RowSpan();
			case 65:
				return this.Create_BamlProperty_GridViewColumn_Header();
			case 66:
				return this.Create_BamlProperty_HeaderedContentControl_HasHeader();
			case 67:
				return this.Create_BamlProperty_HeaderedContentControl_Header();
			case 68:
				return this.Create_BamlProperty_HeaderedContentControl_HeaderTemplate();
			case 69:
				return this.Create_BamlProperty_HeaderedContentControl_HeaderTemplateSelector();
			case 70:
				return this.Create_BamlProperty_HeaderedItemsControl_HasHeader();
			case 71:
				return this.Create_BamlProperty_HeaderedItemsControl_Header();
			case 72:
				return this.Create_BamlProperty_HeaderedItemsControl_HeaderTemplate();
			case 73:
				return this.Create_BamlProperty_HeaderedItemsControl_HeaderTemplateSelector();
			case 74:
				return this.Create_BamlProperty_Hyperlink_NavigateUri();
			case 75:
				return this.Create_BamlProperty_Image_Source();
			case 76:
				return this.Create_BamlProperty_Image_Stretch();
			case 77:
				return this.Create_BamlProperty_ItemsControl_ItemContainerStyle();
			case 78:
				return this.Create_BamlProperty_ItemsControl_ItemContainerStyleSelector();
			case 79:
				return this.Create_BamlProperty_ItemsControl_ItemTemplate();
			case 80:
				return this.Create_BamlProperty_ItemsControl_ItemTemplateSelector();
			case 81:
				return this.Create_BamlProperty_ItemsControl_ItemsPanel();
			case 82:
				return this.Create_BamlProperty_ItemsControl_ItemsSource();
			case 83:
				return this.Create_BamlProperty_MaterialGroup_Children();
			case 84:
				return this.Create_BamlProperty_Model3DGroup_Children();
			case 85:
				return this.Create_BamlProperty_Page_Content();
			case 86:
				return this.Create_BamlProperty_Panel_Background();
			case 87:
				return this.Create_BamlProperty_Path_Data();
			case 88:
				return this.Create_BamlProperty_PathFigure_Segments();
			case 89:
				return this.Create_BamlProperty_PathGeometry_Figures();
			case 90:
				return this.Create_BamlProperty_Popup_Child();
			case 91:
				return this.Create_BamlProperty_Popup_IsOpen();
			case 92:
				return this.Create_BamlProperty_Popup_Placement();
			case 93:
				return this.Create_BamlProperty_Popup_PopupAnimation();
			case 94:
				return this.Create_BamlProperty_RowDefinition_Height();
			case 95:
				return this.Create_BamlProperty_RowDefinition_MaxHeight();
			case 96:
				return this.Create_BamlProperty_RowDefinition_MinHeight();
			case 97:
				return this.Create_BamlProperty_ScrollViewer_CanContentScroll();
			case 98:
				return this.Create_BamlProperty_ScrollViewer_HorizontalScrollBarVisibility();
			case 99:
				return this.Create_BamlProperty_ScrollViewer_VerticalScrollBarVisibility();
			case 100:
				return this.Create_BamlProperty_Shape_Fill();
			case 101:
				return this.Create_BamlProperty_Shape_Stroke();
			case 102:
				return this.Create_BamlProperty_Shape_StrokeThickness();
			case 103:
				return this.Create_BamlProperty_TextBlock_Background();
			case 104:
				return this.Create_BamlProperty_TextBlock_FontFamily();
			case 105:
				return this.Create_BamlProperty_TextBlock_FontSize();
			case 106:
				return this.Create_BamlProperty_TextBlock_FontStretch();
			case 107:
				return this.Create_BamlProperty_TextBlock_FontStyle();
			case 108:
				return this.Create_BamlProperty_TextBlock_FontWeight();
			case 109:
				return this.Create_BamlProperty_TextBlock_Foreground();
			case 110:
				return this.Create_BamlProperty_TextBlock_Text();
			case 111:
				return this.Create_BamlProperty_TextBlock_TextDecorations();
			case 112:
				return this.Create_BamlProperty_TextBlock_TextTrimming();
			case 113:
				return this.Create_BamlProperty_TextBlock_TextWrapping();
			case 114:
				return this.Create_BamlProperty_TextBox_Text();
			case 115:
				return this.Create_BamlProperty_TextElement_Background();
			case 116:
				return this.Create_BamlProperty_TextElement_FontFamily();
			case 117:
				return this.Create_BamlProperty_TextElement_FontSize();
			case 118:
				return this.Create_BamlProperty_TextElement_FontStretch();
			case 119:
				return this.Create_BamlProperty_TextElement_FontStyle();
			case 120:
				return this.Create_BamlProperty_TextElement_FontWeight();
			case 121:
				return this.Create_BamlProperty_TextElement_Foreground();
			case 122:
				return this.Create_BamlProperty_TimelineGroup_Children();
			case 123:
				return this.Create_BamlProperty_Track_IsDirectionReversed();
			case 124:
				return this.Create_BamlProperty_Track_Maximum();
			case 125:
				return this.Create_BamlProperty_Track_Minimum();
			case 126:
				return this.Create_BamlProperty_Track_Orientation();
			case 127:
				return this.Create_BamlProperty_Track_Value();
			case 128:
				return this.Create_BamlProperty_Track_ViewportSize();
			case 129:
				return this.Create_BamlProperty_Transform3DGroup_Children();
			case 130:
				return this.Create_BamlProperty_TransformGroup_Children();
			case 131:
				return this.Create_BamlProperty_UIElement_ClipToBounds();
			case 132:
				return this.Create_BamlProperty_UIElement_Focusable();
			case 133:
				return this.Create_BamlProperty_UIElement_IsEnabled();
			case 134:
				return this.Create_BamlProperty_UIElement_RenderTransform();
			case 135:
				return this.Create_BamlProperty_UIElement_Visibility();
			case 136:
				return this.Create_BamlProperty_Viewport3D_Children();
			case 138:
				return this.Create_BamlProperty_AdornedElementPlaceholder_Child();
			case 139:
				return this.Create_BamlProperty_AdornerDecorator_Child();
			case 140:
				return this.Create_BamlProperty_AnchoredBlock_Blocks();
			case 141:
				return this.Create_BamlProperty_ArrayExtension_Items();
			case 142:
				return this.Create_BamlProperty_BlockUIContainer_Child();
			case 143:
				return this.Create_BamlProperty_Bold_Inlines();
			case 144:
				return this.Create_BamlProperty_BooleanAnimationUsingKeyFrames_KeyFrames();
			case 145:
				return this.Create_BamlProperty_Border_Child();
			case 146:
				return this.Create_BamlProperty_BulletDecorator_Child();
			case 147:
				return this.Create_BamlProperty_Button_Content();
			case 148:
				return this.Create_BamlProperty_ButtonBase_Content();
			case 149:
				return this.Create_BamlProperty_ByteAnimationUsingKeyFrames_KeyFrames();
			case 150:
				return this.Create_BamlProperty_Canvas_Children();
			case 151:
				return this.Create_BamlProperty_CharAnimationUsingKeyFrames_KeyFrames();
			case 152:
				return this.Create_BamlProperty_CheckBox_Content();
			case 153:
				return this.Create_BamlProperty_ColorAnimationUsingKeyFrames_KeyFrames();
			case 154:
				return this.Create_BamlProperty_ComboBox_Items();
			case 155:
				return this.Create_BamlProperty_ComboBoxItem_Content();
			case 156:
				return this.Create_BamlProperty_ContextMenu_Items();
			case 157:
				return this.Create_BamlProperty_ControlTemplate_VisualTree();
			case 158:
				return this.Create_BamlProperty_DataTemplate_VisualTree();
			case 159:
				return this.Create_BamlProperty_DataTrigger_Setters();
			case 160:
				return this.Create_BamlProperty_DecimalAnimationUsingKeyFrames_KeyFrames();
			case 161:
				return this.Create_BamlProperty_Decorator_Child();
			case 162:
				return this.Create_BamlProperty_DockPanel_Children();
			case 163:
				return this.Create_BamlProperty_DocumentViewer_Document();
			case 164:
				return this.Create_BamlProperty_DoubleAnimationUsingKeyFrames_KeyFrames();
			case 165:
				return this.Create_BamlProperty_EventTrigger_Actions();
			case 166:
				return this.Create_BamlProperty_Expander_Content();
			case 167:
				return this.Create_BamlProperty_Figure_Blocks();
			case 168:
				return this.Create_BamlProperty_FixedDocument_Pages();
			case 169:
				return this.Create_BamlProperty_FixedDocumentSequence_References();
			case 170:
				return this.Create_BamlProperty_FixedPage_Children();
			case 171:
				return this.Create_BamlProperty_Floater_Blocks();
			case 172:
				return this.Create_BamlProperty_FlowDocument_Blocks();
			case 173:
				return this.Create_BamlProperty_FlowDocumentPageViewer_Document();
			case 174:
				return this.Create_BamlProperty_FrameworkTemplate_VisualTree();
			case 175:
				return this.Create_BamlProperty_Grid_Children();
			case 176:
				return this.Create_BamlProperty_GridView_Columns();
			case 177:
				return this.Create_BamlProperty_GridViewColumnHeader_Content();
			case 178:
				return this.Create_BamlProperty_GroupBox_Content();
			case 179:
				return this.Create_BamlProperty_GroupItem_Content();
			case 180:
				return this.Create_BamlProperty_HeaderedContentControl_Content();
			case 181:
				return this.Create_BamlProperty_HeaderedItemsControl_Items();
			case 182:
				return this.Create_BamlProperty_HierarchicalDataTemplate_VisualTree();
			case 183:
				return this.Create_BamlProperty_Hyperlink_Inlines();
			case 184:
				return this.Create_BamlProperty_InkCanvas_Children();
			case 185:
				return this.Create_BamlProperty_InkPresenter_Child();
			case 186:
				return this.Create_BamlProperty_InlineUIContainer_Child();
			case 187:
				return this.Create_BamlProperty_InputScopeName_NameValue();
			case 188:
				return this.Create_BamlProperty_Int16AnimationUsingKeyFrames_KeyFrames();
			case 189:
				return this.Create_BamlProperty_Int32AnimationUsingKeyFrames_KeyFrames();
			case 190:
				return this.Create_BamlProperty_Int64AnimationUsingKeyFrames_KeyFrames();
			case 191:
				return this.Create_BamlProperty_Italic_Inlines();
			case 192:
				return this.Create_BamlProperty_ItemsControl_Items();
			case 193:
				return this.Create_BamlProperty_ItemsPanelTemplate_VisualTree();
			case 194:
				return this.Create_BamlProperty_Label_Content();
			case 195:
				return this.Create_BamlProperty_LinearGradientBrush_GradientStops();
			case 196:
				return this.Create_BamlProperty_List_ListItems();
			case 197:
				return this.Create_BamlProperty_ListBox_Items();
			case 198:
				return this.Create_BamlProperty_ListBoxItem_Content();
			case 199:
				return this.Create_BamlProperty_ListItem_Blocks();
			case 200:
				return this.Create_BamlProperty_ListView_Items();
			case 201:
				return this.Create_BamlProperty_ListViewItem_Content();
			case 202:
				return this.Create_BamlProperty_MatrixAnimationUsingKeyFrames_KeyFrames();
			case 203:
				return this.Create_BamlProperty_Menu_Items();
			case 204:
				return this.Create_BamlProperty_MenuBase_Items();
			case 205:
				return this.Create_BamlProperty_MenuItem_Items();
			case 206:
				return this.Create_BamlProperty_ModelVisual3D_Children();
			case 207:
				return this.Create_BamlProperty_MultiBinding_Bindings();
			case 208:
				return this.Create_BamlProperty_MultiDataTrigger_Setters();
			case 209:
				return this.Create_BamlProperty_MultiTrigger_Setters();
			case 210:
				return this.Create_BamlProperty_ObjectAnimationUsingKeyFrames_KeyFrames();
			case 211:
				return this.Create_BamlProperty_PageContent_Child();
			case 212:
				return this.Create_BamlProperty_PageFunctionBase_Content();
			case 213:
				return this.Create_BamlProperty_Panel_Children();
			case 214:
				return this.Create_BamlProperty_Paragraph_Inlines();
			case 215:
				return this.Create_BamlProperty_ParallelTimeline_Children();
			case 216:
				return this.Create_BamlProperty_Point3DAnimationUsingKeyFrames_KeyFrames();
			case 217:
				return this.Create_BamlProperty_PointAnimationUsingKeyFrames_KeyFrames();
			case 218:
				return this.Create_BamlProperty_PriorityBinding_Bindings();
			case 219:
				return this.Create_BamlProperty_QuaternionAnimationUsingKeyFrames_KeyFrames();
			case 220:
				return this.Create_BamlProperty_RadialGradientBrush_GradientStops();
			case 221:
				return this.Create_BamlProperty_RadioButton_Content();
			case 222:
				return this.Create_BamlProperty_RectAnimationUsingKeyFrames_KeyFrames();
			case 223:
				return this.Create_BamlProperty_RepeatButton_Content();
			case 224:
				return this.Create_BamlProperty_RichTextBox_Document();
			case 225:
				return this.Create_BamlProperty_Rotation3DAnimationUsingKeyFrames_KeyFrames();
			case 226:
				return this.Create_BamlProperty_Run_Text();
			case 227:
				return this.Create_BamlProperty_ScrollViewer_Content();
			case 228:
				return this.Create_BamlProperty_Section_Blocks();
			case 229:
				return this.Create_BamlProperty_Selector_Items();
			case 230:
				return this.Create_BamlProperty_SingleAnimationUsingKeyFrames_KeyFrames();
			case 231:
				return this.Create_BamlProperty_SizeAnimationUsingKeyFrames_KeyFrames();
			case 232:
				return this.Create_BamlProperty_Span_Inlines();
			case 233:
				return this.Create_BamlProperty_StackPanel_Children();
			case 234:
				return this.Create_BamlProperty_StatusBar_Items();
			case 235:
				return this.Create_BamlProperty_StatusBarItem_Content();
			case 236:
				return this.Create_BamlProperty_Storyboard_Children();
			case 237:
				return this.Create_BamlProperty_StringAnimationUsingKeyFrames_KeyFrames();
			case 238:
				return this.Create_BamlProperty_Style_Setters();
			case 239:
				return this.Create_BamlProperty_TabControl_Items();
			case 240:
				return this.Create_BamlProperty_TabItem_Content();
			case 241:
				return this.Create_BamlProperty_TabPanel_Children();
			case 242:
				return this.Create_BamlProperty_Table_RowGroups();
			case 243:
				return this.Create_BamlProperty_TableCell_Blocks();
			case 244:
				return this.Create_BamlProperty_TableRow_Cells();
			case 245:
				return this.Create_BamlProperty_TableRowGroup_Rows();
			case 246:
				return this.Create_BamlProperty_TextBlock_Inlines();
			case 247:
				return this.Create_BamlProperty_ThicknessAnimationUsingKeyFrames_KeyFrames();
			case 248:
				return this.Create_BamlProperty_ToggleButton_Content();
			case 249:
				return this.Create_BamlProperty_ToolBar_Items();
			case 250:
				return this.Create_BamlProperty_ToolBarOverflowPanel_Children();
			case 251:
				return this.Create_BamlProperty_ToolBarPanel_Children();
			case 252:
				return this.Create_BamlProperty_ToolBarTray_ToolBars();
			case 253:
				return this.Create_BamlProperty_ToolTip_Content();
			case 254:
				return this.Create_BamlProperty_TreeView_Items();
			case 255:
				return this.Create_BamlProperty_TreeViewItem_Items();
			case 256:
				return this.Create_BamlProperty_Trigger_Setters();
			case 257:
				return this.Create_BamlProperty_Underline_Inlines();
			case 258:
				return this.Create_BamlProperty_UniformGrid_Children();
			case 259:
				return this.Create_BamlProperty_UserControl_Content();
			case 260:
				return this.Create_BamlProperty_Vector3DAnimationUsingKeyFrames_KeyFrames();
			case 261:
				return this.Create_BamlProperty_VectorAnimationUsingKeyFrames_KeyFrames();
			case 262:
				return this.Create_BamlProperty_Viewbox_Child();
			case 263:
				return this.Create_BamlProperty_Viewport3DVisual_Children();
			case 264:
				return this.Create_BamlProperty_VirtualizingPanel_Children();
			case 265:
				return this.Create_BamlProperty_VirtualizingStackPanel_Children();
			case 266:
				return this.Create_BamlProperty_Window_Content();
			case 267:
				return this.Create_BamlProperty_WrapPanel_Children();
			case 268:
				return this.Create_BamlProperty_XmlDataProvider_XmlSerializer();
			}
			throw new InvalidOperationException("Invalid BAML number");
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x001AC5F8 File Offset: 0x001AB5F8
		private uint GetTypeNameHashForPropeties(string typeName)
		{
			uint num = 0U;
			int num2 = 1;
			while (num2 < 15 && num2 < typeName.Length)
			{
				num = 101U * num + (uint)typeName[num2];
				num2++;
			}
			return num;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x001AC62C File Offset: 0x001AB62C
		internal WpfKnownMember CreateKnownMember(string type, string property)
		{
			uint typeNameHashForPropeties = this.GetTypeNameHashForPropeties(type);
			if (typeNameHashForPropeties <= 1888893854U)
			{
				if (typeNameHashForPropeties <= 826277256U)
				{
					if (typeNameHashForPropeties <= 137005044U)
					{
						if (typeNameHashForPropeties <= 100949204U)
						{
							if (typeNameHashForPropeties <= 1041528U)
							{
								if (typeNameHashForPropeties <= 11927U)
								{
									if (typeNameHashForPropeties != 10311U)
									{
										if (typeNameHashForPropeties == 11927U)
										{
											if (property == "Text")
											{
												return this.GetKnownBamlMember(-226);
											}
											return null;
										}
									}
									else
									{
										if (property == "LineJoin")
										{
											return this.Create_BamlProperty_Pen_LineJoin();
										}
										return null;
									}
								}
								else if (typeNameHashForPropeties != 1000001U)
								{
									if (typeNameHashForPropeties != 1001317U)
									{
										if (typeNameHashForPropeties == 1041528U)
										{
											if (property == "Items")
											{
												return this.GetKnownBamlMember(-203);
											}
											return null;
										}
									}
									else
									{
										if (property == "Data")
										{
											return this.GetKnownBamlMember(-87);
										}
										return null;
									}
								}
								else
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-85);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties <= 1152419U)
							{
								if (typeNameHashForPropeties != 1082836U)
								{
									if (typeNameHashForPropeties != 1143319U)
									{
										if (typeNameHashForPropeties == 1152419U)
										{
											if (property == "Inlines")
											{
												return this.GetKnownBamlMember(-232);
											}
											return null;
										}
									}
									else
									{
										if (property == "Inlines")
										{
											return this.GetKnownBamlMember(-143);
										}
										return null;
									}
								}
								else
								{
									if (property == "ListItems")
									{
										return this.GetKnownBamlMember(-196);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 1173619U)
							{
								if (typeNameHashForPropeties != 15810163U)
								{
									if (typeNameHashForPropeties == 100949204U)
									{
										if (property == "Content")
										{
											return this.GetKnownBamlMember(-194);
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-267);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-175);
								}
								if (property == "ColumnDefinitions")
								{
									return this.Create_BamlProperty_Grid_ColumnDefinitions();
								}
								if (!(property == "RowDefinitions"))
								{
									return null;
								}
								return this.Create_BamlProperty_Grid_RowDefinitions();
							}
						}
						else if (typeNameHashForPropeties <= 113302810U)
						{
							if (typeNameHashForPropeties <= 108152214U)
							{
								if (typeNameHashForPropeties != 100949904U)
								{
									if (typeNameHashForPropeties != 101071616U)
									{
										if (typeNameHashForPropeties == 108152214U)
										{
											uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
											if (num <= 840450408U)
											{
												if (num <= 373006096U)
												{
													if (num != 64207306U)
													{
														if (num == 373006096U)
														{
															if (property == "StrokeEndLineCap")
															{
																return this.Create_BamlProperty_Shape_StrokeEndLineCap();
															}
														}
													}
													else if (property == "Stretch")
													{
														return this.Create_BamlProperty_Shape_Stretch();
													}
												}
												else if (num != 826157205U)
												{
													if (num == 840450408U)
													{
														if (property == "Fill")
														{
															return this.GetKnownBamlMember(-100);
														}
													}
												}
												else if (property == "StrokeMiterLimit")
												{
													return this.Create_BamlProperty_Shape_StrokeMiterLimit();
												}
											}
											else if (num <= 3298007219U)
											{
												if (num != 3169123063U)
												{
													if (num == 3298007219U)
													{
														if (property == "StrokeStartLineCap")
														{
															return this.Create_BamlProperty_Shape_StrokeStartLineCap();
														}
													}
												}
												else if (property == "StrokeLineJoin")
												{
													return this.Create_BamlProperty_Shape_StrokeLineJoin();
												}
											}
											else if (num != 3597459781U)
											{
												if (num == 4290083157U)
												{
													if (property == "Stroke")
													{
														return this.GetKnownBamlMember(-101);
													}
												}
											}
											else if (property == "StrokeThickness")
											{
												return this.GetKnownBamlMember(-102);
											}
											return null;
										}
									}
									else
									{
										if (property == "Background")
										{
											return this.GetKnownBamlMember(-86);
										}
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-213);
										}
										if (!(property == "IsItemsHost"))
										{
											return null;
										}
										return this.Create_BamlProperty_Panel_IsItemsHost();
									}
								}
								else
								{
									if (property == "RowGroups")
									{
										return this.GetKnownBamlMember(-242);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 109056765U)
							{
								if (typeNameHashForPropeties != 112414925U)
								{
									if (typeNameHashForPropeties == 113302810U)
									{
										if (property == "Source")
										{
											return this.GetKnownBamlMember(-75);
										}
										if (!(property == "Stretch"))
										{
											return null;
										}
										return this.GetKnownBamlMember(-76);
									}
								}
								else
								{
									if (property == "TextAlignment")
									{
										return this.Create_BamlProperty_Block_TextAlignment();
									}
									return null;
								}
							}
							else
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-146);
								}
								if (!(property == "Bullet"))
								{
									return null;
								}
								return this.Create_BamlProperty_BulletDecorator_Bullet();
							}
						}
						else if (typeNameHashForPropeties <= 118454921U)
						{
							if (typeNameHashForPropeties != 115517852U)
							{
								if (typeNameHashForPropeties == 118453917U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 1739826286U)
									{
										if (num <= 1169574112U)
										{
											if (num != 438536855U)
											{
												if (num == 1169574112U)
												{
													if (property == "IsDirectionReversed")
													{
														return this.GetKnownBamlMember(-123);
													}
												}
											}
											else if (property == "Minimum")
											{
												return this.GetKnownBamlMember(-125);
											}
										}
										else if (num != 1727973954U)
										{
											if (num == 1739826286U)
											{
												if (property == "DecreaseRepeatButton")
												{
													return this.Create_BamlProperty_Track_DecreaseRepeatButton();
												}
											}
										}
										else if (property == "ViewportSize")
										{
											return this.GetKnownBamlMember(-128);
										}
									}
									else if (num <= 2645675226U)
									{
										if (num != 2615309699U)
										{
											if (num == 2645675226U)
											{
												if (property == "IncreaseRepeatButton")
												{
													return this.Create_BamlProperty_Track_IncreaseRepeatButton();
												}
											}
										}
										else if (property == "Thumb")
										{
											return this.Create_BamlProperty_Track_Thumb();
										}
									}
									else if (num != 3310475713U)
									{
										if (num != 3511155050U)
										{
											if (num == 3801439777U)
											{
												if (property == "Maximum")
												{
													return this.GetKnownBamlMember(-124);
												}
											}
										}
										else if (property == "Value")
										{
											return this.GetKnownBamlMember(-127);
										}
									}
									else if (property == "Orientation")
									{
										return this.GetKnownBamlMember(-126);
									}
									return null;
								}
								if (typeNameHashForPropeties == 118454921U)
								{
									if (property == "JournalOwnership")
									{
										return this.Create_BamlProperty_Frame_JournalOwnership();
									}
									if (!(property == "NavigationUIVisibility"))
									{
										return null;
									}
									return this.Create_BamlProperty_Frame_NavigationUIVisibility();
								}
							}
							else
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-90);
								}
								if (property == "IsOpen")
								{
									return this.GetKnownBamlMember(-91);
								}
								if (property == "Placement")
								{
									return this.GetKnownBamlMember(-92);
								}
								if (!(property == "PopupAnimation"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-93);
							}
						}
						else if (typeNameHashForPropeties != 118659550U)
						{
							if (typeNameHashForPropeties != 120760246U)
							{
								if (typeNameHashForPropeties == 137005044U)
								{
									if (property == "FallbackValue")
									{
										return this.Create_BamlProperty_BindingBase_FallbackValue();
									}
									return null;
								}
							}
							else
							{
								if (property == "Setters")
								{
									return this.GetKnownBamlMember(-238);
								}
								if (property == "TargetType")
								{
									return this.Create_BamlProperty_Style_TargetType();
								}
								if (property == "Triggers")
								{
									return this.Create_BamlProperty_Style_Triggers();
								}
								if (property == "BasedOn")
								{
									return this.Create_BamlProperty_Style_BasedOn();
								}
								if (!(property == "Resources"))
								{
									return null;
								}
								return this.Create_BamlProperty_Style_Resources();
							}
						}
						else
						{
							if (property == "Opacity")
							{
								return this.Create_BamlProperty_Brush_Opacity();
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 411789920U)
					{
						if (typeNameHashForPropeties <= 350834986U)
						{
							if (typeNameHashForPropeties <= 213893085U)
							{
								if (typeNameHashForPropeties != 141114174U)
								{
									if (typeNameHashForPropeties != 175175278U)
									{
										if (typeNameHashForPropeties == 213893085U)
										{
											if (property == "Figures")
											{
												return this.GetKnownBamlMember(-89);
											}
											return null;
										}
									}
									else
									{
										if (property == "MaxWidth")
										{
											return this.GetKnownBamlMember(-11);
										}
										if (property == "MinWidth")
										{
											return this.GetKnownBamlMember(-12);
										}
										if (!(property == "Width"))
										{
											return null;
										}
										return this.GetKnownBamlMember(-13);
									}
								}
								else
								{
									if (property == "Setters")
									{
										return this.GetKnownBamlMember(-208);
									}
									if (!(property == "Conditions"))
									{
										return null;
									}
									return this.Create_BamlProperty_MultiDataTrigger_Conditions();
								}
							}
							else if (typeNameHashForPropeties != 276220969U)
							{
								if (typeNameHashForPropeties != 282171645U)
								{
									if (typeNameHashForPropeties == 350834986U)
									{
										if (property == "TileMode")
										{
											return this.Create_BamlProperty_TileBrush_TileMode();
										}
										if (property == "ViewboxUnits")
										{
											return this.Create_BamlProperty_TileBrush_ViewboxUnits();
										}
										if (!(property == "ViewportUnits"))
										{
											return null;
										}
										return this.Create_BamlProperty_TileBrush_ViewportUnits();
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-263);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-190);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties <= 381891668U)
						{
							if (typeNameHashForPropeties != 372593541U)
							{
								if (typeNameHashForPropeties != 374054883U)
								{
									if (typeNameHashForPropeties == 381891668U)
									{
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-3);
										}
										return null;
									}
								}
								else
								{
									if (property == "VisualTree")
									{
										return this.GetKnownBamlMember(-193);
									}
									return null;
								}
							}
							else
							{
								if (property == "References")
								{
									return this.GetKnownBamlMember(-169);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 385774234U)
						{
							if (typeNameHashForPropeties != 389898151U)
							{
								if (typeNameHashForPropeties == 411789920U)
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-177);
									}
									return null;
								}
							}
							else
							{
								if (property == "Content")
								{
									return this.GetKnownBamlMember(-178);
								}
								return null;
							}
						}
						else
						{
							if (property == "Text")
							{
								return this.GetKnownBamlMember(-114);
							}
							if (property == "TextWrapping")
							{
								return this.Create_BamlProperty_TextBox_TextWrapping();
							}
							if (!(property == "TextAlignment"))
							{
								return null;
							}
							return this.Create_BamlProperty_TextBox_TextAlignment();
						}
					}
					else if (typeNameHashForPropeties <= 671959932U)
					{
						if (typeNameHashForPropeties <= 489623484U)
						{
							if (typeNameHashForPropeties != 441893333U)
							{
								if (typeNameHashForPropeties != 486209962U)
								{
									if (typeNameHashForPropeties == 489623484U)
									{
										if (property == "KeyFrames")
										{
											return this.GetKnownBamlMember(-210);
										}
										return null;
									}
								}
								else
								{
									if (property == "KeyFrames")
									{
										return this.GetKnownBamlMember(-188);
									}
									return null;
								}
							}
							else
							{
								if (property == "Document")
								{
									return this.GetKnownBamlMember(-163);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 491630740U)
						{
							if (typeNameHashForPropeties != 649104411U)
							{
								if (typeNameHashForPropeties == 671959932U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-239);
									}
									return null;
								}
							}
							else
							{
								if (property == "Segments")
								{
									return this.GetKnownBamlMember(-88);
								}
								if (property == "IsClosed")
								{
									return this.Create_BamlProperty_PathFigure_IsClosed();
								}
								if (!(property == "IsFilled"))
								{
									return null;
								}
								return this.Create_BamlProperty_PathFigure_IsFilled();
							}
						}
						else
						{
							if (property == "Storyboard")
							{
								return this.GetKnownBamlMember(-2);
							}
							if (!(property == "Name"))
							{
								return null;
							}
							return this.Create_BamlProperty_BeginStoryboard_Name();
						}
					}
					else if (typeNameHashForPropeties <= 767240674U)
					{
						if (typeNameHashForPropeties != 732268889U)
						{
							if (typeNameHashForPropeties != 755422265U)
							{
								if (typeNameHashForPropeties == 767240674U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 2494566484U)
									{
										if (num <= 466695522U)
										{
											if (num != 266367750U)
											{
												if (num != 462246226U)
												{
													if (num == 466695522U)
													{
														if (property == "Height")
														{
															return this.GetKnownBamlMember(-47);
														}
													}
												}
												else if (property == "HorizontalAlignment")
												{
													return this.GetKnownBamlMember(-48);
												}
											}
											else if (property == "Name")
											{
												return this.GetKnownBamlMember(-54);
											}
										}
										else if (num <= 1416096886U)
										{
											if (num != 994238399U)
											{
												if (num == 1416096886U)
												{
													if (property == "Style")
													{
														return this.GetKnownBamlMember(-55);
													}
												}
											}
											else if (property == "Width")
											{
												return this.GetKnownBamlMember(-57);
											}
										}
										else if (num != 1647479251U)
										{
											if (num == 2494566484U)
											{
												if (property == "VerticalAlignment")
												{
													return this.GetKnownBamlMember(-56);
												}
											}
										}
										else if (property == "MaxWidth")
										{
											return this.GetKnownBamlMember(-51);
										}
									}
									else if (num <= 2876759310U)
									{
										if (num != 2506401938U)
										{
											if (num != 2674639432U)
											{
												if (num == 2876759310U)
												{
													if (property == "MaxHeight")
													{
														return this.GetKnownBamlMember(-50);
													}
												}
											}
											else if (property == "MinHeight")
											{
												return this.GetKnownBamlMember(-52);
											}
										}
										else if (property == "Resources")
										{
											return this.Create_BamlProperty_FrameworkElement_Resources();
										}
									}
									else if (num <= 3301734811U)
									{
										if (num != 3286546394U)
										{
											if (num == 3301734811U)
											{
												if (property == "Margin")
												{
													return this.GetKnownBamlMember(-49);
												}
											}
										}
										else if (property == "FlowDirection")
										{
											return this.GetKnownBamlMember(-46);
										}
									}
									else if (num != 4270317017U)
									{
										if (num == 4285775472U)
										{
											if (property == "Triggers")
											{
												return this.Create_BamlProperty_FrameworkElement_Triggers();
											}
										}
									}
									else if (property == "MinWidth")
									{
										return this.GetKnownBamlMember(-53);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-153);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-179);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 790939867U)
					{
						if (typeNameHashForPropeties != 812041272U)
						{
							if (typeNameHashForPropeties == 826277256U)
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-142);
								}
								return null;
							}
						}
						else
						{
							if (property == "Header")
							{
								return this.GetKnownBamlMember(-65);
							}
							return null;
						}
					}
					else
					{
						if (property == "Resources")
						{
							return this.Create_BamlProperty_Application_Resources();
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1489718377U)
				{
					if (typeNameHashForPropeties <= 1197649600U)
					{
						if (typeNameHashForPropeties <= 1089745292U)
						{
							if (typeNameHashForPropeties <= 908592222U)
							{
								if (typeNameHashForPropeties != 837389320U)
								{
									if (typeNameHashForPropeties != 848944877U)
									{
										if (typeNameHashForPropeties == 908592222U)
										{
											if (property == "Items")
											{
												return this.GetKnownBamlMember(-255);
											}
											return null;
										}
									}
									else
									{
										if (property == "Command")
										{
											return this.GetKnownBamlMember(-7);
										}
										if (property == "CommandParameter")
										{
											return this.GetKnownBamlMember(-8);
										}
										if (property == "CommandTarget")
										{
											return this.GetKnownBamlMember(-9);
										}
										if (property == "IsPressed")
										{
											return this.GetKnownBamlMember(-10);
										}
										if (property == "Content")
										{
											return this.GetKnownBamlMember(-148);
										}
										if (!(property == "ClickMode"))
										{
											return null;
										}
										return this.Create_BamlProperty_ButtonBase_ClickMode();
									}
								}
								else
								{
									if (property == "SharedSizeGroup")
									{
										return this.Create_BamlProperty_DefinitionBase_SharedSizeGroup();
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 966650152U)
							{
								if (typeNameHashForPropeties != 971718127U)
								{
									if (typeNameHashForPropeties == 1089745292U)
									{
										uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
										if (num <= 2724873441U)
										{
											if (num <= 1574711207U)
											{
												if (num != 1041509726U)
												{
													if (num != 1240943717U)
													{
														if (num == 1574711207U)
														{
															if (property == "TextTrimming")
															{
																return this.GetKnownBamlMember(-112);
															}
														}
													}
													else if (property == "Inlines")
													{
														return this.GetKnownBamlMember(-246);
													}
												}
												else if (property == "Text")
												{
													return this.GetKnownBamlMember(-110);
												}
											}
											else if (num != 1707985138U)
											{
												if (num != 1813401013U)
												{
													if (num == 2724873441U)
													{
														if (property == "FontSize")
														{
															return this.GetKnownBamlMember(-105);
														}
													}
												}
												else if (property == "FontStyle")
												{
													return this.GetKnownBamlMember(-107);
												}
											}
											else if (property == "TextWrapping")
											{
												return this.GetKnownBamlMember(-113);
											}
										}
										else if (num <= 2994397609U)
										{
											if (num != 2812248845U)
											{
												if (num != 2844640867U)
												{
													if (num == 2994397609U)
													{
														if (property == "TextDecorations")
														{
															return this.GetKnownBamlMember(-111);
														}
													}
												}
												else if (property == "TextAlignment")
												{
													return this.Create_BamlProperty_TextBlock_TextAlignment();
												}
											}
											else if (property == "FontStretch")
											{
												return this.GetKnownBamlMember(-106);
											}
										}
										else if (num <= 3496045264U)
										{
											if (num != 3137079997U)
											{
												if (num == 3496045264U)
												{
													if (property == "FontWeight")
													{
														return this.GetKnownBamlMember(-108);
													}
												}
											}
											else if (property == "Background")
											{
												return this.GetKnownBamlMember(-103);
											}
										}
										else if (num != 3647682272U)
										{
											if (num == 4130445440U)
											{
												if (property == "FontFamily")
												{
													return this.GetKnownBamlMember(-104);
												}
											}
										}
										else if (property == "Foreground")
										{
											return this.GetKnownBamlMember(-109);
										}
										return null;
									}
								}
								else
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-254);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-129);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties <= 1147347271U)
						{
							if (typeNameHashForPropeties != 1133493129U)
							{
								if (typeNameHashForPropeties != 1133525638U)
								{
									if (typeNameHashForPropeties == 1147347271U)
									{
										if (property == "AncestorType")
										{
											return this.Create_BamlProperty_RelativeSource_AncestorType();
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-265);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-264);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 1169860818U)
						{
							if (typeNameHashForPropeties != 1171637538U)
							{
								if (typeNameHashForPropeties == 1197649600U)
								{
									if (property == "KeyFrames")
									{
										return this.GetKnownBamlMember(-189);
									}
									return null;
								}
							}
							else
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-154);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-201);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 1367449766U)
					{
						if (typeNameHashForPropeties <= 1343785127U)
						{
							if (typeNameHashForPropeties != 1236602933U)
							{
								if (typeNameHashForPropeties != 1262679173U)
								{
									if (typeNameHashForPropeties == 1343785127U)
									{
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-83);
										}
										return null;
									}
								}
								else
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-221);
									}
									return null;
								}
							}
							else
							{
								if (property == "LastChildFill")
								{
									return this.GetKnownBamlMember(-40);
								}
								if (!(property == "Children"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-162);
							}
						}
						else if (typeNameHashForPropeties != 1362944236U)
						{
							if (typeNameHashForPropeties != 1366062463U)
							{
								if (typeNameHashForPropeties == 1367449766U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 2748166707U)
									{
										if (num <= 1282463423U)
										{
											if (num != 121036118U)
											{
												if (num != 599956904U)
												{
													if (num == 1282463423U)
													{
														if (property == "BorderThickness")
														{
															return this.GetKnownBamlMember(-26);
														}
													}
												}
												else if (property == "TabIndex")
												{
													return this.GetKnownBamlMember(-36);
												}
											}
											else if (property == "Padding")
											{
												return this.GetKnownBamlMember(-35);
											}
										}
										else if (num <= 1813401013U)
										{
											if (num != 1704356651U)
											{
												if (num == 1813401013U)
												{
													if (property == "FontStyle")
													{
														return this.GetKnownBamlMember(-30);
													}
												}
											}
											else if (property == "Template")
											{
												return this.GetKnownBamlMember(-37);
											}
										}
										else if (num != 2724873441U)
										{
											if (num == 2748166707U)
											{
												if (property == "HorizontalContentAlignment")
												{
													return this.GetKnownBamlMember(-33);
												}
											}
										}
										else if (property == "FontSize")
										{
											return this.GetKnownBamlMember(-28);
										}
									}
									else if (num <= 3496045264U)
									{
										if (num <= 2985448305U)
										{
											if (num != 2812248845U)
											{
												if (num == 2985448305U)
												{
													if (property == "VerticalContentAlignment")
													{
														return this.GetKnownBamlMember(-38);
													}
												}
											}
											else if (property == "FontStretch")
											{
												return this.GetKnownBamlMember(-29);
											}
										}
										else if (num != 3137079997U)
										{
											if (num == 3496045264U)
											{
												if (property == "FontWeight")
												{
													return this.GetKnownBamlMember(-31);
												}
											}
										}
										else if (property == "Background")
										{
											return this.GetKnownBamlMember(-24);
										}
									}
									else if (num <= 3647682272U)
									{
										if (num != 3537472213U)
										{
											if (num == 3647682272U)
											{
												if (property == "Foreground")
												{
													return this.GetKnownBamlMember(-32);
												}
											}
										}
										else if (property == "BorderBrush")
										{
											return this.GetKnownBamlMember(-25);
										}
									}
									else if (num != 3770318996U)
									{
										if (num == 4130445440U)
										{
											if (property == "FontFamily")
											{
												return this.GetKnownBamlMember(-27);
											}
										}
									}
									else if (property == "IsTabStop")
									{
										return this.GetKnownBamlMember(-34);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-219);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-231);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 1462776703U)
					{
						if (typeNameHashForPropeties != 1374402354U)
						{
							if (typeNameHashForPropeties != 1376032174U)
							{
								if (typeNameHashForPropeties == 1462776703U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-249);
									}
									if (!(property == "Orientation"))
									{
										return null;
									}
									return this.Create_BamlProperty_ToolBar_Orientation();
								}
							}
							else
							{
								if (property == "VisualTree")
								{
									return this.GetKnownBamlMember(-158);
								}
								if (property == "Triggers")
								{
									return this.Create_BamlProperty_DataTemplate_Triggers();
								}
								if (property == "DataTemplateKey")
								{
									return this.Create_BamlProperty_DataTemplate_DataTemplateKey();
								}
								if (!(property == "DataType"))
								{
									return null;
								}
								return this.Create_BamlProperty_DataTemplate_DataType();
							}
						}
						else
						{
							if (property == "Setters")
							{
								return this.GetKnownBamlMember(-159);
							}
							if (property == "Value")
							{
								return this.Create_BamlProperty_DataTrigger_Value();
							}
							if (!(property == "Binding"))
							{
								return null;
							}
							return this.Create_BamlProperty_DataTrigger_Binding();
						}
					}
					else if (typeNameHashForPropeties != 1462961127U)
					{
						if (typeNameHashForPropeties != 1481865686U)
						{
							if (typeNameHashForPropeties == 1489718377U)
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-136);
								}
								return null;
							}
						}
						else
						{
							if (property == "Inlines")
							{
								return this.GetKnownBamlMember(-214);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-253);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1631317593U)
				{
					if (typeNameHashForPropeties <= 1539399457U)
					{
						if (typeNameHashForPropeties <= 1534050549U)
						{
							if (typeNameHashForPropeties != 1509448966U)
							{
								if (typeNameHashForPropeties != 1516882570U)
								{
									if (typeNameHashForPropeties == 1534050549U)
									{
										if (property == "Children")
										{
											return this.GetKnownBamlMember(-42);
										}
										return null;
									}
								}
								else
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-248);
									}
									return null;
								}
							}
							else
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-229);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 1535690395U)
						{
							if (typeNameHashForPropeties != 1536792507U)
							{
								if (typeNameHashForPropeties == 1539399457U)
								{
									if (property == "Content")
									{
										return this.GetKnownBamlMember(-212);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-225);
								}
								return null;
							}
						}
						else
						{
							if (property == "Child")
							{
								return this.GetKnownBamlMember(-139);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 1543401471U)
					{
						if (typeNameHashForPropeties != 1540591646U)
						{
							if (typeNameHashForPropeties != 1543239001U)
							{
								if (typeNameHashForPropeties == 1543401471U)
								{
									if (property == "Style")
									{
										return this.GetKnownBamlMember(-45);
									}
									if (property == "Name")
									{
										return this.Create_BamlProperty_FrameworkContentElement_Name();
									}
									if (!(property == "Resources"))
									{
										return null;
									}
									return this.Create_BamlProperty_FrameworkContentElement_Resources();
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-130);
								}
								return null;
							}
						}
						else
						{
							if (property == "CanContentScroll")
							{
								return this.GetKnownBamlMember(-97);
							}
							if (property == "HorizontalScrollBarVisibility")
							{
								return this.GetKnownBamlMember(-98);
							}
							if (property == "VerticalScrollBarVisibility")
							{
								return this.GetKnownBamlMember(-99);
							}
							if (!(property == "Content"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-227);
						}
					}
					else if (typeNameHashForPropeties != 1583456952U)
					{
						if (typeNameHashForPropeties != 1618471045U)
						{
							if (typeNameHashForPropeties == 1631317593U)
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-261);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-150);
							}
							return null;
						}
					}
					else
					{
						if (property == "KeyFrames")
						{
							return this.GetKnownBamlMember(-144);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1742124221U)
				{
					if (typeNameHashForPropeties <= 1663174964U)
					{
						if (typeNameHashForPropeties != 1632072630U)
						{
							if (typeNameHashForPropeties != 1646651323U)
							{
								if (typeNameHashForPropeties == 1663174964U)
								{
									if (property == "VisualTree")
									{
										return this.GetKnownBamlMember(-182);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-251);
								}
								return null;
							}
						}
						else
						{
							if (property == "Text")
							{
								return this.GetKnownBamlMember(-1);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 1681553739U)
					{
						if (typeNameHashForPropeties != 1732790398U)
						{
							if (typeNameHashForPropeties == 1742124221U)
							{
								if (property == "Blocks")
								{
									return this.GetKnownBamlMember(-172);
								}
								return null;
							}
						}
						else
						{
							if (property == "NavigateUri")
							{
								return this.GetKnownBamlMember(-74);
							}
							if (!(property == "Inlines"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-183);
						}
					}
					else
					{
						if (property == "Document")
						{
							return this.GetKnownBamlMember(-41);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 1797740290U)
				{
					if (typeNameHashForPropeties != 1752642139U)
					{
						if (typeNameHashForPropeties != 1796721919U)
						{
							if (typeNameHashForPropeties == 1797740290U)
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-262);
								}
								return null;
							}
						}
						else
						{
							if (property == "BeginTime")
							{
								return this.Create_BamlProperty_Timeline_BeginTime();
							}
							return null;
						}
					}
					else
					{
						if (property == "Items")
						{
							return this.GetKnownBamlMember(-141);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 1831113161U)
				{
					if (typeNameHashForPropeties != 1859848980U)
					{
						if (typeNameHashForPropeties == 1888893854U)
						{
							if (property == "Setters")
							{
								return this.GetKnownBamlMember(-209);
							}
							if (!(property == "Conditions"))
							{
								return null;
							}
							return this.Create_BamlProperty_MultiTrigger_Conditions();
						}
					}
					else
					{
						if (property == "Cells")
						{
							return this.GetKnownBamlMember(-244);
						}
						return null;
					}
				}
				else
				{
					if (property == "Pages")
					{
						return this.GetKnownBamlMember(-168);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 3154930786U)
			{
				if (typeNameHashForPropeties <= 2443733648U)
				{
					if (typeNameHashForPropeties <= 2127983347U)
					{
						if (typeNameHashForPropeties <= 2006016895U)
						{
							if (typeNameHashForPropeties <= 1957772275U)
							{
								if (typeNameHashForPropeties != 1891671667U)
								{
									if (typeNameHashForPropeties != 1940998317U)
									{
										if (typeNameHashForPropeties == 1957772275U)
										{
											if (property == "Blocks")
											{
												return this.GetKnownBamlMember(-199);
											}
											return null;
										}
									}
									else
									{
										if (property == "Bindings")
										{
											return this.GetKnownBamlMember(-218);
										}
										return null;
									}
								}
								else
								{
									if (property == "ToolBars")
									{
										return this.GetKnownBamlMember(-252);
									}
									return null;
								}
							}
							else if (typeNameHashForPropeties != 1971053987U)
							{
								if (typeNameHashForPropeties != 1971172509U)
								{
									if (typeNameHashForPropeties == 2006016895U)
									{
										if (property == "KeyFrames")
										{
											return this.GetKnownBamlMember(-260);
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-184);
									}
									return null;
								}
							}
							else
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-200);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties <= 2067968796U)
						{
							if (typeNameHashForPropeties != 2006957996U)
							{
								if (typeNameHashForPropeties != 2040874456U)
								{
									if (typeNameHashForPropeties == 2067968796U)
									{
										if (property == "IsStroked")
										{
											return this.Create_BamlProperty_PathSegment_IsStroked();
										}
										return null;
									}
								}
								else
								{
									if (property == "Value")
									{
										return this.Create_BamlProperty_Setter_Value();
									}
									if (property == "TargetName")
									{
										return this.Create_BamlProperty_Setter_TargetName();
									}
									if (!(property == "Property"))
									{
										return null;
									}
									return this.Create_BamlProperty_Setter_Property();
								}
							}
							else
							{
								if (property == "Property")
								{
									return this.Create_BamlProperty_Condition_Property();
								}
								if (property == "Value")
								{
									return this.Create_BamlProperty_Condition_Value();
								}
								if (!(property == "Binding"))
								{
									return null;
								}
								return this.Create_BamlProperty_Condition_Binding();
							}
						}
						else
						{
							if (typeNameHashForPropeties == 2075696131U)
							{
								uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
								if (num <= 2812248845U)
								{
									if (num != 1813401013U)
									{
										if (num != 2724873441U)
										{
											if (num == 2812248845U)
											{
												if (property == "FontStretch")
												{
													return this.GetKnownBamlMember(-118);
												}
											}
										}
										else if (property == "FontSize")
										{
											return this.GetKnownBamlMember(-117);
										}
									}
									else if (property == "FontStyle")
									{
										return this.GetKnownBamlMember(-119);
									}
								}
								else if (num <= 3496045264U)
								{
									if (num != 3137079997U)
									{
										if (num == 3496045264U)
										{
											if (property == "FontWeight")
											{
												return this.GetKnownBamlMember(-120);
											}
										}
									}
									else if (property == "Background")
									{
										return this.GetKnownBamlMember(-115);
									}
								}
								else if (num != 3647682272U)
								{
									if (num == 4130445440U)
									{
										if (property == "FontFamily")
										{
											return this.GetKnownBamlMember(-116);
										}
									}
								}
								else if (property == "Foreground")
								{
									return this.GetKnownBamlMember(-121);
								}
								return null;
							}
							if (typeNameHashForPropeties != 2108852657U)
							{
								if (typeNameHashForPropeties == 2127983347U)
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-233);
									}
									if (!(property == "Orientation"))
									{
										return null;
									}
									return this.Create_BamlProperty_StackPanel_Orientation();
								}
							}
							else
							{
								if (property == "Pen")
								{
									return this.Create_BamlProperty_GeometryDrawing_Pen();
								}
								return null;
							}
						}
					}
					else if (typeNameHashForPropeties <= 2246554763U)
					{
						if (typeNameHashForPropeties <= 2195627365U)
						{
							if (typeNameHashForPropeties != 2134797854U)
							{
								if (typeNameHashForPropeties != 2189110588U)
								{
									if (typeNameHashForPropeties == 2195627365U)
									{
										if (property == "Content")
										{
											return this.GetKnownBamlMember(-235);
										}
										return null;
									}
								}
								else
								{
									if (property == "ResourceId")
									{
										return this.Create_BamlProperty_ComponentResourceKey_ResourceId();
									}
									if (!(property == "TypeInTargetAssembly"))
									{
										return null;
									}
									return this.Create_BamlProperty_ComponentResourceKey_TypeInTargetAssembly();
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-247);
								}
								return null;
							}
						}
						else if (typeNameHashForPropeties != 2231796391U)
						{
							if (typeNameHashForPropeties != 2232234900U)
							{
								if (typeNameHashForPropeties == 2246554763U)
								{
									if (property == "Color")
									{
										return this.Create_BamlProperty_SolidColorBrush_Color();
									}
									return null;
								}
							}
							else
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-186);
								}
								return null;
							}
						}
						else
						{
							if (property == "Height")
							{
								return this.GetKnownBamlMember(-94);
							}
							if (property == "MaxHeight")
							{
								return this.GetKnownBamlMember(-95);
							}
							if (!(property == "MinHeight"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-96);
						}
					}
					else if (typeNameHashForPropeties <= 2369223502U)
					{
						if (typeNameHashForPropeties != 2299171064U)
						{
							if (typeNameHashForPropeties != 2361592662U)
							{
								if (typeNameHashForPropeties == 2369223502U)
								{
									if (property == "Child")
									{
										return this.GetKnownBamlMember(-138);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-149);
								}
								return null;
							}
						}
						else
						{
							if (property == "Setters")
							{
								return this.GetKnownBamlMember(-256);
							}
							if (property == "Value")
							{
								return this.Create_BamlProperty_Trigger_Value();
							}
							if (property == "SourceName")
							{
								return this.Create_BamlProperty_Trigger_SourceName();
							}
							if (!(property == "Property"))
							{
								return null;
							}
							return this.Create_BamlProperty_Trigger_Property();
						}
					}
					else
					{
						if (typeNameHashForPropeties == 2414917938U)
						{
							uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
							if (num <= 1507916019U)
							{
								if (num != 607785559U)
								{
									if (num != 1447095626U)
									{
										if (num == 1507916019U)
										{
											if (property == "ItemContainerStyleSelector")
											{
												return this.GetKnownBamlMember(-78);
											}
										}
									}
									else if (property == "ItemsSource")
									{
										return this.GetKnownBamlMember(-82);
									}
								}
								else if (property == "ItemsPanel")
								{
									return this.GetKnownBamlMember(-81);
								}
							}
							else if (num <= 1986528205U)
							{
								if (num != 1864236728U)
								{
									if (num == 1986528205U)
									{
										if (property == "ItemTemplateSelector")
										{
											return this.GetKnownBamlMember(-80);
										}
									}
								}
								else if (property == "ItemTemplate")
								{
									return this.GetKnownBamlMember(-79);
								}
							}
							else if (num != 2200388342U)
							{
								if (num == 3761649711U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-192);
									}
								}
							}
							else if (property == "ItemContainerStyle")
							{
								return this.GetKnownBamlMember(-77);
							}
							return null;
						}
						if (typeNameHashForPropeties != 2420033511U)
						{
							if (typeNameHashForPropeties == 2443733648U)
							{
								if (property == "Blocks")
								{
									return this.GetKnownBamlMember(-167);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-151);
							}
							return null;
						}
					}
				}
				else if (typeNameHashForPropeties <= 2685586543U)
				{
					if (typeNameHashForPropeties <= 2545195941U)
					{
						if (typeNameHashForPropeties <= 2495870938U)
						{
							if (typeNameHashForPropeties != 2450772053U)
							{
								if (typeNameHashForPropeties != 2495415765U)
								{
									if (typeNameHashForPropeties == 2495870938U)
									{
										if (property == "Blocks")
										{
											return this.GetKnownBamlMember(-228);
										}
										return null;
									}
								}
								else
								{
									if (property == "Child")
									{
										return this.GetKnownBamlMember(-185);
									}
									return null;
								}
							}
							else
							{
								if (property == "Content")
								{
									return this.GetKnownBamlMember(-266);
								}
								if (property == "ResizeMode")
								{
									return this.Create_BamlProperty_Window_ResizeMode();
								}
								if (property == "WindowState")
								{
									return this.Create_BamlProperty_Window_WindowState();
								}
								if (property == "Title")
								{
									return this.Create_BamlProperty_Window_Title();
								}
								if (!(property == "AllowsTransparency"))
								{
									return null;
								}
								return this.Create_BamlProperty_Window_AllowsTransparency();
							}
						}
						else if (typeNameHashForPropeties != 2497569086U)
						{
							if (typeNameHashForPropeties != 2545175141U)
							{
								if (typeNameHashForPropeties == 2545195941U)
								{
									if (property == "Document")
									{
										return this.GetKnownBamlMember(-43);
									}
									return null;
								}
							}
							else
							{
								if (property == "Document")
								{
									return this.GetKnownBamlMember(-173);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-58);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 2579011428U)
					{
						if (typeNameHashForPropeties != 2545205957U)
						{
							if (typeNameHashForPropeties != 2575003659U)
							{
								if (typeNameHashForPropeties == 2579011428U)
								{
									if (property == "KeyFrames")
									{
										return this.GetKnownBamlMember(-237);
									}
									return null;
								}
							}
							else
							{
								if (property == "GradientStops")
								{
									return this.GetKnownBamlMember(-195);
								}
								if (property == "StartPoint")
								{
									return this.Create_BamlProperty_LinearGradientBrush_StartPoint();
								}
								if (!(property == "EndPoint"))
								{
									return null;
								}
								return this.Create_BamlProperty_LinearGradientBrush_EndPoint();
							}
						}
						else
						{
							if (property == "Document")
							{
								return this.GetKnownBamlMember(-44);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 2579567368U)
					{
						if (typeNameHashForPropeties != 2615247465U)
						{
							if (typeNameHashForPropeties == 2685586543U)
							{
								if (property == "Items")
								{
									return this.GetKnownBamlMember(-204);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-160);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-198);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 2979510881U)
				{
					if (typeNameHashForPropeties <= 2714779469U)
					{
						if (typeNameHashForPropeties != 2692991063U)
						{
							if (typeNameHashForPropeties != 2699530403U)
							{
								if (typeNameHashForPropeties == 2714779469U)
								{
									uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
									if (num <= 1642243064U)
									{
										if (num <= 1374641664U)
										{
											if (num != 1236810612U)
											{
												if (num == 1374641664U)
												{
													if (property == "RelativeSource")
													{
														return this.Create_BamlProperty_Binding_RelativeSource();
													}
												}
											}
											else if (property == "ElementName")
											{
												return this.Create_BamlProperty_Binding_ElementName();
											}
										}
										else if (num != 1397651250U)
										{
											if (num == 1642243064U)
											{
												if (property == "Source")
												{
													return this.Create_BamlProperty_Binding_Source();
												}
											}
										}
										else if (property == "Mode")
										{
											return this.Create_BamlProperty_Binding_Mode();
										}
									}
									else if (num <= 3649220912U)
									{
										if (num != 2049819534U)
										{
											if (num == 3649220912U)
											{
												if (property == "XPath")
												{
													return this.Create_BamlProperty_Binding_XPath();
												}
											}
										}
										else if (property == "ConverterParameter")
										{
											return this.Create_BamlProperty_Binding_ConverterParameter();
										}
									}
									else if (num != 3652684527U)
									{
										if (num != 3846725399U)
										{
											if (num == 3949388886U)
											{
												if (property == "Path")
												{
													return this.Create_BamlProperty_Binding_Path();
												}
											}
										}
										else if (property == "UpdateSourceTrigger")
										{
											return this.Create_BamlProperty_Binding_UpdateSourceTrigger();
										}
									}
									else if (property == "Converter")
									{
										return this.Create_BamlProperty_Binding_Converter();
									}
									return null;
								}
							}
							else
							{
								if (property == "Command")
								{
									return this.Create_BamlProperty_CommandBinding_Command();
								}
								return null;
							}
						}
						else
						{
							if (property == "Items")
							{
								return this.GetKnownBamlMember(-205);
							}
							if (property == "Role")
							{
								return this.Create_BamlProperty_MenuItem_Role();
							}
							if (!(property == "IsChecked"))
							{
								return null;
							}
							return this.Create_BamlProperty_MenuItem_IsChecked();
						}
					}
					else if (typeNameHashForPropeties != 2742486520U)
					{
						if (typeNameHashForPropeties != 2762527090U)
						{
							if (typeNameHashForPropeties == 2979510881U)
							{
								if (property == "Content")
								{
									return this.GetKnownBamlMember(-155);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-59);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-152);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties <= 3079254648U)
				{
					if (typeNameHashForPropeties != 3042134663U)
					{
						if (typeNameHashForPropeties != 3051751957U)
						{
							if (typeNameHashForPropeties == 3079254648U)
							{
								if (property == "Background")
								{
									return this.GetKnownBamlMember(-4);
								}
								if (property == "BorderBrush")
								{
									return this.GetKnownBamlMember(-5);
								}
								if (property == "BorderThickness")
								{
									return this.GetKnownBamlMember(-6);
								}
								if (!(property == "Child"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-145);
							}
						}
						else
						{
							if (property == "Rows")
							{
								return this.GetKnownBamlMember(-245);
							}
							return null;
						}
					}
					else
					{
						if (property == "Items")
						{
							return this.GetKnownBamlMember(-156);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 3079776431U)
				{
					if (typeNameHashForPropeties != 3145595724U)
					{
						if (typeNameHashForPropeties == 3154930786U)
						{
							if (property == "Focusable")
							{
								return this.GetKnownBamlMember(-18);
							}
							return null;
						}
					}
					else
					{
						if (property == "Blocks")
						{
							return this.GetKnownBamlMember(-243);
						}
						return null;
					}
				}
				else
				{
					if (property == "VisualTree")
					{
						return this.GetKnownBamlMember(-174);
					}
					if (property == "Template")
					{
						return this.Create_BamlProperty_FrameworkTemplate_Template();
					}
					if (!(property == "Resources"))
					{
						return null;
					}
					return this.Create_BamlProperty_FrameworkTemplate_Resources();
				}
			}
			else if (typeNameHashForPropeties <= 3705841878U)
			{
				if (typeNameHashForPropeties <= 3329578860U)
				{
					if (typeNameHashForPropeties <= 3237288451U)
					{
						if (typeNameHashForPropeties <= 3215452047U)
						{
							if (typeNameHashForPropeties != 3159584246U)
							{
								if (typeNameHashForPropeties != 3203967083U)
								{
									if (typeNameHashForPropeties == 3215452047U)
									{
										if (property == "GradientStops")
										{
											return this.GetKnownBamlMember(-220);
										}
										return null;
									}
								}
								else
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-206);
									}
									return null;
								}
							}
							else
							{
								if (property == "VisualTree")
								{
									return this.GetKnownBamlMember(-157);
								}
								if (property == "Triggers")
								{
									return this.Create_BamlProperty_ControlTemplate_Triggers();
								}
								if (!(property == "TargetType"))
								{
									return null;
								}
								return this.Create_BamlProperty_ControlTemplate_TargetType();
							}
						}
						else if (typeNameHashForPropeties != 3221949491U)
						{
							if (typeNameHashForPropeties != 3222460427U)
							{
								if (typeNameHashForPropeties == 3237288451U)
								{
									if (property == "Children")
									{
										return this.GetKnownBamlMember(-241);
									}
									return null;
								}
							}
							else
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-170);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-215);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties <= 3251168569U)
					{
						if (typeNameHashForPropeties != 3239315111U)
						{
							if (typeNameHashForPropeties != 3250492243U)
							{
								if (typeNameHashForPropeties == 3251168569U)
								{
									if (property == "Items")
									{
										return this.GetKnownBamlMember(-197);
									}
									return null;
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-216);
								}
								return null;
							}
						}
						else
						{
							if (property == "KeyFrames")
							{
								return this.GetKnownBamlMember(-164);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 3251291345U)
					{
						if (typeNameHashForPropeties != 3256889750U)
						{
							if (typeNameHashForPropeties == 3329578860U)
							{
								if (property == "Child")
								{
									return this.GetKnownBamlMember(-211);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-240);
							}
							return null;
						}
					}
					else
					{
						if (property == "Bindings")
						{
							return this.GetKnownBamlMember(-207);
						}
						if (property == "Converter")
						{
							return this.Create_BamlProperty_MultiBinding_Converter();
						}
						if (!(property == "ConverterParameter"))
						{
							return null;
						}
						return this.Create_BamlProperty_MultiBinding_ConverterParameter();
					}
				}
				else if (typeNameHashForPropeties <= 3589500084U)
				{
					if (typeNameHashForPropeties <= 3484345457U)
					{
						if (typeNameHashForPropeties != 3359941107U)
						{
							if (typeNameHashForPropeties != 3423765539U)
							{
								if (typeNameHashForPropeties == 3484345457U)
								{
									if (property == "AcceptsTab")
									{
										return this.Create_BamlProperty_TextBoxBase_AcceptsTab();
									}
									if (property == "VerticalScrollBarVisibility")
									{
										return this.Create_BamlProperty_TextBoxBase_VerticalScrollBarVisibility();
									}
									if (!(property == "HorizontalScrollBarVisibility"))
									{
										return null;
									}
									return this.Create_BamlProperty_TextBoxBase_HorizontalScrollBarVisibility();
								}
							}
							else
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-230);
								}
								return null;
							}
						}
						else
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-223);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 3536639290U)
					{
						if (typeNameHashForPropeties != 3582123533U)
						{
							if (typeNameHashForPropeties == 3589500084U)
							{
								if (property == "KeyFrames")
								{
									return this.GetKnownBamlMember(-202);
								}
								return null;
							}
						}
						else
						{
							if (property == "Inlines")
							{
								return this.GetKnownBamlMember(-191);
							}
							return null;
						}
					}
					else
					{
						if (property == "Content")
						{
							return this.GetKnownBamlMember(-19);
						}
						if (property == "ContentSource")
						{
							return this.GetKnownBamlMember(-20);
						}
						if (property == "ContentTemplate")
						{
							return this.GetKnownBamlMember(-21);
						}
						if (property == "ContentTemplateSelector")
						{
							return this.GetKnownBamlMember(-22);
						}
						if (!(property == "RecognizesAccessKey"))
						{
							return null;
						}
						return this.GetKnownBamlMember(-23);
					}
				}
				else if (typeNameHashForPropeties <= 3693620786U)
				{
					if (typeNameHashForPropeties != 3607421190U)
					{
						if (typeNameHashForPropeties != 3627706972U)
						{
							if (typeNameHashForPropeties == 3693620786U)
							{
								if (property == "Children")
								{
									return this.GetKnownBamlMember(-236);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-122);
							}
							return null;
						}
					}
					else
					{
						if (property == "XmlSerializer")
						{
							return this.GetKnownBamlMember(-268);
						}
						if (!(property == "XPath"))
						{
							return null;
						}
						return this.Create_BamlProperty_XmlDataProvider_XPath();
					}
				}
				else if (typeNameHashForPropeties != 3696127683U)
				{
					if (typeNameHashForPropeties != 3699188754U)
					{
						if (typeNameHashForPropeties == 3705841878U)
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-147);
							}
							return null;
						}
					}
					else
					{
						if (property == "Blocks")
						{
							return this.GetKnownBamlMember(-140);
						}
						return null;
					}
				}
				else
				{
					if (property == "GradientStops")
					{
						return this.GetKnownBamlMember(-60);
					}
					if (!(property == "MappingMode"))
					{
						return null;
					}
					return this.Create_BamlProperty_GradientBrush_MappingMode();
				}
			}
			else if (typeNameHashForPropeties <= 4081990243U)
			{
				if (typeNameHashForPropeties <= 3936355701U)
				{
					if (typeNameHashForPropeties <= 3750147462U)
					{
						if (typeNameHashForPropeties != 3726396217U)
						{
							if (typeNameHashForPropeties != 3737794794U)
							{
								if (typeNameHashForPropeties == 3750147462U)
								{
									if (property == "Command")
									{
										return this.Create_BamlProperty_InputBinding_Command();
									}
									return null;
								}
							}
							else
							{
								if (property == "NameValue")
								{
									return this.GetKnownBamlMember(-187);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-258);
							}
							return null;
						}
					}
					else if (typeNameHashForPropeties != 3794574170U)
					{
						if (typeNameHashForPropeties != 3803038822U)
						{
							if (typeNameHashForPropeties == 3936355701U)
							{
								if (property == "Orientation")
								{
									return this.Create_BamlProperty_ScrollBar_Orientation();
								}
								return null;
							}
						}
						else
						{
							if (property == "Items")
							{
								return this.GetKnownBamlMember(-234);
							}
							return null;
						}
					}
					else
					{
						if (property == "HasHeader")
						{
							return this.GetKnownBamlMember(-70);
						}
						if (property == "Header")
						{
							return this.GetKnownBamlMember(-71);
						}
						if (property == "HeaderTemplate")
						{
							return this.GetKnownBamlMember(-72);
						}
						if (property == "HeaderTemplateSelector")
						{
							return this.GetKnownBamlMember(-73);
						}
						if (!(property == "Items"))
						{
							return null;
						}
						return this.GetKnownBamlMember(-181);
					}
				}
				else if (typeNameHashForPropeties <= 4042892829U)
				{
					if (typeNameHashForPropeties != 4013926948U)
					{
						if (typeNameHashForPropeties != 4019572119U)
						{
							if (typeNameHashForPropeties == 4042892829U)
							{
								if (property == "HasHeader")
								{
									return this.GetKnownBamlMember(-66);
								}
								if (property == "Header")
								{
									return this.GetKnownBamlMember(-67);
								}
								if (property == "HeaderTemplate")
								{
									return this.GetKnownBamlMember(-68);
								}
								if (property == "HeaderTemplateSelector")
								{
									return this.GetKnownBamlMember(-69);
								}
								if (!(property == "Content"))
								{
									return null;
								}
								return this.GetKnownBamlMember(-180);
							}
						}
						else
						{
							if (property == "Child")
							{
								return this.GetKnownBamlMember(-161);
							}
							return null;
						}
					}
					else
					{
						if (property == "KeyFrames")
						{
							return this.GetKnownBamlMember(-222);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 4049813583U)
				{
					if (typeNameHashForPropeties != 4060568379U)
					{
						if (typeNameHashForPropeties == 4081990243U)
						{
							uint num = <PrivateImplementationDetails>.ComputeStringHash(property);
							if (num <= 3174257263U)
							{
								if (num <= 1644367095U)
								{
									if (num != 1286637829U)
									{
										if (num == 1644367095U)
										{
											if (property == "ClipToBounds")
											{
												return this.GetKnownBamlMember(-131);
											}
										}
									}
									else if (property == "Visibility")
									{
										return this.GetKnownBamlMember(-135);
									}
								}
								else if (num != 1912421556U)
								{
									if (num != 3084753253U)
									{
										if (num == 3174257263U)
										{
											if (property == "Focusable")
											{
												return this.GetKnownBamlMember(-132);
											}
										}
									}
									else if (property == "InputBindings")
									{
										return this.Create_BamlProperty_UIElement_InputBindings();
									}
								}
								else if (property == "CommandBindings")
								{
									return this.Create_BamlProperty_UIElement_CommandBindings();
								}
							}
							else if (num <= 3668339052U)
							{
								if (num != 3209957741U)
								{
									if (num != 3541024718U)
									{
										if (num == 3668339052U)
										{
											if (property == "SnapsToDevicePixels")
											{
												return this.Create_BamlProperty_UIElement_SnapsToDevicePixels();
											}
										}
									}
									else if (property == "IsEnabled")
									{
										return this.GetKnownBamlMember(-133);
									}
								}
								else if (property == "Uid")
								{
									return this.Create_BamlProperty_UIElement_Uid();
								}
							}
							else if (num != 3697667181U)
							{
								if (num != 3924119651U)
								{
									if (num == 4043177991U)
									{
										if (property == "AllowDrop")
										{
											return this.Create_BamlProperty_UIElement_AllowDrop();
										}
									}
								}
								else if (property == "RenderTransform")
								{
									return this.GetKnownBamlMember(-134);
								}
							}
							else if (property == "RenderTransformOrigin")
							{
								return this.Create_BamlProperty_UIElement_RenderTransformOrigin();
							}
							return null;
						}
					}
					else
					{
						if (property == "KeyFrames")
						{
							return this.GetKnownBamlMember(-217);
						}
						return null;
					}
				}
				else
				{
					if (property == "Content")
					{
						return this.GetKnownBamlMember(-259);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 4237892661U)
			{
				if (typeNameHashForPropeties <= 4130373030U)
				{
					if (typeNameHashForPropeties != 4085468031U)
					{
						if (typeNameHashForPropeties != 4100099324U)
						{
							if (typeNameHashForPropeties == 4130373030U)
							{
								if (property == "Document")
								{
									return this.GetKnownBamlMember(-224);
								}
								return null;
							}
						}
						else
						{
							if (property == "Children")
							{
								return this.GetKnownBamlMember(-84);
							}
							return null;
						}
					}
					else
					{
						if (property == "Children")
						{
							return this.GetKnownBamlMember(-250);
						}
						return null;
					}
				}
				else if (typeNameHashForPropeties != 4206512190U)
				{
					if (typeNameHashForPropeties != 4223882185U)
					{
						if (typeNameHashForPropeties == 4237892661U)
						{
							if (property == "Content")
							{
								return this.GetKnownBamlMember(-14);
							}
							if (property == "ContentTemplate")
							{
								return this.GetKnownBamlMember(-15);
							}
							if (property == "ContentTemplateSelector")
							{
								return this.GetKnownBamlMember(-16);
							}
							if (!(property == "HasContent"))
							{
								return null;
							}
							return this.GetKnownBamlMember(-17);
						}
					}
					else
					{
						if (property == "Gesture")
						{
							return this.Create_BamlProperty_KeyBinding_Gesture();
						}
						if (!(property == "Key"))
						{
							return null;
						}
						return this.Create_BamlProperty_KeyBinding_Key();
					}
				}
				else
				{
					if (property == "Content")
					{
						return this.GetKnownBamlMember(-166);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 4254925692U)
			{
				if (typeNameHashForPropeties != 4251506749U)
				{
					if (typeNameHashForPropeties != 4253354066U)
					{
						if (typeNameHashForPropeties == 4254925692U)
						{
							if (property == "DeferrableContent")
							{
								return this.Create_BamlProperty_ResourceDictionary_DeferrableContent();
							}
							if (property == "Source")
							{
								return this.Create_BamlProperty_ResourceDictionary_Source();
							}
							if (!(property == "MergedDictionaries"))
							{
								return null;
							}
							return this.Create_BamlProperty_ResourceDictionary_MergedDictionaries();
						}
					}
					else
					{
						if (property == "Columns")
						{
							return this.GetKnownBamlMember(-176);
						}
						return null;
					}
				}
				else
				{
					if (property == "Inlines")
					{
						return this.GetKnownBamlMember(-257);
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties != 4272319926U)
			{
				if (typeNameHashForPropeties != 4281390711U)
				{
					if (typeNameHashForPropeties == 4284496765U)
					{
						if (property == "Actions")
						{
							return this.GetKnownBamlMember(-165);
						}
						if (property == "RoutedEvent")
						{
							return this.Create_BamlProperty_EventTrigger_RoutedEvent();
						}
						if (!(property == "SourceName"))
						{
							return null;
						}
						return this.Create_BamlProperty_EventTrigger_SourceName();
					}
				}
				else
				{
					if (property == "Blocks")
					{
						return this.GetKnownBamlMember(-171);
					}
					return null;
				}
			}
			else
			{
				if (property == "ObjectType")
				{
					return this.Create_BamlProperty_ObjectDataProvider_ObjectType();
				}
				return null;
			}
			return null;
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x001AFBEC File Offset: 0x001AEBEC
		internal WpfKnownMember CreateKnownAttachableMember(string type, string property)
		{
			uint typeNameHashForPropeties = this.GetTypeNameHashForPropeties(type);
			if (typeNameHashForPropeties <= 1236602933U)
			{
				if (typeNameHashForPropeties <= 249275044U)
				{
					if (typeNameHashForPropeties != 1173619U)
					{
						if (typeNameHashForPropeties == 249275044U)
						{
							if (property == "DirectionalNavigation")
							{
								return this.Create_BamlProperty_KeyboardNavigation_DirectionalNavigation();
							}
							if (!(property == "TabNavigation"))
							{
								return null;
							}
							return this.Create_BamlProperty_KeyboardNavigation_TabNavigation();
						}
					}
					else
					{
						if (property == "Column")
						{
							return this.GetKnownBamlMember(-61);
						}
						if (property == "ColumnSpan")
						{
							return this.GetKnownBamlMember(-62);
						}
						if (property == "Row")
						{
							return this.GetKnownBamlMember(-63);
						}
						if (!(property == "RowSpan"))
						{
							return null;
						}
						return this.GetKnownBamlMember(-64);
					}
				}
				else if (typeNameHashForPropeties != 378630271U)
				{
					if (typeNameHashForPropeties != 1133493129U)
					{
						if (typeNameHashForPropeties == 1236602933U)
						{
							if (property == "Dock")
							{
								return this.GetKnownBamlMember(-39);
							}
							return null;
						}
					}
					else
					{
						if (property == "IsVirtualizing")
						{
							return this.Create_BamlProperty_VirtualizingPanel_IsVirtualizing();
						}
						return null;
					}
				}
				else
				{
					if (property == "JournalEntryPosition")
					{
						return this.Create_BamlProperty_JournalEntryUnifiedViewConverter_JournalEntryPosition();
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties <= 1618471045U)
			{
				if (typeNameHashForPropeties != 1509448966U)
				{
					if (typeNameHashForPropeties == 1618471045U)
					{
						if (property == "Top")
						{
							return this.Create_BamlProperty_Canvas_Top();
						}
						if (property == "Left")
						{
							return this.Create_BamlProperty_Canvas_Left();
						}
						if (property == "Bottom")
						{
							return this.Create_BamlProperty_Canvas_Bottom();
						}
						if (!(property == "Right"))
						{
							return null;
						}
						return this.Create_BamlProperty_Canvas_Right();
					}
				}
				else
				{
					if (property == "IsSelected")
					{
						return this.Create_BamlProperty_Selector_IsSelected();
					}
					return null;
				}
			}
			else if (typeNameHashForPropeties != 3693620786U)
			{
				if (typeNameHashForPropeties != 3749867153U)
				{
					if (typeNameHashForPropeties == 3951806740U)
					{
						if (property == "ToolTip")
						{
							return this.Create_BamlProperty_ToolTipService_ToolTip();
						}
						return null;
					}
				}
				else
				{
					if (property == "NameScope")
					{
						return this.Create_BamlProperty_NameScope_NameScope();
					}
					return null;
				}
			}
			else
			{
				if (property == "TargetName")
				{
					return this.Create_BamlProperty_Storyboard_TargetName();
				}
				if (!(property == "TargetProperty"))
				{
					return null;
				}
				return this.Create_BamlProperty_Storyboard_TargetProperty();
			}
			return null;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x001AFE28 File Offset: 0x001AEE28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AccessText_Text()
		{
			DependencyProperty textProperty = AccessText.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AccessText)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x001AFE70 File Offset: 0x001AEE70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BeginStoryboard_Storyboard()
		{
			DependencyProperty storyboardProperty = BeginStoryboard.StoryboardProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BeginStoryboard)), "Storyboard", storyboardProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x001AFEA8 File Offset: 0x001AEEA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BitmapEffectGroup_Children()
		{
			DependencyProperty childrenProperty = BitmapEffectGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BitmapEffectGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x001AFEE0 File Offset: 0x001AEEE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_Background()
		{
			DependencyProperty backgroundProperty = Border.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x001AFF28 File Offset: 0x001AEF28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_BorderBrush()
		{
			DependencyProperty borderBrushProperty = Border.BorderBrushProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "BorderBrush", borderBrushProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x001AFF70 File Offset: 0x001AEF70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_BorderThickness()
		{
			DependencyProperty borderThicknessProperty = Border.BorderThicknessProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "BorderThickness", borderThicknessProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x001AFFB8 File Offset: 0x001AEFB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_Command()
		{
			DependencyProperty commandProperty = ButtonBase.CommandProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "Command", commandProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(CommandConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x001B0000 File Offset: 0x001AF000
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_CommandParameter()
		{
			DependencyProperty commandParameterProperty = ButtonBase.CommandParameterProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "CommandParameter", commandParameterProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x001B0050 File Offset: 0x001AF050
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_CommandTarget()
		{
			DependencyProperty commandTargetProperty = ButtonBase.CommandTargetProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "CommandTarget", commandTargetProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x001B0088 File Offset: 0x001AF088
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_IsPressed()
		{
			DependencyProperty isPressedProperty = ButtonBase.IsPressedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "IsPressed", isPressedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x001B00D8 File Offset: 0x001AF0D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColumnDefinition_MaxWidth()
		{
			DependencyProperty maxWidthProperty = ColumnDefinition.MaxWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColumnDefinition)), "MaxWidth", maxWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x001B0120 File Offset: 0x001AF120
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColumnDefinition_MinWidth()
		{
			DependencyProperty minWidthProperty = ColumnDefinition.MinWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColumnDefinition)), "MinWidth", minWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x001B0168 File Offset: 0x001AF168
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColumnDefinition_Width()
		{
			DependencyProperty widthProperty = ColumnDefinition.WidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColumnDefinition)), "Width", widthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(GridLengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x001B01B0 File Offset: 0x001AF1B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x001B0200 File Offset: 0x001AF200
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_ContentTemplate()
		{
			DependencyProperty contentTemplateProperty = ContentControl.ContentTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "ContentTemplate", contentTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x001B0238 File Offset: 0x001AF238
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_ContentTemplateSelector()
		{
			DependencyProperty contentTemplateSelectorProperty = ContentControl.ContentTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "ContentTemplateSelector", contentTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x001B0270 File Offset: 0x001AF270
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentControl_HasContent()
		{
			DependencyProperty hasContentProperty = ContentControl.HasContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentControl)), "HasContent", hasContentProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x001B02C0 File Offset: 0x001AF2C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentElement_Focusable()
		{
			DependencyProperty focusableProperty = ContentElement.FocusableProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentElement)), "Focusable", focusableProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x001B0308 File Offset: 0x001AF308
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_Content()
		{
			DependencyProperty contentProperty = ContentPresenter.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x001B0358 File Offset: 0x001AF358
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_ContentSource()
		{
			DependencyProperty contentSourceProperty = ContentPresenter.ContentSourceProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "ContentSource", contentSourceProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x001B03A0 File Offset: 0x001AF3A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_ContentTemplate()
		{
			DependencyProperty contentTemplateProperty = ContentPresenter.ContentTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "ContentTemplate", contentTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x001B03D8 File Offset: 0x001AF3D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_ContentTemplateSelector()
		{
			DependencyProperty contentTemplateSelectorProperty = ContentPresenter.ContentTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "ContentTemplateSelector", contentTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x001B0410 File Offset: 0x001AF410
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContentPresenter_RecognizesAccessKey()
		{
			DependencyProperty recognizesAccessKeyProperty = ContentPresenter.RecognizesAccessKeyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContentPresenter)), "RecognizesAccessKey", recognizesAccessKeyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x001B0458 File Offset: 0x001AF458
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Background()
		{
			DependencyProperty backgroundProperty = Control.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x001B04A0 File Offset: 0x001AF4A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_BorderBrush()
		{
			DependencyProperty borderBrushProperty = Control.BorderBrushProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "BorderBrush", borderBrushProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x001B04E8 File Offset: 0x001AF4E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_BorderThickness()
		{
			DependencyProperty borderThicknessProperty = Control.BorderThicknessProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "BorderThickness", borderThicknessProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x001B0530 File Offset: 0x001AF530
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontFamily()
		{
			DependencyProperty fontFamilyProperty = Control.FontFamilyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontFamily", fontFamilyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x001B0578 File Offset: 0x001AF578
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontSize()
		{
			DependencyProperty fontSizeProperty = Control.FontSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontSize", fontSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontSizeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x001B05C0 File Offset: 0x001AF5C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontStretch()
		{
			DependencyProperty fontStretchProperty = Control.FontStretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontStretch", fontStretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x001B0608 File Offset: 0x001AF608
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontStyle()
		{
			DependencyProperty fontStyleProperty = Control.FontStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontStyle", fontStyleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x001B0650 File Offset: 0x001AF650
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_FontWeight()
		{
			DependencyProperty fontWeightProperty = Control.FontWeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "FontWeight", fontWeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x001B0698 File Offset: 0x001AF698
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Foreground()
		{
			DependencyProperty foregroundProperty = Control.ForegroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Foreground", foregroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x001B06E0 File Offset: 0x001AF6E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_HorizontalContentAlignment()
		{
			DependencyProperty horizontalContentAlignmentProperty = Control.HorizontalContentAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "HorizontalContentAlignment", horizontalContentAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(HorizontalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x001B0728 File Offset: 0x001AF728
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_IsTabStop()
		{
			DependencyProperty isTabStopProperty = Control.IsTabStopProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "IsTabStop", isTabStopProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x001B0770 File Offset: 0x001AF770
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Padding()
		{
			DependencyProperty paddingProperty = Control.PaddingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Padding", paddingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x001B07B8 File Offset: 0x001AF7B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_TabIndex()
		{
			DependencyProperty tabIndexProperty = Control.TabIndexProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "TabIndex", tabIndexProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x001B0800 File Offset: 0x001AF800
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_Template()
		{
			DependencyProperty templateProperty = Control.TemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "Template", templateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x001B0838 File Offset: 0x001AF838
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Control_VerticalContentAlignment()
		{
			DependencyProperty verticalContentAlignmentProperty = Control.VerticalContentAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Control)), "VerticalContentAlignment", verticalContentAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(VerticalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x001B0880 File Offset: 0x001AF880
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DockPanel_Dock()
		{
			DependencyProperty dockProperty = DockPanel.DockProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DockPanel)), "Dock", dockProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Dock);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x001B08C8 File Offset: 0x001AF8C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DockPanel_LastChildFill()
		{
			DependencyProperty lastChildFillProperty = DockPanel.LastChildFillProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DockPanel)), "LastChildFill", lastChildFillProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x001B0910 File Offset: 0x001AF910
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DocumentViewerBase_Document()
		{
			DependencyProperty documentProperty = DocumentViewerBase.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DocumentViewerBase)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x001B0948 File Offset: 0x001AF948
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DrawingGroup_Children()
		{
			DependencyProperty childrenProperty = DrawingGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DrawingGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x001B0980 File Offset: 0x001AF980
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocumentReader_Document()
		{
			DependencyProperty documentProperty = FlowDocumentReader.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocumentReader)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x001B09B8 File Offset: 0x001AF9B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocumentScrollViewer_Document()
		{
			DependencyProperty documentProperty = FlowDocumentScrollViewer.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocumentScrollViewer)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x001B09F0 File Offset: 0x001AF9F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkContentElement_Style()
		{
			DependencyProperty styleProperty = FrameworkContentElement.StyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkContentElement)), "Style", styleProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x001B0A28 File Offset: 0x001AFA28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_FlowDirection()
		{
			DependencyProperty flowDirectionProperty = FrameworkElement.FlowDirectionProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "FlowDirection", flowDirectionProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FlowDirection);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x001B0A70 File Offset: 0x001AFA70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Height()
		{
			DependencyProperty heightProperty = FrameworkElement.HeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Height", heightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x001B0AB8 File Offset: 0x001AFAB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_HorizontalAlignment()
		{
			DependencyProperty horizontalAlignmentProperty = FrameworkElement.HorizontalAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "HorizontalAlignment", horizontalAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(HorizontalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x001B0B00 File Offset: 0x001AFB00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Margin()
		{
			DependencyProperty marginProperty = FrameworkElement.MarginProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Margin", marginProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x001B0B48 File Offset: 0x001AFB48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MaxHeight()
		{
			DependencyProperty maxHeightProperty = FrameworkElement.MaxHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MaxHeight", maxHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x001B0B90 File Offset: 0x001AFB90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MaxWidth()
		{
			DependencyProperty maxWidthProperty = FrameworkElement.MaxWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MaxWidth", maxWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x001B0BD8 File Offset: 0x001AFBD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MinHeight()
		{
			DependencyProperty minHeightProperty = FrameworkElement.MinHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MinHeight", minHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x001B0C20 File Offset: 0x001AFC20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_MinWidth()
		{
			DependencyProperty minWidthProperty = FrameworkElement.MinWidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "MinWidth", minWidthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x001B0C68 File Offset: 0x001AFC68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Name()
		{
			DependencyProperty nameProperty = FrameworkElement.NameProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Name", nameProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x001B0CB0 File Offset: 0x001AFCB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Style()
		{
			DependencyProperty styleProperty = FrameworkElement.StyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Style", styleProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x001B0CE8 File Offset: 0x001AFCE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_VerticalAlignment()
		{
			DependencyProperty verticalAlignmentProperty = FrameworkElement.VerticalAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "VerticalAlignment", verticalAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(VerticalAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x001B0D30 File Offset: 0x001AFD30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Width()
		{
			DependencyProperty widthProperty = FrameworkElement.WidthProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Width", widthProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x001B0D78 File Offset: 0x001AFD78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GeneralTransformGroup_Children()
		{
			DependencyProperty childrenProperty = GeneralTransformGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GeneralTransformGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x001B0DB0 File Offset: 0x001AFDB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GeometryGroup_Children()
		{
			DependencyProperty childrenProperty = GeometryGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GeometryGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x001B0DE8 File Offset: 0x001AFDE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GradientBrush_GradientStops()
		{
			DependencyProperty gradientStopsProperty = GradientBrush.GradientStopsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GradientBrush)), "GradientStops", gradientStopsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x001B0E20 File Offset: 0x001AFE20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_Column()
		{
			DependencyProperty columnProperty = Grid.ColumnProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "Column", columnProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x001B0E68 File Offset: 0x001AFE68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_ColumnSpan()
		{
			DependencyProperty columnSpanProperty = Grid.ColumnSpanProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "ColumnSpan", columnSpanProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x001B0EB0 File Offset: 0x001AFEB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_Row()
		{
			DependencyProperty rowProperty = Grid.RowProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "Row", rowProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x001B0EF8 File Offset: 0x001AFEF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_RowSpan()
		{
			DependencyProperty rowSpanProperty = Grid.RowSpanProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "RowSpan", rowSpanProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(Int32Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x001B0F40 File Offset: 0x001AFF40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GridViewColumn_Header()
		{
			DependencyProperty headerProperty = GridViewColumn.HeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GridViewColumn)), "Header", headerProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x001B0F90 File Offset: 0x001AFF90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_HasHeader()
		{
			DependencyProperty hasHeaderProperty = HeaderedContentControl.HasHeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "HasHeader", hasHeaderProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x001B0FE0 File Offset: 0x001AFFE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_Header()
		{
			DependencyProperty headerProperty = HeaderedContentControl.HeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "Header", headerProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x001B1030 File Offset: 0x001B0030
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_HeaderTemplate()
		{
			DependencyProperty headerTemplateProperty = HeaderedContentControl.HeaderTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "HeaderTemplate", headerTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x001B1068 File Offset: 0x001B0068
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_HeaderTemplateSelector()
		{
			DependencyProperty headerTemplateSelectorProperty = HeaderedContentControl.HeaderTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "HeaderTemplateSelector", headerTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x001B10A0 File Offset: 0x001B00A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_HasHeader()
		{
			DependencyProperty hasHeaderProperty = HeaderedItemsControl.HasHeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "HasHeader", hasHeaderProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x001B10F0 File Offset: 0x001B00F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_Header()
		{
			DependencyProperty headerProperty = HeaderedItemsControl.HeaderProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "Header", headerProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x001B1140 File Offset: 0x001B0140
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_HeaderTemplate()
		{
			DependencyProperty headerTemplateProperty = HeaderedItemsControl.HeaderTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "HeaderTemplate", headerTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x001B1178 File Offset: 0x001B0178
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_HeaderTemplateSelector()
		{
			DependencyProperty headerTemplateSelectorProperty = HeaderedItemsControl.HeaderTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "HeaderTemplateSelector", headerTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x001B11B0 File Offset: 0x001B01B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Hyperlink_NavigateUri()
		{
			DependencyProperty navigateUriProperty = Hyperlink.NavigateUriProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Hyperlink)), "NavigateUri", navigateUriProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(UriTypeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x001B11F8 File Offset: 0x001B01F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Image_Source()
		{
			DependencyProperty sourceProperty = Image.SourceProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Image)), "Source", sourceProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x001B1240 File Offset: 0x001B0240
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Image_Stretch()
		{
			DependencyProperty stretchProperty = Image.StretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Image)), "Stretch", stretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Stretch);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x001B1288 File Offset: 0x001B0288
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemContainerStyle()
		{
			DependencyProperty itemContainerStyleProperty = ItemsControl.ItemContainerStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemContainerStyle", itemContainerStyleProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x001B12C0 File Offset: 0x001B02C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemContainerStyleSelector()
		{
			DependencyProperty itemContainerStyleSelectorProperty = ItemsControl.ItemContainerStyleSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemContainerStyleSelector", itemContainerStyleSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x001B12F8 File Offset: 0x001B02F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemTemplate()
		{
			DependencyProperty itemTemplateProperty = ItemsControl.ItemTemplateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemTemplate", itemTemplateProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x001B1330 File Offset: 0x001B0330
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemTemplateSelector()
		{
			DependencyProperty itemTemplateSelectorProperty = ItemsControl.ItemTemplateSelectorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemTemplateSelector", itemTemplateSelectorProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x001B1368 File Offset: 0x001B0368
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemsPanel()
		{
			DependencyProperty itemsPanelProperty = ItemsControl.ItemsPanelProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemsPanel", itemsPanelProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x001B13A0 File Offset: 0x001B03A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_ItemsSource()
		{
			DependencyProperty itemsSourceProperty = ItemsControl.ItemsSourceProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "ItemsSource", itemsSourceProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x001B13D8 File Offset: 0x001B03D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MaterialGroup_Children()
		{
			DependencyProperty childrenProperty = MaterialGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MaterialGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x001B1410 File Offset: 0x001B0410
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Model3DGroup_Children()
		{
			DependencyProperty childrenProperty = Model3DGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Model3DGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x001B1448 File Offset: 0x001B0448
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Page_Content()
		{
			DependencyProperty contentProperty = Page.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Page)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x001B1498 File Offset: 0x001B0498
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Panel_Background()
		{
			DependencyProperty backgroundProperty = Panel.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Panel)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x001B14E0 File Offset: 0x001B04E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Path_Data()
		{
			DependencyProperty dataProperty = Path.DataProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Path)), "Data", dataProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(GeometryConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x001B1528 File Offset: 0x001B0528
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathFigure_Segments()
		{
			DependencyProperty segmentsProperty = PathFigure.SegmentsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathFigure)), "Segments", segmentsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x001B1560 File Offset: 0x001B0560
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathGeometry_Figures()
		{
			DependencyProperty figuresProperty = PathGeometry.FiguresProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathGeometry)), "Figures", figuresProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PathFigureCollectionConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x001B15A8 File Offset: 0x001B05A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_Child()
		{
			DependencyProperty childProperty = Popup.ChildProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "Child", childProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x001B15E0 File Offset: 0x001B05E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_IsOpen()
		{
			DependencyProperty isOpenProperty = Popup.IsOpenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "IsOpen", isOpenProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x001B1628 File Offset: 0x001B0628
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_Placement()
		{
			DependencyProperty placementProperty = Popup.PlacementProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "Placement", placementProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PlacementMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x001B1670 File Offset: 0x001B0670
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Popup_PopupAnimation()
		{
			DependencyProperty popupAnimationProperty = Popup.PopupAnimationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Popup)), "PopupAnimation", popupAnimationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PopupAnimation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x001B16B8 File Offset: 0x001B06B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RowDefinition_Height()
		{
			DependencyProperty heightProperty = RowDefinition.HeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RowDefinition)), "Height", heightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(GridLengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x001B1700 File Offset: 0x001B0700
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RowDefinition_MaxHeight()
		{
			DependencyProperty maxHeightProperty = RowDefinition.MaxHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RowDefinition)), "MaxHeight", maxHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x001B1748 File Offset: 0x001B0748
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RowDefinition_MinHeight()
		{
			DependencyProperty minHeightProperty = RowDefinition.MinHeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RowDefinition)), "MinHeight", minHeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x001B1790 File Offset: 0x001B0790
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_CanContentScroll()
		{
			DependencyProperty canContentScrollProperty = ScrollViewer.CanContentScrollProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "CanContentScroll", canContentScrollProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x001B17D8 File Offset: 0x001B07D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_HorizontalScrollBarVisibility()
		{
			DependencyProperty horizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "HorizontalScrollBarVisibility", horizontalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x001B1820 File Offset: 0x001B0820
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_VerticalScrollBarVisibility()
		{
			DependencyProperty verticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "VerticalScrollBarVisibility", verticalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x001B1868 File Offset: 0x001B0868
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_Fill()
		{
			DependencyProperty fillProperty = Shape.FillProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "Fill", fillProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x001B18B0 File Offset: 0x001B08B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_Stroke()
		{
			DependencyProperty strokeProperty = Shape.StrokeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "Stroke", strokeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x001B18F8 File Offset: 0x001B08F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeThickness()
		{
			DependencyProperty strokeThicknessProperty = Shape.StrokeThicknessProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeThickness", strokeThicknessProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x001B1940 File Offset: 0x001B0940
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Background()
		{
			DependencyProperty backgroundProperty = TextBlock.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x001B1988 File Offset: 0x001B0988
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontFamily()
		{
			DependencyProperty fontFamilyProperty = TextBlock.FontFamilyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontFamily", fontFamilyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x001B19D0 File Offset: 0x001B09D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontSize()
		{
			DependencyProperty fontSizeProperty = TextBlock.FontSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontSize", fontSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontSizeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x001B1A18 File Offset: 0x001B0A18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontStretch()
		{
			DependencyProperty fontStretchProperty = TextBlock.FontStretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontStretch", fontStretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x001B1A60 File Offset: 0x001B0A60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontStyle()
		{
			DependencyProperty fontStyleProperty = TextBlock.FontStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontStyle", fontStyleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x001B1AA8 File Offset: 0x001B0AA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_FontWeight()
		{
			DependencyProperty fontWeightProperty = TextBlock.FontWeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "FontWeight", fontWeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x001B1AF0 File Offset: 0x001B0AF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Foreground()
		{
			DependencyProperty foregroundProperty = TextBlock.ForegroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Foreground", foregroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x001B1B38 File Offset: 0x001B0B38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Text()
		{
			DependencyProperty textProperty = TextBlock.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x001B1B80 File Offset: 0x001B0B80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextDecorations()
		{
			DependencyProperty textDecorationsProperty = TextBlock.TextDecorationsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextDecorations", textDecorationsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextDecorationCollectionConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x001B1BC8 File Offset: 0x001B0BC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextTrimming()
		{
			DependencyProperty textTrimmingProperty = TextBlock.TextTrimmingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextTrimming", textTrimmingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextTrimming);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x001B1C10 File Offset: 0x001B0C10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextWrapping()
		{
			DependencyProperty textWrappingProperty = TextBlock.TextWrappingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextWrapping", textWrappingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextWrapping);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x001B1C58 File Offset: 0x001B0C58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBox_Text()
		{
			DependencyProperty textProperty = TextBox.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBox)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x001B1CA0 File Offset: 0x001B0CA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_Background()
		{
			DependencyProperty backgroundProperty = TextElement.BackgroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "Background", backgroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x001B1CE8 File Offset: 0x001B0CE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontFamily()
		{
			DependencyProperty fontFamilyProperty = TextElement.FontFamilyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontFamily", fontFamilyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x001B1D30 File Offset: 0x001B0D30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontSize()
		{
			DependencyProperty fontSizeProperty = TextElement.FontSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontSize", fontSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontSizeConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x001B1D78 File Offset: 0x001B0D78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontStretch()
		{
			DependencyProperty fontStretchProperty = TextElement.FontStretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontStretch", fontStretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x001B1DC0 File Offset: 0x001B0DC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontStyle()
		{
			DependencyProperty fontStyleProperty = TextElement.FontStyleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontStyle", fontStyleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x001B1E08 File Offset: 0x001B0E08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_FontWeight()
		{
			DependencyProperty fontWeightProperty = TextElement.FontWeightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "FontWeight", fontWeightProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x001B1E50 File Offset: 0x001B0E50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextElement_Foreground()
		{
			DependencyProperty foregroundProperty = TextElement.ForegroundProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextElement)), "Foreground", foregroundProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x001B1E98 File Offset: 0x001B0E98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TimelineGroup_Children()
		{
			DependencyProperty childrenProperty = TimelineGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TimelineGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x001B1ED0 File Offset: 0x001B0ED0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_IsDirectionReversed()
		{
			DependencyProperty isDirectionReversedProperty = Track.IsDirectionReversedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "IsDirectionReversed", isDirectionReversedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x001B1F18 File Offset: 0x001B0F18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Maximum()
		{
			DependencyProperty maximumProperty = Track.MaximumProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Maximum", maximumProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x001B1F60 File Offset: 0x001B0F60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Minimum()
		{
			DependencyProperty minimumProperty = Track.MinimumProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Minimum", minimumProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x001B1FA8 File Offset: 0x001B0FA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Orientation()
		{
			DependencyProperty orientationProperty = Track.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Orientation", orientationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x001B1FF0 File Offset: 0x001B0FF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Value()
		{
			DependencyProperty valueProperty = Track.ValueProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Value", valueProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x001B2038 File Offset: 0x001B1038
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_ViewportSize()
		{
			DependencyProperty viewportSizeProperty = Track.ViewportSizeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "ViewportSize", viewportSizeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x001B2080 File Offset: 0x001B1080
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Transform3DGroup_Children()
		{
			DependencyProperty childrenProperty = Transform3DGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Transform3DGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x001B20B8 File Offset: 0x001B10B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TransformGroup_Children()
		{
			DependencyProperty childrenProperty = TransformGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TransformGroup)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x001B20F0 File Offset: 0x001B10F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_ClipToBounds()
		{
			DependencyProperty clipToBoundsProperty = UIElement.ClipToBoundsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "ClipToBounds", clipToBoundsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x001B2138 File Offset: 0x001B1138
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_Focusable()
		{
			DependencyProperty focusableProperty = UIElement.FocusableProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "Focusable", focusableProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x001B2180 File Offset: 0x001B1180
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_IsEnabled()
		{
			DependencyProperty isEnabledProperty = UIElement.IsEnabledProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "IsEnabled", isEnabledProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x001B21C8 File Offset: 0x001B11C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_RenderTransform()
		{
			DependencyProperty renderTransformProperty = UIElement.RenderTransformProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "RenderTransform", renderTransformProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TransformConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x001B2210 File Offset: 0x001B1210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_Visibility()
		{
			DependencyProperty visibilityProperty = UIElement.VisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "Visibility", visibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Visibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x001B2258 File Offset: 0x001B1258
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Viewport3D_Children()
		{
			DependencyProperty childrenProperty = Viewport3D.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Viewport3D)), "Children", childrenProperty, true, false);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x001B2298 File Offset: 0x001B1298
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AdornedElementPlaceholder_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AdornedElementPlaceholder)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((AdornedElementPlaceholder)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((AdornedElementPlaceholder)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x001B231C File Offset: 0x001B131C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AdornerDecorator_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AdornerDecorator)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((AdornerDecorator)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((AdornerDecorator)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x001B23A0 File Offset: 0x001B13A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_AnchoredBlock_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(AnchoredBlock)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((AnchoredBlock)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x001B2408 File Offset: 0x001B1408
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ArrayExtension_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ArrayExtension)), "Items", typeof(IList), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ArrayExtension)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x001B2470 File Offset: 0x001B1470
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BlockUIContainer_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BlockUIContainer)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BlockUIContainer)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BlockUIContainer)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x001B24F4 File Offset: 0x001B14F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Bold_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Bold)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Bold)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x001B255C File Offset: 0x001B155C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BooleanAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BooleanAnimationUsingKeyFrames)), "KeyFrames", typeof(BooleanKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BooleanAnimationUsingKeyFrames)target).KeyFrames = (BooleanKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BooleanAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x001B25E0 File Offset: 0x001B15E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Border_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Border)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Border)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Border)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x001B2664 File Offset: 0x001B1664
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BulletDecorator_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BulletDecorator)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BulletDecorator)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BulletDecorator)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x001B26E8 File Offset: 0x001B16E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Button_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Button)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x001B2738 File Offset: 0x001B1738
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x001B2788 File Offset: 0x001B1788
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ByteAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ByteAnimationUsingKeyFrames)), "KeyFrames", typeof(ByteKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ByteAnimationUsingKeyFrames)target).KeyFrames = (ByteKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ByteAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x001B280C File Offset: 0x001B180C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Canvas)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x001B2874 File Offset: 0x001B1874
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_CharAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(CharAnimationUsingKeyFrames)), "KeyFrames", typeof(CharKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((CharAnimationUsingKeyFrames)target).KeyFrames = (CharKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((CharAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x001B28F8 File Offset: 0x001B18F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_CheckBox_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(CheckBox)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x001B2948 File Offset: 0x001B1948
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ColorAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ColorAnimationUsingKeyFrames)), "KeyFrames", typeof(ColorKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ColorAnimationUsingKeyFrames)target).KeyFrames = (ColorKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ColorAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x001B29CC File Offset: 0x001B19CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComboBox_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComboBox)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ComboBox)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x001B2A34 File Offset: 0x001B1A34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComboBoxItem_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComboBoxItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x001B2A84 File Offset: 0x001B1A84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ContextMenu_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ContextMenu)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ContextMenu)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x001B2AEC File Offset: 0x001B1AEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ControlTemplate_VisualTree()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ControlTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ControlTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ControlTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x001B2B70 File Offset: 0x001B1B70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_VisualTree()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x001B2BF4 File Offset: 0x001B1BF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTrigger_Setters()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTrigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((DataTrigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x001B2C5C File Offset: 0x001B1C5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DecimalAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DecimalAnimationUsingKeyFrames)), "KeyFrames", typeof(DecimalKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DecimalAnimationUsingKeyFrames)target).KeyFrames = (DecimalKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DecimalAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x001B2CE0 File Offset: 0x001B1CE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Decorator_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Decorator)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Decorator)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Decorator)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x001B2D64 File Offset: 0x001B1D64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DockPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DockPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((DockPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x001B2DCC File Offset: 0x001B1DCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DocumentViewer_Document()
		{
			DependencyProperty documentProperty = DocumentViewerBase.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DocumentViewer)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x001B2E04 File Offset: 0x001B1E04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DoubleAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DoubleAnimationUsingKeyFrames)), "KeyFrames", typeof(DoubleKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DoubleAnimationUsingKeyFrames)target).KeyFrames = (DoubleKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DoubleAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x001B2E88 File Offset: 0x001B1E88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_EventTrigger_Actions()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(EventTrigger)), "Actions", typeof(TriggerActionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((EventTrigger)target).Actions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x001B2EF0 File Offset: 0x001B1EF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Expander_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Expander)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x001B2F40 File Offset: 0x001B1F40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Figure_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Figure)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Figure)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x001B2FA8 File Offset: 0x001B1FA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FixedDocument_Pages()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FixedDocument)), "Pages", typeof(PageContentCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FixedDocument)target).Pages);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x001B3010 File Offset: 0x001B2010
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FixedDocumentSequence_References()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FixedDocumentSequence)), "References", typeof(DocumentReferenceCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FixedDocumentSequence)target).References);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x001B3078 File Offset: 0x001B2078
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FixedPage_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FixedPage)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FixedPage)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x001B30E0 File Offset: 0x001B20E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Floater_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Floater)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Floater)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x001B3148 File Offset: 0x001B2148
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocument_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocument)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FlowDocument)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x001B31B0 File Offset: 0x001B21B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FlowDocumentPageViewer_Document()
		{
			DependencyProperty documentProperty = DocumentViewerBase.DocumentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FlowDocumentPageViewer)), "Document", documentProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x001B31E8 File Offset: 0x001B21E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkTemplate_VisualTree()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x001B326C File Offset: 0x001B226C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Grid)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x001B32D4 File Offset: 0x001B22D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GridView_Columns()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GridView)), "Columns", typeof(GridViewColumnCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((GridView)target).Columns);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x001B333C File Offset: 0x001B233C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GridViewColumnHeader_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GridViewColumnHeader)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x001B338C File Offset: 0x001B238C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GroupBox_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GroupBox)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x001B33DC File Offset: 0x001B23DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GroupItem_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GroupItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x001B342C File Offset: 0x001B242C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedContentControl_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedContentControl)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x001B347C File Offset: 0x001B247C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HeaderedItemsControl_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HeaderedItemsControl)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((HeaderedItemsControl)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x001B34E4 File Offset: 0x001B24E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_HierarchicalDataTemplate_VisualTree()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(HierarchicalDataTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((HierarchicalDataTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((HierarchicalDataTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x001B3568 File Offset: 0x001B2568
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Hyperlink_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Hyperlink)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Hyperlink)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x001B35D0 File Offset: 0x001B25D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InkCanvas_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InkCanvas)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((InkCanvas)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x001B3638 File Offset: 0x001B2638
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InkPresenter_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InkPresenter)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((InkPresenter)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((InkPresenter)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x001B36BC File Offset: 0x001B26BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InlineUIContainer_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InlineUIContainer)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((InlineUIContainer)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((InlineUIContainer)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x001B3740 File Offset: 0x001B2740
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InputScopeName_NameValue()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InputScopeName)), "NameValue", typeof(InputScopeNameValue), false, false);
			wpfKnownMember.TypeConverterType = typeof(InputScopeNameValue);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((InputScopeName)target).NameValue = (InputScopeNameValue)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((InputScopeName)target).NameValue);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x001B37D4 File Offset: 0x001B27D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Int16AnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Int16AnimationUsingKeyFrames)), "KeyFrames", typeof(Int16KeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Int16AnimationUsingKeyFrames)target).KeyFrames = (Int16KeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Int16AnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x001B3858 File Offset: 0x001B2858
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Int32AnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Int32AnimationUsingKeyFrames)), "KeyFrames", typeof(Int32KeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Int32AnimationUsingKeyFrames)target).KeyFrames = (Int32KeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Int32AnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x001B38DC File Offset: 0x001B28DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Int64AnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Int64AnimationUsingKeyFrames)), "KeyFrames", typeof(Int64KeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Int64AnimationUsingKeyFrames)target).KeyFrames = (Int64KeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Int64AnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x001B3960 File Offset: 0x001B2960
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Italic_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Italic)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Italic)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x001B39C8 File Offset: 0x001B29C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsControl_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsControl)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ItemsControl)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x001B3A30 File Offset: 0x001B2A30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ItemsPanelTemplate_VisualTree()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ItemsPanelTemplate)), "VisualTree", typeof(FrameworkElementFactory), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ItemsPanelTemplate)target).VisualTree = (FrameworkElementFactory)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ItemsPanelTemplate)target).VisualTree);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x001B3AB4 File Offset: 0x001B2AB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Label_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Label)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x001B3B04 File Offset: 0x001B2B04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_LinearGradientBrush_GradientStops()
		{
			DependencyProperty gradientStopsProperty = GradientBrush.GradientStopsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(LinearGradientBrush)), "GradientStops", gradientStopsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x001B3B3C File Offset: 0x001B2B3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_List_ListItems()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(List)), "ListItems", typeof(ListItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((List)target).ListItems);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x001B3BA4 File Offset: 0x001B2BA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListBox_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListBox)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ListBox)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x001B3C0C File Offset: 0x001B2C0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListBoxItem_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListBoxItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x001B3C5C File Offset: 0x001B2C5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListItem_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListItem)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ListItem)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x001B3CC4 File Offset: 0x001B2CC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListView_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListView)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ListView)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x001B3D2C File Offset: 0x001B2D2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ListViewItem_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ListViewItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x001B3D7C File Offset: 0x001B2D7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MatrixAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MatrixAnimationUsingKeyFrames)), "KeyFrames", typeof(MatrixKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((MatrixAnimationUsingKeyFrames)target).KeyFrames = (MatrixKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((MatrixAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x001B3E00 File Offset: 0x001B2E00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Menu_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Menu)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Menu)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x001B3E68 File Offset: 0x001B2E68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuBase_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuBase)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MenuBase)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x001B3ED0 File Offset: 0x001B2ED0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuItem_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuItem)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MenuItem)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x001B3F38 File Offset: 0x001B2F38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ModelVisual3D_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ModelVisual3D)), "Children", typeof(Visual3DCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ModelVisual3D)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x001B3FA0 File Offset: 0x001B2FA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiBinding_Bindings()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiBinding)), "Bindings", typeof(Collection<BindingBase>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiBinding)target).Bindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x001B4008 File Offset: 0x001B3008
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiDataTrigger_Setters()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiDataTrigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiDataTrigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x001B4070 File Offset: 0x001B3070
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiTrigger_Setters()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiTrigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiTrigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x001B40D8 File Offset: 0x001B30D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ObjectAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ObjectAnimationUsingKeyFrames)), "KeyFrames", typeof(ObjectKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ObjectAnimationUsingKeyFrames)target).KeyFrames = (ObjectKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ObjectAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x001B415C File Offset: 0x001B315C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PageContent_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PageContent)), "Child", typeof(FixedPage), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((PageContent)target).Child = (FixedPage)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((PageContent)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x001B41E0 File Offset: 0x001B31E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PageFunctionBase_Content()
		{
			DependencyProperty contentProperty = Page.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PageFunctionBase)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x001B4230 File Offset: 0x001B3230
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Panel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Panel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Panel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x001B4298 File Offset: 0x001B3298
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Paragraph_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Paragraph)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Paragraph)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x001B4300 File Offset: 0x001B3300
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ParallelTimeline_Children()
		{
			DependencyProperty childrenProperty = TimelineGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ParallelTimeline)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x001B4338 File Offset: 0x001B3338
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Point3DAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Point3DAnimationUsingKeyFrames)), "KeyFrames", typeof(Point3DKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Point3DAnimationUsingKeyFrames)target).KeyFrames = (Point3DKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Point3DAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x001B43BC File Offset: 0x001B33BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PointAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PointAnimationUsingKeyFrames)), "KeyFrames", typeof(PointKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((PointAnimationUsingKeyFrames)target).KeyFrames = (PointKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((PointAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x001B4440 File Offset: 0x001B3440
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PriorityBinding_Bindings()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PriorityBinding)), "Bindings", typeof(Collection<BindingBase>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((PriorityBinding)target).Bindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x001B44A8 File Offset: 0x001B34A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_QuaternionAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(QuaternionAnimationUsingKeyFrames)), "KeyFrames", typeof(QuaternionKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((QuaternionAnimationUsingKeyFrames)target).KeyFrames = (QuaternionKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((QuaternionAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x001B452C File Offset: 0x001B352C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RadialGradientBrush_GradientStops()
		{
			DependencyProperty gradientStopsProperty = GradientBrush.GradientStopsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RadialGradientBrush)), "GradientStops", gradientStopsProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x001B4564 File Offset: 0x001B3564
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RadioButton_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RadioButton)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x001B45B4 File Offset: 0x001B35B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RectAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RectAnimationUsingKeyFrames)), "KeyFrames", typeof(RectKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((RectAnimationUsingKeyFrames)target).KeyFrames = (RectKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((RectAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E2F RID: 11823 RVA: 0x001B4638 File Offset: 0x001B3638
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RepeatButton_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RepeatButton)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x001B4688 File Offset: 0x001B3688
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RichTextBox_Document()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RichTextBox)), "Document", typeof(FlowDocument), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((RichTextBox)target).Document = (FlowDocument)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((RichTextBox)target).Document);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x001B470C File Offset: 0x001B370C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Rotation3DAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Rotation3DAnimationUsingKeyFrames)), "KeyFrames", typeof(Rotation3DKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Rotation3DAnimationUsingKeyFrames)target).KeyFrames = (Rotation3DKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Rotation3DAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x001B4790 File Offset: 0x001B3790
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Run_Text()
		{
			DependencyProperty textProperty = Run.TextProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Run)), "Text", textProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x001B47D8 File Offset: 0x001B37D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollViewer_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollViewer)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x001B4828 File Offset: 0x001B3828
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Section_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Section)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Section)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x001B4890 File Offset: 0x001B3890
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Selector_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Selector)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Selector)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x001B48F8 File Offset: 0x001B38F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_SingleAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(SingleAnimationUsingKeyFrames)), "KeyFrames", typeof(SingleKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((SingleAnimationUsingKeyFrames)target).KeyFrames = (SingleKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((SingleAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x001B497C File Offset: 0x001B397C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_SizeAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(SizeAnimationUsingKeyFrames)), "KeyFrames", typeof(SizeKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((SizeAnimationUsingKeyFrames)target).KeyFrames = (SizeKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((SizeAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x001B4A00 File Offset: 0x001B3A00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Span_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Span)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Span)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x001B4A68 File Offset: 0x001B3A68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StackPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StackPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((StackPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x001B4AD0 File Offset: 0x001B3AD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StatusBar_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StatusBar)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((StatusBar)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x001B4B38 File Offset: 0x001B3B38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StatusBarItem_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StatusBarItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x001B4B88 File Offset: 0x001B3B88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Storyboard_Children()
		{
			DependencyProperty childrenProperty = TimelineGroup.ChildrenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Storyboard)), "Children", childrenProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x001B4BC0 File Offset: 0x001B3BC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StringAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StringAnimationUsingKeyFrames)), "KeyFrames", typeof(StringKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((StringAnimationUsingKeyFrames)target).KeyFrames = (StringKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((StringAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x001B4C44 File Offset: 0x001B3C44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_Setters()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x001B4CAC File Offset: 0x001B3CAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TabControl_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TabControl)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TabControl)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x001B4D14 File Offset: 0x001B3D14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TabItem_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TabItem)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x001B4D64 File Offset: 0x001B3D64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TabPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TabPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TabPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x001B4DCC File Offset: 0x001B3DCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Table_RowGroups()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Table)), "RowGroups", typeof(TableRowGroupCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Table)target).RowGroups);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x001B4E34 File Offset: 0x001B3E34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TableCell_Blocks()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TableCell)), "Blocks", typeof(BlockCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TableCell)target).Blocks);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x001B4E9C File Offset: 0x001B3E9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TableRow_Cells()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TableRow)), "Cells", typeof(TableCellCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TableRow)target).Cells);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x001B4F04 File Offset: 0x001B3F04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TableRowGroup_Rows()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TableRowGroup)), "Rows", typeof(TableRowCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TableRowGroup)target).Rows);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x001B4F6C File Offset: 0x001B3F6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TextBlock)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x001B4FD4 File Offset: 0x001B3FD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ThicknessAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ThicknessAnimationUsingKeyFrames)), "KeyFrames", typeof(ThicknessKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ThicknessAnimationUsingKeyFrames)target).KeyFrames = (ThicknessKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ThicknessAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x001B5058 File Offset: 0x001B4058
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToggleButton_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToggleButton)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x001B50A8 File Offset: 0x001B40A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBar_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBar)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBar)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x001B5110 File Offset: 0x001B4110
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBarOverflowPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBarOverflowPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBarOverflowPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x001B5178 File Offset: 0x001B4178
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBarPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBarPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBarPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x001B51E0 File Offset: 0x001B41E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBarTray_ToolBars()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBarTray)), "ToolBars", typeof(Collection<ToolBar>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ToolBarTray)target).ToolBars);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x001B5248 File Offset: 0x001B4248
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolTip_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolTip)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x001B5298 File Offset: 0x001B4298
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TreeView_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TreeView)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TreeView)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x001B5300 File Offset: 0x001B4300
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TreeViewItem_Items()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TreeViewItem)), "Items", typeof(ItemCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((TreeViewItem)target).Items);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x001B5368 File Offset: 0x001B4368
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_Setters()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "Setters", typeof(SetterBaseCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).Setters);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x001B53D0 File Offset: 0x001B43D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Underline_Inlines()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Underline)), "Inlines", typeof(InlineCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Underline)target).Inlines);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x001B5438 File Offset: 0x001B4438
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UniformGrid_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UniformGrid)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((UniformGrid)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x001B54A0 File Offset: 0x001B44A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UserControl_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UserControl)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x001B54F0 File Offset: 0x001B44F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Vector3DAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Vector3DAnimationUsingKeyFrames)), "KeyFrames", typeof(Vector3DKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Vector3DAnimationUsingKeyFrames)target).KeyFrames = (Vector3DKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Vector3DAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x001B5574 File Offset: 0x001B4574
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VectorAnimationUsingKeyFrames_KeyFrames()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VectorAnimationUsingKeyFrames)), "KeyFrames", typeof(VectorKeyFrameCollection), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((VectorAnimationUsingKeyFrames)target).KeyFrames = (VectorKeyFrameCollection)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((VectorAnimationUsingKeyFrames)target).KeyFrames);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x001B55F8 File Offset: 0x001B45F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Viewbox_Child()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Viewbox)), "Child", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Viewbox)target).Child = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Viewbox)target).Child);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x001B567C File Offset: 0x001B467C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Viewport3DVisual_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Viewport3DVisual)), "Children", typeof(Visual3DCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Viewport3DVisual)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x001B56E4 File Offset: 0x001B46E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VirtualizingPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VirtualizingPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((VirtualizingPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x001B574C File Offset: 0x001B474C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VirtualizingStackPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VirtualizingStackPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((VirtualizingStackPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x001B57B4 File Offset: 0x001B47B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_Content()
		{
			DependencyProperty contentProperty = ContentControl.ContentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "Content", contentProperty, false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x001B5804 File Offset: 0x001B4804
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_WrapPanel_Children()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(WrapPanel)), "Children", typeof(UIElementCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((WrapPanel)target).Children);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x001B586C File Offset: 0x001B486C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_XmlDataProvider_XmlSerializer()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(XmlDataProvider)), "XmlSerializer", typeof(IXmlSerializable), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((XmlDataProvider)target).XmlSerializer);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x001B58D4 File Offset: 0x001B48D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ControlTemplate_Triggers()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ControlTemplate)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ControlTemplate)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x001B593C File Offset: 0x001B493C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_Triggers()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x001B59A4 File Offset: 0x001B49A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_DataTemplateKey()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "DataTemplateKey", typeof(object), true, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).DataTemplateKey);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x001B5A24 File Offset: 0x001B4A24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ControlTemplate_TargetType()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ControlTemplate)), "TargetType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ControlTemplate)target).TargetType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ControlTemplate)target).TargetType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x001B5AC8 File Offset: 0x001B4AC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Resources()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkElement)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkElement)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x001B5B54 File Offset: 0x001B4B54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkTemplate_Template()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkTemplate)), "Template", typeof(TemplateContent), false, false);
			wpfKnownMember.DeferringLoaderType = typeof(TemplateContentLoader);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkTemplate)target).Template = (TemplateContent)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkTemplate)target).Template);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x001B5BF0 File Offset: 0x001B4BF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_ColumnDefinitions()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "ColumnDefinitions", typeof(ColumnDefinitionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Grid)target).ColumnDefinitions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E64 RID: 11876 RVA: 0x001B5C58 File Offset: 0x001B4C58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Grid_RowDefinitions()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Grid)), "RowDefinitions", typeof(RowDefinitionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Grid)target).RowDefinitions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x001B5CC0 File Offset: 0x001B4CC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiTrigger_Conditions()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiTrigger)), "Conditions", typeof(ConditionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiTrigger)target).Conditions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x001B5D28 File Offset: 0x001B4D28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_NameScope_NameScope()
		{
			DependencyProperty nameScopeProperty = NameScope.NameScopeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(NameScope)), "NameScope", nameScopeProperty, false, true);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x001B5D60 File Offset: 0x001B4D60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_TargetType()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "TargetType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Style)target).TargetType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).TargetType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x001B5E04 File Offset: 0x001B4E04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_Triggers()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x001B5E6C File Offset: 0x001B4E6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Setter_Value()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Setter)), "Value", typeof(object), false, false);
			wpfKnownMember.TypeConverterType = typeof(SetterTriggerConditionValueConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Setter)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Setter)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E6A RID: 11882 RVA: 0x001B5F00 File Offset: 0x001B4F00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Setter_TargetName()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Setter)), "TargetName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Setter)target).TargetName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Setter)target).TargetName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x001B5F9C File Offset: 0x001B4F9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Path()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Path", typeof(PropertyPath), false, false);
			wpfKnownMember.TypeConverterType = typeof(PropertyPathConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Path = (PropertyPath)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Path);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x001B6030 File Offset: 0x001B5030
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComponentResourceKey_ResourceId()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComponentResourceKey)), "ResourceId", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ComponentResourceKey)target).ResourceId = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ComponentResourceKey)target).ResourceId);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x001B60CC File Offset: 0x001B50CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ComponentResourceKey_TypeInTargetAssembly()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ComponentResourceKey)), "TypeInTargetAssembly", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ComponentResourceKey)target).TypeInTargetAssembly = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ComponentResourceKey)target).TypeInTargetAssembly);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x001B6168 File Offset: 0x001B5168
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Converter()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Converter", typeof(IValueConverter), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Converter = (IValueConverter)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x001B61EC File Offset: 0x001B51EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Source()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Source", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Source = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Source);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x001B6288 File Offset: 0x001B5288
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_RelativeSource()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "RelativeSource", typeof(RelativeSource), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).RelativeSource = (RelativeSource)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).RelativeSource);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x001B630C File Offset: 0x001B530C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_Mode()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "Mode", typeof(BindingMode), false, false);
			wpfKnownMember.TypeConverterType = typeof(BindingMode);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).Mode = (BindingMode)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).Mode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x001B63A0 File Offset: 0x001B53A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Timeline_BeginTime()
		{
			DependencyProperty beginTimeProperty = Timeline.BeginTimeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Timeline)), "BeginTime", beginTimeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TimeSpanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x001B63E8 File Offset: 0x001B53E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_BasedOn()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "BasedOn", typeof(Style), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Style)target).BasedOn = (Style)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).BasedOn);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x001B6474 File Offset: 0x001B5474
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_ElementName()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "ElementName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).ElementName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).ElementName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x001B6508 File Offset: 0x001B5508
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_UpdateSourceTrigger()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "UpdateSourceTrigger", typeof(UpdateSourceTrigger), false, false);
			wpfKnownMember.TypeConverterType = typeof(UpdateSourceTrigger);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).UpdateSourceTrigger = (UpdateSourceTrigger)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).UpdateSourceTrigger);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x001B659C File Offset: 0x001B559C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ResourceDictionary_DeferrableContent()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ResourceDictionary)), "DeferrableContent", typeof(DeferrableContent), false, false);
			wpfKnownMember.TypeConverterType = typeof(DeferrableContentConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ResourceDictionary)target).DeferrableContent = (DeferrableContent)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ResourceDictionary)target).DeferrableContent);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x001B6630 File Offset: 0x001B5630
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_Value()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "Value", typeof(object), false, false);
			wpfKnownMember.TypeConverterType = typeof(SetterTriggerConditionValueConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Trigger)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x001B66C4 File Offset: 0x001B56C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_SourceName()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "SourceName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Trigger)target).SourceName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).SourceName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x001B6760 File Offset: 0x001B5760
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_RelativeSource_AncestorType()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(RelativeSource)), "AncestorType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((RelativeSource)target).AncestorType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((RelativeSource)target).AncestorType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x001B67FC File Offset: 0x001B57FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_Uid()
		{
			DependencyProperty uidProperty = UIElement.UidProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "Uid", uidProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x001B6844 File Offset: 0x001B5844
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkContentElement_Name()
		{
			DependencyProperty nameProperty = FrameworkContentElement.NameProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkContentElement)), "Name", nameProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x001B688C File Offset: 0x001B588C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkContentElement_Resources()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkContentElement)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkContentElement)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkContentElement)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x001B6918 File Offset: 0x001B5918
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Style_Resources()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Style)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Style)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Style)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x001B69A4 File Offset: 0x001B59A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkTemplate_Resources()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkTemplate)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((FrameworkTemplate)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkTemplate)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x001B6A30 File Offset: 0x001B5A30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Application_Resources()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Application)), "Resources", typeof(ResourceDictionary), false, false);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Application)target).Resources = (ResourceDictionary)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Application)target).Resources);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x001B6ABC File Offset: 0x001B5ABC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiBinding_Converter()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiBinding)), "Converter", typeof(IMultiValueConverter), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((MultiBinding)target).Converter = (IMultiValueConverter)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((MultiBinding)target).Converter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x001B6B40 File Offset: 0x001B5B40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiBinding_ConverterParameter()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiBinding)), "ConverterParameter", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((MultiBinding)target).ConverterParameter = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((MultiBinding)target).ConverterParameter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x001B6BDC File Offset: 0x001B5BDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_LinearGradientBrush_StartPoint()
		{
			DependencyProperty startPointProperty = LinearGradientBrush.StartPointProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(LinearGradientBrush)), "StartPoint", startPointProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PointConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x001B6C24 File Offset: 0x001B5C24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_LinearGradientBrush_EndPoint()
		{
			DependencyProperty endPointProperty = LinearGradientBrush.EndPointProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(LinearGradientBrush)), "EndPoint", endPointProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PointConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x001B6C6C File Offset: 0x001B5C6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_CommandBinding_Command()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(CommandBinding)), "Command", typeof(ICommand), false, false);
			wpfKnownMember.TypeConverterType = typeof(CommandConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((CommandBinding)target).Command = (ICommand)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((CommandBinding)target).Command);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x001B6D00 File Offset: 0x001B5D00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Condition_Property()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Condition)), "Property", typeof(DependencyProperty), false, false);
			wpfKnownMember.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Condition)target).Property = (DependencyProperty)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Condition)target).Property);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x001B6D9C File Offset: 0x001B5D9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Condition_Value()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Condition)), "Value", typeof(object), false, false);
			wpfKnownMember.TypeConverterType = typeof(SetterTriggerConditionValueConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Condition)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Condition)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x001B6E30 File Offset: 0x001B5E30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Condition_Binding()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Condition)), "Binding", typeof(BindingBase), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Condition)target).Binding = (BindingBase)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Condition)target).Binding);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x001B6EB4 File Offset: 0x001B5EB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BindingBase_FallbackValue()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BindingBase)), "FallbackValue", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BindingBase)target).FallbackValue = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BindingBase)target).FallbackValue);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x001B6F50 File Offset: 0x001B5F50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_ResizeMode()
		{
			DependencyProperty resizeModeProperty = Window.ResizeModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "ResizeMode", resizeModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ResizeMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x001B6F98 File Offset: 0x001B5F98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_WindowState()
		{
			DependencyProperty windowStateProperty = Window.WindowStateProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "WindowState", windowStateProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(WindowState);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x001B6FE0 File Offset: 0x001B5FE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_Title()
		{
			DependencyProperty titleProperty = Window.TitleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "Title", titleProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x001B7028 File Offset: 0x001B6028
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeLineJoin()
		{
			DependencyProperty strokeLineJoinProperty = Shape.StrokeLineJoinProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeLineJoin", strokeLineJoinProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineJoin);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x001B7070 File Offset: 0x001B6070
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeStartLineCap()
		{
			DependencyProperty strokeStartLineCapProperty = Shape.StrokeStartLineCapProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeStartLineCap", strokeStartLineCapProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineCap);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x001B70B8 File Offset: 0x001B60B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeEndLineCap()
		{
			DependencyProperty strokeEndLineCapProperty = Shape.StrokeEndLineCapProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeEndLineCap", strokeEndLineCapProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineCap);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x001B7100 File Offset: 0x001B6100
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TileBrush_TileMode()
		{
			DependencyProperty tileModeProperty = TileBrush.TileModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TileBrush)), "TileMode", tileModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TileMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x001B7148 File Offset: 0x001B6148
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TileBrush_ViewboxUnits()
		{
			DependencyProperty viewboxUnitsProperty = TileBrush.ViewboxUnitsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TileBrush)), "ViewboxUnits", viewboxUnitsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushMappingMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x001B7190 File Offset: 0x001B6190
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TileBrush_ViewportUnits()
		{
			DependencyProperty viewportUnitsProperty = TileBrush.ViewportUnitsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TileBrush)), "ViewportUnits", viewportUnitsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushMappingMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x001B71D8 File Offset: 0x001B61D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GeometryDrawing_Pen()
		{
			DependencyProperty penProperty = GeometryDrawing.PenProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GeometryDrawing)), "Pen", penProperty, false, false);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x001B7210 File Offset: 0x001B6210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBox_TextWrapping()
		{
			DependencyProperty textWrappingProperty = TextBox.TextWrappingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBox)), "TextWrapping", textWrappingProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextWrapping);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x001B7258 File Offset: 0x001B6258
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_StackPanel_Orientation()
		{
			DependencyProperty orientationProperty = StackPanel.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(StackPanel)), "Orientation", orientationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x001B72A0 File Offset: 0x001B62A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_Thumb()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "Thumb", typeof(Thumb), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Track)target).Thumb = (Thumb)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Track)target).Thumb);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x001B7324 File Offset: 0x001B6324
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_IncreaseRepeatButton()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "IncreaseRepeatButton", typeof(RepeatButton), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Track)target).IncreaseRepeatButton = (RepeatButton)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Track)target).IncreaseRepeatButton);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x001B73A8 File Offset: 0x001B63A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Track_DecreaseRepeatButton()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Track)), "DecreaseRepeatButton", typeof(RepeatButton), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Track)target).DecreaseRepeatButton = (RepeatButton)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Track)target).DecreaseRepeatButton);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x001B742C File Offset: 0x001B642C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_EventTrigger_RoutedEvent()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(EventTrigger)), "RoutedEvent", typeof(RoutedEvent), false, false);
			wpfKnownMember.TypeConverterType = typeof(RoutedEventConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((EventTrigger)target).RoutedEvent = (RoutedEvent)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((EventTrigger)target).RoutedEvent);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x001B74C0 File Offset: 0x001B64C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_InputBinding_Command()
		{
			DependencyProperty commandProperty = InputBinding.CommandProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(InputBinding)), "Command", commandProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(CommandConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x001B7508 File Offset: 0x001B6508
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyBinding_Gesture()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyBinding)), "Gesture", typeof(InputGesture), false, false);
			wpfKnownMember.TypeConverterType = typeof(KeyGestureConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((KeyBinding)target).Gesture = (InputGesture)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((KeyBinding)target).Gesture);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x001B759C File Offset: 0x001B659C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBox_TextAlignment()
		{
			DependencyProperty textAlignmentProperty = TextBox.TextAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBox)), "TextAlignment", textAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x001B75E4 File Offset: 0x001B65E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBlock_TextAlignment()
		{
			DependencyProperty textAlignmentProperty = TextBlock.TextAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBlock)), "TextAlignment", textAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x001B762C File Offset: 0x001B662C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_JournalEntryUnifiedViewConverter_JournalEntryPosition()
		{
			DependencyProperty journalEntryPositionProperty = JournalEntryUnifiedViewConverter.JournalEntryPositionProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(JournalEntryUnifiedViewConverter)), "JournalEntryPosition", journalEntryPositionProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(JournalEntryPosition);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x001B7674 File Offset: 0x001B6674
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_GradientBrush_MappingMode()
		{
			DependencyProperty mappingModeProperty = GradientBrush.MappingModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(GradientBrush)), "MappingMode", mappingModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BrushMappingMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x001B76BC File Offset: 0x001B66BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuItem_Role()
		{
			DependencyProperty roleProperty = MenuItem.RoleProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuItem)), "Role", roleProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(MenuItemRole);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x001B770C File Offset: 0x001B670C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTrigger_Value()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTrigger)), "Value", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTrigger)target).Value = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTrigger)target).Value);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x001B77A8 File Offset: 0x001B67A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTrigger_Binding()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTrigger)), "Binding", typeof(BindingBase), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTrigger)target).Binding = (BindingBase)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTrigger)target).Binding);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x001B782C File Offset: 0x001B682C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Setter_Property()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Setter)), "Property", typeof(DependencyProperty), false, false);
			wpfKnownMember.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Setter)target).Property = (DependencyProperty)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Setter)target).Property);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x001B78C8 File Offset: 0x001B68C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ResourceDictionary_Source()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ResourceDictionary)), "Source", typeof(Uri), false, false);
			wpfKnownMember.TypeConverterType = typeof(UriTypeConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ResourceDictionary)target).Source = (Uri)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ResourceDictionary)target).Source);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x001B795C File Offset: 0x001B695C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BeginStoryboard_Name()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BeginStoryboard)), "Name", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BeginStoryboard)target).Name = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BeginStoryboard)target).Name);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x001B79F0 File Offset: 0x001B69F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ResourceDictionary_MergedDictionaries()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ResourceDictionary)), "MergedDictionaries", typeof(Collection<ResourceDictionary>), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((ResourceDictionary)target).MergedDictionaries);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x001B7A58 File Offset: 0x001B6A58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyboardNavigation_DirectionalNavigation()
		{
			DependencyProperty directionalNavigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyboardNavigation)), "DirectionalNavigation", directionalNavigationProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(KeyboardNavigationMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x001B7AA0 File Offset: 0x001B6AA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyboardNavigation_TabNavigation()
		{
			DependencyProperty tabNavigationProperty = KeyboardNavigation.TabNavigationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyboardNavigation)), "TabNavigation", tabNavigationProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(KeyboardNavigationMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x001B7AE8 File Offset: 0x001B6AE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ScrollBar_Orientation()
		{
			DependencyProperty orientationProperty = ScrollBar.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ScrollBar)), "Orientation", orientationProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x001B7B30 File Offset: 0x001B6B30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Trigger_Property()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Trigger)), "Property", typeof(DependencyProperty), false, false);
			wpfKnownMember.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Trigger)target).Property = (DependencyProperty)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Trigger)target).Property);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x001B7BCC File Offset: 0x001B6BCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_EventTrigger_SourceName()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(EventTrigger)), "SourceName", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((EventTrigger)target).SourceName = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((EventTrigger)target).SourceName);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x001B7C60 File Offset: 0x001B6C60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DefinitionBase_SharedSizeGroup()
		{
			DependencyProperty sharedSizeGroupProperty = DefinitionBase.SharedSizeGroupProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DefinitionBase)), "SharedSizeGroup", sharedSizeGroupProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x001B7CA8 File Offset: 0x001B6CA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolTipService_ToolTip()
		{
			DependencyProperty toolTipProperty = ToolTipService.ToolTipProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolTipService)), "ToolTip", toolTipProperty, false, true);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x001B7CF8 File Offset: 0x001B6CF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathFigure_IsClosed()
		{
			DependencyProperty isClosedProperty = PathFigure.IsClosedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathFigure)), "IsClosed", isClosedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x001B7D40 File Offset: 0x001B6D40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathFigure_IsFilled()
		{
			DependencyProperty isFilledProperty = PathFigure.IsFilledProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathFigure)), "IsFilled", isFilledProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x001B7D88 File Offset: 0x001B6D88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ButtonBase_ClickMode()
		{
			DependencyProperty clickModeProperty = ButtonBase.ClickModeProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ButtonBase)), "ClickMode", clickModeProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ClickMode);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x001B7DD0 File Offset: 0x001B6DD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Block_TextAlignment()
		{
			DependencyProperty textAlignmentProperty = Block.TextAlignmentProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Block)), "TextAlignment", textAlignmentProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(TextAlignment);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x001B7E18 File Offset: 0x001B6E18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_RenderTransformOrigin()
		{
			DependencyProperty renderTransformOriginProperty = UIElement.RenderTransformOriginProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "RenderTransformOrigin", renderTransformOriginProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PointConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x001B7E60 File Offset: 0x001B6E60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Pen_LineJoin()
		{
			DependencyProperty lineJoinProperty = Pen.LineJoinProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Pen)), "LineJoin", lineJoinProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(PenLineJoin);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x001B7EA8 File Offset: 0x001B6EA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_BulletDecorator_Bullet()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(BulletDecorator)), "Bullet", typeof(UIElement), false, false);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((BulletDecorator)target).Bullet = (UIElement)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((BulletDecorator)target).Bullet);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x001B7F2C File Offset: 0x001B6F2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_SnapsToDevicePixels()
		{
			DependencyProperty snapsToDevicePixelsProperty = UIElement.SnapsToDevicePixelsProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "SnapsToDevicePixels", snapsToDevicePixelsProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x001B7F74 File Offset: 0x001B6F74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_CommandBindings()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "CommandBindings", typeof(CommandBindingCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((UIElement)target).CommandBindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x001B7FDC File Offset: 0x001B6FDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_InputBindings()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "InputBindings", typeof(InputBindingCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((UIElement)target).InputBindings);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x001B8044 File Offset: 0x001B7044
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_SolidColorBrush_Color()
		{
			DependencyProperty colorProperty = SolidColorBrush.ColorProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(SolidColorBrush)), "Color", colorProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ColorConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x001B808C File Offset: 0x001B708C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Brush_Opacity()
		{
			DependencyProperty opacityProperty = Brush.OpacityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Brush)), "Opacity", opacityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x001B80D4 File Offset: 0x001B70D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBoxBase_AcceptsTab()
		{
			DependencyProperty acceptsTabProperty = TextBoxBase.AcceptsTabProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBoxBase)), "AcceptsTab", acceptsTabProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x001B811C File Offset: 0x001B711C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_PathSegment_IsStroked()
		{
			DependencyProperty isStrokedProperty = PathSegment.IsStrokedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(PathSegment)), "IsStroked", isStrokedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x001B8164 File Offset: 0x001B7164
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_VirtualizingPanel_IsVirtualizing()
		{
			DependencyProperty isVirtualizingProperty = VirtualizingPanel.IsVirtualizingProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(VirtualizingPanel)), "IsVirtualizing", isVirtualizingProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x001B81AC File Offset: 0x001B71AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_Stretch()
		{
			DependencyProperty stretchProperty = Shape.StretchProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "Stretch", stretchProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(Stretch);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x001B81F4 File Offset: 0x001B71F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Frame_JournalOwnership()
		{
			DependencyProperty journalOwnershipProperty = Frame.JournalOwnershipProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Frame)), "JournalOwnership", journalOwnershipProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(JournalOwnership);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x001B823C File Offset: 0x001B723C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Frame_NavigationUIVisibility()
		{
			DependencyProperty navigationUIVisibilityProperty = Frame.NavigationUIVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Frame)), "NavigationUIVisibility", navigationUIVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(NavigationUIVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x001B8284 File Offset: 0x001B7284
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Storyboard_TargetName()
		{
			DependencyProperty targetNameProperty = Storyboard.TargetNameProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Storyboard)), "TargetName", targetNameProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x001B82CC File Offset: 0x001B72CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_XmlDataProvider_XPath()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(XmlDataProvider)), "XPath", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((XmlDataProvider)target).XPath = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((XmlDataProvider)target).XPath);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC1 RID: 11969 RVA: 0x001B8360 File Offset: 0x001B7360
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Selector_IsSelected()
		{
			DependencyProperty isSelectedProperty = Selector.IsSelectedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Selector)), "IsSelected", isSelectedProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x001B83A8 File Offset: 0x001B73A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_DataTemplate_DataType()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(DataTemplate)), "DataType", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.Ambient = true;
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((DataTemplate)target).DataType = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((DataTemplate)target).DataType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x001B844C File Offset: 0x001B744C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Shape_StrokeMiterLimit()
		{
			DependencyProperty strokeMiterLimitProperty = Shape.StrokeMiterLimitProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Shape)), "StrokeMiterLimit", strokeMiterLimitProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(DoubleConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x001B8494 File Offset: 0x001B7494
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_UIElement_AllowDrop()
		{
			DependencyProperty allowDropProperty = UIElement.AllowDropProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(UIElement)), "AllowDrop", allowDropProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x001B84DC File Offset: 0x001B74DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MenuItem_IsChecked()
		{
			DependencyProperty isCheckedProperty = MenuItem.IsCheckedProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MenuItem)), "IsChecked", isCheckedProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x001B8524 File Offset: 0x001B7524
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Panel_IsItemsHost()
		{
			DependencyProperty isItemsHostProperty = Panel.IsItemsHostProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Panel)), "IsItemsHost", isItemsHostProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x001B856C File Offset: 0x001B756C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_XPath()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "XPath", typeof(string), false, false);
			wpfKnownMember.TypeConverterType = typeof(StringConverter);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).XPath = (string)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).XPath);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x001B8600 File Offset: 0x001B7600
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Window_AllowsTransparency()
		{
			DependencyProperty allowsTransparencyProperty = Window.AllowsTransparencyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Window)), "AllowsTransparency", allowsTransparencyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(BooleanConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x001B8648 File Offset: 0x001B7648
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ObjectDataProvider_ObjectType()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ObjectDataProvider)), "ObjectType", typeof(Type), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(Type);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((ObjectDataProvider)target).ObjectType = (Type)value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((ObjectDataProvider)target).ObjectType);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x001B86E4 File Offset: 0x001B76E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_ToolBar_Orientation()
		{
			DependencyProperty orientationProperty = ToolBar.OrientationProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(ToolBar)), "Orientation", orientationProperty, true, false);
			wpfKnownMember.TypeConverterType = typeof(Orientation);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x001B8734 File Offset: 0x001B7734
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBoxBase_VerticalScrollBarVisibility()
		{
			DependencyProperty verticalScrollBarVisibilityProperty = TextBoxBase.VerticalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBoxBase)), "VerticalScrollBarVisibility", verticalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ECC RID: 11980 RVA: 0x001B877C File Offset: 0x001B777C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_TextBoxBase_HorizontalScrollBarVisibility()
		{
			DependencyProperty horizontalScrollBarVisibilityProperty = TextBoxBase.HorizontalScrollBarVisibilityProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(TextBoxBase)), "HorizontalScrollBarVisibility", horizontalScrollBarVisibilityProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x001B87C4 File Offset: 0x001B77C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_FrameworkElement_Triggers()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(FrameworkElement)), "Triggers", typeof(TriggerCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((FrameworkElement)target).Triggers);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x001B882C File Offset: 0x001B782C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_MultiDataTrigger_Conditions()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(MultiDataTrigger)), "Conditions", typeof(ConditionCollection), true, false);
			wpfKnownMember.GetDelegate = ((object target) => ((MultiDataTrigger)target).Conditions);
			wpfKnownMember.IsWritePrivate = true;
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x001B8894 File Offset: 0x001B7894
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_KeyBinding_Key()
		{
			DependencyProperty keyProperty = KeyBinding.KeyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(KeyBinding)), "Key", keyProperty, false, false);
			wpfKnownMember.TypeConverterType = typeof(KeyConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x001B88DC File Offset: 0x001B78DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Binding_ConverterParameter()
		{
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Binding)), "ConverterParameter", typeof(object), false, false);
			wpfKnownMember.HasSpecialTypeConverter = true;
			wpfKnownMember.TypeConverterType = typeof(object);
			wpfKnownMember.SetDelegate = delegate(object target, object value)
			{
				((Binding)target).ConverterParameter = value;
			};
			wpfKnownMember.GetDelegate = ((object target) => ((Binding)target).ConverterParameter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x001B8978 File Offset: 0x001B7978
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Top()
		{
			DependencyProperty topProperty = Canvas.TopProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Top", topProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x001B89C0 File Offset: 0x001B79C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Left()
		{
			DependencyProperty leftProperty = Canvas.LeftProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Left", leftProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x001B8A08 File Offset: 0x001B7A08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Bottom()
		{
			DependencyProperty bottomProperty = Canvas.BottomProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Bottom", bottomProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x001B8A50 File Offset: 0x001B7A50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Canvas_Right()
		{
			DependencyProperty rightProperty = Canvas.RightProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Canvas)), "Right", rightProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(LengthConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x001B8A98 File Offset: 0x001B7A98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownMember Create_BamlProperty_Storyboard_TargetProperty()
		{
			DependencyProperty targetPropertyProperty = Storyboard.TargetPropertyProperty;
			WpfKnownMember wpfKnownMember = new WpfKnownMember(this, this.GetXamlType(typeof(Storyboard)), "TargetProperty", targetPropertyProperty, false, true);
			wpfKnownMember.TypeConverterType = typeof(PropertyPathConverter);
			wpfKnownMember.Freeze();
			return wpfKnownMember;
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x001B8AE0 File Offset: 0x001B7AE0
		private WpfKnownType CreateKnownBamlType(short bamlNumber, bool isBamlType, bool useV3Rules)
		{
			switch (bamlNumber)
			{
			case 1:
				return this.Create_BamlType_AccessText(isBamlType, useV3Rules);
			case 2:
				return this.Create_BamlType_AdornedElementPlaceholder(isBamlType, useV3Rules);
			case 3:
				return this.Create_BamlType_Adorner(isBamlType, useV3Rules);
			case 4:
				return this.Create_BamlType_AdornerDecorator(isBamlType, useV3Rules);
			case 5:
				return this.Create_BamlType_AdornerLayer(isBamlType, useV3Rules);
			case 6:
				return this.Create_BamlType_AffineTransform3D(isBamlType, useV3Rules);
			case 7:
				return this.Create_BamlType_AmbientLight(isBamlType, useV3Rules);
			case 8:
				return this.Create_BamlType_AnchoredBlock(isBamlType, useV3Rules);
			case 9:
				return this.Create_BamlType_Animatable(isBamlType, useV3Rules);
			case 10:
				return this.Create_BamlType_AnimationClock(isBamlType, useV3Rules);
			case 11:
				return this.Create_BamlType_AnimationTimeline(isBamlType, useV3Rules);
			case 12:
				return this.Create_BamlType_Application(isBamlType, useV3Rules);
			case 13:
				return this.Create_BamlType_ArcSegment(isBamlType, useV3Rules);
			case 14:
				return this.Create_BamlType_ArrayExtension(isBamlType, useV3Rules);
			case 15:
				return this.Create_BamlType_AxisAngleRotation3D(isBamlType, useV3Rules);
			case 16:
				return this.Create_BamlType_BaseIListConverter(isBamlType, useV3Rules);
			case 17:
				return this.Create_BamlType_BeginStoryboard(isBamlType, useV3Rules);
			case 18:
				return this.Create_BamlType_BevelBitmapEffect(isBamlType, useV3Rules);
			case 19:
				return this.Create_BamlType_BezierSegment(isBamlType, useV3Rules);
			case 20:
				return this.Create_BamlType_Binding(isBamlType, useV3Rules);
			case 21:
				return this.Create_BamlType_BindingBase(isBamlType, useV3Rules);
			case 22:
				return this.Create_BamlType_BindingExpression(isBamlType, useV3Rules);
			case 23:
				return this.Create_BamlType_BindingExpressionBase(isBamlType, useV3Rules);
			case 24:
				return this.Create_BamlType_BindingListCollectionView(isBamlType, useV3Rules);
			case 25:
				return this.Create_BamlType_BitmapDecoder(isBamlType, useV3Rules);
			case 26:
				return this.Create_BamlType_BitmapEffect(isBamlType, useV3Rules);
			case 27:
				return this.Create_BamlType_BitmapEffectCollection(isBamlType, useV3Rules);
			case 28:
				return this.Create_BamlType_BitmapEffectGroup(isBamlType, useV3Rules);
			case 29:
				return this.Create_BamlType_BitmapEffectInput(isBamlType, useV3Rules);
			case 30:
				return this.Create_BamlType_BitmapEncoder(isBamlType, useV3Rules);
			case 31:
				return this.Create_BamlType_BitmapFrame(isBamlType, useV3Rules);
			case 32:
				return this.Create_BamlType_BitmapImage(isBamlType, useV3Rules);
			case 33:
				return this.Create_BamlType_BitmapMetadata(isBamlType, useV3Rules);
			case 34:
				return this.Create_BamlType_BitmapPalette(isBamlType, useV3Rules);
			case 35:
				return this.Create_BamlType_BitmapSource(isBamlType, useV3Rules);
			case 36:
				return this.Create_BamlType_Block(isBamlType, useV3Rules);
			case 37:
				return this.Create_BamlType_BlockUIContainer(isBamlType, useV3Rules);
			case 38:
				return this.Create_BamlType_BlurBitmapEffect(isBamlType, useV3Rules);
			case 39:
				return this.Create_BamlType_BmpBitmapDecoder(isBamlType, useV3Rules);
			case 40:
				return this.Create_BamlType_BmpBitmapEncoder(isBamlType, useV3Rules);
			case 41:
				return this.Create_BamlType_Bold(isBamlType, useV3Rules);
			case 42:
				return this.Create_BamlType_BoolIListConverter(isBamlType, useV3Rules);
			case 43:
				return this.Create_BamlType_Boolean(isBamlType, useV3Rules);
			case 44:
				return this.Create_BamlType_BooleanAnimationBase(isBamlType, useV3Rules);
			case 45:
				return this.Create_BamlType_BooleanAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 46:
				return this.Create_BamlType_BooleanConverter(isBamlType, useV3Rules);
			case 47:
				return this.Create_BamlType_BooleanKeyFrame(isBamlType, useV3Rules);
			case 48:
				return this.Create_BamlType_BooleanKeyFrameCollection(isBamlType, useV3Rules);
			case 49:
				return this.Create_BamlType_BooleanToVisibilityConverter(isBamlType, useV3Rules);
			case 50:
				return this.Create_BamlType_Border(isBamlType, useV3Rules);
			case 51:
				return this.Create_BamlType_BorderGapMaskConverter(isBamlType, useV3Rules);
			case 52:
				return this.Create_BamlType_Brush(isBamlType, useV3Rules);
			case 53:
				return this.Create_BamlType_BrushConverter(isBamlType, useV3Rules);
			case 54:
				return this.Create_BamlType_BulletDecorator(isBamlType, useV3Rules);
			case 55:
				return this.Create_BamlType_Button(isBamlType, useV3Rules);
			case 56:
				return this.Create_BamlType_ButtonBase(isBamlType, useV3Rules);
			case 57:
				return this.Create_BamlType_Byte(isBamlType, useV3Rules);
			case 58:
				return this.Create_BamlType_ByteAnimation(isBamlType, useV3Rules);
			case 59:
				return this.Create_BamlType_ByteAnimationBase(isBamlType, useV3Rules);
			case 60:
				return this.Create_BamlType_ByteAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 61:
				return this.Create_BamlType_ByteConverter(isBamlType, useV3Rules);
			case 62:
				return this.Create_BamlType_ByteKeyFrame(isBamlType, useV3Rules);
			case 63:
				return this.Create_BamlType_ByteKeyFrameCollection(isBamlType, useV3Rules);
			case 64:
				return this.Create_BamlType_CachedBitmap(isBamlType, useV3Rules);
			case 65:
				return this.Create_BamlType_Camera(isBamlType, useV3Rules);
			case 66:
				return this.Create_BamlType_Canvas(isBamlType, useV3Rules);
			case 67:
				return this.Create_BamlType_Char(isBamlType, useV3Rules);
			case 68:
				return this.Create_BamlType_CharAnimationBase(isBamlType, useV3Rules);
			case 69:
				return this.Create_BamlType_CharAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 70:
				return this.Create_BamlType_CharConverter(isBamlType, useV3Rules);
			case 71:
				return this.Create_BamlType_CharIListConverter(isBamlType, useV3Rules);
			case 72:
				return this.Create_BamlType_CharKeyFrame(isBamlType, useV3Rules);
			case 73:
				return this.Create_BamlType_CharKeyFrameCollection(isBamlType, useV3Rules);
			case 74:
				return this.Create_BamlType_CheckBox(isBamlType, useV3Rules);
			case 75:
				return this.Create_BamlType_Clock(isBamlType, useV3Rules);
			case 76:
				return this.Create_BamlType_ClockController(isBamlType, useV3Rules);
			case 77:
				return this.Create_BamlType_ClockGroup(isBamlType, useV3Rules);
			case 78:
				return this.Create_BamlType_CollectionContainer(isBamlType, useV3Rules);
			case 79:
				return this.Create_BamlType_CollectionView(isBamlType, useV3Rules);
			case 80:
				return this.Create_BamlType_CollectionViewSource(isBamlType, useV3Rules);
			case 81:
				return this.Create_BamlType_Color(isBamlType, useV3Rules);
			case 82:
				return this.Create_BamlType_ColorAnimation(isBamlType, useV3Rules);
			case 83:
				return this.Create_BamlType_ColorAnimationBase(isBamlType, useV3Rules);
			case 84:
				return this.Create_BamlType_ColorAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 85:
				return this.Create_BamlType_ColorConvertedBitmap(isBamlType, useV3Rules);
			case 86:
				return this.Create_BamlType_ColorConvertedBitmapExtension(isBamlType, useV3Rules);
			case 87:
				return this.Create_BamlType_ColorConverter(isBamlType, useV3Rules);
			case 88:
				return this.Create_BamlType_ColorKeyFrame(isBamlType, useV3Rules);
			case 89:
				return this.Create_BamlType_ColorKeyFrameCollection(isBamlType, useV3Rules);
			case 90:
				return this.Create_BamlType_ColumnDefinition(isBamlType, useV3Rules);
			case 91:
				return this.Create_BamlType_CombinedGeometry(isBamlType, useV3Rules);
			case 92:
				return this.Create_BamlType_ComboBox(isBamlType, useV3Rules);
			case 93:
				return this.Create_BamlType_ComboBoxItem(isBamlType, useV3Rules);
			case 94:
				return this.Create_BamlType_CommandConverter(isBamlType, useV3Rules);
			case 95:
				return this.Create_BamlType_ComponentResourceKey(isBamlType, useV3Rules);
			case 96:
				return this.Create_BamlType_ComponentResourceKeyConverter(isBamlType, useV3Rules);
			case 97:
				return this.Create_BamlType_CompositionTarget(isBamlType, useV3Rules);
			case 98:
				return this.Create_BamlType_Condition(isBamlType, useV3Rules);
			case 99:
				return this.Create_BamlType_ContainerVisual(isBamlType, useV3Rules);
			case 100:
				return this.Create_BamlType_ContentControl(isBamlType, useV3Rules);
			case 101:
				return this.Create_BamlType_ContentElement(isBamlType, useV3Rules);
			case 102:
				return this.Create_BamlType_ContentPresenter(isBamlType, useV3Rules);
			case 103:
				return this.Create_BamlType_ContentPropertyAttribute(isBamlType, useV3Rules);
			case 104:
				return this.Create_BamlType_ContentWrapperAttribute(isBamlType, useV3Rules);
			case 105:
				return this.Create_BamlType_ContextMenu(isBamlType, useV3Rules);
			case 106:
				return this.Create_BamlType_ContextMenuService(isBamlType, useV3Rules);
			case 107:
				return this.Create_BamlType_Control(isBamlType, useV3Rules);
			case 108:
				return this.Create_BamlType_ControlTemplate(isBamlType, useV3Rules);
			case 109:
				return this.Create_BamlType_ControllableStoryboardAction(isBamlType, useV3Rules);
			case 110:
				return this.Create_BamlType_CornerRadius(isBamlType, useV3Rules);
			case 111:
				return this.Create_BamlType_CornerRadiusConverter(isBamlType, useV3Rules);
			case 112:
				return this.Create_BamlType_CroppedBitmap(isBamlType, useV3Rules);
			case 113:
				return this.Create_BamlType_CultureInfo(isBamlType, useV3Rules);
			case 114:
				return this.Create_BamlType_CultureInfoConverter(isBamlType, useV3Rules);
			case 115:
				return this.Create_BamlType_CultureInfoIetfLanguageTagConverter(isBamlType, useV3Rules);
			case 116:
				return this.Create_BamlType_Cursor(isBamlType, useV3Rules);
			case 117:
				return this.Create_BamlType_CursorConverter(isBamlType, useV3Rules);
			case 118:
				return this.Create_BamlType_DashStyle(isBamlType, useV3Rules);
			case 119:
				return this.Create_BamlType_DataChangedEventManager(isBamlType, useV3Rules);
			case 120:
				return this.Create_BamlType_DataTemplate(isBamlType, useV3Rules);
			case 121:
				return this.Create_BamlType_DataTemplateKey(isBamlType, useV3Rules);
			case 122:
				return this.Create_BamlType_DataTrigger(isBamlType, useV3Rules);
			case 123:
				return this.Create_BamlType_DateTime(isBamlType, useV3Rules);
			case 124:
				return this.Create_BamlType_DateTimeConverter(isBamlType, useV3Rules);
			case 125:
				return this.Create_BamlType_DateTimeConverter2(isBamlType, useV3Rules);
			case 126:
				return this.Create_BamlType_Decimal(isBamlType, useV3Rules);
			case 127:
				return this.Create_BamlType_DecimalAnimation(isBamlType, useV3Rules);
			case 128:
				return this.Create_BamlType_DecimalAnimationBase(isBamlType, useV3Rules);
			case 129:
				return this.Create_BamlType_DecimalAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 130:
				return this.Create_BamlType_DecimalConverter(isBamlType, useV3Rules);
			case 131:
				return this.Create_BamlType_DecimalKeyFrame(isBamlType, useV3Rules);
			case 132:
				return this.Create_BamlType_DecimalKeyFrameCollection(isBamlType, useV3Rules);
			case 133:
				return this.Create_BamlType_Decorator(isBamlType, useV3Rules);
			case 134:
				return this.Create_BamlType_DefinitionBase(isBamlType, useV3Rules);
			case 135:
				return this.Create_BamlType_DependencyObject(isBamlType, useV3Rules);
			case 136:
				return this.Create_BamlType_DependencyProperty(isBamlType, useV3Rules);
			case 137:
				return this.Create_BamlType_DependencyPropertyConverter(isBamlType, useV3Rules);
			case 138:
				return this.Create_BamlType_DialogResultConverter(isBamlType, useV3Rules);
			case 139:
				return this.Create_BamlType_DiffuseMaterial(isBamlType, useV3Rules);
			case 140:
				return this.Create_BamlType_DirectionalLight(isBamlType, useV3Rules);
			case 141:
				return this.Create_BamlType_DiscreteBooleanKeyFrame(isBamlType, useV3Rules);
			case 142:
				return this.Create_BamlType_DiscreteByteKeyFrame(isBamlType, useV3Rules);
			case 143:
				return this.Create_BamlType_DiscreteCharKeyFrame(isBamlType, useV3Rules);
			case 144:
				return this.Create_BamlType_DiscreteColorKeyFrame(isBamlType, useV3Rules);
			case 145:
				return this.Create_BamlType_DiscreteDecimalKeyFrame(isBamlType, useV3Rules);
			case 146:
				return this.Create_BamlType_DiscreteDoubleKeyFrame(isBamlType, useV3Rules);
			case 147:
				return this.Create_BamlType_DiscreteInt16KeyFrame(isBamlType, useV3Rules);
			case 148:
				return this.Create_BamlType_DiscreteInt32KeyFrame(isBamlType, useV3Rules);
			case 149:
				return this.Create_BamlType_DiscreteInt64KeyFrame(isBamlType, useV3Rules);
			case 150:
				return this.Create_BamlType_DiscreteMatrixKeyFrame(isBamlType, useV3Rules);
			case 151:
				return this.Create_BamlType_DiscreteObjectKeyFrame(isBamlType, useV3Rules);
			case 152:
				return this.Create_BamlType_DiscretePoint3DKeyFrame(isBamlType, useV3Rules);
			case 153:
				return this.Create_BamlType_DiscretePointKeyFrame(isBamlType, useV3Rules);
			case 154:
				return this.Create_BamlType_DiscreteQuaternionKeyFrame(isBamlType, useV3Rules);
			case 155:
				return this.Create_BamlType_DiscreteRectKeyFrame(isBamlType, useV3Rules);
			case 156:
				return this.Create_BamlType_DiscreteRotation3DKeyFrame(isBamlType, useV3Rules);
			case 157:
				return this.Create_BamlType_DiscreteSingleKeyFrame(isBamlType, useV3Rules);
			case 158:
				return this.Create_BamlType_DiscreteSizeKeyFrame(isBamlType, useV3Rules);
			case 159:
				return this.Create_BamlType_DiscreteStringKeyFrame(isBamlType, useV3Rules);
			case 160:
				return this.Create_BamlType_DiscreteThicknessKeyFrame(isBamlType, useV3Rules);
			case 161:
				return this.Create_BamlType_DiscreteVector3DKeyFrame(isBamlType, useV3Rules);
			case 162:
				return this.Create_BamlType_DiscreteVectorKeyFrame(isBamlType, useV3Rules);
			case 163:
				return this.Create_BamlType_DockPanel(isBamlType, useV3Rules);
			case 164:
				return this.Create_BamlType_DocumentPageView(isBamlType, useV3Rules);
			case 165:
				return this.Create_BamlType_DocumentReference(isBamlType, useV3Rules);
			case 166:
				return this.Create_BamlType_DocumentViewer(isBamlType, useV3Rules);
			case 167:
				return this.Create_BamlType_DocumentViewerBase(isBamlType, useV3Rules);
			case 168:
				return this.Create_BamlType_Double(isBamlType, useV3Rules);
			case 169:
				return this.Create_BamlType_DoubleAnimation(isBamlType, useV3Rules);
			case 170:
				return this.Create_BamlType_DoubleAnimationBase(isBamlType, useV3Rules);
			case 171:
				return this.Create_BamlType_DoubleAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 172:
				return this.Create_BamlType_DoubleAnimationUsingPath(isBamlType, useV3Rules);
			case 173:
				return this.Create_BamlType_DoubleCollection(isBamlType, useV3Rules);
			case 174:
				return this.Create_BamlType_DoubleCollectionConverter(isBamlType, useV3Rules);
			case 175:
				return this.Create_BamlType_DoubleConverter(isBamlType, useV3Rules);
			case 176:
				return this.Create_BamlType_DoubleIListConverter(isBamlType, useV3Rules);
			case 177:
				return this.Create_BamlType_DoubleKeyFrame(isBamlType, useV3Rules);
			case 178:
				return this.Create_BamlType_DoubleKeyFrameCollection(isBamlType, useV3Rules);
			case 179:
				return this.Create_BamlType_Drawing(isBamlType, useV3Rules);
			case 180:
				return this.Create_BamlType_DrawingBrush(isBamlType, useV3Rules);
			case 181:
				return this.Create_BamlType_DrawingCollection(isBamlType, useV3Rules);
			case 182:
				return this.Create_BamlType_DrawingContext(isBamlType, useV3Rules);
			case 183:
				return this.Create_BamlType_DrawingGroup(isBamlType, useV3Rules);
			case 184:
				return this.Create_BamlType_DrawingImage(isBamlType, useV3Rules);
			case 185:
				return this.Create_BamlType_DrawingVisual(isBamlType, useV3Rules);
			case 186:
				return this.Create_BamlType_DropShadowBitmapEffect(isBamlType, useV3Rules);
			case 187:
				return this.Create_BamlType_Duration(isBamlType, useV3Rules);
			case 188:
				return this.Create_BamlType_DurationConverter(isBamlType, useV3Rules);
			case 189:
				return this.Create_BamlType_DynamicResourceExtension(isBamlType, useV3Rules);
			case 190:
				return this.Create_BamlType_DynamicResourceExtensionConverter(isBamlType, useV3Rules);
			case 191:
				return this.Create_BamlType_Ellipse(isBamlType, useV3Rules);
			case 192:
				return this.Create_BamlType_EllipseGeometry(isBamlType, useV3Rules);
			case 193:
				return this.Create_BamlType_EmbossBitmapEffect(isBamlType, useV3Rules);
			case 194:
				return this.Create_BamlType_EmissiveMaterial(isBamlType, useV3Rules);
			case 195:
				return this.Create_BamlType_EnumConverter(isBamlType, useV3Rules);
			case 196:
				return this.Create_BamlType_EventManager(isBamlType, useV3Rules);
			case 197:
				return this.Create_BamlType_EventSetter(isBamlType, useV3Rules);
			case 198:
				return this.Create_BamlType_EventTrigger(isBamlType, useV3Rules);
			case 199:
				return this.Create_BamlType_Expander(isBamlType, useV3Rules);
			case 200:
				return this.Create_BamlType_Expression(isBamlType, useV3Rules);
			case 201:
				return this.Create_BamlType_ExpressionConverter(isBamlType, useV3Rules);
			case 202:
				return this.Create_BamlType_Figure(isBamlType, useV3Rules);
			case 203:
				return this.Create_BamlType_FigureLength(isBamlType, useV3Rules);
			case 204:
				return this.Create_BamlType_FigureLengthConverter(isBamlType, useV3Rules);
			case 205:
				return this.Create_BamlType_FixedDocument(isBamlType, useV3Rules);
			case 206:
				return this.Create_BamlType_FixedDocumentSequence(isBamlType, useV3Rules);
			case 207:
				return this.Create_BamlType_FixedPage(isBamlType, useV3Rules);
			case 208:
				return this.Create_BamlType_Floater(isBamlType, useV3Rules);
			case 209:
				return this.Create_BamlType_FlowDocument(isBamlType, useV3Rules);
			case 210:
				return this.Create_BamlType_FlowDocumentPageViewer(isBamlType, useV3Rules);
			case 211:
				return this.Create_BamlType_FlowDocumentReader(isBamlType, useV3Rules);
			case 212:
				return this.Create_BamlType_FlowDocumentScrollViewer(isBamlType, useV3Rules);
			case 213:
				return this.Create_BamlType_FocusManager(isBamlType, useV3Rules);
			case 214:
				return this.Create_BamlType_FontFamily(isBamlType, useV3Rules);
			case 215:
				return this.Create_BamlType_FontFamilyConverter(isBamlType, useV3Rules);
			case 216:
				return this.Create_BamlType_FontSizeConverter(isBamlType, useV3Rules);
			case 217:
				return this.Create_BamlType_FontStretch(isBamlType, useV3Rules);
			case 218:
				return this.Create_BamlType_FontStretchConverter(isBamlType, useV3Rules);
			case 219:
				return this.Create_BamlType_FontStyle(isBamlType, useV3Rules);
			case 220:
				return this.Create_BamlType_FontStyleConverter(isBamlType, useV3Rules);
			case 221:
				return this.Create_BamlType_FontWeight(isBamlType, useV3Rules);
			case 222:
				return this.Create_BamlType_FontWeightConverter(isBamlType, useV3Rules);
			case 223:
				return this.Create_BamlType_FormatConvertedBitmap(isBamlType, useV3Rules);
			case 224:
				return this.Create_BamlType_Frame(isBamlType, useV3Rules);
			case 225:
				return this.Create_BamlType_FrameworkContentElement(isBamlType, useV3Rules);
			case 226:
				return this.Create_BamlType_FrameworkElement(isBamlType, useV3Rules);
			case 227:
				return this.Create_BamlType_FrameworkElementFactory(isBamlType, useV3Rules);
			case 228:
				return this.Create_BamlType_FrameworkPropertyMetadata(isBamlType, useV3Rules);
			case 229:
				return this.Create_BamlType_FrameworkPropertyMetadataOptions(isBamlType, useV3Rules);
			case 230:
				return this.Create_BamlType_FrameworkRichTextComposition(isBamlType, useV3Rules);
			case 231:
				return this.Create_BamlType_FrameworkTemplate(isBamlType, useV3Rules);
			case 232:
				return this.Create_BamlType_FrameworkTextComposition(isBamlType, useV3Rules);
			case 233:
				return this.Create_BamlType_Freezable(isBamlType, useV3Rules);
			case 234:
				return this.Create_BamlType_GeneralTransform(isBamlType, useV3Rules);
			case 235:
				return this.Create_BamlType_GeneralTransformCollection(isBamlType, useV3Rules);
			case 236:
				return this.Create_BamlType_GeneralTransformGroup(isBamlType, useV3Rules);
			case 237:
				return this.Create_BamlType_Geometry(isBamlType, useV3Rules);
			case 238:
				return this.Create_BamlType_Geometry3D(isBamlType, useV3Rules);
			case 239:
				return this.Create_BamlType_GeometryCollection(isBamlType, useV3Rules);
			case 240:
				return this.Create_BamlType_GeometryConverter(isBamlType, useV3Rules);
			case 241:
				return this.Create_BamlType_GeometryDrawing(isBamlType, useV3Rules);
			case 242:
				return this.Create_BamlType_GeometryGroup(isBamlType, useV3Rules);
			case 243:
				return this.Create_BamlType_GeometryModel3D(isBamlType, useV3Rules);
			case 244:
				return this.Create_BamlType_GestureRecognizer(isBamlType, useV3Rules);
			case 245:
				return this.Create_BamlType_GifBitmapDecoder(isBamlType, useV3Rules);
			case 246:
				return this.Create_BamlType_GifBitmapEncoder(isBamlType, useV3Rules);
			case 247:
				return this.Create_BamlType_GlyphRun(isBamlType, useV3Rules);
			case 248:
				return this.Create_BamlType_GlyphRunDrawing(isBamlType, useV3Rules);
			case 249:
				return this.Create_BamlType_GlyphTypeface(isBamlType, useV3Rules);
			case 250:
				return this.Create_BamlType_Glyphs(isBamlType, useV3Rules);
			case 251:
				return this.Create_BamlType_GradientBrush(isBamlType, useV3Rules);
			case 252:
				return this.Create_BamlType_GradientStop(isBamlType, useV3Rules);
			case 253:
				return this.Create_BamlType_GradientStopCollection(isBamlType, useV3Rules);
			case 254:
				return this.Create_BamlType_Grid(isBamlType, useV3Rules);
			case 255:
				return this.Create_BamlType_GridLength(isBamlType, useV3Rules);
			case 256:
				return this.Create_BamlType_GridLengthConverter(isBamlType, useV3Rules);
			case 257:
				return this.Create_BamlType_GridSplitter(isBamlType, useV3Rules);
			case 258:
				return this.Create_BamlType_GridView(isBamlType, useV3Rules);
			case 259:
				return this.Create_BamlType_GridViewColumn(isBamlType, useV3Rules);
			case 260:
				return this.Create_BamlType_GridViewColumnHeader(isBamlType, useV3Rules);
			case 261:
				return this.Create_BamlType_GridViewHeaderRowPresenter(isBamlType, useV3Rules);
			case 262:
				return this.Create_BamlType_GridViewRowPresenter(isBamlType, useV3Rules);
			case 263:
				return this.Create_BamlType_GridViewRowPresenterBase(isBamlType, useV3Rules);
			case 264:
				return this.Create_BamlType_GroupBox(isBamlType, useV3Rules);
			case 265:
				return this.Create_BamlType_GroupItem(isBamlType, useV3Rules);
			case 266:
				return this.Create_BamlType_Guid(isBamlType, useV3Rules);
			case 267:
				return this.Create_BamlType_GuidConverter(isBamlType, useV3Rules);
			case 268:
				return this.Create_BamlType_GuidelineSet(isBamlType, useV3Rules);
			case 269:
				return this.Create_BamlType_HeaderedContentControl(isBamlType, useV3Rules);
			case 270:
				return this.Create_BamlType_HeaderedItemsControl(isBamlType, useV3Rules);
			case 271:
				return this.Create_BamlType_HierarchicalDataTemplate(isBamlType, useV3Rules);
			case 272:
				return this.Create_BamlType_HostVisual(isBamlType, useV3Rules);
			case 273:
				return this.Create_BamlType_Hyperlink(isBamlType, useV3Rules);
			case 274:
				return this.Create_BamlType_IAddChild(isBamlType, useV3Rules);
			case 275:
				return this.Create_BamlType_IAddChildInternal(isBamlType, useV3Rules);
			case 276:
				return this.Create_BamlType_ICommand(isBamlType, useV3Rules);
			case 277:
				return this.Create_BamlType_IComponentConnector(isBamlType, useV3Rules);
			case 278:
				return this.Create_BamlType_INameScope(isBamlType, useV3Rules);
			case 279:
				return this.Create_BamlType_IStyleConnector(isBamlType, useV3Rules);
			case 280:
				return this.Create_BamlType_IconBitmapDecoder(isBamlType, useV3Rules);
			case 281:
				return this.Create_BamlType_Image(isBamlType, useV3Rules);
			case 282:
				return this.Create_BamlType_ImageBrush(isBamlType, useV3Rules);
			case 283:
				return this.Create_BamlType_ImageDrawing(isBamlType, useV3Rules);
			case 284:
				return this.Create_BamlType_ImageMetadata(isBamlType, useV3Rules);
			case 285:
				return this.Create_BamlType_ImageSource(isBamlType, useV3Rules);
			case 286:
				return this.Create_BamlType_ImageSourceConverter(isBamlType, useV3Rules);
			case 287:
				return this.Create_BamlType_InPlaceBitmapMetadataWriter(isBamlType, useV3Rules);
			case 288:
				return this.Create_BamlType_InkCanvas(isBamlType, useV3Rules);
			case 289:
				return this.Create_BamlType_InkPresenter(isBamlType, useV3Rules);
			case 290:
				return this.Create_BamlType_Inline(isBamlType, useV3Rules);
			case 291:
				return this.Create_BamlType_InlineCollection(isBamlType, useV3Rules);
			case 292:
				return this.Create_BamlType_InlineUIContainer(isBamlType, useV3Rules);
			case 293:
				return this.Create_BamlType_InputBinding(isBamlType, useV3Rules);
			case 294:
				return this.Create_BamlType_InputDevice(isBamlType, useV3Rules);
			case 295:
				return this.Create_BamlType_InputLanguageManager(isBamlType, useV3Rules);
			case 296:
				return this.Create_BamlType_InputManager(isBamlType, useV3Rules);
			case 297:
				return this.Create_BamlType_InputMethod(isBamlType, useV3Rules);
			case 298:
				return this.Create_BamlType_InputScope(isBamlType, useV3Rules);
			case 299:
				return this.Create_BamlType_InputScopeConverter(isBamlType, useV3Rules);
			case 300:
				return this.Create_BamlType_InputScopeName(isBamlType, useV3Rules);
			case 301:
				return this.Create_BamlType_InputScopeNameConverter(isBamlType, useV3Rules);
			case 302:
				return this.Create_BamlType_Int16(isBamlType, useV3Rules);
			case 303:
				return this.Create_BamlType_Int16Animation(isBamlType, useV3Rules);
			case 304:
				return this.Create_BamlType_Int16AnimationBase(isBamlType, useV3Rules);
			case 305:
				return this.Create_BamlType_Int16AnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 306:
				return this.Create_BamlType_Int16Converter(isBamlType, useV3Rules);
			case 307:
				return this.Create_BamlType_Int16KeyFrame(isBamlType, useV3Rules);
			case 308:
				return this.Create_BamlType_Int16KeyFrameCollection(isBamlType, useV3Rules);
			case 309:
				return this.Create_BamlType_Int32(isBamlType, useV3Rules);
			case 310:
				return this.Create_BamlType_Int32Animation(isBamlType, useV3Rules);
			case 311:
				return this.Create_BamlType_Int32AnimationBase(isBamlType, useV3Rules);
			case 312:
				return this.Create_BamlType_Int32AnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 313:
				return this.Create_BamlType_Int32Collection(isBamlType, useV3Rules);
			case 314:
				return this.Create_BamlType_Int32CollectionConverter(isBamlType, useV3Rules);
			case 315:
				return this.Create_BamlType_Int32Converter(isBamlType, useV3Rules);
			case 316:
				return this.Create_BamlType_Int32KeyFrame(isBamlType, useV3Rules);
			case 317:
				return this.Create_BamlType_Int32KeyFrameCollection(isBamlType, useV3Rules);
			case 318:
				return this.Create_BamlType_Int32Rect(isBamlType, useV3Rules);
			case 319:
				return this.Create_BamlType_Int32RectConverter(isBamlType, useV3Rules);
			case 320:
				return this.Create_BamlType_Int64(isBamlType, useV3Rules);
			case 321:
				return this.Create_BamlType_Int64Animation(isBamlType, useV3Rules);
			case 322:
				return this.Create_BamlType_Int64AnimationBase(isBamlType, useV3Rules);
			case 323:
				return this.Create_BamlType_Int64AnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 324:
				return this.Create_BamlType_Int64Converter(isBamlType, useV3Rules);
			case 325:
				return this.Create_BamlType_Int64KeyFrame(isBamlType, useV3Rules);
			case 326:
				return this.Create_BamlType_Int64KeyFrameCollection(isBamlType, useV3Rules);
			case 327:
				return this.Create_BamlType_Italic(isBamlType, useV3Rules);
			case 328:
				return this.Create_BamlType_ItemCollection(isBamlType, useV3Rules);
			case 329:
				return this.Create_BamlType_ItemsControl(isBamlType, useV3Rules);
			case 330:
				return this.Create_BamlType_ItemsPanelTemplate(isBamlType, useV3Rules);
			case 331:
				return this.Create_BamlType_ItemsPresenter(isBamlType, useV3Rules);
			case 332:
				return this.Create_BamlType_JournalEntry(isBamlType, useV3Rules);
			case 333:
				return this.Create_BamlType_JournalEntryListConverter(isBamlType, useV3Rules);
			case 334:
				return this.Create_BamlType_JournalEntryUnifiedViewConverter(isBamlType, useV3Rules);
			case 335:
				return this.Create_BamlType_JpegBitmapDecoder(isBamlType, useV3Rules);
			case 336:
				return this.Create_BamlType_JpegBitmapEncoder(isBamlType, useV3Rules);
			case 337:
				return this.Create_BamlType_KeyBinding(isBamlType, useV3Rules);
			case 338:
				return this.Create_BamlType_KeyConverter(isBamlType, useV3Rules);
			case 339:
				return this.Create_BamlType_KeyGesture(isBamlType, useV3Rules);
			case 340:
				return this.Create_BamlType_KeyGestureConverter(isBamlType, useV3Rules);
			case 341:
				return this.Create_BamlType_KeySpline(isBamlType, useV3Rules);
			case 342:
				return this.Create_BamlType_KeySplineConverter(isBamlType, useV3Rules);
			case 343:
				return this.Create_BamlType_KeyTime(isBamlType, useV3Rules);
			case 344:
				return this.Create_BamlType_KeyTimeConverter(isBamlType, useV3Rules);
			case 345:
				return this.Create_BamlType_KeyboardDevice(isBamlType, useV3Rules);
			case 346:
				return this.Create_BamlType_Label(isBamlType, useV3Rules);
			case 347:
				return this.Create_BamlType_LateBoundBitmapDecoder(isBamlType, useV3Rules);
			case 348:
				return this.Create_BamlType_LengthConverter(isBamlType, useV3Rules);
			case 349:
				return this.Create_BamlType_Light(isBamlType, useV3Rules);
			case 350:
				return this.Create_BamlType_Line(isBamlType, useV3Rules);
			case 351:
				return this.Create_BamlType_LineBreak(isBamlType, useV3Rules);
			case 352:
				return this.Create_BamlType_LineGeometry(isBamlType, useV3Rules);
			case 353:
				return this.Create_BamlType_LineSegment(isBamlType, useV3Rules);
			case 354:
				return this.Create_BamlType_LinearByteKeyFrame(isBamlType, useV3Rules);
			case 355:
				return this.Create_BamlType_LinearColorKeyFrame(isBamlType, useV3Rules);
			case 356:
				return this.Create_BamlType_LinearDecimalKeyFrame(isBamlType, useV3Rules);
			case 357:
				return this.Create_BamlType_LinearDoubleKeyFrame(isBamlType, useV3Rules);
			case 358:
				return this.Create_BamlType_LinearGradientBrush(isBamlType, useV3Rules);
			case 359:
				return this.Create_BamlType_LinearInt16KeyFrame(isBamlType, useV3Rules);
			case 360:
				return this.Create_BamlType_LinearInt32KeyFrame(isBamlType, useV3Rules);
			case 361:
				return this.Create_BamlType_LinearInt64KeyFrame(isBamlType, useV3Rules);
			case 362:
				return this.Create_BamlType_LinearPoint3DKeyFrame(isBamlType, useV3Rules);
			case 363:
				return this.Create_BamlType_LinearPointKeyFrame(isBamlType, useV3Rules);
			case 364:
				return this.Create_BamlType_LinearQuaternionKeyFrame(isBamlType, useV3Rules);
			case 365:
				return this.Create_BamlType_LinearRectKeyFrame(isBamlType, useV3Rules);
			case 366:
				return this.Create_BamlType_LinearRotation3DKeyFrame(isBamlType, useV3Rules);
			case 367:
				return this.Create_BamlType_LinearSingleKeyFrame(isBamlType, useV3Rules);
			case 368:
				return this.Create_BamlType_LinearSizeKeyFrame(isBamlType, useV3Rules);
			case 369:
				return this.Create_BamlType_LinearThicknessKeyFrame(isBamlType, useV3Rules);
			case 370:
				return this.Create_BamlType_LinearVector3DKeyFrame(isBamlType, useV3Rules);
			case 371:
				return this.Create_BamlType_LinearVectorKeyFrame(isBamlType, useV3Rules);
			case 372:
				return this.Create_BamlType_List(isBamlType, useV3Rules);
			case 373:
				return this.Create_BamlType_ListBox(isBamlType, useV3Rules);
			case 374:
				return this.Create_BamlType_ListBoxItem(isBamlType, useV3Rules);
			case 375:
				return this.Create_BamlType_ListCollectionView(isBamlType, useV3Rules);
			case 376:
				return this.Create_BamlType_ListItem(isBamlType, useV3Rules);
			case 377:
				return this.Create_BamlType_ListView(isBamlType, useV3Rules);
			case 378:
				return this.Create_BamlType_ListViewItem(isBamlType, useV3Rules);
			case 379:
				return this.Create_BamlType_Localization(isBamlType, useV3Rules);
			case 380:
				return this.Create_BamlType_LostFocusEventManager(isBamlType, useV3Rules);
			case 381:
				return this.Create_BamlType_MarkupExtension(isBamlType, useV3Rules);
			case 382:
				return this.Create_BamlType_Material(isBamlType, useV3Rules);
			case 383:
				return this.Create_BamlType_MaterialCollection(isBamlType, useV3Rules);
			case 384:
				return this.Create_BamlType_MaterialGroup(isBamlType, useV3Rules);
			case 385:
				return this.Create_BamlType_Matrix(isBamlType, useV3Rules);
			case 386:
				return this.Create_BamlType_Matrix3D(isBamlType, useV3Rules);
			case 387:
				return this.Create_BamlType_Matrix3DConverter(isBamlType, useV3Rules);
			case 388:
				return this.Create_BamlType_MatrixAnimationBase(isBamlType, useV3Rules);
			case 389:
				return this.Create_BamlType_MatrixAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 390:
				return this.Create_BamlType_MatrixAnimationUsingPath(isBamlType, useV3Rules);
			case 391:
				return this.Create_BamlType_MatrixCamera(isBamlType, useV3Rules);
			case 392:
				return this.Create_BamlType_MatrixConverter(isBamlType, useV3Rules);
			case 393:
				return this.Create_BamlType_MatrixKeyFrame(isBamlType, useV3Rules);
			case 394:
				return this.Create_BamlType_MatrixKeyFrameCollection(isBamlType, useV3Rules);
			case 395:
				return this.Create_BamlType_MatrixTransform(isBamlType, useV3Rules);
			case 396:
				return this.Create_BamlType_MatrixTransform3D(isBamlType, useV3Rules);
			case 397:
				return this.Create_BamlType_MediaClock(isBamlType, useV3Rules);
			case 398:
				return this.Create_BamlType_MediaElement(isBamlType, useV3Rules);
			case 399:
				return this.Create_BamlType_MediaPlayer(isBamlType, useV3Rules);
			case 400:
				return this.Create_BamlType_MediaTimeline(isBamlType, useV3Rules);
			case 401:
				return this.Create_BamlType_Menu(isBamlType, useV3Rules);
			case 402:
				return this.Create_BamlType_MenuBase(isBamlType, useV3Rules);
			case 403:
				return this.Create_BamlType_MenuItem(isBamlType, useV3Rules);
			case 404:
				return this.Create_BamlType_MenuScrollingVisibilityConverter(isBamlType, useV3Rules);
			case 405:
				return this.Create_BamlType_MeshGeometry3D(isBamlType, useV3Rules);
			case 406:
				return this.Create_BamlType_Model3D(isBamlType, useV3Rules);
			case 407:
				return this.Create_BamlType_Model3DCollection(isBamlType, useV3Rules);
			case 408:
				return this.Create_BamlType_Model3DGroup(isBamlType, useV3Rules);
			case 409:
				return this.Create_BamlType_ModelVisual3D(isBamlType, useV3Rules);
			case 410:
				return this.Create_BamlType_ModifierKeysConverter(isBamlType, useV3Rules);
			case 411:
				return this.Create_BamlType_MouseActionConverter(isBamlType, useV3Rules);
			case 412:
				return this.Create_BamlType_MouseBinding(isBamlType, useV3Rules);
			case 413:
				return this.Create_BamlType_MouseDevice(isBamlType, useV3Rules);
			case 414:
				return this.Create_BamlType_MouseGesture(isBamlType, useV3Rules);
			case 415:
				return this.Create_BamlType_MouseGestureConverter(isBamlType, useV3Rules);
			case 416:
				return this.Create_BamlType_MultiBinding(isBamlType, useV3Rules);
			case 417:
				return this.Create_BamlType_MultiBindingExpression(isBamlType, useV3Rules);
			case 418:
				return this.Create_BamlType_MultiDataTrigger(isBamlType, useV3Rules);
			case 419:
				return this.Create_BamlType_MultiTrigger(isBamlType, useV3Rules);
			case 420:
				return this.Create_BamlType_NameScope(isBamlType, useV3Rules);
			case 421:
				return this.Create_BamlType_NavigationWindow(isBamlType, useV3Rules);
			case 422:
				return this.Create_BamlType_NullExtension(isBamlType, useV3Rules);
			case 423:
				return this.Create_BamlType_NullableBoolConverter(isBamlType, useV3Rules);
			case 424:
				return this.Create_BamlType_NullableConverter(isBamlType, useV3Rules);
			case 425:
				return this.Create_BamlType_NumberSubstitution(isBamlType, useV3Rules);
			case 426:
				return this.Create_BamlType_Object(isBamlType, useV3Rules);
			case 427:
				return this.Create_BamlType_ObjectAnimationBase(isBamlType, useV3Rules);
			case 428:
				return this.Create_BamlType_ObjectAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 429:
				return this.Create_BamlType_ObjectDataProvider(isBamlType, useV3Rules);
			case 430:
				return this.Create_BamlType_ObjectKeyFrame(isBamlType, useV3Rules);
			case 431:
				return this.Create_BamlType_ObjectKeyFrameCollection(isBamlType, useV3Rules);
			case 432:
				return this.Create_BamlType_OrthographicCamera(isBamlType, useV3Rules);
			case 433:
				return this.Create_BamlType_OuterGlowBitmapEffect(isBamlType, useV3Rules);
			case 434:
				return this.Create_BamlType_Page(isBamlType, useV3Rules);
			case 435:
				return this.Create_BamlType_PageContent(isBamlType, useV3Rules);
			case 436:
				return this.Create_BamlType_PageFunctionBase(isBamlType, useV3Rules);
			case 437:
				return this.Create_BamlType_Panel(isBamlType, useV3Rules);
			case 438:
				return this.Create_BamlType_Paragraph(isBamlType, useV3Rules);
			case 439:
				return this.Create_BamlType_ParallelTimeline(isBamlType, useV3Rules);
			case 440:
				return this.Create_BamlType_ParserContext(isBamlType, useV3Rules);
			case 441:
				return this.Create_BamlType_PasswordBox(isBamlType, useV3Rules);
			case 442:
				return this.Create_BamlType_Path(isBamlType, useV3Rules);
			case 443:
				return this.Create_BamlType_PathFigure(isBamlType, useV3Rules);
			case 444:
				return this.Create_BamlType_PathFigureCollection(isBamlType, useV3Rules);
			case 445:
				return this.Create_BamlType_PathFigureCollectionConverter(isBamlType, useV3Rules);
			case 446:
				return this.Create_BamlType_PathGeometry(isBamlType, useV3Rules);
			case 447:
				return this.Create_BamlType_PathSegment(isBamlType, useV3Rules);
			case 448:
				return this.Create_BamlType_PathSegmentCollection(isBamlType, useV3Rules);
			case 449:
				return this.Create_BamlType_PauseStoryboard(isBamlType, useV3Rules);
			case 450:
				return this.Create_BamlType_Pen(isBamlType, useV3Rules);
			case 451:
				return this.Create_BamlType_PerspectiveCamera(isBamlType, useV3Rules);
			case 452:
				return this.Create_BamlType_PixelFormat(isBamlType, useV3Rules);
			case 453:
				return this.Create_BamlType_PixelFormatConverter(isBamlType, useV3Rules);
			case 454:
				return this.Create_BamlType_PngBitmapDecoder(isBamlType, useV3Rules);
			case 455:
				return this.Create_BamlType_PngBitmapEncoder(isBamlType, useV3Rules);
			case 456:
				return this.Create_BamlType_Point(isBamlType, useV3Rules);
			case 457:
				return this.Create_BamlType_Point3D(isBamlType, useV3Rules);
			case 458:
				return this.Create_BamlType_Point3DAnimation(isBamlType, useV3Rules);
			case 459:
				return this.Create_BamlType_Point3DAnimationBase(isBamlType, useV3Rules);
			case 460:
				return this.Create_BamlType_Point3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 461:
				return this.Create_BamlType_Point3DCollection(isBamlType, useV3Rules);
			case 462:
				return this.Create_BamlType_Point3DCollectionConverter(isBamlType, useV3Rules);
			case 463:
				return this.Create_BamlType_Point3DConverter(isBamlType, useV3Rules);
			case 464:
				return this.Create_BamlType_Point3DKeyFrame(isBamlType, useV3Rules);
			case 465:
				return this.Create_BamlType_Point3DKeyFrameCollection(isBamlType, useV3Rules);
			case 466:
				return this.Create_BamlType_Point4D(isBamlType, useV3Rules);
			case 467:
				return this.Create_BamlType_Point4DConverter(isBamlType, useV3Rules);
			case 468:
				return this.Create_BamlType_PointAnimation(isBamlType, useV3Rules);
			case 469:
				return this.Create_BamlType_PointAnimationBase(isBamlType, useV3Rules);
			case 470:
				return this.Create_BamlType_PointAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 471:
				return this.Create_BamlType_PointAnimationUsingPath(isBamlType, useV3Rules);
			case 472:
				return this.Create_BamlType_PointCollection(isBamlType, useV3Rules);
			case 473:
				return this.Create_BamlType_PointCollectionConverter(isBamlType, useV3Rules);
			case 474:
				return this.Create_BamlType_PointConverter(isBamlType, useV3Rules);
			case 475:
				return this.Create_BamlType_PointIListConverter(isBamlType, useV3Rules);
			case 476:
				return this.Create_BamlType_PointKeyFrame(isBamlType, useV3Rules);
			case 477:
				return this.Create_BamlType_PointKeyFrameCollection(isBamlType, useV3Rules);
			case 478:
				return this.Create_BamlType_PointLight(isBamlType, useV3Rules);
			case 479:
				return this.Create_BamlType_PointLightBase(isBamlType, useV3Rules);
			case 480:
				return this.Create_BamlType_PolyBezierSegment(isBamlType, useV3Rules);
			case 481:
				return this.Create_BamlType_PolyLineSegment(isBamlType, useV3Rules);
			case 482:
				return this.Create_BamlType_PolyQuadraticBezierSegment(isBamlType, useV3Rules);
			case 483:
				return this.Create_BamlType_Polygon(isBamlType, useV3Rules);
			case 484:
				return this.Create_BamlType_Polyline(isBamlType, useV3Rules);
			case 485:
				return this.Create_BamlType_Popup(isBamlType, useV3Rules);
			case 486:
				return this.Create_BamlType_PresentationSource(isBamlType, useV3Rules);
			case 487:
				return this.Create_BamlType_PriorityBinding(isBamlType, useV3Rules);
			case 488:
				return this.Create_BamlType_PriorityBindingExpression(isBamlType, useV3Rules);
			case 489:
				return this.Create_BamlType_ProgressBar(isBamlType, useV3Rules);
			case 490:
				return this.Create_BamlType_ProjectionCamera(isBamlType, useV3Rules);
			case 491:
				return this.Create_BamlType_PropertyPath(isBamlType, useV3Rules);
			case 492:
				return this.Create_BamlType_PropertyPathConverter(isBamlType, useV3Rules);
			case 493:
				return this.Create_BamlType_QuadraticBezierSegment(isBamlType, useV3Rules);
			case 494:
				return this.Create_BamlType_Quaternion(isBamlType, useV3Rules);
			case 495:
				return this.Create_BamlType_QuaternionAnimation(isBamlType, useV3Rules);
			case 496:
				return this.Create_BamlType_QuaternionAnimationBase(isBamlType, useV3Rules);
			case 497:
				return this.Create_BamlType_QuaternionAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 498:
				return this.Create_BamlType_QuaternionConverter(isBamlType, useV3Rules);
			case 499:
				return this.Create_BamlType_QuaternionKeyFrame(isBamlType, useV3Rules);
			case 500:
				return this.Create_BamlType_QuaternionKeyFrameCollection(isBamlType, useV3Rules);
			case 501:
				return this.Create_BamlType_QuaternionRotation3D(isBamlType, useV3Rules);
			case 502:
				return this.Create_BamlType_RadialGradientBrush(isBamlType, useV3Rules);
			case 503:
				return this.Create_BamlType_RadioButton(isBamlType, useV3Rules);
			case 504:
				return this.Create_BamlType_RangeBase(isBamlType, useV3Rules);
			case 505:
				return this.Create_BamlType_Rect(isBamlType, useV3Rules);
			case 506:
				return this.Create_BamlType_Rect3D(isBamlType, useV3Rules);
			case 507:
				return this.Create_BamlType_Rect3DConverter(isBamlType, useV3Rules);
			case 508:
				return this.Create_BamlType_RectAnimation(isBamlType, useV3Rules);
			case 509:
				return this.Create_BamlType_RectAnimationBase(isBamlType, useV3Rules);
			case 510:
				return this.Create_BamlType_RectAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 511:
				return this.Create_BamlType_RectConverter(isBamlType, useV3Rules);
			case 512:
				return this.Create_BamlType_RectKeyFrame(isBamlType, useV3Rules);
			case 513:
				return this.Create_BamlType_RectKeyFrameCollection(isBamlType, useV3Rules);
			case 514:
				return this.Create_BamlType_Rectangle(isBamlType, useV3Rules);
			case 515:
				return this.Create_BamlType_RectangleGeometry(isBamlType, useV3Rules);
			case 516:
				return this.Create_BamlType_RelativeSource(isBamlType, useV3Rules);
			case 517:
				return this.Create_BamlType_RemoveStoryboard(isBamlType, useV3Rules);
			case 518:
				return this.Create_BamlType_RenderOptions(isBamlType, useV3Rules);
			case 519:
				return this.Create_BamlType_RenderTargetBitmap(isBamlType, useV3Rules);
			case 520:
				return this.Create_BamlType_RepeatBehavior(isBamlType, useV3Rules);
			case 521:
				return this.Create_BamlType_RepeatBehaviorConverter(isBamlType, useV3Rules);
			case 522:
				return this.Create_BamlType_RepeatButton(isBamlType, useV3Rules);
			case 523:
				return this.Create_BamlType_ResizeGrip(isBamlType, useV3Rules);
			case 524:
				return this.Create_BamlType_ResourceDictionary(isBamlType, useV3Rules);
			case 525:
				return this.Create_BamlType_ResourceKey(isBamlType, useV3Rules);
			case 526:
				return this.Create_BamlType_ResumeStoryboard(isBamlType, useV3Rules);
			case 527:
				return this.Create_BamlType_RichTextBox(isBamlType, useV3Rules);
			case 528:
				return this.Create_BamlType_RotateTransform(isBamlType, useV3Rules);
			case 529:
				return this.Create_BamlType_RotateTransform3D(isBamlType, useV3Rules);
			case 530:
				return this.Create_BamlType_Rotation3D(isBamlType, useV3Rules);
			case 531:
				return this.Create_BamlType_Rotation3DAnimation(isBamlType, useV3Rules);
			case 532:
				return this.Create_BamlType_Rotation3DAnimationBase(isBamlType, useV3Rules);
			case 533:
				return this.Create_BamlType_Rotation3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 534:
				return this.Create_BamlType_Rotation3DKeyFrame(isBamlType, useV3Rules);
			case 535:
				return this.Create_BamlType_Rotation3DKeyFrameCollection(isBamlType, useV3Rules);
			case 536:
				return this.Create_BamlType_RoutedCommand(isBamlType, useV3Rules);
			case 537:
				return this.Create_BamlType_RoutedEvent(isBamlType, useV3Rules);
			case 538:
				return this.Create_BamlType_RoutedEventConverter(isBamlType, useV3Rules);
			case 539:
				return this.Create_BamlType_RoutedUICommand(isBamlType, useV3Rules);
			case 540:
				return this.Create_BamlType_RoutingStrategy(isBamlType, useV3Rules);
			case 541:
				return this.Create_BamlType_RowDefinition(isBamlType, useV3Rules);
			case 542:
				return this.Create_BamlType_Run(isBamlType, useV3Rules);
			case 543:
				return this.Create_BamlType_RuntimeNamePropertyAttribute(isBamlType, useV3Rules);
			case 544:
				return this.Create_BamlType_SByte(isBamlType, useV3Rules);
			case 545:
				return this.Create_BamlType_SByteConverter(isBamlType, useV3Rules);
			case 546:
				return this.Create_BamlType_ScaleTransform(isBamlType, useV3Rules);
			case 547:
				return this.Create_BamlType_ScaleTransform3D(isBamlType, useV3Rules);
			case 548:
				return this.Create_BamlType_ScrollBar(isBamlType, useV3Rules);
			case 549:
				return this.Create_BamlType_ScrollContentPresenter(isBamlType, useV3Rules);
			case 550:
				return this.Create_BamlType_ScrollViewer(isBamlType, useV3Rules);
			case 551:
				return this.Create_BamlType_Section(isBamlType, useV3Rules);
			case 552:
				return this.Create_BamlType_SeekStoryboard(isBamlType, useV3Rules);
			case 553:
				return this.Create_BamlType_Selector(isBamlType, useV3Rules);
			case 554:
				return this.Create_BamlType_Separator(isBamlType, useV3Rules);
			case 555:
				return this.Create_BamlType_SetStoryboardSpeedRatio(isBamlType, useV3Rules);
			case 556:
				return this.Create_BamlType_Setter(isBamlType, useV3Rules);
			case 557:
				return this.Create_BamlType_SetterBase(isBamlType, useV3Rules);
			case 558:
				return this.Create_BamlType_Shape(isBamlType, useV3Rules);
			case 559:
				return this.Create_BamlType_Single(isBamlType, useV3Rules);
			case 560:
				return this.Create_BamlType_SingleAnimation(isBamlType, useV3Rules);
			case 561:
				return this.Create_BamlType_SingleAnimationBase(isBamlType, useV3Rules);
			case 562:
				return this.Create_BamlType_SingleAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 563:
				return this.Create_BamlType_SingleConverter(isBamlType, useV3Rules);
			case 564:
				return this.Create_BamlType_SingleKeyFrame(isBamlType, useV3Rules);
			case 565:
				return this.Create_BamlType_SingleKeyFrameCollection(isBamlType, useV3Rules);
			case 566:
				return this.Create_BamlType_Size(isBamlType, useV3Rules);
			case 567:
				return this.Create_BamlType_Size3D(isBamlType, useV3Rules);
			case 568:
				return this.Create_BamlType_Size3DConverter(isBamlType, useV3Rules);
			case 569:
				return this.Create_BamlType_SizeAnimation(isBamlType, useV3Rules);
			case 570:
				return this.Create_BamlType_SizeAnimationBase(isBamlType, useV3Rules);
			case 571:
				return this.Create_BamlType_SizeAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 572:
				return this.Create_BamlType_SizeConverter(isBamlType, useV3Rules);
			case 573:
				return this.Create_BamlType_SizeKeyFrame(isBamlType, useV3Rules);
			case 574:
				return this.Create_BamlType_SizeKeyFrameCollection(isBamlType, useV3Rules);
			case 575:
				return this.Create_BamlType_SkewTransform(isBamlType, useV3Rules);
			case 576:
				return this.Create_BamlType_SkipStoryboardToFill(isBamlType, useV3Rules);
			case 577:
				return this.Create_BamlType_Slider(isBamlType, useV3Rules);
			case 578:
				return this.Create_BamlType_SolidColorBrush(isBamlType, useV3Rules);
			case 579:
				return this.Create_BamlType_SoundPlayerAction(isBamlType, useV3Rules);
			case 580:
				return this.Create_BamlType_Span(isBamlType, useV3Rules);
			case 581:
				return this.Create_BamlType_SpecularMaterial(isBamlType, useV3Rules);
			case 582:
				return this.Create_BamlType_SpellCheck(isBamlType, useV3Rules);
			case 583:
				return this.Create_BamlType_SplineByteKeyFrame(isBamlType, useV3Rules);
			case 584:
				return this.Create_BamlType_SplineColorKeyFrame(isBamlType, useV3Rules);
			case 585:
				return this.Create_BamlType_SplineDecimalKeyFrame(isBamlType, useV3Rules);
			case 586:
				return this.Create_BamlType_SplineDoubleKeyFrame(isBamlType, useV3Rules);
			case 587:
				return this.Create_BamlType_SplineInt16KeyFrame(isBamlType, useV3Rules);
			case 588:
				return this.Create_BamlType_SplineInt32KeyFrame(isBamlType, useV3Rules);
			case 589:
				return this.Create_BamlType_SplineInt64KeyFrame(isBamlType, useV3Rules);
			case 590:
				return this.Create_BamlType_SplinePoint3DKeyFrame(isBamlType, useV3Rules);
			case 591:
				return this.Create_BamlType_SplinePointKeyFrame(isBamlType, useV3Rules);
			case 592:
				return this.Create_BamlType_SplineQuaternionKeyFrame(isBamlType, useV3Rules);
			case 593:
				return this.Create_BamlType_SplineRectKeyFrame(isBamlType, useV3Rules);
			case 594:
				return this.Create_BamlType_SplineRotation3DKeyFrame(isBamlType, useV3Rules);
			case 595:
				return this.Create_BamlType_SplineSingleKeyFrame(isBamlType, useV3Rules);
			case 596:
				return this.Create_BamlType_SplineSizeKeyFrame(isBamlType, useV3Rules);
			case 597:
				return this.Create_BamlType_SplineThicknessKeyFrame(isBamlType, useV3Rules);
			case 598:
				return this.Create_BamlType_SplineVector3DKeyFrame(isBamlType, useV3Rules);
			case 599:
				return this.Create_BamlType_SplineVectorKeyFrame(isBamlType, useV3Rules);
			case 600:
				return this.Create_BamlType_SpotLight(isBamlType, useV3Rules);
			case 601:
				return this.Create_BamlType_StackPanel(isBamlType, useV3Rules);
			case 602:
				return this.Create_BamlType_StaticExtension(isBamlType, useV3Rules);
			case 603:
				return this.Create_BamlType_StaticResourceExtension(isBamlType, useV3Rules);
			case 604:
				return this.Create_BamlType_StatusBar(isBamlType, useV3Rules);
			case 605:
				return this.Create_BamlType_StatusBarItem(isBamlType, useV3Rules);
			case 606:
				return this.Create_BamlType_StickyNoteControl(isBamlType, useV3Rules);
			case 607:
				return this.Create_BamlType_StopStoryboard(isBamlType, useV3Rules);
			case 608:
				return this.Create_BamlType_Storyboard(isBamlType, useV3Rules);
			case 609:
				return this.Create_BamlType_StreamGeometry(isBamlType, useV3Rules);
			case 610:
				return this.Create_BamlType_StreamGeometryContext(isBamlType, useV3Rules);
			case 611:
				return this.Create_BamlType_StreamResourceInfo(isBamlType, useV3Rules);
			case 612:
				return this.Create_BamlType_String(isBamlType, useV3Rules);
			case 613:
				return this.Create_BamlType_StringAnimationBase(isBamlType, useV3Rules);
			case 614:
				return this.Create_BamlType_StringAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 615:
				return this.Create_BamlType_StringConverter(isBamlType, useV3Rules);
			case 616:
				return this.Create_BamlType_StringKeyFrame(isBamlType, useV3Rules);
			case 617:
				return this.Create_BamlType_StringKeyFrameCollection(isBamlType, useV3Rules);
			case 618:
				return this.Create_BamlType_StrokeCollection(isBamlType, useV3Rules);
			case 619:
				return this.Create_BamlType_StrokeCollectionConverter(isBamlType, useV3Rules);
			case 620:
				return this.Create_BamlType_Style(isBamlType, useV3Rules);
			case 621:
				return this.Create_BamlType_Stylus(isBamlType, useV3Rules);
			case 622:
				return this.Create_BamlType_StylusDevice(isBamlType, useV3Rules);
			case 623:
				return this.Create_BamlType_TabControl(isBamlType, useV3Rules);
			case 624:
				return this.Create_BamlType_TabItem(isBamlType, useV3Rules);
			case 625:
				return this.Create_BamlType_TabPanel(isBamlType, useV3Rules);
			case 626:
				return this.Create_BamlType_Table(isBamlType, useV3Rules);
			case 627:
				return this.Create_BamlType_TableCell(isBamlType, useV3Rules);
			case 628:
				return this.Create_BamlType_TableColumn(isBamlType, useV3Rules);
			case 629:
				return this.Create_BamlType_TableRow(isBamlType, useV3Rules);
			case 630:
				return this.Create_BamlType_TableRowGroup(isBamlType, useV3Rules);
			case 631:
				return this.Create_BamlType_TabletDevice(isBamlType, useV3Rules);
			case 632:
				return this.Create_BamlType_TemplateBindingExpression(isBamlType, useV3Rules);
			case 633:
				return this.Create_BamlType_TemplateBindingExpressionConverter(isBamlType, useV3Rules);
			case 634:
				return this.Create_BamlType_TemplateBindingExtension(isBamlType, useV3Rules);
			case 635:
				return this.Create_BamlType_TemplateBindingExtensionConverter(isBamlType, useV3Rules);
			case 636:
				return this.Create_BamlType_TemplateKey(isBamlType, useV3Rules);
			case 637:
				return this.Create_BamlType_TemplateKeyConverter(isBamlType, useV3Rules);
			case 638:
				return this.Create_BamlType_TextBlock(isBamlType, useV3Rules);
			case 639:
				return this.Create_BamlType_TextBox(isBamlType, useV3Rules);
			case 640:
				return this.Create_BamlType_TextBoxBase(isBamlType, useV3Rules);
			case 641:
				return this.Create_BamlType_TextComposition(isBamlType, useV3Rules);
			case 642:
				return this.Create_BamlType_TextCompositionManager(isBamlType, useV3Rules);
			case 643:
				return this.Create_BamlType_TextDecoration(isBamlType, useV3Rules);
			case 644:
				return this.Create_BamlType_TextDecorationCollection(isBamlType, useV3Rules);
			case 645:
				return this.Create_BamlType_TextDecorationCollectionConverter(isBamlType, useV3Rules);
			case 646:
				return this.Create_BamlType_TextEffect(isBamlType, useV3Rules);
			case 647:
				return this.Create_BamlType_TextEffectCollection(isBamlType, useV3Rules);
			case 648:
				return this.Create_BamlType_TextElement(isBamlType, useV3Rules);
			case 649:
				return this.Create_BamlType_TextSearch(isBamlType, useV3Rules);
			case 650:
				return this.Create_BamlType_ThemeDictionaryExtension(isBamlType, useV3Rules);
			case 651:
				return this.Create_BamlType_Thickness(isBamlType, useV3Rules);
			case 652:
				return this.Create_BamlType_ThicknessAnimation(isBamlType, useV3Rules);
			case 653:
				return this.Create_BamlType_ThicknessAnimationBase(isBamlType, useV3Rules);
			case 654:
				return this.Create_BamlType_ThicknessAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 655:
				return this.Create_BamlType_ThicknessConverter(isBamlType, useV3Rules);
			case 656:
				return this.Create_BamlType_ThicknessKeyFrame(isBamlType, useV3Rules);
			case 657:
				return this.Create_BamlType_ThicknessKeyFrameCollection(isBamlType, useV3Rules);
			case 658:
				return this.Create_BamlType_Thumb(isBamlType, useV3Rules);
			case 659:
				return this.Create_BamlType_TickBar(isBamlType, useV3Rules);
			case 660:
				return this.Create_BamlType_TiffBitmapDecoder(isBamlType, useV3Rules);
			case 661:
				return this.Create_BamlType_TiffBitmapEncoder(isBamlType, useV3Rules);
			case 662:
				return this.Create_BamlType_TileBrush(isBamlType, useV3Rules);
			case 663:
				return this.Create_BamlType_TimeSpan(isBamlType, useV3Rules);
			case 664:
				return this.Create_BamlType_TimeSpanConverter(isBamlType, useV3Rules);
			case 665:
				return this.Create_BamlType_Timeline(isBamlType, useV3Rules);
			case 666:
				return this.Create_BamlType_TimelineCollection(isBamlType, useV3Rules);
			case 667:
				return this.Create_BamlType_TimelineGroup(isBamlType, useV3Rules);
			case 668:
				return this.Create_BamlType_ToggleButton(isBamlType, useV3Rules);
			case 669:
				return this.Create_BamlType_ToolBar(isBamlType, useV3Rules);
			case 670:
				return this.Create_BamlType_ToolBarOverflowPanel(isBamlType, useV3Rules);
			case 671:
				return this.Create_BamlType_ToolBarPanel(isBamlType, useV3Rules);
			case 672:
				return this.Create_BamlType_ToolBarTray(isBamlType, useV3Rules);
			case 673:
				return this.Create_BamlType_ToolTip(isBamlType, useV3Rules);
			case 674:
				return this.Create_BamlType_ToolTipService(isBamlType, useV3Rules);
			case 675:
				return this.Create_BamlType_Track(isBamlType, useV3Rules);
			case 676:
				return this.Create_BamlType_Transform(isBamlType, useV3Rules);
			case 677:
				return this.Create_BamlType_Transform3D(isBamlType, useV3Rules);
			case 678:
				return this.Create_BamlType_Transform3DCollection(isBamlType, useV3Rules);
			case 679:
				return this.Create_BamlType_Transform3DGroup(isBamlType, useV3Rules);
			case 680:
				return this.Create_BamlType_TransformCollection(isBamlType, useV3Rules);
			case 681:
				return this.Create_BamlType_TransformConverter(isBamlType, useV3Rules);
			case 682:
				return this.Create_BamlType_TransformGroup(isBamlType, useV3Rules);
			case 683:
				return this.Create_BamlType_TransformedBitmap(isBamlType, useV3Rules);
			case 684:
				return this.Create_BamlType_TranslateTransform(isBamlType, useV3Rules);
			case 685:
				return this.Create_BamlType_TranslateTransform3D(isBamlType, useV3Rules);
			case 686:
				return this.Create_BamlType_TreeView(isBamlType, useV3Rules);
			case 687:
				return this.Create_BamlType_TreeViewItem(isBamlType, useV3Rules);
			case 688:
				return this.Create_BamlType_Trigger(isBamlType, useV3Rules);
			case 689:
				return this.Create_BamlType_TriggerAction(isBamlType, useV3Rules);
			case 690:
				return this.Create_BamlType_TriggerBase(isBamlType, useV3Rules);
			case 691:
				return this.Create_BamlType_TypeExtension(isBamlType, useV3Rules);
			case 692:
				return this.Create_BamlType_TypeTypeConverter(isBamlType, useV3Rules);
			case 693:
				return this.Create_BamlType_Typography(isBamlType, useV3Rules);
			case 694:
				return this.Create_BamlType_UIElement(isBamlType, useV3Rules);
			case 695:
				return this.Create_BamlType_UInt16(isBamlType, useV3Rules);
			case 696:
				return this.Create_BamlType_UInt16Converter(isBamlType, useV3Rules);
			case 697:
				return this.Create_BamlType_UInt32(isBamlType, useV3Rules);
			case 698:
				return this.Create_BamlType_UInt32Converter(isBamlType, useV3Rules);
			case 699:
				return this.Create_BamlType_UInt64(isBamlType, useV3Rules);
			case 700:
				return this.Create_BamlType_UInt64Converter(isBamlType, useV3Rules);
			case 701:
				return this.Create_BamlType_UShortIListConverter(isBamlType, useV3Rules);
			case 702:
				return this.Create_BamlType_Underline(isBamlType, useV3Rules);
			case 703:
				return this.Create_BamlType_UniformGrid(isBamlType, useV3Rules);
			case 704:
				return this.Create_BamlType_Uri(isBamlType, useV3Rules);
			case 705:
				return this.Create_BamlType_UriTypeConverter(isBamlType, useV3Rules);
			case 706:
				return this.Create_BamlType_UserControl(isBamlType, useV3Rules);
			case 707:
				return this.Create_BamlType_Validation(isBamlType, useV3Rules);
			case 708:
				return this.Create_BamlType_Vector(isBamlType, useV3Rules);
			case 709:
				return this.Create_BamlType_Vector3D(isBamlType, useV3Rules);
			case 710:
				return this.Create_BamlType_Vector3DAnimation(isBamlType, useV3Rules);
			case 711:
				return this.Create_BamlType_Vector3DAnimationBase(isBamlType, useV3Rules);
			case 712:
				return this.Create_BamlType_Vector3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 713:
				return this.Create_BamlType_Vector3DCollection(isBamlType, useV3Rules);
			case 714:
				return this.Create_BamlType_Vector3DCollectionConverter(isBamlType, useV3Rules);
			case 715:
				return this.Create_BamlType_Vector3DConverter(isBamlType, useV3Rules);
			case 716:
				return this.Create_BamlType_Vector3DKeyFrame(isBamlType, useV3Rules);
			case 717:
				return this.Create_BamlType_Vector3DKeyFrameCollection(isBamlType, useV3Rules);
			case 718:
				return this.Create_BamlType_VectorAnimation(isBamlType, useV3Rules);
			case 719:
				return this.Create_BamlType_VectorAnimationBase(isBamlType, useV3Rules);
			case 720:
				return this.Create_BamlType_VectorAnimationUsingKeyFrames(isBamlType, useV3Rules);
			case 721:
				return this.Create_BamlType_VectorCollection(isBamlType, useV3Rules);
			case 722:
				return this.Create_BamlType_VectorCollectionConverter(isBamlType, useV3Rules);
			case 723:
				return this.Create_BamlType_VectorConverter(isBamlType, useV3Rules);
			case 724:
				return this.Create_BamlType_VectorKeyFrame(isBamlType, useV3Rules);
			case 725:
				return this.Create_BamlType_VectorKeyFrameCollection(isBamlType, useV3Rules);
			case 726:
				return this.Create_BamlType_VideoDrawing(isBamlType, useV3Rules);
			case 727:
				return this.Create_BamlType_ViewBase(isBamlType, useV3Rules);
			case 728:
				return this.Create_BamlType_Viewbox(isBamlType, useV3Rules);
			case 729:
				return this.Create_BamlType_Viewport3D(isBamlType, useV3Rules);
			case 730:
				return this.Create_BamlType_Viewport3DVisual(isBamlType, useV3Rules);
			case 731:
				return this.Create_BamlType_VirtualizingPanel(isBamlType, useV3Rules);
			case 732:
				return this.Create_BamlType_VirtualizingStackPanel(isBamlType, useV3Rules);
			case 733:
				return this.Create_BamlType_Visual(isBamlType, useV3Rules);
			case 734:
				return this.Create_BamlType_Visual3D(isBamlType, useV3Rules);
			case 735:
				return this.Create_BamlType_VisualBrush(isBamlType, useV3Rules);
			case 736:
				return this.Create_BamlType_VisualTarget(isBamlType, useV3Rules);
			case 737:
				return this.Create_BamlType_WeakEventManager(isBamlType, useV3Rules);
			case 738:
				return this.Create_BamlType_WhitespaceSignificantCollectionAttribute(isBamlType, useV3Rules);
			case 739:
				return this.Create_BamlType_Window(isBamlType, useV3Rules);
			case 740:
				return this.Create_BamlType_WmpBitmapDecoder(isBamlType, useV3Rules);
			case 741:
				return this.Create_BamlType_WmpBitmapEncoder(isBamlType, useV3Rules);
			case 742:
				return this.Create_BamlType_WrapPanel(isBamlType, useV3Rules);
			case 743:
				return this.Create_BamlType_WriteableBitmap(isBamlType, useV3Rules);
			case 744:
				return this.Create_BamlType_XamlBrushSerializer(isBamlType, useV3Rules);
			case 745:
				return this.Create_BamlType_XamlInt32CollectionSerializer(isBamlType, useV3Rules);
			case 746:
				return this.Create_BamlType_XamlPathDataSerializer(isBamlType, useV3Rules);
			case 747:
				return this.Create_BamlType_XamlPoint3DCollectionSerializer(isBamlType, useV3Rules);
			case 748:
				return this.Create_BamlType_XamlPointCollectionSerializer(isBamlType, useV3Rules);
			case 749:
				return this.Create_BamlType_XamlReader(isBamlType, useV3Rules);
			case 750:
				return this.Create_BamlType_XamlStyleSerializer(isBamlType, useV3Rules);
			case 751:
				return this.Create_BamlType_XamlTemplateSerializer(isBamlType, useV3Rules);
			case 752:
				return this.Create_BamlType_XamlVector3DCollectionSerializer(isBamlType, useV3Rules);
			case 753:
				return this.Create_BamlType_XamlWriter(isBamlType, useV3Rules);
			case 754:
				return this.Create_BamlType_XmlDataProvider(isBamlType, useV3Rules);
			case 755:
				return this.Create_BamlType_XmlLangPropertyAttribute(isBamlType, useV3Rules);
			case 756:
				return this.Create_BamlType_XmlLanguage(isBamlType, useV3Rules);
			case 757:
				return this.Create_BamlType_XmlLanguageConverter(isBamlType, useV3Rules);
			case 758:
				return this.Create_BamlType_XmlNamespaceMapping(isBamlType, useV3Rules);
			case 759:
				return this.Create_BamlType_ZoomPercentageConverter(isBamlType, useV3Rules);
			default:
				throw new InvalidOperationException("Invalid BAML number");
			}
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x001BB190 File Offset: 0x001BA190
		private uint GetTypeNameHash(string typeName)
		{
			uint num = 0U;
			int num2 = 0;
			while (num2 < 26 && num2 < typeName.Length)
			{
				num = 101U * num + (uint)typeName[num2];
				num2++;
			}
			return num;
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x001BB1C4 File Offset: 0x001BA1C4
		protected WpfKnownType CreateKnownBamlType(string typeName, bool isBamlType, bool useV3Rules)
		{
			uint typeNameHash = this.GetTypeNameHash(typeName);
			if (typeNameHash <= 2045195350U)
			{
				if (typeNameHash <= 961185762U)
				{
					if (typeNameHash <= 384741759U)
					{
						if (typeNameHash <= 158646293U)
						{
							if (typeNameHash <= 79385712U)
							{
								if (typeNameHash <= 44267921U)
								{
									if (typeNameHash <= 10713943U)
									{
										if (typeNameHash <= 878704U)
										{
											if (typeNameHash == 826391U)
											{
												return this.Create_BamlType_Pen(isBamlType, useV3Rules);
											}
											if (typeNameHash == 848409U)
											{
												return this.Create_BamlType_Run(isBamlType, useV3Rules);
											}
											if (typeNameHash == 878704U)
											{
												return this.Create_BamlType_Uri(isBamlType, useV3Rules);
											}
										}
										else
										{
											if (typeNameHash == 7210206U)
											{
												return this.Create_BamlType_Vector3DKeyFrameCollection(isBamlType, useV3Rules);
											}
											if (typeNameHash == 8626695U)
											{
												return this.Create_BamlType_Typography(isBamlType, useV3Rules);
											}
											if (typeNameHash == 10713943U)
											{
												return this.Create_BamlType_AxisAngleRotation3D(isBamlType, useV3Rules);
											}
										}
									}
									else if (typeNameHash <= 21757238U)
									{
										if (typeNameHash == 17341202U)
										{
											return this.Create_BamlType_RectKeyFrameCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 19590438U)
										{
											return this.Create_BamlType_ItemsPanelTemplate(isBamlType, useV3Rules);
										}
										if (typeNameHash == 21757238U)
										{
											return this.Create_BamlType_Quaternion(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 27438720U)
										{
											return this.Create_BamlType_FigureLength(isBamlType, useV3Rules);
										}
										if (typeNameHash == 35895921U)
										{
											return this.Create_BamlType_ComponentResourceKeyConverter(isBamlType, useV3Rules);
										}
										if (typeNameHash == 44267921U)
										{
											return this.Create_BamlType_GridViewRowPresenter(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 72192662U)
								{
									if (typeNameHash <= 69143185U)
									{
										if (typeNameHash == 50494706U)
										{
											return this.Create_BamlType_CommandBindingCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 56425604U)
										{
											return this.Create_BamlType_SplinePoint3DKeyFrame(isBamlType, useV3Rules);
										}
										if (typeNameHash == 69143185U)
										{
											return this.Create_BamlType_Bold(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 69246004U)
										{
											return this.Create_BamlType_Byte(isBamlType, useV3Rules);
										}
										if (typeNameHash == 70100982U)
										{
											return this.Create_BamlType_Char(isBamlType, useV3Rules);
										}
										if (typeNameHash == 72192662U)
										{
											return this.Create_BamlType_MatrixCamera(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 74324990U)
								{
									if (typeNameHash == 72224805U)
									{
										return this.Create_BamlType_Enum(isBamlType, useV3Rules);
									}
									if (typeNameHash == 74282775U)
									{
										return this.Create_BamlType_RotateTransform(isBamlType, useV3Rules);
									}
									if (typeNameHash == 74324990U)
									{
										return this.Create_BamlType_Grid(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 74355593U)
									{
										return this.Create_BamlType_Guid(isBamlType, useV3Rules);
									}
									if (typeNameHash == 79385192U)
									{
										return this.Create_BamlType_Line(isBamlType, useV3Rules);
									}
									if (typeNameHash == 79385712U)
									{
										return this.Create_BamlType_List(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 116324695U)
							{
								if (typeNameHash <= 86639180U)
								{
									if (typeNameHash <= 83425397U)
									{
										if (typeNameHash == 80374705U)
										{
											return this.Create_BamlType_Menu(isBamlType, useV3Rules);
										}
										if (typeNameHash == 83424081U)
										{
											return this.Create_BamlType_Page(isBamlType, useV3Rules);
										}
										if (typeNameHash == 83425397U)
										{
											return this.Create_BamlType_Path(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 85525098U)
										{
											return this.Create_BamlType_Rect(isBamlType, useV3Rules);
										}
										if (typeNameHash == 86598511U)
										{
											return this.Create_BamlType_Size(isBamlType, useV3Rules);
										}
										if (typeNameHash == 86639180U)
										{
											return this.Create_BamlType_Visual(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 95311897U)
								{
									if (typeNameHash == 86667402U)
									{
										return this.Create_BamlType_Span(isBamlType, useV3Rules);
									}
									if (typeNameHash == 92454412U)
									{
										return this.Create_BamlType_ColorAnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
									if (typeNameHash == 95311897U)
									{
										return this.Create_BamlType_KeyboardDevice(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 98196275U)
									{
										return this.Create_BamlType_DoubleConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 114848175U)
									{
										return this.Create_BamlType_XamlPoint3DCollectionSerializer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 116324695U)
									{
										return this.Create_BamlType_SByte(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 141025390U)
							{
								if (typeNameHash <= 133371900U)
								{
									if (typeNameHash == 117546261U)
									{
										return this.Create_BamlType_SplineVector3DKeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 129393695U)
									{
										return this.Create_BamlType_VectorAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 133371900U)
									{
										return this.Create_BamlType_DoubleIListConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 133966438U)
									{
										return this.Create_BamlType_ScrollContentPresenter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 138822808U)
									{
										return this.Create_BamlType_UIElementCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 141025390U)
									{
										return this.Create_BamlType_CharKeyFrame(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 151882568U)
							{
								if (typeNameHash == 149784707U)
								{
									return this.Create_BamlType_TextDecorationCollectionConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 150436622U)
								{
									return this.Create_BamlType_SplineRotation3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 151882568U)
								{
									return this.Create_BamlType_ModelVisual3D(isBamlType, useV3Rules);
								}
							}
							else if (typeNameHash <= 155230905U)
							{
								if (typeNameHash == 153543503U)
								{
									return this.Create_BamlType_CollectionView(isBamlType, useV3Rules);
								}
								if (typeNameHash == 155230905U)
								{
									return this.Create_BamlType_Shape(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 157696880U)
								{
									return this.Create_BamlType_BrushConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 158646293U)
								{
									return this.Create_BamlType_TranslateTransform3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 254218629U)
						{
							if (typeNameHash <= 185134902U)
							{
								if (typeNameHash <= 167522129U)
								{
									if (typeNameHash <= 160906176U)
									{
										if (typeNameHash == 158796542U)
										{
											return this.Create_BamlType_TileBrush(isBamlType, useV3Rules);
										}
										if (typeNameHash == 159112278U)
										{
											return this.Create_BamlType_DecimalAnimationBase(isBamlType, useV3Rules);
										}
										if (typeNameHash == 160906176U)
										{
											return this.Create_BamlType_GroupItem(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 162191870U)
										{
											return this.Create_BamlType_ThicknessKeyFrameCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 163112773U)
										{
											return this.Create_BamlType_WmpBitmapEncoder(isBamlType, useV3Rules);
										}
										if (typeNameHash == 167522129U)
										{
											return this.Create_BamlType_EventManager(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 172295577U)
								{
									if (typeNameHash == 167785563U)
									{
										return this.Create_BamlType_XamlInt32CollectionSerializer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 167838937U)
									{
										return this.Create_BamlType_Style(isBamlType, useV3Rules);
									}
									if (typeNameHash == 172295577U)
									{
										return this.Create_BamlType_SeekStoryboard(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 176201414U)
									{
										return this.Create_BamlType_BindingListCollectionView(isBamlType, useV3Rules);
									}
									if (typeNameHash == 180014290U)
									{
										return this.Create_BamlType_ProgressBar(isBamlType, useV3Rules);
									}
									if (typeNameHash == 185134902U)
									{
										return this.Create_BamlType_Int16Converter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 230922235U)
							{
								if (typeNameHash <= 193712015U)
								{
									if (typeNameHash == 185603331U)
									{
										return this.Create_BamlType_WhitespaceSignificantCollectionAttribute(isBamlType, useV3Rules);
									}
									if (typeNameHash == 188925504U)
									{
										return this.Create_BamlType_DiscreteInt64KeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 193712015U)
									{
										return this.Create_BamlType_ModifierKeysConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 208056328U)
									{
										return this.Create_BamlType_Int64AnimationBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 220163992U)
									{
										return this.Create_BamlType_GeometryCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 230922235U)
									{
										return this.Create_BamlType_ThicknessAnimationBase(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 246620386U)
							{
								if (typeNameHash == 236543168U)
								{
									return this.Create_BamlType_CultureInfo(isBamlType, useV3Rules);
								}
								if (typeNameHash == 240474481U)
								{
									return this.Create_BamlType_MultiDataTrigger(isBamlType, useV3Rules);
								}
								if (typeNameHash == 246620386U)
								{
									return this.Create_BamlType_HeaderedContentControl(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 252088996U)
								{
									return this.Create_BamlType_Table(isBamlType, useV3Rules);
								}
								if (typeNameHash == 253854091U)
								{
									return this.Create_BamlType_DoubleAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 254218629U)
								{
									return this.Create_BamlType_DiscreteVector3DKeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 314824934U)
						{
							if (typeNameHash <= 278513255U)
							{
								if (typeNameHash <= 262392462U)
								{
									if (typeNameHash == 259495020U)
									{
										return this.Create_BamlType_Thumb(isBamlType, useV3Rules);
									}
									if (typeNameHash == 260974524U)
									{
										return this.Create_BamlType_KeyGestureConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 262392462U)
									{
										return this.Create_BamlType_TextBox(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 265347790U)
									{
										return this.Create_BamlType_OuterGlowBitmapEffect(isBamlType, useV3Rules);
									}
									if (typeNameHash == 269593009U)
									{
										return this.Create_BamlType_Track(isBamlType, useV3Rules);
									}
									if (typeNameHash == 278513255U)
									{
										return this.Create_BamlType_Vector3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 291478073U)
							{
								if (typeNameHash == 283659891U)
								{
									return this.Create_BamlType_PenLineJoin(isBamlType, useV3Rules);
								}
								if (typeNameHash == 285954745U)
								{
									return this.Create_BamlType_TemplateKeyConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 291478073U)
								{
									return this.Create_BamlType_GifBitmapDecoder(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 297191555U)
								{
									return this.Create_BamlType_LineSegment(isBamlType, useV3Rules);
								}
								if (typeNameHash == 300220768U)
								{
									return this.Create_BamlType_CharAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 314824934U)
								{
									return this.Create_BamlType_Int32RectConverter(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 339935827U)
						{
							if (typeNameHash <= 333511440U)
							{
								if (typeNameHash == 324370636U)
								{
									return this.Create_BamlType_Thickness(isBamlType, useV3Rules);
								}
								if (typeNameHash == 326446886U)
								{
									return this.Create_BamlType_DecimalAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 333511440U)
								{
									return this.Create_BamlType_PngBitmapDecoder(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 337659401U)
								{
									return this.Create_BamlType_Point3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 339474011U)
								{
									return this.Create_BamlType_Decimal(isBamlType, useV3Rules);
								}
								if (typeNameHash == 339935827U)
								{
									return this.Create_BamlType_DiscreteByteKeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 363476966U)
						{
							if (typeNameHash == 340792718U)
							{
								return this.Create_BamlType_Int16Animation(isBamlType, useV3Rules);
							}
							if (typeNameHash == 357673449U)
							{
								return this.Create_BamlType_RuntimeNamePropertyAttribute(isBamlType, useV3Rules);
							}
							if (typeNameHash == 363476966U)
							{
								return this.Create_BamlType_UInt64Converter(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 374151590U)
						{
							if (typeNameHash == 373217479U)
							{
								return this.Create_BamlType_TemplateBindingExpression(isBamlType, useV3Rules);
							}
							if (typeNameHash == 374151590U)
							{
								return this.Create_BamlType_BindingBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 374415758U)
							{
								return this.Create_BamlType_ToggleButton(isBamlType, useV3Rules);
							}
							if (typeNameHash == 384741759U)
							{
								return this.Create_BamlType_RadialGradientBrush(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 655979150U)
					{
						if (typeNameHash <= 511132298U)
						{
							if (typeNameHash <= 435869667U)
							{
								if (typeNameHash <= 411745576U)
								{
									if (typeNameHash <= 390343400U)
									{
										if (typeNameHash == 386930200U)
										{
											return this.Create_BamlType_EmissiveMaterial(isBamlType, useV3Rules);
										}
										if (typeNameHash == 387234139U)
										{
											return this.Create_BamlType_Decorator(isBamlType, useV3Rules);
										}
										if (typeNameHash == 390343400U)
										{
											return this.Create_BamlType_RichTextBox(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 409173380U)
										{
											return this.Create_BamlType_Polyline(isBamlType, useV3Rules);
										}
										if (typeNameHash == 409221055U)
										{
											return this.Create_BamlType_LinearThicknessKeyFrame(isBamlType, useV3Rules);
										}
										if (typeNameHash == 411745576U)
										{
											return this.Create_BamlType_StatusBarItem(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 425410901U)
								{
									if (typeNameHash == 412334313U)
									{
										return this.Create_BamlType_DocumentViewer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 414460394U)
									{
										return this.Create_BamlType_MultiBinding(isBamlType, useV3Rules);
									}
									if (typeNameHash == 425410901U)
									{
										return this.Create_BamlType_PresentationSource(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 431709905U)
									{
										return this.Create_BamlType_RowDefinitionCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 433371184U)
									{
										return this.Create_BamlType_MeshGeometry3D(isBamlType, useV3Rules);
									}
									if (typeNameHash == 435869667U)
									{
										return this.Create_BamlType_ContextMenuService(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 492584280U)
							{
								if (typeNameHash <= 473143590U)
								{
									if (typeNameHash == 461968488U)
									{
										return this.Create_BamlType_RenderTargetBitmap(isBamlType, useV3Rules);
									}
									if (typeNameHash == 465416194U)
									{
										return this.Create_BamlType_AdornedElementPlaceholder(isBamlType, useV3Rules);
									}
									if (typeNameHash == 473143590U)
									{
										return this.Create_BamlType_BitmapEffect(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 481300314U)
									{
										return this.Create_BamlType_Int64AnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
									if (typeNameHash == 490900943U)
									{
										return this.Create_BamlType_IAddChildInternal(isBamlType, useV3Rules);
									}
									if (typeNameHash == 492584280U)
									{
										return this.Create_BamlType_MouseGestureConverter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 507138120U)
							{
								if (typeNameHash == 501987435U)
								{
									return this.Create_BamlType_Rotation3DAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 504184511U)
								{
									return this.Create_BamlType_ToolBarPanel(isBamlType, useV3Rules);
								}
								if (typeNameHash == 507138120U)
								{
									return this.Create_BamlType_BooleanConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 509621479U)
								{
									return this.Create_BamlType_Double(isBamlType, useV3Rules);
								}
								if (typeNameHash == 511076833U)
								{
									return this.Create_BamlType_Localization(isBamlType, useV3Rules);
								}
								if (typeNameHash == 511132298U)
								{
									return this.Create_BamlType_DynamicResourceExtension(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 602421868U)
						{
							if (typeNameHash <= 566074239U)
							{
								if (typeNameHash <= 532150459U)
								{
									if (typeNameHash == 522405838U)
									{
										return this.Create_BamlType_UShortIListConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 525600274U)
									{
										return this.Create_BamlType_TemplateBindingExtensionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 532150459U)
									{
										return this.Create_BamlType_DateTimeConverter2(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 554920085U)
									{
										return this.Create_BamlType_FontFamily(isBamlType, useV3Rules);
									}
									if (typeNameHash == 563168829U)
									{
										return this.Create_BamlType_Rect3D(isBamlType, useV3Rules);
									}
									if (typeNameHash == 566074239U)
									{
										return this.Create_BamlType_Expander(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 577530966U)
							{
								if (typeNameHash == 568845828U)
								{
									return this.Create_BamlType_ScrollBarVisibility(isBamlType, useV3Rules);
								}
								if (typeNameHash == 571143672U)
								{
									return this.Create_BamlType_GridViewRowPresenterBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 577530966U)
								{
									return this.Create_BamlType_DataTrigger(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 582823334U)
								{
									return this.Create_BamlType_UniformGrid(isBamlType, useV3Rules);
								}
								if (typeNameHash == 585590105U)
								{
									return this.Create_BamlType_CombinedGeometry(isBamlType, useV3Rules);
								}
								if (typeNameHash == 602421868U)
								{
									return this.Create_BamlType_MouseBinding(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 620560167U)
						{
							if (typeNameHash <= 615309592U)
							{
								if (typeNameHash == 603960058U)
								{
									return this.Create_BamlType_ColorAnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 614788594U)
								{
									return this.Create_BamlType_ContextMenu(isBamlType, useV3Rules);
								}
								if (typeNameHash == 615309592U)
								{
									return this.Create_BamlType_UIElement(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 615357807U)
								{
									return this.Create_BamlType_VectorAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 615898683U)
								{
									return this.Create_BamlType_TypeExtension(isBamlType, useV3Rules);
								}
								if (typeNameHash == 620560167U)
								{
									return this.Create_BamlType_GeneralTransformGroup(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 627070138U)
						{
							if (typeNameHash == 620850810U)
							{
								return this.Create_BamlType_SizeAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 623567164U)
							{
								return this.Create_BamlType_PageContent(isBamlType, useV3Rules);
							}
							if (typeNameHash == 627070138U)
							{
								return this.Create_BamlType_SplineColorKeyFrame(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 646994170U)
						{
							if (typeNameHash == 640587303U)
							{
								return this.Create_BamlType_RoutingStrategy(isBamlType, useV3Rules);
							}
							if (typeNameHash == 646994170U)
							{
								return this.Create_BamlType_LinearVectorKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 649244994U)
							{
								return this.Create_BamlType_CommandBinding(isBamlType, useV3Rules);
							}
							if (typeNameHash == 655979150U)
							{
								return this.Create_BamlType_SpecularMaterial(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 821654102U)
					{
						if (typeNameHash <= 727501438U)
						{
							if (typeNameHash <= 686841832U)
							{
								if (typeNameHash <= 672969529U)
								{
									if (typeNameHash == 664895538U)
									{
										return this.Create_BamlType_TriggerAction(isBamlType, useV3Rules);
									}
									if (typeNameHash == 665996286U)
									{
										return this.Create_BamlType_QuaternionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 672969529U)
									{
										return this.Create_BamlType_CornerRadiusConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 685428999U)
									{
										return this.Create_BamlType_PixelFormat(isBamlType, useV3Rules);
									}
									if (typeNameHash == 686620977U)
									{
										return this.Create_BamlType_XamlStyleSerializer(isBamlType, useV3Rules);
									}
									if (typeNameHash == 686841832U)
									{
										return this.Create_BamlType_GeometryConverter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 712702706U)
							{
								if (typeNameHash == 687971593U)
								{
									return this.Create_BamlType_JpegBitmapDecoder(isBamlType, useV3Rules);
								}
								if (typeNameHash == 698201008U)
								{
									return this.Create_BamlType_GridLength(isBamlType, useV3Rules);
								}
								if (typeNameHash == 712702706U)
								{
									return this.Create_BamlType_DocumentReference(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 713325256U)
								{
									return this.Create_BamlType_FrameworkElementFactory(isBamlType, useV3Rules);
								}
								if (typeNameHash == 725957013U)
								{
									return this.Create_BamlType_Int32AnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 727501438U)
								{
									return this.Create_BamlType_JournalOwnership(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 779609571U)
						{
							if (typeNameHash <= 748275923U)
							{
								if (typeNameHash == 734249444U)
								{
									return this.Create_BamlType_BevelBitmapEffect(isBamlType, useV3Rules);
								}
								if (typeNameHash == 741421013U)
								{
									return this.Create_BamlType_DiscreteCharKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 748275923U)
								{
									return this.Create_BamlType_UInt16Converter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 749660283U)
								{
									return this.Create_BamlType_InlineCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 758538788U)
								{
									return this.Create_BamlType_ICommand(isBamlType, useV3Rules);
								}
								if (typeNameHash == 779609571U)
								{
									return this.Create_BamlType_ScaleTransform3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 784826098U)
						{
							if (typeNameHash == 782411712U)
							{
								return this.Create_BamlType_FrameworkPropertyMetadata(isBamlType, useV3Rules);
							}
							if (typeNameHash == 784038997U)
							{
								return this.Create_BamlType_TextDecoration(isBamlType, useV3Rules);
							}
							if (typeNameHash == 784826098U)
							{
								return this.Create_BamlType_Underline(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 787776053U)
							{
								return this.Create_BamlType_IStyleConnector(isBamlType, useV3Rules);
							}
							if (typeNameHash == 807830300U)
							{
								return this.Create_BamlType_DefinitionBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 821654102U)
							{
								return this.Create_BamlType_QuaternionAnimation(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 905080928U)
					{
						if (typeNameHash <= 874593556U)
						{
							if (typeNameHash <= 861523813U)
							{
								if (typeNameHash == 832085183U)
								{
									return this.Create_BamlType_NullableBoolConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 840953286U)
								{
									return this.Create_BamlType_PointKeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 861523813U)
								{
									return this.Create_BamlType_PriorityBindingExpression(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 863295067U)
								{
									return this.Create_BamlType_ColorConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 864192108U)
								{
									return this.Create_BamlType_ThicknessConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 874593556U)
								{
									return this.Create_BamlType_ClockController(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 896504879U)
						{
							if (typeNameHash == 874609234U)
							{
								return this.Create_BamlType_DoubleAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 880110784U)
							{
								return this.Create_BamlType_ExpressionConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 896504879U)
							{
								return this.Create_BamlType_DoubleCollection(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 897586265U)
							{
								return this.Create_BamlType_SplineRectKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 897706848U)
							{
								return this.Create_BamlType_TextBlock(isBamlType, useV3Rules);
							}
							if (typeNameHash == 905080928U)
							{
								return this.Create_BamlType_FixedDocumentSequence(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 926965831U)
					{
						if (typeNameHash <= 916823320U)
						{
							if (typeNameHash == 906240700U)
							{
								return this.Create_BamlType_UserControl(isBamlType, useV3Rules);
							}
							if (typeNameHash == 912040738U)
							{
								return this.Create_BamlType_TextEffectCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 916823320U)
							{
								return this.Create_BamlType_InputDevice(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 921174220U)
							{
								return this.Create_BamlType_TriggerCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 922642898U)
							{
								return this.Create_BamlType_PointLight(isBamlType, useV3Rules);
							}
							if (typeNameHash == 926965831U)
							{
								return this.Create_BamlType_InputScopeName(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 937862401U)
					{
						if (typeNameHash == 936485592U)
						{
							return this.Create_BamlType_FrameworkRichTextComposition(isBamlType, useV3Rules);
						}
						if (typeNameHash == 937814480U)
						{
							return this.Create_BamlType_StrokeCollectionConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 937862401U)
						{
							return this.Create_BamlType_GlyphTypeface(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 949941650U)
					{
						if (typeNameHash == 948576441U)
						{
							return this.Create_BamlType_ArcSegment(isBamlType, useV3Rules);
						}
						if (typeNameHash == 949941650U)
						{
							return this.Create_BamlType_PropertyPath(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 959679175U)
						{
							return this.Create_BamlType_XamlPathDataSerializer(isBamlType, useV3Rules);
						}
						if (typeNameHash == 961185762U)
						{
							return this.Create_BamlType_Border(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1563195901U)
				{
					if (typeNameHash <= 1283950600U)
					{
						if (typeNameHash <= 1083922042U)
						{
							if (typeNameHash <= 1021162590U)
							{
								if (typeNameHash <= 997998281U)
								{
									if (typeNameHash <= 991727131U)
									{
										if (typeNameHash == 967604372U)
										{
											return this.Create_BamlType_FormatConvertedBitmap(isBamlType, useV3Rules);
										}
										if (typeNameHash == 977040319U)
										{
											return this.Create_BamlType_Validation(isBamlType, useV3Rules);
										}
										if (typeNameHash == 991727131U)
										{
											return this.Create_BamlType_MouseActionConverter(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 996200203U)
										{
											return this.Create_BamlType_AnimationTimeline(isBamlType, useV3Rules);
										}
										if (typeNameHash == 997254168U)
										{
											return this.Create_BamlType_Geometry(isBamlType, useV3Rules);
										}
										if (typeNameHash == 997998281U)
										{
											return this.Create_BamlType_ComboBox(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 1019262156U)
								{
									if (typeNameHash == 1016377725U)
									{
										return this.Create_BamlType_InputMethod(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1018952883U)
									{
										return this.Create_BamlType_ColorAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1019262156U)
									{
										return this.Create_BamlType_PathSegmentCollection(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1019849924U)
									{
										return this.Create_BamlType_ThicknessAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1020537735U)
									{
										return this.Create_BamlType_Material(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1021162590U)
									{
										return this.Create_BamlType_Vector3DConverter(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1056330559U)
							{
								if (typeNameHash <= 1043347506U)
								{
									if (typeNameHash == 1029614653U)
									{
										return this.Create_BamlType_Point3DCollectionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1042012617U)
									{
										return this.Create_BamlType_Rectangle(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1043347506U)
									{
										return this.Create_BamlType_BorderGapMaskConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1049460504U)
									{
										return this.Create_BamlType_XmlNamespaceMappingCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1054011130U)
									{
										return this.Create_BamlType_ThemeDictionaryExtension(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1056330559U)
									{
										return this.Create_BamlType_GifBitmapEncoder(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1069777608U)
							{
								if (typeNameHash == 1060097603U)
								{
									return this.Create_BamlType_ColumnDefinitionCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1067429912U)
								{
									return this.Create_BamlType_ObjectDataProvider(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1069777608U)
								{
									return this.Create_BamlType_MouseGesture(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1082938778U)
								{
									return this.Create_BamlType_TableColumn(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1083837605U)
								{
									return this.Create_BamlType_KeyboardNavigation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1083922042U)
								{
									return this.Create_BamlType_PageFunctionBase(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1178626248U)
						{
							if (typeNameHash <= 1117366565U)
							{
								if (typeNameHash <= 1098363926U)
								{
									if (typeNameHash == 1085414201U)
									{
										return this.Create_BamlType_LateBoundBitmapDecoder(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1094052145U)
									{
										return this.Create_BamlType_RectAnimationBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1098363926U)
									{
										return this.Create_BamlType_PngBitmapEncoder(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1104645377U)
									{
										return this.Create_BamlType_ContentElement(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1107478903U)
									{
										return this.Create_BamlType_DecimalConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1117366565U)
									{
										return this.Create_BamlType_PointAnimationUsingPath(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1158811630U)
							{
								if (typeNameHash == 1130648825U)
								{
									return this.Create_BamlType_SplineInt16KeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1150413556U)
								{
									return this.Create_BamlType_WriteableBitmap(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1158811630U)
								{
									return this.Create_BamlType_ListViewItem(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1159768689U)
								{
									return this.Create_BamlType_LinearRectKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1176820406U)
								{
									return this.Create_BamlType_Vector3DAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1178626248U)
								{
									return this.Create_BamlType_InlineUIContainer(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1210906771U)
						{
							if (typeNameHash <= 1186185889U)
							{
								if (typeNameHash == 1183725611U)
								{
									return this.Create_BamlType_ContainerVisual(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1184273902U)
								{
									return this.Create_BamlType_MediaElement(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1186185889U)
								{
									return this.Create_BamlType_MarkupExtension(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1209646082U)
								{
									return this.Create_BamlType_TranslateTransform(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1210722572U)
								{
									return this.Create_BamlType_BaseIListConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1210906771U)
								{
									return this.Create_BamlType_VectorCollection(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1239296217U)
						{
							if (typeNameHash == 1221854500U)
							{
								return this.Create_BamlType_FontStyleConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1227117227U)
							{
								return this.Create_BamlType_FontWeightConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1239296217U)
							{
								return this.Create_BamlType_TextComposition(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 1263136719U)
						{
							if (typeNameHash == 1253725583U)
							{
								return this.Create_BamlType_BulletDecorator(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1263136719U)
							{
								return this.Create_BamlType_DecimalAnimation(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1263268373U)
							{
								return this.Create_BamlType_Model3DGroup(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1283950600U)
							{
								return this.Create_BamlType_ResizeGrip(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1412591399U)
					{
						if (typeNameHash <= 1359074139U)
						{
							if (typeNameHash <= 1318159567U)
							{
								if (typeNameHash <= 1291553535U)
								{
									if (typeNameHash == 1285079965U)
									{
										return this.Create_BamlType_DashStyle(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1285743637U)
									{
										return this.Create_BamlType_StreamGeometryContext(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1291553535U)
									{
										return this.Create_BamlType_SplineInt32KeyFrame(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1305993458U)
									{
										return this.Create_BamlType_TextEffect(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1318104087U)
									{
										return this.Create_BamlType_BooleanAnimationBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1318159567U)
									{
										return this.Create_BamlType_ImageDrawing(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1339394931U)
							{
								if (typeNameHash == 1337691186U)
								{
									return this.Create_BamlType_LinearColorKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1338939476U)
								{
									return this.Create_BamlType_TemplateBindingExtension(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1339394931U)
								{
									return this.Create_BamlType_ToolBar(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1339579355U)
								{
									return this.Create_BamlType_ToolTip(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1347486791U)
								{
									return this.Create_BamlType_ColorKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1359074139U)
								{
									return this.Create_BamlType_Viewport3DVisual(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1395061043U)
						{
							if (typeNameHash <= 1370978769U)
							{
								if (typeNameHash == 1366171760U)
								{
									return this.Create_BamlType_ImageMetadata(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1369509399U)
								{
									return this.Create_BamlType_DialogResultConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1370978769U)
								{
									return this.Create_BamlType_ClockGroup(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1373930089U)
								{
									return this.Create_BamlType_XamlReader(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1392278866U)
								{
									return this.Create_BamlType_Size3DConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1395061043U)
								{
									return this.Create_BamlType_TreeView(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1411264711U)
						{
							if (typeNameHash == 1399972982U)
							{
								return this.Create_BamlType_SingleKeyFrameCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1407254931U)
							{
								return this.Create_BamlType_Inline(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1411264711U)
							{
								return this.Create_BamlType_PointAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1412280093U)
							{
								return this.Create_BamlType_GridSplitter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1412505639U)
							{
								return this.Create_BamlType_CollectionContainer(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1412591399U)
							{
								return this.Create_BamlType_ToolBarTray(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1483217979U)
					{
						if (typeNameHash <= 1441084717U)
						{
							if (typeNameHash <= 1423253394U)
							{
								if (typeNameHash == 1419366049U)
								{
									return this.Create_BamlType_Camera(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1420568068U)
								{
									return this.Create_BamlType_Canvas(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1423253394U)
								{
									return this.Create_BamlType_ResourceDictionary(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1423763428U)
								{
									return this.Create_BamlType_Point3DAnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1433323584U)
								{
									return this.Create_BamlType_TextAlignment(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1441084717U)
								{
									return this.Create_BamlType_GridView(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1452824079U)
						{
							if (typeNameHash == 1451810926U)
							{
								return this.Create_BamlType_ParserContext(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1451899428U)
							{
								return this.Create_BamlType_QuaternionAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1452824079U)
							{
								return this.Create_BamlType_JpegBitmapEncoder(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1453546252U)
							{
								return this.Create_BamlType_TickBar(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1463715626U)
							{
								return this.Create_BamlType_DependencyPropertyConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1483217979U)
							{
								return this.Create_BamlType_XamlVector3DCollectionSerializer(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1514216138U)
					{
						if (typeNameHash <= 1503988241U)
						{
							if (typeNameHash == 1497057972U)
							{
								return this.Create_BamlType_BlockUIContainer(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1503494182U)
							{
								return this.Create_BamlType_Paragraph(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1503988241U)
							{
								return this.Create_BamlType_Storyboard(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1505495632U)
							{
								return this.Create_BamlType_Freezable(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1505896427U)
							{
								return this.Create_BamlType_FlowDocument(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1514216138U)
							{
								return this.Create_BamlType_PropertyPathConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1528777786U)
					{
						if (typeNameHash == 1518131472U)
						{
							return this.Create_BamlType_GeometryDrawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1525454651U)
						{
							return this.Create_BamlType_ZoomPercentageConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1528777786U)
						{
							return this.Create_BamlType_LengthConverter(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 1551028176U)
					{
						if (typeNameHash == 1534031197U)
						{
							return this.Create_BamlType_MatrixTransform(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1551028176U)
						{
							return this.Create_BamlType_DocumentViewerBase(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1553353434U)
						{
							return this.Create_BamlType_GuidelineSet(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1563195901U)
						{
							return this.Create_BamlType_HierarchicalDataTemplate(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1817616839U)
				{
					if (typeNameHash <= 1683116109U)
					{
						if (typeNameHash <= 1638466145U)
						{
							if (typeNameHash <= 1596615863U)
							{
								if (typeNameHash <= 1587772992U)
								{
									if (typeNameHash == 1566189877U)
									{
										return this.Create_BamlType_CornerRadius(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1566963134U)
									{
										return this.Create_BamlType_SplineSizeKeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1587772992U)
									{
										return this.Create_BamlType_Button(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 1587863541U)
									{
										return this.Create_BamlType_JournalEntryListConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1588658228U)
									{
										return this.Create_BamlType_DiscretePoint3DKeyFrame(isBamlType, useV3Rules);
									}
									if (typeNameHash == 1596615863U)
									{
										return this.Create_BamlType_TextElement(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 1630772625U)
							{
								if (typeNameHash == 1599263472U)
								{
									return this.Create_BamlType_KeyTimeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1610838933U)
								{
									return this.Create_BamlType_MediaPlayer(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1630772625U)
								{
									return this.Create_BamlType_FixedPage(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1636299558U)
								{
									return this.Create_BamlType_BeginStoryboard(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1636350275U)
								{
									return this.Create_BamlType_VectorKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1638466145U)
								{
									return this.Create_BamlType_JournalEntry(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1665515158U)
						{
							if (typeNameHash <= 1648736402U)
							{
								if (typeNameHash == 1641446656U)
								{
									return this.Create_BamlType_AffineTransform3D(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1648168330U)
								{
									return this.Create_BamlType_SpotLight(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1648736402U)
								{
									return this.Create_BamlType_DiscreteVectorKeyFrame(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1649262223U)
								{
									return this.Create_BamlType_Condition(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1661775612U)
								{
									return this.Create_BamlType_TransformConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1665515158U)
								{
									return this.Create_BamlType_Animatable(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1673388557U)
						{
							if (typeNameHash == 1667234335U)
							{
								return this.Create_BamlType_Glyphs(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1669447028U)
							{
								return this.Create_BamlType_ByteConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1673388557U)
							{
								return this.Create_BamlType_DiscreteQuaternionKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1676692392U)
							{
								return this.Create_BamlType_GradientStopCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1682538720U)
							{
								return this.Create_BamlType_MediaClock(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1683116109U)
							{
								return this.Create_BamlType_QuaternionRotation3D(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1741197127U)
					{
						if (typeNameHash <= 1714171663U)
						{
							if (typeNameHash <= 1698047614U)
							{
								if (typeNameHash == 1684223221U)
								{
									return this.Create_BamlType_Rotation3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1689931813U)
								{
									return this.Create_BamlType_Int16AnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1698047614U)
								{
									return this.Create_BamlType_KeyboardNavigationMode(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1700491611U)
								{
									return this.Create_BamlType_CompositionTarget(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1709260677U)
								{
									return this.Create_BamlType_Section(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1714171663U)
								{
									return this.Create_BamlType_FrameworkPropertyMetadataOptions(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1727243753U)
						{
							if (typeNameHash == 1720156579U)
							{
								return this.Create_BamlType_TriggerBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1726725401U)
							{
								return this.Create_BamlType_Separator(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1727243753U)
							{
								return this.Create_BamlType_XmlLanguage(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1730845471U)
							{
								return this.Create_BamlType_NameScope(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1737370437U)
							{
								return this.Create_BamlType_MouseDevice(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1741197127U)
							{
								return this.Create_BamlType_NullableConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1798811252U)
					{
						if (typeNameHash <= 1774798759U)
						{
							if (typeNameHash == 1749703332U)
							{
								return this.Create_BamlType_Point3DAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1754018176U)
							{
								return this.Create_BamlType_LineGeometry(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1774798759U)
							{
								return this.Create_BamlType_Transform3DCollection(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1784618733U)
							{
								return this.Create_BamlType_PathGeometry(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1792077897U)
							{
								return this.Create_BamlType_StaticResourceExtension(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1798811252U)
							{
								return this.Create_BamlType_Int32Collection(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1811071644U)
					{
						if (typeNameHash == 1799179879U)
						{
							return this.Create_BamlType_FrameworkContentElement(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1810393776U)
						{
							return this.Create_BamlType_XmlLangPropertyAttribute(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1811071644U)
						{
							return this.Create_BamlType_PageContentCollection(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 1813359201U)
					{
						if (typeNameHash == 1811729200U)
						{
							return this.Create_BamlType_BooleanKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1813359201U)
						{
							return this.Create_BamlType_Rect3DConverter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1815264388U)
						{
							return this.Create_BamlType_ThicknessKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1817616839U)
						{
							return this.Create_BamlType_RadioButton(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1949943781U)
				{
					if (typeNameHash <= 1872596098U)
					{
						if (typeNameHash <= 1844348898U)
						{
							if (typeNameHash <= 1838328148U)
							{
								if (typeNameHash == 1825104844U)
								{
									return this.Create_BamlType_ByteAnimation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1829145558U)
								{
									return this.Create_BamlType_LinearSizeKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1838328148U)
								{
									return this.Create_BamlType_TextCompositionManager(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 1838910454U)
								{
									return this.Create_BamlType_LinearDoubleKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1841269873U)
								{
									return this.Create_BamlType_LinearInt16KeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 1844348898U)
								{
									return this.Create_BamlType_RotateTransform3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 1851065478U)
						{
							if (typeNameHash == 1847171633U)
							{
								return this.Create_BamlType_RoutedEvent(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1847800773U)
							{
								return this.Create_BamlType_RepeatBehaviorConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1851065478U)
							{
								return this.Create_BamlType_Int16KeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1857902570U)
							{
								return this.Create_BamlType_DiscreteColorKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1867638374U)
							{
								return this.Create_BamlType_LinearDecimalKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1872596098U)
							{
								return this.Create_BamlType_GroupBox(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1908047602U)
					{
						if (typeNameHash <= 1894131576U)
						{
							if (typeNameHash == 1886012771U)
							{
								return this.Create_BamlType_SByteConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1888712354U)
							{
								return this.Create_BamlType_SplineVectorKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1894131576U)
							{
								return this.Create_BamlType_ToolTipService(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1899232249U)
							{
								return this.Create_BamlType_DockPanel(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1899479598U)
							{
								return this.Create_BamlType_GeneralTransformCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1908047602U)
							{
								return this.Create_BamlType_InputScope(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1921765486U)
					{
						if (typeNameHash == 1908918452U)
						{
							return this.Create_BamlType_Int32CollectionConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1912422369U)
						{
							return this.Create_BamlType_LineBreak(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1921765486U)
						{
							return this.Create_BamlType_HostVisual(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1930264739U)
						{
							return this.Create_BamlType_ObjectAnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1941591540U)
						{
							return this.Create_BamlType_ListBoxItem(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1949943781U)
						{
							return this.Create_BamlType_Point3DConverter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 1992540733U)
				{
					if (typeNameHash <= 1977826323U)
					{
						if (typeNameHash <= 1961606018U)
						{
							if (typeNameHash == 1950874384U)
							{
								return this.Create_BamlType_Expression(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1952565839U)
							{
								return this.Create_BamlType_BitmapDecoder(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1961606018U)
							{
								return this.Create_BamlType_SingleAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 1972001235U)
							{
								return this.Create_BamlType_PathFigureCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1974320711U)
							{
								return this.Create_BamlType_Rotation3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 1977826323U)
							{
								return this.Create_BamlType_InputScopeNameConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 1982598063U)
					{
						if (typeNameHash == 1978946399U)
						{
							return this.Create_BamlType_PauseStoryboard(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1981708784U)
						{
							return this.Create_BamlType_MatrixAnimationBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1982598063U)
						{
							return this.Create_BamlType_Adorner(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 1983189101U)
						{
							return this.Create_BamlType_QuaternionAnimationBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1987454919U)
						{
							return this.Create_BamlType_SplineThicknessKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 1992540733U)
						{
							return this.Create_BamlType_Stretch(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 2020314122U)
				{
					if (typeNameHash <= 2009851621U)
					{
						if (typeNameHash == 2001481592U)
						{
							return this.Create_BamlType_Window(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2002174583U)
						{
							return this.Create_BamlType_LinearInt32KeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2009851621U)
						{
							return this.Create_BamlType_MatrixKeyFrame(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 2011970188U)
						{
							return this.Create_BamlType_Int32KeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2012322180U)
						{
							return this.Create_BamlType_PasswordBox(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2020314122U)
						{
							return this.Create_BamlType_Italic(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 2022237748U)
				{
					if (typeNameHash == 2020350540U)
					{
						return this.Create_BamlType_GeometryModel3D(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2021668905U)
					{
						return this.Create_BamlType_PointLightBase(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2022237748U)
					{
						return this.Create_BamlType_DiscreteMatrixKeyFrame(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 2042108315U)
				{
					if (typeNameHash == 2026683522U)
					{
						return this.Create_BamlType_TransformedBitmap(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2042108315U)
					{
						return this.Create_BamlType_ColumnDefinition(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 2043908275U)
					{
						return this.Create_BamlType_PenLineCap(isBamlType, useV3Rules);
					}
					if (typeNameHash == 2045195350U)
					{
						return this.Create_BamlType_StickyNoteControl(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 3150804609U)
			{
				if (typeNameHash <= 2625820903U)
				{
					if (typeNameHash <= 2368443675U)
					{
						if (typeNameHash <= 2242877643U)
						{
							if (typeNameHash <= 2147133521U)
							{
								if (typeNameHash <= 2095403106U)
								{
									if (typeNameHash <= 2086386488U)
									{
										if (typeNameHash == 2057591265U)
										{
											return this.Create_BamlType_ColorConvertedBitmapExtension(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2082963390U)
										{
											return this.Create_BamlType_CharKeyFrameCollection(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2086386488U)
										{
											return this.Create_BamlType_MatrixTransform3D(isBamlType, useV3Rules);
										}
									}
									else
									{
										if (typeNameHash == 2090698417U)
										{
											return this.Create_BamlType_ResourceKey(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2090772835U)
										{
											return this.Create_BamlType_CharIListConverter(isBamlType, useV3Rules);
										}
										if (typeNameHash == 2095403106U)
										{
											return this.Create_BamlType_RectKeyFrame(isBamlType, useV3Rules);
										}
									}
								}
								else if (typeNameHash <= 2118568062U)
								{
									if (typeNameHash == 2105601597U)
									{
										return this.Create_BamlType_Point3DAnimation(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2116926181U)
									{
										return this.Create_BamlType_ListBox(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2118568062U)
									{
										return this.Create_BamlType_NumberSubstitution(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2134177976U)
									{
										return this.Create_BamlType_DrawingBrush(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2145171279U)
									{
										return this.Create_BamlType_InputLanguageManager(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2147133521U)
									{
										return this.Create_BamlType_RepeatBehavior(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2213541446U)
							{
								if (typeNameHash <= 2187607252U)
								{
									if (typeNameHash == 2175789292U)
									{
										return this.Create_BamlType_Trigger(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2181752774U)
									{
										return this.Create_BamlType_Hyperlink(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2187607252U)
									{
										return this.Create_BamlType_ContentControl(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2194377413U)
									{
										return this.Create_BamlType_TimeSpan(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2203399086U)
									{
										return this.Create_BamlType_SetterBase(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2213541446U)
									{
										return this.Create_BamlType_FrameworkTemplate(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2233723591U)
							{
								if (typeNameHash == 2220064835U)
								{
									return this.Create_BamlType_Timeline(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2224116138U)
								{
									return this.Create_BamlType_InputScopeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2233723591U)
								{
									return this.Create_BamlType_DoubleKeyFrameCollection(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2239188145U)
								{
									return this.Create_BamlType_ControlTemplate(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2242193787U)
								{
									return this.Create_BamlType_StringKeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2242877643U)
								{
									return this.Create_BamlType_GestureRecognizer(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2316123992U)
						{
							if (typeNameHash <= 2286541048U)
							{
								if (typeNameHash <= 2270334750U)
								{
									if (typeNameHash == 2245298560U)
									{
										return this.Create_BamlType_KeyBinding(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2247557147U)
									{
										return this.Create_BamlType_ContentPresenter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2270334750U)
									{
										return this.Create_BamlType_XmlDataProvider(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2275985883U)
									{
										return this.Create_BamlType_SizeConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2283191896U)
									{
										return this.Create_BamlType_TableRow(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2286541048U)
									{
										return this.Create_BamlType_Boolean(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2303212540U)
							{
								if (typeNameHash == 2291279447U)
								{
									return this.Create_BamlType_ItemsControl(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2293688015U)
								{
									return this.Create_BamlType_PolyLineSegment(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2303212540U)
								{
									return this.Create_BamlType_Transform3DGroup(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2305986747U)
								{
									return this.Create_BamlType_IComponentConnector(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2309433103U)
								{
									return this.Create_BamlType_TextSearch(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2316123992U)
								{
									return this.Create_BamlType_DrawingCollection(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2359651825U)
						{
							if (typeNameHash <= 2341092446U)
							{
								if (typeNameHash == 2325564233U)
								{
									return this.Create_BamlType_DrawingContext(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2338158216U)
								{
									return this.Create_BamlType_JournalEntryUnifiedViewConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2341092446U)
								{
									return this.Create_BamlType_BitmapMetadata(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2357823000U)
								{
									return this.Create_BamlType_MenuBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2357963071U)
								{
									return this.Create_BamlType_ListCollectionView(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2359651825U)
								{
									return this.Create_BamlType_Point3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2361798307U)
						{
							if (typeNameHash == 2359651926U)
							{
								return this.Create_BamlType_Point4D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2361481257U)
							{
								return this.Create_BamlType_DiscreteInt16KeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2361798307U)
							{
								return this.Create_BamlType_Int32AnimationBase(isBamlType, useV3Rules);
							}
						}
						else if (typeNameHash <= 2365227520U)
						{
							if (typeNameHash == 2364198568U)
							{
								return this.Create_BamlType_Matrix3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2365227520U)
							{
								return this.Create_BamlType_MenuItem(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2366255821U)
							{
								return this.Create_BamlType_Vector3DAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2368443675U)
							{
								return this.Create_BamlType_DecimalKeyFrameCollection(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2510108609U)
					{
						if (typeNameHash <= 2414694242U)
						{
							if (typeNameHash <= 2387462803U)
							{
								if (typeNameHash <= 2375952462U)
								{
									if (typeNameHash == 2371777274U)
									{
										return this.Create_BamlType_InkPresenter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2372223669U)
									{
										return this.Create_BamlType_Int64KeyFrameCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2375952462U)
									{
										return this.Create_BamlType_CursorConverter(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2382404033U)
									{
										return this.Create_BamlType_RectangleGeometry(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2384031129U)
									{
										return this.Create_BamlType_RowDefinition(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2387462803U)
									{
										return this.Create_BamlType_StringKeyFrame(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2399848930U)
							{
								if (typeNameHash == 2395439922U)
								{
									return this.Create_BamlType_Rotation3DAnimationBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2397363533U)
								{
									return this.Create_BamlType_DiffuseMaterial(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2399848930U)
								{
									return this.Create_BamlType_DiscreteStringKeyFrame(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2401541526U)
								{
									return this.Create_BamlType_ViewBase(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2405666083U)
								{
									return this.Create_BamlType_AnchoredBlock(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2414694242U)
								{
									return this.Create_BamlType_ProjectionCamera(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2480012208U)
						{
							if (typeNameHash <= 2431643699U)
							{
								if (typeNameHash == 2419799105U)
								{
									return this.Create_BamlType_EventSetter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2431400980U)
								{
									return this.Create_BamlType_ContentWrapperAttribute(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2431643699U)
								{
									return this.Create_BamlType_SizeAnimation(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2439005345U)
								{
									return this.Create_BamlType_SizeAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2464017094U)
								{
									return this.Create_BamlType_FontSizeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2480012208U)
								{
									return this.Create_BamlType_BooleanToVisibilityConverter(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2500030087U)
						{
							if (typeNameHash == 2495537998U)
							{
								return this.Create_BamlType_Ellipse(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2496400610U)
							{
								return this.Create_BamlType_DataTemplate(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2500030087U)
							{
								return this.Create_BamlType_OrthographicCamera(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2500854951U)
							{
								return this.Create_BamlType_Setter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2507216059U)
							{
								return this.Create_BamlType_Geometry3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2510108609U)
							{
								return this.Create_BamlType_Point3DKeyFrameCollection(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2564710999U)
					{
						if (typeNameHash <= 2537793262U)
						{
							if (typeNameHash <= 2522385967U)
							{
								if (typeNameHash == 2510136870U)
								{
									return this.Create_BamlType_DoubleAnimationUsingPath(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2518729620U)
								{
									return this.Create_BamlType_KeySpline(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2522385967U)
								{
									return this.Create_BamlType_DiscreteInt32KeyFrame(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2523498610U)
								{
									return this.Create_BamlType_UriTypeConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2526122409U)
								{
									return this.Create_BamlType_KeyConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2537793262U)
								{
									return this.Create_BamlType_BitmapSource(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2549694123U)
						{
							if (typeNameHash == 2540000939U)
							{
								return this.Create_BamlType_VectorKeyFrameCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2546467590U)
							{
								return this.Create_BamlType_AdornerDecorator(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2549694123U)
							{
								return this.Create_BamlType_GridViewColumn(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2563020253U)
							{
								return this.Create_BamlType_GuidConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2563899293U)
							{
								return this.Create_BamlType_StaticExtension(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2564710999U)
							{
								return this.Create_BamlType_StopStoryboard(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2590675289U)
					{
						if (typeNameHash <= 2579083438U)
						{
							if (typeNameHash == 2568847263U)
							{
								return this.Create_BamlType_CheckBox(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2573940685U)
							{
								return this.Create_BamlType_CachedBitmap(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2579083438U)
							{
								return this.Create_BamlType_EventTrigger(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2586667908U)
							{
								return this.Create_BamlType_MaterialGroup(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2588718206U)
							{
								return this.Create_BamlType_BindingExpressionBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2590675289U)
							{
								return this.Create_BamlType_StatusBar(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2603137612U)
					{
						if (typeNameHash == 2594318825U)
						{
							return this.Create_BamlType_EnumConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2599258965U)
						{
							return this.Create_BamlType_DateTimeConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2603137612U)
						{
							return this.Create_BamlType_ComponentResourceKey(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 2616916250U)
					{
						if (typeNameHash == 2604679664U)
						{
							return this.Create_BamlType_FigureLengthConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2616916250U)
						{
							return this.Create_BamlType_CroppedBitmap(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 2622762262U)
						{
							return this.Create_BamlType_Int16KeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2625820903U)
						{
							return this.Create_BamlType_ItemCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 2880449407U)
				{
					if (typeNameHash <= 2750913568U)
					{
						if (typeNameHash <= 2697498068U)
						{
							if (typeNameHash <= 2677748290U)
							{
								if (typeNameHash <= 2644858326U)
								{
									if (typeNameHash == 2630693784U)
									{
										return this.Create_BamlType_ComboBoxItem(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2632968446U)
									{
										return this.Create_BamlType_SetterBaseCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2644858326U)
									{
										return this.Create_BamlType_SolidColorBrush(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 2654418985U)
									{
										return this.Create_BamlType_DrawingGroup(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2667804183U)
									{
										return this.Create_BamlType_FrameworkTextComposition(isBamlType, useV3Rules);
									}
									if (typeNameHash == 2677748290U)
									{
										return this.Create_BamlType_XmlNamespaceMapping(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 2687716458U)
							{
								if (typeNameHash == 2683039828U)
								{
									return this.Create_BamlType_Polygon(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2685434095U)
								{
									return this.Create_BamlType_Block(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2687716458U)
								{
									return this.Create_BamlType_PolyQuadraticBezierSegment(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2691678720U)
								{
									return this.Create_BamlType_Brush(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2695798729U)
								{
									return this.Create_BamlType_DiscreteRectKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2697498068U)
								{
									return this.Create_BamlType_StreamGeometry(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2717418325U)
						{
							if (typeNameHash <= 2707718720U)
							{
								if (typeNameHash == 2697933609U)
								{
									return this.Create_BamlType_SplinePointKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2704854826U)
								{
									return this.Create_BamlType_MultiBindingExpression(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2707718720U)
								{
									return this.Create_BamlType_AdornerLayer(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2712654300U)
								{
									return this.Create_BamlType_KeyGesture(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2714912374U)
								{
									return this.Create_BamlType_ColorConvertedBitmap(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2717418325U)
								{
									return this.Create_BamlType_BitmapEncoder(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2737207145U)
						{
							if (typeNameHash == 2723992168U)
							{
								return this.Create_BamlType_ScrollBar(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2727123374U)
							{
								return this.Create_BamlType_SplineDecimalKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2737207145U)
							{
								return this.Create_BamlType_GeometryGroup(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2741821828U)
							{
								return this.Create_BamlType_DependencyProperty(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2745869284U)
							{
								return this.Create_BamlType_TabletDevice(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2750913568U)
							{
								return this.Create_BamlType_TabControl(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2829278862U)
					{
						if (typeNameHash <= 2789494496U)
						{
							if (typeNameHash <= 2770480768U)
							{
								if (typeNameHash == 2752355982U)
								{
									return this.Create_BamlType_Vector3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2764779975U)
								{
									return this.Create_BamlType_SizeKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2770480768U)
								{
									return this.Create_BamlType_FontStretchConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2775858686U)
								{
									return this.Create_BamlType_DiscreteRotation3DKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2779938702U)
								{
									return this.Create_BamlType_ByteAnimationUsingKeyFrames(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2789494496U)
								{
									return this.Create_BamlType_Clock(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2823948821U)
						{
							if (typeNameHash == 2792556015U)
							{
								return this.Create_BamlType_Color(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2821657175U)
							{
								return this.Create_BamlType_StringConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2823948821U)
							{
								return this.Create_BamlType_PointAnimationBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2825488812U)
							{
								return this.Create_BamlType_CollectionViewSource(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2828266559U)
							{
								return this.Create_BamlType_DoubleKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2829278862U)
							{
								return this.Create_BamlType_BmpBitmapDecoder(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2854085569U)
					{
						if (typeNameHash <= 2836690659U)
						{
							if (typeNameHash == 2830133971U)
							{
								return this.Create_BamlType_InputBindingCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2833582507U)
							{
								return this.Create_BamlType_PathFigure(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2836690659U)
							{
								return this.Create_BamlType_SplineByteKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2840652686U)
							{
								return this.Create_BamlType_DiscreteDoubleKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2853214736U)
							{
								return this.Create_BamlType_NavigationWindow(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2854085569U)
							{
								return this.Create_BamlType_Control(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 2857244043U)
					{
						if (typeNameHash == 2855103477U)
						{
							return this.Create_BamlType_LinearQuaternionKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2856553785U)
						{
							return this.Create_BamlType_GlyphRunDrawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2857244043U)
						{
							return this.Create_BamlType_DrawingImage(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 2867809997U)
					{
						if (typeNameHash == 2865322288U)
						{
							return this.Create_BamlType_CultureInfoConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2867809997U)
						{
							return this.Create_BamlType_QuaternionKeyFrameCollection(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 2872636339U)
						{
							return this.Create_BamlType_RemoveStoryboard(isBamlType, useV3Rules);
						}
						if (typeNameHash == 2880449407U)
						{
							return this.Create_BamlType_DataTemplateKey(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3040108565U)
				{
					if (typeNameHash <= 2990868428U)
					{
						if (typeNameHash <= 2923120250U)
						{
							if (typeNameHash <= 2892711692U)
							{
								if (typeNameHash == 2884063696U)
								{
									return this.Create_BamlType_FontStretch(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2884746986U)
								{
									return this.Create_BamlType_WrapPanel(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2892711692U)
								{
									return this.Create_BamlType_TiffBitmapDecoder(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 2906462199U)
								{
									return this.Create_BamlType_DiscreteThicknessKeyFrame(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2910782830U)
								{
									return this.Create_BamlType_Single(isBamlType, useV3Rules);
								}
								if (typeNameHash == 2923120250U)
								{
									return this.Create_BamlType_Size3D(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 2958853687U)
						{
							if (typeNameHash == 2953557280U)
							{
								return this.Create_BamlType_TableCell(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2954759040U)
							{
								return this.Create_BamlType_KeyTime(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2958853687U)
							{
								return this.Create_BamlType_ObjectKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 2971239814U)
							{
								return this.Create_BamlType_DiscreteObjectKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2979874461U)
							{
								return this.Create_BamlType_LinearSingleKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2990868428U)
							{
								return this.Create_BamlType_IconBitmapDecoder(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3008357171U)
					{
						if (typeNameHash <= 3000815496U)
						{
							if (typeNameHash == 2992435596U)
							{
								return this.Create_BamlType_Orientation(isBamlType, useV3Rules);
							}
							if (typeNameHash == 2997528560U)
							{
								return this.Create_BamlType_PathFigureCollectionConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3000815496U)
							{
								return this.Create_BamlType_Viewbox(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3005129221U)
							{
								return this.Create_BamlType_SingleAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3005265189U)
							{
								return this.Create_BamlType_TextBoxBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3008357171U)
							{
								return this.Create_BamlType_DecimalKeyFrame(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3024874406U)
					{
						if (typeNameHash == 3016322347U)
						{
							return this.Create_BamlType_DoubleAnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3017582335U)
						{
							return this.Create_BamlType_ObjectKeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3024874406U)
						{
							return this.Create_BamlType_VectorAnimationBase(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3031820371U)
						{
							return this.Create_BamlType_VideoDrawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3035410420U)
						{
							return this.Create_BamlType_TypeTypeConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3040108565U)
						{
							return this.Create_BamlType_HeaderedItemsControl(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3098873083U)
				{
					if (typeNameHash <= 3062728027U)
					{
						if (typeNameHash <= 3055664686U)
						{
							if (typeNameHash == 3040714245U)
							{
								return this.Create_BamlType_BoolIListConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3042527602U)
							{
								return this.Create_BamlType_ResumeStoryboard(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3055664686U)
							{
								return this.Create_BamlType_SkewTransform(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3056264338U)
							{
								return this.Create_BamlType_GridViewHeaderRowPresenter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3061725932U)
							{
								return this.Create_BamlType_FrameworkElement(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3062728027U)
							{
								return this.Create_BamlType_DiscreteBooleanKeyFrame(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3080628638U)
					{
						if (typeNameHash == 3077777987U)
						{
							return this.Create_BamlType_MediaTimeline(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3078955044U)
						{
							return this.Create_BamlType_FontStyle(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3080628638U)
						{
							return this.Create_BamlType_SplineDoubleKeyFrame(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3087488479U)
						{
							return this.Create_BamlType_Object(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3091177486U)
						{
							return this.Create_BamlType_PointCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3098873083U)
						{
							return this.Create_BamlType_LinearByteKeyFrame(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3119883437U)
				{
					if (typeNameHash <= 3109717207U)
					{
						if (typeNameHash == 3100133790U)
						{
							return this.Create_BamlType_Rotation3DKeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3107715695U)
						{
							return this.Create_BamlType_Frame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3109717207U)
						{
							return this.Create_BamlType_Int16AnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3114475758U)
						{
							return this.Create_BamlType_FlowDocumentPageViewer(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3114500727U)
						{
							return this.Create_BamlType_BindingExpression(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3119883437U)
						{
							return this.Create_BamlType_XamlPointCollectionSerializer(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3131559342U)
				{
					if (typeNameHash == 3120891186U)
					{
						return this.Create_BamlType_DurationConverter(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3124512808U)
					{
						return this.Create_BamlType_StreamResourceInfo(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3131559342U)
					{
						return this.Create_BamlType_QuaternionKeyFrame(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 3133507978U)
				{
					if (typeNameHash == 3131853152U)
					{
						return this.Create_BamlType_XamlBrushSerializer(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3133507978U)
					{
						return this.Create_BamlType_TabItem(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 3134642154U)
					{
						return this.Create_BamlType_SkipStoryboardToFill(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3150804609U)
					{
						return this.Create_BamlType_InPlaceBitmapMetadataWriter(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 3681601765U)
			{
				if (typeNameHash <= 3421308621U)
				{
					if (typeNameHash <= 3319196847U)
					{
						if (typeNameHash <= 3225341700U)
						{
							if (typeNameHash <= 3184112475U)
							{
								if (typeNameHash <= 3160936067U)
								{
									if (typeNameHash == 3156616619U)
									{
										return this.Create_BamlType_TimelineCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3157049388U)
									{
										return this.Create_BamlType_BitmapPalette(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3160936067U)
									{
										return this.Create_BamlType_ByteAnimationBase(isBamlType, useV3Rules);
									}
								}
								else
								{
									if (typeNameHash == 3168456847U)
									{
										return this.Create_BamlType_ColorKeyFrameCollection(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3179498907U)
									{
										return this.Create_BamlType_DoubleCollectionConverter(isBamlType, useV3Rules);
									}
									if (typeNameHash == 3184112475U)
									{
										return this.Create_BamlType_ThicknessAnimationUsingKeyFrames(isBamlType, useV3Rules);
									}
								}
							}
							else if (typeNameHash <= 3213500826U)
							{
								if (typeNameHash == 3187081615U)
								{
									return this.Create_BamlType_BitmapEffectGroup(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3191294337U)
								{
									return this.Create_BamlType_SetStoryboardSpeedRatio(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3213500826U)
								{
									return this.Create_BamlType_MenuScrollingVisibilityConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3217781231U)
								{
									return this.Create_BamlType_Slider(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3223906597U)
								{
									return this.Create_BamlType_ScrollViewer(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3225341700U)
								{
									return this.Create_BamlType_BitmapFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3253060368U)
						{
							if (typeNameHash <= 3245663526U)
							{
								if (typeNameHash == 3232444943U)
								{
									return this.Create_BamlType_SizeKeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3239409257U)
								{
									return this.Create_BamlType_Point3DCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3245663526U)
								{
									return this.Create_BamlType_ItemsPresenter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3249372079U)
								{
									return this.Create_BamlType_ItemContainerTemplateKey(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3251319004U)
								{
									return this.Create_BamlType_StylusDevice(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3253060368U)
								{
									return this.Create_BamlType_SplineInt64KeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3273980620U)
						{
							if (typeNameHash == 3254666903U)
							{
								return this.Create_BamlType_BlurBitmapEffect(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3269592841U)
							{
								return this.Create_BamlType_TimeSpanConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3273980620U)
							{
								return this.Create_BamlType_ByteKeyFrameCollection(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3277780192U)
							{
								return this.Create_BamlType_StrokeCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3310116788U)
							{
								return this.Create_BamlType_Model3DCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3319196847U)
							{
								return this.Create_BamlType_ControllableStoryboardAction(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3388398850U)
					{
						if (typeNameHash <= 3342933789U)
						{
							if (typeNameHash <= 3335227078U)
							{
								if (typeNameHash == 3326778732U)
								{
									return this.Create_BamlType_Int32KeyFrameCollection(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3332504754U)
								{
									return this.Create_BamlType_DirectionalLight(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3335227078U)
								{
									return this.Create_BamlType_TemplateBindingExpressionConverter(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3337633457U)
								{
									return this.Create_BamlType_MatrixConverter(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3337984719U)
								{
									return this.Create_BamlType_Visual3D(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3342933789U)
								{
									return this.Create_BamlType_SplineQuaternionKeyFrame(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3363408079U)
						{
							if (typeNameHash == 3347030199U)
							{
								return this.Create_BamlType_MultiTrigger(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3361683901U)
							{
								return this.Create_BamlType_XmlLanguageConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3363408079U)
							{
								return this.Create_BamlType_ListItem(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3365175598U)
							{
								return this.Create_BamlType_DiscreteSizeKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3376689791U)
							{
								return this.Create_BamlType_ListView(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3388398850U)
							{
								return this.Create_BamlType_RectConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3408554657U)
					{
						if (typeNameHash <= 3402758583U)
						{
							if (typeNameHash == 3391091418U)
							{
								return this.Create_BamlType_BitmapEffectInput(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3402641106U)
							{
								return this.Create_BamlType_ItemContainerTemplate(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3402758583U)
							{
								return this.Create_BamlType_ButtonBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3402930733U)
							{
								return this.Create_BamlType_CharAnimationBase(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3404714779U)
							{
								return this.Create_BamlType_CommandConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3408554657U)
							{
								return this.Create_BamlType_LinearPointKeyFrame(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3415963604U)
					{
						if (typeNameHash == 3414744787U)
						{
							return this.Create_BamlType_Image(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3415963406U)
						{
							return this.Create_BamlType_Int16(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3415963604U)
						{
							return this.Create_BamlType_Int32(isBamlType, useV3Rules);
						}
					}
					else if (typeNameHash <= 3418350262U)
					{
						if (typeNameHash == 3415963909U)
						{
							return this.Create_BamlType_Int64(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3418350262U)
						{
							return this.Create_BamlType_PointKeyFrame(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3421308423U)
						{
							return this.Create_BamlType_UInt16(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3421308621U)
						{
							return this.Create_BamlType_UInt32(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3564745150U)
				{
					if (typeNameHash <= 3517750932U)
					{
						if (typeNameHash <= 3462744685U)
						{
							if (typeNameHash <= 3440459974U)
							{
								if (typeNameHash == 3421308926U)
								{
									return this.Create_BamlType_UInt64(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3431074367U)
								{
									return this.Create_BamlType_ToolBarOverflowPanel(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3440459974U)
								{
									return this.Create_BamlType_InkCanvas(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3446789391U)
								{
									return this.Create_BamlType_FocusManager(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3448499789U)
								{
									return this.Create_BamlType_Matrix(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3462744685U)
								{
									return this.Create_BamlType_Floater(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3503874477U)
						{
							if (typeNameHash == 3491907900U)
							{
								return this.Create_BamlType_LinearPoint3DKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3493262121U)
							{
								return this.Create_BamlType_DropShadowBitmapEffect(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3503874477U)
							{
								return this.Create_BamlType_LinearVector3DKeyFrame(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3505868102U)
							{
								return this.Create_BamlType_Cursor(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3515909783U)
							{
								return this.Create_BamlType_Viewport3D(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3517750932U)
							{
								return this.Create_BamlType_ParallelTimeline(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3544056666U)
					{
						if (typeNameHash <= 3527208580U)
						{
							if (typeNameHash == 3521445823U)
							{
								return this.Create_BamlType_StringAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3523046863U)
							{
								return this.Create_BamlType_MaterialCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3527208580U)
							{
								return this.Create_BamlType_RenderOptions(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3532370792U)
							{
								return this.Create_BamlType_BitmapImage(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3538186783U)
							{
								return this.Create_BamlType_Binding(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3544056666U)
							{
								return this.Create_BamlType_RectAnimation(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3551608724U)
					{
						if (typeNameHash == 3545069055U)
						{
							return this.Create_BamlType_FontWeight(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3545729620U)
						{
							return this.Create_BamlType_KeySplineConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3551608724U)
						{
							return this.Create_BamlType_Int32Converter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3557950823U)
						{
							return this.Create_BamlType_VisualTarget(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3564716316U)
						{
							return this.Create_BamlType_RangeBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3564745150U)
						{
							return this.Create_BamlType_CharConverter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3626720937U)
				{
					if (typeNameHash <= 3594131348U)
					{
						if (typeNameHash <= 3578060752U)
						{
							if (typeNameHash == 3567044273U)
							{
								return this.Create_BamlType_MatrixAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3574070525U)
							{
								return this.Create_BamlType_RepeatButton(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3578060752U)
							{
								return this.Create_BamlType_RoutedUICommand(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3580418462U)
							{
								return this.Create_BamlType_Point4DConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3581886304U)
							{
								return this.Create_BamlType_PolyBezierSegment(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3594131348U)
							{
								return this.Create_BamlType_BmpBitmapEncoder(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3610917888U)
					{
						if (typeNameHash == 3597588278U)
						{
							return this.Create_BamlType_StringAnimationBase(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3603120821U)
						{
							return this.Create_BamlType_TemplateKey(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3610917888U)
						{
							return this.Create_BamlType_FlowDocumentScrollViewer(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3613077086U)
						{
							return this.Create_BamlType_GeneralTransform(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3626508971U)
						{
							return this.Create_BamlType_InputBinding(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3626720937U)
						{
							return this.Create_BamlType_TableRowGroup(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3656924396U)
				{
					if (typeNameHash <= 3638153921U)
					{
						if (typeNameHash == 3627100744U)
						{
							return this.Create_BamlType_AnimationClock(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3633058264U)
						{
							return this.Create_BamlType_Drawing(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3638153921U)
						{
							return this.Create_BamlType_RelativeSource(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3639115055U)
						{
							return this.Create_BamlType_BooleanAnimationUsingKeyFrames(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3646628024U)
						{
							return this.Create_BamlType_Matrix3DConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3656924396U)
						{
							return this.Create_BamlType_PathSegment(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3661012133U)
				{
					if (typeNameHash == 3657564178U)
					{
						return this.Create_BamlType_TiffBitmapEncoder(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3660631367U)
					{
						return this.Create_BamlType_TabPanel(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3661012133U)
					{
						return this.Create_BamlType_LinearGradientBrush(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 3666411286U)
				{
					if (typeNameHash == 3666191229U)
					{
						return this.Create_BamlType_Selector(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3666411286U)
					{
						return this.Create_BamlType_SingleConverter(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 3670807738U)
					{
						return this.Create_BamlType_GradientBrush(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3681601765U)
					{
						return this.Create_BamlType_GlyphRun(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4029087036U)
			{
				if (typeNameHash <= 3870757565U)
				{
					if (typeNameHash <= 3743141867U)
					{
						if (typeNameHash <= 3714572384U)
						{
							if (typeNameHash <= 3705322145U)
							{
								if (typeNameHash == 3692579028U)
								{
									return this.Create_BamlType_Application(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3693227583U)
								{
									return this.Create_BamlType_WmpBitmapDecoder(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3705322145U)
								{
									return this.Create_BamlType_DateTime(isBamlType, useV3Rules);
								}
							}
							else
							{
								if (typeNameHash == 3707266540U)
								{
									return this.Create_BamlType_Int32Animation(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3711361102U)
								{
									return this.Create_BamlType_Figure(isBamlType, useV3Rules);
								}
								if (typeNameHash == 3714572384U)
								{
									return this.Create_BamlType_Label(isBamlType, useV3Rules);
								}
							}
						}
						else if (typeNameHash <= 3726867806U)
						{
							if (typeNameHash == 3722866108U)
							{
								return this.Create_BamlType_Light(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3725053631U)
							{
								return this.Create_BamlType_EmbossBitmapEffect(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3726867806U)
							{
								return this.Create_BamlType_FlowDocumentReader(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3734175699U)
							{
								return this.Create_BamlType_PixelFormatConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3741909743U)
							{
								return this.Create_BamlType_FixedDocument(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3743141867U)
							{
								return this.Create_BamlType_PointIListConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3800911244U)
					{
						if (typeNameHash <= 3761318360U)
						{
							if (typeNameHash == 3746015879U)
							{
								return this.Create_BamlType_XamlWriter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3746078580U)
							{
								return this.Create_BamlType_RectAnimationUsingKeyFrames(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3761318360U)
							{
								return this.Create_BamlType_ContentPropertyAttribute(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3780531133U)
							{
								return this.Create_BamlType_TransformGroup(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3797319853U)
							{
								return this.Create_BamlType_TextDecorationCollection(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3800911244U)
							{
								return this.Create_BamlType_Vector3DCollectionConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3850431872U)
					{
						if (typeNameHash == 3822069102U)
						{
							return this.Create_BamlType_SingleAnimation(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3831772414U)
						{
							return this.Create_BamlType_Int32Rect(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3850431872U)
						{
							return this.Create_BamlType_ScaleTransform(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3862075430U)
						{
							return this.Create_BamlType_PointConverter(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3870219055U)
						{
							return this.Create_BamlType_LostFocusEventManager(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3870757565U)
						{
							return this.Create_BamlType_MatrixKeyFrameCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3938642252U)
				{
					if (typeNameHash <= 3908606180U)
					{
						if (typeNameHash <= 3895289908U)
						{
							if (typeNameHash == 3894194634U)
							{
								return this.Create_BamlType_IAddChild(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3894580134U)
							{
								return this.Create_BamlType_EllipseGeometry(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3895289908U)
							{
								return this.Create_BamlType_AmbientLight(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 3903386330U)
							{
								return this.Create_BamlType_RelativeSourceMode(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3908473888U)
							{
								return this.Create_BamlType_SoundPlayerAction(isBamlType, useV3Rules);
							}
							if (typeNameHash == 3908606180U)
							{
								return this.Create_BamlType_DrawingVisual(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 3925684252U)
					{
						if (typeNameHash == 3921789478U)
						{
							return this.Create_BamlType_Vector3DCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3924959263U)
						{
							return this.Create_BamlType_Transform3D(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3925684252U)
						{
							return this.Create_BamlType_Transform(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3927457614U)
						{
							return this.Create_BamlType_QuadraticBezierSegment(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3928766041U)
						{
							return this.Create_BamlType_DiscretePointKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3938642252U)
						{
							return this.Create_BamlType_GridViewColumnHeader(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3965893796U)
				{
					if (typeNameHash <= 3950543104U)
					{
						if (typeNameHash == 3944256480U)
						{
							return this.Create_BamlType_NullExtension(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3948871275U)
						{
							return this.Create_BamlType_Vector(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3950543104U)
						{
							return this.Create_BamlType_ImageSourceConverter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 3957573606U)
						{
							return this.Create_BamlType_LinearRotation3DKeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3963681416U)
						{
							return this.Create_BamlType_LinearInt64KeyFrame(isBamlType, useV3Rules);
						}
						if (typeNameHash == 3965893796U)
						{
							return this.Create_BamlType_DocumentReferenceCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 3977525494U)
				{
					if (typeNameHash == 3969230566U)
					{
						return this.Create_BamlType_SingleKeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3973477021U)
					{
						return this.Create_BamlType_Int64KeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3977525494U)
					{
						return this.Create_BamlType_NavigationUIVisibility(isBamlType, useV3Rules);
					}
				}
				else if (typeNameHash <= 3991721253U)
				{
					if (typeNameHash == 3981616693U)
					{
						return this.Create_BamlType_DiscreteSingleKeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 3991721253U)
					{
						return this.Create_BamlType_RoutedEventConverter(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4017733246U)
					{
						return this.Create_BamlType_PointAnimation(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4029087036U)
					{
						return this.Create_BamlType_VirtualizingPanel(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4145310526U)
			{
				if (typeNameHash <= 4064409625U)
				{
					if (typeNameHash <= 4048739983U)
					{
						if (typeNameHash <= 4035632042U)
						{
							if (typeNameHash == 4029842000U)
							{
								return this.Create_BamlType_ImageSource(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4034507500U)
							{
								return this.Create_BamlType_ByteKeyFrame(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4035632042U)
							{
								return this.Create_BamlType_ObjectAnimationBase(isBamlType, useV3Rules);
							}
						}
						else
						{
							if (typeNameHash == 4039072664U)
							{
								return this.Create_BamlType_XamlTemplateSerializer(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4043614448U)
							{
								return this.Create_BamlType_CultureInfoIetfLanguageTagConverter(isBamlType, useV3Rules);
							}
							if (typeNameHash == 4048739983U)
							{
								return this.Create_BamlType_VectorCollectionConverter(isBamlType, useV3Rules);
							}
						}
					}
					else if (typeNameHash <= 4059589051U)
					{
						if (typeNameHash == 4056415476U)
						{
							return this.Create_BamlType_BezierSegment(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4056944842U)
						{
							return this.Create_BamlType_SpellCheck(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4059589051U)
						{
							return this.Create_BamlType_String(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 4059695088U)
						{
							return this.Create_BamlType_BooleanKeyFrameCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4061092706U)
						{
							return this.Create_BamlType_TreeViewItem(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4064409625U)
						{
							return this.Create_BamlType_FontFamilyConverter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 4095303241U)
				{
					if (typeNameHash <= 4080336864U)
					{
						if (typeNameHash == 4066832480U)
						{
							return this.Create_BamlType_Stylus(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4070164174U)
						{
							return this.Create_BamlType_DependencyObject(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4080336864U)
						{
							return this.Create_BamlType_GridLengthConverter(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 4083954870U)
						{
							return this.Create_BamlType_BitmapEffectCollection(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4093700264U)
						{
							return this.Create_BamlType_GradientStop(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4095303241U)
						{
							return this.Create_BamlType_Int64Converter(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 4130936400U)
				{
					if (typeNameHash == 4099991372U)
					{
						return this.Create_BamlType_INameScope(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4114749745U)
					{
						return this.Create_BamlType_UInt32Converter(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4130936400U)
					{
						return this.Create_BamlType_Panel(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4135277332U)
					{
						return this.Create_BamlType_Model3D(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4138410030U)
					{
						return this.Create_BamlType_VirtualizingStackPanel(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4145310526U)
					{
						return this.Create_BamlType_Point(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4221592645U)
			{
				if (typeNameHash <= 4199195260U)
				{
					if (typeNameHash <= 4147517467U)
					{
						if (typeNameHash == 4145382636U)
						{
							return this.Create_BamlType_Popup(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4147170844U)
						{
							return this.Create_BamlType_MatrixAnimationUsingPath(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4147517467U)
						{
							return this.Create_BamlType_InputManager(isBamlType, useV3Rules);
						}
					}
					else
					{
						if (typeNameHash == 4156353754U)
						{
							return this.Create_BamlType_Duration(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4156625073U)
						{
							return this.Create_BamlType_DataChangedEventManager(isBamlType, useV3Rules);
						}
						if (typeNameHash == 4199195260U)
						{
							return this.Create_BamlType_TransformCollection(isBamlType, useV3Rules);
						}
					}
				}
				else if (typeNameHash <= 4205340967U)
				{
					if (typeNameHash == 4200653599U)
					{
						return this.Create_BamlType_DocumentPageView(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4202675952U)
					{
						return this.Create_BamlType_TimelineGroup(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4205340967U)
					{
						return this.Create_BamlType_PerspectiveCamera(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4212267451U)
					{
						return this.Create_BamlType_AccessText(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4219821822U)
					{
						return this.Create_BamlType_RoutedCommand(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4221592645U)
					{
						return this.Create_BamlType_SplineSingleKeyFrame(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4243618870U)
			{
				if (typeNameHash <= 4233318098U)
				{
					if (typeNameHash == 4227383631U)
					{
						return this.Create_BamlType_ImageBrush(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4232579606U)
					{
						return this.Create_BamlType_Vector3D(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4233318098U)
					{
						return this.Create_BamlType_StackPanel(isBamlType, useV3Rules);
					}
				}
				else
				{
					if (typeNameHash == 4234029471U)
					{
						return this.Create_BamlType_Rotation3DKeyFrame(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4239529341U)
					{
						return this.Create_BamlType_PriorityBinding(isBamlType, useV3Rules);
					}
					if (typeNameHash == 4243618870U)
					{
						return this.Create_BamlType_PointCollectionConverter(isBamlType, useV3Rules);
					}
				}
			}
			else if (typeNameHash <= 4259355998U)
			{
				if (typeNameHash == 4250838544U)
				{
					return this.Create_BamlType_ArrayExtension(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4250961057U)
				{
					return this.Create_BamlType_Int64Animation(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4259355998U)
				{
					return this.Create_BamlType_DiscreteDecimalKeyFrame(isBamlType, useV3Rules);
				}
			}
			else if (typeNameHash <= 4265248728U)
			{
				if (typeNameHash == 4260680252U)
				{
					return this.Create_BamlType_VisualBrush(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4265248728U)
				{
					return this.Create_BamlType_DynamicResourceExtensionConverter(isBamlType, useV3Rules);
				}
			}
			else
			{
				if (typeNameHash == 4268703175U)
				{
					return this.Create_BamlType_VectorConverter(isBamlType, useV3Rules);
				}
				if (typeNameHash == 4291638393U)
				{
					return this.Create_BamlType_WeakEventManager(isBamlType, useV3Rules);
				}
			}
			return null;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x001BFDA0 File Offset: 0x001BEDA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AccessText(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 1, "AccessText", typeof(AccessText), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AccessText());
			wpfKnownType.ContentPropertyName = "Text";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EDA RID: 11994 RVA: 0x001BFE24 File Offset: 0x001BEE24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AdornedElementPlaceholder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 2, "AdornedElementPlaceholder", typeof(AdornedElementPlaceholder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AdornedElementPlaceholder());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x001BFEA8 File Offset: 0x001BEEA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Adorner(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 3, "Adorner", typeof(Adorner), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x001BFEFC File Offset: 0x001BEEFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AdornerDecorator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 4, "AdornerDecorator", typeof(AdornerDecorator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AdornerDecorator());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x001BFF80 File Offset: 0x001BEF80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AdornerLayer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 5, "AdornerLayer", typeof(AdornerLayer), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x001BFFD3 File Offset: 0x001BEFD3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AffineTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 6, "AffineTransform3D", typeof(AffineTransform3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x001BFFF4 File Offset: 0x001BEFF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AmbientLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 7, "AmbientLight", typeof(AmbientLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AmbientLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE0 RID: 12000 RVA: 0x001C0044 File Offset: 0x001BF044
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AnchoredBlock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 8, "AnchoredBlock", typeof(AnchoredBlock), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x001C0097 File Offset: 0x001BF097
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Animatable(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 9, "Animatable", typeof(Animatable), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x001C00B8 File Offset: 0x001BF0B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AnimationClock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 10, "AnimationClock", typeof(AnimationClock), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x001C00D9 File Offset: 0x001BF0D9
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AnimationTimeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 11, "AnimationTimeline", typeof(AnimationTimeline), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x001C0108 File Offset: 0x001BF108
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Application(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 12, "Application", typeof(Application), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Application());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x001C015C File Offset: 0x001BF15C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ArcSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 13, "ArcSegment", typeof(ArcSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ArcSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x001C01B0 File Offset: 0x001BF1B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ArrayExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 14, "ArrayExtension", typeof(ArrayExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ArrayExtension());
			wpfKnownType.ContentPropertyName = "Items";
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(Type));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new ArrayExtension((Type)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x001C0254 File Offset: 0x001BF254
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_AxisAngleRotation3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 15, "AxisAngleRotation3D", typeof(AxisAngleRotation3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new AxisAngleRotation3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x001C02A5 File Offset: 0x001BF2A5
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BaseIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 16, "BaseIListConverter", typeof(BaseIListConverter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x001C02C8 File Offset: 0x001BF2C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BeginStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 17, "BeginStoryboard", typeof(BeginStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BeginStoryboard());
			wpfKnownType.ContentPropertyName = "Storyboard";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x001C0330 File Offset: 0x001BF330
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BevelBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 18, "BevelBitmapEffect", typeof(BevelBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BevelBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x001C0384 File Offset: 0x001BF384
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 19, "BezierSegment", typeof(BezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x001C03D8 File Offset: 0x001BF3D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Binding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 20, "Binding", typeof(Binding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Binding());
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(string));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new Binding((string)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x001C046E File Offset: 0x001BF46E
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 21, "BindingBase", typeof(BindingBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x001C048F File Offset: 0x001BF48F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 22, "BindingExpression", typeof(BindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x001C04C0 File Offset: 0x001BF4C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingExpressionBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 23, "BindingExpressionBase", typeof(BindingExpressionBase), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x001C04F1 File Offset: 0x001BF4F1
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BindingListCollectionView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 24, "BindingListCollectionView", typeof(BindingListCollectionView), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x001C0512 File Offset: 0x001BF512
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 25, "BitmapDecoder", typeof(BitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x001C0533 File Offset: 0x001BF533
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 26, "BitmapEffect", typeof(BitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x001C0554 File Offset: 0x001BF554
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffectCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 27, "BitmapEffectCollection", typeof(BitmapEffectCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapEffectCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x001C05AC File Offset: 0x001BF5AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffectGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 28, "BitmapEffectGroup", typeof(BitmapEffectGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapEffectGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x001C0608 File Offset: 0x001BF608
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEffectInput(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 29, "BitmapEffectInput", typeof(BitmapEffectInput), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapEffectInput());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x001C0659 File Offset: 0x001BF659
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 30, "BitmapEncoder", typeof(BitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x001C067A File Offset: 0x001BF67A
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 31, "BitmapFrame", typeof(BitmapFrame), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x001C06AC File Offset: 0x001BF6AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapImage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 32, "BitmapImage", typeof(BitmapImage), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BitmapImage());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x001C070D File Offset: 0x001BF70D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapMetadata(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 33, "BitmapMetadata", typeof(BitmapMetadata), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x001C072E File Offset: 0x001BF72E
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapPalette(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 34, "BitmapPalette", typeof(BitmapPalette), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x001C074F File Offset: 0x001BF74F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BitmapSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 35, "BitmapSource", typeof(BitmapSource), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x001C0780 File Offset: 0x001BF780
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Block(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 36, "Block", typeof(Block), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x001C07C0 File Offset: 0x001BF7C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BlockUIContainer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 37, "BlockUIContainer", typeof(BlockUIContainer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BlockUIContainer());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x001C083C File Offset: 0x001BF83C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BlurBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 38, "BlurBitmapEffect", typeof(BlurBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BlurBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x001C088D File Offset: 0x001BF88D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BmpBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 39, "BmpBitmapDecoder", typeof(BmpBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F00 RID: 12032 RVA: 0x001C08B0 File Offset: 0x001BF8B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BmpBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 40, "BmpBitmapEncoder", typeof(BmpBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BmpBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x001C0904 File Offset: 0x001BF904
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Bold(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 41, "Bold", typeof(Bold), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Bold());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x001C0980 File Offset: 0x001BF980
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BoolIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 42, "BoolIListConverter", typeof(BoolIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BoolIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x001C09D4 File Offset: 0x001BF9D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Boolean(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 43, "Boolean", typeof(bool), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => false);
			wpfKnownType.TypeConverterType = typeof(BooleanConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x001C0A35 File Offset: 0x001BFA35
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 44, "BooleanAnimationBase", typeof(BooleanAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x001C0A64 File Offset: 0x001BFA64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 45, "BooleanAnimationUsingKeyFrames", typeof(BooleanAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x001C0ACC File Offset: 0x001BFACC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 46, "BooleanConverter", typeof(BooleanConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x001C0B1D File Offset: 0x001BFB1D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 47, "BooleanKeyFrame", typeof(BooleanKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x001C0B40 File Offset: 0x001BFB40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 48, "BooleanKeyFrameCollection", typeof(BooleanKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x001C0B98 File Offset: 0x001BFB98
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BooleanToVisibilityConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 49, "BooleanToVisibilityConverter", typeof(BooleanToVisibilityConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BooleanToVisibilityConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x001C0BEC File Offset: 0x001BFBEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Border(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 50, "Border", typeof(Border), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Border());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x001C0C70 File Offset: 0x001BFC70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BorderGapMaskConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 51, "BorderGapMaskConverter", typeof(BorderGapMaskConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BorderGapMaskConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x001C0CC1 File Offset: 0x001BFCC1
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Brush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 52, "Brush", typeof(Brush), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x001C0CF4 File Offset: 0x001BFCF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BrushConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 53, "BrushConverter", typeof(BrushConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BrushConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x001C0D48 File Offset: 0x001BFD48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_BulletDecorator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 54, "BulletDecorator", typeof(BulletDecorator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new BulletDecorator());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x001C0DCC File Offset: 0x001BFDCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Button(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 55, "Button", typeof(Button), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Button());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x001C0E50 File Offset: 0x001BFE50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ButtonBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 56, "ButtonBase", typeof(ButtonBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x001C0EB0 File Offset: 0x001BFEB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Byte(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 57, "Byte", typeof(byte), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(ByteConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x001C0F14 File Offset: 0x001BFF14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 58, "ByteAnimation", typeof(ByteAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x001C0F70 File Offset: 0x001BFF70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 59, "ByteAnimationBase", typeof(ByteAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x001C0F9C File Offset: 0x001BFF9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 60, "ByteAnimationUsingKeyFrames", typeof(ByteAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x001C1004 File Offset: 0x001C0004
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 61, "ByteConverter", typeof(ByteConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x001C1055 File Offset: 0x001C0055
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 62, "ByteKeyFrame", typeof(ByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x001C1078 File Offset: 0x001C0078
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ByteKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 63, "ByteKeyFrameCollection", typeof(ByteKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ByteKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x001C10D0 File Offset: 0x001C00D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CachedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 64, "CachedBitmap", typeof(CachedBitmap), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x001C1101 File Offset: 0x001C0101
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Camera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 65, "Camera", typeof(Camera), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x001C1124 File Offset: 0x001C0124
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Canvas(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 66, "Canvas", typeof(Canvas), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Canvas());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x001C11A8 File Offset: 0x001C01A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Char(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 67, "Char", typeof(char), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => '\0');
			wpfKnownType.TypeConverterType = typeof(CharConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x001C1209 File Offset: 0x001C0209
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 68, "CharAnimationBase", typeof(CharAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x001C1238 File Offset: 0x001C0238
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 69, "CharAnimationUsingKeyFrames", typeof(CharAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x001C12A0 File Offset: 0x001C02A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 70, "CharConverter", typeof(CharConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x001C12F4 File Offset: 0x001C02F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 71, "CharIListConverter", typeof(CharIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x001C1345 File Offset: 0x001C0345
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 72, "CharKeyFrame", typeof(CharKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x001C1368 File Offset: 0x001C0368
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CharKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 73, "CharKeyFrameCollection", typeof(CharKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CharKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x001C13C0 File Offset: 0x001C03C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CheckBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 74, "CheckBox", typeof(CheckBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CheckBox());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x001C1444 File Offset: 0x001C0444
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Clock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 75, "Clock", typeof(Clock), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x001C1465 File Offset: 0x001C0465
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ClockController(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 76, "ClockController", typeof(ClockController), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x001C1486 File Offset: 0x001C0486
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ClockGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 77, "ClockGroup", typeof(ClockGroup), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x001C14A8 File Offset: 0x001C04A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CollectionContainer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 78, "CollectionContainer", typeof(CollectionContainer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CollectionContainer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x001C14F9 File Offset: 0x001C04F9
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CollectionView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 79, "CollectionView", typeof(CollectionView), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x001C151C File Offset: 0x001C051C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CollectionViewSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 80, "CollectionViewSource", typeof(CollectionViewSource), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CollectionViewSource());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x001C1570 File Offset: 0x001C0570
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Color(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 81, "Color", typeof(Color), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Color));
			wpfKnownType.TypeConverterType = typeof(ColorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x001C15D4 File Offset: 0x001C05D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 82, "ColorAnimation", typeof(ColorAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x001C1630 File Offset: 0x001C0630
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 83, "ColorAnimationBase", typeof(ColorAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x001C165C File Offset: 0x001C065C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 84, "ColorAnimationUsingKeyFrames", typeof(ColorAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x001C16C4 File Offset: 0x001C06C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorConvertedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 85, "ColorConvertedBitmap", typeof(ColorConvertedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorConvertedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x001C1728 File Offset: 0x001C0728
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorConvertedBitmapExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 86, "ColorConvertedBitmapExtension", typeof(ColorConvertedBitmapExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorConvertedBitmapExtension());
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(object));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new ColorConvertedBitmapExtension(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x001C17C0 File Offset: 0x001C07C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 87, "ColorConverter", typeof(ColorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x001C1811 File Offset: 0x001C0811
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 88, "ColorKeyFrame", typeof(ColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x001C1834 File Offset: 0x001C0834
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColorKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 89, "ColorKeyFrameCollection", typeof(ColorKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColorKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x001C188C File Offset: 0x001C088C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColumnDefinition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 90, "ColumnDefinition", typeof(ColumnDefinition), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ColumnDefinition());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x001C18FC File Offset: 0x001C08FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CombinedGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 91, "CombinedGeometry", typeof(CombinedGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CombinedGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x001C1960 File Offset: 0x001C0960
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComboBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 92, "ComboBox", typeof(ComboBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComboBox());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x001C19E4 File Offset: 0x001C09E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComboBoxItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 93, "ComboBoxItem", typeof(ComboBoxItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComboBoxItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x001C1A68 File Offset: 0x001C0A68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CommandConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 94, "CommandConverter", typeof(CommandConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CommandConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x001C1ABC File Offset: 0x001C0ABC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComponentResourceKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 95, "ComponentResourceKey", typeof(ComponentResourceKey), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComponentResourceKey());
			wpfKnownType.TypeConverterType = typeof(ComponentResourceKeyConverter);
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 2;
			List<Type> list = new List<Type>();
			list.Add(typeof(Type));
			list.Add(typeof(object));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new ComponentResourceKey((Type)arguments[0], arguments[1])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x001C1B74 File Offset: 0x001C0B74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ComponentResourceKeyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 96, "ComponentResourceKeyConverter", typeof(ComponentResourceKeyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ComponentResourceKeyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x001C1BC5 File Offset: 0x001C0BC5
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CompositionTarget(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 97, "CompositionTarget", typeof(CompositionTarget), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x001C1BE8 File Offset: 0x001C0BE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Condition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 98, "Condition", typeof(Condition), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Condition());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x001C1C3C File Offset: 0x001C0C3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContainerVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 99, "ContainerVisual", typeof(ContainerVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContainerVisual());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x001C1C90 File Offset: 0x001C0C90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 100, "ContentControl", typeof(ContentControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentControl());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x001C1D14 File Offset: 0x001C0D14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 101, "ContentElement", typeof(ContentElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentElement());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x001C1D68 File Offset: 0x001C0D68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 102, "ContentPresenter", typeof(ContentPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x001C1DE4 File Offset: 0x001C0DE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentPropertyAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 103, "ContentPropertyAttribute", typeof(ContentPropertyAttribute), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContentPropertyAttribute());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x001C1E35 File Offset: 0x001C0E35
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContentWrapperAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 104, "ContentWrapperAttribute", typeof(ContentWrapperAttribute), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x001C1E58 File Offset: 0x001C0E58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContextMenu(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 105, "ContextMenu", typeof(ContextMenu), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ContextMenu());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x001C1EDC File Offset: 0x001C0EDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ContextMenuService(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 106, "ContextMenuService", typeof(ContextMenuService), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x001C1F00 File Offset: 0x001C0F00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Control(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 107, "Control", typeof(Control), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Control());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x001C1F7C File Offset: 0x001C0F7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ControlTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 108, "ControlTemplate", typeof(ControlTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ControlTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "TargetType";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x001C1FE3 File Offset: 0x001C0FE3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ControllableStoryboardAction(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 109, "ControllableStoryboardAction", typeof(ControllableStoryboardAction), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x001C2004 File Offset: 0x001C1004
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CornerRadius(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 110, "CornerRadius", typeof(CornerRadius), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(CornerRadius));
			wpfKnownType.TypeConverterType = typeof(CornerRadiusConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x001C2068 File Offset: 0x001C1068
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CornerRadiusConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 111, "CornerRadiusConverter", typeof(CornerRadiusConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CornerRadiusConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x001C20BC File Offset: 0x001C10BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CroppedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 112, "CroppedBitmap", typeof(CroppedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CroppedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x001C211D File Offset: 0x001C111D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CultureInfo(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 113, "CultureInfo", typeof(CultureInfo), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(CultureInfoConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x001C2150 File Offset: 0x001C1150
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CultureInfoConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 114, "CultureInfoConverter", typeof(CultureInfoConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CultureInfoConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x001C21A4 File Offset: 0x001C11A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CultureInfoIetfLanguageTagConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 115, "CultureInfoIetfLanguageTagConverter", typeof(CultureInfoIetfLanguageTagConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CultureInfoIetfLanguageTagConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x001C21F5 File Offset: 0x001C11F5
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Cursor(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 116, "Cursor", typeof(Cursor), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(CursorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x001C2228 File Offset: 0x001C1228
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CursorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 117, "CursorConverter", typeof(CursorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CursorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x001C227C File Offset: 0x001C127C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DashStyle(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 118, "DashStyle", typeof(DashStyle), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DashStyle());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x001C22CD File Offset: 0x001C12CD
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataChangedEventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 119, "DataChangedEventManager", typeof(DataChangedEventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x001C22F0 File Offset: 0x001C12F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 120, "DataTemplate", typeof(DataTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DataTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "DataTemplateKey";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x001C2358 File Offset: 0x001C1358
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataTemplateKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 121, "DataTemplateKey", typeof(DataTemplateKey), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DataTemplateKey());
			wpfKnownType.TypeConverterType = typeof(TemplateKeyConverter);
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(object));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new DataTemplateKey(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x001C2400 File Offset: 0x001C1400
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DataTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 122, "DataTrigger", typeof(DataTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DataTrigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x001C245C File Offset: 0x001C145C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DateTime(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 123, "DateTime", typeof(DateTime), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(DateTime));
			wpfKnownType.HasSpecialValueConverter = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x001C24B4 File Offset: 0x001C14B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DateTimeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 124, "DateTimeConverter", typeof(DateTimeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DateTimeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x001C2508 File Offset: 0x001C1508
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DateTimeConverter2(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 125, "DateTimeConverter2", typeof(DateTimeConverter2), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DateTimeConverter2());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x001C255C File Offset: 0x001C155C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Decimal(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 126, "Decimal", typeof(decimal), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0m);
			wpfKnownType.TypeConverterType = typeof(DecimalConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x001C25C0 File Offset: 0x001C15C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 127, "DecimalAnimation", typeof(DecimalAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x001C261C File Offset: 0x001C161C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 128, "DecimalAnimationBase", typeof(DecimalAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x001C264C File Offset: 0x001C164C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 129, "DecimalAnimationUsingKeyFrames", typeof(DecimalAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x001C26B8 File Offset: 0x001C16B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 130, "DecimalConverter", typeof(DecimalConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x001C270C File Offset: 0x001C170C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 131, "DecimalKeyFrame", typeof(DecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x001C2730 File Offset: 0x001C1730
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DecimalKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 132, "DecimalKeyFrameCollection", typeof(DecimalKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DecimalKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x001C278C File Offset: 0x001C178C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Decorator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 133, "Decorator", typeof(Decorator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Decorator());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x001C2814 File Offset: 0x001C1814
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DefinitionBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 134, "DefinitionBase", typeof(DefinitionBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x001C2860 File Offset: 0x001C1860
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DependencyObject(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 135, "DependencyObject", typeof(DependencyObject), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DependencyObject());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x001C28B4 File Offset: 0x001C18B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DependencyProperty(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 136, "DependencyProperty", typeof(DependencyProperty), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(DependencyPropertyConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x001C28E8 File Offset: 0x001C18E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DependencyPropertyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 137, "DependencyPropertyConverter", typeof(DependencyPropertyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DependencyPropertyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x001C293C File Offset: 0x001C193C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DialogResultConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 138, "DialogResultConverter", typeof(DialogResultConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DialogResultConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x001C2990 File Offset: 0x001C1990
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiffuseMaterial(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 139, "DiffuseMaterial", typeof(DiffuseMaterial), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiffuseMaterial());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x001C29E4 File Offset: 0x001C19E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DirectionalLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 140, "DirectionalLight", typeof(DirectionalLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DirectionalLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x001C2A38 File Offset: 0x001C1A38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteBooleanKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 141, "DiscreteBooleanKeyFrame", typeof(DiscreteBooleanKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteBooleanKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x001C2A8C File Offset: 0x001C1A8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 142, "DiscreteByteKeyFrame", typeof(DiscreteByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteByteKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x001C2AE0 File Offset: 0x001C1AE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteCharKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 143, "DiscreteCharKeyFrame", typeof(DiscreteCharKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteCharKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x001C2B34 File Offset: 0x001C1B34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 144, "DiscreteColorKeyFrame", typeof(DiscreteColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteColorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x001C2B88 File Offset: 0x001C1B88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteDecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 145, "DiscreteDecimalKeyFrame", typeof(DiscreteDecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteDecimalKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x001C2BDC File Offset: 0x001C1BDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteDoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 146, "DiscreteDoubleKeyFrame", typeof(DiscreteDoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteDoubleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x001C2C30 File Offset: 0x001C1C30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteInt16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 147, "DiscreteInt16KeyFrame", typeof(DiscreteInt16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteInt16KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x001C2C84 File Offset: 0x001C1C84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteInt32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 148, "DiscreteInt32KeyFrame", typeof(DiscreteInt32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteInt32KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x001C2CD8 File Offset: 0x001C1CD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteInt64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 149, "DiscreteInt64KeyFrame", typeof(DiscreteInt64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteInt64KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x001C2D2C File Offset: 0x001C1D2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteMatrixKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 150, "DiscreteMatrixKeyFrame", typeof(DiscreteMatrixKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteMatrixKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x001C2D80 File Offset: 0x001C1D80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteObjectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 151, "DiscreteObjectKeyFrame", typeof(DiscreteObjectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteObjectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x001C2DD4 File Offset: 0x001C1DD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscretePoint3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 152, "DiscretePoint3DKeyFrame", typeof(DiscretePoint3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscretePoint3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x001C2E28 File Offset: 0x001C1E28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscretePointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 153, "DiscretePointKeyFrame", typeof(DiscretePointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscretePointKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x001C2E7C File Offset: 0x001C1E7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteQuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 154, "DiscreteQuaternionKeyFrame", typeof(DiscreteQuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteQuaternionKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x001C2ED0 File Offset: 0x001C1ED0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteRectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 155, "DiscreteRectKeyFrame", typeof(DiscreteRectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteRectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x001C2F24 File Offset: 0x001C1F24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteRotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 156, "DiscreteRotation3DKeyFrame", typeof(DiscreteRotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteRotation3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x001C2F78 File Offset: 0x001C1F78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteSingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 157, "DiscreteSingleKeyFrame", typeof(DiscreteSingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteSingleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x001C2FCC File Offset: 0x001C1FCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteSizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 158, "DiscreteSizeKeyFrame", typeof(DiscreteSizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteSizeKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x001C3020 File Offset: 0x001C2020
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteStringKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 159, "DiscreteStringKeyFrame", typeof(DiscreteStringKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteStringKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x001C3074 File Offset: 0x001C2074
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 160, "DiscreteThicknessKeyFrame", typeof(DiscreteThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteThicknessKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x001C30C8 File Offset: 0x001C20C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteVector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 161, "DiscreteVector3DKeyFrame", typeof(DiscreteVector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteVector3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x001C311C File Offset: 0x001C211C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DiscreteVectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 162, "DiscreteVectorKeyFrame", typeof(DiscreteVectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DiscreteVectorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x001C3170 File Offset: 0x001C2170
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DockPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 163, "DockPanel", typeof(DockPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DockPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x001C31F8 File Offset: 0x001C21F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentPageView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 164, "DocumentPageView", typeof(DocumentPageView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DocumentPageView());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x001C3274 File Offset: 0x001C2274
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentReference(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 165, "DocumentReference", typeof(DocumentReference), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DocumentReference());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x001C32EC File Offset: 0x001C22EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 166, "DocumentViewer", typeof(DocumentViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DocumentViewer());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x001C3374 File Offset: 0x001C2374
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentViewerBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 167, "DocumentViewerBase", typeof(DocumentViewerBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x001C33D8 File Offset: 0x001C23D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Double(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 168, "Double", typeof(double), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0.0);
			wpfKnownType.TypeConverterType = typeof(DoubleConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x001C343C File Offset: 0x001C243C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 169, "DoubleAnimation", typeof(DoubleAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x001C349B File Offset: 0x001C249B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 170, "DoubleAnimationBase", typeof(DoubleAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x001C34CC File Offset: 0x001C24CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 171, "DoubleAnimationUsingKeyFrames", typeof(DoubleAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x001C3538 File Offset: 0x001C2538
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleAnimationUsingPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 172, "DoubleAnimationUsingPath", typeof(DoubleAnimationUsingPath), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleAnimationUsingPath());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x001C3598 File Offset: 0x001C2598
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 173, "DoubleCollection", typeof(DoubleCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleCollection());
			wpfKnownType.TypeConverterType = typeof(DoubleCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x001C3604 File Offset: 0x001C2604
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 174, "DoubleCollectionConverter", typeof(DoubleCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x001C3658 File Offset: 0x001C2658
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 175, "DoubleConverter", typeof(DoubleConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x001C36AC File Offset: 0x001C26AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 176, "DoubleIListConverter", typeof(DoubleIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x001C3700 File Offset: 0x001C2700
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 177, "DoubleKeyFrame", typeof(DoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x001C3724 File Offset: 0x001C2724
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DoubleKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 178, "DoubleKeyFrameCollection", typeof(DoubleKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DoubleKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x001C377F File Offset: 0x001C277F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Drawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 179, "Drawing", typeof(Drawing), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x001C37A4 File Offset: 0x001C27A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 180, "DrawingBrush", typeof(DrawingBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x001C3808 File Offset: 0x001C2808
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 181, "DrawingCollection", typeof(DrawingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x001C3863 File Offset: 0x001C2863
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingContext(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 182, "DrawingContext", typeof(DrawingContext), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x001C3888 File Offset: 0x001C2888
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 183, "DrawingGroup", typeof(DrawingGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x001C38E8 File Offset: 0x001C28E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingImage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 184, "DrawingImage", typeof(DrawingImage), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingImage());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x001C394C File Offset: 0x001C294C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DrawingVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 185, "DrawingVisual", typeof(DrawingVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DrawingVisual());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x001C39A0 File Offset: 0x001C29A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DropShadowBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 186, "DropShadowBitmapEffect", typeof(DropShadowBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DropShadowBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x001C39F4 File Offset: 0x001C29F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Duration(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 187, "Duration", typeof(Duration), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Duration));
			wpfKnownType.TypeConverterType = typeof(DurationConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x001C3A58 File Offset: 0x001C2A58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DurationConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 188, "DurationConverter", typeof(DurationConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DurationConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x001C3AAC File Offset: 0x001C2AAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DynamicResourceExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 189, "DynamicResourceExtension", typeof(DynamicResourceExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DynamicResourceExtension());
			wpfKnownType.TypeConverterType = typeof(DynamicResourceExtensionConverter);
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(object));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new DynamicResourceExtension(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x001C3B58 File Offset: 0x001C2B58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DynamicResourceExtensionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 190, "DynamicResourceExtensionConverter", typeof(DynamicResourceExtensionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new DynamicResourceExtensionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x001C3BAC File Offset: 0x001C2BAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Ellipse(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 191, "Ellipse", typeof(Ellipse), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Ellipse());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x001C3C28 File Offset: 0x001C2C28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EllipseGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 192, "EllipseGeometry", typeof(EllipseGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EllipseGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x001C3C8C File Offset: 0x001C2C8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EmbossBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 193, "EmbossBitmapEffect", typeof(EmbossBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EmbossBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x001C3CE0 File Offset: 0x001C2CE0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EmissiveMaterial(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 194, "EmissiveMaterial", typeof(EmissiveMaterial), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EmissiveMaterial());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x001C3D34 File Offset: 0x001C2D34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EnumConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 195, "EnumConverter", typeof(EnumConverter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x001C3D58 File Offset: 0x001C2D58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 196, "EventManager", typeof(EventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x001C3D7C File Offset: 0x001C2D7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EventSetter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 197, "EventSetter", typeof(EventSetter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EventSetter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x001C3DD0 File Offset: 0x001C2DD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_EventTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 198, "EventTrigger", typeof(EventTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new EventTrigger());
			wpfKnownType.ContentPropertyName = "Actions";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x001C3E30 File Offset: 0x001C2E30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Expander(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 199, "Expander", typeof(Expander), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Expander());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x001C3EB7 File Offset: 0x001C2EB7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Expression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 200, "Expression", typeof(Expression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x001C3EEC File Offset: 0x001C2EEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ExpressionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 201, "ExpressionConverter", typeof(ExpressionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ExpressionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x001C3F40 File Offset: 0x001C2F40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Figure(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 202, "Figure", typeof(Figure), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Figure());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x001C3FBC File Offset: 0x001C2FBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FigureLength(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 203, "FigureLength", typeof(FigureLength), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FigureLength));
			wpfKnownType.TypeConverterType = typeof(FigureLengthConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x001C4020 File Offset: 0x001C3020
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FigureLengthConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 204, "FigureLengthConverter", typeof(FigureLengthConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FigureLengthConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA5 RID: 12197 RVA: 0x001C4074 File Offset: 0x001C3074
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FixedDocument(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 205, "FixedDocument", typeof(FixedDocument), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FixedDocument());
			wpfKnownType.ContentPropertyName = "Pages";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x001C40F0 File Offset: 0x001C30F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FixedDocumentSequence(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 206, "FixedDocumentSequence", typeof(FixedDocumentSequence), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FixedDocumentSequence());
			wpfKnownType.ContentPropertyName = "References";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x001C416C File Offset: 0x001C316C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FixedPage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 207, "FixedPage", typeof(FixedPage), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FixedPage());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x001C41F4 File Offset: 0x001C31F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Floater(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 208, "Floater", typeof(Floater), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Floater());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x001C4270 File Offset: 0x001C3270
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocument(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 209, "FlowDocument", typeof(FlowDocument), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocument());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x001C42EC File Offset: 0x001C32EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocumentPageViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 210, "FlowDocumentPageViewer", typeof(FlowDocumentPageViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocumentPageViewer());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x001C4374 File Offset: 0x001C3374
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocumentReader(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 211, "FlowDocumentReader", typeof(FlowDocumentReader), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocumentReader());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x001C43FC File Offset: 0x001C33FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FlowDocumentScrollViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 212, "FlowDocumentScrollViewer", typeof(FlowDocumentScrollViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FlowDocumentScrollViewer());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x001C4483 File Offset: 0x001C3483
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FocusManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 213, "FocusManager", typeof(FocusManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x001C44A8 File Offset: 0x001C34A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontFamily(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 214, "FontFamily", typeof(FontFamily), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontFamily());
			wpfKnownType.TypeConverterType = typeof(FontFamilyConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x001C450C File Offset: 0x001C350C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontFamilyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 215, "FontFamilyConverter", typeof(FontFamilyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontFamilyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x001C4560 File Offset: 0x001C3560
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontSizeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 216, "FontSizeConverter", typeof(FontSizeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontSizeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x001C45B4 File Offset: 0x001C35B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStretch(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 217, "FontStretch", typeof(FontStretch), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FontStretch));
			wpfKnownType.TypeConverterType = typeof(FontStretchConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x001C4618 File Offset: 0x001C3618
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStretchConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 218, "FontStretchConverter", typeof(FontStretchConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontStretchConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB3 RID: 12211 RVA: 0x001C466C File Offset: 0x001C366C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStyle(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 219, "FontStyle", typeof(FontStyle), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FontStyle));
			wpfKnownType.TypeConverterType = typeof(FontStyleConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB4 RID: 12212 RVA: 0x001C46D0 File Offset: 0x001C36D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontStyleConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 220, "FontStyleConverter", typeof(FontStyleConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontStyleConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x001C4724 File Offset: 0x001C3724
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontWeight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 221, "FontWeight", typeof(FontWeight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(FontWeight));
			wpfKnownType.TypeConverterType = typeof(FontWeightConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x001C4788 File Offset: 0x001C3788
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FontWeightConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 222, "FontWeightConverter", typeof(FontWeightConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FontWeightConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x001C47DC File Offset: 0x001C37DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FormatConvertedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 223, "FormatConvertedBitmap", typeof(FormatConvertedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FormatConvertedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x001C4840 File Offset: 0x001C3840
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Frame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 224, "Frame", typeof(Frame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Frame());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x001C48BC File Offset: 0x001C38BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkContentElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 225, "FrameworkContentElement", typeof(FrameworkContentElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkContentElement());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x001C4930 File Offset: 0x001C3930
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 226, "FrameworkElement", typeof(FrameworkElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkElement());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x001C49AC File Offset: 0x001C39AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkElementFactory(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 227, "FrameworkElementFactory", typeof(FrameworkElementFactory), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkElementFactory());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x001C4A00 File Offset: 0x001C3A00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkPropertyMetadata(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 228, "FrameworkPropertyMetadata", typeof(FrameworkPropertyMetadata), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new FrameworkPropertyMetadata());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x001C4A54 File Offset: 0x001C3A54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkPropertyMetadataOptions(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 229, "FrameworkPropertyMetadataOptions", typeof(FrameworkPropertyMetadataOptions), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => FrameworkPropertyMetadataOptions.None);
			wpfKnownType.TypeConverterType = typeof(FrameworkPropertyMetadataOptions);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x001C4AB8 File Offset: 0x001C3AB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkRichTextComposition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 230, "FrameworkRichTextComposition", typeof(FrameworkRichTextComposition), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x001C4ADC File Offset: 0x001C3ADC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 231, "FrameworkTemplate", typeof(FrameworkTemplate), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x001C4B0B File Offset: 0x001C3B0B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_FrameworkTextComposition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 232, "FrameworkTextComposition", typeof(FrameworkTextComposition), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x001C4B2F File Offset: 0x001C3B2F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Freezable(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 233, "Freezable", typeof(Freezable), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x001C4B53 File Offset: 0x001C3B53
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeneralTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 234, "GeneralTransform", typeof(GeneralTransform), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x001C4B78 File Offset: 0x001C3B78
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeneralTransformCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 235, "GeneralTransformCollection", typeof(GeneralTransformCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeneralTransformCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x001C4BD4 File Offset: 0x001C3BD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeneralTransformGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 236, "GeneralTransformGroup", typeof(GeneralTransformGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeneralTransformGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x001C4C33 File Offset: 0x001C3C33
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Geometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 237, "Geometry", typeof(Geometry), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x001C4C67 File Offset: 0x001C3C67
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Geometry3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 238, "Geometry3D", typeof(Geometry3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x001C4C8C File Offset: 0x001C3C8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 239, "GeometryCollection", typeof(GeometryCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x001C4CE8 File Offset: 0x001C3CE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 240, "GeometryConverter", typeof(GeometryConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x001C4D3C File Offset: 0x001C3D3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 241, "GeometryDrawing", typeof(GeometryDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x001C4D90 File Offset: 0x001C3D90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 242, "GeometryGroup", typeof(GeometryGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x001C4E00 File Offset: 0x001C3E00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GeometryModel3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 243, "GeometryModel3D", typeof(GeometryModel3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GeometryModel3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x001C4E54 File Offset: 0x001C3E54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GestureRecognizer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 244, "GestureRecognizer", typeof(GestureRecognizer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GestureRecognizer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x001C4EA8 File Offset: 0x001C3EA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GifBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 245, "GifBitmapDecoder", typeof(GifBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x001C4ECC File Offset: 0x001C3ECC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GifBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 246, "GifBitmapEncoder", typeof(GifBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GifBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x001C4F20 File Offset: 0x001C3F20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GlyphRun(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 247, "GlyphRun", typeof(GlyphRun), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GlyphRun());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x001C4F74 File Offset: 0x001C3F74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GlyphRunDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 248, "GlyphRunDrawing", typeof(GlyphRunDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GlyphRunDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x001C4FC8 File Offset: 0x001C3FC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GlyphTypeface(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 249, "GlyphTypeface", typeof(GlyphTypeface), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GlyphTypeface());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x001C501C File Offset: 0x001C401C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Glyphs(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 250, "Glyphs", typeof(Glyphs), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Glyphs());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x001C5098 File Offset: 0x001C4098
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GradientBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 251, "GradientBrush", typeof(GradientBrush), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "GradientStops";
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x001C50D8 File Offset: 0x001C40D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GradientStop(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 252, "GradientStop", typeof(GradientStop), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GradientStop());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x001C512C File Offset: 0x001C412C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GradientStopCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 253, "GradientStopCollection", typeof(GradientStopCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GradientStopCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x001C5188 File Offset: 0x001C4188
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Grid(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 254, "Grid", typeof(Grid), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Grid());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x001C5210 File Offset: 0x001C4210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridLength(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 255, "GridLength", typeof(GridLength), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(GridLength));
			wpfKnownType.TypeConverterType = typeof(GridLengthConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x001C5274 File Offset: 0x001C4274
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridLengthConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 256, "GridLengthConverter", typeof(GridLengthConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridLengthConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x001C52C8 File Offset: 0x001C42C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridSplitter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 257, "GridSplitter", typeof(GridSplitter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridSplitter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x001C5344 File Offset: 0x001C4344
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 258, "GridView", typeof(GridView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridView());
			wpfKnownType.ContentPropertyName = "Columns";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x001C53A4 File Offset: 0x001C43A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewColumn(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 259, "GridViewColumn", typeof(GridViewColumn), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewColumn());
			wpfKnownType.ContentPropertyName = "Header";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x001C5404 File Offset: 0x001C4404
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewColumnHeader(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 260, "GridViewColumnHeader", typeof(GridViewColumnHeader), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewColumnHeader());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x001C548C File Offset: 0x001C448C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewHeaderRowPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 261, "GridViewHeaderRowPresenter", typeof(GridViewHeaderRowPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewHeaderRowPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x001C5508 File Offset: 0x001C4508
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewRowPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 262, "GridViewRowPresenter", typeof(GridViewRowPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GridViewRowPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x001C5584 File Offset: 0x001C4584
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GridViewRowPresenterBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 263, "GridViewRowPresenterBase", typeof(GridViewRowPresenterBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x001C55DC File Offset: 0x001C45DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GroupBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 264, "GroupBox", typeof(GroupBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GroupBox());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x001C5664 File Offset: 0x001C4664
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GroupItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 265, "GroupItem", typeof(GroupItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GroupItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x001C56EC File Offset: 0x001C46EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Guid(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 266, "Guid", typeof(Guid), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Guid));
			wpfKnownType.TypeConverterType = typeof(GuidConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x001C5750 File Offset: 0x001C4750
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GuidConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 267, "GuidConverter", typeof(GuidConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GuidConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x001C57A4 File Offset: 0x001C47A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_GuidelineSet(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 268, "GuidelineSet", typeof(GuidelineSet), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new GuidelineSet());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x001C57F8 File Offset: 0x001C47F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HeaderedContentControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 269, "HeaderedContentControl", typeof(HeaderedContentControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HeaderedContentControl());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x001C5880 File Offset: 0x001C4880
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HeaderedItemsControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 270, "HeaderedItemsControl", typeof(HeaderedItemsControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HeaderedItemsControl());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x001C5908 File Offset: 0x001C4908
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HierarchicalDataTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 271, "HierarchicalDataTemplate", typeof(HierarchicalDataTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HierarchicalDataTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "DataTemplateKey";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x001C5974 File Offset: 0x001C4974
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_HostVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 272, "HostVisual", typeof(HostVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new HostVisual());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x001C59C8 File Offset: 0x001C49C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Hyperlink(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 273, "Hyperlink", typeof(Hyperlink), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Hyperlink());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x001C5A44 File Offset: 0x001C4A44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IAddChild(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 274, "IAddChild", typeof(IAddChild), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x001C5A68 File Offset: 0x001C4A68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IAddChildInternal(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 275, "IAddChildInternal", typeof(IAddChildInternal), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x001C5A8C File Offset: 0x001C4A8C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ICommand(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 276, "ICommand", typeof(ICommand), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(CommandConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x001C5AC0 File Offset: 0x001C4AC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IComponentConnector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 277, "IComponentConnector", typeof(IComponentConnector), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x001C5AE4 File Offset: 0x001C4AE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_INameScope(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 278, "INameScope", typeof(INameScope), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x001C5B08 File Offset: 0x001C4B08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IStyleConnector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 279, "IStyleConnector", typeof(IStyleConnector), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x001C5B2C File Offset: 0x001C4B2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_IconBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 280, "IconBitmapDecoder", typeof(IconBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x001C5B50 File Offset: 0x001C4B50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Image(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 281, "Image", typeof(Image), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Image());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x001C5BCC File Offset: 0x001C4BCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 282, "ImageBrush", typeof(ImageBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ImageBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x001C5C30 File Offset: 0x001C4C30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 283, "ImageDrawing", typeof(ImageDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ImageDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x001C5C84 File Offset: 0x001C4C84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageMetadata(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 284, "ImageMetadata", typeof(ImageMetadata), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF5 RID: 12277 RVA: 0x001C5CA8 File Offset: 0x001C4CA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 285, "ImageSource", typeof(ImageSource), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF6 RID: 12278 RVA: 0x001C5CDC File Offset: 0x001C4CDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ImageSourceConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 286, "ImageSourceConverter", typeof(ImageSourceConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ImageSourceConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF7 RID: 12279 RVA: 0x001C5D30 File Offset: 0x001C4D30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InPlaceBitmapMetadataWriter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 287, "InPlaceBitmapMetadataWriter", typeof(InPlaceBitmapMetadataWriter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF8 RID: 12280 RVA: 0x001C5D54 File Offset: 0x001C4D54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InkCanvas(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 288, "InkCanvas", typeof(InkCanvas), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InkCanvas());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FF9 RID: 12281 RVA: 0x001C5DDC File Offset: 0x001C4DDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InkPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 289, "InkPresenter", typeof(InkPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InkPresenter());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FFA RID: 12282 RVA: 0x001C5E64 File Offset: 0x001C4E64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Inline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 290, "Inline", typeof(Inline), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FFB RID: 12283 RVA: 0x001C5EB0 File Offset: 0x001C4EB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InlineCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 291, "InlineCollection", typeof(InlineCollection), isBamlType, useV3Rules);
			wpfKnownType.WhitespaceSignificantCollection = true;
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x001C5EE4 File Offset: 0x001C4EE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InlineUIContainer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 292, "InlineUIContainer", typeof(InlineUIContainer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InlineUIContainer());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x001C5F60 File Offset: 0x001C4F60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 293, "InputBinding", typeof(InputBinding), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x001C5F84 File Offset: 0x001C4F84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 294, "InputDevice", typeof(InputDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x001C5FA8 File Offset: 0x001C4FA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputLanguageManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 295, "InputLanguageManager", typeof(InputLanguageManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x001C5FCC File Offset: 0x001C4FCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 296, "InputManager", typeof(InputManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x001C5FF0 File Offset: 0x001C4FF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputMethod(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 297, "InputMethod", typeof(InputMethod), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003002 RID: 12290 RVA: 0x001C6014 File Offset: 0x001C5014
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScope(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 298, "InputScope", typeof(InputScope), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScope());
			wpfKnownType.TypeConverterType = typeof(InputScopeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x001C6078 File Offset: 0x001C5078
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScopeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 299, "InputScopeConverter", typeof(InputScopeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScopeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003004 RID: 12292 RVA: 0x001C60CC File Offset: 0x001C50CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScopeName(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 300, "InputScopeName", typeof(InputScopeName), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScopeName());
			wpfKnownType.ContentPropertyName = "NameValue";
			wpfKnownType.TypeConverterType = typeof(InputScopeNameConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003005 RID: 12293 RVA: 0x001C613C File Offset: 0x001C513C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputScopeNameConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 301, "InputScopeNameConverter", typeof(InputScopeNameConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputScopeNameConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003006 RID: 12294 RVA: 0x001C6190 File Offset: 0x001C5190
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 302, "Int16", typeof(short), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(Int16Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003007 RID: 12295 RVA: 0x001C61F4 File Offset: 0x001C51F4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16Animation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 303, "Int16Animation", typeof(Int16Animation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16Animation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003008 RID: 12296 RVA: 0x001C6253 File Offset: 0x001C5253
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16AnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 304, "Int16AnimationBase", typeof(Int16AnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003009 RID: 12297 RVA: 0x001C6284 File Offset: 0x001C5284
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16AnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 305, "Int16AnimationUsingKeyFrames", typeof(Int16AnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16AnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x001C62F0 File Offset: 0x001C52F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 306, "Int16Converter", typeof(Int16Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x001C6344 File Offset: 0x001C5344
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 307, "Int16KeyFrame", typeof(Int16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x001C6368 File Offset: 0x001C5368
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int16KeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 308, "Int16KeyFrameCollection", typeof(Int16KeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int16KeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x001C63C4 File Offset: 0x001C53C4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 309, "Int32", typeof(int), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(Int32Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x001C6428 File Offset: 0x001C5428
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Animation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 310, "Int32Animation", typeof(Int32Animation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32Animation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x001C6487 File Offset: 0x001C5487
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32AnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 311, "Int32AnimationBase", typeof(Int32AnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x001C64B8 File Offset: 0x001C54B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32AnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 312, "Int32AnimationUsingKeyFrames", typeof(Int32AnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32AnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x001C6524 File Offset: 0x001C5524
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Collection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 313, "Int32Collection", typeof(Int32Collection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32Collection());
			wpfKnownType.TypeConverterType = typeof(Int32CollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x001C6590 File Offset: 0x001C5590
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32CollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 314, "Int32CollectionConverter", typeof(Int32CollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32CollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x001C65E4 File Offset: 0x001C55E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 315, "Int32Converter", typeof(Int32Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x001C6638 File Offset: 0x001C5638
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 316, "Int32KeyFrame", typeof(Int32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x001C665C File Offset: 0x001C565C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32KeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 317, "Int32KeyFrameCollection", typeof(Int32KeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32KeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003016 RID: 12310 RVA: 0x001C66B8 File Offset: 0x001C56B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32Rect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 318, "Int32Rect", typeof(Int32Rect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Int32Rect));
			wpfKnownType.TypeConverterType = typeof(Int32RectConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003017 RID: 12311 RVA: 0x001C671C File Offset: 0x001C571C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int32RectConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 319, "Int32RectConverter", typeof(Int32RectConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int32RectConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x001C6770 File Offset: 0x001C5770
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 320, "Int64", typeof(long), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0L);
			wpfKnownType.TypeConverterType = typeof(Int64Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x001C67D4 File Offset: 0x001C57D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64Animation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 321, "Int64Animation", typeof(Int64Animation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64Animation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x001C6833 File Offset: 0x001C5833
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64AnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 322, "Int64AnimationBase", typeof(Int64AnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x001C6864 File Offset: 0x001C5864
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64AnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 323, "Int64AnimationUsingKeyFrames", typeof(Int64AnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64AnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x001C68D0 File Offset: 0x001C58D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 324, "Int64Converter", typeof(Int64Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x001C6924 File Offset: 0x001C5924
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 325, "Int64KeyFrame", typeof(Int64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x001C6948 File Offset: 0x001C5948
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Int64KeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 326, "Int64KeyFrameCollection", typeof(Int64KeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Int64KeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x001C69A4 File Offset: 0x001C59A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Italic(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 327, "Italic", typeof(Italic), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Italic());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x001C6A20 File Offset: 0x001C5A20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 328, "ItemCollection", typeof(ItemCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x001C6A4C File Offset: 0x001C5A4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemsControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 329, "ItemsControl", typeof(ItemsControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemsControl());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x001C6AD4 File Offset: 0x001C5AD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemsPanelTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 330, "ItemsPanelTemplate", typeof(ItemsPanelTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemsPanelTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x001C6B34 File Offset: 0x001C5B34
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemsPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 331, "ItemsPresenter", typeof(ItemsPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemsPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x001C6BB0 File Offset: 0x001C5BB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalEntry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 332, "JournalEntry", typeof(JournalEntry), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x001C6BD4 File Offset: 0x001C5BD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalEntryListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 333, "JournalEntryListConverter", typeof(JournalEntryListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new JournalEntryListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x001C6C28 File Offset: 0x001C5C28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalEntryUnifiedViewConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 334, "JournalEntryUnifiedViewConverter", typeof(JournalEntryUnifiedViewConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new JournalEntryUnifiedViewConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x001C6C7C File Offset: 0x001C5C7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JpegBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 335, "JpegBitmapDecoder", typeof(JpegBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x001C6CA0 File Offset: 0x001C5CA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JpegBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 336, "JpegBitmapEncoder", typeof(JpegBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new JpegBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x001C6CF4 File Offset: 0x001C5CF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 337, "KeyBinding", typeof(KeyBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyBinding());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x001C6D48 File Offset: 0x001C5D48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 338, "KeyConverter", typeof(KeyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x001C6D9C File Offset: 0x001C5D9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyGesture(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 339, "KeyGesture", typeof(KeyGesture), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(KeyGestureConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x001C6DD0 File Offset: 0x001C5DD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyGestureConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 340, "KeyGestureConverter", typeof(KeyGestureConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyGestureConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x001C6E24 File Offset: 0x001C5E24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeySpline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 341, "KeySpline", typeof(KeySpline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeySpline());
			wpfKnownType.TypeConverterType = typeof(KeySplineConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x001C6E88 File Offset: 0x001C5E88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeySplineConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 342, "KeySplineConverter", typeof(KeySplineConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeySplineConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x001C6EDC File Offset: 0x001C5EDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyTime(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 343, "KeyTime", typeof(KeyTime), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(KeyTime));
			wpfKnownType.TypeConverterType = typeof(KeyTimeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x001C6F40 File Offset: 0x001C5F40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyTimeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 344, "KeyTimeConverter", typeof(KeyTimeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new KeyTimeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x001C6F94 File Offset: 0x001C5F94
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyboardDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 345, "KeyboardDevice", typeof(KeyboardDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x001C6FB8 File Offset: 0x001C5FB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Label(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 346, "Label", typeof(Label), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Label());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x001C703F File Offset: 0x001C603F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LateBoundBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 347, "LateBoundBitmapDecoder", typeof(LateBoundBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x001C7064 File Offset: 0x001C6064
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LengthConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 348, "LengthConverter", typeof(LengthConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LengthConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x001C70B8 File Offset: 0x001C60B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Light(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 349, "Light", typeof(Light), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x001C70DC File Offset: 0x001C60DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Line(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 350, "Line", typeof(Line), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Line());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x001C7158 File Offset: 0x001C6158
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LineBreak(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 351, "LineBreak", typeof(LineBreak), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LineBreak());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x001C71CC File Offset: 0x001C61CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LineGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 352, "LineGeometry", typeof(LineGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LineGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x001C7230 File Offset: 0x001C6230
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LineSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 353, "LineSegment", typeof(LineSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LineSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x001C7284 File Offset: 0x001C6284
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 354, "LinearByteKeyFrame", typeof(LinearByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearByteKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600303B RID: 12347 RVA: 0x001C72D8 File Offset: 0x001C62D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 355, "LinearColorKeyFrame", typeof(LinearColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearColorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600303C RID: 12348 RVA: 0x001C732C File Offset: 0x001C632C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearDecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 356, "LinearDecimalKeyFrame", typeof(LinearDecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearDecimalKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x001C7380 File Offset: 0x001C6380
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearDoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 357, "LinearDoubleKeyFrame", typeof(LinearDoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearDoubleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x001C73D4 File Offset: 0x001C63D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearGradientBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 358, "LinearGradientBrush", typeof(LinearGradientBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearGradientBrush());
			wpfKnownType.ContentPropertyName = "GradientStops";
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x001C7444 File Offset: 0x001C6444
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearInt16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 359, "LinearInt16KeyFrame", typeof(LinearInt16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearInt16KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x001C7498 File Offset: 0x001C6498
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearInt32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 360, "LinearInt32KeyFrame", typeof(LinearInt32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearInt32KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x001C74EC File Offset: 0x001C64EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearInt64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 361, "LinearInt64KeyFrame", typeof(LinearInt64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearInt64KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x001C7540 File Offset: 0x001C6540
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearPoint3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 362, "LinearPoint3DKeyFrame", typeof(LinearPoint3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearPoint3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x001C7594 File Offset: 0x001C6594
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearPointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 363, "LinearPointKeyFrame", typeof(LinearPointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearPointKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x001C75E8 File Offset: 0x001C65E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearQuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 364, "LinearQuaternionKeyFrame", typeof(LinearQuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearQuaternionKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x001C763C File Offset: 0x001C663C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearRectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 365, "LinearRectKeyFrame", typeof(LinearRectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearRectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x001C7690 File Offset: 0x001C6690
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearRotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 366, "LinearRotation3DKeyFrame", typeof(LinearRotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearRotation3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x001C76E4 File Offset: 0x001C66E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearSingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 367, "LinearSingleKeyFrame", typeof(LinearSingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearSingleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x001C7738 File Offset: 0x001C6738
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearSizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 368, "LinearSizeKeyFrame", typeof(LinearSizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearSizeKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x001C778C File Offset: 0x001C678C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 369, "LinearThicknessKeyFrame", typeof(LinearThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearThicknessKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x001C77E0 File Offset: 0x001C67E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearVector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 370, "LinearVector3DKeyFrame", typeof(LinearVector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearVector3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x001C7834 File Offset: 0x001C6834
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LinearVectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 371, "LinearVectorKeyFrame", typeof(LinearVectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new LinearVectorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x001C7888 File Offset: 0x001C6888
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_List(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 372, "List", typeof(List), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new List());
			wpfKnownType.ContentPropertyName = "ListItems";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x001C7904 File Offset: 0x001C6904
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 373, "ListBox", typeof(ListBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListBox());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x001C798C File Offset: 0x001C698C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListBoxItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 374, "ListBoxItem", typeof(ListBoxItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListBoxItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x001C7A13 File Offset: 0x001C6A13
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListCollectionView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 375, "ListCollectionView", typeof(ListCollectionView), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x001C7A38 File Offset: 0x001C6A38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 376, "ListItem", typeof(ListItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListItem());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x001C7AB4 File Offset: 0x001C6AB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 377, "ListView", typeof(ListView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListView());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x001C7B3C File Offset: 0x001C6B3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ListViewItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 378, "ListViewItem", typeof(ListViewItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ListViewItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x001C7BC3 File Offset: 0x001C6BC3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Localization(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 379, "Localization", typeof(Localization), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x001C7BE7 File Offset: 0x001C6BE7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_LostFocusEventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 380, "LostFocusEventManager", typeof(LostFocusEventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x001C7C0B File Offset: 0x001C6C0B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MarkupExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 381, "MarkupExtension", typeof(MarkupExtension), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x001C7C2F File Offset: 0x001C6C2F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Material(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 382, "Material", typeof(Material), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x001C7C54 File Offset: 0x001C6C54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MaterialCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 383, "MaterialCollection", typeof(MaterialCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MaterialCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x001C7CB0 File Offset: 0x001C6CB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MaterialGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 384, "MaterialGroup", typeof(MaterialGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MaterialGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x001C7D10 File Offset: 0x001C6D10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Matrix(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 385, "Matrix", typeof(Matrix), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Matrix));
			wpfKnownType.TypeConverterType = typeof(MatrixConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x001C7D74 File Offset: 0x001C6D74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Matrix3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 386, "Matrix3D", typeof(Matrix3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Matrix3D));
			wpfKnownType.TypeConverterType = typeof(Matrix3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x001C7DD8 File Offset: 0x001C6DD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Matrix3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 387, "Matrix3DConverter", typeof(Matrix3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Matrix3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x001C7E2C File Offset: 0x001C6E2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 388, "MatrixAnimationBase", typeof(MatrixAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x001C7E5C File Offset: 0x001C6E5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 389, "MatrixAnimationUsingKeyFrames", typeof(MatrixAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x001C7EC8 File Offset: 0x001C6EC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixAnimationUsingPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 390, "MatrixAnimationUsingPath", typeof(MatrixAnimationUsingPath), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixAnimationUsingPath());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x001C7F28 File Offset: 0x001C6F28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 391, "MatrixCamera", typeof(MatrixCamera), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixCamera());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x001C7F7C File Offset: 0x001C6F7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 392, "MatrixConverter", typeof(MatrixConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x001C7FD0 File Offset: 0x001C6FD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 393, "MatrixKeyFrame", typeof(MatrixKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x001C7FF4 File Offset: 0x001C6FF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 394, "MatrixKeyFrameCollection", typeof(MatrixKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x001C8050 File Offset: 0x001C7050
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 395, "MatrixTransform", typeof(MatrixTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x001C80B4 File Offset: 0x001C70B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MatrixTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 396, "MatrixTransform3D", typeof(MatrixTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MatrixTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x001C8108 File Offset: 0x001C7108
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaClock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 397, "MediaClock", typeof(MediaClock), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x001C812C File Offset: 0x001C712C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 398, "MediaElement", typeof(MediaElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MediaElement());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x001C81A8 File Offset: 0x001C71A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaPlayer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 399, "MediaPlayer", typeof(MediaPlayer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MediaPlayer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x001C81FC File Offset: 0x001C71FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MediaTimeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 400, "MediaTimeline", typeof(MediaTimeline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MediaTimeline());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x001C825C File Offset: 0x001C725C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Menu(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 401, "Menu", typeof(Menu), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Menu());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x001C82E4 File Offset: 0x001C72E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MenuBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 402, "MenuBase", typeof(MenuBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x001C8348 File Offset: 0x001C7348
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MenuItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 403, "MenuItem", typeof(MenuItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MenuItem());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x001C83D0 File Offset: 0x001C73D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MenuScrollingVisibilityConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 404, "MenuScrollingVisibilityConverter", typeof(MenuScrollingVisibilityConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MenuScrollingVisibilityConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x001C8424 File Offset: 0x001C7424
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MeshGeometry3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 405, "MeshGeometry3D", typeof(MeshGeometry3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MeshGeometry3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x001C8478 File Offset: 0x001C7478
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Model3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 406, "Model3D", typeof(Model3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x001C849C File Offset: 0x001C749C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Model3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 407, "Model3DCollection", typeof(Model3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Model3DCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x001C84F8 File Offset: 0x001C74F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Model3DGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 408, "Model3DGroup", typeof(Model3DGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Model3DGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x001C8558 File Offset: 0x001C7558
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ModelVisual3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 409, "ModelVisual3D", typeof(ModelVisual3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ModelVisual3D());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x001C85B8 File Offset: 0x001C75B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ModifierKeysConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 410, "ModifierKeysConverter", typeof(ModifierKeysConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ModifierKeysConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x001C860C File Offset: 0x001C760C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseActionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 411, "MouseActionConverter", typeof(MouseActionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseActionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x001C8660 File Offset: 0x001C7660
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 412, "MouseBinding", typeof(MouseBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseBinding());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x001C86B4 File Offset: 0x001C76B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 413, "MouseDevice", typeof(MouseDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x001C86D8 File Offset: 0x001C76D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseGesture(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 414, "MouseGesture", typeof(MouseGesture), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseGesture());
			wpfKnownType.TypeConverterType = typeof(MouseGestureConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x001C873C File Offset: 0x001C773C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MouseGestureConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 415, "MouseGestureConverter", typeof(MouseGestureConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MouseGestureConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x001C8790 File Offset: 0x001C7790
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 416, "MultiBinding", typeof(MultiBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MultiBinding());
			wpfKnownType.ContentPropertyName = "Bindings";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x001C87EF File Offset: 0x001C77EF
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiBindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 417, "MultiBindingExpression", typeof(MultiBindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x001C8824 File Offset: 0x001C7824
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiDataTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 418, "MultiDataTrigger", typeof(MultiDataTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MultiDataTrigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x001C8884 File Offset: 0x001C7884
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_MultiTrigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 419, "MultiTrigger", typeof(MultiTrigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MultiTrigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x001C88E4 File Offset: 0x001C78E4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NameScope(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 420, "NameScope", typeof(NameScope), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NameScope());
			wpfKnownType.CollectionKind = XamlCollectionKind.Dictionary;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x001C8940 File Offset: 0x001C7940
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NavigationWindow(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 421, "NavigationWindow", typeof(NavigationWindow), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NavigationWindow());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x001C89BC File Offset: 0x001C79BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NullExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 422, "NullExtension", typeof(NullExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NullExtension());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x001C8A10 File Offset: 0x001C7A10
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NullableBoolConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 423, "NullableBoolConverter", typeof(NullableBoolConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NullableBoolConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x001C8A64 File Offset: 0x001C7A64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NullableConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 424, "NullableConverter", typeof(NullableConverter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x001C8A88 File Offset: 0x001C7A88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NumberSubstitution(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 425, "NumberSubstitution", typeof(NumberSubstitution), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new NumberSubstitution());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x001C8ADC File Offset: 0x001C7ADC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Object(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 426, "Object", typeof(object), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new object());
			wpfKnownType.HasSpecialValueConverter = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x001C8B37 File Offset: 0x001C7B37
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 427, "ObjectAnimationBase", typeof(ObjectAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x001C8B68 File Offset: 0x001C7B68
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 428, "ObjectAnimationUsingKeyFrames", typeof(ObjectAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ObjectAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x001C8BD4 File Offset: 0x001C7BD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectDataProvider(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 429, "ObjectDataProvider", typeof(ObjectDataProvider), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ObjectDataProvider());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x001C8C28 File Offset: 0x001C7C28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 430, "ObjectKeyFrame", typeof(ObjectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x001C8C4C File Offset: 0x001C7C4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ObjectKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 431, "ObjectKeyFrameCollection", typeof(ObjectKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ObjectKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x001C8CA8 File Offset: 0x001C7CA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_OrthographicCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 432, "OrthographicCamera", typeof(OrthographicCamera), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new OrthographicCamera());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x001C8CFC File Offset: 0x001C7CFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_OuterGlowBitmapEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 433, "OuterGlowBitmapEffect", typeof(OuterGlowBitmapEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new OuterGlowBitmapEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x001C8D50 File Offset: 0x001C7D50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Page(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 434, "Page", typeof(Page), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Page());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x001C8DD8 File Offset: 0x001C7DD8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PageContent(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 435, "PageContent", typeof(PageContent), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PageContent());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x001C8E60 File Offset: 0x001C7E60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PageFunctionBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 436, "PageFunctionBase", typeof(PageFunctionBase), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x001C8EC4 File Offset: 0x001C7EC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Panel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 437, "Panel", typeof(Panel), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x001C8F28 File Offset: 0x001C7F28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Paragraph(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 438, "Paragraph", typeof(Paragraph), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Paragraph());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x001C8FA4 File Offset: 0x001C7FA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ParallelTimeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 439, "ParallelTimeline", typeof(ParallelTimeline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ParallelTimeline());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x001C9010 File Offset: 0x001C8010
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ParserContext(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 440, "ParserContext", typeof(ParserContext), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ParserContext());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x001C9064 File Offset: 0x001C8064
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PasswordBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 441, "PasswordBox", typeof(PasswordBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PasswordBox());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x001C90E0 File Offset: 0x001C80E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Path(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 442, "Path", typeof(Path), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Path());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x001C915C File Offset: 0x001C815C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathFigure(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 443, "PathFigure", typeof(PathFigure), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathFigure());
			wpfKnownType.ContentPropertyName = "Segments";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x001C91BC File Offset: 0x001C81BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathFigureCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 444, "PathFigureCollection", typeof(PathFigureCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathFigureCollection());
			wpfKnownType.TypeConverterType = typeof(PathFigureCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x001C9228 File Offset: 0x001C8228
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathFigureCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 445, "PathFigureCollectionConverter", typeof(PathFigureCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathFigureCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x001C927C File Offset: 0x001C827C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 446, "PathGeometry", typeof(PathGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathGeometry());
			wpfKnownType.ContentPropertyName = "Figures";
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x001C92EB File Offset: 0x001C82EB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 447, "PathSegment", typeof(PathSegment), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x001C9310 File Offset: 0x001C8310
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PathSegmentCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 448, "PathSegmentCollection", typeof(PathSegmentCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PathSegmentCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x001C936C File Offset: 0x001C836C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PauseStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 449, "PauseStoryboard", typeof(PauseStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PauseStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x001C93C0 File Offset: 0x001C83C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Pen(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 450, "Pen", typeof(Pen), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Pen());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x001C9414 File Offset: 0x001C8414
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PerspectiveCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 451, "PerspectiveCamera", typeof(PerspectiveCamera), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PerspectiveCamera());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x001C9468 File Offset: 0x001C8468
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PixelFormat(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 452, "PixelFormat", typeof(PixelFormat), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(PixelFormat));
			wpfKnownType.TypeConverterType = typeof(PixelFormatConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x001C94CC File Offset: 0x001C84CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PixelFormatConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 453, "PixelFormatConverter", typeof(PixelFormatConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PixelFormatConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x001C9520 File Offset: 0x001C8520
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PngBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 454, "PngBitmapDecoder", typeof(PngBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x001C9544 File Offset: 0x001C8544
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PngBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 455, "PngBitmapEncoder", typeof(PngBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PngBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x001C9598 File Offset: 0x001C8598
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 456, "Point", typeof(Point), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Point));
			wpfKnownType.TypeConverterType = typeof(PointConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x001C95FC File Offset: 0x001C85FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 457, "Point3D", typeof(Point3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Point3D));
			wpfKnownType.TypeConverterType = typeof(Point3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x001C9660 File Offset: 0x001C8660
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 458, "Point3DAnimation", typeof(Point3DAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x001C96BF File Offset: 0x001C86BF
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 459, "Point3DAnimationBase", typeof(Point3DAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x001C96F0 File Offset: 0x001C86F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 460, "Point3DAnimationUsingKeyFrames", typeof(Point3DAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x001C975C File Offset: 0x001C875C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 461, "Point3DCollection", typeof(Point3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DCollection());
			wpfKnownType.TypeConverterType = typeof(Point3DCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x001C97C8 File Offset: 0x001C87C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 462, "Point3DCollectionConverter", typeof(Point3DCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x001C981C File Offset: 0x001C881C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 463, "Point3DConverter", typeof(Point3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x001C9870 File Offset: 0x001C8870
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 464, "Point3DKeyFrame", typeof(Point3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x001C9894 File Offset: 0x001C8894
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point3DKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 465, "Point3DKeyFrameCollection", typeof(Point3DKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point3DKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x001C98F0 File Offset: 0x001C88F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point4D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 466, "Point4D", typeof(Point4D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Point4D));
			wpfKnownType.TypeConverterType = typeof(Point4DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x001C9954 File Offset: 0x001C8954
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Point4DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 467, "Point4DConverter", typeof(Point4DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Point4DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x001C99A8 File Offset: 0x001C89A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 468, "PointAnimation", typeof(PointAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x001C9A07 File Offset: 0x001C8A07
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 469, "PointAnimationBase", typeof(PointAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x001C9A38 File Offset: 0x001C8A38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 470, "PointAnimationUsingKeyFrames", typeof(PointAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x001C9AA4 File Offset: 0x001C8AA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointAnimationUsingPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 471, "PointAnimationUsingPath", typeof(PointAnimationUsingPath), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointAnimationUsingPath());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x001C9B04 File Offset: 0x001C8B04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 472, "PointCollection", typeof(PointCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointCollection());
			wpfKnownType.TypeConverterType = typeof(PointCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B1 RID: 12465 RVA: 0x001C9B70 File Offset: 0x001C8B70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 473, "PointCollectionConverter", typeof(PointCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x001C9BC4 File Offset: 0x001C8BC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 474, "PointConverter", typeof(PointConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x001C9C18 File Offset: 0x001C8C18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 475, "PointIListConverter", typeof(PointIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x001C9C6C File Offset: 0x001C8C6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 476, "PointKeyFrame", typeof(PointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x001C9C90 File Offset: 0x001C8C90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 477, "PointKeyFrameCollection", typeof(PointKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x001C9CEC File Offset: 0x001C8CEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 478, "PointLight", typeof(PointLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PointLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x001C9D40 File Offset: 0x001C8D40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PointLightBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 479, "PointLightBase", typeof(PointLightBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B8 RID: 12472 RVA: 0x001C9D64 File Offset: 0x001C8D64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PolyBezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 480, "PolyBezierSegment", typeof(PolyBezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PolyBezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030B9 RID: 12473 RVA: 0x001C9DB8 File Offset: 0x001C8DB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PolyLineSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 481, "PolyLineSegment", typeof(PolyLineSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PolyLineSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x001C9E0C File Offset: 0x001C8E0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PolyQuadraticBezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 482, "PolyQuadraticBezierSegment", typeof(PolyQuadraticBezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PolyQuadraticBezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x001C9E60 File Offset: 0x001C8E60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Polygon(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 483, "Polygon", typeof(Polygon), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Polygon());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x001C9EDC File Offset: 0x001C8EDC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Polyline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 484, "Polyline", typeof(Polyline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Polyline());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x001C9F58 File Offset: 0x001C8F58
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Popup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 485, "Popup", typeof(Popup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Popup());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x001C9FDF File Offset: 0x001C8FDF
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PresentationSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 486, "PresentationSource", typeof(PresentationSource), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x001CA004 File Offset: 0x001C9004
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PriorityBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 487, "PriorityBinding", typeof(PriorityBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PriorityBinding());
			wpfKnownType.ContentPropertyName = "Bindings";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x001CA063 File Offset: 0x001C9063
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PriorityBindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 488, "PriorityBindingExpression", typeof(PriorityBindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C1 RID: 12481 RVA: 0x001CA098 File Offset: 0x001C9098
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ProgressBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 489, "ProgressBar", typeof(ProgressBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ProgressBar());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x001CA114 File Offset: 0x001C9114
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ProjectionCamera(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 490, "ProjectionCamera", typeof(ProjectionCamera), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x001CA138 File Offset: 0x001C9138
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PropertyPath(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 491, "PropertyPath", typeof(PropertyPath), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(PropertyPathConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x001CA16C File Offset: 0x001C916C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PropertyPathConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 492, "PropertyPathConverter", typeof(PropertyPathConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new PropertyPathConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C5 RID: 12485 RVA: 0x001CA1C0 File Offset: 0x001C91C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuadraticBezierSegment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 493, "QuadraticBezierSegment", typeof(QuadraticBezierSegment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuadraticBezierSegment());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C6 RID: 12486 RVA: 0x001CA214 File Offset: 0x001C9214
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Quaternion(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 494, "Quaternion", typeof(Quaternion), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Quaternion));
			wpfKnownType.TypeConverterType = typeof(QuaternionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C7 RID: 12487 RVA: 0x001CA278 File Offset: 0x001C9278
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 495, "QuaternionAnimation", typeof(QuaternionAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C8 RID: 12488 RVA: 0x001CA2D7 File Offset: 0x001C92D7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 496, "QuaternionAnimationBase", typeof(QuaternionAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030C9 RID: 12489 RVA: 0x001CA308 File Offset: 0x001C9308
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 497, "QuaternionAnimationUsingKeyFrames", typeof(QuaternionAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030CA RID: 12490 RVA: 0x001CA374 File Offset: 0x001C9374
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 498, "QuaternionConverter", typeof(QuaternionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030CB RID: 12491 RVA: 0x001CA3C8 File Offset: 0x001C93C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 499, "QuaternionKeyFrame", typeof(QuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030CC RID: 12492 RVA: 0x001CA3EC File Offset: 0x001C93EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 500, "QuaternionKeyFrameCollection", typeof(QuaternionKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x001CA448 File Offset: 0x001C9448
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_QuaternionRotation3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 501, "QuaternionRotation3D", typeof(QuaternionRotation3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new QuaternionRotation3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x001CA49C File Offset: 0x001C949C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RadialGradientBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 502, "RadialGradientBrush", typeof(RadialGradientBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RadialGradientBrush());
			wpfKnownType.ContentPropertyName = "GradientStops";
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x001CA50C File Offset: 0x001C950C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RadioButton(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 503, "RadioButton", typeof(RadioButton), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RadioButton());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x001CA594 File Offset: 0x001C9594
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RangeBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 504, "RangeBase", typeof(RangeBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x001CA5EC File Offset: 0x001C95EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 505, "Rect", typeof(Rect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Rect));
			wpfKnownType.TypeConverterType = typeof(RectConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x001CA650 File Offset: 0x001C9650
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rect3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 506, "Rect3D", typeof(Rect3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Rect3D));
			wpfKnownType.TypeConverterType = typeof(Rect3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x001CA6B4 File Offset: 0x001C96B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rect3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 507, "Rect3DConverter", typeof(Rect3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rect3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x001CA708 File Offset: 0x001C9708
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 508, "RectAnimation", typeof(RectAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x001CA767 File Offset: 0x001C9767
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 509, "RectAnimationBase", typeof(RectAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x001CA798 File Offset: 0x001C9798
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 510, "RectAnimationUsingKeyFrames", typeof(RectAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x001CA804 File Offset: 0x001C9804
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 511, "RectConverter", typeof(RectConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x001CA858 File Offset: 0x001C9858
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 512, "RectKeyFrame", typeof(RectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x001CA87C File Offset: 0x001C987C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 513, "RectKeyFrameCollection", typeof(RectKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x001CA8D8 File Offset: 0x001C98D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rectangle(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 514, "Rectangle", typeof(Rectangle), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rectangle());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x001CA954 File Offset: 0x001C9954
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RectangleGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 515, "RectangleGeometry", typeof(RectangleGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RectangleGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x001CA9B8 File Offset: 0x001C99B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RelativeSource(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 516, "RelativeSource", typeof(RelativeSource), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RelativeSource());
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(RelativeSourceMode));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new RelativeSource((RelativeSourceMode)arguments[0])));
			Dictionary<int, Baml6ConstructorInfo> constructors2 = wpfKnownType.Constructors;
			int key2 = 3;
			List<Type> list2 = new List<Type>();
			list2.Add(typeof(RelativeSourceMode));
			list2.Add(typeof(Type));
			list2.Add(typeof(int));
			constructors2.Add(key2, new Baml6ConstructorInfo(list2, (object[] arguments) => new RelativeSource((RelativeSourceMode)arguments[0], (Type)arguments[1], (int)arguments[2])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x001CAAB8 File Offset: 0x001C9AB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RemoveStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 517, "RemoveStoryboard", typeof(RemoveStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RemoveStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x001CAB0C File Offset: 0x001C9B0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RenderOptions(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 518, "RenderOptions", typeof(RenderOptions), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x001CAB30 File Offset: 0x001C9B30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RenderTargetBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 519, "RenderTargetBitmap", typeof(RenderTargetBitmap), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x001CAB64 File Offset: 0x001C9B64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RepeatBehavior(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 520, "RepeatBehavior", typeof(RepeatBehavior), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(RepeatBehavior));
			wpfKnownType.TypeConverterType = typeof(RepeatBehaviorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x001CABC8 File Offset: 0x001C9BC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RepeatBehaviorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 521, "RepeatBehaviorConverter", typeof(RepeatBehaviorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RepeatBehaviorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x001CAC1C File Offset: 0x001C9C1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RepeatButton(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 522, "RepeatButton", typeof(RepeatButton), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RepeatButton());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x001CACA4 File Offset: 0x001C9CA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResizeGrip(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 523, "ResizeGrip", typeof(ResizeGrip), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ResizeGrip());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x001CAD20 File Offset: 0x001C9D20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResourceDictionary(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 524, "ResourceDictionary", typeof(ResourceDictionary), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ResourceDictionary());
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.CollectionKind = XamlCollectionKind.Dictionary;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x001CAD82 File Offset: 0x001C9D82
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResourceKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 525, "ResourceKey", typeof(ResourceKey), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x001CADA8 File Offset: 0x001C9DA8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ResumeStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 526, "ResumeStoryboard", typeof(ResumeStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ResumeStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x001CADFC File Offset: 0x001C9DFC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RichTextBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 527, "RichTextBox", typeof(RichTextBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RichTextBox());
			wpfKnownType.ContentPropertyName = "Document";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x001CAE84 File Offset: 0x001C9E84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RotateTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 528, "RotateTransform", typeof(RotateTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RotateTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x001CAEE8 File Offset: 0x001C9EE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RotateTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 529, "RotateTransform3D", typeof(RotateTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RotateTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x001CAF3C File Offset: 0x001C9F3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 530, "Rotation3D", typeof(Rotation3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x001CAF60 File Offset: 0x001C9F60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 531, "Rotation3DAnimation", typeof(Rotation3DAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rotation3DAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x001CAFBF File Offset: 0x001C9FBF
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 532, "Rotation3DAnimationBase", typeof(Rotation3DAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x001CAFF0 File Offset: 0x001C9FF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 533, "Rotation3DAnimationUsingKeyFrames", typeof(Rotation3DAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rotation3DAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x001CB05A File Offset: 0x001CA05A
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 534, "Rotation3DKeyFrame", typeof(Rotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x001CB080 File Offset: 0x001CA080
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Rotation3DKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 535, "Rotation3DKeyFrameCollection", typeof(Rotation3DKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Rotation3DKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x001CB0DC File Offset: 0x001CA0DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedCommand(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 536, "RoutedCommand", typeof(RoutedCommand), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RoutedCommand());
			wpfKnownType.TypeConverterType = typeof(CommandConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x001CB140 File Offset: 0x001CA140
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedEvent(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 537, "RoutedEvent", typeof(RoutedEvent), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(RoutedEventConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x001CB174 File Offset: 0x001CA174
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedEventConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 538, "RoutedEventConverter", typeof(RoutedEventConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RoutedEventConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x001CB1C8 File Offset: 0x001CA1C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutedUICommand(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 539, "RoutedUICommand", typeof(RoutedUICommand), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RoutedUICommand());
			wpfKnownType.TypeConverterType = typeof(CommandConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x001CB22C File Offset: 0x001CA22C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RoutingStrategy(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 540, "RoutingStrategy", typeof(RoutingStrategy), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => RoutingStrategy.Tunnel);
			wpfKnownType.TypeConverterType = typeof(RoutingStrategy);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x001CB290 File Offset: 0x001CA290
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RowDefinition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 541, "RowDefinition", typeof(RowDefinition), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new RowDefinition());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x001CB304 File Offset: 0x001CA304
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Run(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 542, "Run", typeof(Run), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Run());
			wpfKnownType.ContentPropertyName = "Text";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x001CB380 File Offset: 0x001CA380
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RuntimeNamePropertyAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 543, "RuntimeNamePropertyAttribute", typeof(RuntimeNamePropertyAttribute), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x001CB3A4 File Offset: 0x001CA3A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SByte(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 544, "SByte", typeof(sbyte), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(SByteConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x001CB408 File Offset: 0x001CA408
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SByteConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 545, "SByteConverter", typeof(SByteConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SByteConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x001CB45C File Offset: 0x001CA45C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScaleTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 546, "ScaleTransform", typeof(ScaleTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScaleTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x001CB4C0 File Offset: 0x001CA4C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScaleTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 547, "ScaleTransform3D", typeof(ScaleTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScaleTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x001CB514 File Offset: 0x001CA514
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 548, "ScrollBar", typeof(ScrollBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScrollBar());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x001CB590 File Offset: 0x001CA590
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollContentPresenter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 549, "ScrollContentPresenter", typeof(ScrollContentPresenter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScrollContentPresenter());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x001CB60C File Offset: 0x001CA60C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollViewer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 550, "ScrollViewer", typeof(ScrollViewer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ScrollViewer());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x001CB694 File Offset: 0x001CA694
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Section(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 551, "Section", typeof(Section), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Section());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x001CB710 File Offset: 0x001CA710
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SeekStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 552, "SeekStoryboard", typeof(SeekStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SeekStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x001CB764 File Offset: 0x001CA764
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Selector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 553, "Selector", typeof(Selector), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x001CB7C8 File Offset: 0x001CA7C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Separator(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 554, "Separator", typeof(Separator), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Separator());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x001CB844 File Offset: 0x001CA844
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SetStoryboardSpeedRatio(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 555, "SetStoryboardSpeedRatio", typeof(SetStoryboardSpeedRatio), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SetStoryboardSpeedRatio());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x001CB898 File Offset: 0x001CA898
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Setter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 556, "Setter", typeof(Setter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Setter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x001CB8EC File Offset: 0x001CA8EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SetterBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 557, "SetterBase", typeof(SetterBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x001CB910 File Offset: 0x001CA910
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Shape(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 558, "Shape", typeof(Shape), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x001CB968 File Offset: 0x001CA968
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Single(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 559, "Single", typeof(float), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0f);
			wpfKnownType.TypeConverterType = typeof(SingleConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x001CB9CC File Offset: 0x001CA9CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 560, "SingleAnimation", typeof(SingleAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x001CBA2B File Offset: 0x001CAA2B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 561, "SingleAnimationBase", typeof(SingleAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x001CBA5C File Offset: 0x001CAA5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 562, "SingleAnimationUsingKeyFrames", typeof(SingleAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x001CBAC8 File Offset: 0x001CAAC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 563, "SingleConverter", typeof(SingleConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x001CBB1C File Offset: 0x001CAB1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 564, "SingleKeyFrame", typeof(SingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x001CBB40 File Offset: 0x001CAB40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SingleKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 565, "SingleKeyFrameCollection", typeof(SingleKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SingleKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x001CBB9C File Offset: 0x001CAB9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Size(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 566, "Size", typeof(Size), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Size));
			wpfKnownType.TypeConverterType = typeof(SizeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x001CBC00 File Offset: 0x001CAC00
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Size3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 567, "Size3D", typeof(Size3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Size3D));
			wpfKnownType.TypeConverterType = typeof(Size3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x001CBC64 File Offset: 0x001CAC64
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Size3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 568, "Size3DConverter", typeof(Size3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Size3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x001CBCB8 File Offset: 0x001CACB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 569, "SizeAnimation", typeof(SizeAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x001CBD17 File Offset: 0x001CAD17
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 570, "SizeAnimationBase", typeof(SizeAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x001CBD48 File Offset: 0x001CAD48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 571, "SizeAnimationUsingKeyFrames", typeof(SizeAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x001CBDB4 File Offset: 0x001CADB4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 572, "SizeConverter", typeof(SizeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x001CBE08 File Offset: 0x001CAE08
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 573, "SizeKeyFrame", typeof(SizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x001CBE2C File Offset: 0x001CAE2C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SizeKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 574, "SizeKeyFrameCollection", typeof(SizeKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SizeKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x001CBE88 File Offset: 0x001CAE88
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SkewTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 575, "SkewTransform", typeof(SkewTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SkewTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x001CBEEC File Offset: 0x001CAEEC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SkipStoryboardToFill(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 576, "SkipStoryboardToFill", typeof(SkipStoryboardToFill), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SkipStoryboardToFill());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x001CBF40 File Offset: 0x001CAF40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Slider(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 577, "Slider", typeof(Slider), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Slider());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x001CBFBC File Offset: 0x001CAFBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SolidColorBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 578, "SolidColorBrush", typeof(SolidColorBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SolidColorBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x001CC020 File Offset: 0x001CB020
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SoundPlayerAction(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 579, "SoundPlayerAction", typeof(SoundPlayerAction), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SoundPlayerAction());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x001CC074 File Offset: 0x001CB074
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Span(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 580, "Span", typeof(Span), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Span());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x001CC0F0 File Offset: 0x001CB0F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SpecularMaterial(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 581, "SpecularMaterial", typeof(SpecularMaterial), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SpecularMaterial());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x001CC144 File Offset: 0x001CB144
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SpellCheck(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 582, "SpellCheck", typeof(SpellCheck), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x001CC168 File Offset: 0x001CB168
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineByteKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 583, "SplineByteKeyFrame", typeof(SplineByteKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineByteKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x001CC1BC File Offset: 0x001CB1BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineColorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 584, "SplineColorKeyFrame", typeof(SplineColorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineColorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x001CC210 File Offset: 0x001CB210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineDecimalKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 585, "SplineDecimalKeyFrame", typeof(SplineDecimalKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineDecimalKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x001CC264 File Offset: 0x001CB264
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineDoubleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 586, "SplineDoubleKeyFrame", typeof(SplineDoubleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineDoubleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x001CC2B8 File Offset: 0x001CB2B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineInt16KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 587, "SplineInt16KeyFrame", typeof(SplineInt16KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineInt16KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x001CC30C File Offset: 0x001CB30C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineInt32KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 588, "SplineInt32KeyFrame", typeof(SplineInt32KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineInt32KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x001CC360 File Offset: 0x001CB360
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineInt64KeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 589, "SplineInt64KeyFrame", typeof(SplineInt64KeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineInt64KeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x001CC3B4 File Offset: 0x001CB3B4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplinePoint3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 590, "SplinePoint3DKeyFrame", typeof(SplinePoint3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplinePoint3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x001CC408 File Offset: 0x001CB408
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplinePointKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 591, "SplinePointKeyFrame", typeof(SplinePointKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplinePointKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x001CC45C File Offset: 0x001CB45C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineQuaternionKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 592, "SplineQuaternionKeyFrame", typeof(SplineQuaternionKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineQuaternionKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x001CC4B0 File Offset: 0x001CB4B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineRectKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 593, "SplineRectKeyFrame", typeof(SplineRectKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineRectKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x001CC504 File Offset: 0x001CB504
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineRotation3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 594, "SplineRotation3DKeyFrame", typeof(SplineRotation3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineRotation3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x001CC558 File Offset: 0x001CB558
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineSingleKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 595, "SplineSingleKeyFrame", typeof(SplineSingleKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineSingleKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x001CC5AC File Offset: 0x001CB5AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineSizeKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 596, "SplineSizeKeyFrame", typeof(SplineSizeKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineSizeKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x001CC600 File Offset: 0x001CB600
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 597, "SplineThicknessKeyFrame", typeof(SplineThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineThicknessKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x001CC654 File Offset: 0x001CB654
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineVector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 598, "SplineVector3DKeyFrame", typeof(SplineVector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineVector3DKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x001CC6A8 File Offset: 0x001CB6A8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SplineVectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 599, "SplineVectorKeyFrame", typeof(SplineVectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SplineVectorKeyFrame());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x001CC6FC File Offset: 0x001CB6FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SpotLight(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 600, "SpotLight", typeof(SpotLight), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SpotLight());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x001CC750 File Offset: 0x001CB750
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StackPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 601, "StackPanel", typeof(StackPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StackPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x001CC7D8 File Offset: 0x001CB7D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StaticExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 602, "StaticExtension", typeof(System.Windows.Markup.StaticExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new MS.Internal.Markup.StaticExtension());
			wpfKnownType.HasSpecialValueConverter = true;
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(string));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new MS.Internal.Markup.StaticExtension((string)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x001CC878 File Offset: 0x001CB878
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StaticResourceExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 603, "StaticResourceExtension", typeof(StaticResourceExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StaticResourceExtension());
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(object));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new StaticResourceExtension(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x001CC914 File Offset: 0x001CB914
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StatusBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 604, "StatusBar", typeof(StatusBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StatusBar());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x001CC99C File Offset: 0x001CB99C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StatusBarItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 605, "StatusBarItem", typeof(StatusBarItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StatusBarItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x001CCA24 File Offset: 0x001CBA24
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StickyNoteControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 606, "StickyNoteControl", typeof(StickyNoteControl), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x001CCA7C File Offset: 0x001CBA7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StopStoryboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 607, "StopStoryboard", typeof(StopStoryboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StopStoryboard());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x001CCAD0 File Offset: 0x001CBAD0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Storyboard(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 608, "Storyboard", typeof(Storyboard), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Storyboard());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x001CCB3C File Offset: 0x001CBB3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StreamGeometry(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 609, "StreamGeometry", typeof(StreamGeometry), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StreamGeometry());
			wpfKnownType.TypeConverterType = typeof(GeometryConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x001CCBA0 File Offset: 0x001CBBA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StreamGeometryContext(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 610, "StreamGeometryContext", typeof(StreamGeometryContext), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x001CCBC4 File Offset: 0x001CBBC4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StreamResourceInfo(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 611, "StreamResourceInfo", typeof(StreamResourceInfo), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StreamResourceInfo());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x001CCC18 File Offset: 0x001CBC18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_String(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 612, "String", typeof(string), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(StringConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x001CCC4C File Offset: 0x001CBC4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 613, "StringAnimationBase", typeof(StringAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x001CCC7C File Offset: 0x001CBC7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 614, "StringAnimationUsingKeyFrames", typeof(StringAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StringAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x001CCCE8 File Offset: 0x001CBCE8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 615, "StringConverter", typeof(StringConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StringConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x001CCD3C File Offset: 0x001CBD3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 616, "StringKeyFrame", typeof(StringKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x001CCD60 File Offset: 0x001CBD60
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StringKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 617, "StringKeyFrameCollection", typeof(StringKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StringKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x001CCDBC File Offset: 0x001CBDBC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StrokeCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 618, "StrokeCollection", typeof(StrokeCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StrokeCollection());
			wpfKnownType.TypeConverterType = typeof(StrokeCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x001CCE28 File Offset: 0x001CBE28
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StrokeCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 619, "StrokeCollectionConverter", typeof(StrokeCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new StrokeCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x001CCE7C File Offset: 0x001CBE7C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Style(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 620, "Style", typeof(Style), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Style());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.DictionaryKeyPropertyName = "TargetType";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x001CCEE6 File Offset: 0x001CBEE6
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Stylus(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 621, "Stylus", typeof(Stylus), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x001CCF0A File Offset: 0x001CBF0A
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_StylusDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 622, "StylusDevice", typeof(StylusDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x001CCF30 File Offset: 0x001CBF30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 623, "TabControl", typeof(TabControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TabControl());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x001CCFB8 File Offset: 0x001CBFB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 624, "TabItem", typeof(TabItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TabItem());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x001CD040 File Offset: 0x001CC040
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 625, "TabPanel", typeof(TabPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TabPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x001CD0C8 File Offset: 0x001CC0C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Table(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 626, "Table", typeof(Table), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Table());
			wpfKnownType.ContentPropertyName = "RowGroups";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x001CD144 File Offset: 0x001CC144
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableCell(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 627, "TableCell", typeof(TableCell), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableCell());
			wpfKnownType.ContentPropertyName = "Blocks";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x001CD1C0 File Offset: 0x001CC1C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableColumn(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 628, "TableColumn", typeof(TableColumn), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableColumn());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x001CD234 File Offset: 0x001CC234
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableRow(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 629, "TableRow", typeof(TableRow), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableRow());
			wpfKnownType.ContentPropertyName = "Cells";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x001CD2B0 File Offset: 0x001CC2B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TableRowGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 630, "TableRowGroup", typeof(TableRowGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TableRowGroup());
			wpfKnownType.ContentPropertyName = "Rows";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x001CD32C File Offset: 0x001CC32C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TabletDevice(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 631, "TabletDevice", typeof(TabletDevice), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x001CD350 File Offset: 0x001CC350
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExpression(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 632, "TemplateBindingExpression", typeof(TemplateBindingExpression), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(TemplateBindingExpressionConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x001CD384 File Offset: 0x001CC384
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExpressionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 633, "TemplateBindingExpressionConverter", typeof(TemplateBindingExpressionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateBindingExpressionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x001CD3D8 File Offset: 0x001CC3D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 634, "TemplateBindingExtension", typeof(TemplateBindingExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateBindingExtension());
			wpfKnownType.TypeConverterType = typeof(TemplateBindingExtensionConverter);
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(DependencyProperty));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new TemplateBindingExtension((DependencyProperty)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x001CD484 File Offset: 0x001CC484
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateBindingExtensionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 635, "TemplateBindingExtensionConverter", typeof(TemplateBindingExtensionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateBindingExtensionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x001CD4D8 File Offset: 0x001CC4D8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 636, "TemplateKey", typeof(TemplateKey), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(TemplateKeyConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x001CD50C File Offset: 0x001CC50C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TemplateKeyConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 637, "TemplateKeyConverter", typeof(TemplateKeyConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TemplateKeyConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x001CD560 File Offset: 0x001CC560
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextBlock(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 638, "TextBlock", typeof(TextBlock), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextBlock());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x001CD5E8 File Offset: 0x001CC5E8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextBox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 639, "TextBox", typeof(TextBox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextBox());
			wpfKnownType.ContentPropertyName = "Text";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x001CD670 File Offset: 0x001CC670
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextBoxBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 640, "TextBoxBase", typeof(TextBoxBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x001CD6C7 File Offset: 0x001CC6C7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextComposition(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 641, "TextComposition", typeof(TextComposition), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x001CD6EB File Offset: 0x001CC6EB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextCompositionManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 642, "TextCompositionManager", typeof(TextCompositionManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x001CD710 File Offset: 0x001CC710
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextDecoration(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 643, "TextDecoration", typeof(TextDecoration), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextDecoration());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x001CD764 File Offset: 0x001CC764
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextDecorationCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 644, "TextDecorationCollection", typeof(TextDecorationCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextDecorationCollection());
			wpfKnownType.TypeConverterType = typeof(TextDecorationCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x001CD7D0 File Offset: 0x001CC7D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextDecorationCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 645, "TextDecorationCollectionConverter", typeof(TextDecorationCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextDecorationCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x001CD824 File Offset: 0x001CC824
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextEffect(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 646, "TextEffect", typeof(TextEffect), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextEffect());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x001CD878 File Offset: 0x001CC878
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextEffectCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 647, "TextEffectCollection", typeof(TextEffectCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TextEffectCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x001CD8D4 File Offset: 0x001CC8D4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 648, "TextElement", typeof(TextElement), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x001CD920 File Offset: 0x001CC920
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextSearch(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 649, "TextSearch", typeof(TextSearch), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x001CD944 File Offset: 0x001CC944
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThemeDictionaryExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 650, "ThemeDictionaryExtension", typeof(ThemeDictionaryExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThemeDictionaryExtension());
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(string));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new ThemeDictionaryExtension((string)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x001CD9E0 File Offset: 0x001CC9E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Thickness(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 651, "Thickness", typeof(Thickness), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Thickness));
			wpfKnownType.TypeConverterType = typeof(ThicknessConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x001CDA44 File Offset: 0x001CCA44
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 652, "ThicknessAnimation", typeof(ThicknessAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x001CDAA3 File Offset: 0x001CCAA3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 653, "ThicknessAnimationBase", typeof(ThicknessAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x001CDAD4 File Offset: 0x001CCAD4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 654, "ThicknessAnimationUsingKeyFrames", typeof(ThicknessAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x001CDB40 File Offset: 0x001CCB40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 655, "ThicknessConverter", typeof(ThicknessConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x001CDB94 File Offset: 0x001CCB94
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 656, "ThicknessKeyFrame", typeof(ThicknessKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x001CDBB8 File Offset: 0x001CCBB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ThicknessKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 657, "ThicknessKeyFrameCollection", typeof(ThicknessKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ThicknessKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x001CDC14 File Offset: 0x001CCC14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Thumb(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 658, "Thumb", typeof(Thumb), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Thumb());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x001CDC90 File Offset: 0x001CCC90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TickBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 659, "TickBar", typeof(TickBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TickBar());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x001CDD0C File Offset: 0x001CCD0C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TiffBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 660, "TiffBitmapDecoder", typeof(TiffBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x001CDD30 File Offset: 0x001CCD30
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TiffBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 661, "TiffBitmapEncoder", typeof(TiffBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TiffBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x001CDD84 File Offset: 0x001CCD84
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TileBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 662, "TileBrush", typeof(TileBrush), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x001CDDB8 File Offset: 0x001CCDB8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimeSpan(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 663, "TimeSpan", typeof(TimeSpan), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(TimeSpan));
			wpfKnownType.TypeConverterType = typeof(TimeSpanConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x001CDE1C File Offset: 0x001CCE1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimeSpanConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 664, "TimeSpanConverter", typeof(TimeSpanConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TimeSpanConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x001CDE70 File Offset: 0x001CCE70
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Timeline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 665, "Timeline", typeof(Timeline), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x001CDEA0 File Offset: 0x001CCEA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimelineCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 666, "TimelineCollection", typeof(TimelineCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TimelineCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x001CDEFB File Offset: 0x001CCEFB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TimelineGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 667, "TimelineGroup", typeof(TimelineGroup), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x001CDF38 File Offset: 0x001CCF38
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToggleButton(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 668, "ToggleButton", typeof(ToggleButton), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToggleButton());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x001CDFC0 File Offset: 0x001CCFC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBar(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 669, "ToolBar", typeof(ToolBar), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBar());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x001CE048 File Offset: 0x001CD048
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBarOverflowPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 670, "ToolBarOverflowPanel", typeof(ToolBarOverflowPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBarOverflowPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x001CE0D0 File Offset: 0x001CD0D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBarPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 671, "ToolBarPanel", typeof(ToolBarPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBarPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x001CE158 File Offset: 0x001CD158
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolBarTray(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 672, "ToolBarTray", typeof(ToolBarTray), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolBarTray());
			wpfKnownType.ContentPropertyName = "ToolBars";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x001CE1E0 File Offset: 0x001CD1E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolTip(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 673, "ToolTip", typeof(ToolTip), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ToolTip());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600317A RID: 12666 RVA: 0x001CE267 File Offset: 0x001CD267
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ToolTipService(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 674, "ToolTipService", typeof(ToolTipService), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x001CE28C File Offset: 0x001CD28C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Track(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 675, "Track", typeof(Track), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Track());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x001CE308 File Offset: 0x001CD308
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 676, "Transform", typeof(Transform), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x001CE33C File Offset: 0x001CD33C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 677, "Transform3D", typeof(Transform3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600317E RID: 12670 RVA: 0x001CE360 File Offset: 0x001CD360
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 678, "Transform3DCollection", typeof(Transform3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Transform3DCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600317F RID: 12671 RVA: 0x001CE3BC File Offset: 0x001CD3BC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Transform3DGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 679, "Transform3DGroup", typeof(Transform3DGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Transform3DGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003180 RID: 12672 RVA: 0x001CE41C File Offset: 0x001CD41C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 680, "TransformCollection", typeof(TransformCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003181 RID: 12673 RVA: 0x001CE478 File Offset: 0x001CD478
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 681, "TransformConverter", typeof(TransformConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x001CE4CC File Offset: 0x001CD4CC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformGroup(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 682, "TransformGroup", typeof(TransformGroup), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformGroup());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003183 RID: 12675 RVA: 0x001CE53C File Offset: 0x001CD53C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TransformedBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 683, "TransformedBitmap", typeof(TransformedBitmap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TransformedBitmap());
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x001CE5A0 File Offset: 0x001CD5A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TranslateTransform(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 684, "TranslateTransform", typeof(TranslateTransform), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TranslateTransform());
			wpfKnownType.TypeConverterType = typeof(TransformConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x001CE604 File Offset: 0x001CD604
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TranslateTransform3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 685, "TranslateTransform3D", typeof(TranslateTransform3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TranslateTransform3D());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x001CE658 File Offset: 0x001CD658
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TreeView(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 686, "TreeView", typeof(TreeView), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TreeView());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x001CE6E0 File Offset: 0x001CD6E0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TreeViewItem(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 687, "TreeViewItem", typeof(TreeViewItem), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TreeViewItem());
			wpfKnownType.ContentPropertyName = "Items";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x001CE768 File Offset: 0x001CD768
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Trigger(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 688, "Trigger", typeof(Trigger), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Trigger());
			wpfKnownType.ContentPropertyName = "Setters";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003189 RID: 12681 RVA: 0x001CE7C7 File Offset: 0x001CD7C7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TriggerAction(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 689, "TriggerAction", typeof(TriggerAction), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600318A RID: 12682 RVA: 0x001CE7EB File Offset: 0x001CD7EB
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TriggerBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 690, "TriggerBase", typeof(TriggerBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600318B RID: 12683 RVA: 0x001CE810 File Offset: 0x001CD810
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TypeExtension(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 691, "TypeExtension", typeof(TypeExtension), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TypeExtension());
			wpfKnownType.HasSpecialValueConverter = true;
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(Type));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new TypeExtension((Type)arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x001CE8B0 File Offset: 0x001CD8B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TypeTypeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 692, "TypeTypeConverter", typeof(TypeTypeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new TypeTypeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x001CE904 File Offset: 0x001CD904
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Typography(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 693, "Typography", typeof(Typography), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x001CE928 File Offset: 0x001CD928
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UIElement(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 694, "UIElement", typeof(UIElement), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UIElement());
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x001CE988 File Offset: 0x001CD988
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt16(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 695, "UInt16", typeof(ushort), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0);
			wpfKnownType.TypeConverterType = typeof(UInt16Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x001CE9EC File Offset: 0x001CD9EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt16Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 696, "UInt16Converter", typeof(UInt16Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UInt16Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x001CEA40 File Offset: 0x001CDA40
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt32(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 697, "UInt32", typeof(uint), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0U);
			wpfKnownType.TypeConverterType = typeof(UInt32Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x001CEAA4 File Offset: 0x001CDAA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt32Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 698, "UInt32Converter", typeof(UInt32Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UInt32Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003193 RID: 12691 RVA: 0x001CEAF8 File Offset: 0x001CDAF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt64(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 699, "UInt64", typeof(ulong), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => 0UL);
			wpfKnownType.TypeConverterType = typeof(UInt64Converter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003194 RID: 12692 RVA: 0x001CEB5C File Offset: 0x001CDB5C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UInt64Converter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 700, "UInt64Converter", typeof(UInt64Converter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UInt64Converter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x001CEBB0 File Offset: 0x001CDBB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UShortIListConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 701, "UShortIListConverter", typeof(UShortIListConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UShortIListConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x001CEC04 File Offset: 0x001CDC04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Underline(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 702, "Underline", typeof(Underline), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Underline());
			wpfKnownType.ContentPropertyName = "Inlines";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x001CEC80 File Offset: 0x001CDC80
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UniformGrid(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 703, "UniformGrid", typeof(UniformGrid), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UniformGrid());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x001CED07 File Offset: 0x001CDD07
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Uri(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 704, "Uri", typeof(Uri), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(UriTypeConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x001CED3C File Offset: 0x001CDD3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UriTypeConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 705, "UriTypeConverter", typeof(UriTypeConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UriTypeConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600319A RID: 12698 RVA: 0x001CED90 File Offset: 0x001CDD90
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UserControl(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 706, "UserControl", typeof(UserControl), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new UserControl());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x001CEE17 File Offset: 0x001CDE17
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Validation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 707, "Validation", typeof(Validation), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x001CEE3C File Offset: 0x001CDE3C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 708, "Vector", typeof(Vector), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Vector));
			wpfKnownType.TypeConverterType = typeof(VectorConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x001CEEA0 File Offset: 0x001CDEA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 709, "Vector3D", typeof(Vector3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => default(Vector3D));
			wpfKnownType.TypeConverterType = typeof(Vector3DConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x001CEF04 File Offset: 0x001CDF04
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 710, "Vector3DAnimation", typeof(Vector3DAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x001CEF63 File Offset: 0x001CDF63
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 711, "Vector3DAnimationBase", typeof(Vector3DAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x001CEF94 File Offset: 0x001CDF94
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 712, "Vector3DAnimationUsingKeyFrames", typeof(Vector3DAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x001CF000 File Offset: 0x001CE000
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 713, "Vector3DCollection", typeof(Vector3DCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DCollection());
			wpfKnownType.TypeConverterType = typeof(Vector3DCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x001CF06C File Offset: 0x001CE06C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 714, "Vector3DCollectionConverter", typeof(Vector3DCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x001CF0C0 File Offset: 0x001CE0C0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 715, "Vector3DConverter", typeof(Vector3DConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x001CF114 File Offset: 0x001CE114
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 716, "Vector3DKeyFrame", typeof(Vector3DKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x001CF138 File Offset: 0x001CE138
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Vector3DKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 717, "Vector3DKeyFrameCollection", typeof(Vector3DKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Vector3DKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x001CF194 File Offset: 0x001CE194
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorAnimation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 718, "VectorAnimation", typeof(VectorAnimation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorAnimation());
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x001CF1F3 File Offset: 0x001CE1F3
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorAnimationBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 719, "VectorAnimationBase", typeof(VectorAnimationBase), isBamlType, useV3Rules);
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x001CF224 File Offset: 0x001CE224
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorAnimationUsingKeyFrames(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 720, "VectorAnimationUsingKeyFrames", typeof(VectorAnimationUsingKeyFrames), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorAnimationUsingKeyFrames());
			wpfKnownType.ContentPropertyName = "KeyFrames";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031A9 RID: 12713 RVA: 0x001CF290 File Offset: 0x001CE290
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 721, "VectorCollection", typeof(VectorCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorCollection());
			wpfKnownType.TypeConverterType = typeof(VectorCollectionConverter);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031AA RID: 12714 RVA: 0x001CF2FC File Offset: 0x001CE2FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorCollectionConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 722, "VectorCollectionConverter", typeof(VectorCollectionConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorCollectionConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031AB RID: 12715 RVA: 0x001CF350 File Offset: 0x001CE350
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 723, "VectorConverter", typeof(VectorConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031AC RID: 12716 RVA: 0x001CF3A4 File Offset: 0x001CE3A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorKeyFrame(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 724, "VectorKeyFrame", typeof(VectorKeyFrame), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x001CF3C8 File Offset: 0x001CE3C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VectorKeyFrameCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 725, "VectorKeyFrameCollection", typeof(VectorKeyFrameCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VectorKeyFrameCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x001CF424 File Offset: 0x001CE424
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VideoDrawing(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 726, "VideoDrawing", typeof(VideoDrawing), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VideoDrawing());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x001CF478 File Offset: 0x001CE478
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ViewBase(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 727, "ViewBase", typeof(ViewBase), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x001CF49C File Offset: 0x001CE49C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Viewbox(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 728, "Viewbox", typeof(Viewbox), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Viewbox());
			wpfKnownType.ContentPropertyName = "Child";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x001CF524 File Offset: 0x001CE524
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Viewport3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 729, "Viewport3D", typeof(Viewport3D), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Viewport3D());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x001CF5AC File Offset: 0x001CE5AC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Viewport3DVisual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 730, "Viewport3DVisual", typeof(Viewport3DVisual), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Viewport3DVisual());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x001CF60C File Offset: 0x001CE60C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VirtualizingPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 731, "VirtualizingPanel", typeof(VirtualizingPanel), isBamlType, useV3Rules);
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x001CF670 File Offset: 0x001CE670
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VirtualizingStackPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 732, "VirtualizingStackPanel", typeof(VirtualizingStackPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VirtualizingStackPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x001CF6F7 File Offset: 0x001CE6F7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Visual(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 733, "Visual", typeof(Visual), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x001CF71B File Offset: 0x001CE71B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Visual3D(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 734, "Visual3D", typeof(Visual3D), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x001CF740 File Offset: 0x001CE740
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VisualBrush(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 735, "VisualBrush", typeof(VisualBrush), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new VisualBrush());
			wpfKnownType.TypeConverterType = typeof(BrushConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B8 RID: 12728 RVA: 0x001CF7A4 File Offset: 0x001CE7A4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_VisualTarget(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 736, "VisualTarget", typeof(VisualTarget), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x001CF7C8 File Offset: 0x001CE7C8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WeakEventManager(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 737, "WeakEventManager", typeof(WeakEventManager), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031BA RID: 12730 RVA: 0x001CF7EC File Offset: 0x001CE7EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WhitespaceSignificantCollectionAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 738, "WhitespaceSignificantCollectionAttribute", typeof(WhitespaceSignificantCollectionAttribute), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new WhitespaceSignificantCollectionAttribute());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031BB RID: 12731 RVA: 0x001CF840 File Offset: 0x001CE840
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Window(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 739, "Window", typeof(Window), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new Window());
			wpfKnownType.ContentPropertyName = "Content";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x001CF8C7 File Offset: 0x001CE8C7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WmpBitmapDecoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 740, "WmpBitmapDecoder", typeof(WmpBitmapDecoder), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031BD RID: 12733 RVA: 0x001CF8EC File Offset: 0x001CE8EC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WmpBitmapEncoder(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 741, "WmpBitmapEncoder", typeof(WmpBitmapEncoder), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new WmpBitmapEncoder());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x001CF940 File Offset: 0x001CE940
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WrapPanel(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 742, "WrapPanel", typeof(WrapPanel), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new WrapPanel());
			wpfKnownType.ContentPropertyName = "Children";
			wpfKnownType.RuntimeNamePropertyName = "Name";
			wpfKnownType.XmlLangPropertyName = "Language";
			wpfKnownType.UidPropertyName = "Uid";
			wpfKnownType.IsUsableDuringInit = true;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x001CF9C7 File Offset: 0x001CE9C7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_WriteableBitmap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 743, "WriteableBitmap", typeof(WriteableBitmap), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(ImageSourceConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x001CF9FC File Offset: 0x001CE9FC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlBrushSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 744, "XamlBrushSerializer", typeof(XamlBrushSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlBrushSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x001CFA50 File Offset: 0x001CEA50
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlInt32CollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 745, "XamlInt32CollectionSerializer", typeof(XamlInt32CollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlInt32CollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x001CFAA4 File Offset: 0x001CEAA4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlPathDataSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 746, "XamlPathDataSerializer", typeof(XamlPathDataSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlPathDataSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x001CFAF8 File Offset: 0x001CEAF8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlPoint3DCollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 747, "XamlPoint3DCollectionSerializer", typeof(XamlPoint3DCollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlPoint3DCollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x001CFB4C File Offset: 0x001CEB4C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlPointCollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 748, "XamlPointCollectionSerializer", typeof(XamlPointCollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlPointCollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x001CFBA0 File Offset: 0x001CEBA0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlReader(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 749, "XamlReader", typeof(System.Windows.Markup.XamlReader), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new System.Windows.Markup.XamlReader());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x001CFBF4 File Offset: 0x001CEBF4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlStyleSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 750, "XamlStyleSerializer", typeof(XamlStyleSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlStyleSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x001CFC48 File Offset: 0x001CEC48
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlTemplateSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 751, "XamlTemplateSerializer", typeof(XamlTemplateSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlTemplateSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x001CFC9C File Offset: 0x001CEC9C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlVector3DCollectionSerializer(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 752, "XamlVector3DCollectionSerializer", typeof(XamlVector3DCollectionSerializer), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XamlVector3DCollectionSerializer());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x001CFCF0 File Offset: 0x001CECF0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XamlWriter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 753, "XamlWriter", typeof(System.Windows.Markup.XamlWriter), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x001CFD14 File Offset: 0x001CED14
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlDataProvider(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 754, "XmlDataProvider", typeof(XmlDataProvider), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlDataProvider());
			wpfKnownType.ContentPropertyName = "XmlSerializer";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x001CFD73 File Offset: 0x001CED73
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlLangPropertyAttribute(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 755, "XmlLangPropertyAttribute", typeof(XmlLangPropertyAttribute), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x001CFD97 File Offset: 0x001CED97
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlLanguage(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 756, "XmlLanguage", typeof(XmlLanguage), isBamlType, useV3Rules);
			wpfKnownType.TypeConverterType = typeof(XmlLanguageConverter);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x001CFDCC File Offset: 0x001CEDCC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlLanguageConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 757, "XmlLanguageConverter", typeof(XmlLanguageConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlLanguageConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x001CFE20 File Offset: 0x001CEE20
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlNamespaceMapping(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 758, "XmlNamespaceMapping", typeof(XmlNamespaceMapping), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlNamespaceMapping());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x001CFE74 File Offset: 0x001CEE74
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ZoomPercentageConverter(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 759, "ZoomPercentageConverter", typeof(ZoomPercentageConverter), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ZoomPercentageConverter());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x001CFEC8 File Offset: 0x001CEEC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CommandBinding(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "CommandBinding", typeof(CommandBinding), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CommandBinding());
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x001CFF18 File Offset: 0x001CEF18
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_XmlNamespaceMappingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "XmlNamespaceMappingCollection", typeof(XmlNamespaceMappingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new XmlNamespaceMappingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x001CFF6F File Offset: 0x001CEF6F
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PageContentCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "PageContentCollection", typeof(PageContentCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x001CFF96 File Offset: 0x001CEF96
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_DocumentReferenceCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "DocumentReferenceCollection", typeof(DocumentReferenceCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x001CFFC0 File Offset: 0x001CEFC0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyboardNavigationMode(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "KeyboardNavigationMode", typeof(KeyboardNavigationMode), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => KeyboardNavigationMode.Continue);
			wpfKnownType.TypeConverterType = typeof(KeyboardNavigationMode);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x001D0020 File Offset: 0x001CF020
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Enum(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "Enum", typeof(Enum), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x001D0040 File Offset: 0x001CF040
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RelativeSourceMode(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "RelativeSourceMode", typeof(RelativeSourceMode), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => RelativeSourceMode.PreviousData);
			wpfKnownType.TypeConverterType = typeof(RelativeSourceMode);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x001D00A0 File Offset: 0x001CF0A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PenLineJoin(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "PenLineJoin", typeof(PenLineJoin), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => PenLineJoin.Miter);
			wpfKnownType.TypeConverterType = typeof(PenLineJoin);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x001D0100 File Offset: 0x001CF100
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_PenLineCap(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "PenLineCap", typeof(PenLineCap), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => PenLineCap.Flat);
			wpfKnownType.TypeConverterType = typeof(PenLineCap);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x001D0160 File Offset: 0x001CF160
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_InputBindingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "InputBindingCollection", typeof(InputBindingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new InputBindingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x001D01B8 File Offset: 0x001CF1B8
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_CommandBindingCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "CommandBindingCollection", typeof(CommandBindingCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new CommandBindingCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x001D0210 File Offset: 0x001CF210
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Stretch(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "Stretch", typeof(Stretch), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => Stretch.None);
			wpfKnownType.TypeConverterType = typeof(Stretch);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x001D0270 File Offset: 0x001CF270
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_Orientation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "Orientation", typeof(Orientation), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => Orientation.Horizontal);
			wpfKnownType.TypeConverterType = typeof(Orientation);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x001D02D0 File Offset: 0x001CF2D0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TextAlignment(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "TextAlignment", typeof(TextAlignment), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => TextAlignment.Left);
			wpfKnownType.TypeConverterType = typeof(TextAlignment);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x001D0330 File Offset: 0x001CF330
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_NavigationUIVisibility(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "NavigationUIVisibility", typeof(NavigationUIVisibility), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => NavigationUIVisibility.Automatic);
			wpfKnownType.TypeConverterType = typeof(NavigationUIVisibility);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x001D0390 File Offset: 0x001CF390
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_JournalOwnership(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "JournalOwnership", typeof(JournalOwnership), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => JournalOwnership.Automatic);
			wpfKnownType.TypeConverterType = typeof(JournalOwnership);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x001D03F0 File Offset: 0x001CF3F0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ScrollBarVisibility(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ScrollBarVisibility", typeof(ScrollBarVisibility), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => ScrollBarVisibility.Disabled);
			wpfKnownType.TypeConverterType = typeof(ScrollBarVisibility);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x001D0450 File Offset: 0x001CF450
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_TriggerCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "TriggerCollection", typeof(TriggerCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x001D0477 File Offset: 0x001CF477
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_UIElementCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "UIElementCollection", typeof(UIElementCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x001D04A0 File Offset: 0x001CF4A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_SetterBaseCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "SetterBaseCollection", typeof(SetterBaseCollection), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new SetterBaseCollection());
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x001D04F7 File Offset: 0x001CF4F7
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ColumnDefinitionCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ColumnDefinitionCollection", typeof(ColumnDefinitionCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x001D051E File Offset: 0x001CF51E
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_RowDefinitionCollection(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "RowDefinitionCollection", typeof(RowDefinitionCollection), isBamlType, useV3Rules);
			wpfKnownType.CollectionKind = XamlCollectionKind.Collection;
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x001D0548 File Offset: 0x001CF548
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemContainerTemplate(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ItemContainerTemplate", typeof(ItemContainerTemplate), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemContainerTemplate());
			wpfKnownType.ContentPropertyName = "Template";
			wpfKnownType.DictionaryKeyPropertyName = "ItemContainerTemplateKey";
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x001D05B0 File Offset: 0x001CF5B0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_ItemContainerTemplateKey(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "ItemContainerTemplateKey", typeof(ItemContainerTemplateKey), isBamlType, useV3Rules);
			wpfKnownType.DefaultConstructor = (() => new ItemContainerTemplateKey());
			wpfKnownType.TypeConverterType = typeof(TemplateKeyConverter);
			Dictionary<int, Baml6ConstructorInfo> constructors = wpfKnownType.Constructors;
			int key = 1;
			List<Type> list = new List<Type>();
			list.Add(typeof(object));
			constructors.Add(key, new Baml6ConstructorInfo(list, (object[] arguments) => new ItemContainerTemplateKey(arguments[0])));
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x001D0655 File Offset: 0x001CF655
		[MethodImpl(MethodImplOptions.NoInlining)]
		private WpfKnownType Create_BamlType_KeyboardNavigation(bool isBamlType, bool useV3Rules)
		{
			WpfKnownType wpfKnownType = new WpfKnownType(this, 0, "KeyboardNavigation", typeof(KeyboardNavigation), isBamlType, useV3Rules);
			wpfKnownType.Freeze();
			return wpfKnownType;
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x001D0675 File Offset: 0x001CF675
		public WpfSharedBamlSchemaContext()
		{
			this.Initialize();
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x001D0683 File Offset: 0x001CF683
		public WpfSharedBamlSchemaContext(XamlSchemaContextSettings settings) : base(settings)
		{
			this.Initialize();
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x001D0694 File Offset: 0x001CF694
		private void Initialize()
		{
			this._syncObject = new object();
			this._knownBamlAssemblies = new Baml6Assembly[5];
			this._knownBamlTypes = new WpfKnownType[760];
			this._masterTypeTable = new Dictionary<Type, XamlType>(256);
			this._knownBamlMembers = new WpfKnownMember[269];
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x001D06E8 File Offset: 0x001CF6E8
		internal string GetKnownBamlString(short stringId)
		{
			string result;
			if (stringId != -2)
			{
				if (stringId == -1)
				{
					result = "Name";
				}
				else
				{
					result = null;
				}
			}
			else
			{
				result = "Uid";
			}
			return result;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x001D0714 File Offset: 0x001CF714
		internal Baml6Assembly GetKnownBamlAssembly(short assemblyId)
		{
			if (assemblyId > 0)
			{
				throw new ArgumentException(SR.Get("AssemblyIdNegative"));
			}
			assemblyId = -assemblyId;
			Baml6Assembly baml6Assembly = this._knownBamlAssemblies[(int)assemblyId];
			if (baml6Assembly == null)
			{
				baml6Assembly = this.CreateKnownBamlAssembly(assemblyId);
				this._knownBamlAssemblies[(int)assemblyId] = baml6Assembly;
			}
			return baml6Assembly;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x001D0758 File Offset: 0x001CF758
		internal Baml6Assembly CreateKnownBamlAssembly(short assemblyId)
		{
			Baml6Assembly result;
			switch (assemblyId)
			{
			case 0:
				result = new Baml6Assembly(typeof(double).Assembly);
				break;
			case 1:
				result = new Baml6Assembly(typeof(Uri).Assembly);
				break;
			case 2:
				result = new Baml6Assembly(typeof(DependencyObject).Assembly);
				break;
			case 3:
				result = new Baml6Assembly(typeof(UIElement).Assembly);
				break;
			case 4:
				result = new Baml6Assembly(typeof(FrameworkElement).Assembly);
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x001D07F8 File Offset: 0x001CF7F8
		internal WpfKnownType GetKnownBamlType(short typeId)
		{
			if (typeId >= 0)
			{
				throw new ArgumentException(SR.Get("KnownTypeIdNegative"));
			}
			typeId = -typeId;
			object syncObject = this._syncObject;
			WpfKnownType wpfKnownType;
			lock (syncObject)
			{
				wpfKnownType = this._knownBamlTypes[(int)typeId];
				if (wpfKnownType == null)
				{
					wpfKnownType = this.CreateKnownBamlType(typeId, true, true);
					this._knownBamlTypes[(int)typeId] = wpfKnownType;
					this._masterTypeTable.Add(wpfKnownType.UnderlyingType, wpfKnownType);
				}
			}
			return wpfKnownType;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x001D0884 File Offset: 0x001CF884
		internal WpfKnownMember GetKnownBamlMember(short memberId)
		{
			if (memberId >= 0)
			{
				throw new ArgumentException(SR.Get("KnownTypeIdNegative"));
			}
			memberId = -memberId;
			object syncObject = this._syncObject;
			WpfKnownMember wpfKnownMember;
			lock (syncObject)
			{
				wpfKnownMember = this._knownBamlMembers[(int)memberId];
				if (wpfKnownMember == null)
				{
					wpfKnownMember = this.CreateKnownMember(memberId);
					this._knownBamlMembers[(int)memberId] = wpfKnownMember;
				}
			}
			return wpfKnownMember;
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x001D08FC File Offset: 0x001CF8FC
		public override XamlType GetXamlType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			XamlType xamlType = this.GetKnownXamlType(type);
			if (xamlType == null)
			{
				xamlType = this.GetUnknownXamlType(type);
			}
			return xamlType;
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x001D0938 File Offset: 0x001CF938
		private XamlType GetUnknownXamlType(Type type)
		{
			object syncObject = this._syncObject;
			XamlType xamlType;
			lock (syncObject)
			{
				if (!this._masterTypeTable.TryGetValue(type, out xamlType))
				{
					WpfSharedXamlSchemaContext.RequireRuntimeType(type);
					xamlType = new WpfXamlType(type, this, true, true);
					this._masterTypeTable.Add(type, xamlType);
				}
			}
			return xamlType;
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x001D09A0 File Offset: 0x001CF9A0
		internal XamlType GetKnownXamlType(Type type)
		{
			object syncObject = this._syncObject;
			XamlType xamlType;
			lock (syncObject)
			{
				if (!this._masterTypeTable.TryGetValue(type, out xamlType))
				{
					xamlType = this.CreateKnownBamlType(type.Name, true, true);
					if (xamlType == null && this._themeHelpers != null)
					{
						foreach (ThemeKnownTypeHelper themeKnownTypeHelper in this._themeHelpers)
						{
							xamlType = themeKnownTypeHelper.GetKnownXamlType(type.Name);
							if (xamlType != null && xamlType.UnderlyingType == type)
							{
								break;
							}
						}
					}
					if (xamlType != null && xamlType.UnderlyingType == type)
					{
						WpfKnownType wpfKnownType = xamlType as WpfKnownType;
						if (wpfKnownType != null)
						{
							this._knownBamlTypes[(int)wpfKnownType.BamlNumber] = wpfKnownType;
						}
						this._masterTypeTable.Add(type, xamlType);
					}
					else
					{
						xamlType = null;
					}
				}
			}
			return xamlType;
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x001D0AB8 File Offset: 0x001CFAB8
		internal XamlValueConverter<XamlDeferringLoader> GetDeferringLoader(Type loaderType)
		{
			return base.GetValueConverter<XamlDeferringLoader>(loaderType, null);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x001D0AC2 File Offset: 0x001CFAC2
		internal XamlValueConverter<TypeConverter> GetTypeConverter(Type converterType)
		{
			if (converterType.IsEnum)
			{
				return base.GetValueConverter<TypeConverter>(typeof(EnumConverter), this.GetXamlType(converterType));
			}
			return base.GetValueConverter<TypeConverter>(converterType, null);
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x001D0AEC File Offset: 0x001CFAEC
		protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			return base.GetXamlType(xamlNamespace, name, typeArguments);
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x001D0AEC File Offset: 0x001CFAEC
		public XamlType GetXamlTypeExposed(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			return base.GetXamlType(xamlNamespace, name, typeArguments);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001D0AF8 File Offset: 0x001CFAF8
		internal Type ResolvePrefixedNameWithAdditionalWpfSemantics(string prefixedName, DependencyObject element)
		{
			XmlnsDictionary xmlnsDictionary = element.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty) as XmlnsDictionary;
			Hashtable hashtable = element.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty) as Hashtable;
			if (xmlnsDictionary == null)
			{
				if (this._wpfDefaultNamespace == null)
				{
					this._wpfDefaultNamespace = new XmlnsDictionary
					{
						{
							string.Empty,
							"http://schemas.microsoft.com/winfx/2006/xaml/presentation"
						}
					};
				}
				xmlnsDictionary = this._wpfDefaultNamespace;
			}
			else if (hashtable != null && hashtable.Count > 0)
			{
				Type typeFromName = XamlTypeMapper.GetTypeFromName(prefixedName, element);
				if (typeFromName != null)
				{
					return typeFromName;
				}
			}
			XamlTypeName xamlTypeName;
			if (XamlTypeName.TryParse(prefixedName, xmlnsDictionary, out xamlTypeName))
			{
				XamlType xamlType = base.GetXamlType(xamlTypeName);
				if (xamlType != null)
				{
					return xamlType.UnderlyingType;
				}
			}
			return null;
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060031F9 RID: 12793 RVA: 0x001D0BA1 File Offset: 0x001CFBA1
		internal XamlMember StaticExtensionMemberTypeProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._xStaticMemberProperty.Value;
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x060031FA RID: 12794 RVA: 0x001D0BAD File Offset: 0x001CFBAD
		internal XamlMember TypeExtensionTypeProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._xTypeTypeProperty.Value;
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x001D0BB9 File Offset: 0x001CFBB9
		internal XamlMember ResourceDictionaryDeferredContentProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._resourceDictionaryDefContentProperty.Value;
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x001D0BC5 File Offset: 0x001CFBC5
		internal XamlType ResourceDictionaryType
		{
			get
			{
				return WpfSharedBamlSchemaContext._resourceDictionaryType.Value;
			}
		}

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x001D0BD1 File Offset: 0x001CFBD1
		internal XamlType EventSetterType
		{
			get
			{
				return WpfSharedBamlSchemaContext._eventSetterType.Value;
			}
		}

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x060031FE RID: 12798 RVA: 0x001D0BDD File Offset: 0x001CFBDD
		internal XamlMember EventSetterEventProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._eventSetterEventProperty.Value;
			}
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x001D0BE9 File Offset: 0x001CFBE9
		internal XamlMember EventSetterHandlerProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._eventSetterHandlerProperty.Value;
			}
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x001D0BF5 File Offset: 0x001CFBF5
		internal XamlMember FrameworkTemplateTemplateProperty
		{
			get
			{
				return WpfSharedBamlSchemaContext._frameworkTemplateTemplateProperty.Value;
			}
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x001D0C01 File Offset: 0x001CFC01
		internal XamlType StaticResourceExtensionType
		{
			get
			{
				return WpfSharedBamlSchemaContext._staticResourceExtensionType.Value;
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003202 RID: 12802 RVA: 0x001D0C0D File Offset: 0x001CFC0D
		// (set) Token: 0x06003203 RID: 12803 RVA: 0x001D0C15 File Offset: 0x001CFC15
		internal Baml2006ReaderSettings Settings { get; set; }

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003204 RID: 12804 RVA: 0x001D0C1E File Offset: 0x001CFC1E
		internal List<ThemeKnownTypeHelper> ThemeKnownTypeHelpers
		{
			get
			{
				if (this._themeHelpers == null)
				{
					this._themeHelpers = new List<ThemeKnownTypeHelper>();
				}
				return this._themeHelpers;
			}
		}

		// Token: 0x04001BB6 RID: 7094
		private const int KnownPropertyCount = 268;

		// Token: 0x04001BB7 RID: 7095
		private const int KnownTypeCount = 759;

		// Token: 0x04001BB8 RID: 7096
		private object _syncObject;

		// Token: 0x04001BB9 RID: 7097
		private Baml6Assembly[] _knownBamlAssemblies;

		// Token: 0x04001BBA RID: 7098
		private WpfKnownType[] _knownBamlTypes;

		// Token: 0x04001BBB RID: 7099
		private WpfKnownMember[] _knownBamlMembers;

		// Token: 0x04001BBC RID: 7100
		private Dictionary<Type, XamlType> _masterTypeTable;

		// Token: 0x04001BBD RID: 7101
		private XmlnsDictionary _wpfDefaultNamespace;

		// Token: 0x04001BBE RID: 7102
		private List<ThemeKnownTypeHelper> _themeHelpers;

		// Token: 0x04001BC0 RID: 7104
		private static readonly Lazy<XamlMember> _xStaticMemberProperty = new Lazy<XamlMember>(() => XamlLanguage.Static.GetMember("MemberType"));

		// Token: 0x04001BC1 RID: 7105
		private static readonly Lazy<XamlMember> _xTypeTypeProperty = new Lazy<XamlMember>(() => XamlLanguage.Static.GetMember("Type"));

		// Token: 0x04001BC2 RID: 7106
		private static readonly Lazy<XamlMember> _resourceDictionaryDefContentProperty = new Lazy<XamlMember>(() => WpfSharedBamlSchemaContext._resourceDictionaryType.Value.GetMember("DeferrableContent"));

		// Token: 0x04001BC3 RID: 7107
		private static readonly Lazy<XamlType> _resourceDictionaryType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(ResourceDictionary)));

		// Token: 0x04001BC4 RID: 7108
		private static readonly Lazy<XamlType> _eventSetterType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(EventSetter)));

		// Token: 0x04001BC5 RID: 7109
		private static readonly Lazy<XamlMember> _eventSetterEventProperty = new Lazy<XamlMember>(() => WpfSharedBamlSchemaContext._eventSetterType.Value.GetMember("Event"));

		// Token: 0x04001BC6 RID: 7110
		private static readonly Lazy<XamlMember> _eventSetterHandlerProperty = new Lazy<XamlMember>(() => WpfSharedBamlSchemaContext._eventSetterType.Value.GetMember("Handler"));

		// Token: 0x04001BC7 RID: 7111
		private static readonly Lazy<XamlMember> _frameworkTemplateTemplateProperty = new Lazy<XamlMember>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(FrameworkTemplate)).GetMember("Template"));

		// Token: 0x04001BC8 RID: 7112
		private static readonly Lazy<XamlType> _staticResourceExtensionType = new Lazy<XamlType>(() => System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(typeof(StaticResourceExtension)));
	}
}
