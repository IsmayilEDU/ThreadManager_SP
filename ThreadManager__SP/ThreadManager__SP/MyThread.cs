using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadManager__SP
{
    public class MyThread
    {
        private static int CurrentId;

        public int Id { get; set; }
        public string Name { get; set; }
        public Thread thread { get; set; }
        public string Situation { get; set; }

        static MyThread()
        {
            CurrentId = 0;
        }
        public MyThread()
        {
            CurrentId += 1;
            Id = CurrentId;
            Name = $"Thread {Id}";
            thread = new Thread(_ => { });
            Situation = nameof(ThreadManager__SP.Situation.Created);
        }

        public override string ToString()
        {
            return $"{Name} - {Situation}";
        }

    }
}
