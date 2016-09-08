using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modd
{
    public abstract class Annotatable
    {

        // The dictionary of annotations supports the attachment of arbitrary information to the nodes of the graph
        // related operations support their maintenance.
        private Dictionary<string, object> annotations = new Dictionary<string, object>();

        public virtual void AddAnnotation(string source, object annotation)
        {
            annotations[source] = annotation;
        }


        public virtual void DeleteAnnotation(string source)
        {
            annotations.Remove(source);
        }

        public virtual object GetAnnotation(string source)
        {
            return annotations.ContainsKey(source) ? annotations[source] : null;
        }

        public virtual T GetAnnotation<T>(string source)
        {
            return (T)(GetAnnotation(source));
        }
        internal bool HasAnnotation(string source)
        {
            return annotations.ContainsKey(source);
        }

    }
}
