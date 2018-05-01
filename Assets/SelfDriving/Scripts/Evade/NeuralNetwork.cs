using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class VectorArray // 1 x n의 벡터
{
    public int num_dimension_;
    public double[] data;

    public VectorArray() {
        num_dimension_ = 0;
        data = new double[0];
    }
    public VectorArray(int num)
    {
        data = new double[0];
        Initialize(num);
    }

    public VectorArray( VectorArray vector_)
    {
        Initialize(vector_.num_dimension_, false);
        for (int i = 0; i < num_dimension_; i++)
        {
            data[i] = vector_.data[i];
        }
    }

    public void Initialize(int num_, bool wantInit = false)
    {
        num_dimension_ = num_;
        data = null;
        
        if(num_dimension_ > 0)
        {
            data = new double[num_dimension_];
            if (wantInit)
            {
                for (int i = 0; i < num_dimension_; i++)
                {
                    data[i] = 0;
                }
            }
        }

      
    }

   
}

public class Matrix    // 매트릭스를 1차원 배열로 구현
{
    public int num_rows_;
    public int num_cols_;
    public double[] data;

    public Matrix()
    {
        num_rows_ = 0;
        num_cols_ = 0;
        data = null;
    }

    public void Initialize(int _m, int _n, bool wantInit = true)
    {
        int num_all_old = num_rows_ * num_cols_; // 기존 매트릭스 크기

        num_rows_ = _m;
        num_cols_ = _n;

        int num_all = num_rows_ * num_cols_;

        if (num_all_old != num_all)
        {

            data = new double[num_all];

            if (wantInit)
            {
                for (int i = 0; i < num_all; i++)
                {
                    data[i] = 0;
                }
            }
        }
    }

    public void Multiply( VectorArray vector,  VectorArray result)
    {

        for (int row = 0; row < num_rows_; row++)
        {
            result.data[row] = 0; // 초기화

            int index = row * num_cols_;
            double temp;

            for (int col = 0; col < num_cols_; col++, index++) // § 않이
            {
                temp = data[index];
                temp *= vector.data[col];

                result.data[row] += temp;
            }
        }
        // 뉴런의 가중치의 행렬과 입력값의 벡터의 행렬을 내적하여 시그마의 행렬로 정렬하는 함수이다.
        // 축약된 식으로 표현하면 "시그마 = Wx"
    }

    public void MultiplyTransposed( VectorArray vector,  VectorArray result)
    {
        // back propagation 을 할 때에는 가중치(w)의 행렬을 뒤짚은 형태에서 에러가 곱해진 미분 값의 행렬을 곱한다.
        

        for (int col = 0; col < num_cols_; col++)
        {
            result.data[col] = 0;

            for (int row = 0, ix = col; row < num_rows_; row++, ix += num_cols_)
            {
                result.data[col] += data[ix] * vector.data[row];
            }
        }
    }

    public int get1DIndex(int row, int column)
    {
        return (int)(column + row * num_cols_);
    }

    public double getValue(int row, int column)
    {
        return data[get1DIndex(row, column)];
    }

    public void AddValue(int row, int column, double addVal)
    {
        data[get1DIndex(row, column)] += addVal;
    }

}



public class NeuralNetwork {

    public int num_input_; // 입력 레이어 개수
    public int num_output_;    // 출력 레이어 개수
    public int num_all_layers_; // 모든 레이어 개수.

    public double bias_;   // Bias
    public double alpha_;  // 학습수치

    public VectorArray[] layer_neuron_act_;
    public VectorArray[] layer_neuron_grad_; // Back-prop할때 사용할 변화값
    public Matrix[] weights_;

    public VectorArray num_layer_acts_;

    public NeuralNetwork() { }
    public NeuralNetwork(int numInput, int numOutput, int numHiddenLayers) {
        Initialize(numInput, numOutput, numHiddenLayers);
    }

    public void Initialize(int numInput, int numOutput, int numHiddenLayers)
    {
        num_layer_acts_ = new VectorArray();    //  할당

        num_layer_acts_.Initialize(numHiddenLayers + 2); // input + output + hidden

        num_layer_acts_.data[0] = numInput + 1; // 0번째 레이어는 input +1은 바이어스 때문
        for (int i = 1; i < numHiddenLayers + 1; i++)
        {
            num_layer_acts_.data[i] = numInput + 1; // default data
        }

        num_layer_acts_.data[numHiddenLayers + 1] = numOutput + 1; // 마지막 레이어는 출력 레이어이다.

        Initialize( num_layer_acts_, numHiddenLayers);
    }

    public void Initialize( VectorArray numLayerActs, int numHiddenLayers)
    {
        num_input_ = (int)(numLayerActs.data[0]) - 1; // input의 개수, -1은 바이어스 때문
        num_output_ = (int)(num_layer_acts_.data[numHiddenLayers + 1]) - 1; // output의 개수 -1은 바이어스 때문
        num_all_layers_ = numHiddenLayers + 2; // 히든 + 인풋 + 아웃풋 = 모든 레이어 개수

        bias_ = 1;
        alpha_ = 0.15;  // default

        layer_neuron_act_ = new VectorArray[num_all_layers_]; // 모든 레이어 개수에 맞춰 할당.

        for (int i = 0; i < num_all_layers_; i++)   // 내부 Vector도 할당
        {
            layer_neuron_act_[i] = new VectorArray();
        }

        for (int i = 0; i < num_all_layers_; i++)
        {
            layer_neuron_act_[i].Initialize((int)num_layer_acts_.data[i], true);
            layer_neuron_act_[i].data[(int)num_layer_acts_.data[i] - 1] = bias_;    // 벡터의 마지막 값은 바이어스
            
        }


        // gradient 
        layer_neuron_grad_ = new VectorArray[num_all_layers_]; // §

        for (int i = 0; i < num_all_layers_; i++)   // 할당
        {
            layer_neuron_grad_[i] = new VectorArray();
            layer_neuron_grad_[i].Initialize((int)num_layer_acts_.data[i], true);
        }
        

        weights_ = new Matrix[num_all_layers_ - 1];   // -1은 weight는 연결관계이기 때문에 layer보다 1개가 적다.

        for (int i = 0; i < num_all_layers_ - 1; i++)   // 할당
        {
            weights_[i] = new Matrix();
        }

        for (int i = 0; i < weights_.Length; i++)   //
        {
            // 여기 잘 이해가 안되는걸
            // row x column = (dimension of next layer  - 1 for bias) x  (dimension of prev layer - this includes bias)
            weights_[i].Initialize(layer_neuron_act_[i + 1].num_dimension_ - 1, layer_neuron_act_[i].num_dimension_);
            // -1 is for bias. y = W [x b]^T. Don't subtract 1 if you want [y b]^T = W [x b]^T.

           
            System.Random r = new System.Random();
            
            for (int j = 0; j < weights_[i].num_rows_ * weights_[i].num_cols_; j++)
            {
                weights_[i].data[j] = (double)UnityEngine.Random.Range(0.0f, 0.099f); // §
            }
          
        }
        //TODO: Temporary array to store weight matrices from previous step for momentum term.
    }

    public double getSigmoid(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
        // 1 / (1 + log-x) = Sigmoid(x)
        // http://taewan.kim/post/sigmoid_diff/
    }
    public double getSigmoidGradFromY(double y)   // y에다 getSigmoid(x)를 집어넣어서 사용.
    {
    	return (1.0 - y) * y;
    }

    public double getRelu(double x)    // RELU : 0보다 작으면 0, 크면 그 값을 반환
    {
        return 0.0 > x ? 0.0 : x;   // 현재 모델이 음수를 인식하지 않는 이유.
    }

    public double getReluGradFromY(double x) // RELU grad는 x, y가 동일함
    {
        if (x > 0.0) return 1.0;
        else return 0.0;
    }

    public double getLRelu(double x)   // 0보다 작으면 1 / 10 배의 값을 반환
    {
        return x > 0.0 ? x : 0.01 * x;
    }

    public double getLReluGradFromY(double x)
    {
        if (x > 0.0) return 1.0;
        else return 0.01;
    }

    public void ApplySigmoidToVector( VectorArray vector)
    {
        for (int i = 0; i < vector.num_dimension_ - 1; i++) // -1은 바이어스는 활성화 시키지 않는 상수이기 때문
        {
            vector.data[i] = getSigmoid(vector.data[i]);
        }
    }

    public void ApplyReluToVector( VectorArray vector)
    {
        for (int i = 0; i < vector.num_dimension_ - 1; i++) // -1은 바이어스는 활성화 시키지 않는 상수이기 때문
        {
            vector.data[i] = getRelu(vector.data[i]);
        }
    }

    public void ApplyLReluToVector( VectorArray vector)
    {
        for (int i = 0; i < vector.num_dimension_ - 1; i++) // -1은 바이어스는 활성화 시키지 않는 상수이기 때문
        {
            vector.data[i] = getLRelu(vector.data[i]);
        }
    }


    public void PropForward()
    {
        for (int i = 0; i < weights_.Length; i++)
        {

            // layer_neuron_act_ [l + 1], 마지막 구성 요소는 업데이트되지 않아야합니다. bias임.
            weights_[i].Multiply( layer_neuron_act_[i],  layer_neuron_act_[i + 1]);

            //UnityEngine.Debug.Log(layer_neuron_act_[i].data[] + "  " + layer_neuron_act_[i + 1]);
            // 활성화
            ApplyReluToVector( layer_neuron_act_[i + 1]);
        }
    }


    public void PropBackward( VectorArray target)
    {
        int i = layer_neuron_grad_.Length - 1;

        // calculate gradients of output layer
        for (int d = 0; d < layer_neuron_grad_[i].num_dimension_ - 1; d++) // 마지막 하나는 bias
        {
            double output_value = layer_neuron_act_[i].data[d];
            layer_neuron_grad_[i].data[d] = (target.data[d] - output_value) * getReluGradFromY(output_value); 
        }


        // calculate gradients of hidden layers
        for (int j = weights_.Length - 1; j >= 0; j--)
        {
            weights_[j].MultiplyTransposed( layer_neuron_grad_[j + 1],  layer_neuron_grad_[j]);
            
            for (int d = 0; d < layer_neuron_act_[j].num_dimension_ - 1; d++)
            {
                layer_neuron_grad_[j].data[d] = layer_neuron_grad_[j].data[d] * getLReluGradFromY(layer_neuron_act_[j].data[d]);   
            }
        }
        
        for (int j = weights_.Length - 1; j >= 0; j--)
        {
            UpdateWeight( weights_[j],  layer_neuron_grad_[j + 1],  layer_neuron_act_[j]);
        }
    }
    
    public void UpdateWeight( Matrix weight_matrix,  VectorArray next_layer_grad,  VectorArray prev_layer_act)
    {
        for (int row = 0; row < weight_matrix.num_rows_; row++)
        {
            for (int col = 0; col < weight_matrix.num_cols_; col++)
            {
                double delta_w = alpha_ * next_layer_grad.data[row] * prev_layer_act.data[col];

                
                weight_matrix.AddValue(row, col, delta_w); 
                //TODO: update momentum term
            }
        }
    }

    public void setInputVector( VectorArray input)
    {
        if(input.num_dimension_ < num_input_)
        {
           UnityEngine.Debug.Log("input dimension is wrong\n");
        }

        for (int d = 0; d < num_input_; d++)
        {
            layer_neuron_act_[0].data[d] = input.data[d];
        }
    }

    public void CopyOutputVector( VectorArray copy, bool copy_bias = false)
    {
        VectorArray output_layer_act = new VectorArray( layer_neuron_act_[layer_neuron_act_.Length - 1]);

        if (copy_bias)
        {
            copy.Initialize(num_output_ + 1, false);

            for (int d = 0; d < num_output_ + 1; d++)
            {
                copy.data[d] = output_layer_act.data[d];
            }
        }
        else
        {
            copy.Initialize(num_output_, false);

            for (int d = 0; d < num_output_; d++)
            {
                copy.data[d] = output_layer_act.data[d];
            }
        }
    }


    
}

