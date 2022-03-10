using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public static class TextTokenizer {
        public static Token[] Tokenize(string str, bool bMergeTokens = true) {
            List<Token> lstTokens = new List<Token>();

            // convert to tokens
            foreach (char c in str) {
                Token tok = new Token() { Text = c.ToString() };

                if (c == '\n')
                    tok.Type = TokenType.LineBreak;
                else if (char.IsWhiteSpace(c))
                    tok.Type = TokenType.Trimmable | TokenType.WordBreak;
                else if (char.IsSeparator(c))
                    tok.Type = TokenType.WordBreak;
                else
                    tok.Type = TokenType.Content;

                lstTokens.Add(tok);
            }

            // merge tokens
            if (bMergeTokens) {
                for (int nToken = lstTokens.Count; nToken > 0; nToken--) {
                    Token tokCur = lstTokens[nToken];
                    if ((tokCur.Type & TokenType.LineBreak) != 0)
                        continue;

                    Token tokLeft = lstTokens[nToken - 1];
                    if (tokLeft.Type == tokCur.Type) {
                        lstTokens[nToken - 1] = new Token() {
                            Text = tokLeft.Text + tokCur.Text,
                            Type = tokCur.Type
                        };

                        lstTokens.RemoveAt(nToken);
                    }
                }
            }

            return lstTokens.ToArray();
        }

        public static Token[][] ToLines(IEnumerable<Token> enuTokens) {
            List<Token[]> lstLines = new List<Token[]>();
            List<Token> lstCurLine = new List<Token>();

            foreach (Token tok in enuTokens) {
                if ((tok.Type & TokenType.LineBreak) != 0) {
                    lstLines.Add(lstCurLine.ToArray());
                    lstCurLine = new List<Token>();
                } else {
                    lstCurLine.Add(tok);
                }
            }
            
            if (lstCurLine.Count > 0)
                lstLines.Add(lstCurLine.ToArray());

            return lstLines.ToArray();
        }

        private static void TrimLine(IList<Token> lstTokens, bool bTrimStart, bool bTrimEnd) {
            if (bTrimStart)
                while ((lstTokens.Count > 0) && ((lstTokens[0].Type & TokenType.Trimmable) != 0))
                    lstTokens.RemoveAt(0);

            if (bTrimEnd)
                while ((lstTokens.Count > 0) && ((lstTokens[lstTokens.Count - 1].Type & TokenType.Trimmable) != 0))
                    lstTokens.RemoveAt(lstTokens.Count - 1);
        }

        public static Token[][] ToWrappedLines(IEnumerable<Token> enuTokens, StringWidthDelegate procGetSize, float fAreaWidth, bool bTrimOnWrap = true) {
            List<Token[]> lstLines = new List<Token[]>();

            List<Token> lstCurLine = new List<Token>();
            float fCurLineWidth = 0.0f;
            bool bStartedByWrap = false, bEndedByWrap;

            foreach (Token tok in enuTokens) {
                float fTokenWidth = procGetSize(tok.Text);

                if (((tok.Type & TokenType.LineBreak) != 0) || (fCurLineWidth + fTokenWidth > fAreaWidth)) {
                    bEndedByWrap = (tok.Type & TokenType.LineBreak) == 0;
                    if (bTrimOnWrap) TrimLine(lstCurLine, bStartedByWrap, bEndedByWrap);
                    bStartedByWrap = bEndedByWrap;

                    lstLines.Add(lstCurLine.ToArray());
                    lstCurLine = new List<Token>();
                }

                if ((tok.Type & TokenType.LineBreak) == 0) {
                    lstCurLine.Add(tok);
                    fCurLineWidth += fTokenWidth;
                }
            }

            if (lstCurLine.Count > 0) {
                if (bTrimOnWrap) TrimLine(lstCurLine, bStartedByWrap, false);
                lstLines.Add(lstCurLine.ToArray());
            }

            return lstLines.ToArray();
        }

        public static string LinesToString(Token[][] arrLines, string strLineSeparator) {
            string strCombined = "";

            for (int nLine = 0; nLine < arrLines.Length; nLine++) {
                strCombined += TokensToString(arrLines[nLine]);
                if (nLine < (arrLines.Length - 1))
                    strCombined += strLineSeparator;
            }

            return strCombined;
        }

        public static string TokensToString(IEnumerable<Token> enuTokens, string strSeparator = "") {
            string strCombined = "";

            foreach (Token tok in enuTokens)
                strCombined += tok.Text + strSeparator;

            return strCombined;
        }

        public delegate float StringWidthDelegate(string str);

        [Flags]
        public enum TokenType {
            Content = 0,
            LineBreak = 1 << 0,
            Trimmable = 1 << 1,
            WordBreak = 1 << 2
        }

        public struct Token {
            public TokenType Type;
            public string Text;
        }

    }
}
