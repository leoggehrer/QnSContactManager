//MdStart
using QnSContactManager.Logic.DataContext;

namespace QnSContactManager.Logic.Controllers.Persistence
{
    internal abstract partial class QnSContactManagerController<I, E> : GenericController<I, E>
       where I : Contracts.IIdentifiable
       where E : Entities.IdentityObject, I, Contracts.ICopyable<I>, new()
    {
        internal IQnSContactManagerContext QnSContactManagerContext => (IQnSContactManagerContext)Context;

        protected QnSContactManagerController(IContext context)
            : base(context)
        {
        }
        protected QnSContactManagerController(ControllerObject controller)
            : base(controller)
        {
        }
    }
}
//MdEnd
