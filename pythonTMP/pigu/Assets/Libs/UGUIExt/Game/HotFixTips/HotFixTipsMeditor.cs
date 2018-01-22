using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;

namespace ZhuYuU3d.Game
{ 
public class HotFixTipsMeditor : Mediator
{

        public new const string NAME = "HotFixTipsMeditor";
        public HotFixTipsPage View
        {
            get { return View; }
        }

        public HotFixTipsMeditor(HotFixTipsPage view) : base(NAME, view)
        {
            _view = view;
        }

        HotFixTipsPage _view = null;


        public override void OnRegister()
        {
            base.OnRegister();
        }
        public override void OnRemove()
        {
            base.OnRemove();
        }

        public override void HandleNotification(INotification notification)
        {
        }

        public override IList<string> ListNotificationInterests()
        {
            return new List<string>();
        }

    }

}


