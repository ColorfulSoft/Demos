//*************************************************************************************************
//* (C) ColorfulSoft, 2019. ��� ����� ��������.
//*************************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralArt
{

    // � ����������� ������������ ��������� (VGG-16) ��� ������� ����� ���� �������� 2x2. ��� �����������,
    // ��� �������� ������������� � ���.

    ///<summary>���� ������������� �������/���������� (Maximum Pooling).</summary>
    public sealed class MaxPoolLayer : Layer
    {

        ///<summary>������ �������� �������.</summary>
        public int InputWidth
        {
            get;
            set;
        }

        ///<summary>������ �������� �������.</summary>
        public int InputHeight
        {
            get;
            set;
        }

        ///<summary>������� �������� �������.</summary>
        public int InputDepth
        {
            get;
            set;
        }

        ///<summary>������ ��������� �������.</summary>
        public int OutputWidth
        {
            get;
            set;
        }

        ///<summary>������ ��������� �������.</summary>
        public int OutputHeight
        {
            get;
            set;
        }

        ///<summary>������� ��������� �������.</summary>
        public int OutputDepth
        {
            get;
            set;
        }

        ///<summary>������� ������.</summary>
        public Tensor Input
        {
            get;
            set;
        }

        ///<summary>�������� ������.</summary>
        public Tensor Output
        {
            get;
            set;
        }

        ///<summary>������� ������������ ��������� �� ��� X.</summary>
        public int[] SwitchX
        {
            get;
            set;
        }

        ///<summary>������� ������������ ��������� �� ��� Y.</summary>
        public int[] SwitchY
        {
            get;
            set;
        }

        ///<summary>�������������� ���� ������������� �������/���������� (Maximum Pooling).</summary>
        public MaxPoolLayer()
        {
            this.InputWidth = 0;
            this.InputHeight = 0;
            this.InputDepth = 0;
            this.OutputWidth = 0;
            this.OutputHeight = 0;
            this.OutputDepth = 0;
        }

        ///<summary>������ ��������������� ����� ���� ������������� �������/���������� (Maximum Pooling).</summary>
        ///<param name="input">������� ������.</param>
        public Tensor Forward(Tensor input)
        {
            this.Input = input;
            this.InputWidth = input.Width;
            this.InputHeight = input.Height;
            this.InputDepth = input.Depth;
            this.OutputWidth = input.Width / 2;
            this.OutputHeight = input.Height / 2;
            this.OutputDepth = input.Depth;
            this.SwitchX = new int[this.OutputWidth * this.OutputHeight * this.OutputDepth];
            this.SwitchY = new int[this.OutputWidth * this.OutputHeight * this.OutputDepth];
            var A = new Tensor(this.OutputWidth, this.OutputHeight, this.OutputDepth, true);
            Parallel.For(0, this.OutputDepth, (int d) =>
            {
                for(int ax = 0; ax < this.OutputWidth; ax++)
                {
                    var x = 2 * ax;
                    for(int ay = 0; ay < this.OutputHeight;  ay++)
                    {
                        var y = 2 * ay; 
                        float a = float.MinValue;
                        var winx = -1;
                        var winy = -1;
                        for(byte fx = 0; fx < 2; fx++)
                        {
                            for(byte fy = 0; fy < 2; fy++)
                            {
                                var oy = y + fy;
                                var ox = x + fx;
                                if((oy >= 0) && (oy < input.Height) && (ox >= 0) && (ox < input.Width))
                                {
                                    var v = input.GetW(ox, oy, d);
                                    if (v > a)
                                    {
                                        a = v;
                                        winx = ox;
                                        winy = oy;
                                    }
                                }
                            }
                        }
                        int n = ((this.OutputWidth * ay) + ax) * this.OutputDepth + d;
                        this.SwitchX[n] = winx;
                        this.SwitchY[n] = winy;
                        A.SetW(ax, ay, d, a);
                    }
                }
            });
            this.Output = A;
            return A;
        }

        ///<summary>�������� ��������������� (���������) ����� ���� ������������� �������/���������� (Maximum Pooling).</summary>
        public void Backward()
        {
            var V = this.Input;
            V.DW = new float[V.W.Length];
            var A = this.Output;
            Parallel.For(0, this.OutputDepth, (int d) =>
            {
                for(int ax = 0; ax < this.OutputWidth; ax++)
                {
                    for(int ay = 0; ay < this.OutputHeight; ay++)
                    {
                        float a = A.GetDW(ax, ay, d);
                        int n = ((this.OutputWidth * ay) + ax) * this.OutputDepth + d;
                        V.AddDW(this.SwitchX[n], this.SwitchY[n], d, a);
                    }
                }
            });
        }

    }

}