/*
 namespace System.Threading
{
    public sealed class Lock
    {
        public void Enter();
        public void Exit();
        public Scope EnterScope();
    
        public ref struct Scope
        {
            public void Dispose();
        }
    }
}
*/

var modifier = new LockDemo();
modifier.Modify();

internal class LockDemo
{
    private readonly Lock _lockObj = new();

    public void Modify()
    {
        lock (_lockObj)
        {
            Console.WriteLine("I'm a critical section associated with _lockObj");
        }

        using (_lockObj.EnterScope())
        {
            Console.WriteLine("I'm another critical section associated with _lockObj");
        }

        _lockObj.Enter();
        try
        {
            Console.WriteLine("I'm also a critical section associated with _lockObj");
        }
        finally
        {
            _lockObj.Exit();
        }

        if (_lockObj.TryEnter())
        {
            try
            {
                Console.WriteLine("I'm also another critical section associated with _lockObj");
            }
            finally
            {
                _lockObj.Exit();
            }
        }
    }
}