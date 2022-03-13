using System;
using System.Collections.Generic;
using System.Text;

namespace Azimecha.Drawing.Internal {
    public interface INumericFunctions<T> {
        T Zero { get; }
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
        T Negate(T x);
    }

    public struct NumericHelper : INumericFunctions<int>, INumericFunctions<float> {
        int INumericFunctions<int>.Zero => 0;
        int INumericFunctions<int>.Add(int a, int b) => a + b;
        int INumericFunctions<int>.Divide(int a, int b) => a / b;
        int INumericFunctions<int>.Multiply(int a, int b) => a * b;
        int INumericFunctions<int>.Subtract(int a, int b) => a - b;
        int INumericFunctions<int>.Negate(int x) => -x;

        float INumericFunctions<float>.Zero => 0;
        float INumericFunctions<float>.Add(float a, float b) => a + b;
        float INumericFunctions<float>.Divide(float a, float b) => a / b;
        float INumericFunctions<float>.Multiply(float a, float b) => a * b;
        float INumericFunctions<float>.Subtract(float a, float b) => a - b;
        float INumericFunctions<float>.Negate(float x) => -x;
    }
}
