using System;
using Sitecore.Data.Items;
using Castle.DynamicProxy;
using XBlogHelper.ItemMapper.DynamicProxy;
using System.Collections.Generic;
using System.Reflection;
using Sitecore.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using Sitecore.StringExtensions;

namespace XBlogHelper.ItemMapper
{
    internal class TypeMapper
    {
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        // to avoid creating multiple delegates we'll maintain a dictionary
        /// <summary>
        /// dictionary of object delegates
        /// </summary>
        private static Dictionary<ConstructorInfo, Delegate> _constructorDelegates = new Dictionary<ConstructorInfo, Delegate>();
        public static object CreateObject(Item item, Type targetType, bool isLazyLoad)
        {
            if (isLazyLoad)
                return _proxyGenerator.CreateClassProxy(targetType, new ClassInterceptor(item, targetType));

            Delegate conMethod = GetConstructorDelegate(targetType);
            object target = conMethod.DynamicInvoke(targetType.GetConstructor(Type.EmptyTypes).GetParameters());

            DataHandler.SetItemData(target, targetType, item, isLazyLoad);

            return target;

        }
        #region get class constructor

        /// <summary>
        /// creates and gets a constructor delegate
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Delegate GetConstructorDelegate(Type type)
        {
            Assert.IsNotNull(type, "XCore: class type is not specified.");

            // get constructor for the given targetType
            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

            Assert.IsNotNull(constructor, "XCore: No constructor for class {0} found.".FormatWith(type.GetType()));

            // test to see if the constructor already has a delegate defined
            Delegate delegateObj = null;
            lock (_constructorDelegates)
            {
                delegateObj = _constructorDelegates.ContainsKey(constructor) ? _constructorDelegates[constructor] : null;
            }

            // if no delegate was found, create one and add it to the dictionary
            if (delegateObj == null)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                if (parameters.Any())
                    throw new Exception("XCore: currently only supports parameterless constructors");

                DynamicMethod dynamicMethod = new DynamicMethod("XCore_DM_{0}".FormatWith(type.Name), type, Type.EmptyTypes, type);

                ILGenerator ilGenerator = dynamicMethod.GetILGenerator();

                ilGenerator.DeclareLocal(type);
                ilGenerator.Emit(OpCodes.Newobj, constructor);
                ilGenerator.Emit(OpCodes.Ret);

                Type delegateType = typeof(Func<>).MakeGenericType(type);
                delegateObj = dynamicMethod.CreateDelegate(delegateType);

                lock (_constructorDelegates)
                {
                    _constructorDelegates[constructor] = delegateObj;
                }
            }
            return delegateObj;
        }
        #endregion


    }
}
