﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing {
    public class DrawingException : Exception {
        protected DrawingException() : base() { }
        protected DrawingException(string strMessage) : base(strMessage) { }
        protected DrawingException(string strMessage, Exception exInner) : base(strMessage, exInner) { }
    }

    public class ObjectCreationFailedException : DrawingException {
        public ObjectCreationFailedException() : base() { }
        public ObjectCreationFailedException(string strMessage) : base(strMessage) { }
        public ObjectCreationFailedException(string strMessage, Exception exInner) : base(strMessage, exInner) { }
    }

    public class InfoQueryFailedException : DrawingException {
        public InfoQueryFailedException() : base() { }
        public InfoQueryFailedException(string strMessage) : base(strMessage) { }
        public InfoQueryFailedException(string strMessage, Exception exInner) : base(strMessage, exInner) { }
    }
}
