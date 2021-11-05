using Aplicacion.Nucleo.Base;
using Infraestructura.Transversal.Log.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Nucleo.Observador
{
    public abstract class Observable
    {
        //List of observers
        protected Dictionary<Observer, string> _observers = new Dictionary<Observer, string>();

        /// <summary>
        /// Attach an observer
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="eventName"></param>
        public void attach(Observer observer, string eventName = null)
        {
            this._observers.Add(observer, eventName);
        }

        /// <summary>
        /// Detach an observer
        /// </summary>
        /// <param name="observer"></param>
        public void detach(Observer observer)
        {
            this._observers.Remove(observer);
        }

        /// <summary>
        /// Notify an event to registered observers
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventValue"></param>
        public void NotificarEvento(string eventName, InformationModel informacionPersistida)
        {
            foreach (Observer key in _observers.Keys)
            {
                if (_observers[key] == eventName )
                {
                    key.NotificarEvento(informacionPersistida);
                }
            }
        }
        public void NotificarExcepcion(string eventName,ErrorModel errorModelo)
        {
            foreach (Observer key in _observers.Keys)
            {
                if (_observers[key] == eventName)
                {
                    key.NotificarExcepcion(errorModelo);
                }
            }
        }
    }
}
