//@QnSCodeCopy
//MdStart
using System;

namespace QnSContactManager.Logic.Exceptions
{
    public class AuthorizationException : LogicException
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der LogicException-Klasse 
        /// mit einer angegebenen Fehlermeldung.
        /// </summary>
        /// <param name="errorType">Identification der Fehlermeldung.</param>
        public AuthorizationException(ErrorType errorType)
            : base(errorType)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der LogicException-Klasse 
        /// mit einer angegebenen Fehlermeldung.
        /// </summary>
        /// <param name="errorType">Identification der Fehlermeldung.</param>
        /// <param name="message">Die Meldung, in der der Fehler beschrieben wird.</param>
        public AuthorizationException(ErrorType errorType, string message)
            : base(errorType, message)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der LogicException-Klasse 
        /// mit einer angegebenen Fehlermeldung.
        /// </summary>
        /// <param name="errorType">Identification der Fehlermeldung.</param>
        /// <param name="ex">Exception die aufgetreten ist.</param>
        public AuthorizationException(ErrorType errorType, Exception ex)
            : base(errorType, ex.InnerException)
        {
        }
    }
}
//MdEnd
