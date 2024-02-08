using System.ComponentModel;
using System.Reflection;

namespace RabbitMQ.In.Practice.Infrastructure {
    public static class EnumExtension {
        public static string GetDescription(this Enum self) {
            var field = self.GetType().GetField(self.ToString());

            if (field is not null) {
                var attr = field.GetCustomAttribute<DescriptionAttribute>(inherit: false);
                if (attr is not null) {
                    return attr.Description;
                }
            }

            return self.ToString();
        }
    }
}
