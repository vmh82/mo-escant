﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Helpers
{
    public class AsyncRunner
    {
        public static void Run(Task task, Action<Task> onError = null)
        {
            if (onError == null)
            {
                task.ContinueWith((task1, o) => { }, TaskContinuationOptions.OnlyOnFaulted);
            }
            else
            {
                task.ContinueWith(onError, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}
