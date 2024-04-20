using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using MS.Utility;

namespace System.Windows.Media.Animation
{
	// Token: 0x0200043E RID: 1086
	public class Storyboard : ParallelTimeline
	{
		// Token: 0x0600349C RID: 13468 RVA: 0x001DBA34 File Offset: 0x001DAA34
		static Storyboard()
		{
			PropertyMetadata propertyMetadata = new PropertyMetadata();
			propertyMetadata.FreezeValueCallback = new FreezeValueCallback(Storyboard.TargetFreezeValueCallback);
			Storyboard.TargetProperty = DependencyProperty.RegisterAttached("Target", typeof(DependencyObject), typeof(Storyboard), propertyMetadata);
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x001DBADF File Offset: 0x001DAADF
		protected override Freezable CreateInstanceCore()
		{
			return new Storyboard();
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x001DBAE6 File Offset: 0x001DAAE6
		public new Storyboard Clone()
		{
			return (Storyboard)base.Clone();
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x001DBAF3 File Offset: 0x001DAAF3
		public static void SetTarget(DependencyObject element, DependencyObject value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Storyboard.TargetProperty, value);
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x001DBB0F File Offset: 0x001DAB0F
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public static DependencyObject GetTarget(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (DependencyObject)element.GetValue(Storyboard.TargetProperty);
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		private static bool TargetFreezeValueCallback(DependencyObject d, DependencyProperty dp, EntryIndex entryIndex, PropertyMetadata metadata, bool isChecking)
		{
			return true;
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x001DBB2F File Offset: 0x001DAB2F
		public static void SetTargetName(DependencyObject element, string name)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			element.SetValue(Storyboard.TargetNameProperty, name);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x001DBB59 File Offset: 0x001DAB59
		public static string GetTargetName(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (string)element.GetValue(Storyboard.TargetNameProperty);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x001DBB79 File Offset: 0x001DAB79
		public static void SetTargetProperty(DependencyObject element, PropertyPath path)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			element.SetValue(Storyboard.TargetPropertyProperty, path);
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x001DBBA3 File Offset: 0x001DABA3
		public static PropertyPath GetTargetProperty(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (PropertyPath)element.GetValue(Storyboard.TargetPropertyProperty);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x001DBBC4 File Offset: 0x001DABC4
		internal static DependencyObject ResolveTargetName(string targetName, INameScope nameScope, DependencyObject element)
		{
			FrameworkElement frameworkElement = element as FrameworkElement;
			FrameworkContentElement frameworkContentElement = element as FrameworkContentElement;
			object obj;
			object obj2;
			if (frameworkElement != null)
			{
				if (nameScope != null)
				{
					obj = ((FrameworkTemplate)nameScope).FindName(targetName, frameworkElement);
					obj2 = nameScope;
				}
				else
				{
					obj = frameworkElement.FindName(targetName);
					obj2 = frameworkElement;
				}
			}
			else
			{
				if (frameworkContentElement == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NoNameScope", new object[]
					{
						targetName
					}));
				}
				obj = frameworkContentElement.FindName(targetName);
				obj2 = frameworkContentElement;
			}
			if (obj == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
				{
					targetName,
					obj2.GetType().ToString()
				}));
			}
			DependencyObject dependencyObject = obj as DependencyObject;
			if (dependencyObject == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_TargetNameNotDependencyObject", new object[]
				{
					targetName
				}));
			}
			return dependencyObject;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x001DBC88 File Offset: 0x001DAC88
		internal static BeginStoryboard ResolveBeginStoryboardName(string targetName, INameScope nameScope, FrameworkElement fe, FrameworkContentElement fce)
		{
			object obj;
			if (nameScope != null)
			{
				obj = nameScope.FindName(targetName);
				if (obj == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
					{
						targetName,
						nameScope.GetType().ToString()
					}));
				}
			}
			else if (fe != null)
			{
				obj = fe.FindName(targetName);
				if (obj == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
					{
						targetName,
						fe.GetType().ToString()
					}));
				}
			}
			else
			{
				if (fce == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NoNameScope", new object[]
					{
						targetName
					}));
				}
				obj = fce.FindName(targetName);
				if (obj == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NameNotFound", new object[]
					{
						targetName,
						fce.GetType().ToString()
					}));
				}
			}
			BeginStoryboard beginStoryboard = obj as BeginStoryboard;
			if (beginStoryboard == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_BeginStoryboardNameNotFound", new object[]
				{
					targetName
				}));
			}
			return beginStoryboard;
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x001DBD80 File Offset: 0x001DAD80
		private void ClockTreeWalkRecursive(Clock currentClock, DependencyObject containingObject, INameScope nameScope, DependencyObject parentObject, string parentObjectName, PropertyPath parentPropertyPath, HandoffBehavior handoffBehavior, HybridDictionary clockMappings, long layer)
		{
			Timeline timeline = currentClock.Timeline;
			DependencyObject dependencyObject = parentObject;
			string text = parentObjectName;
			PropertyPath propertyPath = parentPropertyPath;
			string text2 = (string)timeline.GetValue(Storyboard.TargetNameProperty);
			if (text2 != null)
			{
				if (nameScope is Style)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_TargetNameNotAllowedInStyle", new object[]
					{
						text2
					}));
				}
				text = text2;
			}
			DependencyObject dependencyObject2 = (DependencyObject)timeline.GetValue(Storyboard.TargetProperty);
			if (dependencyObject2 != null)
			{
				dependencyObject = dependencyObject2;
				text = null;
			}
			PropertyPath propertyPath2 = (PropertyPath)timeline.GetValue(Storyboard.TargetPropertyProperty);
			if (propertyPath2 != null)
			{
				propertyPath = propertyPath2;
			}
			if (currentClock is AnimationClock)
			{
				AnimationClock animationClock = (AnimationClock)currentClock;
				if (dependencyObject == null)
				{
					if (text != null)
					{
						DependencyObject element = Helper.FindMentor(containingObject);
						dependencyObject = Storyboard.ResolveTargetName(text, nameScope, element);
					}
					else
					{
						dependencyObject = (containingObject as FrameworkElement);
						if (dependencyObject == null)
						{
							dependencyObject = (containingObject as FrameworkContentElement);
						}
						if (dependencyObject == null)
						{
							throw new InvalidOperationException(SR.Get("Storyboard_NoTarget", new object[]
							{
								timeline.GetType().ToString()
							}));
						}
					}
				}
				if (propertyPath == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_TargetPropertyRequired", new object[]
					{
						timeline.GetType().ToString()
					}));
				}
				using (propertyPath.SetContext(dependencyObject))
				{
					if (propertyPath.Length < 1)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathEmpty"));
					}
					Storyboard.VerifyPathIsAnimatable(propertyPath);
					if (propertyPath.Length != 1)
					{
						this.ProcessComplexPath(clockMappings, dependencyObject, propertyPath, animationClock, handoffBehavior, layer);
						return;
					}
					DependencyProperty dependencyProperty = propertyPath.GetAccessor(0) as DependencyProperty;
					if (dependencyProperty == null)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathMustPointToDependencyProperty", new object[]
						{
							propertyPath.Path
						}));
					}
					Storyboard.VerifyAnimationIsValid(dependencyProperty, animationClock);
					Storyboard.ObjectPropertyPair mappingKey = new Storyboard.ObjectPropertyPair(dependencyObject, dependencyProperty);
					Storyboard.UpdateMappings(clockMappings, mappingKey, animationClock);
					return;
				}
			}
			if (currentClock is MediaClock)
			{
				Storyboard.ApplyMediaClock(nameScope, containingObject, dependencyObject, text, (MediaClock)currentClock);
				return;
			}
			ClockGroup clockGroup = currentClock as ClockGroup;
			if (clockGroup != null)
			{
				ClockCollection children = clockGroup.Children;
				for (int i = 0; i < children.Count; i++)
				{
					this.ClockTreeWalkRecursive(children[i], containingObject, nameScope, dependencyObject, text, propertyPath, handoffBehavior, clockMappings, layer);
				}
			}
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x001DBFAC File Offset: 0x001DAFAC
		private static void ApplyMediaClock(INameScope nameScope, DependencyObject containingObject, DependencyObject currentObject, string currentObjectName, MediaClock mediaClock)
		{
			MediaElement mediaElement;
			if (currentObjectName != null)
			{
				DependencyObject element = Helper.FindMentor(containingObject);
				mediaElement = (Storyboard.ResolveTargetName(currentObjectName, nameScope, element) as MediaElement);
				if (mediaElement == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_MediaElementNotFound", new object[]
					{
						currentObjectName
					}));
				}
			}
			else if (currentObject != null)
			{
				mediaElement = (currentObject as MediaElement);
			}
			else
			{
				mediaElement = (containingObject as MediaElement);
			}
			if (mediaElement == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_MediaElementRequired"));
			}
			mediaElement.Clock = mediaClock;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x001DC020 File Offset: 0x001DB020
		private static void UpdateMappings(HybridDictionary clockMappings, Storyboard.ObjectPropertyPair mappingKey, AnimationClock animationClock)
		{
			object obj = clockMappings[mappingKey];
			if (obj == null)
			{
				clockMappings[mappingKey] = animationClock;
				return;
			}
			if (obj is AnimationClock)
			{
				clockMappings[mappingKey] = new List<AnimationClock>
				{
					(AnimationClock)obj,
					animationClock
				};
				return;
			}
			((List<AnimationClock>)obj).Add(animationClock);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x001DC078 File Offset: 0x001DB078
		private static void ApplyAnimationClocks(HybridDictionary clockMappings, HandoffBehavior handoffBehavior, long layer)
		{
			foreach (object obj in clockMappings)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				Storyboard.ObjectPropertyPair objectPropertyPair = (Storyboard.ObjectPropertyPair)dictionaryEntry.Key;
				object value = dictionaryEntry.Value;
				List<AnimationClock> list;
				if (value is AnimationClock)
				{
					list = new List<AnimationClock>(1);
					list.Add((AnimationClock)value);
				}
				else
				{
					list = (List<AnimationClock>)value;
				}
				AnimationStorage.ApplyAnimationClocksToLayer(objectPropertyPair.DependencyObject, objectPropertyPair.DependencyProperty, list, handoffBehavior, layer);
			}
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x001DC11C File Offset: 0x001DB11C
		internal static void VerifyPathIsAnimatable(PropertyPath path)
		{
			bool flag = true;
			for (int i = 0; i < path.Length; i++)
			{
				object item = path.GetItem(i);
				object accessor = path.GetAccessor(i);
				if (item == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathObjectNotFound", new object[]
					{
						Storyboard.AccessorName(path, i - 1),
						path.Path
					}));
				}
				if (accessor == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathPropertyNotFound", new object[]
					{
						path.Path
					}));
				}
				if (i == 1)
				{
					Freezable freezable = item as Freezable;
					if (freezable != null && freezable.IsFrozen)
					{
						flag = false;
					}
				}
				else if (flag)
				{
					Freezable freezable = item as Freezable;
					if (freezable != null && freezable.IsFrozen)
					{
						if (i > 0)
						{
							throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathFrozenCheckFailed", new object[]
							{
								Storyboard.AccessorName(path, i - 1),
								path.Path,
								freezable.GetType().ToString()
							}));
						}
						throw new InvalidOperationException(SR.Get("Storyboard_ImmutableTargetNotSupported", new object[]
						{
							path.Path
						}));
					}
				}
				if (i == path.Length - 1)
				{
					DependencyObject dependencyObject = item as DependencyObject;
					DependencyProperty dependencyProperty = accessor as DependencyProperty;
					if (dependencyObject == null)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathMustPointToDependencyObject", new object[]
						{
							Storyboard.AccessorName(path, i - 1),
							path.Path
						}));
					}
					if (dependencyProperty == null)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathMustPointToDependencyProperty", new object[]
						{
							path.Path
						}));
					}
					if (flag && dependencyObject.IsSealed)
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathSealedCheckFailed", new object[]
						{
							dependencyProperty.Name,
							path.Path,
							dependencyObject
						}));
					}
					if (!AnimationStorage.IsPropertyAnimatable(dependencyObject, dependencyProperty))
					{
						throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathIncludesNonAnimatableProperty", new object[]
						{
							path.Path,
							dependencyProperty.Name
						}));
					}
				}
			}
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x001DC320 File Offset: 0x001DB320
		private static string AccessorName(PropertyPath path, int index)
		{
			object accessor = path.GetAccessor(index);
			if (accessor is DependencyProperty)
			{
				return ((DependencyProperty)accessor).Name;
			}
			if (accessor is PropertyInfo)
			{
				return ((PropertyInfo)accessor).Name;
			}
			if (accessor is PropertyDescriptor)
			{
				return ((PropertyDescriptor)accessor).Name;
			}
			return "[Unknown]";
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x001DC378 File Offset: 0x001DB378
		private static void VerifyAnimationIsValid(DependencyProperty targetProperty, AnimationClock animationClock)
		{
			if (!AnimationStorage.IsAnimationClockValid(targetProperty, animationClock))
			{
				throw new InvalidOperationException(SR.Get("Storyboard_AnimationMismatch", new object[]
				{
					animationClock.Timeline.GetType(),
					targetProperty.Name,
					targetProperty.PropertyType
				}));
			}
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x001DC3C4 File Offset: 0x001DB3C4
		private void ProcessComplexPath(HybridDictionary clockMappings, DependencyObject targetObject, PropertyPath path, AnimationClock animationClock, HandoffBehavior handoffBehavior, long layer)
		{
			DependencyProperty dependencyProperty = path.GetAccessor(0) as DependencyProperty;
			object value = targetObject.GetValue(dependencyProperty);
			DependencyObject dependencyObject = path.LastItem as DependencyObject;
			DependencyProperty dependencyProperty2 = path.LastAccessor as DependencyProperty;
			if (dependencyObject == null || dependencyProperty2 == null || dependencyProperty == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_PropertyPathUnresolved", new object[]
				{
					path.Path
				}));
			}
			Storyboard.VerifyAnimationIsValid(dependencyProperty2, animationClock);
			if (this.PropertyCloningRequired(value))
			{
				this.VerifyComplexPathSupport(targetObject);
				Freezable freezable = ((Freezable)value).Clone();
				Storyboard.SetComplexPathClone(targetObject, dependencyProperty, value, freezable);
				targetObject.InvalidateProperty(dependencyProperty);
				if (targetObject.GetValue(dependencyProperty) != freezable)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_ImmutableTargetNotSupported", new object[]
					{
						path.Path
					}));
				}
				using (path.SetContext(targetObject))
				{
					dependencyObject = (path.LastItem as DependencyObject);
					dependencyProperty2 = (path.LastAccessor as DependencyProperty);
				}
				Storyboard.ChangeListener.ListenToChangesOnFreezable(targetObject, freezable, dependencyProperty, (Freezable)value);
			}
			Storyboard.ObjectPropertyPair mappingKey = new Storyboard.ObjectPropertyPair(dependencyObject, dependencyProperty2);
			Storyboard.UpdateMappings(clockMappings, mappingKey, animationClock);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x001DC4EC File Offset: 0x001DB4EC
		private bool PropertyCloningRequired(object targetPropertyValue)
		{
			return targetPropertyValue is Freezable && ((Freezable)targetPropertyValue).IsFrozen;
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x001DC51C File Offset: 0x001DB51C
		private void VerifyComplexPathSupport(DependencyObject targetObject)
		{
			if (FrameworkElement.DType.IsInstanceOfType(targetObject))
			{
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(targetObject))
			{
				return;
			}
			throw new InvalidOperationException(SR.Get("Storyboard_ComplexPathNotSupported", new object[]
			{
				targetObject.GetType().ToString()
			}));
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x001DC568 File Offset: 0x001DB568
		internal static void GetComplexPathValue(DependencyObject targetObject, DependencyProperty targetProperty, ref EffectiveValueEntry entry, PropertyMetadata metadata)
		{
			Storyboard.CloneCacheEntry complexPathClone = Storyboard.GetComplexPathClone(targetObject, targetProperty);
			if (complexPathClone != null)
			{
				object value = entry.Value;
				if (value == DependencyProperty.UnsetValue && complexPathClone.Source == metadata.GetDefaultValue(targetObject, targetProperty))
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.Default;
					entry.SetAnimatedValue(complexPathClone.Clone, DependencyProperty.UnsetValue);
					return;
				}
				DeferredReference deferredReference = value as DeferredReference;
				if (deferredReference != null)
				{
					value = deferredReference.GetValue(entry.BaseValueSourceInternal);
					entry.Value = value;
				}
				if (complexPathClone.Source == value)
				{
					Storyboard.CloneEffectiveValue(ref entry, complexPathClone);
					return;
				}
				Storyboard.SetComplexPathClone(targetObject, targetProperty, DependencyProperty.UnsetValue, DependencyProperty.UnsetValue);
			}
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x001DC5F8 File Offset: 0x001DB5F8
		private static void CloneEffectiveValue(ref EffectiveValueEntry entry, Storyboard.CloneCacheEntry cacheEntry)
		{
			object clone = cacheEntry.Clone;
			if (!entry.IsExpression)
			{
				entry.Value = clone;
				return;
			}
			entry.ModifiedValue.ExpressionValue = clone;
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x001DC628 File Offset: 0x001DB628
		public void Begin(FrameworkElement containingObject)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, false);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x001DC633 File Offset: 0x001DB633
		public void Begin(FrameworkElement containingObject, HandoffBehavior handoffBehavior)
		{
			this.Begin(containingObject, handoffBehavior, false);
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x001DC63E File Offset: 0x001DB63E
		public void Begin(FrameworkElement containingObject, bool isControllable)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, isControllable);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x001DC649 File Offset: 0x001DB649
		public void Begin(FrameworkElement containingObject, HandoffBehavior handoffBehavior, bool isControllable)
		{
			this.BeginCommon(containingObject, null, handoffBehavior, isControllable, Storyboard.Layers.Code);
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x001DC65A File Offset: 0x001DB65A
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate)
		{
			this.Begin(containingObject, frameworkTemplate, HandoffBehavior.SnapshotAndReplace, false);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x001DC666 File Offset: 0x001DB666
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate, HandoffBehavior handoffBehavior)
		{
			this.Begin(containingObject, frameworkTemplate, handoffBehavior, false);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x001DC672 File Offset: 0x001DB672
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate, bool isControllable)
		{
			this.Begin(containingObject, frameworkTemplate, HandoffBehavior.SnapshotAndReplace, isControllable);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x001DC67E File Offset: 0x001DB67E
		public void Begin(FrameworkElement containingObject, FrameworkTemplate frameworkTemplate, HandoffBehavior handoffBehavior, bool isControllable)
		{
			this.BeginCommon(containingObject, frameworkTemplate, handoffBehavior, isControllable, Storyboard.Layers.Code);
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x001DC690 File Offset: 0x001DB690
		public void Begin(FrameworkContentElement containingObject)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, false);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x001DC69B File Offset: 0x001DB69B
		public void Begin(FrameworkContentElement containingObject, HandoffBehavior handoffBehavior)
		{
			this.Begin(containingObject, handoffBehavior, false);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x001DC6A6 File Offset: 0x001DB6A6
		public void Begin(FrameworkContentElement containingObject, bool isControllable)
		{
			this.Begin(containingObject, HandoffBehavior.SnapshotAndReplace, isControllable);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x001DC649 File Offset: 0x001DB649
		public void Begin(FrameworkContentElement containingObject, HandoffBehavior handoffBehavior, bool isControllable)
		{
			this.BeginCommon(containingObject, null, handoffBehavior, isControllable, Storyboard.Layers.Code);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x001DC6B4 File Offset: 0x001DB6B4
		public void Begin()
		{
			INameScope nameScope = null;
			HandoffBehavior handoffBehavior = HandoffBehavior.SnapshotAndReplace;
			bool isControllable = true;
			long code = Storyboard.Layers.Code;
			this.BeginCommon(this, nameScope, handoffBehavior, isControllable, code);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x001DC6DC File Offset: 0x001DB6DC
		internal void BeginCommon(DependencyObject containingObject, INameScope nameScope, HandoffBehavior handoffBehavior, bool isControllable, long layer)
		{
			if (containingObject == null)
			{
				throw new ArgumentNullException("containingObject");
			}
			if (!HandoffBehaviorEnum.IsDefined(handoffBehavior))
			{
				throw new ArgumentException(SR.Get("Storyboard_UnrecognizedHandoffBehavior"));
			}
			if (base.BeginTime == null)
			{
				return;
			}
			if (MediaContext.CurrentMediaContext.TimeManager == null)
			{
				return;
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardBegin, new object[]
				{
					this,
					base.Name,
					containingObject,
					nameScope
				});
			}
			HybridDictionary clockMappings = new HybridDictionary();
			Clock clock = base.CreateClock(isControllable);
			this.ClockTreeWalkRecursive(clock, containingObject, nameScope, null, null, null, handoffBehavior, clockMappings, layer);
			Storyboard.ApplyAnimationClocks(clockMappings, handoffBehavior, layer);
			if (isControllable)
			{
				this.SetStoryboardClock(containingObject, clock);
			}
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x001DC78D File Offset: 0x001DB78D
		public double? GetCurrentGlobalSpeed(FrameworkElement containingObject)
		{
			return this.GetCurrentGlobalSpeedImpl(containingObject);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x001DC78D File Offset: 0x001DB78D
		public double? GetCurrentGlobalSpeed(FrameworkContentElement containingObject)
		{
			return this.GetCurrentGlobalSpeedImpl(containingObject);
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x001DC798 File Offset: 0x001DB798
		public double GetCurrentGlobalSpeed()
		{
			double? currentGlobalSpeedImpl = this.GetCurrentGlobalSpeedImpl(this);
			if (currentGlobalSpeedImpl != null)
			{
				return currentGlobalSpeedImpl.Value;
			}
			return 0.0;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x001DC7C8 File Offset: 0x001DB7C8
		private double? GetCurrentGlobalSpeedImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentGlobalSpeed;
			}
			return null;
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x001DC7F0 File Offset: 0x001DB7F0
		public int? GetCurrentIteration(FrameworkElement containingObject)
		{
			return this.GetCurrentIterationImpl(containingObject);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x001DC7F0 File Offset: 0x001DB7F0
		public int? GetCurrentIteration(FrameworkContentElement containingObject)
		{
			return this.GetCurrentIterationImpl(containingObject);
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x001DC7FC File Offset: 0x001DB7FC
		public int GetCurrentIteration()
		{
			int? currentIterationImpl = this.GetCurrentIterationImpl(this);
			if (currentIterationImpl != null)
			{
				return currentIterationImpl.Value;
			}
			return 0;
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x001DC824 File Offset: 0x001DB824
		private int? GetCurrentIterationImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentIteration;
			}
			return null;
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x001DC84C File Offset: 0x001DB84C
		public double? GetCurrentProgress(FrameworkElement containingObject)
		{
			return this.GetCurrentProgressImpl(containingObject);
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x001DC84C File Offset: 0x001DB84C
		public double? GetCurrentProgress(FrameworkContentElement containingObject)
		{
			return this.GetCurrentProgressImpl(containingObject);
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x001DC858 File Offset: 0x001DB858
		public double GetCurrentProgress()
		{
			double? currentProgressImpl = this.GetCurrentProgressImpl(this);
			if (currentProgressImpl != null)
			{
				return currentProgressImpl.Value;
			}
			return 0.0;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x001DC888 File Offset: 0x001DB888
		private double? GetCurrentProgressImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentProgress;
			}
			return null;
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x001DC8B0 File Offset: 0x001DB8B0
		public ClockState GetCurrentState(FrameworkElement containingObject)
		{
			return this.GetCurrentStateImpl(containingObject);
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x001DC8B0 File Offset: 0x001DB8B0
		public ClockState GetCurrentState(FrameworkContentElement containingObject)
		{
			return this.GetCurrentStateImpl(containingObject);
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x001DC8B9 File Offset: 0x001DB8B9
		public ClockState GetCurrentState()
		{
			return this.GetCurrentStateImpl(this);
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x001DC8C4 File Offset: 0x001DB8C4
		private ClockState GetCurrentStateImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentState;
			}
			return ClockState.Stopped;
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x001DC8E4 File Offset: 0x001DB8E4
		public TimeSpan? GetCurrentTime(FrameworkElement containingObject)
		{
			return this.GetCurrentTimeImpl(containingObject);
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x001DC8E4 File Offset: 0x001DB8E4
		public TimeSpan? GetCurrentTime(FrameworkContentElement containingObject)
		{
			return this.GetCurrentTimeImpl(containingObject);
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x001DC8F0 File Offset: 0x001DB8F0
		public TimeSpan GetCurrentTime()
		{
			TimeSpan? currentTimeImpl = this.GetCurrentTimeImpl(this);
			if (currentTimeImpl != null)
			{
				return currentTimeImpl.Value;
			}
			return default(TimeSpan);
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x001DC920 File Offset: 0x001DB920
		private TimeSpan? GetCurrentTimeImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			if (storyboardClock != null)
			{
				return storyboardClock.CurrentTime;
			}
			return null;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x001DC948 File Offset: 0x001DB948
		public bool GetIsPaused(FrameworkElement containingObject)
		{
			return this.GetIsPausedImpl(containingObject);
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x001DC948 File Offset: 0x001DB948
		public bool GetIsPaused(FrameworkContentElement containingObject)
		{
			return this.GetIsPausedImpl(containingObject);
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x001DC951 File Offset: 0x001DB951
		public bool GetIsPaused()
		{
			return this.GetIsPausedImpl(this);
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x001DC95C File Offset: 0x001DB95C
		private bool GetIsPausedImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject);
			return storyboardClock != null && storyboardClock.IsPaused;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x001DC97C File Offset: 0x001DB97C
		public void Pause(FrameworkElement containingObject)
		{
			this.PauseImpl(containingObject);
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x001DC97C File Offset: 0x001DB97C
		public void Pause(FrameworkContentElement containingObject)
		{
			this.PauseImpl(containingObject);
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x001DC985 File Offset: 0x001DB985
		public void Pause()
		{
			this.PauseImpl(this);
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x001DC990 File Offset: 0x001DB990
		private void PauseImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Pause);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Pause();
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardPause, this, base.Name, this);
			}
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x001DC9CE File Offset: 0x001DB9CE
		public void Remove(FrameworkElement containingObject)
		{
			this.RemoveImpl(containingObject);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x001DC9CE File Offset: 0x001DB9CE
		public void Remove(FrameworkContentElement containingObject)
		{
			this.RemoveImpl(containingObject);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x001DC9D7 File Offset: 0x001DB9D7
		public void Remove()
		{
			this.RemoveImpl(this);
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x001DC9E0 File Offset: 0x001DB9E0
		private void RemoveImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Remove);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Remove();
				HybridDictionary value = Storyboard.StoryboardClockTreesField.GetValue(containingObject);
				if (value != null)
				{
					value.Remove(this);
				}
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardRemove, this, base.Name, containingObject);
			}
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x001DCA34 File Offset: 0x001DBA34
		public void Resume(FrameworkElement containingObject)
		{
			this.ResumeImpl(containingObject);
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x001DCA34 File Offset: 0x001DBA34
		public void Resume(FrameworkContentElement containingObject)
		{
			this.ResumeImpl(containingObject);
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x001DCA3D File Offset: 0x001DBA3D
		public void Resume()
		{
			this.ResumeImpl(this);
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x001DCA48 File Offset: 0x001DBA48
		private void ResumeImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Resume);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Resume();
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardResume, this, base.Name, containingObject);
			}
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x001DCA86 File Offset: 0x001DBA86
		public void Seek(FrameworkElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekImpl(containingObject, offset, origin);
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x001DCA86 File Offset: 0x001DBA86
		public void Seek(FrameworkContentElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekImpl(containingObject, offset, origin);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x001DCA91 File Offset: 0x001DBA91
		public void Seek(TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekImpl(this, offset, origin);
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x001DCA9C File Offset: 0x001DBA9C
		public void Seek(TimeSpan offset)
		{
			this.SeekImpl(this, offset, TimeSeekOrigin.BeginTime);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x001DCAA8 File Offset: 0x001DBAA8
		private void SeekImpl(DependencyObject containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Seek);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Seek(offset, origin);
			}
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x001DCACF File Offset: 0x001DBACF
		public void SeekAlignedToLastTick(FrameworkElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekAlignedToLastTickImpl(containingObject, offset, origin);
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x001DCACF File Offset: 0x001DBACF
		public void SeekAlignedToLastTick(FrameworkContentElement containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekAlignedToLastTickImpl(containingObject, offset, origin);
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x001DCADA File Offset: 0x001DBADA
		public void SeekAlignedToLastTick(TimeSpan offset, TimeSeekOrigin origin)
		{
			this.SeekAlignedToLastTickImpl(this, offset, origin);
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x001DCAE5 File Offset: 0x001DBAE5
		public void SeekAlignedToLastTick(TimeSpan offset)
		{
			this.SeekAlignedToLastTickImpl(this, offset, TimeSeekOrigin.BeginTime);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x001DCAF0 File Offset: 0x001DBAF0
		private void SeekAlignedToLastTickImpl(DependencyObject containingObject, TimeSpan offset, TimeSeekOrigin origin)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.SeekAlignedToLastTick);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.SeekAlignedToLastTick(offset, origin);
			}
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x001DCB17 File Offset: 0x001DBB17
		public void SetSpeedRatio(FrameworkElement containingObject, double speedRatio)
		{
			this.SetSpeedRatioImpl(containingObject, speedRatio);
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x001DCB17 File Offset: 0x001DBB17
		public void SetSpeedRatio(FrameworkContentElement containingObject, double speedRatio)
		{
			this.SetSpeedRatioImpl(containingObject, speedRatio);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x001DCB21 File Offset: 0x001DBB21
		public void SetSpeedRatio(double speedRatio)
		{
			this.SetSpeedRatioImpl(this, speedRatio);
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x001DCB2C File Offset: 0x001DBB2C
		private void SetSpeedRatioImpl(DependencyObject containingObject, double speedRatio)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.SetSpeedRatio);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.SpeedRatio = speedRatio;
			}
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x001DCB52 File Offset: 0x001DBB52
		public void SkipToFill(FrameworkElement containingObject)
		{
			this.SkipToFillImpl(containingObject);
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x001DCB52 File Offset: 0x001DBB52
		public void SkipToFill(FrameworkContentElement containingObject)
		{
			this.SkipToFillImpl(containingObject);
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x001DCB5B File Offset: 0x001DBB5B
		public void SkipToFill()
		{
			this.SkipToFillImpl(this);
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x001DCB64 File Offset: 0x001DBB64
		private void SkipToFillImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.SkipToFill);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.SkipToFill();
			}
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x001DCB89 File Offset: 0x001DBB89
		public void Stop(FrameworkElement containingObject)
		{
			this.StopImpl(containingObject);
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x001DCB89 File Offset: 0x001DBB89
		public void Stop(FrameworkContentElement containingObject)
		{
			this.StopImpl(containingObject);
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x001DCB92 File Offset: 0x001DBB92
		public void Stop()
		{
			this.StopImpl(this);
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x001DCB9C File Offset: 0x001DBB9C
		private void StopImpl(DependencyObject containingObject)
		{
			Clock storyboardClock = this.GetStoryboardClock(containingObject, false, Storyboard.InteractiveOperation.Stop);
			if (storyboardClock != null)
			{
				storyboardClock.Controller.Stop();
			}
			if (TraceAnimation.IsEnabled)
			{
				TraceAnimation.TraceActivityItem(TraceAnimation.StoryboardStop, this, base.Name, containingObject);
			}
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x001DCBDA File Offset: 0x001DBBDA
		private Clock GetStoryboardClock(DependencyObject o)
		{
			return this.GetStoryboardClock(o, true, Storyboard.InteractiveOperation.Unknown);
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x001DCBE8 File Offset: 0x001DBBE8
		private Clock GetStoryboardClock(DependencyObject o, bool throwIfNull, Storyboard.InteractiveOperation operation)
		{
			Clock result = null;
			WeakReference weakReference = null;
			HybridDictionary value = Storyboard.StoryboardClockTreesField.GetValue(o);
			if (value != null)
			{
				weakReference = (value[this] as WeakReference);
			}
			if (weakReference == null)
			{
				if (throwIfNull)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_NeverApplied"));
				}
				if (TraceAnimation.IsEnabledOverride)
				{
					TraceAnimation.Trace(TraceEventType.Warning, TraceAnimation.StoryboardNotApplied, operation, this, o);
				}
			}
			if (weakReference != null)
			{
				result = (weakReference.Target as Clock);
			}
			return result;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x001DCC58 File Offset: 0x001DBC58
		private void SetStoryboardClock(DependencyObject o, Clock clock)
		{
			HybridDictionary hybridDictionary = Storyboard.StoryboardClockTreesField.GetValue(o);
			if (hybridDictionary == null)
			{
				hybridDictionary = new HybridDictionary();
				Storyboard.StoryboardClockTreesField.SetValue(o, hybridDictionary);
			}
			hybridDictionary[this] = new WeakReference(clock);
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x001DCC94 File Offset: 0x001DBC94
		private static Storyboard.CloneCacheEntry GetComplexPathClone(DependencyObject o, DependencyProperty dp)
		{
			FrugalMap value = Storyboard.ComplexPathCloneField.GetValue(o);
			if (value[dp.GlobalIndex] != DependencyProperty.UnsetValue)
			{
				return (Storyboard.CloneCacheEntry)value[dp.GlobalIndex];
			}
			return null;
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x001DCCD8 File Offset: 0x001DBCD8
		private static void SetComplexPathClone(DependencyObject o, DependencyProperty dp, object source, object clone)
		{
			FrugalMap value = Storyboard.ComplexPathCloneField.GetValue(o);
			if (clone != DependencyProperty.UnsetValue)
			{
				value[dp.GlobalIndex] = new Storyboard.CloneCacheEntry(source, clone);
			}
			else
			{
				value[dp.GlobalIndex] = DependencyProperty.UnsetValue;
			}
			Storyboard.ComplexPathCloneField.SetValue(o, value);
		}

		// Token: 0x04001C5D RID: 7261
		public static readonly DependencyProperty TargetProperty;

		// Token: 0x04001C5E RID: 7262
		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.RegisterAttached("TargetName", typeof(string), typeof(Storyboard));

		// Token: 0x04001C5F RID: 7263
		public static readonly DependencyProperty TargetPropertyProperty = DependencyProperty.RegisterAttached("TargetProperty", typeof(PropertyPath), typeof(Storyboard));

		// Token: 0x04001C60 RID: 7264
		private static readonly UncommonField<HybridDictionary> StoryboardClockTreesField = new UncommonField<HybridDictionary>();

		// Token: 0x04001C61 RID: 7265
		private static readonly UncommonField<FrugalMap> ComplexPathCloneField = new UncommonField<FrugalMap>();

		// Token: 0x02000AC1 RID: 2753
		private class ObjectPropertyPair
		{
			// Token: 0x06008ADD RID: 35549 RVA: 0x00338A72 File Offset: 0x00337A72
			public ObjectPropertyPair(DependencyObject o, DependencyProperty p)
			{
				this._object = o;
				this._property = p;
			}

			// Token: 0x06008ADE RID: 35550 RVA: 0x00338A88 File Offset: 0x00337A88
			public override int GetHashCode()
			{
				return this._object.GetHashCode() ^ this._property.GetHashCode();
			}

			// Token: 0x06008ADF RID: 35551 RVA: 0x00338AA1 File Offset: 0x00337AA1
			public override bool Equals(object o)
			{
				return o != null && o is Storyboard.ObjectPropertyPair && this.Equals((Storyboard.ObjectPropertyPair)o);
			}

			// Token: 0x06008AE0 RID: 35552 RVA: 0x00338ABC File Offset: 0x00337ABC
			public bool Equals(Storyboard.ObjectPropertyPair key)
			{
				return this._object.Equals(key._object) && this._property == key._property;
			}

			// Token: 0x17001E62 RID: 7778
			// (get) Token: 0x06008AE1 RID: 35553 RVA: 0x00338AE1 File Offset: 0x00337AE1
			public DependencyObject DependencyObject
			{
				get
				{
					return this._object;
				}
			}

			// Token: 0x17001E63 RID: 7779
			// (get) Token: 0x06008AE2 RID: 35554 RVA: 0x00338AE9 File Offset: 0x00337AE9
			public DependencyProperty DependencyProperty
			{
				get
				{
					return this._property;
				}
			}

			// Token: 0x04004648 RID: 17992
			private DependencyObject _object;

			// Token: 0x04004649 RID: 17993
			private DependencyProperty _property;
		}

		// Token: 0x02000AC2 RID: 2754
		private class CloneCacheEntry
		{
			// Token: 0x06008AE3 RID: 35555 RVA: 0x00338AF1 File Offset: 0x00337AF1
			internal CloneCacheEntry(object source, object clone)
			{
				this.Source = source;
				this.Clone = clone;
			}

			// Token: 0x0400464A RID: 17994
			internal object Source;

			// Token: 0x0400464B RID: 17995
			internal object Clone;
		}

		// Token: 0x02000AC3 RID: 2755
		internal class ChangeListener
		{
			// Token: 0x06008AE4 RID: 35556 RVA: 0x00338B07 File Offset: 0x00337B07
			internal ChangeListener(DependencyObject target, Freezable clone, DependencyProperty property, Freezable original)
			{
				this._target = target;
				this._property = property;
				this._clone = clone;
				this._original = original;
			}

			// Token: 0x06008AE5 RID: 35557 RVA: 0x00338B2C File Offset: 0x00337B2C
			internal void InvalidatePropertyOnCloneChange(object source, EventArgs e)
			{
				Storyboard.CloneCacheEntry complexPathClone = Storyboard.GetComplexPathClone(this._target, this._property);
				if (complexPathClone != null && complexPathClone.Clone == this._clone)
				{
					this._target.InvalidateSubProperty(this._property);
					return;
				}
				this.Cleanup();
			}

			// Token: 0x06008AE6 RID: 35558 RVA: 0x00338B74 File Offset: 0x00337B74
			internal void InvalidatePropertyOnOriginalChange(object source, EventArgs e)
			{
				this._target.InvalidateProperty(this._property);
				this.Cleanup();
			}

			// Token: 0x06008AE7 RID: 35559 RVA: 0x00338B8D File Offset: 0x00337B8D
			internal static void ListenToChangesOnFreezable(DependencyObject target, Freezable clone, DependencyProperty dp, Freezable original)
			{
				new Storyboard.ChangeListener(target, clone, dp, original).Setup();
			}

			// Token: 0x06008AE8 RID: 35560 RVA: 0x00338BA0 File Offset: 0x00337BA0
			private void Setup()
			{
				EventHandler value = new EventHandler(this.InvalidatePropertyOnCloneChange);
				this._clone.Changed += value;
				if (this._original.IsFrozen)
				{
					this._original = null;
					return;
				}
				value = new EventHandler(this.InvalidatePropertyOnOriginalChange);
				this._original.Changed += value;
			}

			// Token: 0x06008AE9 RID: 35561 RVA: 0x00338BF4 File Offset: 0x00337BF4
			private void Cleanup()
			{
				EventHandler value = new EventHandler(this.InvalidatePropertyOnCloneChange);
				this._clone.Changed -= value;
				if (this._original != null)
				{
					value = new EventHandler(this.InvalidatePropertyOnOriginalChange);
					this._original.Changed -= value;
				}
				this._target = null;
				this._property = null;
				this._clone = null;
				this._original = null;
			}

			// Token: 0x0400464C RID: 17996
			private DependencyObject _target;

			// Token: 0x0400464D RID: 17997
			private DependencyProperty _property;

			// Token: 0x0400464E RID: 17998
			private Freezable _clone;

			// Token: 0x0400464F RID: 17999
			private Freezable _original;
		}

		// Token: 0x02000AC4 RID: 2756
		internal static class Layers
		{
			// Token: 0x04004650 RID: 18000
			internal static long ElementEventTrigger = 1L;

			// Token: 0x04004651 RID: 18001
			internal static long StyleOrTemplateEventTrigger = 1L;

			// Token: 0x04004652 RID: 18002
			internal static long Code = 1L;

			// Token: 0x04004653 RID: 18003
			internal static long PropertyTriggerStartLayer = 2L;
		}

		// Token: 0x02000AC5 RID: 2757
		private enum InteractiveOperation : ushort
		{
			// Token: 0x04004655 RID: 18005
			Unknown,
			// Token: 0x04004656 RID: 18006
			Pause,
			// Token: 0x04004657 RID: 18007
			Remove,
			// Token: 0x04004658 RID: 18008
			Resume,
			// Token: 0x04004659 RID: 18009
			Seek,
			// Token: 0x0400465A RID: 18010
			SeekAlignedToLastTick,
			// Token: 0x0400465B RID: 18011
			SetSpeedRatio,
			// Token: 0x0400465C RID: 18012
			SkipToFill,
			// Token: 0x0400465D RID: 18013
			Stop
		}
	}
}
