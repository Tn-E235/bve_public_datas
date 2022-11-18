using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PITempCS.TASC {
    unsafe
    public class TASC {

        private KASOKUDO k = new KASOKUDO();    // 加速度
        private double beacon_location = 0;     // TASC地上子位置(Z)
        private double stop = 0.0;              // 停止位置(Z)
        private int brakeNotch = 0;             // 出力ブレーキノッチ
        private int counter = 0;                // ノッチ操作
        private State state;                    // 車両情報
        private Boolean enable = false;         // TASC動作状態

        private const double MARGEN = 0.35;     // 停止位置誤差[m]

        public TASC() {
            brakeNotch = 0;
            enable = false;
            counter = 0;
        }

        public void calc(State *st) {
            state = *st;
            k.calc(st);

            if (!enable) return;

            const int up = 60;
            if (counter % up == 0){
                double kasokudo = k.getKasokudo();
                double zankyori = stop - state.Z;
                double oku = getPatternBxN(zankyori + (zankyori * (MARGEN / 2.0)), kasokudo);
                double mae = getPatternBxN(zankyori - (zankyori * (MARGEN / 2.0)), kasokudo);
                if (oku < state.V) {
                    if(brakeNotch < 8)++brakeNotch;
                } else if (mae > state.V) {
                    if (brakeNotch > 0) --brakeNotch;
                }
            }

            ++counter;
            counter = counter % up;

        }

        // 設定ブレーキノッチを取得
        public int getBrakeNotch() {
            return enable ? brakeNotch : 0;
        }

        // 停車位置を設定
        public void setStopPosition(int s) {
            stop = state.Z + s;
            beacon_location = state.Z;
        }
        
        // 残り距離を取得
        public double getNokoriKyori() {
            return enable ? stop - state.Z : 0;
        }

        // TASC動作状態切り替え
        public void setEnable(Boolean b) {
            enable = b;
        }

        // TASCの動作状態を取得
        public Boolean isEnable() {
            return enable;
        }

        // 加速度を取得
        public double getKasokudo() {
            return k.getKasokudo();
        }

        // パターン速度を取得
        public double getPattern(double zankyori, double kasokudo) {
            return enable ? getPatternBxN(zankyori, kasokudo) : 0.0;
        }

        // 推定停車位置を取得
        public double getSuiteiTeisyaKyori() {
            return 0.0;
        }

        /* ------------------------------------------------------------------*/
        // 減速度に対するパターンを生成
        private double getPatternBxN(double zankyori, double kasokudo) {
            double margin = 0;
            double pattern =
                Math.Sqrt(7.2 * (kasokudo * -1) * (zankyori - margin));
            return pattern;
        }
        // ブレーキパターンに達しているか
        private Boolean isBrakePattern(double zankyori, double kasokudo, float speed) {
            return (getPatternBxN(zankyori, kasokudo) > speed) ? false : true;
        }

        // ブレーキパターンに接近しているか
        private Boolean isPatternAP(double zankyori, double kasokudo, float speed) {
            double postion = speed / 3600.0 * 2.5 * 1000.0;
            return (getPatternBxN(zankyori - postion, kasokudo) >= speed) ? false : true;
        }

        // パターンから残り距離を計算
        private double getPatterinKyori(double pattern, double kasokudo) {
            return Math.Pow(pattern, 2) / (7.2 * kasokudo);
        }

    }
}
