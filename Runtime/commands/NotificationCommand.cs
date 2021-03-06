using System;
using BeatThat.Controllers;
using BeatThat.Notifications;
using BeatThat.Service;

namespace BeatThat.Commands
{
    /// <summary>
    /// Command that registers itself to execute 
    /// when a specific notification type is dispatched by the NotificationBus,
    /// with that notification type left as a public property so that it can be set via the unity editor.
    /// </summary>
    abstract public class NotificationCommand<ArgType> : NotificationCommandBase<ArgType>
	{
		public NotificationType m_notificationType;

		override public string notificationType { get { return m_notificationType != null? m_notificationType.notificationType: null; } }
	}
		
	abstract public class NotificationCommand : NotificationCommand<Notification> 
    {
        override public void Execute(Notification n)
        {
            Execute();
        }

        virtual public void Execute()
        {
            throw new NotImplementedException("must override either Execute() (with no args) or Execute(Notification)");
        }
    }


	/// <summary>
	/// Command that registers itself to execute 
	/// when a specific notification type is dispatched by the NotificationBus,
	/// with that notification type provided by the subclass (usually just hard coded).
	/// </summary>
	abstract public class NotificationCommandBase<ArgType> : Subcontroller, AutoInitService
	{
		#pragma warning disable 618
		public void InitService (Services services)
		{
			Bind();
		}
		#pragma warning restore 618

		abstract public string notificationType { get; }

		abstract public void Execute(ArgType n);

		override protected void BindSubcontroller()
		{
			Bind(this.notificationType, this.requestAction);
		}

		private void OnRequest(ArgType n)
		{
			Execute(n);
		}

		private Action<ArgType> requestAction { get { return m_requestAction?? (m_requestAction = this.OnRequest); } }
		private Action<ArgType> m_requestAction;
	}

	abstract public class NotificationCommandBase : NotificationCommandBase<Notification> 
    {
        override public void Execute(Notification n)
        {
            Execute();
        }

        virtual public void Execute()
        {
            throw new NotImplementedException("must override either Execute() (with no args) or Execute(Notification)");
        }
    }

}

