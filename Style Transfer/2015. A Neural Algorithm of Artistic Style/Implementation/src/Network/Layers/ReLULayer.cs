//*************************************************************************************************
//* (C) ColorfulSoft, 2019. ��� ����� ��������.
//*************************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralArt
{

    ///<summary>���� �������� ������������ (ReLU).</summary>
    public sealed class ReluLayer : Layer
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

        ///<summary>�������������� ���� �������� ������������ (ReLU).</summary>
        public ReluLayer()
        {
            this.InputWidth = 0;
            this.InputHeight = 0;
            this.InputDepth = 0;
            this.OutputWidth = 0;
            this.OutputHeight = 0;
            this.OutputDepth = 0;
        }

        ///<summary>������ ��������������� ����� ���� �������� ������������ (ReLU).</summary>
        ///<param name="input">������� ������.</param>
        public Tensor Forward(Tensor input)
        {
            this.Input = input;
            this.InputWidth = input.Width;
            this.InputHeight = input.Height;
            this.InputDepth = input.Depth;
            this.OutputWidth = input.Width;
            this.OutputHeight = input.Height;
            this.OutputDepth = input.Depth;
            var Result = new Tensor(input.Width, input.Height, input.Depth, true);
            Parallel.For(0, OutputDepth, (int d) =>
            {
                for(int y = 0; y < OutputHeight; y++)
                {
                    for(int x = 0; x < OutputWidth; x++)
                    {
                        var v = input.GetW(x, y, d);
                        Result.SetW(x, y, d, ((v > 0.0f) ? (v) : (0.0f)));
                    }
                }
            });
            this.Output = Result;
            return Result;
        }

        ///<summary>�������� ��������������� (���������) ����� ���� �������� ������������ (ReLU).</summary>
        public void Backward()
        {
            var V = this.Input;
            var V2 = this.Output;
            V.DW = new float[V.W.Length];
            Parallel.For(0, this.OutputDepth, (int d) =>
            {
                for(int y = 0; y < this.OutputHeight; y++)
                {
                    for(int x = 0; x < this.OutputWidth; x++)
                    {
                        if(V2.GetW(x, y, d) <= 0.0f)
                        {
                            V.SetDW(x, y, d, 0.0f);
                        }
                        else
                        {
                            V.SetDW(x, y, d, V2.GetDW(x, y, d));
                        }
                    }
                }
            });
        }

    }

}