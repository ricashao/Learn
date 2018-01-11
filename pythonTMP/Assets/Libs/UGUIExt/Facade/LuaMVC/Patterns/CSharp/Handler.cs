
using System.Collections.Generic;

namespace PureMVC.Patterns
{
    public interface IHandler
    {
        string HandlerName { get; }
        IList<string> HandleNotification();
        void Request(INotification notification);
        void Response();
        void OnRegister();
        void OnRemove();
    }

    public class Handler : IHandler
    {
        protected string m_handlerName;
        public string HandlerName
        {
            get { return m_handlerName; }
        }
        public const string NAME = "Handler";

        protected IProxy Proxy { get; set; }


        public Handler(){}
        public Handler( string name )
        {
            m_handlerName = name;
        } 
        public Handler( string name , IProxy proxy )
        {
            m_handlerName = name;
            Proxy = proxy;
        }

        public virtual IList<string> HandleNotification()
        {
            return new List<string>();
        }
        public virtual void Request(INotification notification)
        {

        }
        public virtual void Response()
        {

        }

        public virtual void OnRegister()
        {

        }
        public virtual void OnRemove()
        {

        }
    }
}