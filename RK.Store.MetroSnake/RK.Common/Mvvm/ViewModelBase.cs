using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RK.Common.Mvvm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Raises when one of the public properties have changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event using the member within the given expression.
        /// </summary>
        /// <typeparam name="T">Type of the member.</typeparam>
        /// <param name="getPropertyExpression">The linq epxression to parse.</param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> getPropertyExpression)
        {
            LambdaExpression lambdaExpression = getPropertyExpression as LambdaExpression;
            if (lambdaExpression != null)
            {
                MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    RaisePropertyChanged(memberExpression.Member.Name);
                    return;
                }
            }

            throw new InvalidOperationException("Unable to process given expression!");
        }

        /// <summary>
        /// Gets the name of the member the given expression points to.
        /// </summary>
        /// <typeparam name="T">Type of the mebmer.</typeparam>
        /// <param name="getMemberExpression">The expression pointing to the member.</param>
        public string GetMemberName<T>(Expression<Func<T>> getMemberExpression)
        {
            LambdaExpression lambdaExpression = getMemberExpression as LambdaExpression;
            if (lambdaExpression != null)
            {
                MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression.Member.Name;
                }
            }

            throw new InvalidOperationException("Unable to process given expression!");
        }

        /// <summary>
        /// Gets the property the given expression points to.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="getPropertyExpression">The expression that points to the property.</param>
        protected PropertyInfo GetProperty<T>(Expression<Func<T>> getPropertyExpression)
        {
            LambdaExpression lambdaExpression = getPropertyExpression as LambdaExpression;
            if (lambdaExpression != null)
            {
                MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression.Member as PropertyInfo;
                }
            }

            throw new InvalidOperationException("Unable to process given expression!");
        }

        /// <summary>
        /// Gets the foeöd the given expression points to.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="getPropertyExpression">The expression that points to the field.</param>
        protected FieldInfo GetField<T>(Expression<Func<T>> getPropertyExpression)
        {
            LambdaExpression lambdaExpression = getPropertyExpression as LambdaExpression;
            if (lambdaExpression != null)
            {
                MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;
                if (memberExpression != null)
                {
                    return memberExpression.Member as FieldInfo;
                }
            }

            throw new InvalidOperationException("Unable to process given expression!");
        }

        /// <summary>
        /// Notifies a changed property.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(
            [CallerMemberName]
            string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName)) { return; }
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
    }
}
