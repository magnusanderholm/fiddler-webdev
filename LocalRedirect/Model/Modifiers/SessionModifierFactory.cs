namespace Fiddler.LocalRedirect.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class SessionModifierFactory
    {
        private static IEnumerable<ConstructorInfo> availableSessionModifierConstructors;
        private readonly Settings settings;

        static SessionModifierFactory()
        {
            var childSettingType = typeof(ChildSetting);
            var iSessionModifierType = typeof(ISessionModifier);
            var modifierAttributeType = typeof(ModifierAttribute);
            var constructorParemeterTypes = new Type[] { typeof(UrlRule) };
            var assembly = typeof(SessionModifierFactory).Assembly;
            availableSessionModifierConstructors =
                (from t in assembly.GetTypes()
                 where t.IsClass && childSettingType.IsAssignableFrom(t) && iSessionModifierType.IsAssignableFrom(t)
                 let modAttr = (ModifierAttribute)t.GetCustomAttributes(modifierAttributeType, true).FirstOrDefault()
                 let constr = t.GetConstructor(constructorParemeterTypes)
                 where modAttr != null && constr != null && modAttr.IsEnabled
                 orderby modAttr.Order ascending, t.Name ascending
                 select constr).ToArray();
        }

        public SessionModifierFactory(Settings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<ISessionModifier> Create(UrlRule urlRule)
        {
            var constructorParameters = new object[] { urlRule };
            return availableSessionModifierConstructors
                .Select(c => (ChildSetting)c.Invoke(constructorParameters))
                .ToArray();
        }

        public UrlRule Create()
        {
            var urlRule = new UrlRule(settings);
            var constructorParameters = new object[] { urlRule };
            foreach (var m in availableSessionModifierConstructors.Select(c => (ChildSetting)c.Invoke(constructorParameters)))
                urlRule.Children.Add(m);
            return urlRule;
        }
    }
}
