using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows
{
	// Token: 0x0200035E RID: 862
	public class EventSetter : SetterBase
	{
		// Token: 0x06002081 RID: 8321 RVA: 0x00175985 File Offset: 0x00174985
		public EventSetter()
		{
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0017598D File Offset: 0x0017498D
		public EventSetter(RoutedEvent routedEvent, Delegate handler)
		{
			if (routedEvent == null)
			{
				throw new ArgumentNullException("routedEvent");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this._event = routedEvent;
			this._handler = handler;
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002083 RID: 8323 RVA: 0x001759BF File Offset: 0x001749BF
		// (set) Token: 0x06002084 RID: 8324 RVA: 0x001759C7 File Offset: 0x001749C7
		public RoutedEvent Event
		{
			get
			{
				return this._event;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.CheckSealed();
				this._event = value;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x001759E4 File Offset: 0x001749E4
		// (set) Token: 0x06002086 RID: 8326 RVA: 0x001759EC File Offset: 0x001749EC
		[TypeConverter(typeof(EventSetterHandlerConverter))]
		public Delegate Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.CheckSealed();
				this._handler = value;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x00175A09 File Offset: 0x00174A09
		// (set) Token: 0x06002088 RID: 8328 RVA: 0x00175A11 File Offset: 0x00174A11
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool HandledEventsToo
		{
			get
			{
				return this._handledEventsToo;
			}
			set
			{
				base.CheckSealed();
				this._handledEventsToo = value;
			}
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00175A20 File Offset: 0x00174A20
		internal override void Seal()
		{
			if (this._event == null)
			{
				throw new ArgumentException(SR.Get("NullPropertyIllegal", new object[]
				{
					"EventSetter.Event"
				}));
			}
			if (this._handler == null)
			{
				throw new ArgumentException(SR.Get("NullPropertyIllegal", new object[]
				{
					"EventSetter.Handler"
				}));
			}
			if (this._handler.GetType() != this._event.HandlerType)
			{
				throw new ArgumentException(SR.Get("HandlerTypeIllegal"));
			}
			base.Seal();
		}

		// Token: 0x04000FE5 RID: 4069
		private RoutedEvent _event;

		// Token: 0x04000FE6 RID: 4070
		private Delegate _handler;

		// Token: 0x04000FE7 RID: 4071
		private bool _handledEventsToo;
	}
}
