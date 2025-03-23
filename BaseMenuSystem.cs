using System;
using UnityEngine;

namespace UI.UISystems
{
    public class BaseMenuSystem<T> : MonoBehaviour where T : Controller
    {
        protected T Controller;
        private void Start()
        {
            Controller.Initialize();
        }
    }
}