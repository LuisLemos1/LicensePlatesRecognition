using System;
using System.Runtime.Serialization;

namespace LeituraMatriculas
{
    [Serializable]

    public class ExceptionMatriculaDesconhecida : Exception
    {
        public ExceptionMatriculaDesconhecida()
        {
        }

        public ExceptionMatriculaDesconhecida(string message) : base(message)
        {
        }

        public ExceptionMatriculaDesconhecida(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionMatriculaDesconhecida(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}