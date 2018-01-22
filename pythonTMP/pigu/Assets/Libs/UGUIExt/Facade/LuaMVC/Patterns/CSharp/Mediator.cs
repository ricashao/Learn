using System;
using UnityEngine;

namespace PureMVC.Patterns
{
    using System.Collections.Generic;
    using ZhuYuU3d;

    public interface IMediator
    {
        void HandleNotification(INotification notification);
        IList<string> ListNotificationInterests();
        void OnRegister();
        void OnRemove();
        string MediatorName { get; }
        object ViewComponent { get; set; }
        //UIPage view{ get; set; }
    }
    
    public class Mediator : Notifier, IMediator
    {
        protected string m_mediatorName;
        public virtual string MediatorName
        {
            get { return this.m_mediatorName; }
        }
        public const string NAME = "Mediator";
        public object ViewComponent { get; set; }
        //public UIPage view { get; set; }

        public Mediator() : this("Mediator", null)
        {
        }
        public Mediator(string mediatorName) : this(mediatorName, null)
        {
        }
        public Mediator(string mediatorName, object viewComponent)
        {
            this.m_mediatorName = (mediatorName != null) ? mediatorName : "Mediator";
            this.ViewComponent = viewComponent;
        }
        //public Mediator(string mediatorName)//, UIPage view)
        //{
        //    this.m_mediatorName = (mediatorName != null) ? mediatorName : "Mediator";
        //    if (null == view)
        //    {
        //        throw new Exception(string.Format("The {0}'s view is null.", m_mediatorName));
        //    }
        //    //this.view = view;
        //    //this.view.Mediator = this;
        //}

        public virtual void HandleNotification(INotification notification)
        {
        }
        public virtual IList<string> ListNotificationInterests()
        {
            return new List<string>();
        }
        public virtual void OnRegister()
        {
            //if( null != view)
            //    view.Initialize();
        }

        public virtual void OnRemove()
        {

        }
    }
}