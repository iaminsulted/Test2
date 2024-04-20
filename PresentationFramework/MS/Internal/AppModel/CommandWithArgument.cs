using System;
using System.Windows;
using System.Windows.Input;

namespace MS.Internal.AppModel
{
	// Token: 0x02000290 RID: 656
	internal class CommandWithArgument
	{
		// Token: 0x060018DC RID: 6364 RVA: 0x001620D2 File Offset: 0x001610D2
		public CommandWithArgument(RoutedCommand command) : this(command, null)
		{
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x001620DC File Offset: 0x001610DC
		public CommandWithArgument(RoutedCommand command, object argument)
		{
			this._command = new SecurityCriticalDataForSet<RoutedCommand>(command);
			this._argument = argument;
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x001620F8 File Offset: 0x001610F8
		public bool Execute(IInputElement target, object argument)
		{
			if (argument == null)
			{
				argument = this._argument;
			}
			if (this._command.Value is ISecureCommand)
			{
				bool flag;
				if (this._command.Value.CriticalCanExecute(argument, target, true, out flag))
				{
					this._command.Value.ExecuteCore(argument, target, true);
					return true;
				}
				return false;
			}
			else
			{
				if (this._command.Value.CanExecute(argument, target))
				{
					this._command.Value.Execute(argument, target);
					return true;
				}
				return false;
			}
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0016217C File Offset: 0x0016117C
		public bool QueryEnabled(IInputElement target, object argument)
		{
			if (argument == null)
			{
				argument = this._argument;
			}
			if (this._command.Value is ISecureCommand)
			{
				bool flag;
				return this._command.Value.CriticalCanExecute(argument, target, true, out flag);
			}
			return this._command.Value.CanExecute(argument, target);
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060018E0 RID: 6368 RVA: 0x001621CE File Offset: 0x001611CE
		public RoutedCommand Command
		{
			get
			{
				return this._command.Value;
			}
		}

		// Token: 0x04000D6F RID: 3439
		private object _argument;

		// Token: 0x04000D70 RID: 3440
		private SecurityCriticalDataForSet<RoutedCommand> _command;
	}
}
