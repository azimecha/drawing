using Azimecha.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.AGG {
    public class TrueTypeFontFile : FontSet {
        public TrueTypeFontFile(IDataBuffer bufData, bool bDisposeBuffer = true)
            : base(CreateOnTTFData(bufData, bDisposeBuffer)) { }

        public TrueTypeFontFile(byte[] arrData) 
            : this(new PinnedArrayDataBuffer<byte>(arrData)) { }

        public TrueTypeFontFile(string strPath) 
            : this(MemoryMapping.MapFileReadOnly(strPath, false)) { }

        public TrueTypeFontFile(System.IO.Stream stmData)
            : this(HGlobalDataBuffer<byte>.FromStream(stmData)) { }

        private static SafeFontSetHandle CreateOnTTFData(IDataBuffer bufData, bool bDisposeBuffer) {
            SafeFontSetHandle hFontSet = new SafeFontSetHandle();

            BufferUtils.TryPassOwnership(bufData, bDisposeBuffer, infBuffer => {
                hFontSet.TakeObject(Interop.Functions.Loader.GetMethod<Interop.Functions.AwCreateFontSetOnTTF>()(infBuffer), true);
                if (!hFontSet.IsHandleValid)
                    throw new ObjectCreationFailedException("Error creating font set from TTF data. The data may be corrupt.");
            });

            return hFontSet;
        }
    }
}
