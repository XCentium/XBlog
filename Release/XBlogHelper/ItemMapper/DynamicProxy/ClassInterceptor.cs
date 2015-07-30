using System;
using Castle.DynamicProxy;
using Sitecore.Data.Items;

namespace XBlogHelper.ItemMapper.DynamicProxy
{
    internal class ClassInterceptor : IInterceptor
    {
        private Item _item;
        private Type _targetType;
        private object _classType;
        public ClassInterceptor(Item item, Type targetType)
        {
            _item = item;
            _targetType = targetType;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_classType == null)
                _classType = TypeMapper.CreateObject(_item, _targetType, false);

            invocation.ReturnValue = invocation.Method.Invoke(_classType, invocation.Arguments);
        }
    }
}
