using System;
using System.Windows;
using System.Windows.Input;

namespace MS.Internal.Commands
{
	// Token: 0x0200026A RID: 618
	internal static class CommandHelpers
	{
		// Token: 0x060017E7 RID: 6119 RVA: 0x0015FA5A File Offset: 0x0015EA5A
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, null);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0015FA66 File Offset: 0x0015EA66
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, InputGesture inputGesture)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				inputGesture
			});
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0015FA7B File Offset: 0x0015EA7B
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, Key key)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				new KeyGesture(key)
			});
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x0015FA95 File Offset: 0x0015EA95
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				inputGesture,
				inputGesture2
			});
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x0015FAAF File Offset: 0x0015EAAF
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, null);
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0015FABB File Offset: 0x0015EABB
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture
			});
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x0015FAD1 File Offset: 0x0015EAD1
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, Key key)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				new KeyGesture(key)
			});
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0015FAEC File Offset: 0x0015EAEC
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture,
				inputGesture2
			});
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0015FB07 File Offset: 0x0015EB07
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2, InputGesture inputGesture3, InputGesture inputGesture4)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				inputGesture,
				inputGesture2,
				inputGesture3,
				inputGesture4
			});
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x0015FB2C File Offset: 0x0015EB2C
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, Key key, ModifierKeys modifierKeys, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				new KeyGesture(key, modifierKeys)
			});
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0015FB54 File Offset: 0x0015EB54
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, string srid1, string srid2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new InputGesture[]
			{
				KeyGesture.CreateFromResourceStrings(SR.Get(srid1), SR.Get(srid2))
			});
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x0015FB88 File Offset: 0x0015EB88
		internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, string srid1, string srid2)
		{
			CommandHelpers.PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new InputGesture[]
			{
				KeyGesture.CreateFromResourceStrings(SR.Get(srid1), SR.Get(srid2))
			});
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0015FBBC File Offset: 0x0015EBBC
		private static void PrivateRegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, params InputGesture[] inputGestures)
		{
			CommandManager.RegisterClassCommandBinding(controlType, new CommandBinding(command, executedRoutedEventHandler, canExecuteRoutedEventHandler));
			if (inputGestures != null)
			{
				for (int i = 0; i < inputGestures.Length; i++)
				{
					CommandManager.RegisterClassInputBinding(controlType, new InputBinding(command, inputGestures[i]));
				}
			}
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x0015FBFC File Offset: 0x0015EBFC
		internal static bool CanExecuteCommandSource(ICommandSource commandSource)
		{
			ICommand command = commandSource.Command;
			if (command == null)
			{
				return false;
			}
			object commandParameter = commandSource.CommandParameter;
			IInputElement inputElement = commandSource.CommandTarget;
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				if (inputElement == null)
				{
					inputElement = (commandSource as IInputElement);
				}
				return routedCommand.CanExecute(commandParameter, inputElement);
			}
			return command.CanExecute(commandParameter);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x0015FC47 File Offset: 0x0015EC47
		internal static void ExecuteCommandSource(ICommandSource commandSource)
		{
			CommandHelpers.CriticalExecuteCommandSource(commandSource, false);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x0015FC50 File Offset: 0x0015EC50
		internal static void CriticalExecuteCommandSource(ICommandSource commandSource, bool userInitiated)
		{
			ICommand command = commandSource.Command;
			if (command != null)
			{
				object commandParameter = commandSource.CommandParameter;
				IInputElement inputElement = commandSource.CommandTarget;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					if (inputElement == null)
					{
						inputElement = (commandSource as IInputElement);
					}
					if (routedCommand.CanExecute(commandParameter, inputElement))
					{
						routedCommand.ExecuteCore(commandParameter, inputElement, userInitiated);
						return;
					}
				}
				else if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0015FCB0 File Offset: 0x0015ECB0
		internal static void ExecuteCommand(ICommand command, object parameter, IInputElement target)
		{
			RoutedCommand routedCommand = command as RoutedCommand;
			if (routedCommand != null)
			{
				if (routedCommand.CanExecute(parameter, target))
				{
					routedCommand.Execute(parameter, target);
					return;
				}
			}
			else if (command.CanExecute(parameter))
			{
				command.Execute(parameter);
			}
		}
	}
}
