//*************************************************************************************************
//* (C) ColorfulSoft, 2019. ��� ����� ��������.
//*************************************************************************************************

using System;

namespace NeuralArt
{

    ///<summary>��������� ���� ���������.</summary>
    public interface Layer
    {

        ///<summary>������� ������ ����.</summary>
        Tensor Input
        {
            get;
            set;
        }

        ///<summary>�������� ������.</summary>
        Tensor Output
        {
            get;
            set;
        }

        ///<summary>������ �������� �������.</summary>
        int InputWidth
        {
            get;
            set;
        }

        ///<summary>������ �������� �������.</summary>
        int InputHeight
        {
            get;
            set;
        }

        ///<summary>������� �������� �������.</summary>
        int InputDepth
        {
            get;
            set;
        }

        ///<summary>������ ��������� �������.</summary>
        int OutputWidth
        {
            get;
            set;
        }

        ///<summary>������ ��������� �������.</summary>
        int OutputHeight
        {
            get;
            set;
        }

        ///<summary>������� ��������� �������.</summary>
        int OutputDepth
        {
            get;
            set;
        }

        ///<summary>����� ������� ��������������� ����� ����.</summary>
        ///<param name="input">������� ������.</param>
        Tensor Forward(Tensor input);

        ///<summary>����� ��������� ��������������� (���������) ����� ����.</summary>
        void Backward();

    }

}