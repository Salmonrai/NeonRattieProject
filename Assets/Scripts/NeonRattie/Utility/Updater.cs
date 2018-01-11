using System;
using System.Collections.Generic;

namespace NeonRattie.Utility
{
    public class Updater: IUpdater
    {
        public Action<float> UpdateAction { get; set; }
        
        public void Update(float deltaTime)
        {
            if (UpdateAction == null)
            {
                return;
            }
            UpdateAction(deltaTime);
        }

        public void Add(Action<float> action)
        {
            UpdateAction += action;
        }
    }
}