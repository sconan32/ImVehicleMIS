using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ImVehicleCore.Specifications
{
    public  class QueryStringSpecification<T> : BaseSpecification<T>
    {

        public QueryStringSpecification(string queryString)
        {
            Criteria = BuildCriteria(queryString);

        }


        protected virtual Expression<Func<T, bool>> BuildCriteria(string queryString)
        {
            List<Expression> expressions = new List<Expression>();
            ParameterExpression argParam = Expression.Parameter(typeof(T), "t");
            var properties = GetNamePropertyMap();
            List<char> operators = new List<char>();
            string[] splits = queryString.Split('+', '-', '*');
            if (splits.Length > 0)
            {
                int handledLength = 0;
                for (int exprNum = 0; exprNum < splits.Length; exprNum++)
                {
                    int innerLength = 0;
                    string expr = splits[exprNum];
                    string[] exprSplit = expr.Split(new string[] { "=", "==", "!=", ">", "<", ">=", "<=" }, StringSplitOptions.None);
                    if (exprSplit.Length != 2)
                    {
                        continue;
                    }
                    Tuple<string, Type> propertyInfo;
                    if (!properties.TryGetValue(exprSplit[0].Trim(), out propertyInfo))
                    {
                        continue;
                    }
                    Expression property = Expression.Property(argParam, propertyInfo.Item1);
                    innerLength += exprSplit[0].Length;
                    var valueIdx = expr.IndexOf(exprSplit[1]);
                    string op = expr.Substring(innerLength, valueIdx - innerLength);
                    var type = propertyInfo.Item2;
                    var value = ConvertStringToType(exprSplit[1].Trim(), type);
                    if (value == null)
                    {
                        continue;
                    }
                    switch (op)
                    {
                        case "=":
                        case "==":
                            expressions.Add(Expression.Equal(property, Expression.Constant(value)));
                            break;
                        case "!=":
                            expressions.Add(Expression.NotEqual(property, Expression.Constant(value)));
                            break;
                        case ">":
                            expressions.Add(Expression.GreaterThan(property, Expression.Constant(value)));
                            break;
                        case "<":
                            expressions.Add(Expression.LessThan(property, Expression.Constant(value)));
                            break;
                        case ">=":
                            expressions.Add(Expression.GreaterThanOrEqual(property, Expression.Constant(value)));
                            break;
                        case "<=":
                            expressions.Add(Expression.LessThanOrEqual(property, Expression.Constant(value)));
                            break;
                        default:
                            continue;
                    }

                    handledLength += splits[exprNum].Length;
                    if (handledLength < queryString.Length)
                    {
                        operators.Add(queryString[handledLength]);
                        handledLength += 1;
                    }
                }

                if (expressions.Count > 0)
                {
                    Expression retExp = expressions[0];
                    for (int i = 0; i < operators.Count; i++)
                    {
                        switch (operators[i])
                        {
                            case '+':
                                retExp = Expression.Or(retExp, expressions[i + 1]);
                                break;
                            case '*':
                                retExp = Expression.And(retExp, expressions[i + 1]);
                                break;
                            case '-':
                                retExp = Expression.And(retExp, Expression.Not(expressions[i + 1]));
                                break;
                        }
                    }
                    var lambda = Expression.Lambda<Func<T, bool>>(retExp, argParam);
                    return lambda;

                }

            }
            return t => true;

        }


             protected virtual Dictionary<string, Tuple<string, Type>> GetNamePropertyMap()
        {
            Dictionary<string, Tuple<string, Type>> dictionary = new Dictionary<string, Tuple<string, Type>>();

            foreach (var propInfo in typeof(T).GetProperties())
            {
                var type = propInfo.PropertyType;
                var prop = propInfo.Name;
                dictionary.Add(prop, new Tuple<string, Type>(prop, type));
                var dd = propInfo.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (dd.Length > 0)
                {
                    var name = dd.FirstOrDefault(a => a is DisplayAttribute);
                    if (name != null)
                    {
                        dictionary[(name as DisplayAttribute).Name] = new Tuple<string, Type>(prop, type);
                    }
                }
            }


            return dictionary;
        }


        private object ConvertStringToType(string value, Type type)
        {
            return Convert.ChangeType(value, type);
        }
    }
}
