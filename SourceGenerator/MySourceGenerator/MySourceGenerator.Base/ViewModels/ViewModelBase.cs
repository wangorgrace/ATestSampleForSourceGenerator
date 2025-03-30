using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MySourceGenerator.Base.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the name of the changed property.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="propertyName">(optional) The name of the changed property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            InvalidateCommands(propertyName);
        }

        /// <summary>
        /// Invalidates the commands for the changed propertyName
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected virtual void InvalidateCommands(string propertyName) { }
    }
}
