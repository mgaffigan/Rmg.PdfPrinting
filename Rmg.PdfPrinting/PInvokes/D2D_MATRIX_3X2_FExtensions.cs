using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Win32.Graphics.Direct2D.Common;

internal partial struct D2D_MATRIX_3X2_F
{
    public float _11
    {
        get => Anonymous.Anonymous2._11;
        set => Anonymous.Anonymous2._11 = value;
    }

    public float _12
    {
        get => Anonymous.Anonymous2._12;
        set => Anonymous.Anonymous2._12 = value;
    }

    public float _21
    {
        get => Anonymous.Anonymous2._21;
        set => Anonymous.Anonymous2._21 = value;
    }

    public float _22
    {
        get => Anonymous.Anonymous2._22;
        set => Anonymous.Anonymous2._22 = value;
    }

    public float _31
    {
        get => Anonymous.Anonymous2._31;
        set => Anonymous.Anonymous2._31 = value;
    }

    public float _32
    {
        get => Anonymous.Anonymous2._32;
        set => Anonymous.Anonymous2._32 = value;
    }

    public static D2D_MATRIX_3X2_F operator *(D2D_MATRIX_3X2_F a, D2D_MATRIX_3X2_F b)
    {
        return new D2D_MATRIX_3X2_F
        {
            _11 = a._11 * b._11 + a._12 * b._21,
            _12 = a._11 * b._12 + a._12 * b._22,
            _21 = a._21 * b._11 + a._22 * b._21,
            _22 = a._21 * b._12 + a._22 * b._22,
            _31 = a._31 * b._11 + a._32 * b._21 + b._31,
            _32 = a._31 * b._12 + a._32 * b._22 + b._32,
        };
    }

    public static D2D_MATRIX_3X2_F Scale(float x, float y)
    {
        var val = new D2D_MATRIX_3X2_F();
        val._11 = x;
        val._22 = y;
        return val;
    }

    public static D2D_MATRIX_3X2_F Identity => Scale(1f, 1f);
}
