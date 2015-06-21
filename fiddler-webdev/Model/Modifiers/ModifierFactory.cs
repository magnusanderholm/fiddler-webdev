namespace Fiddler.Webdev.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ModifierFactory
    {
        private static IEnumerable<ConstructorInfo> availableSessionModifierConstructors;
        private readonly Settings settings;

        static ModifierFactory()
        {
            var modifierType = typeof(Modifier);
            var iSessionModifierType = typeof(ISessionModifier);
            var modifierAttributeType = typeof(ModifierAttribute);
            var constructorParemeterTypes = new Type[] { typeof(UrlRule) };
            var assembly = typeof(ModifierFactory).Assembly;
            availableSessionModifierConstructors =
                (from t in assembly.GetTypes()
                 where t.IsClass && modifierType.IsAssignableFrom(t) && iSessionModifierType.IsAssignableFrom(t)
                 let modAttr = (ModifierAttribute)t.GetCustomAttributes(modifierAttributeType, true).FirstOrDefault()
                 let constr = t.GetConstructor(constructorParemeterTypes)
                 where modAttr != null && constr != null && modAttr.IsEnabled                 
                 select constr).ToArray();
        }

        public ModifierFactory(Settings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<ISessionModifier> Create(UrlRule urlRule)
        {
            var constructorParameters = new object[] { urlRule };
            return availableSessionModifierConstructors
                .Select(c => (Modifier)c.Invoke(constructorParameters))
                .ToArray();
        }

        public UrlRule Create()
        {
            var urlRule = new UrlRule(settings);
            var constructorParameters = new object[] { urlRule };
            foreach (var m in availableSessionModifierConstructors.Select(c => (Modifier)c.Invoke(constructorParameters)))
                urlRule.Modifiers.Add(m);
            return urlRule;
        }
    }
}
