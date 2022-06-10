using System;
using System.Runtime.InteropServices;

namespace RealTimeFilter
{
    /// <summary>
    /// Input Method Manager Apiクラス
    /// </summary>
    public static class Imm
    {
        /// <summary>
        /// IMM(Imput Method Manager)のコンテキストの取得。
        /// コンテキストは必ず ImmReleaseContext で開放すること。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("Imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        /// <summary>
        /// IMMコンテキストの開放
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hIMC"></param>
        /// <returns></returns>
        [DllImport("Imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        /// <summary>
        /// 現在入力中の文字列の取得
        /// </summary>
        /// <param name="hIMC"></param>
        /// <param name="dwIndex"></param>
        /// <param name="lpBuf"></param>
        /// <param name="dwBufLen"></param>
        /// <returns></returns>
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        public static extern int ImmGetCompositionStringW(IntPtr hIMC, int dwIndex, byte[] lpBuf, int dwBufLen);

        /// <summary>
        /// 現在編集中の文字列を取得するフラグ
        /// </summary>
        public const int GCS_COMPSTR = 8;
    }
}
