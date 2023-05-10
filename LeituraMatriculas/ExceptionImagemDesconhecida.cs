using System;
using System.Runtime.Serialization;

namespace LeituraMatriculas
{
    [Serializable]

    public class ExceptionImagemDesconhecida : Exception
    {
        public ExceptionImagemDesconhecida()
        {
        }

        public ExceptionImagemDesconhecida(string message) : base(message)
        {
        }

        public ExceptionImagemDesconhecida(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionImagemDesconhecida(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}