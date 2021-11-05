using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericExtensions
{
    public static class LinkedListExtensions
    {
        public static LinkedListNode<T> FindNext<T> (this LinkedListNode<T> init, Func<T,bool> filter, bool inclusive = true)
        {
            if (!inclusive)
                init = init.Next;
            while(init != null && !filter(init.Value))
            {
                init = init.Next;
            }
            return init;
        }
        public static LinkedListNode<T> FindPrevious<T>(this LinkedListNode<T> init, Func<T, bool> filter, bool inclusive = true)
        {
            if (!inclusive)
                init = init.Previous;
            while (init != null && !filter(init.Value))
            {
                init = init.Previous;
            }
            return init;
        }
        public static LinkedListNode<T> FindFirst<T>(this LinkedList<T> list, Func<T, bool> filter)
        {
            return list.First.FindNext(filter);
        }
    }
}
