using System.Threading;

namespace Modd
{
    public abstract class Identifiable: Annotatable
    {
        private static int _counter = 0;
        protected Identifiable()
        {
            _id = Interlocked.Increment(ref _counter);
        }
        private int _id;
        public int InstanceIdentifier { get { return _id; } }
    }
    public abstract class BaseDtVertexType : Identifiable
    {
        protected BaseDtVertexType() : base()
        {
        }
    }
}