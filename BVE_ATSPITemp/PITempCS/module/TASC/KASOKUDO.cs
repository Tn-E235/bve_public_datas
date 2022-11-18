using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PITempCS.TASC {
    unsafe
    public class KASOKUDO {
        
        // 記録データ構造体
        public struct KASOKUDO_DATA {
            public float speed;
            public double kasokudo;
            public int time;
        };

        // 記録数
        int mem_num;
        const int DEFAULT_MEM_NUM = 60;
        // 記録データ
        private KASOKUDO_DATA[] kasokudo_data = new KASOKUDO_DATA[DEFAULT_MEM_NUM]; 
        // 記録カウンタ
        private int counter = 0;

        // コンストラクタ
        public KASOKUDO() {
            counter = 0;
            mem_num = DEFAULT_MEM_NUM;
            // 初期化
            for (int i = 0; i < mem_num; ++i) 
                kasokudo_data[i] = new KASOKUDO_DATA();
        }

        public KASOKUDO(int n) {
            counter = 0;
            mem_num = Math.Min(n, DEFAULT_MEM_NUM);
            // 初期化
            for (int i = 0; i < mem_num; ++i)
                kasokudo_data[i] = new KASOKUDO_DATA();
        }

        // 動作部
        public void calc(State* st) {
            kasokudo_data[counter].speed = st->V;
            kasokudo_data[counter].time = st->T;
            kasokudo_data[counter].kasokudo = calcKasokudo();
            ++counter;
            counter %= mem_num;
        }

        // 加速度を取得する
        public double getKasokudo() {
            double kasokudo = 0.0;
            for (int i = 0; i < mem_num; ++i) {
                kasokudo += kasokudo_data[i].kasokudo;
            }
            return kasokudo / mem_num;
        }

        /* ----------------------------------------------------------------- */
        // 加速度を計算する
        private double calcKasokudo() {
            int idx = counter == 0 ? mem_num - 1 : counter - 1;
            float s = kasokudo_data[counter].speed - kasokudo_data[idx].speed;
            double t = kasokudo_data[counter].time - kasokudo_data[idx].time;
            return (s / (t / 1000));
        }
    }
}
