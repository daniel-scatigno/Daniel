using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Globalization;
using System.ComponentModel;
using System.Linq.Expressions;
using System;
using System.Reflection;
using System.Resources;
using System.Collections.Generic;

namespace NCPC.Utils
{
   public static class ExpressionUtil
   {
      /// <summary>
      ///     Gets the corresponding <see cref="PropertyInfo" /> from an <see cref="Expression" />.
      /// </summary>
      /// <param name="property">The expression that selects the property to get info on.</param>
      /// <returns>The property info collected from the expression.</returns>
      /// <exception cref="ArgumentNullException">When <paramref name="property" /> is <c>null</c>.</exception>
      /// <exception cref="ArgumentException">The expression doesn't indicate a valid property."</exception>
      public static PropertyInfo GetPropertyInfo<T, P>(Expression<Func<T, P>> property)
      {
         if (property == null)
         {
            throw new ArgumentNullException(nameof(property));
         }

         if (property.Body is UnaryExpression unaryExp)
         {
            if (unaryExp.Operand is MemberExpression memberExp)
            {
               return (PropertyInfo)memberExp.Member;
            }
         }
         else if (property.Body is MemberExpression memberExp)
         {
            return (PropertyInfo)memberExp.Member;
         }

         throw new ArgumentException($"The expression doesn't indicate a valid property. [ {property} ]");
      }

      public static T GetAttribute<T>(this PropertyInfo member, bool isRequired) where T : Attribute
      {
         var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

         if (attribute == null && isRequired)
         {
            throw new ArgumentException(
                string.Format(CultureInfo.InvariantCulture,
                    "The {0} attribute must be defined on member {1}",
                    typeof(T).Name,
                    member.Name));
         }

         return (T)attribute;
      }

      public static T GetAttribute<T>(this Type member, bool isRequired) where T : Attribute
      {
         var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

         if (attribute == null && isRequired)
         {
            throw new ArgumentException(
                string.Format(CultureInfo.InvariantCulture,
                    "The {0} attribute must be defined on member {1}",
                    typeof(T).Name,
                    member.Name));
         }

         return (T)attribute;
      }

      public static string GetDisplayNameFor(this object obj, string propertyName)
      {
         return obj.GetType().GetProperty(propertyName).GetDisplayName();
      }

      public static string GetDisplayName<TType>(Expression<Func<TType, object>> expression)
      {
         var prop = ExpressionUtil.GetPropertyInfo<TType, object>(expression);
         return GetDisplayName(prop);
      }

      public static bool IsRequired(this PropertyInfo property)
      {
         var requiredAttr = property.GetAttribute<RequiredAttribute>(false);
         return (requiredAttr!=null);
      }

      public static string GetDisplayName(this PropertyInfo property)
      {         
         var attr = property.GetAttribute<DisplayAttribute>(false);
         if (attr == null)
         {
            return property.Name;
         }

         string displayValue = attr.Name;

         if (attr.ResourceType != null)
         {
            var resource = attr.ResourceType.GetProperties().Where(x => x.Name == attr.Name).FirstOrDefault();

            if (resource != null)
            {
               try
               {
                  displayValue = resource.GetValue(null).ToString();
               }
               catch (Exception ex)
               {
                  displayValue = "ResourceNotFound";
               }
            }
         }

         //TODO Implementar algo parecido para pegar o nome de um recurso, Localizar
         //Eu implementei de outra forma, porém pode ser que esta forma possa ser util
         //ResourceManager resourceManager = new ResourceManager(displayAttribute.ResourceType);
         //var entry = resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)
         //                           .OfType<DictionaryEntry>()
         //                           .FirstOrDefault(p => p.Key.ToString() == displayAttribute.Name);

         return displayValue;
      }

      public static string GetDisplayName(this Type type)
      {
         var attr = type.GetAttribute<DisplayAttribute>(false);
         if (attr == null)
         {
            return type.Name;
         }
         var resource = attr.ResourceType.GetProperties().Where(x => x.Name == attr.Name).FirstOrDefault();
         string displayValue = attr.Name;
         if (resource != null)
         {
            displayValue = resource.GetValue(null).ToString();
         }

         return displayValue;
      }

      public static object GetParentValue<TValue>(this Expression<Func<TValue, object>> func, TValue value) where TValue : class
      {
         //TODO melhorar este método, pois se ele tiver vários objetos filhos, não ira retornar o Valor do Pai

         var memberExp = func.Body as MemberExpression;
         while (memberExp != null)
         {
            var memberInfo = memberExp.Member;
            memberExp = memberExp.Expression as MemberExpression;
            TValue parentValue = null;
            try
            {
               PropertyInfo p = ((PropertyInfo)memberInfo);
               var obj = p.GetValue(value);
               if (obj != null)
                  return obj;
            }
            catch (Exception ex)
            {

            }
         }
         return "não implementado";

      }

      public static Expression<Func<T, bool>> EqualsToExpression<T>(this PropertyInfo prop, object value)
      {
         var parameterExpression = Expression.Parameter(typeof(T));
         var property = Expression.Property(parameterExpression, prop);
         var constant = Expression.Constant(value);
         var expression = Expression.Equal(property, constant);
         var lambda = Expression.Lambda<Func<T, bool>>(expression, parameterExpression);

         return lambda;
      }

      public static string ToFieldName<T, TResult>(this Expression<Func<T, TResult>> exp)
      {
         MemberExpression expr;

         if (exp.Body is MemberExpression)
            // .Net interpreted this code trivially like t => t.Id
            expr = (MemberExpression)exp.Body;
         else
            // .Net wrapped this code in Convert to reduce errors, meaning it's t => Convert(t.Id) - get at the
            // t.Id inside
            expr = (MemberExpression)((UnaryExpression)exp.Body).Operand;

         var expressions = GetMembersOnPath(expr);
         string name = String.Join(".", expressions.Select(m => m.Member.Name).Reverse());
         // Console.WriteLine("Expression:" + name);
         // Console.WriteLine("Expression2:" + exp.ToString());

         return name;

      }

      private static IEnumerable<MemberExpression> GetMembersOnPath(MemberExpression expression)
      {
         while (expression != null)
         {
            yield return expression;
            expression = expression.Expression as MemberExpression;
         }
      }




   }
}