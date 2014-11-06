namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class EventBus : IEventBus 
    {        
        private LinkedList<Subscription> subscribers = new LinkedList<Subscription>();

        public IDisposable SubscribeTo<TSender, TMessage>(Action<TSender, TMessage> onRecieved)
        {            
            // TOOD throw if onReceived.Target is null. We do not support static methods right now.
                                                
            // TODO Create a open delegate from onRecieved it will NOT have a Target property set hence
            //      it will not keep the parent object alive
            //var d = Delegate.CreateDelegate(onRecieved.GetType(), onRecieved.Method);
            var subscriptionNode = subscribers.AddLast(new Subscription(typeof(TSender), typeof(TMessage), onRecieved));

            return Disposable.Create(subscriptionNode, (_subscriptionNode) => 
            {
                if (_subscriptionNode.List != null)
                    _subscriptionNode.List.Remove(_subscriptionNode);
            });
        }

        public void Publish<TMessage>(object sender, TMessage message)
        {
            foreach (var node in AsNodes(subscribers))
            {                
                if (node.Value.IsMatch(sender, message))
                {
                    if (!node.Value.Invoke(sender, message))
                        subscribers.Remove(node);
                }                    
            }            
        }

        private static IEnumerable<LinkedListNode<T>> AsNodes<T>(LinkedList<T> list)
        {
            var node = list.First;
            while (node != null)
            {
                yield return node;
                node = node.Next;
            }
        }

        //static Func<T, object, object> MagicMethod<T>(MethodInfo method) where T : class
        //{
        //    // First fetch the generic form
        //    MethodInfo genericHelper = typeof(EventBus).GetMethod("MagicMethodHelper",
        //        BindingFlags.Static | BindingFlags.NonPublic);

        //    // Now supply the type arguments
        //    MethodInfo constructedHelper = genericHelper.MakeGenericMethod
        //        (typeof(T), method.GetParameters()[0].ParameterType, method.ReturnType);

        //    // Now call it. The null argument is because it's a static method.
        //    object ret = constructedHelper.Invoke(null, new object[] { method });

        //    // Cast the result to the right kind of delegate and return it
        //    return (Func<T, object, object>)ret;
        //}

        //static Func<TTarget, object, object> MagicMethodHelper<TTarget, TParam, TReturn>(MethodInfo method)
        //    where TTarget : class
        //{
        //    // Convert the slow MethodInfo into a fast, strongly typed, open delegate
        //    Func<TTarget, TParam, TReturn> func = (Func<TTarget, TParam, TReturn>)Delegate.CreateDelegate
        //        (typeof(Func<TTarget, TParam, TReturn>), method);

        //    // Now create a more weakly typed delegate which will call the strongly typed one
        //    Func<TTarget, object, object> ret = (TTarget target, object param) => func(target, (TParam)param);
        //    return ret;
        //}

        class Subscription
        {            
            private readonly WeakReference receiverRef;
            private readonly Type senderType, messageType, receiverType;
            private readonly MethodInfo onRecievedHandlerMethodInfo;

            public Subscription(Type senderType, Type messageType, Delegate onRecievedHandler)
            {
                this.receiverRef = new WeakReference(onRecievedHandler.Target);
                this.senderType = senderType;
                this.messageType = messageType;
                this.receiverType = onRecievedHandler.Target.GetType();
                this.onRecievedHandlerMethodInfo = onRecievedHandler.Method;
            }

            public bool IsMatch(object sender, object message)
            {
                return senderType.IsAssignableFrom(sender.GetType()) && messageType.IsAssignableFrom(message.GetType());
            }
            
            public bool Invoke(object sender, object message)
            {
                bool invoked = true;                
                if (IsMatch(sender, message))
                {
                    var h = (object)receiverRef.Target;
                    if (h != null) 
                    {
                        // TODO This can be replaced with a compiled expression with a little trickery.
                        //      just note that this expression must be a "open delegate" as it cannot
                        //      contain a Target to avoid memory leaks.
                        onRecievedHandlerMethodInfo.Invoke(h, new object[] { sender, message });                        
                    }
                        
                    invoked = h != null;                    
                }
                return invoked;
            }      
        }
    }
}
