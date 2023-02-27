using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Core {
    public class MultipleErrorException : Exception {
        private Exception[] _arrExceptions;

        public MultipleErrorException(IEnumerable<Exception> enuExceptions)
            : base("Multiple errors have occurred")
        {
            TakeExceptions(enuExceptions);
        }

        public MultipleErrorException(params Exception[] arrExceptions)
            : base($"{arrExceptions.Length} errors have occurred")
        {
            TakeExceptions(arrExceptions);
        }

        private void TakeExceptions(IEnumerable<Exception> enuExceptions) {
            List<Exception> lstExceptions = new List<Exception>();

            foreach (Exception exCurrent in enuExceptions) {
                if (exCurrent is MultipleErrorException exMulti)
                    lstExceptions.AddRange(exMulti.Errors);
                else
                    lstExceptions.Add(exCurrent);
            }

            _arrExceptions = lstExceptions.ToArray();
        }

        public override string Message {
            get {
                switch (_arrExceptions.Length) {
                    case 0:
                        return "Unspecified errors occurred";

                    case 1:
                        return "An error occurred - " + _arrExceptions[0].Message;

                    default:
                        return _arrExceptions.Length + " errors occurred - " + _arrExceptions.Join(", ");
                }
            }
        }

        public override string StackTrace {
            get {
                string strTrace = string.Empty;

                for (int n = 0; n < _arrExceptions.Length; n++) {
                    strTrace += $"---- Error {n} ----\n";
                    strTrace += _arrExceptions[n].StackTrace;
                    strTrace += "\n\n";
                }

                strTrace += "---- Consolidated at ----\n";
                strTrace += base.StackTrace;
                return strTrace;
            }
        }

        public IEnumerable<Exception> Errors => _arrExceptions;
        public int ErrorCount => _arrExceptions.Length;
        public Exception GetError(int nIndex) => _arrExceptions[nIndex];

        public static void MaybeThrow(IEnumerable<Exception> enuExceptions) {
            if (enuExceptions?.IsEmpty() ?? true)
                return;

            if (enuExceptions.GetCount() == 1)
                throw enuExceptions.GetItem(0);

            throw new MultipleErrorException(enuExceptions);
        }
    }
}
